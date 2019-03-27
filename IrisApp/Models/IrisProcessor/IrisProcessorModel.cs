namespace IrisApp.Models.IrisProcessor
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using IrisApp.Models.Home;
    using IrisApp.ViewModels.Settings;

    public abstract class IrisProcessorModel
    {
        private bool isProcessorReady = false;
        private readonly string pathToImages = "IrisDB";
        private List<LogModel> resultLogs = null;

        public IrisProcessorModel()
        {
            this.resultLogs = new List<LogModel>();
        }

        public bool IsProcessorReady
        {
            get => this.isProcessorReady;
            protected set
            {
                if (this.isProcessorReady == value)
                {
                    return;
                }

                this.isProcessorReady = value;
            }
        }

        public string PathToImages => this.pathToImages;

        public abstract Task<List<SubjectModel>> GetAllSubjectsAsync();

        public abstract List<int> GetAllSubjectIDs();

        public abstract List<SourceModel> GetDevices();

        public List<LogModel> GetLogs()
        {
            List<LogModel> logs = new List<LogModel>(this.resultLogs);
            this.resultLogs.Clear();
            return logs;
        }

        public abstract object GetPreviewControl(bool beforeCapturingFromDevice = false);

        public abstract Tuple<EnrollmentViewModel, MatchingViewModel> GetSettings();

        public abstract Task IdentifyAsync();

        public abstract Task<bool> LoadFromImageAsync(string pathToImageFile, char eye);

        public abstract Task<bool> LoadFromScannerAsync(object device, char eye);

        public abstract bool RemoveSubject(int subjectID);

        public abstract void SaveImage(string pathToImageFile);

        public abstract Task<SampleModel> SaveToDBAsync(int subjectID);

        public abstract void SetSettings(Tuple<EnrollmentViewModel, MatchingViewModel> settings);

        protected void AddLog(bool isSuccess, string description, string name)
        {
            this.resultLogs.Add(new LogModel() { Code = isSuccess ? 'S' : 'E', Description = description, Name = name });
        }

        protected abstract bool ConnectToDB();

        protected abstract Task<object> GetSubjectAsync(int subjectID);
    }
}
