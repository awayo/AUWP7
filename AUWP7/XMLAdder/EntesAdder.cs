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
    public class EntesAdder: XMLAdder
    {
        public EntesAdder(): base()
        {

        }

        public override void processXML(string xmlData)
        {
            try {
            xml = XElement.Parse(xmlData);
            System.Collections.IEnumerable list = xml.Descendants("Ente");
            System.Collections.IEnumerator enume = list.GetEnumerator();
            while (enume.MoveNext() && this.Collection.Count < collectionSize)
            {
                String uri = "";
                XElement a = (XElement)enume.Current;

                Ente ente = new Ente();

                ente.Id = Int32.Parse(a.Element("Id").Value);
                ente.Uid = Int32.Parse(a.Element("Uid").Value);

                if (a.Element("Avatar") != null)
                {
                    uri = a.Element("Avatar").Value;
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
                        ente.Avatar = uri;
                    }
                }
                String valorSub = "";
                bool escriboGuion = false;
                if (a.Element("Ciudad") != null && !a.Element("Ciudad").Value.Equals(""))
                {
                    valorSub = valorSub + a.Element("Ciudad").Value;
                    escriboGuion = true;
                    ente.Ciudad = a.Element("Ciudad").Value;
                }


                if (a.Element("TituloUsuario") != null && !a.Element("TituloUsuario").Value.Equals(""))
                {
                    if (escriboGuion)
                    {
                        valorSub = valorSub + " - ";
                    }
                    valorSub = valorSub + a.Element("TituloUsuario").Value;
                    escriboGuion = true;
                    ente.Titulo = a.Element("TituloUsuario").Value;
                }

                if (a.Element("Bio") != null && !a.Element("Bio").Value.Equals(""))
                {
                    if (escriboGuion)
                    {
                        valorSub = valorSub + " - ";
                    }
                    valorSub = valorSub + a.Element("Bio").Value;
                    ente.Bio = a.Element("Bio").Value;
                }
                if (a.Element("Edad") != null)
                {
                    ente.Edad = Int32.Parse(a.Element("Edad").Value);
                }

                ente.Sexo = a.Element("Sexo").Value;
                ente.Nombre = a.Element("Nombre").Value;
                this.Collection.Add(new ItemViewModel()
                {
                    Titulo = a.Element("Nombre").Value,
                    Subtitulo = valorSub,
                    Url = uri,
                    Id = ente.Id
                });

                List<int> series = new List<int>();
                int capis = 0;

                foreach (XElement e in a.Elements("Serie"))
                {
                    string ocupacion = e.Attribute("rol").Value;
                    int capitulos = Int32.Parse(e.Attribute("capitulos").Value);
                    int serie = Int32.Parse(e.Value);
                    if (!series.Contains(serie))
                    {
                        series.Add(serie);
                        capis += capitulos;

                        //Sacarlo para tener en cuenta los cargos
                        Ocupacion o = new Ocupacion(ocupacion);
                        o.Serie = serie;
                        o.Capis = capitulos;
                        ente.addOcupacion(o);
                    }

                }

                ente.SeriesHechas = series.Count;
                ente.CapitulosHechos = capis;

                AU.Instance.addEnte(ente);
            }
            AU.Instance.EntesLoaded = true;
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
