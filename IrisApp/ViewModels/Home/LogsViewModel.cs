namespace IrisApp.ViewModels.Home
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;
    using IrisApp.Infrastructure;
    using IrisApp.Models.Home;
    using IrisApp.Models.IrisProcessor;
    using IrisApp.Utils;

    public class LogsViewModel : BaseViewModel, IPageViewModel
    {
        public LogsViewModel(IrisProcessorModel processor, ObservableCollection<LogModel> logs)
            : base(processor, logs)
        {
        }

        public ICommand DeleteAllLogsCommand => new RelayCommand<bool>(deleteSelectedIsChecked =>
        {
            try
            {
                if (deleteSelectedIsChecked)
                {
                    this.Logs.RemoveAll(x => x.IsSelected);
                }
                else
                {
                    this.Logs.Clear();
                }
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
