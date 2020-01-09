using System;
using System.Configuration;
using System.Runtime.CompilerServices;

namespace Framework.Engine
{
    /// <summary>
    /// 引起由其产生和销毁
    /// </summary>
    public class EngineProvider
    {
        private const string ConfigAppkey = "appKey";
        private const string ConfigSecretkey = "secretKey";
        private const string ConfigServerUri = "serverUri";
        private const string ConfigServerTimeout = "serverTimeout";
        public static string AppKey;
        public static string SecretKey;
        public static string ServerUri;
        public static string ServerTimeout;
        private static IEngine _engine;
        private static readonly object EngineLock = new object();

        static EngineProvider()
        {
            AppKey = ConfigurationManager.AppSettings[ConfigAppkey];
            SecretKey = ConfigurationManager.AppSettings[ConfigSecretkey];
            ServerUri = ConfigurationManager.AppSettings[ConfigServerUri];
            ServerTimeout = ConfigurationManager.AppSettings[ConfigServerTimeout];
        }

        /// <summary>
        /// 直接将四种评分引擎初始化
        /// </summary>
        /// <returns></returns>
        public static IEngine GetEngine(bool CreateNew = false)
        {
            lock (EngineLock)
            {
                if (!CreateNew)
                {
                    if (_engine != null) return _engine;
                }
                if (string.IsNullOrWhiteSpace(AppKey) || string.IsNullOrWhiteSpace(SecretKey))
                    throw new Exception("评分引擎启动失败.");
                try
                {
                    var enqine = new DefaultEngine(AppKey, SecretKey, ServerUri, ServerTimeout);
                    enqine.Create();
                    _engine = enqine;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return _engine;
        }

        /// <summary>
        ///删除引擎
        /// </summary>
        public static void DeleteEngine()
        {
            //try
            //{
            lock (EngineLock)
            {
                if (_engine == null)
                    return;
                var enqine = (DefaultEngine)_engine;
                enqine.Delete();
            }
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception("评分引擎销毁失败.");
            //}
        }
    }
}
