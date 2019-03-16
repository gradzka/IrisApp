// https://www.technical-recipes.com/2018/navigating-between-views-in-wpf-mvvm/

namespace IrisApp.ViewModels
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;

    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

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
