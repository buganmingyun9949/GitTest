namespace ST.Common.WebApi
{

    public enum ScoreType
    {
        /// <summary>
        /// 未评分
        /// </summary>
        NoScore = 0,

        /// <summary>
        /// 已评分
        /// </summary>
        ScoreSuccess,

        /// <summary>
        /// 评分失败
        /// </summary>
        ScoreFailure
    }
}