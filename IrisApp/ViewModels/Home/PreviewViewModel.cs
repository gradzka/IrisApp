namespace IrisApp.ViewModels.Home
{
    public class PreviewViewModel : BaseViewModel, IPageViewModel
    {
        private char chosenEye = 'L';

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
