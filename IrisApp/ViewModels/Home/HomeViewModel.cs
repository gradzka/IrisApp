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
        private object windowsFormsHost;

        public HomeViewModel(IrisProcessorModel processor, ObservableCollection<LogModel> logs)
            : base(processor, logs)
        {
            this.DialogViewModel = new DialogViewModel(processor, logs);
            this.LogsViewModel = new LogsViewModel(processor, logs);
            this.PreviewViewModel = new PreviewViewModel(processor, logs);
            this.sources = new ObservableCollection<SourceModel>();
            this.CollectSources();
        }

        public DialogViewModel DialogViewModel { get; set; }

        public LogsViewModel LogsViewModel { get; set; }

        public PreviewViewModel PreviewViewModel { get; set; }

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

        public object WindowsFormsHost
        {
            get => this.windowsFormsHost;
            set
            {
                if (this.windowsFormsHost == value)
                {
                    return;
                }

                this.windowsFormsHost = value;
                this.OnPropertyChanged(nameof(this.WindowsFormsHost));
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

        public ICommand UseSelectedSourceCommand => new RelayCommand<Action>(async param =>
        {
            if (this.SelectedSource == null)
            {
                this.Logs.Insert(0, new LogModel() { Code = 'E', Description = "No source selected", Name = "Source" });
            }

            // File
            else if (this.SelectedSource == this.Sources[0])
            {
                Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog
                {
                    Filter = "Image files (*.png;*.jpeg)|*.png;*.jpeg|All files (*.*)|*.*"
                };
                if (openFileDialog.ShowDialog() == true)
                {
                    await this.Processor.LoadFromImageAsync(openFileDialog.FileName);
                    this.WindowsFormsHost = this.Processor.GetPreviewControl();
                    this.GetLogsFromProcessor();
                }
            }

            // Device
            else
            {
                this.WindowsFormsHost = this.Processor.GetPreviewControl(true);
                await this.Processor.LoadFromScannerAsync(this.SelectedSource.Device, this.PreviewViewModel.ChosenEye);
                this.WindowsFormsHost = this.Processor.GetPreviewControl();
                this.GetLogsFromProcessor();
            }
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
