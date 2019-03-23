namespace IrisApp.ViewModels.Settings
{
    public class EnrollmentViewModel : BaseViewModel, IPageViewModel
    {
        private byte innerBoundaryRadiusFrom;
        private byte innerBoundaryRadiusTo;
        private byte outerBoundaryRadiusFrom;
        private byte outerBoundaryRadiusTo;
        private byte qualityThreshold;

        public EnrollmentViewModel()
        {
            this.SetFactorySettings();
        }

        public byte InnerBoundaryRadiusFrom
        {
            get => this.innerBoundaryRadiusFrom;
            set
            {
                if (this.innerBoundaryRadiusFrom == value)
                {
                    return;
                }

                this.innerBoundaryRadiusFrom = value;
                this.OnPropertyChanged(nameof(this.InnerBoundaryRadiusFrom));
            }
        }

        public byte InnerBoundaryRadiusTo
        {
            get => this.innerBoundaryRadiusTo;
            set
            {
                if (this.innerBoundaryRadiusTo == value)
                {
                    return;
                }

                this.innerBoundaryRadiusTo = value;
                this.OnPropertyChanged(nameof(this.InnerBoundaryRadiusTo));
            }
        }

        public byte OuterBoundaryRadiusFrom
        {
            get => this.outerBoundaryRadiusFrom;
            set
            {
                if (this.outerBoundaryRadiusFrom == value)
                {
                    return;
                }

                this.outerBoundaryRadiusFrom = value;
                this.OnPropertyChanged(nameof(this.OuterBoundaryRadiusFrom));
            }
        }

        public byte OuterBoundaryRadiusTo
        {
            get => this.outerBoundaryRadiusTo;
            set
            {
                if (this.outerBoundaryRadiusTo == value)
                {
                    return;
                }

                this.outerBoundaryRadiusTo = value;
                this.OnPropertyChanged(nameof(this.OuterBoundaryRadiusTo));
            }
        }

        public byte QualityThreshold
        {
            get => this.qualityThreshold;
            set
            {
                if (this.qualityThreshold == value)
                {
                    return;
                }

                this.qualityThreshold = value;
                this.OnPropertyChanged(nameof(this.QualityThreshold));
            }
        }

        public void SetFactorySettings()
        {
            this.InnerBoundaryRadiusFrom = 20;
            this.InnerBoundaryRadiusTo = 80;
            this.OuterBoundaryRadiusFrom = 70;
            this.OuterBoundaryRadiusTo = 170;
            this.QualityThreshold = 10;
        }
    }
}
