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
    public class AU
    {
        private Dictionary<int,Ente> entes;

        public Dictionary<int, Ente> Entes
        {
            get { return entes; }
        }

        private Dictionary<int, Serie> precuelas;

        public Dictionary<int, Serie> Precuelas
        {
            get { return precuelas; }
            set { precuelas = value; }
        }

        private Dictionary<int,Serie> series;

        public Dictionary<int,Serie> Series
        {
            get { return series; }
        }

        private Dictionary<int, Capitulo> capitulos;

        public Dictionary<int, Capitulo> Capitulos
        {
            get { return capitulos; }
            set { capitulos = value; }
        }

        public bool CapisLoaded = false;
        public bool SeriesLoaded = false;
        public bool EntesLoaded = false;

        private static AU instance;

        private AU()
        {
            entes = new Dictionary<int, Ente>();
            series = new Dictionary<int,Serie>();
            precuelas = new Dictionary<int, Serie>();
            capitulos = new Dictionary<int, Capitulo>();
        }

        public static AU Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AU();
                }
                return instance;
            }
        }
        public void addSerie(Serie s)
        {
            series.Add(s.Id, s);
                
        }

        public void addEnte(Ente e)
        {
            entes.Add(e.Id,e);
        }

        public void addCapitulo(Capitulo c)
        {
            capitulos.Add(c.Id,c);
        }
    }
}
