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
using Delay;
using System.Windows.Data;
using System.Globalization;
using System.ComponentModel;

namespace AUWP7
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);

            SupportedOrientations = SupportedPageOrientation.Portrait;
        }

        // Load data for the ViewModel Items
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            
            if (!App.ViewModel.IsDataLoaded)
            {
                progressBar1.Visibility = Visibility.Visible;
                textCargando.Visibility = Visibility.Visible;
                App.ViewModel.LoadData();
            }

            App.ViewModel.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(ViewModel_PropertyChanged);

        }

        private void series_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ItemViewModel s = (ItemViewModel)((ListBox)sender).SelectedItem;
            Int32 Id = s.Id;
            Uri nUri = new Uri(string.Format("/DetalleSerie.xaml?id={0}", Id), UriKind.Relative);
            ((App)Application.Current).RootFrame.Navigate(nUri);
        }

        private void novedades_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ItemViewModel s = (ItemViewModel)((ListBox)sender).SelectedItem;
            Int32 Id = s.Id;
            Uri nUri = new Uri(string.Format("/DetalleNoticia.xaml?nid={0}", Id), UriKind.Relative);
            ((App)Application.Current).RootFrame.Navigate(nUri);
        }

        private void entes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ItemViewModel s = (ItemViewModel)((ListBox)sender).SelectedItem;
            Int32 Id = s.Id;
            Uri nUri = new Uri(string.Format("/DetalleEnte.xaml?eid={0}", Id), UriKind.Relative);
            ((App)Application.Current).RootFrame.Navigate(nUri);
        }

        void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            progressBar1.Visibility = Visibility.Collapsed;
            textCargando.Visibility = Visibility.Collapsed;
            
        }

        private void PhoneApplicationPage_GotFocus(object sender, RoutedEventArgs e)
        {

            //webBrowser1.Navigate(new Uri("http://foro.aunder.org"));
            //webBrowser1.InvokeScript("eval", "history.go()");
        }
    }
}