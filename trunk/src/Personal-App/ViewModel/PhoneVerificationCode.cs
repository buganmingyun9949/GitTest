using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using ST.Models;

namespace Personal_App.ViewModel
{
    public class PhoneVerificationCode : MainViewModel
    {
        private int _interval;//记录倒计时长
        private string idCode;//在规定时间内保存验证码
        private int idCodeTime;//设置验证码的有效时间（秒）
        private int idCodeNum = 6;//设置验证码的位数

        public void GetCode(string phoneNum)
        {
            //获取验证码
            timerSend = new Timer(1000);
            timerSend.AutoReset = true;
            timerSend.Elapsed += Timer_Elapsed;
            _interval = SecondNum;
            timerSend.Start();

            //在验证码有效期内，再次请求验证码，需要先关闭上一次的
            if (timerTime != null)
            {
                timerTime.Close();
                timerTime.Dispose();
            }
            //验证码的有效期
            timerTime = new Timer(1000);
            timerTime.AutoReset = true;
            timerTime.Elapsed += TimerTime_Elapsed;
            timerTime.Start();
            idCodeTime = SaveTime;
            IdCode = RandomCode.RandomCodeCommand(idCodeNum);
            PhoneNum = phoneNum;
        }

        public void ClearCode()
        {
            BtnIsEnable = true;
            BtnContent = "获取验证码";
            timerSend.Stop();
            timerSend.Dispose();
        }

        #region 获取验证码倒计时
        Timer timerSend;
        Timer timerTime;
        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            BtnIsEnable = false;
            BtnContent = "(" + (_interval--) + ")秒后重新发";

            if (_interval <= -1)
            {
                BtnIsEnable = true;
                BtnContent = "获取验证码";
                timerSend.Stop();
                timerSend.Dispose();
            }
            //throw new NotImplementedException();
        }
        private void TimerTime_Elapsed(object sender, ElapsedEventArgs e)
        {
            idCodeTime--;
            if (idCodeTime <= 0)
            {
                IdCode = "";
                timerTime.Stop();
                timerTime.Dispose();
            }
            Console.WriteLine(IdCode);
            //throw new NotImplementedException();
        }
        #endregion

        #region 字段
        //*************************************************************************************************//上线时需要修改
        private int secondNum = 60;//设置倒计时长
        private int saveTime = 60;//设置保存验证码时长
                                  //*************************************************************************************************//
        private string btnContent = "获取验证码";//设置获取验证码按钮显示的名称
        private bool btnIsEnable = true;//设置获取验证码按钮是否可用
        private string currentColor = "Red";//设置当前内容颜色

        private string phoneNum;//记录是否是发送验证码的手机号
        public int SecondNum
        {
            get
            {
                return secondNum;
            }

            set
            {
                secondNum = value;
            }
        }

        public int SaveTime
        {
            get
            {
                return saveTime;
            }

            set
            {
                saveTime = value;
            }
        }

        public string BtnContent
        {
            get
            {
                return btnContent;
            }

            set
            {
                btnContent = value;
                RaisePropertyChanged("BtnContent");
            }
        }

        public bool BtnIsEnable
        {
            get
            {
                return btnIsEnable;
            }

            set
            {
                btnIsEnable = value;
                RaisePropertyChanged("BtnIsEnable");
            }
        }

        public string IdCode
        {
            get
            {
                return idCode;
            }

            set
            {
                idCode = value;
                RaisePropertyChanged("IdCode");
            }
        }

        public string PhoneNum
        {
            get
            {
                return phoneNum;
            }

            set
            {
                phoneNum = value;
                RaisePropertyChanged("PhoneNum");
            }
        }

        public string CurrentColor
        {
            get
            {
                return currentColor;
            }

            set
            {
                currentColor = value;
                RaisePropertyChanged("CurrentColor");
            }
        }

        #endregion
    }
}
