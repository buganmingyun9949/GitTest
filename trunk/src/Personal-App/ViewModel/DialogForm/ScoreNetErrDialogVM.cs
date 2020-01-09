namespace Personal_App.ViewModel
{
    public class ScoreNetErrDialogVM
    {
        public const string ViewName = "ScoreNetErrDialogVM";

        public ScoreNetErrDialogVM()
        { 
        }

        #region << Btn Command >>

        private RelayCommand _ReuploadNowCmd;//立即上传

        public RelayCommand ReuploadNowCmd
        {
            get
            {
                return _ReuploadNowCmd ?? (_ReuploadNowCmd = new RelayCommand(
                           (action) =>
                           {

                           }));
            }
        }

        private RelayCommand _ReuploadLaterCmd;//稍后上传

        public RelayCommand ReuploadLaterCmd
        {
            get
            {
                return _ReuploadLaterCmd ?? (_ReuploadLaterCmd = new RelayCommand(
                           (action) =>
                           {

                           }));
            }
        }

        #endregion

        #region << 自定义 >>
        
        private async void ExecuteRunExtendedDialog(object o)
        {
            if (string.IsNullOrEmpty(""))
            {

            }
        }

        #endregion

    }
}