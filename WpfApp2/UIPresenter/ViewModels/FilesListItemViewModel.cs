using Common.Data.Enums;
using System;
using System.IO;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using UI.InversionOfControl;
using WpfApp2;

namespace UI.UIPresenter.ViewModels
{
    public class FilesListItemViewModel : BaseViewModel
    {

        bool showImagePreview;

        #region Public Members

        public ICommand RemoveAttachedFile { get; set; }

        /// <summary>
        /// If true provide button to delate attachment
        /// </summary>
        public bool IsAttachmentsList { get; set; }

        /// <summary>
        /// Gets file name
        /// </summary>
        public string FileName => Path.GetFileName(FilePath);

        public string FilePath { get; set; }

        public bool IsImage { get {
                var a = Enum.GetNames(typeof(ImageTypes));
                foreach (var item in a)
                    if (Path.GetExtension(FilePath).Equals("." + item, StringComparison.CurrentCultureIgnoreCase))
                        return true;
                return false;
            } }

        public bool ShowImagePreview => IsImage && showImagePreview;

        public string ImageHeight
        {
            get
            {
                //Access chacker
                bool getAccess;
                string height = "0";

                do
                {
                    getAccess = false;
                    try
                    {
                        //Try to get access
                        height = IsImage ? new BitmapImage(new Uri(FilePath)).Height.ToString() : "0";
                    }
                    catch (Exception e)
                    {
                        getAccess = true;
                    }
                }
                while (getAccess);

                return height;
            }

        }

        public string ImageWidth {
            get
            {
                //Access chacker
                bool getAccess;
                string width = "0";

                do
                {
                    getAccess = false;
                    try
                    {
                        //Try to get access
                        width = IsImage ? new BitmapImage(new Uri(FilePath)).Width.ToString() : "0";
                    }
                    catch (Exception e)
                    {
                        getAccess = true;
                    }
                }
                while (getAccess);

                return width;
            }

        }


        #endregion

        #region Constructor

        public FilesListItemViewModel(string filePath, bool isAttachmentsList, bool showImagePreview = true)
        {
            FilePath = filePath;
            IsAttachmentsList = isAttachmentsList;
            this.showImagePreview = showImagePreview;

            //Setup commands
            RemoveAttachedFile = new RelayCommand(removeAttachedFile);
        }

        #endregion

        #region Private Method

        void removeAttachedFile()
        {

            ApplicationService.GetChatViewModel.Attachments.Remove(FilePath);

            var attachentsList = ApplicationService.GetChatViewModel.AttachmentsList;
            for (int i = 0; i < attachentsList.Items.Count; i++)
                if(attachentsList.Items[i].FilePath == FilePath)
                    App.Current.Dispatcher.Invoke(() =>
                    {
                        attachentsList.Items.RemoveAt(i);
                    });
        }

        #endregion
    }
}
