namespace IrisApp.ViewModels.Home
{
    using System;
    using System.Collections.ObjectModel;

    public class HomeViewModel : BaseViewModel, IPageViewModel
    {

        private ObservableCollection<DeviceSourceViewModel> deviceSources;
        private ObservableCollection<NotificationViewModel> notifications;
        private string selectedDeviceSource = string.Empty;

        public HomeViewModel()
        {
            this.Notifications = new ObservableCollection<NotificationViewModel>();
            this.AddNotification('M', "Material Design", "Material Design");
            this.AddNotification('D', "Dragablz", "Dragablz Tab Control");
            this.AddNotification('P', "Predator", "If it bleeds, we can kill it");

            this.deviceSources = new ObservableCollection<DeviceSourceViewModel>();
            this.AddDeviceSource("iris");
            this.AddDeviceSource("cam");
        }

        public ObservableCollection<DeviceSourceViewModel> DeviceSources
        {
            get => this.deviceSources;
            set
            {
                if (this.deviceSources == value)
                {
                    return;
                }

                this.deviceSources = value;
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

        public string SelectedDeviceSource
        {
            get => this.selectedDeviceSource;
            set
            {
                if (this.selectedDeviceSource == value)
                {
                    return;
                }

                this.selectedDeviceSource = value;
                this.OnPropertyChanged(nameof(this.SelectedDeviceSource));
            }
        }

        private void AddDeviceSource(string name)
        {
            try
            {
                this.DeviceSources.Add(new DeviceSourceViewModel
                {
                    Name = name
                });
            }
            catch (Exception e)
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
            catch (Exception e)
            {
                // TODO
            }
        }
    }
}
