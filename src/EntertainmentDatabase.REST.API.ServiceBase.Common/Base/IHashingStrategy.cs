namespace EntertainmentDatabase.REST.API.ServiceBase.Common.Base
{
    public interface IHashingStrategy
    {
        byte[] GenerateSalt();

        string CreateSaltedHash(string unhashed, byte[] salt);
    }
}
