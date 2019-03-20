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
        private ObservableCollection<SubjectViewModel> subjects;
        private char chosenEye = 'L';
        private bool isDialogOpen;
        private SourceViewModel selectedSource;
        private SubjectViewModel selectedSubject;

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

            this.subjects = new ObservableCollection<SubjectViewModel>();
            this.AddSubject(new SubjectViewModel() { SubjectID = 0 });
            this.AddSubject(new SubjectViewModel() { SubjectID = 1 });
            this.AddSubject(new SubjectViewModel() { SubjectID = 2 });
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

        public bool IsDialogOpen
        {
            get => this.isDialogOpen;

            set
            {
                if (this.isDialogOpen == value)
                {
                    return;
                }

                this.isDialogOpen = value;
                this.OnPropertyChanged(nameof(this.IsDialogOpen));
            }
        }

        public SubjectViewModel SelectedSubject
        {
            get => this.selectedSubject;
            set
            {
                if (this.selectedSubject == value)
                {
                    return;
                }

                this.selectedSubject = value;
                this.OnPropertyChanged(nameof(this.SelectedSubject));
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

        public ObservableCollection<SubjectViewModel> Subjects
        {
            get => this.subjects;
            set
            {
                if (this.subjects == value)
                {
                    return;
                }

                this.subjects = value;
            }
        }

        public ICommand CloseDialogCommand => new RelayCommand<Action>(param =>
        {
            this.IsDialogOpen = false;
        });

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

        public ICommand SaveCommand => new RelayCommand<bool>(subjectsComboboxIsEnabled =>
        {
            if (subjectsComboboxIsEnabled == true)
            {
                SubjectViewModel selectedSubject = this.SelectedSubject;

                // existing subject
            }
            else
            {
                // create new subject
            }

            // TODO
            this.IsDialogOpen = false;
        });

        public ICommand ScanSourcesCommand => new RelayCommand<Action>(param =>
        {
            // TODO
            this.IsDialogOpen = false;
        });

        public ICommand ShowDialogCommand => new RelayCommand<Action>(param =>
        {
            this.IsDialogOpen = true;
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

        private void AddSubject(SubjectViewModel subject)
        {
            try
            {
                this.Subjects.Add(subject);
            }
            catch (Exception)
            {
                // TODO
            }
        }
    }
}
