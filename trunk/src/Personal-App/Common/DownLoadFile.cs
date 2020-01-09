using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Framework.Logging;
using ST.Common;
using ST.Common.ToolsHelper;
using ST.Common.WebApi;
using ST.Models.Paper;

namespace Personal_App.Common
{
    public class DownLoadFile
    {
        private string _fileUrl;//文件地址
        private int _threadNum;    //线程数量
        private Thread[] _thread;   //线程数组
        private Dictionary<string, string> _aduioFileDic;//文件信息

        /// <summary>
        /// /// 构造函数
        /// /// </summary>
        /// /// <param name="threahNum">线程数量</param>
        /// /// <param name="fileUrl">文件Url路径</param>
        /// /// <param name="savePath">本地保存路径</param>
        public DownLoadFile()
        {
            if (_aduioFileDic == null)
                _aduioFileDic = new Dictionary<string, string>();


            //_aduioFileDic = JsonHelper.FromJson<Dictionary<string, string>>(GlobalUser.SelectPaperInfo.paper_assets.ToString());

            var myAnswerResults = new List<Exam_Attend_Result_Item>();
            if (!string.IsNullOrEmpty(GlobalUser.SelectExamAttendResult.Trim()))
                myAnswerResults = JsonHelper.FromJson<List<Exam_Attend_Result_Item>>(GlobalUser.SelectExamAttendResult).Where(w => w.user_answer.Contains("records.")).ToList();

            for (int i = 0; i < myAnswerResults.Count; i++)
            {
                var userAnswer = myAnswerResults[i].user_answer;
                var userAnswerSplit = userAnswer.Split('/');
                if (userAnswerSplit.Length == 2)
                    _aduioFileDic.Add(userAnswerSplit[1], $"http://{userAnswer}.mp3");
            }

            if (_aduioFileDic != null)
                this._threadNum = _aduioFileDic.Keys.Count;
            this._thread = new Thread[_threadNum];
        }

        public void Start()
        {
            for (int i = 0; i < _threadNum; i++)
            {
                //下载指定位置的数据
                _thread[i] = new Thread(Download);
                _thread[i].Name = System.IO.Path.GetFileNameWithoutExtension(_fileUrl) + "_{0}".Replace("{0}", Convert.ToString(i + 1));
                _thread[i].Start(_aduioFileDic[_aduioFileDic.Keys.ToList()[i]]);

            }
        }

        private void Download(object obj)
        {
            try
            {
                Log4NetHelper.Info(GlobalUser.AUDIODATAFOLDER);
                string url = obj as string;

                if (!url.Contains("records."))
                    url = $"{WebApiProxy.MEDIAURL}{url}";

                var fileByte = WebApiProxy.GetAudioFile(url);
                if (fileByte != null)
                    File.WriteAllBytes(Path.Combine(GlobalUser.AUDIODATAFOLDER, Path.GetFileName(url)), fileByte);
                else
                {
                    Log4NetHelper.Error($"下载 音频失败, err:{url}");
                }
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error($"下载 音频失败, err:{ex}");
            }
        }


    }
}