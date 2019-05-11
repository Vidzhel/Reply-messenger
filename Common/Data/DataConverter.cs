using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace CommonLibs.Data
{

    /// <summary>
    /// Provide methods which hepl convert data
    /// </summary>
    public class DataConverter
    {

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
