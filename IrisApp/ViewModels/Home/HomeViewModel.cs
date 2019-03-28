namespace IrisApp.ViewModels.Home
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using IrisApp.Models.Home;
    using IrisApp.Models.IrisProcessor;
    using IrisApp.Utils;

    public class HomeViewModel : BaseViewModel, IPageViewModel
    {
        private ObservableCollection<SourceModel> sources;
        private SourceModel selectedSource;

        public HomeViewModel(IrisProcessorModel processor, ObservableCollection<LogModel> logs)
            : base(processor, logs)
        {
            this.SaveDialogViewModel = new SaveDialogViewModel(processor, logs);
            this.EyeDialogViewModel = new EyeDialogViewModel(processor, logs);
            this.LogsViewModel = new LogsViewModel(processor, logs);
            this.Sources = new ObservableCollection<SourceModel>();
            this.CollectSources();
        }

        public SaveDialogViewModel SaveDialogViewModel { get; set; }

        public EyeDialogViewModel EyeDialogViewModel { get; set; }

        public LogsViewModel LogsViewModel { get; set; }

        public SourceModel SelectedSource
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

        public ObservableCollection<SourceModel> Sources
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

        public ICommand IdentifyCommand => new RelayCommand<Action>(async param =>
        {
            await this.Processor.IdentifyAsync();
            this.GetLogsFromProcessor();
        });

        public ICommand ScanSourcesCommand => new RelayCommand<Action>(param =>
        {
            this.Sources.Clear();
            this.SelectedSource = null;
            this.CollectSources();
        });

        private void CollectSources()
        {
            try
            {
                if (this.Processor.IsProcessorReady)
                {
                    this.Sources.Add(new SourceModel() { Name = "File", Device = null });
                    List<SourceModel> devices = this.Processor.GetDevices();
                    if (devices != null)
                    {
                        foreach (SourceModel device in devices)
                        {
                            this.Sources.Add(device);
                        }
                    }
                }
            }
            catch (Exception)
            {
                this.Sources.Clear();
            }
            finally
            {
                this.GetLogsFromProcessor();
            }
        }
    }
}
