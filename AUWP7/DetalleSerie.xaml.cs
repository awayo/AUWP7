using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.Windows.Navigation;
using AUWP7.Model;
using System.IO.IsolatedStorage;
using System.IO;
using System.Windows.Media.Imaging;
using Microsoft.Phone.Shell;

namespace AUWP7
{
    public partial class DetalleSerie : PhoneApplicationPage
    {
        public DetalleSerie()
        {
            InitializeComponent();
        }

        private String une(String[] datos)
        {
            String ret = "";

            int length = datos.Length;

            for (int i = 0; i < length; i++ )
            {
                String dato = datos[i];

                ret = ret + dato;

                if (i > 0 && i < length - 1)
                {
                    ret = ret + ", ";
                }
            }

            return ret;
        }

        private Serie s;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (this.NavigationContext.QueryString.ContainsKey("id"))
            {
                var id = Int32.Parse(this.NavigationContext.QueryString["id"]);
                  if (AU.Instance.Series.TryGetValue(id, out s))
                {
                    PageTitle.Text = s.Nombre;
                    sinopsis.Text = s.Sinopsis;
                    formato.Text = s.Formato;
                    generos.Text = une(s.Generos);
                    estudio.Text = une(s.Estudios);


                    ApplicationBar.Buttons.Clear();
                    if (s.Precuela != 0)
                    {
                        ApplicationBarIconButton a = new ApplicationBarIconButton(new Uri("/icons/appbar.back.rest.png", UriKind.Relative)) { Text = "Precuela" };
                        a.Click += new EventHandler(ApplicationBarIconButton_Click_Precuela);
                        ApplicationBar.Buttons.Add(a);
                    }

                    if (s.Secuela != 0)
                    {
                        ApplicationBarIconButton a = new ApplicationBarIconButton(new Uri("/icons/appbar.next.rest.png", UriKind.Relative)) { Text = "Secuela" };
                        a.Click += new EventHandler(ApplicationBarIconButton_Click_Secuela);
                        ApplicationBar.Buttons.Add(a);
                    }
                    else
                    {
                        Serie sAux;
                        if (AU.Instance.Precuelas.TryGetValue(s.Id, out sAux))
                        {
                            s.Secuela = sAux.Id;
                            ApplicationBarIconButton a = new ApplicationBarIconButton(new Uri("/icons/appbar.next.rest.png", UriKind.Relative)) { Text = "Secuela" };
                            a.Click += new EventHandler(ApplicationBarIconButton_Click_Secuela);
                            ApplicationBar.Buttons.Add(a);
                        }
                    }

                    if (ApplicationBar.Buttons.Count == 0)
                    {
                        ApplicationBar.IsVisible = false;
                    }

                    var file = s.Imagen;
                    using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
                    {

                        if (myIsolatedStorage.FileExists(file))
                        {
                            using (var imageStream = myIsolatedStorage.OpenFile(file, FileMode.Open, FileAccess.Read))
                            {
                                WriteableBitmap temp = new WriteableBitmap(0, 0);
                                try
                                {
                                    WriteableBitmap aux = new WriteableBitmap(0, 0);
                                    aux.SetSource(imageStream);
                                    image1.Source = (ImageSource)aux;
                                }
                                catch (ArgumentException ae)
                                {

                                }

                            }
                        }
                    }
                }
            }
            else
            {
                // I use this condition to handle creating new items.
            }
        }

        private void PageTitle_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
        	this.Storyboard1.Begin();
        }

        private void precuela_Click(object sender, RoutedEventArgs e)
        {
            Int32 Id = (Int32)((Button) sender).Tag;
            Uri nUri = new Uri(string.Format("/DetalleSerie.xaml?id={0}", Id), UriKind.Relative);
            ((App)Application.Current).RootFrame.Navigate(nUri);
        }

        private void ApplicationBarIconButton_Click_Secuela(object sender, EventArgs e)
        {
            if (s.Secuela > 0)
            {
                Uri nUri = new Uri(string.Format("/DetalleSerie.xaml?id={0}", s.Secuela), UriKind.Relative);
                ((App)Application.Current).RootFrame.Navigate(nUri);
            }
        }

        private void ApplicationBarIconButton_Click_Precuela(object sender, EventArgs e)
        {
            if (s.Precuela > 0)
            {
                Uri nUri = new Uri(string.Format("/DetalleSerie.xaml?id={0}", s.Precuela), UriKind.Relative);
                ((App)Application.Current).RootFrame.Navigate(nUri);
            }
        }
    }
}