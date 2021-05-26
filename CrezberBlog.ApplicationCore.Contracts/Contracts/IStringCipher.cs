namespace CrezberBlog.ApplicationCore.Contracts
{
    public interface IStringCipher
    {
        string Encrypt(string text, string keyString);

        string DecryptString(string cipherText, string keyString);

    }
}
