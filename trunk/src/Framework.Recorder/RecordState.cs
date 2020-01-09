namespace Framework.Recorder
{
    public enum RecordState
    {
        IDLE,                          //空闲
        RECORDING,                     //录音
        PLAYING,                       //播放
        PAUSEING,                      //暂停
        STOPPED                        //停止
    }


    public enum VideoPlayFile
    {
        URI,                          //流媒体
        FileInfo,                     //普通文件流
        NONE,                       //无
    }
}
