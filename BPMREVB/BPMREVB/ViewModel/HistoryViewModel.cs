using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace BPMREVB.ViewModel
{
    class HistoryViewModel : INotifyPropertyChanged
    {
        private string defaultText = "Button";
        private string displayText = "";
        public event PropertyChangedEventHandler PropertyChanged;

        public HistoryViewModel()
        {
            AddNewLabel = new Command<string>((key) => { displayText += defaultText + key; });
        } 
    public ICommand AddNewLabel{ protected set; get; }
                public string DisplayText
        {
            protected set 
            {
                if (displayText != value)
                {
                    displayText = value;
                    OnPropertyChanged("DisplayText");
                }
            }
            get { return displayText; }
        }
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
