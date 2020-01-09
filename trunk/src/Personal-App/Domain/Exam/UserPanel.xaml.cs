using System.Windows.Controls;

namespace Personal_App.Domain.Exam
{
    /// <summary>
    /// UserInfoUC.xaml 的交互逻辑
    /// </summary>
    public partial class UserPanel : UserControl
    {
        public UserPanel()
        {
            InitializeComponent();

            //var userInfo = GlobalUser.USER;
            //var cardInfo = GlobalUser.USER.Card;
            //if (userInfo != null)
            //{
            //    UserInfoVM userInfoVM = new UserInfoVM()
            //    {
            //        User = GlobalUser.USER,
            //        //UserName = userInfo.UserName,
            //        //UserAvatar = userInfo.Avatar,
            //        //UserPhone = userInfo.Mobile,
            //        //CardNumber = cardInfo.CardNo,
            //        //CardGrade = cardInfo.CardName,
            //        //CardExpire = String.Format("{0}-{1}", cardInfo.ValidityBegin, cardInfo.ValidityEnd)
            //    };



            //    string pagerName = GlobalUser.SelectPaperViewName;
            //    if (!string.IsNullOrWhiteSpace(pagerName))
            //    {
            //        pagerName = Regex.Replace(pagerName, @"\d", "");
            //        pagerName = pagerName.Substring(pagerName.Length - 4, 4);
            //        userInfoVM.SelectPaperViewName = pagerName;
            //    }
            //    userInfoVM.SelectPaperNumber = GlobalUser.SelectPaperNumber;
            //    DataContext = userInfoVM;
            //if (!IsLoaded)
            //{
            //    if (!HttpHelper.RemoteFileExists(GlobalUser.USER.Avatar))
            //    {
            //        //GlobalUser.USER.Avatar = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "head.png");
            //        AvatarImage.Source = ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.head);
            //    }
            //}

            //            }
            //            else
            //            {

            //                //MessageBox.Show("获取用户信息失败，请重新登录！");
            //#if(DEBUG)
            //                UserInfoVM userInfoVM = new UserInfoVM()
            //                {
            //                    User = GlobalUser.USER,
            //                    //UserName = "张晓萌",
            //                    //UserAvatar = "",
            //                    //UserPhone = "13567823421",
            //                    //CardNumber = "BJ12345678",
            //                    //CardGrade = "北京九年级",
            //                    //CardExpire = "2017.11.14 - 2018.2.15"
            //                };
            //                DataContext = userInfoVM;
            //#endif

            //            }
        }

        //private void AvatarImage_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        //{
        //    if (!HttpHelper.RemoteFileExists(GlobalUser.USER.Avatar))
        //    {
        //        //GlobalUser.USER.Avatar = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "head.png");
        //        AvatarImage.Source = ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.head);
        //    }
        //}

        //private void AvatarImage_SourceUpdated(object sender, DataTransferEventArgs e)
        //{
        //    if (!HttpHelper.RemoteFileExists(GlobalUser.USER.Avatar))
        //    {
        //        //GlobalUser.USER.Avatar = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "head.png");
        //        AvatarImage.Source = ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.head);
        //    }
        //}
    }
}
