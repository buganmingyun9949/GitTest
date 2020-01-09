namespace Personal_App.ViewModel
{
    public class ScoreServiceErrDialogVM
    {
        public const string ViewName = "ScoreServiceErrDialogVM";

        public ScoreServiceErrDialogVM()
        { 
        }

        #region << Btn Command >>

        private RelayCommand _AnswerNowCmd;//立即上传

        public RelayCommand AnswerNowCmd
        {
            get
            {
                return _AnswerNowCmd ?? (_AnswerNowCmd = new RelayCommand(
                           (action) =>
                           {

                           }));
            }
        }

        private RelayCommand _AnswerLaterCmd;//稍后上传

        public RelayCommand AnswerLaterCmd
        {
            get
            {
                return _AnswerLaterCmd ?? (_AnswerLaterCmd = new RelayCommand(
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