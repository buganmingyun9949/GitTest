using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace ST.Models.Api
{
    public class SyncScoreHistoryItem
    {
        /// <summary>
        /// Last
        /// </summary>
        public SyncScoreDetailItem last { get; set; }
        /// <summary>
        /// Best
        /// </summary>
        public SyncScoreDetailItem best { get; set; }
    }
}
