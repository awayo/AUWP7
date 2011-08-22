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
using System.Xml.Linq;
using System.IO.IsolatedStorage;
using System.IO;

namespace AUWP7.XMLAdder
{
    public class SeriesAdder: XMLAdder
    {
        public SeriesAdder(): base()
        {

        }

        public override void au_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                xml = XElement.Parse(e.Result);
                System.Collections.IEnumerable list = xml.Descendants("Serie");
                System.Collections.IEnumerator enume = list.GetEnumerator();
                while (enume.MoveNext() && this.Collection.Count < collectionSize)
                {
                    String uri = "";
                    XElement a = (XElement)enume.Current;
                    if (a.Element("Imagen") != null)
                    {
                        uri = a.Element("Imagen").Value;
                        using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
                        {
                            String file = uri.Replace("http://www.aunder.org/au/Imagenes/", "");
                            file = HttpUtility.UrlDecode(file);
                            file = file.Replace(" ", "_");

                            if (!myIsolatedStorage.FileExists("/imgData/"+file))
                            {
                                WebClient wc = new WebClient();
                                wc.OpenReadCompleted += new OpenReadCompletedEventHandler(wc_OpenReadCompleted);
                                wc.OpenReadAsync(new Uri(uri), file);
                            }
                            else
                            {
                                    file = "/imgData/" + file;
                                    uri = file;
                            }
                        }
                    }
                    this.Collection.Add(new ItemViewModel()
                    {
                        Titulo = a.Element("Nombre").Value,
                        Subtitulo = a.Element("Estudio").Value + " - " + a.Element("Genero").Value,
                        Url = filterName(uri.Replace("http://www.aunder.org/au/Imagenes/", "").Replace(" ", "_"))
                    });
                }

                if (!isXMLLoaded)
                {
                    save();
                }
                isXMLLoaded = true;

            }
        }

    }
}
