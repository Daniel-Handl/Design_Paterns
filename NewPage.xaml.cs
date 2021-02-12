using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using GalaSoft.MvvmLight.Command;
using System.ComponentModel;

namespace DesignPaterns
{
    /// <summary>
    /// Interakční logika pro NewPage.xaml
    /// </summary>
    public partial class NewPage : Page
    {
        public NewPage()
        {
            InitializeComponent(); 
            DataContext = new ZpravaViewModel();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var z in ZpravaModel.ZpravaDatabase.VsechnyZpravy)
            {

                sb.Append(z + "\n");
                sb.Append("\n");
            }
            MessageBox.Show(sb.ToString());
        }
    }




    public class ZpravaViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        public ZpravaViewModel()
        {
            if (ZpravaModel.ZpravaDatabase.VsechnyZpravy.Count == 0)
            {
                Jmeno = "";
            }
            else
            {
                Jmeno = jmeno;
            }
        }

        private string jmeno;
        private string rc;
        private DateTime datum;

        // Zprava je bindovaná z GUI
        public string Jmeno
        {
            get { return jmeno; }
            set
            {
                jmeno = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Jmeno)));
            }
        }

        public string Rc
        {
            get { return rc; }
            set
            {
                rc = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Rc)));
            }
        }
        public DateTime Datum
        {
            get { return datum; }
            set
            {
                datum = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Datum)));
            }
        }

        private static ICommand _sendCommand;
        public int i = 0;

        // SendCommand je bindovaný z GUI
        public ICommand SendCommand
        {
            get
            {

                if (_sendCommand == null)
                {
                    // RelayCommand je definovaný v MVVMLight
                    _sendCommand = new RelayCommand(
                        () => {
                            Debug.WriteLine(Jmeno);
                            string[] sp = jmeno.Split(' ');
                            string j = sp[0];
                            string p = sp[1];
                            ZpravaModel.ZpravaDatabase.VsechnyZpravy[jmeno] = new Person(j,p,Rc,Datum.ToString());

                        });
                    
                }
                return _sendCommand;
            }
        }
    }



    public class ZpravaModel
    {
        static ZpravaModel inst = null;

        public static ZpravaModel ZpravaDatabase
        {
            get
            {
                if (inst == null)
                {
                    inst = new ZpravaModel();
                }
                return inst;
            }
        }

        private ZpravaModel()
        {
            VsechnyZpravy = new Dictionary<string, object>();

        }

        public Dictionary<string, object> VsechnyZpravy;
    }

    public class Person
    {
        public Person(string jmeno,string prijmeni, string rc, string datum)
        {
            Jmeno = jmeno;
            Prijmeni = prijmeni;
            Rc = rc;
            Datum = datum;
        }

        public string Jmeno { get; set; }
        public string Prijmeni { get; set; }
        public string Rc { get; set; }
        public string Datum{ get; set; }

    }

}
