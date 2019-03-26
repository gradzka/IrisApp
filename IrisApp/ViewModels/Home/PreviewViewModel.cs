namespace IrisApp.ViewModels.Home
{
    using System.Collections.ObjectModel;
    using IrisApp.Models.Home;
    using IrisApp.Models.IrisProcessor;

    public class PreviewViewModel : BaseViewModel, IPageViewModel
    {
        private char chosenEye = 'L';

        public PreviewViewModel(IrisProcessorModel processor, ObservableCollection<LogModel> logs)
            : base(processor, logs)
        {
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
    }
}
