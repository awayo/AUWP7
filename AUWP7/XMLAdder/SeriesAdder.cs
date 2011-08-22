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
using AUWP7.Model;
using System.Collections.Generic;

namespace AUWP7.XMLAdder
{
    public class SeriesAdder: XMLAdder
    {
        public SeriesAdder(): base()
        {

        }

        public override void processXML(String xmlData)
        {
            try
            {
                xml = XElement.Parse(xmlData);
                System.Collections.IEnumerable list = xml.Descendants("Serie");
                System.Collections.IEnumerator enume = list.GetEnumerator();
                while (enume.MoveNext() && this.Collection.Count < collectionSize)
                {
                    Serie s = new Serie();


                    String uri = "";
                    XElement a = (XElement)enume.Current;

                    s.Id = Int32.Parse(a.Element("Id").Value);
                    s.Nombre = a.Element("Nombre").Value;
                    s.Generos = a.Element("Genero").Value.Split(',');
                    s.Estudios = a.Element("Estudio").Value.Split(',');
                    s.Sinopsis = a.Element("Sinopsis").Value;

                    if (a.Element("Precuela").Value != null)
                    {
                        s.Precuela = Int32.Parse(a.Element("Precuela").Value);
                        if (s.Precuela != 0)
                        {
                            AU.Instance.Precuelas.Add(s.Precuela, s);
                        }
                    }

                    if (a.Element("Secuela").Value != null)
                    {
                        s.Secuela = Int32.Parse(a.Element("Secuela").Value);
                    }

                    s.Formato = " "+a.Element("Contenedor").Value + "\t" + a.Element("CodecVideo").Value + "(" + a.Element("Resolucion").Value + ")\r\n \t\t" + a.Element("CodecAudio").Value;


                    if (a.Element("Imagen") != null)
                    {
                        uri = a.Element("Imagen").Value;
                        using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
                        {
                            String file = Hash.Hash.GetHash(uri, Hash.Hash.HashType.SHA1);

                            if (!myIsolatedStorage.FileExists("/imgData/" + file))
                            {
                                WebClient wc = new WebClient();
                                wc.OpenReadCompleted += new OpenReadCompletedEventHandler(wc_OpenReadCompleted);
                                wc.OpenReadAsync(new Uri(uri), file);
                            }
                            file = "/imgData/" + file;
                            uri = file;
                            s.Imagen = uri;
                        }
                    }

                    
                    this.Collection.Add(new ItemViewModel()
                    {
                        Id = s.Id,
                        Titulo = s.Nombre,
                        Subtitulo = a.Element("Estudio").Value + " - " + a.Element("Genero").Value,
                        Url = uri
                    });

                    AU.Instance.addSerie(s);
                }
                AU.Instance.SeriesLoaded = true;
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
                processXML(e.Result);
                if (!isXMLLoaded)
                {
                    save(xmlData);
                }
                isXMLLoaded = true;
            }
        }

    }
}
