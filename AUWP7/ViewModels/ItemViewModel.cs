using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.IO.IsolatedStorage;
using System.Windows.Media.Imaging;
using System.IO;

namespace AUWP7
{
    public class ItemViewModel : INotifyPropertyChanged
    {

        private int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        private string _Titulo;
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
        public string Titulo
        {
            get
            {
                return _Titulo;
            }
            set
            {
                if (value != _Titulo)
                {
                    _Titulo = value;
                    NotifyPropertyChanged("Titulo");
                }
            }
        }

        private string _Subtitulo;
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
        public string Subtitulo
        {
            get
            {
                return _Subtitulo;
            }
            set
            {
                if (value != _Subtitulo)
                {
                    _Subtitulo = value;
                    NotifyPropertyChanged("Subtitulo");
                }
            }
        }

        private String _url;

        public String Url
        {
            get { return _url; }
            set { _url = value; }
        }

        private ImageSource _Imagen;
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
        public ImageSource Imagen
        {
            get
            {
                using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (!myIsolatedStorage.FileExists(_url))
                    {
                        Uri uri = new Uri("/icons/image.loading.png", UriKind.Relative);
                        BitmapImage w = new BitmapImage(uri);
                        return w;
                    }
                    else
                    {
                        using (var imageStream = myIsolatedStorage.OpenFile(
                            _url, FileMode.Open, FileAccess.Read))
                        {
                            WriteableBitmap temp = new WriteableBitmap(0, 0);
                            try
                            {
                                
                                temp.SetSource(imageStream);
                                
                            }
                            catch (ArgumentException ae)
                            {
                                System.Diagnostics.Debug.WriteLine(ae.Message);
                            }

                            return temp;
                        }
                    }
                }
            }
            set
            {
                if (value != _Imagen)
                {
                    _Imagen = value;
                    NotifyPropertyChanged("Imagen");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}