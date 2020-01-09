using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace ST.Models.Api
{
    public class TaskInfo
    {
        public string UserPhone { get; set; }

        /// <summary>
        /// 作业ID
        /// </summary>
        public string ZyID { get; set; }

        /// <summary>
        /// 作业名称
        /// </summary>
        public string ZyName { get; set; }

        /// <summary>
        /// 题目清单
        /// </summary>
        public List<TaskSubInfo> ZySubs { get; set; }

        /// <summary>
        /// 重做次数(任何重新开始做)
        /// </summary>
        [DefaultValue(0)]
        public int AgainCount { get; set; }

        /// <summary>
        /// 重做次数(完成后重新开始做)
        /// </summary>
        [DefaultValue(0)]
        public int ConpleteAgainCount { get; set; }

        /// <summary>
        /// 当前记录的作业 是否有未完成的题目
        /// 只标记大题作为统计结果
        /// 并且 大题数量 > 1 
        /// </summary>
        public bool CurrentTaskDone
        {
            get
            {
                if (ZySubs == null || ZySubs.Where(w => w.Done).Count() == ZySubs.Count || ZySubs.Where(w => w.Done == false).Count() == ZySubs.Count)
                {
                    return true;
                }
                else
                {
                    return false; //ZySubs.Sum(m => m.DoneInt) > 0 ? false : true;
                }
            }
        }

    }

    public class TaskSubInfo
    {
        /// <summary>
        /// 题目 ID
        /// </summary>
        public string SubId { get; set; }

        /// <summary>
        /// 题目名称
        /// </summary>
        public string SubName { get; set; }

        /// <summary>
        /// 是否完成
        /// true:完成
        /// false:未完成
        /// </summary>
        public bool Done { get; set; }

        /// <summary>
        /// 是否完成
        /// 0:完成
        /// 1:未完成
        /// </summary>
        public int DoneInt
        {
            get
            {
                if (Done)
                {
                    return 0;
                }
                else
                {
                    return 1;
                }
            }

        }
    }
}
