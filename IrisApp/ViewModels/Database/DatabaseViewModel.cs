namespace IrisApp.ViewModels.Database
{
    using System.Collections.ObjectModel;
    using IrisApp.ViewModels.Home;

    public class DatabaseViewModel : BaseViewModel, IPageViewModel
    {
        private ObservableCollection<SubjectViewModel> subjects;

        public DatabaseViewModel()
        {
            this.Subjects = new ObservableCollection<SubjectViewModel>();
            this.Subjects.Add(new SubjectViewModel() { SubjectID = 1, Path = "xxx", ProbesNo = 0 });
            this.Subjects.Add(new SubjectViewModel() { SubjectID = 2, Path = "yyy", ProbesNo = 1 });
            this.Subjects.Add(new SubjectViewModel() { SubjectID = 3, Path = "zzz", ProbesNo = 2 });
        }

        public ObservableCollection<SubjectViewModel> Subjects
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
