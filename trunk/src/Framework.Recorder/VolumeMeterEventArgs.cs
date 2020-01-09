using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Recorder
{
    public class VolumeMeterEventArgs : EventArgs
    {
        private float m_fMaxValue;
        public float MaxValue
        {
            get
            {
                return m_fMaxValue;
            }
            set
            {
                m_fMaxValue = value;
            }
        }

        private int m_iSecondsRecorded;
        public int SecondsRecorded
        {
            get
            {
                return m_iSecondsRecorded;
            }
            set
            {
                m_iSecondsRecorded = value;
            }
        }

        public VolumeMeterEventArgs(float maxValue, int secondsRecorded)
        {
            m_fMaxValue = maxValue;
            m_iSecondsRecorded = secondsRecorded;
        }
    }
}
