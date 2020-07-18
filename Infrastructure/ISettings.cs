namespace Infrastructure
{
    public interface ISettings
    {
        string GetMongoDB();
        string GetDatabaseName();
        string GetSMTPServer();
        string GetSMTPMailId();
        string GetSMTPPassword();

    }
}