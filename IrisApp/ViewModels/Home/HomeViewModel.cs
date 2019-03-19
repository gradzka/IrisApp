namespace IrisApp.ViewModels.Home
{
    using System;
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using IrisApp.Utils;

    public class HomeViewModel : BaseViewModel, IPageViewModel
    {

        private ObservableCollection<SourceViewModel> sources;
        private ObservableCollection<NotificationViewModel> notifications;
        private char chosenEye = 'L';
        private string selectedSource = string.Empty;

        public HomeViewModel()
        {
            this.Notifications = new ObservableCollection<NotificationViewModel>();
            for (int i = 0; i < 5; i++)
            {
                this.AddNotification('M', "Material Design", "Material Design");
                this.AddNotification('D', "Dragablz", "Dragablz Tab Control");
                this.AddNotification('P', "Predator", "If it bleeds, we can kill it");
            }

            this.sources = new ObservableCollection<SourceViewModel>();
            this.AddSource("iris");
            this.AddSource("cam");
        }

        public ObservableCollection<SourceViewModel> Sources
        {
            get => this.sources;
            set
            {
                if (this.sources == value)
                {
                    return;
                }

                this.sources = value;
            }
        }

        public ObservableCollection<NotificationViewModel> Notifications
        {
            get => this.notifications;
            set
            {
                if (this.notifications == value)
                {
                    return;
                }

                this.notifications = value;
            }
        }

        public char ChosenEye
        {
            get => this.chosenEye;

            set
            {
                if (this.chosenEye == value)
                {
                    return;
                }

                this.chosenEye = value;
                this.OnPropertyChanged(nameof(this.ChosenEye));
            }
        }

        public string SelectedSource
        {
            get => this.selectedSource;
            set
            {
                if (this.selectedSource == value)
                {
                    return;
                }

                this.selectedSource = value;
                this.OnPropertyChanged(nameof(this.SelectedSource));
            }
        }

        public ICommand DeleteAllNotifications => new RelayCommand<Action>(param =>
        {
            try
            {
                this.Notifications.Clear();
            }
            catch (Exception)
            {
                // TODO
            }
        });

        public ICommand ScanSources => new RelayCommand<Action>(param =>
        {
            // TODO
        });

        public ICommand UseSelectedSource => new RelayCommand<Action>(param =>
        {
            // TODO
        });

        public ICommand SaveTemplateToDatabase => new RelayCommand<Action>(param =>
        {
            // TODO
        });

        public ICommand SaveImage => new RelayCommand<Action>(param =>
        {
            // TODO
        });

        public ICommand Identify => new RelayCommand<Action>(param =>
        {
            // TODO
        });

        private void AddSource(string name)
        {
            try
            {
                this.Sources.Add(new SourceViewModel
                {
                    Name = name
                });
            }
            catch (Exception)
            {
                // TODO
            }
        }

        private void AddNotification(char code, string name, string description)
        {
            try
            {
                this.Notifications.Add(new NotificationViewModel
                {
                    Code = code,
                    Name = name,
                    Description = description,
                    Notifications = this.Notifications
                });
            }
            catch (Exception)
            {
                // TODO
            }
        }
    }
}
