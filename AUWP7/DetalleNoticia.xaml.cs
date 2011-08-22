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

namespace AUWP7
{
    public partial class DetalleNoticia : PhoneApplicationPage
    {
        public DetalleNoticia()
        {
            InitializeComponent();
        }

        private int serieId = 0;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ApplicationBar.Opacity = 0.5;
            if (this.NavigationContext.QueryString.ContainsKey("nid"))
            {
                var nid = Int32.Parse(this.NavigationContext.QueryString["nid"]);
                Capitulo c;

                if (AU.Instance.Capitulos.TryGetValue(nid, out c))
                {
                    PageTitle.Text = c.Nombre;
                    sinopsis.Text = c.TextoNoti;
                    textAutor.Text = c.NotiAutor;
                    textFecha.Text = c.ReleaseDate.ToString();
                    var i = 0;

                    serieId = c.Serie;
                    foreach (String file in c.Imagenes)
                    {
                        using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
                        {

                            if (myIsolatedStorage.FileExists(file))
                            {
                                using (var imageStream = myIsolatedStorage.OpenFile(file, FileMode.Open, FileAccess.Read))
                                {
                                    WriteableBitmap temp = new WriteableBitmap(0, 0);
                                    try
                                    {
                                        Image image1 = new Image();
                                        WriteableBitmap aux = new WriteableBitmap(0, 0);
                                        aux.SetSource(imageStream);
                                        image1.Source = (ImageSource)aux;
                                        image1.HorizontalAlignment = HorizontalAlignment.Left;
                                        image1.VerticalAlignment = VerticalAlignment.Top;
                                        image1.Stretch = Stretch.Uniform;
                                        ColumnDefinition colDef3 = new ColumnDefinition();
                                        imagenes.ColumnDefinitions.Add(colDef3);
                                        Grid.SetColumn(image1, i);
                                        i++;
                                        imagenes.Children.Add(image1);


                                    }
                                    catch (ArgumentException ae)
                                    {

                                    }

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

        private void ApplicationBarIconButton_Click(object sender, System.EventArgs e)
        {
            Uri nUri = new Uri(string.Format("/DetalleSerie.xaml?id={0}", serieId), UriKind.Relative);
            ((App)Application.Current).RootFrame.Navigate(nUri);
        }
    }
}