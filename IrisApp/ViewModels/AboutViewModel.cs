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
                if (((string)param).Equals("0", StringComparison.Ordinal))
                {
                    System.Diagnostics.Process.Start("https://github.com/gradzka/IrisApp");
                }
                else if (((string)param).Equals("1", StringComparison.Ordinal))
                {
                    System.Diagnostics.Process.Start("https://github.com/gradzka");
                }
                else if (((string)param).Equals("2", StringComparison.Ordinal))
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
