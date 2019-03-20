namespace IrisApp.ViewModels
{
    using System;
    using System.Windows.Input;
    using IrisApp.Utils;

    public class AboutViewModel : BaseViewModel, IPageViewModel
    {
        public ICommand OpenLinkCommand => new RelayCommand<object>(param =>
        {
            try
            {
                if ((string)param == "0")
                {
                    System.Diagnostics.Process.Start("https://github.com/gradzka/IrisApp");
                }
                else if ((string)param == "1")
                {
                    System.Diagnostics.Process.Start("https://github.com/gradzka");
                }
                else if ((string)param == "2")
                {
                    System.Diagnostics.Process.Start("https://github.com/kazimierczak-robert");
                }
            }
            catch (Exception)
            {
                // TODO
            }
        });
    }
}
