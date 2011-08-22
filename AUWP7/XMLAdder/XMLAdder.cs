using System;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.IsolatedStorage;
using System.Net;
using System.Text;
using System.Windows.Media.Imaging;
using System.Xml.Linq;
using System.ComponentModel;
using System.Collections.Generic;
using AUWP7.Model;

namespace AUWP7.XMLAdder
{


    public class XMLAdder: INotifyPropertyChanged
    {
        public XElement xml { get; set; }
        public string nombre { get; set; }
        public string url { get; set; }
        public int collectionSize { get; set; }

        private String check;
        private bool _loadMore;
        private bool cont = false;

        public bool LoadMore
        {
            get { return _loadMore; }
            set
            {
                if (_loadMore != value)
                {
                    _loadMore = value;
                    OnPropertyChanged("LoadMore");
                }
            }
        }

        private FileStream _xml;
        protected string xmlData;
        protected bool isXMLLoaded;
        WebClient au;
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<ItemViewModel> PublicCollection { get; set; }

        protected List<ItemViewModel> Collection;

        private bool debugging = false;

        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>


        public XMLAdder()
        {
            au = new WebClient();
            PublicCollection = new ObservableCollection<ItemViewModel>();
            Collection = new List<ItemViewModel>();
            isXMLLoaded = false;
            collectionSize = 30;
            if (debugging) {
                IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication();
                myIsolatedStorage.Remove();
            }

        }

        protected void toGUI()
        {
            ((App)App.Current).RootFrame.Dispatcher.BeginInvoke(new Action(() =>
                 {
                    foreach (ItemViewModel item in Collection)
                    {
                        this.PublicCollection.Add(item);
                    }
                }
            ));
            if (AU.Instance.CapisLoaded && AU.Instance.SeriesLoaded && AU.Instance.EntesLoaded)
            {
                this.OnPropertyChanged("GUI");
            }
        }

        public void start(object sender, DoWorkEventArgs e)
        {
            //
            DateTime Limit = DateTime.UtcNow;
            try
            {

                using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    au.DownloadStringCompleted += au_DownloadStringCompletedCheck;
                    au.DownloadStringAsync(new Uri(url+"?check=1"));
                    AUWP7.Utils.Adler32 ad32 = new AUWP7.Utils.Adler32();
                    if (myIsolatedStorage.FileExists(nombre + ".xml")) {
                       IsolatedStorageFileStream isofile = myIsolatedStorage.OpenFile(nombre + ".xml", FileMode.OpenOrCreate, FileAccess.ReadWrite);
                       byte[] buffer = new byte[isofile.Length]; 

                       isofile.Read(buffer, 0, Int32.Parse(""+isofile.Length));

                       ad32.Update(buffer);
                       isofile.Close();
                    }

                    while (!cont) ;
                    string Hex = String.Format("{0:X8}", ad32.Checksum);
                    bool valido = Hex.Equals(this.check.ToUpper());

                    if (myIsolatedStorage.FileExists(nombre + ".xml") && valido)
                    {
                       IsolatedStorageFileStream isofile = myIsolatedStorage.OpenFile(nombre + ".xml", FileMode.OpenOrCreate, FileAccess.ReadWrite);
                       StreamReader sr = new StreamReader(isofile);
                       xmlData = sr.ReadToEnd();
                       processXML(xmlData);
                       isofile.Close();
                    }
                    else
                    {
                        au.DownloadStringCompleted += au_DownloadStringCompleted;
                        au.DownloadStringAsync(new Uri(url));

                   }

                }

            }
            catch (System.UnauthorizedAccessException ea)
            {
                System.Diagnostics.Debug.WriteLine(ea.Message);
            }
            catch (System.MethodAccessException es)
            {
                System.Diagnostics.Debug.WriteLine(es.Message);
            }
            catch (Exception es)
            {
                System.Diagnostics.Debug.WriteLine(es.Message);
                au.DownloadStringCompleted += au_DownloadStringCompleted;
                au.DownloadStringAsync(new Uri(url));
            }
        }

        public virtual void au_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
        
        }

        public void au_DownloadStringCompletedCheck(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                check = e.Result;
                this.cont = true;
            }
        }

        public virtual void processXML(String xmlData)
        {

        }

        public void save(String data)
        {
            App.Current.RootVisual.UpdateLayout();
            using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                IsolatedStorageFileStream isofile = myIsolatedStorage.OpenFile(nombre + ".xml", FileMode.OpenOrCreate, FileAccess.ReadWrite);
                StreamWriter sw = new StreamWriter(isofile);
                sw.Write(data);
                sw.Flush();
                isofile.Close();
                DateTime Limit2 = DateTime.UtcNow;
                Limit2.AddHours(-2);
                if (myIsolatedStorage.FileExists("test" + nombre + Limit2.Hour))
                {
                    myIsolatedStorage.DeleteFile("test" + nombre + Limit2.Hour);
                }
                Limit2.AddHours(1);
                //myIsolatedStorage.OpenFile("test" + nombre + Limit2.Hour, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            }
        }

        protected String filterName(String s)
        {
            String ret = "";

            ret = s.Replace("%5B","[");
            ret = ret.Replace("%5D", "]");
            ret = ret.Replace("%20", "_");

            return ret;
        }

        protected void wc_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            if (e.Error == null && !e.Cancelled)
            {
                String url = "";
                try
                {
                    url = (String)e.UserState;
                    url = "/imgData/" + url;
                    using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
                    {
                        if (!myIsolatedStorage.DirectoryExists("/imgData"))
                        {
                            myIsolatedStorage.CreateDirectory("/imgData");
                        }

                        if (!myIsolatedStorage.FileExists(url))
                        {

                            IsolatedStorageFileStream fileStream = myIsolatedStorage.CreateFile(url);

                            BitmapImage bitmap = new BitmapImage();
                            bitmap.SetSource(e.Result);
                            WriteableBitmap wb = new WriteableBitmap(bitmap);

                            // Encode WriteableBitmap object to a JPEG stream.
                            System.Windows.Media.Imaging.Extensions.SaveJpeg(wb, fileStream, wb.PixelWidth, wb.PixelHeight, 0, 85);

                            //wb.SaveJpeg(fileStream, wb.PixelWidth, wb.PixelHeight, 0, 85);
                            fileStream.Close();
                        }
                        else
                        {

                        }

                    }
                }
                catch (Exception ex)
                {
                    //Exception handle appropriately for your app
                }
            }
            else
            {
                //Either cancelled or error handle appropriately for your app
            }

        }

        // Create the OnPropertyChanged method to raise the event
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
