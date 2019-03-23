namespace IrisApp.ViewModels.Home
{
    using System;
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using IrisApp.Models.Home;
    using IrisApp.Utils;

    public class LogsViewModel : BaseViewModel, IPageViewModel
    {
        private ObservableCollection<LogModel> notifications;

        public LogsViewModel()
        {
            this.Notifications = new ObservableCollection<LogModel>();
            for (int i = 0; i < 5; i++)
            {
                this.AddNotification(new LogModel() { Code = 'M', Name = "Material Design", Description = "Material Design" });
                this.AddNotification(new LogModel() { Code = 'D', Name = "Dragablz", Description = "Dragablz Tab Control" });
                this.AddNotification(new LogModel() { Code = 'P', Name = "Predator", Description = "If it bleeds, we can kill it"});
            }
        }

        public ObservableCollection<LogModel> Notifications
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

        public ICommand DeleteNotificationCommand => new RelayCommand<LogModel>(param =>
        {
            try
            {
                this.Notifications.Remove(param);
            }
            catch (Exception)
            {
                // TODO
            }
        });

        private void AddNotification(LogModel notification)
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
    }
}
