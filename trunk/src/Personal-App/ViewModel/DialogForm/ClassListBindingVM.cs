
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using ST.Common.ToolsHelper;
using ST.Common.WebApi;
using ST.Models.Api;
using Personal_App.Common;
using Personal_App.Domain;
using MaterialDesignThemes.Wpf;
using ST.Common;
using ST.Models.Paper;
using Card = ST.Models.Api.Card;

namespace Personal_App.ViewModel
{
    public class ClassListBindingVM : MainViewModel, IView
    {
        public void Activated(object state)
        {
            throw new NotImplementedException();
        }

        public const string ViewName = "ClassListBinding";

        public ClassListBindingVM()
        {
            ErrMsgOut = "";
        }

        public ClassListBindingVM(UserControl uc, WrapPanel classPanel, string phoneNum):this()
        {

            this._uc = uc as ClassListBinding;

            this._classPanel = classPanel;

            _uc.btnBindingClassCmd.IsEnabled = false;

            _classPanel.Children.Clear();

            AddClassList(phoneNum);
        }

        #region << 字段 属性 >>

        private ClassListBinding _uc;

        private WrapPanel _classPanel;

        private string _selected_class_id = "-1";
        private string _selected_class_name = "无";

        private string _errMsgOut;

        public string ErrMsgOut
        {
            get
            {
                return _errMsgOut;
            }
            set
            {
                _errMsgOut = value;
                RaisePropertyChanged("ErrMsgOut");
            }
        }

        #endregion

        #region << Btn Command >>
        
        private RelayCommand bindingClassCmd;//加入 新班级

        public RelayCommand BindingClassCmd
        {
            get
            {
                return bindingClassCmd ?? (bindingClassCmd = new RelayCommand(
                           (action) =>
                           {
                               //打开班级列表
                               //show the dialog
                               ExecuteRunExtendedDialog(action);
                           }));
            }
        }

        #endregion

        #region << 自定义 >>

        private void AddClassList(string phoneNum)
        {

            GetTeacherClassListModel model = new GetTeacherClassListModel()
            {
                phone = phoneNum,
                token = GlobalUser.USER.Token
            };
            //开始绑定
            var result = WebApiProxy.GetHtmlRespInfo(model, ApiType.GetTeacherClassList, null, "get");

            if (result.retCode == 0)
            {

                List<ClassInfo> classInfos =
                    JsonHelper.FromJson<List<ClassInfo>>(result.retData.ToString());
                RadioButton rbtn = null;
                for (int i = 0; i < classInfos.Count; i++)
                {
                    rbtn = new RadioButton();
                    rbtn.Style = _uc.Resources["ClassBoxRadioButton"] as Style;
                    rbtn.Margin = new Thickness(8);
                    rbtn.Content = classInfos[i].Class_name;
                    rbtn.CommandParameter = classInfos[i].Class_id;
                    rbtn.Checked += Rbtn_Checked;

                    _classPanel.Children.Add(rbtn);
                }

                if (classInfos.Count < 1)
                {
                    ErrMsgOut = "没有班级可供添加！";
                }
            }
            else
            {
                //绑定失败
                ErrMsgOut = result.retMsg;
            }
        }

        private void Rbtn_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton rbtn = sender as RadioButton;
            if (rbtn.IsChecked == true)
            {
                _selected_class_id = rbtn.CommandParameter.ToString();
                _selected_class_name = rbtn.Content.ToString();
                _uc.btnBindingClassCmd.IsEnabled = true;
            }
        }

        private async void ExecuteRunExtendedDialog(object o)
        {
            if (_selected_class_id == "-1")
            {
                ErrMsgOut = "还未选中班级，无法绑定！";
            }
            else
            {

                UserBindClassModel model = new UserBindClassModel()
                {
                    class_id = _selected_class_id,
                };
                //开始绑定
                var result = WebApiProxy.GetHtmlRespInfo(model, ApiType.UserBindClass, GlobalUser.USER.Token);

                if (result.retCode == 0)
                {
                    ErrMsgOut = $"您已绑定班级 {_selected_class_name}！";

                    GlobalUser.CLASSINFO = new ClassInfo()
                    {
                        Class_id = _selected_class_id,
                        Class_name = _selected_class_name,
                        Token = GlobalUser.USER.Token,
                    };

                    //_uc.CurrentCloseBtn.CommandParameter = true;

                    await Task.Factory.StartNew(() => Thread.Sleep(333))
                        .ContinueWith(t =>
                        {
                            RememberUser();

                            _uc.CurrentCloseBtn.CommandParameter = true;

                            ButtonAutomationPeer peer =
                                new ButtonAutomationPeer(_uc.CurrentCloseBtn);

                            IInvokeProvider invokeProv =
                                peer.GetPattern(PatternInterface.Invoke)
                                    as IInvokeProvider;
                            invokeProv.Invoke();

                        }, TaskScheduler.FromCurrentSynchronizationContext());

                }
                else
                {
                    //绑定失败
                    ErrMsgOut = result.retMsg;
                }

            }
        }

        #endregion
    }
}