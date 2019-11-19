using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AppDemo
{
    public class LanguageViewModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members

        // This method is called by the Set accessor of each property. 
        // The CallerMemberName attribute that is applied to the optional propertyName 
        // parameter causes the property name of the caller to be substituted as an argument. 
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion


        string _sLanguageCode = string.Empty;
        public string sLanguageCode
        {
            get { return _sLanguageCode; }
            set
            {
                if (_sLanguageCode != value)
                {
                    _sLanguageCode = value;
                    NotifyPropertyChanged();
                }
            }
        }


        string _sLanguageDisplay = string.Empty;
        public string sLanguageDisplay
        {
            get { return _sLanguageDisplay; }
            set
            {
                if (_sLanguageDisplay != value)
                {
                    _sLanguageDisplay = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public LanguageViewModel(string sLanguageCode, string sLanguageDisplay)
        {
            this.sLanguageCode = sLanguageCode;
            this.sLanguageDisplay = sLanguageDisplay;
        }

        public override string ToString()
        {
            return sLanguageDisplay;
        }
    }
}
