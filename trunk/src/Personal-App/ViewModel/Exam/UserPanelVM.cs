using ST.Models.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;
using ST.Common;

namespace Personal_App.ViewModel.Exam
{
    public class UserPanelVM : MainViewModel
    {
        /// <summary>
        /// 初始化 <see cref="UserPanelVM"/> 类的新实例。
        /// </summary>
        public UserPanelVM()
        {
            User = GlobalUser.USER;
            RaisePropertyChanged("User");
            RaisePropertyChanged("SelectPaperViewName");
            RaisePropertyChanged("SelectPaperNumber");
        }

        private string _examTitle;

        /// <summary>
        /// 考试名称。 
        /// </summary>
        public string ExamTitle
        {
            get
            {
                return _examTitle;
            }
            set
            {
                _examTitle = value;
                RaisePropertyChanged("ExamTitle");
            }
        }

        /// <summary>
        /// 获取或设置用户信息。
        /// </summary>
        public UserInfo User { get; set; }

        //public string UserName { get; set; }

        //public string UserAvatar { get; set; }

        //public string UserPhone { get; set; }

        //public string CardNumber { get; set; }

        //public string CardGrade { get; set; }

        //public string CardExpire { get; set; }

        /// <summary>
        /// 选中试卷名称。
        /// </summary>
        public string SelectPaperViewName { get; set; } = "模拟试题";

        /// <summary>
        /// 选中试卷编号。
        /// </summary>
        public string SelectPaperNumber { get; set; } = "1";
    }
}
