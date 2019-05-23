using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;

namespace Common.Data.Security
{
    /// <summary>
    /// Helpers for <see cref="SecureString"/>
    /// </summary>
    public static class SecureStringHelper
    {

        /// <summary>
        /// Usecures a secure string to plane text
        /// </summary>
        /// <param name="secureString">secure string</param>
        /// <returns></returns>
        public static string Unsecure(this SecureString secureString)
        {
            if (secureString == null)
                return "";

            // get pointer for an unsecure string in memory
            var unmanagedString = IntPtr.Zero;

            try
            {
                //Unsecure string
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(secureString);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                //Clean up any memory allocations
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }

        /// <summary>
        /// Gets hash string from secured password and salt string
        /// </summary>
        /// <param name="secureString"></param>
        /// <param name="salt">Random constant to make decrypting more dificult</param>
        /// <returns></returns>
        public static string GetHash(this SecureString secureString, string salt = "I(<cOol>" )
        {
            // usually i will use User Name as a value and some ford for a salt
            // get salted byte[] buffer, containing value, password and some (constant) salt
            byte[] buffer;
            using (MemoryStream stream = new MemoryStream())
            using (StreamWriter writer = new StreamWriter(stream))
            {
                writer.Write(salt);
                writer.Write(secureString.Unsecure());
                writer.Flush();

                buffer = stream.ToArray();
            }

            // create a hash
            SHA1 sha1 = SHA1.Create();

            return Encoding.Default.GetString(sha1.ComputeHash(buffer));
        }
        
    }
}
