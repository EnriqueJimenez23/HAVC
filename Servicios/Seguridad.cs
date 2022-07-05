using System;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace CapaServicios.Servicios
{
    public class Seguridad
    {
        #region Members

        private static readonly string PassPhrase; // Passphrase from which a pseudo-random password will be derived. The derived password will be used to generate the encryption key. Passphrase can be any string. In this example we assume that this passphrase is an ASCII string.
        private static readonly string SaltValue; // Salt value used along with passphrase to generate password. Salt can be any string. In this example we assume that salt is an ASCII string.
        private static readonly int PasswordIterations; // Number of iterations used to generate password. One or two iterations should be enough.
        private static readonly string InitVector; // Initialization vector (or IV). This value is required to encrypt the first block of plaintext data. For RijndaelManaged class IV must be exactly 16 ASCII characters long.
        private static readonly int KeySize; // Size of encryption key in bits. Allowed values are: 128, 192, and 256. Longer keys are more secure than shorter keys.

        #endregion

        #region Methods

        /// <summary>
        /// Encripta una cadena en texto plano usando AES
        /// </summary>
        /// <param name="textoPlano">Cadena en texto plano</param>
        /// <returns>Cadena encriptada</returns>
        public static string Encriptar(string textoPlano)
        {
            try
            {
                // Convert strings into byte arrays. Let us assume that strings only contain ASCII codes. If strings include Unicode characters, use Unicode, UTF7, or UTF8 encoding.
                byte[] initVectorBytes = Encoding.ASCII.GetBytes(InitVector);
                byte[] saltValueBytes = Encoding.ASCII.GetBytes(SaltValue);

                // Convert our plaintext into a byte array. Let us assume that plaintext contains UTF8-encoded characters.
                byte[] plainTextBytes = Encoding.UTF8.GetBytes(textoPlano);

                // First, we must create a password, from which the key will be derived. This password will be generated from the specified passphrase and salt value. The password will be created using the specified hash algorithm. Password creation can be done in several iterations.
                //PasswordDeriveBytes password = new PasswordDeriveBytes(PassPhrase, saltValueBytes, HashAlgorithm, PasswordIterations);
                Rfc2898DeriveBytes password = new Rfc2898DeriveBytes(PassPhrase, saltValueBytes, PasswordIterations);
            
                // Use the password to generate pseudo-random bytes for the encryption key. Specify the size of the key in bytes (instead of bits).
                byte[] keyBytes = password.GetBytes(KeySize / 8);

                // Create uninitialized Rijndael encryption object. It is reasonable to set encryption mode to Cipher Block Chaining (CBC). Use default options for other symmetric key parameters.
                RijndaelManaged symmetricKey = new RijndaelManaged { Mode = CipherMode.CBC };

                // Generate encryptor from the existing key bytes and initialization vector. Key size will be defined based on the number of the key bytes.
                ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);

                // Define memory stream which will be used to hold encrypted data.
                MemoryStream memoryStream = new MemoryStream();

                // Define cryptographic stream (always use Write mode for encryption).
                CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);

                // Start encrypting.
                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);

                // Finish encrypting.
                cryptoStream.FlushFinalBlock();

                // Convert our encrypted data from a memory stream into a byte array.
                byte[] cipherTextBytes = memoryStream.ToArray();

                // Close both streams.
                memoryStream.Close();
                cryptoStream.Close();

                // Convert encrypted data into a base64-encoded string.
                string cipherText = Convert.ToBase64String(cipherTextBytes);

                // Return encrypted string.
                return HttpServerUtility.UrlTokenEncode(Encoding.UTF8.GetBytes(cipherText));
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Desencripta una cadena encriptada usando AES
        /// </summary>
        /// <param name="textoEncriptado">Cadena encriptada</param>
        /// <returns>Cadena en texto plano </returns>
        public static string Desencriptar(string textoEncriptado)
        {
            try
            {
                var temp = HttpServerUtility.UrlTokenDecode(textoEncriptado);
                if (temp == null)
                    return null;

                // Convert strings defining encryption key characteristics into byte arrays. Let us assume that strings only contain ASCII codes. If strings include Unicode characters, use Unicode, UTF7, or UTF8 encoding.
                byte[] initVectorBytes = Encoding.ASCII.GetBytes(InitVector);
                byte[] saltValueBytes = Encoding.ASCII.GetBytes(SaltValue);

                // Convert our ciphertext into a byte array.
                byte[] cipherTextBytes = Convert.FromBase64String(Encoding.UTF8.GetString(temp));

                // First, we must create a password, from which the key will be derived. This password will be generated from the specified passphrase and salt value. The password will be created using the specified hash algorithm. Password creation can be done in several iterations.
                //PasswordDeriveBytes password = new PasswordDeriveBytes(PassPhrase, saltValueBytes, HashAlgorithm, PasswordIterations);
                Rfc2898DeriveBytes password = new Rfc2898DeriveBytes(PassPhrase, saltValueBytes, PasswordIterations);

                // Use the password to generate pseudo-random bytes for the encryption key. Specify the size of the key in bytes (instead of bits).
                byte[] keyBytes = password.GetBytes(KeySize / 8);

                // Create uninitialized Rijndael encryption object.
                RijndaelManaged symmetricKey = new RijndaelManaged { Mode = CipherMode.CBC }; // It is reasonable to set encryption mode to Cipher Block Chaining (CBC). Use default options for other symmetric key parameters.
                
                // Generate decryptor from the existing key bytes and initialization vector. Key size will be defined based on the number of the key  bytes.
                ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);

                // Define memory stream which will be used to hold encrypted data.
                MemoryStream memoryStream = new MemoryStream(cipherTextBytes);

                // Define cryptographic stream (always use Read mode for encryption).
                CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);

                // Since at this point we don't know what the size of decrypted data will be, allocate the buffer long enough to hold ciphertext; plaintext is never longer than ciphertext.
                byte[] plainTextBytes = new byte[cipherTextBytes.Length];

                // Start decrypting.
                int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);

                // Close both streams.
                memoryStream.Close();
                cryptoStream.Close();

                // Convert decrypted data into a string. Let us assume that the original plaintext string was UTF8-encoded.
                string plainText = Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);

                // Return decrypted string.   
                return plainText;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Genera un Token de seguridad para un registro
        /// </summary>
        /// <param name="nombreEntidad">Nombre de la entidad o tabla donde se encuentra el registro</param>
        /// <param name="registroId">Id del registro</param>
        /// <returns>Token de seguridad</returns>
        public static string CrearToken(string nombreEntidad, int registroId)
        {
            return CrearToken(nombreEntidad, registroId.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Genera un Token de seguridad para un registro
        /// </summary>
        /// <param name="nombreEntidad">Nombre de la entidad o tabla donde se encuentra el registro</param>
        /// <param name="registroId">Id del registro</param>
        /// <returns>Token de seguridad</returns>
        public static string CrearToken(string nombreEntidad, string registroId)
        {
            return Encriptar(string.Format("{0}|{1}", nombreEntidad, registroId));
        }

        #endregion

        #region Constructor

        static Seguridad()
        {
            PassPhrase = "Pas5pr@se"; // can be any string
            SaltValue = "s@1tValue"; // can be any string
            //HashAlgorithm = "MD5";
            PasswordIterations = 2; // can be any number
            InitVector = "@1B2c3D4e5F6g7H8"; // must be 16 bytes
            KeySize = 256; // can be 192 or 128
        }

        #endregion
    }
}
