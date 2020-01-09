using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personal_App.ViewModel
{
    public class MessageDialogVM : MainViewModel
    {
        /// <summary>
        /// 消息内容。
        /// </summary>
        public string MessageContent { get; set; }


        private string _MsgTitle;
        public string MsgTitle
        {
            get { return _MsgTitle; }

            set
            {
                _MsgTitle = value;
                RaisePropertyChanged("MsgTitle");
            }
        }

        private string _MsgContent;
        public string MsgContent
        {
            get { return _MsgContent; }

            set
            {
                _MsgContent = value;
                RaisePropertyChanged("MsgContent");
            }
        }
    }
}
