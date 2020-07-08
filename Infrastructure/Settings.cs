using Microsoft.Extensions.Configuration;
using Models;
using System;

namespace Infrastructure
{
    public class Settings : ISettings
    {
        private readonly SettingsModel _settings;

        public Settings(IConfiguration configuration)
        {
            this._settings = configuration.GetSection("ApplicationSettings").Get<SettingsModel>();
        }

        public string GetMongoDB()
        {
            return _settings.MongoDB.ConnectionString;
        }

        public string GetDatabaseName()
        {
            return _settings.MongoDB.DatabaseName;
        }

        public string GetSMTPServer()
        {
            return _settings.SMTP.server;
        }
    }
}
