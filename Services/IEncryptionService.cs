public interface IEncryptionService
{
    string Encrypt(string plainText, string? userKey = null);
    string Decrypt(string cipherText, string? userKey = null);
}
