namespace IrisApp.ViewModels.Settings
{
    using System;
    using System.Windows.Input;
    using IrisApp.Utils;

    public class SettingsViewModel : BaseViewModel, IPageViewModel
    {
        public SettingsViewModel()
        {
            this.EnrollmentViewModel = new EnrollmentViewModel();
            this.MatchingViewModel = new MatchingViewModel();
        }

        public EnrollmentViewModel EnrollmentViewModel { get; set; }

        public MatchingViewModel MatchingViewModel { get; set; }

        public ICommand ResetCommand => new RelayCommand<Action>(param =>
        {
            try
            {
                this.EnrollmentViewModel.SetFactorySettings();
                this.MatchingViewModel.SetFactorySettings();
            }
            catch (Exception)
            {
                // TODO
            }
        });

        public ICommand SaveCommand => new RelayCommand<Action>(param =>
        {
            try
            {
                // TODO
            }
            catch (Exception)
            {
                // TODO
            }
        });

        public ICommand UndoCommand => new RelayCommand<Action>(param =>
        {
            try
            {
                // TODO
            }
            catch (Exception)
            {
                // TODO
            }
        });
    }
}
