﻿using System;
using BinarySerialization;

namespace XBee.Frames.AtCommands
{
    internal class SleepPeriodResponseData : AtCommandResponseFrameData
    {
        public ushort PeriodBase { get; set; }

        [Ignore]
        public TimeSpan Period => TimeSpan.FromMilliseconds(PeriodBase * SleepPeriodCommand.PeriodMultiplier);
    }
}
