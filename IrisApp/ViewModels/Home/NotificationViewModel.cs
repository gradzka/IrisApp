// Based on https://github.com/MaterialDesignInXAML/MaterialDesignInXamlToolkit/blob/master/MainDemo.Wpf/Domain/SelectableViewModel.cs

namespace IrisApp.ViewModels.Home
{
    using System;
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using IrisApp.Utils;

    public class NotificationViewModel : BaseViewModel, IPageViewModel
    {
        private char code;
        private string description;
        private bool isSelected;
        private string name;

        public char Code
        {
            get => this.code;
            set
            {
                if (this.code == value)
                {
                    return;
                }

                this.code = value;
                this.OnPropertyChanged(nameof(this.Code));
            }
        }

        public string Description
        {
            get => this.description;
            set
            {
                if (this.description == value)
                {
                    return;
                }

                this.description = value;
                this.OnPropertyChanged(nameof(this.Description));
            }
        }

        public bool IsSelected
        {
            get => this.isSelected;
            set
            {
                if (this.isSelected == value)
                {
                    return;
                }

                this.isSelected = value;
                this.OnPropertyChanged(nameof(this.IsSelected));
            }
        }

        public string Name
        {
            get => this.name;
            set
            {
                if (this.name == value)
                {
                    return;
                }

                this.name = value;
                this.OnPropertyChanged(nameof(this.Name));
            }
        }

        public ObservableCollection<NotificationViewModel> Notifications { get; set; }

        public ICommand DeleteNotification => new RelayCommand<Action>(param =>
        {
            try
            {
                this.Notifications.Remove(this);
            }
            catch (Exception e)
            {
                // TODO
            }
        });
    }
}
