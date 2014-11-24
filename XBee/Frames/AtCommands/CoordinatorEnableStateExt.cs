﻿namespace XBee.Frames.AtCommands
{
    public enum CoordinatorEnableStateExt : byte
    {
        StandardRouter = 0,
        IndirectMessageCoordinator = 1,
        NonRoutingModule = 2,
        NonRoutingCoordinator = 3,
        IndirectMessagePoller = 4,
        NonRoutingPoller = 6
    }
}
