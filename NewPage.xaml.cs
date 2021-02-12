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
using System.Text.RegularExpressions;

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

                sb.Append(z.Value.ToString() + "\n");
                
            }
            MessageBox.Show(sb.ToString());
        }
    }




    public class ZpravaViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        public ZpravaViewModel()
        {
            if (ZpravaModel.ZpravaDatabase.VsechnyZpravy.Keys.Count == 0)
            {
                Jmeno = "";
            }
            else
            {
                int ic = 0;
                Dictionary<string, object>.KeyCollection kc = ZpravaModel.ZpravaDatabase.VsechnyZpravy.Keys;
                foreach (var key in kc)
                {
                    if (ic == ZpravaModel.ZpravaDatabase.VsechnyZpravy.Count-1)
                    {
                        ZpravaModel.ZpravaDatabase.VsechnyZpravy[key] = new Person(j, p, Rc, Datum.ToString());
                    }
                    ic++;
                }
            }
        }
        private string j;
        private string p;
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
                if (value.Contains(' '))
                {
                string[] sp = value.Split(' ');
                j = sp[0];
                p = sp[1];
                }
                else
                {
                    j = value;
                    p = "";
                }

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
                            Debug.WriteLine(Rc);
                            Debug.WriteLine(Datum);
                            ZpravaModel.ZpravaDatabase.VsechnyZpravy[Jmeno] = new Person(j,p,Rc,Datum.ToString());

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

    interface IStringValidator
    {
        bool IsValid(string s);
    }
    interface IDateValidator
    {
        bool IsValidAbove(string s, out bool nadpod);
    }


    public class Person
    {
        readonly IDateValidator datumValidator;
        readonly IStringValidator pValidator;
        readonly IStringValidator jValidator;
        readonly IStringValidator rcValidator;

        public Person(string jmeno,string prijmeni, string rc, string datum,)
        {
            Jmeno = jmeno;
            Prijmeni = prijmeni;
            Rc = rc;
            Datum = datum;
        }

        public override string ToString()
        {
            return $"{Jmeno} {Prijmeni},Rodné číslo: {Rc},Narozen: {Datum}";
        }
        public string Jmeno { get; set; }
        public string Prijmeni { get; set; }
        public string Rc { get; set; }
        public string Datum{ get; set; }

    }
    class Rc55Validator : IStringValidator
    {
        public bool IsValid(string s) 
        {
          Regex rx = new Regex(@"\D\D\D\D\D\D/\D\D\D\D", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            return rx.IsMatch(s);
        }
    }
    class Rc54Validator : IStringValidator
    {
        public bool IsValid(string s)
        {
            Regex rx = new Regex(@"\D\D\D\D\D\D/\D\D\D", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            return rx.IsMatch(s);
        }
    }
    class JPValidator : IStringValidator
    {
        public bool IsValid(string s)
        {
                return (!s.Contains(' ') && s.Length > 1);            
        }
    }
    class DatumValidator : IDateValidator
    {
        public bool IsValidAbove(string s, out bool nadpod)
        {
            if (DateTime.TryParse(s,out DateTime d))
            {
                if (d.Date > new DateTime(1954,1,1))
                {
                    nadpod = true;
                }
                else
                {
                    nadpod = false;
                }
                return true;
            }
            else
            {
                nadpod = false;
                return false;                
            }


        }
    }



}
