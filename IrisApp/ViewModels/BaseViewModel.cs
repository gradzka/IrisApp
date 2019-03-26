// https://www.technical-recipes.com/2018/navigating-between-views-in-wpf-mvvm/

namespace IrisApp.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Diagnostics;
    using IrisApp.Models.Home;
    using IrisApp.Models.IrisProcessor;

    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<LogModel> logs;

        public BaseViewModel(IrisProcessorModel processor, ObservableCollection<LogModel> logs)
        {
            this.Processor = processor;
            this.Logs = logs;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<LogModel> Logs
        {
            get => this.logs;
            set
            {
                if (this.logs == value)
                {
                    return;
                }

                this.logs = value;
            }
        }

        protected IrisProcessorModel Processor { get; set; }

        protected void GetLogsFromProcessor()
        {
            foreach (LogModel log in this.Processor.GetLogs())
            {
                this.Logs.Insert(0, log);
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            this.VerifyPropertyName(propertyName);
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        [Conditional("DEBUG")]
        private void VerifyPropertyName(string propertyName)
        {
            if (TypeDescriptor.GetProperties(this)[propertyName] == null)
            {
                throw new ArgumentNullException(this.GetType().Name + " does not contain property: " + propertyName);
            }
        }
    }
}
