
namespace CommonLibs.Data
{
    /// <summary>
    /// Represent file
    /// </summary>
    public class File
    {

        #region Public Members

        public string FileName { get; set; }

        public string FileNameOnServer{ get; set; }

        public string Checksum { get; set; }

        #endregion

        #region Constructor

        public File(string fileName, string fileNameOnServer, string checksum)
        {
            FileName = fileName;
            FileNameOnServer = fileNameOnServer;
            Checksum = checksum;
        }

        private File() { }

        #endregion
    }
}
