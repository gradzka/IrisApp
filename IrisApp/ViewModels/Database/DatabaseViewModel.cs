namespace IrisApp.ViewModels.Database
{
    using System.Collections.ObjectModel;
    using IrisApp.Models.Home;

    public class DatabaseViewModel : BaseViewModel, IPageViewModel
    {
        private ObservableCollection<SubjectModel> subjects;

        public DatabaseViewModel()
        {
            this.Subjects = new ObservableCollection<SubjectModel>()
            {
                new SubjectModel() { SubjectID = 1, Path = "xxx", ProbesNo = 0 },
                new SubjectModel() { SubjectID = 2, Path = "yyy", ProbesNo = 1 },
                new SubjectModel() { SubjectID = 3, Path = "zzz", ProbesNo = 2 }
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
    }
}
