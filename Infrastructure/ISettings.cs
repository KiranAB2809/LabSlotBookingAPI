namespace Infrastructure
{
    public interface ISettings
    {
        string GetMongoDB();
        string GetDatabaseName();
        string GetSMTPServer();
    }
}