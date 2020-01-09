using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace ST.Models.Api
{
    public class SyncScoreDetailItem
    {

        /// <summary>
        /// 351604
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 49161
        /// </summary>
        public string exam_attend_id { get; set; }
        /// <summary>
        /// 0
        /// </summary>
        public string topic_id { get; set; }
        /// <summary>
        /// 2
        /// </summary>
        public string topic_type { get; set; }
        /// <summary>
        /// 36040
        /// </summary>
        public string item_id { get; set; }
        /// <summary>
        /// 0.90
        /// </summary>
        public float exam_score { get; set; }
        /// <summary>
        /// records.17kouyu.com/5bc58ed430d8abae9e0001f0
        /// </summary>
        public string user_answer { get; set; }
        /// <summary>
        /// 3
        /// </summary>
        public string score_status { get; set; }
        /// <summary>
        /// {"recordId":"5bc58ed430d8abae9e0001f0","tokenId":"5bc58ed73327932be900000b","applicationId":"1511140684000046","audioUrl":"records.17kouyu.com/5bc58ed430d8abae9e0001f0","dtLastResponse":"2018-10-16 15:10:17:606","params":{"app":{"timestamp":"1539673815","sig":"d1bfc2262958186c20e7dcd2ccae9958cbf042fe","applicationId":"1511140684000046","userId":"userId","clientId":"aec1176aaaf06e25"},"request":{"coreType":"sent.eval","getParam":1,"tokenId":"5bc58ed73327932be900000b","refText":"What did he do?","precision":0.1,"agegroup":3,"paragraph_need_word_score":1,"mode":"school","scale":1,"attachAudioUrl":1,"phoneme_output":1,"slack":0,"dict_type":"KK"},"audio":{"sampleRate":16000,"channel":1,"sampleBytes":2,"compress":"speex","audioType":"ogg"}},"eof":1,"refText":"What did he do?","result":{"fluency":1,"words":[{"scores":{"prominence":0,"overall":0.8,"pronunciation":0.8},"charType":0,"word":"What","phonics":[{"overall":0.9,"phoneme":["w"],"spell":"Wh"},{"overall":1,"phoneme":["ɔ"],"spell":"a"},{"overall":0.7,"phoneme":["t"],"spell":"t"}],"span":{"end":206,"start":151},"phonemes":[{"span":{"end":174,"start":151},"phoneme":"w","pronunciation":0.9},{"span":{"end":186,"start":174},"phoneme":"ɔ","pronunciation":1},{"span":{"end":206,"start":186},"phoneme":"t","pronunciation":0.7}]},{"scores":{"prominence":0,"overall":0.9,"pronunciation":0.9},"charType":0,"word":"did","phonics":[{"overall":0.8,"phoneme":["d"],"spell":"d"},{"overall":0.9,"phoneme":["ɪ"],"spell":"i"},{"overall":0.9,"phoneme":["d"],"spell":"d"}],"span":{"end":233,"start":206},"phonemes":[{"span":{"end":215,"start":206},"phoneme":"d","pronunciation":0.8},{"span":{"end":224,"start":215},"phoneme":"ɪ","pronunciation":0.9},{"span":{"end":233,"start":224},"phoneme":"d","pronunciation":0.9}]},{"scores":{"prominence":0,"overall":1,"pronunciation":1},"charType":0,"word":"he","phonics":[{"overall":1,"phoneme":["h"],"spell":"h"},{"overall":1,"phoneme":["i"],"spell":"e"}],"span":{"end":282,"start":233},"phonemes":[{"span":{"end":265,"start":233},"phoneme":"h","pronunciation":1},{"span":{"end":282,"start":265},"phoneme":"i","pronunciation":1}]},{"scores":{"prominence":0,"overall":0.8,"pronunciation":0.8},"charType":0,"word":"do?","phonics":[{"overall":0.9,"phoneme":["d"],"spell":"d"},{"overall":0.8,"phoneme":["u"],"spell":"o"}],"span":{"end":327,"start":282},"phonemes":[{"span":{"end":297,"start":282},"phoneme":"d","pronunciation":0.9},{"span":{"end":327,"start":297},"phoneme":"u","pronunciation":0.8}]}],"duration":"4.220","kernel_version":"3.1.4","rear_tone":"fall","speed":136,"integrity":1,"pronunciation":0.9,"resource_version":"1.5.5","rhythm":0.6,"overall":0.9}}
        /// </summary>
        public string score_result { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string item_remark { get; set; }
        /// <summary>
        /// 10
        /// </summary>
        public string agent_id { get; set; }
        /// <summary>
        /// 1539673818
        /// </summary>
        public string created_at { get; set; }
        /// <summary>
        /// 1539673818
        /// </summary>
        public string updated_at { get; set; }
        /// <summary>
        /// 16
        /// </summary>
        public string qs_type { get; set; }
        /// <summary>
        /// 13562
        /// </summary>
        public string qs_id { get; set; }
        /// <summary>
        /// 2426
        /// </summary>
        public string exam_id { get; set; }
        /// <summary>
        /// 1.00
        /// </summary>
        public float item_score { get; set; }
        /// <summary>
        /// What did he do?
        /// </summary>
        public string item_answer { get; set; }
        /// <summary>
        /// 4511
        /// </summary>
        public string student_id { get; set; }
        /// <summary>
        /// 1
        /// </summary>
        public string item_no { get; set; }
        /// <summary>
        /// 0
        /// </summary>
        public string is_current { get; set; }

    }
}
