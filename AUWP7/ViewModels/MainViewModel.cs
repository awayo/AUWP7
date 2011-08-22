using System;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Collections;
using System.Net;
using System.Xml.Linq;
using System.Linq;
using AUWP7.XMLAdder;
using System.Threading;


namespace AUWP7
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            this.Series = new ObservableCollection<ItemViewModel>();
            this.Entes = new ObservableCollection<ItemViewModel>();
            this.Noticias = new ObservableCollection<ItemViewModel>();
            this.NotiA = new NoticiasAdder();
            this.SeriA = new SeriesAdder();
            this.EnteA = new EntesAdder();
        }

        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public ObservableCollection<ItemViewModel> Series { get; private set; }
        public ObservableCollection<ItemViewModel> Entes { get; private set; }
        public ObservableCollection<ItemViewModel> Noticias { get; private set; }
        public NoticiasAdder NotiA;
        public SeriesAdder SeriA;
        public EntesAdder EnteA;

        private string _sampleProperty = "Series / Entes";
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding
        /// </summary>
        /// <returns></returns>
        public string SampleProperty
        {
            get
            {
                return _sampleProperty;
            }
            set
            {
                if (value != _sampleProperty)
                {
                    _sampleProperty = value;
                    NotifyPropertyChanged("SampleProperty");
                }
            }
        }

        public bool IsDataLoaded
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates and adds a few ItemViewModel objects into the Items collection.
        /// </summary>
        public void LoadData()
        {
            NotiA.nombre = "Noticias";
            NotiA.url = "http://www.aunder.org/xml/newsxml.php";
            NotiA.PublicCollection = Noticias;

            //SeriA.start();
            BackgroundWorker b = new BackgroundWorker();

            b.DoWork += new DoWorkEventHandler(NotiA.start);

            EnteA.nombre = "Entes";
            EnteA.url = "http://www.aunder.org/xml/entesxml.php";
            EnteA.PublicCollection = Entes;
            EnteA.collectionSize = 130;
            //EnteA.start();
            b.DoWork += new DoWorkEventHandler(EnteA.start);

            SeriA.nombre = "Series";
            SeriA.url = "http://www.aunder.org/xml/seriesxml.php";
            SeriA.PublicCollection = Series;
            SeriA.collectionSize = 230;
            //NotiA.start();
            b.DoWork += new DoWorkEventHandler(SeriA.start);
            b.RunWorkerCompleted += new RunWorkerCompletedEventHandler(is_loaded);
            b.RunWorkerAsync();


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

        void is_loaded(object sender, RunWorkerCompletedEventArgs e)
        {
            this.IsDataLoaded = true;
            NotifyPropertyChanged("IsdataLoaded");
        }

    }
}