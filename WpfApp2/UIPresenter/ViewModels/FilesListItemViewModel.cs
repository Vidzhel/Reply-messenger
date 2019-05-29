using System.IO;
using System.Windows.Input;
using UI.InversionOfControl;
using WpfApp2;

namespace UI.UIPresenter.ViewModels
{
    public class FilesListItemViewModel : BaseViewModel
    {

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

        #endregion

        #region Constructor

        public FilesListItemViewModel(string filePath, bool isAttachmentsList)
        {
            FilePath = filePath;
            IsAttachmentsList = isAttachmentsList;


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
