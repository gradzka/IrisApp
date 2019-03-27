namespace IrisApp.Models.IrisProcessor
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Forms.Integration;
    using IrisApp.Models.Home;
    using IrisApp.ViewModels.Settings;
    using Neurotec.Biometrics;
    using Neurotec.Biometrics.Client;
    using Neurotec.Biometrics.Gui;
    using Neurotec.Devices;
    using Neurotec.Licensing;

    public class VeriEyeProcessorModel : IrisProcessorModel
    {
        private NBiometricClient biometricClient;
        private NIris iris;
        private NSubject subject;

        public VeriEyeProcessorModel()
        {
            if (this.ObtainLicenses())
            {
                this.biometricClient = new NBiometricClient
                {
                    UseDeviceManager = true,
                    CustomDataSchema = NBiographicDataSchema.Parse("(Path string)")
                };
                this.IsProcessorReady = this.ConnectToDB();
            }
        }

        public override async Task<List<SubjectModel>> GetAllSubjectsAsync()
        {
            try
            {
                if (!this.IsProcessorReady)
                {
                    this.AddLog(false, "No license obtained", "License");
                    return null;
                }

                List<SubjectModel> subjects = new List<SubjectModel>();
                foreach (int subjectID in this.GetAllSubjectIDs())
                {
                    NSubject subject = (NSubject)await this.GetSubjectAsync(subjectID);
                    subjects.Add(new SubjectModel()
                    {
                        Path = subject.GetProperty("Path").ToString(),
                        SamplesCount = subject.Irises.Count,
                        SubjectID = subjectID
                    });
                }

                return subjects;
            }
            catch (Exception)
            {
                this.AddLog(false, "Unexcepted error during obtaining subjectIs", "Database");
                return null;
            }
        }

        public override List<int> GetAllSubjectIDs()
        {
            try
            {
                if (!this.IsProcessorReady)
                {
                    this.AddLog(false, "No license obtained", "License");
                    return null;
                }

                string[] status = this.biometricClient.ListIds();

                return Array.ConvertAll(status, int.Parse).ToList();
            }
            catch (Exception)
            {
                this.AddLog(false, "Unexcepted error during obtaining subjectIDs", "Database");
                return null;
            }
        }

        public override List<SourceModel> GetDevices()
        {
            try
            {
                if (!this.IsProcessorReady)
                {
                    this.AddLog(false, "No license obtained", "License");
                    return null;
                }

                this.biometricClient.DeviceManager.DeviceTypes = NDeviceType.IrisScanner;
                this.biometricClient.DeviceManager.Initialize();

                List<SourceModel> devices = new List<SourceModel>();
                foreach (NDevice device in this.biometricClient.DeviceManager.Devices)
                {
                    devices.Add(new SourceModel() { Name = device.DisplayName, Device = device });
                }

                this.AddLog(true, devices.Count + " devices found", "Source");
                return devices;
            }
            catch (Exception)
            {
                this.AddLog(false, "Unexcepted error during getting devices", "License");
                return null;
            }
        }

        public override object GetPreviewControl(bool beforeCapturingFromDevice = false)
        {
            try
            {
                if (!this.IsProcessorReady)
                {
                    this.AddLog(false, "No license obtained", "License");
                    return false;
                }

                if (beforeCapturingFromDevice)
                {
                    this.iris = new NIris();
                }

                return new WindowsFormsHost() { Child = new NIrisView() { Iris = this.iris } };
            }
            catch (Exception)
            {
                this.AddLog(false, "Unexcepted error during getting preview control", "Preview");
                return null;
            }
        }

        public override Tuple<EnrollmentViewModel, MatchingViewModel> GetSettings()
        {
            try
            {
                if (!this.IsProcessorReady)
                {
                    this.AddLog(false, "No license obtained", "License");
                    return null;
                }

                EnrollmentViewModel enrollmentViewModel = new EnrollmentViewModel
                {
                    InnerBoundaryRadiusFrom = Convert.ToByte(this.biometricClient.GetProperty("Irises.InnerBoundaryFrom")),
                    InnerBoundaryRadiusTo = Convert.ToByte(this.biometricClient.GetProperty("Irises.InnerBoundaryTo")),
                    OuterBoundaryRadiusFrom = Convert.ToByte(this.biometricClient.GetProperty("Irises.OuterBoundaryFrom")),
                    OuterBoundaryRadiusTo = Convert.ToByte(this.biometricClient.GetProperty("Irises.OuterBoundaryTo")),
                    QualityThreshold = this.biometricClient.IrisesQualityThreshold
                };

                MatchingViewModel matchingViewModel = new MatchingViewModel();
                matchingViewModel.SelectedFAR = matchingViewModel.FAR.FirstOrDefault(x => x.Key == this.MatchingThresholdToFAR(this.biometricClient.MatchingThreshold) * 100);
                matchingViewModel.MaximalRotation = (int)this.biometricClient.IrisesMaximalRotation;
                matchingViewModel.SelectedMatchingSpeed = matchingViewModel.MatchingSpeed.FirstOrDefault(x => x.Equals(this.biometricClient.IrisesMatchingSpeed.ToString(), StringComparison.Ordinal));
                matchingViewModel.MaximalResultCount = this.biometricClient.MatchingMaximalResultCount;
                matchingViewModel.IsFirstReadOnlyChecked = this.biometricClient.MatchingFirstResultOnly;
                this.AddLog(true, "Getting settings successful", "Settings");

                return new Tuple<EnrollmentViewModel, MatchingViewModel>(enrollmentViewModel, matchingViewModel);
            }
            catch (Exception)
            {
                this.AddLog(false, "Unexcepted error getting settings", "Settings");
                return null;
            }
        }

        public override async Task IdentifyAsync()
        {
            try
            {
                if (!this.IsProcessorReady)
                {
                    this.AddLog(false, "No license obtained", "License");
                    return;
                }

                if (this.subject == null)
                {
                    this.AddLog(false, "No image to identify", "Identify");
                    return;
                }

                NBiometricTask task = this.biometricClient.CreateTask(NBiometricOperations.Identify, this.subject);
                NBiometricTask performedTask = await this.biometricClient.PerformTaskAsync(task);
                if (performedTask.Status != NBiometricStatus.Ok)
                {
                    this.AddLog(false, "Identification failed (status = " + performedTask.Status + ")", "Identify");
                    return;
                }

                this.AddLog(true, "Identification successful\n" + this.subject.MatchingResults[0].Id + " " + this.subject.MatchingResults[0].Score.ToString(), "Identify");
                return;
            }
            catch (Exception)
            {
                this.AddLog(false, "Unexcepted error during identification", "Identify");
                return;
            }
        }

        public override async Task<bool> LoadFromImageAsync(string pathToImageFile, char eye)
        {
            try
            {
                if (!this.IsProcessorReady)
                {
                    this.AddLog(false, "No license obtained", "License");
                    return false;
                }

                if (!File.Exists("Irises.ndf"))
                {
                    this.AddLog(false, "Irises.ndf not found", "Irises.ndf");
                    return false;
                }

                this.subject = new NSubject();

                this.iris = new NIris()
                {
                    FileName = pathToImageFile
                };

                if (eye == 'L')
                {
                    this.iris.Position = NEPosition.Left;
                }
                else if (eye == 'R')
                {
                    this.iris.Position = NEPosition.Right;
                }
                else if (eye == 'U')
                {
                    this.iris.Position = NEPosition.Unknown;
                }
                else
                {
                    this.AddLog(false, "Incorrect eye parameter received", "Capture/Extraction");
                    return false;
                }

                this.subject.Irises.Add(this.iris);
                this.biometricClient.IrisesTemplateSize = NTemplateSize.Large;

                NBiometricTask task = this.biometricClient.CreateTask(NBiometricOperations.CreateTemplate, this.subject);
                NBiometricTask performedTask = await this.biometricClient.PerformTaskAsync(task);
                if (performedTask.Status != NBiometricStatus.Ok)
                {
                    this.AddLog(false, "Extraction failed (status = " + performedTask.Status + ")", "Extraction");
                    return false;
                }

                this.AddLog(true, "Extraction successful", "Extraction");
                var a = this.subject.GetTemplate();
                return true;
            }
            catch (Exception)
            {
                this.AddLog(false, "Unexcepted error during loading from image", "Extraction");
                return false;
            }
        }

        public override async Task<bool> LoadFromScannerAsync(object device, char eye)
        {
            try
            {
                if (!this.IsProcessorReady)
                {
                    this.AddLog(false, "No license obtained", "License");
                    return false;
                }

                if (!File.Exists("Irises.ndf"))
                {
                    this.AddLog(false, "Irises.ndf not found", "Irises.ndf");
                    return false;
                }

                this.subject = new NSubject();
                if (this.iris == null)
                {
                    this.AddLog(false, "Preview control not loaded", "Preview");
                    return false;
                }

                this.biometricClient.IrisScanner = (NIrisScanner)device;

                if (eye == 'L')
                {
                    this.iris.Position = NEPosition.Left;
                }
                else if (eye == 'R')
                {
                    this.iris.Position = NEPosition.Right;
                }
                else if (eye == 'U')
                {
                    this.iris.Position = NEPosition.Unknown;
                }
                else
                {
                    this.AddLog(false, "Incorrect eye parameter received", "Capture/Extraction");
                    return false;
                }

                this.subject.Irises.Add(this.iris);

                this.biometricClient.IrisesTemplateSize = NTemplateSize.Large;
                NBiometricTask task = this.biometricClient.CreateTask(NBiometricOperations.Capture | NBiometricOperations.CreateTemplate, this.subject);
                NBiometricTask performedTask = await this.biometricClient.PerformTaskAsync(task);
                if (performedTask.Status != NBiometricStatus.Ok)
                {
                    this.AddLog(false, "Capturing or extraction failed (status = " + performedTask.Status + ")", "Capture/Extraction");
                    return false;
                }

                this.AddLog(true, "Capturing and extraction successful", "Capture/Extraction");
                return true;
            }
            catch (Exception)
            {
                this.AddLog(false, "Unexcepted error during loading from scanner", "Capture/Extracion");
                return false;
            }
        }

        public override bool RemoveSubject(int subjectID)
        {
            try
            {
                if (!this.IsProcessorReady)
                {
                    this.AddLog(false, "No license obtained", "License");
                    return false;
                }

                var status = this.biometricClient.Delete(subjectID.ToString());
                if (status != NBiometricStatus.Ok)
                {
                    this.AddLog(false, "Unexcepted error removing subject", "Database");
                    return false;
                }

                this.AddLog(true, "Subject removed successfully", "Database");
                return true;
            }
            catch (Exception)
            {
                this.AddLog(false, "Unexcepted error removing subject", "Database");
                return false;
            }
        }

        public override void SaveImage(string pathToImageFile)
        {
            try
            {
                if (!this.IsProcessorReady)
                {
                    this.AddLog(false, "No license obtained", "License");
                    return;
                }

                if (this.subject == null || this.subject.Irises.Count == 0 || (this.subject.Irises.Count > 0 && this.subject.Irises[this.subject.Irises.Count - 1].Image == null))
                {
                    this.AddLog(false, "No valid image to save found", "Save image");
                    return;
                }

                this.subject.Irises[this.subject.Irises.Count - 1].Image.Save(pathToImageFile);
                this.AddLog(true, "Saving image successful", "Save image");
            }
            catch (Exception)
            {
                this.AddLog(false, "Unexcepted error during saving image", "Save image");
                return;
            }
        }

        public override async Task<SampleModel> SaveToDBAsync(int subjectID)
        {
            try
            {
                if (!this.IsProcessorReady)
                {
                    this.AddLog(false, "No license obtained", "License");
                    return null;
                }

                // new subject
                if (subjectID == 0)
                {
                    subjectID = this.CreateNextSubjectID();
                    if (subjectID == 0)
                    {
                        this.AddLog(false, "Enroll failed (cannot create subjectID)", "Save template");
                        return null;
                    }

                    this.subject.Id = subjectID.ToString();
                    this.subject.SetProperty("Path", "IrisDB\\" + subjectID.ToString());
                    NBiometricTask task = this.biometricClient.CreateTask(NBiometricOperations.Enroll, this.subject);
                    NBiometricTask performedTask = await this.biometricClient.PerformTaskAsync(task);
                    if (performedTask.Status != NBiometricStatus.Ok)
                    {
                        this.AddLog(false, "Enroll failed (status = " + performedTask.Status + ")", "Save template");
                        return null;
                    }

                    this.AddLog(true, "Enroll successful" + subjectID.ToString(), "Save template");
                    return new SampleModel() { SubjectID = subjectID, TemplateID = 1, Path = Path.Combine("IrisDB", subjectID.ToString()), ChosenEye = this.subject.Irises.Last().Position.ToString()[0] };
                }

                // existing
                else
                {
                    NSubject subj = (NSubject)await this.GetSubjectAsync(subjectID);

                    NIris newIris = this.subject.Irises[0];
                    this.subject.Irises.Remove(newIris);
                    subj.Irises.Add(newIris);
                    this.subject = subj;

                    NBiometricTask task = this.biometricClient.CreateTask(NBiometricOperations.Update, subj);
                    NBiometricTask performedTask = await this.biometricClient.PerformTaskAsync(task);
                    if (performedTask.Status != NBiometricStatus.Ok)
                    {
                        this.AddLog(false, "Enroll failed (status = " + performedTask.Status + ")", "Save template");
                        return null;
                    }

                    this.AddLog(true, "Enroll successful " + subjectID.ToString(), "Save template");
                    return new SampleModel() { SubjectID = subjectID, TemplateID = subj.Irises.Count, Path = Path.Combine("IrisDB", subjectID.ToString()), ChosenEye = subj.Irises.Last().Position.ToString()[0] };
                }
            }
            catch (Exception)
            {
                this.AddLog(false, "Unexcepted error during saving template", "Save template");
                return null;
            }
        }

        public override void SetSettings(Tuple<EnrollmentViewModel, MatchingViewModel> settings)
        {
            try
            {
                if (!this.IsProcessorReady)
                {
                    this.AddLog(false, "No license obtained", "License");
                    return;
                }

                this.biometricClient.SetProperty("Irises.InnerBoundaryFrom", settings.Item1.InnerBoundaryRadiusFrom);
                this.biometricClient.SetProperty("Irises.InnerBoundaryTo", settings.Item1.InnerBoundaryRadiusTo);
                this.biometricClient.SetProperty("Irises.OuterBoundaryFrom", settings.Item1.OuterBoundaryRadiusFrom);
                this.biometricClient.SetProperty("Irises.OuterBoundaryTo", settings.Item1.OuterBoundaryRadiusTo);
                this.biometricClient.IrisesQualityThreshold = settings.Item1.QualityThreshold;
                this.biometricClient.MatchingThreshold = this.FARToMatchingThreshold(settings.Item2.SelectedFAR.Key / 100);
                this.biometricClient.IrisesMaximalRotation = (float)settings.Item2.MaximalRotation;
                Enum.TryParse(settings.Item2.SelectedMatchingSpeed, out NMatchingSpeed myStatus);
                this.biometricClient.IrisesMatchingSpeed = myStatus;
                this.biometricClient.MatchingMaximalResultCount = settings.Item2.MaximalResultCount;
                this.biometricClient.MatchingFirstResultOnly = settings.Item2.IsFirstReadOnlyChecked;

                this.AddLog(true, "Saving settings successful", "Settings");
            }
            catch (Exception)
            {
                this.AddLog(false, "Unexcepted error during saving settings", "Settings");
                return;
            }
        }

        protected override bool ConnectToDB()
        {
            try
            {
                this.biometricClient.DatabaseConnection = null;
                this.biometricClient.RemoteConnections.Clear();
                this.biometricClient.SetDatabaseConnectionToSQLite("./IrisDB.db");

                this.AddLog(true, "Connected to database successfully", "Database");
                return true;
            }
            catch (Exception)
            {
                this.AddLog(false, "Unexcepted error during connecting to database", "Database");
                return false;
            }
        }

        protected override async Task<object> GetSubjectAsync(int subjectID)
        {
            try
            {
                NSubject subj = new NSubject { Id = subjectID.ToString() };
                NBiometricTask task = this.biometricClient.CreateTask(NBiometricOperations.Get, subj);
                NBiometricTask performedTask = await this.biometricClient.PerformTaskAsync(task);
                if (performedTask.Status != NBiometricStatus.Ok)
                {
                    return null;
                }

                return performedTask.Subjects.Count > 0 ? performedTask.Subjects[0] : null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private int CreateNextSubjectID()
        {
            try
            {
                return this.GetAllSubjectIDs().LastOrDefault() + 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        private int FARToMatchingThreshold(double far)
        {
            if (far > 1)
            {
                far = 1;
            }
            else if (far <= 0.0)
            {
                far = 1E-100;
            }

            return (int)Math.Round(-Math.Log10(far) * 12.0);
        }

        private double MatchingThresholdToFAR(int threshold)
        {
            if (threshold < 0)
            {
                threshold = 0;
            }

            return Math.Pow(10.0, -threshold / 12.0);
        }

        private bool ObtainLicenses()
        {
            string[] components = new string[] { "Biometrics.IrisExtraction", "Biometrics.IrisMatching", "Devices.IrisScanners" };

            try
            {
                foreach (string component in components)
                {
                    if (!NLicense.ObtainComponents("/local", 5000, component))
                    {
                        this.AddLog(false, "Cannot obtain \"" + component + "\" license", "License");
                        return false;
                    }
                }

                this.AddLog(true, "Licenses obtained successfully", "License");
                return true;
            }
            catch (Exception)
            {
                this.AddLog(false, "Unexcepted error during obtaining licenses", "License");
                return false;
            }
        }
    }
}
