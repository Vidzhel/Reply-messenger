using Dapper;
using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace CommonLibs.Data
{
    public class FileManager
    {

        /// <summary>
        /// Check if in the directory already exist a file with same name, and return unique
        /// </summary>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        public static string CheckOnUniqueness(string fullPath)
        {
            int count = 1;

            string fileNameOnly = Path.GetFileNameWithoutExtension(fullPath);
            string extension = Path.GetExtension(fullPath);
            string path = Path.GetDirectoryName(fullPath);
            string newFullPath = fullPath;

            while (System.IO.File.Exists(newFullPath))
            {
                string tempFileName = string.Format("{0}({1})", fileNameOnly, count++);
                newFullPath = Path.Combine(path, tempFileName + extension);
            }
            return newFullPath;
        }

        /// <summary>
        /// Check if there is a file on the path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsFileExist(string path)
        {
            return System.IO.File.Exists(path);
        }

        /// <summary>
        /// Check if there is a directory on the path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsDirectoryExist(string path)
        {
            return Directory.Exists(path);
        }

        /// <summary>
        /// Creates folder in the same directory as a file
        /// </summary>
        /// <param name="name">name of new directory</param>
        /// <returns></returns>
        public static void CreateLocalFolder(string name)
        {
            //Get current directory path
            var currentPath = Directory.GetCurrentDirectory();

            //Add name of new directory
            var newDirectoryPath = currentPath + @"\" + name;

            //Create directory
            Directory.CreateDirectory(newDirectoryPath);
        }

        /// <summary>
        /// Creates a file in a local directory, if directory doesn't exist, than return false
        /// </summary>
        /// <param name="name"></param>
        public static bool CreateFileInLocalDir(string dirName,string fileName)
        {
            var dirPath = Directory.GetCurrentDirectory() + @"\" + dirName;

            //Check if directory exist
            if (!Directory.Exists(dirPath))
                return false;

            //Create file
            System.IO.File.Create(dirPath + @"\" + fileName);
            return true;
        }

        /// <summary>
        /// Opens file manager with specific filter and returns file path
        /// </summary>
        /// <param name="filter">Example: txt files (*.txt)|*.txt|All files (*.*)|*.*</param>
        public static string OpenFileDialogForm(string filter)
        {
            var filePath = string.Empty;

            var t = new Thread((ThreadStart)(() => {

                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.InitialDirectory = "c:\\";
                    openFileDialog.Filter = filter;
                    openFileDialog.FilterIndex = 2;
                    openFileDialog.RestoreDirectory = true;

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        //Get the path of specified file
                        filePath = openFileDialog.FileName;
                    }

                }


            }));

            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            t.Join();


            return filePath;
        }

        /// <summary>
        /// Check if the main messanger folders exist, it not then create them
        /// </summary>
        public static void CheckClientRequiredFolders()
        {
            var mainMessengerDir = Directory.GetCurrentDirectory() + @"\Reply Messenger";

            //Check if main messanger dir exist
            if (!Directory.Exists(mainMessengerDir))
                //Create dir
                Directory.CreateDirectory(mainMessengerDir);

            //Check if saved files dir exist
            if (!Directory.Exists(mainMessengerDir + @"\Saved Files"))
                Directory.CreateDirectory(mainMessengerDir + @"\Saved Files");

            //Check if user files dir exist
            if (!Directory.Exists(mainMessengerDir + @"\User Files"))
                Directory.CreateDirectory(mainMessengerDir + @"\User Files");




        }

        /// <summary>
        /// Crecks and creates data base if it doesn't exist
        /// </summary>
        /// <param name="databaseConnectionString"></param>
        public static void ClientDatabaseIntegrityCheck(string databaseConnectionString) {

            //Check for the main directories
            CheckClientRequiredFolders();

            //Check if database exist
            if (!System.IO.File.Exists(Directory.GetCurrentDirectory() + @"\Reply Messenger" + @"\SQLiteLocalDB.db"))
            {

                //Create database
                SQLiteConnection.CreateFile(Directory.GetCurrentDirectory() + @"\Reply Messenger" + @"\SQLiteLocalDB.db");

                #region requests

                var createContactsTable = @"CREATE TABLE Contacts (
                                            Id    INTEGER NOT NULL UNIQUE,
                                            UserName  TEXT NOT NULL,
	                                        ProfilePhoto  BLOB,
	                                        Email TEXT NOT NULL UNIQUE,
                                            Bio   TEXT,
	                                        Online    TEXT NOT NULL,
	                                        LastTimeUpdated   TEXT NOT NULL,
	                                        ChatsId   INTEGER
                                        )";

                var createGroupsTable = @"CREATE TABLE Groups (
                                            Id    INTEGER NOT NULL,
                                            Name  TEXT NOT NULL,
                                            AdminsId  TEXT NOT NULL,
                                            MembersId TEXT,
                                            Image TEXT,
                                            IsPrivate TEXT NOT NULL,
                                            IsChannel TEXT NOT NULL,
                                            UsersOnline   INTEGER,
                                            LastTimeUpdated   INTEGER
                                        )";

                var createMessagesTable = @"CREATE TABLE Messages(
                                            Id    INTEGER NOT NULL,
                                            SenderId  INTEGER NOT NULL,
                                            ReceiverId    INTEGER NOT NULL,
                                            Data  TEXT NOT NULL,
                                            DataType  TEXT NOT NULL,
                                            Date  TEXT NOT NULL,
                                            Status    TEXT NOT NULL,
                                            LastTimeUpdated   TEXT NOT NULL,
                                            Attachments   TEXT NOT NULL
                                        )";

                #endregion

                //Create tables
                using (IDbConnection con = new SQLiteConnection(databaseConnectionString))
                {
                    con.Execute(createContactsTable);
                    con.Execute(createGroupsTable);
                    con.Execute(createMessagesTable);
                }

            }

        }

        /// <summary>
        /// Crecks and creates data base if it doesn't exist
        /// </summary>
        /// <param name="databaseConnectionString"></param>
        public static void ServerDatabaseIntegrityCheck(string databaseConnectionString)
        {

            //Check for the main directories
            CheckServerRequiredFolders();

            //Check if database exist
            if (!System.IO.File.Exists(Directory.GetCurrentDirectory() + @"\Reply Messenger Server" + @"\SQLiteRemoteDB.db"))
            {

                //Create database
                SQLiteConnection.CreateFile(Directory.GetCurrentDirectory() + @"\Reply Messenger Server" + @"\SQLiteRemoteDB.db");

                #region requests

                var createFilesTable = @"CREATE TABLE Files (
                                            Id    INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                                            FileName  TEXT NOT NULL,
	                                        FileNameOnServer  TEXT NOT NULL,
	                                        Checksum  TEXT NOT NULL
                                        )";

                var createUsersTable = @"CREATE TABLE Users (
                                            Id    INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	                                        UserName  TEXT NOT NULL,
	                                        Password  TEXT NOT NULL,
	                                        Email TEXT NOT NULL UNIQUE,
                                            Bio   TEXT,
	                                        Online    TEXT NOT NULL,
	                                        ChatsId   TEXT,
	                                        ProfilePhoto  TEXT,
	                                        ContactsId    TEXT,
	                                        LastTimeUpdated   INTEGER
                                        )";

                var createGroupsTable = @"CREATE TABLE Groups (
                                            Id    INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
                                            Name  TEXT NOT NULL,
                                            AdminsId  TEXT NOT NULL,
                                            MembersId TEXT,
                                            Image TEXT,
                                            IsPrivate TEXT NOT NULL,
                                            IsChannel TEXT NOT NULL,
                                            UsersOnline   INTEGER,
                                            LastTimeUpdated   INTEGER
                                        )";

                var createMessagesTable = @"CREATE TABLE Messages(
                                            Id    INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
                                            SenderId  INTEGER NOT NULL,
                                            ReceiverId    INTEGER NOT NULL,
                                            Data  TEXT NOT NULL,
                                            DataType  TEXT NOT NULL,
                                            Date  TEXT NOT NULL,
                                            Status    TEXT NOT NULL,
                                            LastTimeUpdated   TEXT NOT NULL,
                                            Attachments   TEXT
                                        )";

                #endregion

                //Create tables
                using (IDbConnection con = new SQLiteConnection(databaseConnectionString))
                {
                    con.Execute(createFilesTable);
                    con.Execute(createUsersTable);
                    con.Execute(createGroupsTable);
                    con.Execute(createMessagesTable);
                }

            }

        }

        /// <summary>
        /// Check if the main server folders exist, it not then create them
        /// </summary>
        public static void CheckServerRequiredFolders()
        {
            var mainDir = Directory.GetCurrentDirectory() + @"\Reply Messenger Server";

            //Check if main messanger dir exist
            if (!Directory.Exists(mainDir))
                //Create dir
                Directory.CreateDirectory(mainDir);

            //Check if saved files dir exist
            if (!Directory.Exists(mainDir + @"\Saved Files"))
                Directory.CreateDirectory(mainDir + @"\Saved Files");

        }
    }
}
