namespace IrisApp.ViewModels.Settings
{
    using System;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Windows.Input;
    using IrisApp.Models.Home;
    using IrisApp.Models.IrisProcessor;
    using IrisApp.Utils;
    using Newtonsoft.Json;

    public class SettingsViewModel : BaseViewModel, IPageViewModel
    {
        public SettingsViewModel(IrisProcessorModel processor, ObservableCollection<LogModel> logs)
            : base(processor, logs)
        {
            this.EnrollmentViewModel = new EnrollmentViewModel();
            this.MatchingViewModel = new MatchingViewModel();

            try
            {
                if (File.Exists("Settings.json"))
                {
                    Tuple<EnrollmentViewModel, MatchingViewModel> loadedSettings = JsonConvert.DeserializeObject<Tuple<EnrollmentViewModel, MatchingViewModel>>(File.ReadAllText("Settings.json"));
                    this.EnrollmentViewModel.SetValues(loadedSettings.Item1);
                    this.MatchingViewModel.SetValues(loadedSettings.Item2);
                }
            }
            catch (Exception)
            {
            }

            if (processor.IsProcessorReady)
            {
                this.SaveSettings();
            }
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
            this.SaveSettings();
        });

        public ICommand UndoCommand => new RelayCommand<Action>(param =>
        {
            try
            {
                Tuple<EnrollmentViewModel, MatchingViewModel> previousSettings = this.Processor.GetSettings();
                this.EnrollmentViewModel.SetValues(previousSettings.Item1);
                this.MatchingViewModel.SetValues(previousSettings.Item2);
            }
            catch (Exception)
            {
                // TODO
            }
            finally
            {
                this.GetLogsFromProcessor();
            }
        });

        private void SaveSettings()
        {
            try
            {
                Tuple<EnrollmentViewModel, MatchingViewModel> settings = new Tuple<EnrollmentViewModel, MatchingViewModel>(this.EnrollmentViewModel, this.MatchingViewModel);
                this.Processor.SetSettings(settings);
                File.WriteAllText("Settings.json", JsonConvert.SerializeObject(settings));
            }
            catch (Exception)
            {
                // TODO
            }
            finally
            {
                this.GetLogsFromProcessor();
            }
        }
    }
}
