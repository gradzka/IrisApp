namespace IrisApp.ViewModels.Database
{
    using System;
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using IrisApp.Models.Home;
    using IrisApp.Utils;

    public class DatabaseViewModel : BaseViewModel, IPageViewModel
    {
        private ObservableCollection<SubjectModel> subjects;

        public DatabaseViewModel()
        {
            this.Subjects = new ObservableCollection<SubjectModel>()
            {
                new SubjectModel() { SubjectID = 1, Path = "C:\\Users\\User\\Documents\\Samples\\1", SamplesCount = 0 },
                new SubjectModel() { SubjectID = 2, Path = "C:\\Users\\User\\Documents\\Samples\\2", SamplesCount = 1 },
                new SubjectModel() { SubjectID = 3, Path = "C:\\Users\\User\\Documents\\Samples\\3", SamplesCount = 2 },
                new SubjectModel() { SubjectID = 4, Path = "C:\\Users\\User\\Documents\\Samples\\4", SamplesCount = 4 },
                new SubjectModel() { SubjectID = 5, Path = "C:\\Users\\User\\Documents\\Samples\\5", SamplesCount = 1 },
                new SubjectModel() { SubjectID = 6, Path = "C:\\Users\\User\\Documents\\Samples\\6", SamplesCount = 2 },
                new SubjectModel() { SubjectID = 7, Path = "C:\\Users\\User\\Documents\\Samples\\7", SamplesCount = 0 },
                new SubjectModel() { SubjectID = 8, Path = "C:\\Users\\User\\Documents\\Samples\\8", SamplesCount = 4 },
                new SubjectModel() { SubjectID = 9, Path = "C:\\Users\\User\\Documents\\Samples\\9", SamplesCount = 4 },
                new SubjectModel() { SubjectID = 10, Path = "C:\\Users\\User\\Documents\\Samples\\10", SamplesCount = 0 },
                new SubjectModel() { SubjectID = 11, Path = "C:\\Users\\User\\Documents\\Samples\\11", SamplesCount = 1 },
                new SubjectModel() { SubjectID = 12, Path = "C:\\Users\\User\\Documents\\Samples\\12", SamplesCount = 2 }
            };
        }

        public ObservableCollection<SubjectModel> Subjects
        {
            get => this.subjects;
            set
            {
                if (this.subjects == value)
                {
                    return;
                }

                this.subjects = value;
            }
        }

        public ICommand DeleteAllCommand => new RelayCommand<Action>(param =>
        {
            try
            {
                this.Subjects.Clear();

                // TODO -> delete from DB, srsly?
            }
            catch (Exception)
            {
                // TODO
            }
        });

        public ICommand DeleteTupleCommand => new RelayCommand<SubjectModel>(param =>
        {
            try
            {
                this.Subjects.Remove(param);

                // TODO, from DB too
            }
            catch (Exception)
            {
                // TODO
            }
        });

        public ICommand GoToFolderCommand => new RelayCommand<string>(param =>
        {
            // TODO
            // if path exists open dialog
        });

        public ICommand RefreshCommand => new RelayCommand<Action>(param =>
        {
            // TODO
        });
    }
}
