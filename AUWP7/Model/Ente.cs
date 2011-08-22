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
    public class Ente
    {
        private int _id;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private int _uid;

        public int Uid
        {
            get { return _uid; }
            set { _uid = value; }
        }

        private String _avatar;

        public String Avatar
        {
            get { return _avatar; }
            set { _avatar = value; }
        }

        private String _titulo;

        public String Titulo
        {
            get { return _titulo; }
            set { _titulo = value; }
        }

        private String _ciudad;

        public String Ciudad
        {
            get { return _ciudad; }
            set { _ciudad = value; }
        }

        private String _bio;

        public String Bio
        {
            get { return _bio; }
            set { _bio = value; }
        }

        private String _sexo;

        public String Sexo
        {
            get { return _sexo; }
            set { _sexo = value; }
        }

        private int _edad;

        public int Edad
        {
            get { return _edad; }
            set { _edad = value; }
        }

        public bool Activo;
        private String nombre;

        public String Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }
        private LinkedList<Ocupacion> ocupaciones;

        public LinkedList<Ocupacion> Ocupaciones
        {
            get { return ocupaciones; }
        }
        private int capitulosHechos;

        public int CapitulosHechos
        {
            get { return capitulosHechos; }
            set { capitulosHechos = value; }
        }

        private int seriesHechas;

        public int SeriesHechas
        {
            get { return seriesHechas; }
            set { seriesHechas = value; }
        }

        public Ente()
        {
            ocupaciones = new LinkedList<Ocupacion>();
        }

        public void addOcupacion(Ocupacion o)
        {
            ocupaciones.AddLast(o);
        }

    }
}
