using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using Framework.Engine.Models;
using Framework.Engine.Models.BaseParameters;
using Framework.Engine.Models.MetaParameters;
using Framework.Engine.Models.MetaParameters.answers;
using Framework.Engine.Models.MetaParameters.RefTexts;
using Newtonsoft.Json;

namespace Framework.Engine
{
    public abstract class BaseEngine : IEngine
    {
        #region Extents

        private const string EngineDll = "skegn.dll";

        [DllImport(EngineDll, CharSet = CharSet.Ansi, EntryPoint = "skegn_new")]
        internal static extern IntPtr skegn_new(string cfg);
        [DllImport(EngineDll, CharSet = CharSet.Ansi, EntryPoint = "skegn_delete")]
        internal static extern int skegn_delete(IntPtr engine);
        [DllImport(EngineDll, CharSet = CharSet.Ansi, EntryPoint = "skegn_start")]
        internal static extern int skegn_start(IntPtr engine, string param, byte[] id,
            [MarshalAs(UnmanagedType.FunctionPtr)]EngineDelegete.EngineCallback callback,
            IntPtr userdata);
        [DllImport(EngineDll, CharSet = CharSet.Ansi, EntryPoint = "skegn_stop")]
        internal static extern int skegn_stop(IntPtr engine);
        [DllImport(EngineDll, CharSet = CharSet.Ansi, EntryPoint = "skegn_feed")]
        internal static extern int skegn_feed(IntPtr engine, byte[] data, int size);
        [DllImport(EngineDll, CharSet = CharSet.Ansi, EntryPoint = "skegn_cancel")]
        internal static extern int skegn_cancel(IntPtr engine);
        [DllImport(EngineDll, CharSet = CharSet.Ansi, EntryPoint = "skegn_get_device_id")]
        internal static extern int skegn_get_device_id(byte[] id);
        [DllImport(EngineDll, CharSet = CharSet.Ansi, EntryPoint = "skegn_opt")]
        internal static extern int skegn_opt(IntPtr engine, int opt, byte[] data, int size);

        #endregion

        #region Property

        public const string LocalProvision = "skegn.provision";
        public const string DefaultUserId = "stkouyu";
        public const string AssetsDirectoryName = "assets";
        public const string AssetsFileName = "skegn.provision";
        public const string ResouceDirectoryName = "resources";

        protected string AppKey { get; set; }

        protected string SecretKey { get; set; }
        protected string ServerUri { get; set; }
        protected string ServerTimeout { get; set; }

        protected IntPtr EngineInstance = IntPtr.Zero;

        #endregion

        protected BaseEngine(string appKey, string secretKey, string serverUri, string serverTimeout)
        {
            AppKey = appKey;
            SecretKey = secretKey;
            ServerUri = serverUri;
            ServerTimeout = serverTimeout;
        }

        public virtual void Create()
        {
            //验证参数
            ValidateEngineParameters();

            //创建
            string cfg = "{\"appKey\":\"" + AppKey + "\",\"secretKey\":\"" + SecretKey + "\",\"provision\":\"" + LocalProvision + "\",\"cloud\":{\"server\":\""
                            + ServerUri + "\",\"serverList\":\"\",\"connectTimeout\":20,\"serverTimeout\":"+ ServerTimeout + "}}";

            if (EngineInstance == IntPtr.Zero)
            {
                EngineInstance = skegn_new(cfg);
                if (EngineInstance == IntPtr.Zero)
                {
                    throw new Exception("Engine new failed[errorMessage(" + cfg + ")");
                }
            }
        }

        private void ValidateEngineParameters()
        {
            var binPath = AppDomain.CurrentDomain.BaseDirectory;
            var dllFullPath = Path.Combine(binPath, EngineDll);
            if (!File.Exists(dllFullPath))
                throw new Exception("Can not find engine dll.");
            var assetsFileName = Path.Combine(binPath, AssetsDirectoryName, AssetsFileName);
            if (!File.Exists(assetsFileName))
            {
                //throw new Exception("Can not find aiengine.provision.");
            }
            var resourceDir = Path.Combine(binPath, ResouceDirectoryName);
            if (!Directory.Exists(resourceDir))
            {
                //throw new Exception("Can not find resource.");
            }
        }
        internal protected virtual int Delete()
        {
            return skegn_delete(EngineInstance);
        }

        public virtual int Start(BaseEngineStartUpParameter parameter, ref string recId, EngineDelegete.EngineCallback callback, IntPtr userdata, string userID = null)
        {
            var engineParameter = WrapParameter(parameter, userID);
            var param = JsonConvert.SerializeObject(engineParameter,
                Formatting.Indented,
                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            var id = new byte[64];
            var result = skegn_start(EngineInstance, param, id, callback, userdata);
            recId = Encoding.ASCII.GetString(id).Replace("\0", string.Empty);
            //Log4NetHelper.Info($"ID:{recId},param:{param}");
            recId = $"ID:{recId},param:{param}";
            return result;
        }

        public virtual int Stop()
        {
            return skegn_stop(EngineInstance);
        }

        public virtual int Feed(byte[] data)
        {
            if (data == null || data.Length < 1)
                return -1;
            return skegn_feed(EngineInstance, data, data.Length);
        }

        public virtual int Cancel()
        {
            return skegn_cancel(EngineInstance);
        }

        public virtual int Opt(int opt, ref string result)
        {
            var data = new byte[256];
            var rId = skegn_opt(EngineInstance, opt, data, data.Length);
            result = Encoding.ASCII.GetString(data).Replace("\0", string.Empty);
            return rId;
        }

        private InnerParameter WrapParameter(BaseEngineStartUpParameter innerParameter,string userID = null)
        {
            if (innerParameter == null)
                throw new Exception("Parameter is illegal.");
            var paramter = new InnerParameter()
            {
                app = new app()
                {
                    userId = string.IsNullOrEmpty(userID) ? DefaultUserId : userID,
                },
                audio = new audio()
                {
                    audioType = innerParameter.audioType, //"wav",
                    channel = 1,
                    sampleBytes = 2,
                    sampleRate = innerParameter.sampleRate, //16000,
                    quality = 8,
                    complexity = 2,
                    compress = innerParameter.compress //"raw"
                },
                coreProvideType = "cloud",

            };

            paramter.request = GetRecRequest(innerParameter);

            //if (innerParameter.EType == EngineType.OPEN)
            //{
            //    paramter.request = new recRequest
            //    {
            //        coreType = innerParameter.EType,
            //        refText = GetRefText(innerParameter),
            //        keywords = "",
            //        qClass = innerParameter.QsClass,
            //        qType = innerParameter.QsType
            //    };
            //}
            //else
            //{
            //    paramter.request = new commonRequest()
            //    {
            //        coreType = innerParameter.EType,
            //        precision = 0.5,
            //        rank = innerParameter.Rank,
            //        refText = GetRefText(innerParameter)
            //    };
            //}
            return paramter;
        }

        private BaseRequest GetRecRequest(BaseEngineStartUpParameter parameter)
        {
            BaseRequest rec = null;
            switch (parameter.EType)
            {
                case EngineType.PARA:
                case EngineType.OPEN:
                    {
                        var openParameter = (EngineStartUpQsOpenParameter)parameter;
                        rec = new recRequest()
                        {
                            coreType = openParameter.EType,
                            refText = openParameter.RefText,
                            keywords = openParameter.KeyWords,
                            qClass = openParameter.QsClass,
                            qType = openParameter.QsType,
                            precision = openParameter.Precision,
                            scale = parameter.Scale,
                            getParam = parameter.getParam,
                            attachAudioUrl = parameter.attachAudioUrl,
                            paragraph_need_word_score = parameter.paragraph_need_word_score,
                        };
                    }
                    break;
                case EngineType.WORD:
                    {
                        var openParameter = (EngineStartUpSingleParameter)parameter;
                        rec = new commonRequest()
                        {
                            coreType = openParameter.EType,
                            precision = openParameter.Precision,
                            scale = parameter.Scale,
                            refText = openParameter.RefText,
                            attachAudioUrl = parameter.attachAudioUrl,
                            getParam = parameter.getParam,
                        };
                    }
                    break;
                case EngineType.SENT:
                case EngineType.CHOICE:
                    {
                        var openParameter = (EngineStartUpSingleParameter)parameter;
                        rec = new commonRequest()
                        {
                            coreType = openParameter.EType,
                            precision = openParameter.Precision,
                            scale = parameter.Scale,
                            refText = openParameter.RefText,
                            attachAudioUrl = parameter.attachAudioUrl,
                            getParam = parameter.getParam,
                        };
                    }
                    break;
                default:
                    throw new Exception("Undefined");
            }
            return rec;
        }

        private object GetRefText(BaseEngineStartUpParameter parameter)
        {
            object refText = null;
            //switch (parameter.EType)
            //{
            //    case EngineType.OPEN:
            //        {
            //            var multiParameter = (EngineStartUpQsOpenParameter)parameter;

            //            refText = multiParameter.RefText;
            //            //((oesyrefText)refText).lm = multiParameter.RefText.Select(item => new oesyanswer()
            //            //{
            //            //    answer = 2,
            //            //    text = item
            //            //}).ToArray();
            //        }
            //        break;
            //    case EngineType.SENT:
            //        {
            //            var singleParameter = (EngineStartUpQsOpenParameter)parameter;
            //            refText = singleParameter.RefText;
            //        }
            //        break;
            //    case EngineType.CHOICE:
            //        {
            //            var singleParameter = (EngineStartUpQsOpenParameter)parameter;
            //            refText = singleParameter.RefText;
            //        }
            //        break;
            //    default:
            //        throw new Exception("Undefined");
            //}
            return refText;
        }
    }
}
