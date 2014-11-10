using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CardWorkbench.Models
{
    /// <summary>
    /// 通道状态
    /// </summary>
    public class ChannelStatus
    {
        //开启状态
        public int bRun { get; set; }

        //同步模式错误数
        public int ErrorsInSyncPattern { get; set; }

        //同步模式是否超过16个错误
        public int bMoreThan16ErrorsInSyncPattern { get; set; }

        //输入是否反向
        public int bInputInverted { get; set; }

        //是否丢失输入时钟
        public int bLostInputClock { get; set; }

        //位滑是否修正
        public int bBitSlipCorrected { get; set; }

        //同步模式是否ok
        public int bSyncPatternOk { get; set; }

        //帧同步状态
        public enum MCFS_SYNC_STATUS { Search, Verify, Lock };
    }
}
