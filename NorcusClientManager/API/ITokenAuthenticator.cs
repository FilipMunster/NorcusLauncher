namespace NorcusClientManager.API
{
    public interface ITokenAuthenticator
    {
        bool IsTokenValid(string token);
    }
}