using System;
using System.Collections.Generic;

namespace AUWP7.Model
{
    public class Serie
    {
        private int precuela;

        public int Precuela
        {
            get { return precuela; }
            set { precuela = value; }
        }

        private int secuela;

        public int Secuela
        {
            get { return secuela; }
            set { secuela = value; }
        }

        private String imagen;

        public String Imagen
        {
            get { return imagen; }
            set { imagen = value; }
        }

        private String sinopsis;

        public String Sinopsis
        {
            get { return sinopsis; }
            set { sinopsis = value; }
        }

        private int _id;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private String nombre;

        public String Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }
        private String[] estudios;

        public String[] Estudios
        {
            get { return estudios; }
            set { estudios = value; }
        }
        private String[] generos;

        public String[] Generos
        {
            get { return generos; }
            set { generos = value; }
        }
        private int capitulosTotales;

        public int CapitulosTotales
        {
            get { return capitulosTotales; }
            set { capitulosTotales = value; }
        }
        private int capitulosHechos;

        public int CapitulosHechos
        {
            get { return capitulosHechos; }
            set { capitulosHechos = value; }
        }

        private String formato;

        public String Formato
        {
            get { return formato; }
            set { formato = value; }
        }

        private Dictionary<String, Capitulo> capitulos;

        public Serie()
        {
            capitulos = new Dictionary<string, Capitulo>();
        }

    }
}
