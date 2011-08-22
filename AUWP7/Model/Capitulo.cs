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
using System.Collections.Generic;

namespace AUWP7.Model
{
    public class Capitulo
    {
        //Nombre de la serie
        public int Id;
        private String nombre;

        private String textoNoti;

        private String notiAutor;

        private List<String> imagenes;

        public List<String> Imagenes
        {
            get { return imagenes; }
            set { imagenes = value; }
        }

        private DateTime releaseDate;

        public DateTime ReleaseDate
        {
            get { return releaseDate; }
            set { releaseDate = value; }
        }

        public String NotiAutor
        {
            get { return notiAutor; }
            set { notiAutor = value; }
        }

        private int serie;

        public int Serie
        {
            get { return serie; }
            set { serie = value; }
        }

        public String TextoNoti
        {
            get { return textoNoti; }
            set { textoNoti = value; }
        }

        private int capitulo;
        private string version;
        private Dictionary<Ocupacion, Ente> trabajadores;

        public string Version
        {
            get { return version; }
            set { version = value; }
        }

        public String Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }

        public String NombreLargo
        {
            get { return nombre+" "+capitulo+" v"+version; }
        }

        public void addEnte(Ocupacion o, Ente e)
        {
            trabajadores.Add(o, e);
        }

        public List<Ente> getEntes() {
            List<Ente> ret = new List<Ente>();

            Dictionary<Ocupacion, Ente>.Enumerator e = trabajadores.GetEnumerator();
            ret.Add(e.Current.Value);

            while (e.MoveNext())
            {
                ret.Add(e.Current.Value);
            }

            return ret;
        }

        public Capitulo()
        {
            trabajadores = new Dictionary<Ocupacion,Ente>();
            imagenes = new List<string>();
        }

    }
}
