using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight.Messaging;
using Personal_App.Domain.Menu;
using Personal_App.ViewModel.Menu;

namespace Personal_App.ViewModel
{
    public class TyHomeworkControlVM : MainViewModel
    {
        public const string ViewName = "TyHomeworkControlVM";


        #region << Btn Command>>


        private RelayCommand backTrainCmd;//返回按钮

        public RelayCommand BackTrainCmd
        {
            get
            {
                return backTrainCmd ?? (backTrainCmd = new RelayCommand(
                           (action) =>
                           {

                               Messenger.Default.Send(new NavigateMessage(MenuFullVerVM.ViewName, MenuFullVerVM.ViewName, false), "ShowUserpapers");
                           }));
            }
        }

        #endregion
    }
}
