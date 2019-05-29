using ClientLibs.Core.DataAccess;
using CommonLibs.Connections.Repositories;
using CommonLibs.Connections.Repositories.Tables;
using CommonLibs.Data;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using WpfApp2;

namespace UI.UIPresenter.ViewModels
{
    public class FilesListViewModel : BaseViewModel
    {

        #region Public Member

        public ObservableCollection<FilesListItemViewModel> Items { get; set; } = new ObservableCollection<FilesListItemViewModel>();

        public bool IsAttacmentsList { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Create list of files
        /// </summary>
        /// <param name="filePaths">list of file pathes</param>
        /// <param name="isAttacmentsList">Is attachments list</param>
        public FilesListViewModel(List<string> filePaths, bool isAttacmentsList = false)
        {
            IsAttacmentsList = isAttacmentsList;

            foreach (var filePath in filePaths)
                Items.Add(new FilesListItemViewModel(filePath, isAttacmentsList));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isGroupContactsList">If true provide button to remove user from a group</param>
        /// <param name="isInviteList">If true provide button to invite user to a group</param>
        public FilesListViewModel(bool isAttachmentsList = false)
        {
            IsAttacmentsList = isAttachmentsList;

        }

        #endregion
        
    }
}
