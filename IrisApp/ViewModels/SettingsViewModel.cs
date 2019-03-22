namespace IrisApp.ViewModels
{
    public class SettingsViewModel : BaseViewModel, IPageViewModel
    {
        private byte innerBoundaryRadiusFrom = 20;

        public byte InnerBoundaryRadiusFrom
        {
            get => this.innerBoundaryRadiusFrom;
            set
            {
                if (this.innerBoundaryRadiusFrom == value)
                {
                    return;
                }

                if (value < 10)
                {
                    this.innerBoundaryRadiusFrom = 10;
                }
                else
                {
                    this.innerBoundaryRadiusFrom = value;
                }

                this.OnPropertyChanged(nameof(this.InnerBoundaryRadiusFrom));
            }
        }
    }
}
