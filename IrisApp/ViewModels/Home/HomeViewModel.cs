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
        private SourceViewModel selectedSource;

        public HomeViewModel()
        {
            this.Notifications = new ObservableCollection<NotificationViewModel>();
            for (int i = 0; i < 5; i++)
            {
                this.AddNotification(new NotificationViewModel() { Code = 'M', Name = "Material Design", Description = "Material Design", Notifications = this.Notifications });
                this.AddNotification(new NotificationViewModel() { Code = 'D', Name = "Dragablz", Description = "Dragablz Tab Control", Notifications = this.Notifications });
                this.AddNotification(new NotificationViewModel() { Code = 'P', Name = "Predator", Description = "If it bleeds, we can kill it", Notifications = this.Notifications });
            }

            this.sources = new ObservableCollection<SourceViewModel>();
            this.AddSource("iris");
            this.AddSource("cam");
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

        public SourceViewModel SelectedSource
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

        public ICommand DeleteAllNotificationsCommand => new RelayCommand<Action>(param =>
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

        public ICommand IdentifyCommand => new RelayCommand<Action>(param =>
        {
            // TODO
        });

        public ICommand ScanSourcesCommand => new RelayCommand<Action>(param =>
        {
            // TODO
        });

        public ICommand UseSelectedSourceCommand => new RelayCommand<Action>(param =>
        {
            // TODO
        });

        private void AddNotification(NotificationViewModel notification)
        {
            try
            {
                this.Notifications.Add(notification);
            }
            catch (Exception)
            {
                // TODO
            }
        }

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
    }
}
