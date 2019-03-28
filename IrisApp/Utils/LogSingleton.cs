namespace IrisApp.Utils
{
    using IrisApp.Models.Home;

    public sealed class LogSingleton
    {
        private static LogSingleton instance;

        private LogSingleton()
        {
        }

        public static LogSingleton Instance
        {
            get
            {
                if (instance is null)
                {
                    instance = new LogSingleton();
                }

                return instance;
            }
        }

        public LogModel CaptureExtractionFailed => new LogModel { Code = 'E', Description = "Capture/Extraction failed", Name = "Capture/Extraction" };

        public LogModel CaptureExtractionResult => new LogModel { Code = 'S', Description = "Capture & extraction done", Name = "Capture/Extraction" };

        public LogModel DatabaseConnectionDone => new LogModel { Code = 'S', Description = "Connected to database", Name = "Database" };

        public LogModel DatabaseConnectionError => new LogModel { Code = 'E', Description = "Not connected to database", Name = "Database" };

        public LogModel DirectoryNotFound => new LogModel { Code = 'E', Description = "Directory not found", Name = "Database" };

        public LogModel DeleteSubjectError => new LogModel { Code = 'E', Description = "Subject deleted", Name = "Database" };

        public LogModel DeleteSubjectDone => new LogModel { Code = 'S', Description = "Subject not deleted", Name = "Database" };

        public LogModel DevicesUnavailable => new LogModel { Code = 'E', Description = "Devices unavailable", Name = "Source" };

        public LogModel ExtractionFailed => new LogModel { Code = 'E', Description = "Extraction failed", Name = "Extraction" };

        public LogModel ExtractionResult => new LogModel { Code = 'S', Description = "Extraction done", Name = "Extraction" };

        public LogModel FoundDevice => new LogModel { Code = 'S', Description = "device found", Name = "Source" };

        public LogModel FoundDevices => new LogModel { Code = 'S', Description = "devices found", Name = "Source" };

        public LogModel IdentificationFailed => new LogModel { Code = 'E', Description = "Identification failed", Name = "Identify" };

        public LogModel IdentificationResult => new LogModel { Code = 'S', Description = "Identification result ", Name = "Identify" };

        public LogModel IrisesNDF => new LogModel { Code = 'E', Description = "Irises.ndf not found", Name = "Irises.ndf" };

        public LogModel LicensesUnavailable => new LogModel { Code = 'E', Description = "Licenses unavailable", Name = "License" };

        public LogModel NoSourceSelected => new LogModel { Code = 'E', Description = "No source selected", Name = "Source" };

        public LogModel PreviewUnavailable => new LogModel { Code = 'E', Description = "Preview unavailable", Name = "Preview" };

        public LogModel SaveImageError => new LogModel { Code = 'E', Description = "No image to save", Name = "Save image" };

        public LogModel SaveImageDone => new LogModel { Code = 'S', Description = "Image saved", Name = "Save image" };

        public LogModel SaveTemplateError => new LogModel { Code = 'E', Description = "Enroll failed", Name = "Save template" };

        public LogModel SaveTemplateDone => new LogModel { Code = 'S', Description = "Enroll done", Name = "Save template" };

        public LogModel SettingsRestored => new LogModel { Code = 'E', Description = "Restored to the default settings", Name = "Settings" };

        public LogModel SettingsSaveDone => new LogModel { Code = 'S', Description = "Settings saved", Name = "Settings" };

        public LogModel SettingsSaveError => new LogModel { Code = 'E', Description = "Settings not saved", Name = "Settings" };

        public LogModel SubjectsUnavailable => new LogModel { Code = 'E', Description = "Subjects unavailable", Name = "Database" };

        public LogModel SubjectIDsUnavailable => new LogModel { Code = 'E', Description = "Subject IDs unavailable", Name = "Database" };

        public LogModel UseSelectedSource => new LogModel { Code = 'I', Description = "Use selected source", Name = "Identify" };
    }
}
