using GalaSoft.MvvmLight.Messaging;
using Personal_App.Domain;
using Personal_App.ViewModel.Menu;

namespace Personal_App.ViewModel
{
    public class FreePracticsDetailControlVM: MainViewModel
    {
        public const string ViewName = "FreePracticsDetailControlVM";

        public FreePracticsDetailControlVM(int index, string name, int qsType)
        {
            this._qsType = qsType;

            this.TxtName = name;

            this.BtnBgImg = $"/Resources/lx{index}.png";
        }


        #region << 属性 字段 >>

        private int _qsType;

        private string _TxtName;

        public string TxtName
        {
            get => _TxtName;
            set
            {
                _TxtName = value;
                RaisePropertyChanged("TxtName");
            }
        }


        private object _BtnBgImg;
        /// <summary>
        /// 图片。
        /// </summary>
        public object BtnBgImg
        {
            get
            {
                return _BtnBgImg;
            }
            set
            {
                _BtnBgImg = value;
                RaisePropertyChanged("BtnBgImg");
            }
        }

        #endregion

        #region << Btn Command >>

        private RelayCommand _ShowFreePracticsUcCmd;

        public RelayCommand ShowFreePracticsUcCmd
        {
            get
            {
                return _ShowFreePracticsUcCmd ?? (_ShowFreePracticsUcCmd = new RelayCommand(
                           (action) => { Go(); }));
            }
        }

        #endregion

        #region << 自定义方法 >>

        private void Go()
        {
            switch (_qsType)
            {
                case 4:

                    //加载 单词清单
                    var view4 = new SyncListControl();
                    view4.DataContext = new SyncListControlVM(view4.TrainDetailContentPanel,
                        _qsType);

                    Messenger.Default.Send(
                        new NavigateMessage(SyncListControlVM.ViewName, "N", view4, true),
                        "ShowUserpapers");
                    break;
                case 5:


                    //加载 单元清单
                    var view5 = new SyncUnitListControl();
                    view5.DataContext = new SyncUnitListControlVM(view5.SyncUnitContentPanel,
                        _qsType);

                    Messenger.Default.Send(
                        new NavigateMessage(SyncUnitListControlVM.ViewName, "N", view5, true),
                        "ShowUserpapers");
                    break;
                case 3:

                    Messenger.Default.Send(
                        new NavigateMessage(MenuTrainVM.ViewName, MenuTrainVM.ViewName, true),
                        "ShowUserpapers");
                    break;
                case 2:

                    Messenger.Default.Send(
                        new NavigateMessage(MenuExamVM.ViewName, MenuExamVM.ViewName, true),
                        "ShowUserpapers");
                    break;
                case 0:

                    Messenger.Default.Send(
                        new NavigateMessage(MenuHomeworkVM.ViewName, MenuHomeworkVM.ViewName, true),
                        "ShowUserpapers");
                    break;
            }
        }

        #endregion
    }
}