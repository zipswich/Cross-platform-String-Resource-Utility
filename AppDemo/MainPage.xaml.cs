using ClassLibraryDemo;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using XResourceUtils;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace AppDemo
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        const string sLanguageCodeKey = "LanguageCodeKey";
        ObservableCollection<LanguageViewModel> _ocLanguages = new ObservableCollection<LanguageViewModel>();
        public ObservableCollection<LanguageViewModel> ocLanguages
        {
            get { return _ocLanguages; }
            set
            {
                if (_ocLanguages != value)
                {
                    _ocLanguages = value;
                    NotifyPropertyChanged();
                }
            }
        }
        LanguageViewModel _languageSelected = null;
        public LanguageViewModel languageSelected
        {
            get { return _languageSelected; }
            set
            {
                if (_languageSelected != value)
                {
                    if (_languageSelected == null)
                    {
                        _languageSelected = value;
                    }
                    else
                    {
                        _languageSelected = value;
                        SetLanguageAsync(value);
                    }
                    NotifyPropertyChanged();
                }
            }
        }


        string _sBindingText = string.Empty;
        public string sBindingText
        {
            get { return _sBindingText; }
            set
            {
                if (_sBindingText != value)
                {
                    _sBindingText = value;
                    NotifyPropertyChanged();
                }
            }
        }


        string _sBindingLibraryText = string.Empty;
        public string sBindingLibraryText
        {
            get { return _sBindingLibraryText; }
            set
            {
                if (_sBindingLibraryText != value)
                {
                    _sBindingLibraryText = value;
                    NotifyPropertyChanged();
                }
            }
        }





        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ocLanguages.Clear();
            ocLanguages.Add(new LanguageViewModel("default", "Default"));
            ocLanguages.Add(new LanguageViewModel("de", "German"));
            ocLanguages.Add(new LanguageViewModel("es", "Spanish"));
            ocLanguages.Add(new LanguageViewModel("fr", "French"));
            ocLanguages.Add(new LanguageViewModel("it", "Italian"));
            ocLanguages.Add(new LanguageViewModel("nl", "Dutch"));
            ocLanguages.Add(new LanguageViewModel("pt", "Portuguese"));
            ocLanguages.Add(new LanguageViewModel("ru", "Russian"));
            ocLanguages.Add(new LanguageViewModel("uk", "Ukrainian"));
            ocLanguages.Add(new LanguageViewModel("zh", "Chinese"));

            string sCurrentLanguageCode = Utility.GetOrSetDefaultSettingValue<string>(sLanguageCodeKey, ocLanguages.FirstOrDefault().sLanguageCode);

            languageSelected = ocLanguages.FirstOrDefault(r => r.sLanguageCode == sCurrentLanguageCode);
            UpdatePage();
            base.OnNavigatedTo(e);
        }

        private void UpdatePage()
        {
            sBindingText = XRUtils.GetString("welcome");
            sBindingLibraryText = XRUtils.GetString("from_a_library");
            Bindings.Update();
        }

        private void SetLanguageAsync(LanguageViewModel value)
        {
            try
            {
                if (value == null)
                {
                    Debug.WriteLine("Strange! LanguageViewModel value is null in SetLanguageAsync()");
                }
                else
                {
                    if (value.sLanguageCode == Utility.GetOrSetDefaultSettingValue(sLanguageCodeKey, ocLanguages.FirstOrDefault().sLanguageCode))
                    {
                        //do nothing
                    }
                    else
                    {
                        Utility.SetSettingValue(sLanguageCodeKey, value.sLanguageCode);
                        App.InitializeXRUtils();
                        UpdatePage();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("From SetLanguageAsync:" + ex.Message);
            }
        }

    }
}
