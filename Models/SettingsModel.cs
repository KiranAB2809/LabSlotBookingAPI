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
        public string Server { get; set; }
        public string FromMail { get; set; }
        public string Password { get; set; }

    }

    public class MongoSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
