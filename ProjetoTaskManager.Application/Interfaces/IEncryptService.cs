namespace ProjetoTaskManager.Application.Interfaces
{
    public interface IEncryptService
    {
        string Hash(string value);
        bool Verify(string value, string hash);
    }
}