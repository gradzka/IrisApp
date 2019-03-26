namespace IrisApp
{
    using System.Collections.ObjectModel;
    using System.Windows;
    using IrisApp.Models.Home;
    using IrisApp.Models.IrisProcessor;
    using IrisApp.ViewModels;
    using IrisApp.Views;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            MainWindow app = new MainWindow();
            MainWindowViewModel context = new MainWindowViewModel(new VeriEyeProcessorModel(), new ObservableCollection<LogModel>());
            app.DataContext = context;
            app.Show();
        }
    }
}
