namespace ST.Common.WebApi
{

    public enum ScoreType
    {
        /// <summary>
        /// 未评分
        /// </summary>
        NoScore = 0,

        /// <summary>
        /// 评分中
        /// </summary>
        Scoring,

        /// <summary>
        /// 已评分
        /// </summary>
        ScoreSuccess,

        /// <summary>
        /// 评分失败
        /// </summary>
        ScoreFailure
    }


    public enum MenuType
    {
        /// <summary>
        /// 首页
        /// </summary>
        Main = 0,

        /// <summary>
        /// 专项训练
        /// </summary>
        Train,

        /// <summary>
        /// 我的作业
        /// </summary>
        Task,

        /// <summary>
        /// 模考
        /// </summary>
        Exam,

        /// <summary>
        /// 同步
        /// </summary>
        Sync,

        /// <summary>
        /// 报纸同步
        /// </summary>
        PaperSync,

        /// <summary>
        /// 体验  完整版
        /// </summary>
        TyFullVer,
    }

}