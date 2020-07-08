using System;

namespace Models
{
    public class SettingsModel
    {
        public MongoSettings MongoDB { get; set; }
        public SMTPSettings SMTP { get; set; }
    }

    public class SMTPSettings
    {
        public string server { get; set; }
    }

    public class MongoSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
