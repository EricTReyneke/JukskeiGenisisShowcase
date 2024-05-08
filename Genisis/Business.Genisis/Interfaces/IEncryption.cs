namespace Business.Genisis.Interfaces
{
    public interface IEncryption
    {
        /// <summary>
        /// Encrypts the plainText one way. Cant be Decrypted.
        /// </summary>
        /// <param name="plainText">Plain text to be decrypted.</param>
        /// <returns>Encrypted string.</returns>
        string OneWayHashEncryption(string plainText);

        /// <summary>
        /// Encrypts a given plaintext string using a specified key.
        /// </summary>
        /// <param name="plainText">The text to be encrypted.</param>
        /// <returns>The encrypted text represented as a Base64 string.</returns>
        string Encrypt(string plainText);

        /// <summary>
        /// Decrypts a given ciphertext string using a specified key.
        /// </summary>
        /// <param name="cipherText">The encrypted text to be decrypted, in Base64 format.</param>
        /// <returns>The decrypted text.</returns>
        string Decrypt(string cipherText);
    }
}