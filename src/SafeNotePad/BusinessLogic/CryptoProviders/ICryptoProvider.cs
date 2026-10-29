namespace SafeNotePad.BusinessLogic.CryptoProviders
{
    interface ICryptoProvider
    {
        string Decrypt(string encryptedPassword);

        string Encrypt(string password);
    }
}
