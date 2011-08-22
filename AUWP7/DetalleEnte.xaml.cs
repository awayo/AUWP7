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
using System.Windows.Media.Imaging;
using System.IO;

namespace AUWP7
{
    public partial class DetalleEnte : PhoneApplicationPage
    {
        public DetalleEnte()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (this.NavigationContext.QueryString.ContainsKey("eid"))
            {
                var nid = Int32.Parse(this.NavigationContext.QueryString["eid"]);
                Ente c;

                if (AU.Instance.Entes.TryGetValue(nid, out c))
                {
                    PageTitle.Text = c.Nombre;
                    textSexo.Text = c.Sexo;
                    if (c.Sexo == null || c.Sexo=="")
                    {
                        textSexo.Text = "Sin definir";
                    }
                    textBio.Text = c.Bio;
                    if (c.Bio == null || c.Bio == "")
                    {
                        textBio.Text = "Sin definir";
                    }

                    textEdad.Text = c.Edad.ToString();
                    if (textEdad.Text == "0")
                    {
                        textEdad.Text = "Sin definir";
                    }
                    textCiudad.Text = c.Ciudad;
                    if (c.Ciudad == null || c.Ciudad == "")
                    {
                        textCiudad.Text = "Sin definir";
                    }

                    textTitulo.Text = c.Titulo;
                    if (c.Titulo == null || c.Titulo == "")
                    {
                        textTitulo.Visibility = Visibility.Collapsed;
                    }

                    seriescapis.Text = " " + c.SeriesHechas + " series y " + c.CapitulosHechos + " capítulos";
                    var i = 0;
                    var file = c.Avatar;
                    using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
                        {

                            if (myIsolatedStorage.FileExists(file))
                            {
                                using (var imageStream = myIsolatedStorage.OpenFile(file, FileMode.Open, FileAccess.Read))
                                {
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
                    foreach (Ocupacion o in c.Ocupaciones)
                    {
                        Serie s;
                        if (AU.Instance.Series.TryGetValue(o.Serie, out s))
                        {
                            TextBlock t = new TextBlock();
                            t.TextWrapping = TextWrapping.Wrap;
                            t.Width = listBox1.Width-10;
                            t.LineHeight = 50;
                            t.FontSize = 25;
                            t.Text = s.Nombre + " " + o.Capis + " capítulos.";
                            listBox1.Items.Add(t);

                        }
                    }
                }
            }
            else
            {
                // I use this condition to handle creating new items.
            }
        }

    }
}