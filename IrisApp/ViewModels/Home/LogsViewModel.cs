namespace IrisApp.ViewModels.Home
{
    using System;
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using IrisApp.Models.Home;
    using IrisApp.Models.IrisProcessor;
    using IrisApp.Utils;

    public class LogsViewModel : BaseViewModel, IPageViewModel
    {
        public LogsViewModel(IrisProcessorModel processor, ObservableCollection<LogModel> logs)
            : base(processor, logs)
        {
        }

        public ICommand DeleteAllLogsCommand => new RelayCommand<Action>(param =>
        {
            try
            {
                this.Logs.Clear();
            }
            catch (Exception)
            {
                // TODO
            }
        });

        public ICommand DeleteLogCommand => new RelayCommand<LogModel>(param =>
        {
            try
            {
                this.Logs.Remove(param);
            }
            catch (Exception)
            {
                // TODO
            }
        });
    }
}
