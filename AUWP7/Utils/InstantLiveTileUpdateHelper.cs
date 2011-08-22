using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.IO;
using System.Xml;
using System.Text;
using System.Diagnostics;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Notification;
using System.Collections.ObjectModel;
using System.Threading;

namespace AUWP7.Utils
{
    public class InstantLiveTileUpdateHelper
    {
        public InstantLiveTileUpdateHelper(ShellTileSchedule schedule)
        {
            tileSchedule = schedule;
        }

        public InstantLiveTileUpdateHelper()
        {
        }

        private ShellTileSchedule tileSchedule;

        public void UpdateLiveTileAndStartSchedule(string liveTileTitle, int? liveTileCount)
        {
            if (tileSchedule == null)
                throw new ArgumentException("You cannot call this method unless you have initialised", "tileSchedule");

            UpdateLiveTile(tileSchedule.RemoteImageUri, liveTileTitle, liveTileCount, tileSchedule.Start);
        }

        public void UpdateLiveTile(Uri liveTileUri, string liveTileTitle, int? liveTileCount, Action onComplete)
        {
            HttpNotificationChannel toastChannel = HttpNotificationChannel.Find("liveTileChannel");
            if (toastChannel != null)
            {
                toastChannel.Close();
            }

            toastChannel = new HttpNotificationChannel("liveTileChannel");


            toastChannel.ChannelUriUpdated +=
                (s, e) =>
                {
                    Debug.WriteLine(String.Format("Is image an absolute Uri: {0}", tileSchedule.RemoteImageUri.IsAbsoluteUri));
                    if (liveTileUri.IsAbsoluteUri)
                    {
                        toastChannel.BindToShellTile(new Collection<Uri> { liveTileUri });
                    }
                    else
                    {
                        toastChannel.BindToShellTile();
                    }

                    SendTileToPhone(e.ChannelUri, liveTileUri.ToString(), liveTileCount, liveTileTitle,
                                () =>
                                {
                                    //Give it some time to let the update propagate
                                    Thread.Sleep(TimeSpan.FromSeconds(10));

                                    toastChannel.UnbindToShellTile();
                                    toastChannel.Close();

                                    //Call the "complete" delegate
                                    if (onComplete != null)
                                        onComplete();
                                }
                        );
                };
            toastChannel.Open();
        }

        private void SendTileToPhone(Uri notificationUrl, string liveTileUri, int? count, string liveTileTitle, Action onComplete)
        {
            var stream = new MemoryStream();
            var settings = new XmlWriterSettings
            {
                Indent = true,
                Encoding = Encoding.UTF8
            };

            XmlWriter w = XmlWriter.Create(stream, settings);
            w.WriteStartDocument();
            w.WriteStartElement("wp", "Notification", "WPNotification");
            w.WriteStartElement("wp", "Tile", "WPNotification");
            if (!string.IsNullOrEmpty(liveTileUri))
            {
                Debug.WriteLine(String.Format("Tile Uri in xml: {0}", liveTileUri));
                w.WriteStartElement("wp", "BackgroundImage", "WPNotification");
                w.WriteValue(liveTileUri);
                w.WriteEndElement();
            }
            if (count.HasValue)
            {
                w.WriteStartElement("wp", "Count", "WPNotification");
                w.WriteValue(count.ToString());
                w.WriteEndElement();
            }
            w.WriteStartElement("wp", "Title", "WPNotification");
            w.WriteString(liveTileTitle ?? String.Empty);
            w.WriteEndElement();

            w.WriteEndElement();
            w.Close();

            byte[] payload = stream.ToArray();

            Debug.WriteLine("Sending tile request update payload");
            //Check the length of the payload and reject it if too long));
            if (payload.Length > 1024)
            {
                throw new ArgumentOutOfRangeException(
                    string.Format("Payload is too long. Maximum payload size shouldn't exceed {0} bytes",
                                    1024));
            }

            var httpRequest = (HttpWebRequest)WebRequest.Create(notificationUrl);
            httpRequest.Method = "POST";
            httpRequest.ContentType = "text/xml;";
            httpRequest.Headers["X-MessageID"] = Guid.NewGuid().ToString();
            httpRequest.Headers["X-NotificationClass"] = 1.ToString();
            httpRequest.Headers["X-WindowsPhone-Target"] = "token";

            httpRequest.BeginGetRequestStream(
                ar =>
                {
                    Stream requestStream = httpRequest.EndGetRequestStream(ar);

                    Debug.WriteLine("Sending payload in uploaded request stream...");
                    requestStream.BeginWrite(
                        payload, 0, payload.Length,
                        iar =>
                        {
                            requestStream.EndWrite(iar);
                            requestStream.Close();

                            httpRequest.BeginGetResponse(
                                iarr =>
                                {
                                    if (onComplete != null)
                                        onComplete();
                                },
                                null);
                        },
                        null);
                },
                null);
        }
    }
}
