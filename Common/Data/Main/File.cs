
namespace CommonLibs.Data
{
    /// <summary>
    /// Represent file
    /// </summary>
    public class File
    {

        #region Public Members

        public string FileName { get; set; }

        public string Checksum { get; set; }

        #endregion

        #region Constructor

        public File(string fileName, string checksum)
        {
            FileName = fileName;
            Checksum = checksum;
        }

        private File() { }

        #endregion
    }
}
