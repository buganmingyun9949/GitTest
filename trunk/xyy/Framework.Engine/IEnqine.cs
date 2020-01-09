using System;
using System.Runtime.InteropServices;
using Framework.Engine.Models;

namespace Framework.Engine
{

    public class EngineDelegete
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int EngineCallback(IntPtr usrdata, [MarshalAs(UnmanagedType.LPStr)] string id, int type, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1, SizeParamIndex = 4)] byte[] message, int size);
    }

    public interface IEngine
    {
        /// <summary>
        /// 启动评分引擎
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="recId"></param>
        /// <param name="callback"></param>
        /// <param name="userdata"></param>
        /// <returns></returns>
        int Start(BaseEngineStartUpParameter parameter, ref string recId, EngineDelegete.EngineCallback callback,
            IntPtr userdata, string userID = null);

        /// <summary>
        /// 停止评分
        /// </summary>
        /// <returns></returns>
        int Stop();

        /// <summary>
        /// 接受数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        int Feed(byte[] data);

        /// <summary>
        /// 取消
        /// </summary>
        /// <returns></returns>
        int Cancel();


        /// <summary>
        /// 
        /// </summary>
        /// <param name="opt"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        int Opt(int opt, ref string result);
    }
}
