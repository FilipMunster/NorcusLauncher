namespace NorcusClientManager.API
{
    public interface ITokenAuthenticator
    {
        bool Authenticate(string token);
    }
}