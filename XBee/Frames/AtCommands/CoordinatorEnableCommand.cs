﻿using BinarySerialization;

namespace XBee.Frames.AtCommands
{
    public class CoordinatorEnableCommand : AtCommandFrameContent
    {
        public CoordinatorEnableCommand() : base("CE")
        {
        }

        public CoordinatorEnableCommand(bool enable) : this()
        {
            EnableState = enable ? CoordinatorEnableState.Coordinator : CoordinatorEnableState.EndDevice;
        }

        [Ignore]
        public CoordinatorEnableState? EnableState
        {
            get { return Parameter as CoordinatorEnableState?; }
            set { Parameter = value; }
        }
    }
}
