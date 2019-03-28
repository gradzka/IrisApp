// https://rachel53461.wordpress.com/2011/12/18/navigation-with-mvvm-2/

namespace IrisApp.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;
    using IrisApp.Models.Home;
    using IrisApp.Models.IrisProcessor;
    using IrisApp.Utils;
    using IrisApp.ViewModels.Database;
    using IrisApp.ViewModels.Home;
    using IrisApp.ViewModels.Settings;

    public class MainWindowViewModel : BaseViewModel
    {
        private IPageViewModel currentPageViewModel;
        private WindowState currentWindowState;
        private List<IPageViewModel> pageViewModels;
        private int selectedPageIndex = 0;

        public MainWindowViewModel(IrisProcessorModel processor, ObservableCollection<LogModel> logs)
            : base(processor, logs)
        {
            // Add available pages and set page
            this.PageViewModels.Add(new HomeViewModel(processor, logs));
            this.PageViewModels.Add(new DatabaseViewModel(processor, logs));
            this.PageViewModels.Add(new SettingsViewModel(processor, logs));
            this.PageViewModels.Add(new AboutViewModel(processor, logs));
            this.CurrentPageViewModel = this.PageViewModels[0];
        }

        public IPageViewModel CurrentPageViewModel
        {
            get => this.currentPageViewModel;

            set
            {
                if (this.currentPageViewModel == value)
                {
                    return;
                }

                this.currentPageViewModel = value;
                this.OnPropertyChanged(nameof(this.CurrentPageViewModel));
            }
        }

        public WindowState CurrentWindowState
        {
            get => this.currentWindowState;

            set
            {
                if (this.currentWindowState == value)
                {
                    return;
                }

                this.currentWindowState = value;
                this.OnPropertyChanged(nameof(this.CurrentWindowState));
            }
        }

        public List<IPageViewModel> PageViewModels
        {
            get
            {
                if (this.pageViewModels is null)
                {
                    this.pageViewModels = new List<IPageViewModel>();
                }

                return this.pageViewModels;
            }
        }

        public int SelectedPageIndex
        {
            get => this.selectedPageIndex;

            set
            {
                if (this.selectedPageIndex == value)
                {
                    return;
                }

                this.selectedPageIndex = value;
                this.OnGoToPage(null);
                this.OnPropertyChanged(nameof(this.SelectedPageIndex));
            }
        }

        public ICommand MaximizeOrRestoreAppCommand => new RelayCommand<Action>(param =>
        {
            this.CurrentWindowState = this.CurrentWindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        });

        public ICommand MinimizeAppCommand => new RelayCommand<Action>(param => { this.CurrentWindowState = WindowState.Minimized; });

        public ICommand RestoreAppCommand => new RelayCommand<Action>(param => { this.CurrentWindowState = WindowState.Normal; });

        private void ChangeViewModel(IPageViewModel viewModel)
        {
            if (!this.PageViewModels.Contains(viewModel))
            {
                this.PageViewModels.Add(viewModel);
            }

            this.CurrentPageViewModel = this.PageViewModels.FirstOrDefault(vm => vm == viewModel);
        }

        private void OnGoToPage(object obj)
        {
            this.ChangeViewModel(this.PageViewModels[this.SelectedPageIndex]);
        }
    }
}
