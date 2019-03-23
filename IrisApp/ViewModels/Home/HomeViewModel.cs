namespace IrisApp.ViewModels.Home
{
    using System;
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using IrisApp.Models.Home;
    using IrisApp.Utils;

    public class HomeViewModel : BaseViewModel, IPageViewModel
    {
        private ObservableCollection<SourceModel> sources;
        private SourceModel selectedSource;

        public HomeViewModel()
        {
            this.DialogViewModel = new DialogViewModel();
            this.LogsViewModel = new LogsViewModel();
            this.PreviewViewModel = new PreviewViewModel();
            this.sources = new ObservableCollection<SourceModel>();
            this.AddSource("iris");
            this.AddSource("cam");
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

        private void AddSource(string name)
        {
            try
            {
                this.Sources.Add(new SourceModel
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
