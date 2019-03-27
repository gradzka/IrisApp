namespace IrisApp.ViewModels.Home
{
    using System;
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using IrisApp.Models.Home;
    using IrisApp.Models.IrisProcessor;
    using IrisApp.Utils;

    public class EyeDialogViewModel : BaseViewModel, IPageViewModel
    {
        private char chosenEye;
        private ObservableCollection<EyeModel> eyes;
        private bool isDialogOpen;
        private object windowsFormsHost;

        public EyeDialogViewModel(IrisProcessorModel processor, ObservableCollection<LogModel> logs)
            : base(processor, logs)
        {
            this.Eyes = new ObservableCollection<EyeModel>();
            this.FillEyes();
            this.ChosenEye = this.Eyes[2].Sign;
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

        public ObservableCollection<EyeModel> Eyes
        {
            get => this.eyes;
            set
            {
                if (this.eyes == value)
                {
                    return;
                }

                this.eyes = value;
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

        public SourceModel SelectedSourceRef { get; set; }

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

        public ICommand CloseDialogCommand => new RelayCommand<Action>(param =>
        {
            this.IsDialogOpen = false;
        });

        public ICommand OKCommand => new RelayCommand<Action>(async param =>
        {
            this.IsDialogOpen = false;

            if (this.SelectedSourceRef.Name.Equals("File", StringComparison.Ordinal))
            {
                Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog
                {
                    Filter = "Image files (*.png;*.jpeg)|*.png;*.jpeg|All files (*.*)|*.*"
                };
                if (openFileDialog.ShowDialog() == true)
                {
                    await this.Processor.LoadFromImageAsync(openFileDialog.FileName, this.ChosenEye);
                    this.WindowsFormsHost = this.Processor.GetPreviewControl();
                    this.GetLogsFromProcessor();
                }
            }

            // Device
            else
            {
                this.WindowsFormsHost = this.Processor.GetPreviewControl(true);
                await this.Processor.LoadFromScannerAsync(this.SelectedSourceRef.Device, this.ChosenEye);
                this.WindowsFormsHost = this.Processor.GetPreviewControl();
                this.GetLogsFromProcessor();
            }
        });

        public ICommand ShowDialogCommand => new RelayCommand<SourceModel>(param =>
        {
            if (param is null)
            {
                this.Logs.Insert(0, LogSingleton.Instance.NoSourceSelected);
                return;
            }

            this.IsDialogOpen = true;
            this.SelectedSourceRef = param;
        });

        private void FillEyes()
        {
            try
            {
                this.Eyes.Add(new EyeModel { Sign = 'L', Name = "Left" });
                this.Eyes.Add(new EyeModel { Sign = 'R', Name = "Right" });
                this.Eyes.Add(new EyeModel { Sign = 'U', Name = "Unknown" });
            }
            catch (Exception)
            {
                // TODO
            }
        }
    }
}
