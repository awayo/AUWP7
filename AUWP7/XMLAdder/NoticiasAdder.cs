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
using System.Windows.Media.Imaging;
using Microsoft.Xna.Framework.Media;
using System.IO.IsolatedStorage;
using System.Windows.Resources;
using System.IO;
using System.Windows.Threading;
using AUWP7.Model;
using System.Globalization;

namespace AUWP7.XMLAdder
{
    public class NoticiasAdder : XMLAdder
    {

        public NoticiasAdder(): base()
        {

        }

        public override void processXML(string xmlData)
        {
            try
            {
                xml = XElement.Parse(xmlData);
                System.Collections.IEnumerable list = xml.Descendants("Noticia");
                System.Collections.IEnumerator enume = list.GetEnumerator();
                var uri1 = "";
                while (enume.MoveNext() && this.Collection.Count < collectionSize)
                {
                    Capitulo c = new Capitulo();
                    String uri = "";
                    XElement a = (XElement)enume.Current;
                    try
                    {
                    c.Nombre = a.Element("Titulo").Value;
                    if (a.Element("Serie") != null)
                    {
                        c.Serie = Int32.Parse(a.Element("Serie").Value);
                        c.TextoNoti = a.Element("Texto").Value;
                        c.NotiAutor = a.Element("Autor").Value;

                        string fecha = a.Element("Fecha").Value;


                        try
                        {
                            c.ReleaseDate = DateTime.ParseExact(fecha, "dd.mm.yy", CultureInfo.InvariantCulture);
                        }
                        catch (FormatException fe)
                        {
                            c.ReleaseDate = DateTime.Now;
                        }

                        if (a.Element("Imagen") != null)
                        {

                            uri1 = "/imgData/" + Hash.Hash.GetHash(a.Element("Imagen").Value + ".thumb.jpg", Hash.Hash.HashType.SHA1);
                            foreach (XElement b in a.Elements("Imagen"))
                            {

                                uri = b.Value + ".thumb.jpg";
                                using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
                                {
                                    String file = Hash.Hash.GetHash(uri, Hash.Hash.HashType.SHA1);
                                    c.Imagenes.Add("/imgData/" + file);
                                    if (!myIsolatedStorage.FileExists("/imgData/" + file))
                                    {
                                        WebClient wc = new WebClient();
                                        wc.OpenReadCompleted += new OpenReadCompletedEventHandler(wc_OpenReadCompleted);
                                        wc.OpenReadAsync(new Uri(uri), file);
                                    }
                                    file = "/imgData/" + file;
                                    uri = file;
                                }
                            }
                        }
                    }
                    }
                    catch (NullReferenceException e)
                    {
                        System.Diagnostics.Debug.WriteLine(e.Message);
                    }

                    c.Id = Int32.Parse(a.Element("Id").Value);

                    if (c.Serie != 0)
                    {
                        this.Collection.Add(new ItemViewModel()
                        {
                            Titulo = a.Element("Titulo").Value,
                            Subtitulo = a.Element("Fecha").Value + " " + a.Element("Autor").Value,
                            Id = c.Id,
                            Url = uri1

                        });
                    }
                    AU.Instance.addCapitulo(c);
                }
                AU.Instance.CapisLoaded = true;
                toGUI();
                //LoadMore = true;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
        }

        public override void au_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                xmlData = e.Result;
                this.processXML(e.Result);
                if (!isXMLLoaded)
                {
                    save(xmlData);
                }
                isXMLLoaded = true;
            }
        }

    }
}
