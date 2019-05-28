using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;

namespace CommonLibs.Data
{

    /// <summary>
    /// Provide methods which hepl convert data
    /// </summary>
    public class DataConverter
    {

        /// <summary>
        /// Gets file path and returns computed checksum of it
        /// </summary>
        /// <param name="filePath">full path to a file</param>
        /// <returns>hash string</returns>
        public static string CalculateChecksum(string filePath)
        {
            if (!System.IO.File.Exists(filePath))
                throw new ArgumentException("File doesn't exist");

            using (var md5 = MD5.Create())
            {
                using (var stream = System.IO.File.OpenRead(filePath))
                {
                    return Encoding.Default.GetString(md5.ComputeHash(stream));
                }
            }
        }


        /// <summary>
        /// Gets file content and returns computed checksum of it
        /// </summary>
        /// <param name="content">Content of a file</param>
        /// <returns>hash string</returns>
        public static string CalculateChecksum(byte[] content)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = new MemoryStream(content))
                {
                    return Encoding.Default.GetString(md5.ComputeHash(stream));
                }
            }
        }

        /// <summary>
        /// Get string representation of list
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string ListToString<T>(List<T> list)
        {
            if (list == null || list.Count == 0)
                return "";

            string result = String.Empty;

            foreach (var item in list)
            {
                result += item + " ";
            }

            return result;
        }

        /// <summary>
        /// Get list of int values from string(space separated)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static List<int> StringToIntList(string value)
        {
            var strArray = value.Split(new char[] { ' ' });
            var intList = new List<int>();

            for (int i = 0; i < strArray.Length; i++)
                if (strArray[i] != "")
                    intList.Add(Convert.ToInt32(strArray[i]));
                        
            return intList;
        }

        /// <summary>
        /// Get list of string values from string(space separated)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static List<string> StringToStrList(string value)
        {
            var strArray = value.Split(new char[] { ' ' });
            var strList = new List<string>();

            for (int i = 0; i < strArray.Length; i++)
                if (strArray[i] != "")
                    strList.Add(strArray[i]);

            return strList;
        }

        /// <summary>
        /// Merge two byte arrays
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="arr1"></param>
        /// <returns></returns>
        static public byte[] MergeByteArrays(byte[] arr, byte[] arr1)
        {
            List<byte> ls = new List<byte>();
            ls.AddRange(arr);
            ls.AddRange(arr1);

            return ls.ToArray();
        }

        /// <summary>
        /// Convert byte array to an object
        /// </summary>
        /// <param name="data">data to convert</param>
        /// <returns></returns>
        static public object DeserializeData(byte[] data)
        {
            //Create binary formatter for deserialize data
            BinaryFormatter formatter = new BinaryFormatter();

            try
            {

                using (MemoryStream ms = new MemoryStream())
                {
                    //return formatter.Deserialize(ms);
                    ms.Write(data, 0, data.Length);
                    ms.Seek(0, SeekOrigin.Begin);
                    var obj = formatter.Deserialize(ms);
                    return obj;
                }

            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Convert object to an array of bytes
        /// </summary>
        /// <param name="dataToSerialize">object to convert</param>
        /// <returns></returns>
        static public byte[] SerializeData(object dataToSerialize)
        {

            //Create binary formatter for serialize data
            BinaryFormatter formatter = new BinaryFormatter();

            try
            {

                using (MemoryStream ms = new MemoryStream())
                {
                    formatter.Serialize(ms, dataToSerialize);

                    return ms.ToArray();
                }

            }
            catch (Exception)
            {
                return null;
            }
        }

    }
}
