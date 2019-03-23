namespace IrisApp.ViewModels.Settings
{
    using System;
    using System.Collections.ObjectModel;
    using IrisApp.Models.Settings;

    public class MatchingViewModel : BaseViewModel, IPageViewModel
    {
        private ObservableCollection<FARComboboxModel> fAR;
        private bool isFirstReadOnlyChecked;
        private ObservableCollection<string> matchingSpeed;
        private int maximalResultCount;
        private int maximalRotation;
        private FARComboboxModel selectedFAR;
        private string selectedMatchingSpeed;

        public MatchingViewModel()
        {
            this.FAR = new ObservableCollection<FARComboboxModel>();
            this.MatchingSpeed = new ObservableCollection<string>();
            this.SetFactorySettings();
        }

        public ObservableCollection<FARComboboxModel> FAR
        {
            get => this.fAR;

            set
            {
                if (this.fAR == value)
                {
                    return;
                }

                this.fAR = value;
            }
        }

        public bool IsFirstReadOnlyChecked
        {
            get => this.isFirstReadOnlyChecked;

            set
            {
                if (this.isFirstReadOnlyChecked == value)
                {
                    return;
                }

                this.isFirstReadOnlyChecked = value;
                this.OnPropertyChanged(nameof(this.IsFirstReadOnlyChecked));
            }
        }

        public ObservableCollection<string> MatchingSpeed
        {
            get => this.matchingSpeed;

            set
            {
                if (this.matchingSpeed == value)
                {
                    return;
                }

                this.matchingSpeed = value;
                this.OnPropertyChanged(nameof(this.MatchingSpeed));
            }
        }

        public int MaximalResultCount
        {
            get => this.maximalResultCount;

            set
            {
                if (this.maximalResultCount == value)
                {
                    return;
                }

                this.maximalResultCount = value;
                this.OnPropertyChanged(nameof(this.MaximalResultCount));
            }
        }

        public int MaximalRotation
        {
            get => this.maximalRotation;

            set
            {
                if (this.maximalRotation == value)
                {
                    return;
                }

                this.maximalRotation = value;
                this.OnPropertyChanged(nameof(this.MaximalRotation));
            }
        }

        public FARComboboxModel SelectedFAR
        {
            get => this.selectedFAR;

            set
            {
                if (this.selectedFAR == value)
                {
                    return;
                }

                this.selectedFAR = value;
                this.OnPropertyChanged(nameof(this.SelectedFAR));
            }
        }

        public string SelectedMatchingSpeed
        {
            get => this.selectedMatchingSpeed;

            set
            {
                if (this.selectedMatchingSpeed == value)
                {
                    return;
                }

                this.selectedMatchingSpeed = value;
                this.OnPropertyChanged(nameof(this.SelectedMatchingSpeed));
            }
        }

        public void SetFactorySettings()
        {
            try
            {
                this.FAR.Add(new FARComboboxModel { Key = 0.1m, Value = "0.1%" });
                this.FAR.Add(new FARComboboxModel { Key = 0.01m, Value = "0.01%" });
                this.FAR.Add(new FARComboboxModel { Key = 0.001m, Value = "0.001%" });
                this.FAR.Add(new FARComboboxModel { Key = 0.0001m, Value = "0.0001%" });
                this.IsFirstReadOnlyChecked = false;
                this.MatchingSpeed.Add("Low");
                this.MatchingSpeed.Add("Medium");
                this.MatchingSpeed.Add("High");
                this.MaximalResultCount = 10;
                this.MaximalRotation = 15;
                this.SelectedFAR = this.FAR[0];
                this.SelectedMatchingSpeed = this.MatchingSpeed[0];
            }
            catch (Exception)
            {
                // TODO
            }
        }
    }
}
