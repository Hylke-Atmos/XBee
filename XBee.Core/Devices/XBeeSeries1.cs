﻿using System;
using System.Threading;
using System.Threading.Tasks;
using XBee.Frames;
using XBee.Frames.AtCommands;

namespace XBee.Devices
{
    internal class XBeeSeries1 : XBeeNode
    {
        internal XBeeSeries1(XBeeController controller,
            HardwareVersion hardwareVersion = HardwareVersion.XBeeSeries1,
            NodeAddress address = null) : base(controller, hardwareVersion, address)
        {
        }

        /// <summary>
        ///     Gets a value that indicates whether this node is a coordinator node.
        /// </summary>
        /// <returns>True if this is a coordinator node</returns>
        public virtual async Task<bool> IsCoordinatorAsync()
        {
            var response =
                await ExecuteAtQueryAsync<CoordinatorEnableResponseData>(new CoordinatorEnableCommand())
                    .ConfigureAwait(false);

            if (response.EnableState == null)
            {
                throw new InvalidOperationException("No valid coordinator state returned.");
            }

            return response.EnableState.Value == CoordinatorEnableState.Coordinator;
        }

        /// <summary>
        ///     Sets a value indicating whether this node is a coordinator node.
        /// </summary>
        /// <param name="enable">True if this is a coordinator node</param>
        public virtual Task SetCoordinatorAsync(bool enable)
        {
            return ExecuteAtCommandAsync(new CoordinatorEnableCommand(enable));
        }

        /// <summary>
        ///     Gets flags indicating the configured sleep options for this node.
        /// </summary>
        public async Task<SleepOptions> GetSleepOptionsAsync()
        {
            var response = await ExecuteAtQueryAsync<SleepOptionsResponseData>(new SleepOptionsCommand())
                .ConfigureAwait(false);

            if (response.Options == null)
            {
                throw new InvalidOperationException("No valid sleep options returned.");
            }

            return response.Options.Value;
        }

        /// <summary>
        ///     Sets flags indicating sleep options for this node.
        /// </summary>
        /// <param name="options">Sleep options</param>
        public Task SetSleepOptionsAsync(SleepOptions options)
        {
            return ExecuteAtCommandAsync(new SleepOptionsCommand(options));
        }

        public override async Task TransmitDataAsync(byte[] data, CancellationToken cancellationToken,
            bool enableAck = true)
        {
            if (Address == null)
            {
                throw new InvalidOperationException("Can't send data to local device.");
            }

            var transmitRequest = new TxRequestFrame(Address.LongAddress, data);

            if (!enableAck)
            {
                transmitRequest.Options = TransmitOptions.DisableAck;
                await Controller.ExecuteAsync(transmitRequest, cancellationToken).ConfigureAwait(false);
            }
            else
            {
                var response = await Controller.ExecuteQueryAsync<TxStatusFrame>(transmitRequest, cancellationToken)
                    .ConfigureAwait(false);

                if (response.Status != DeliveryStatus.Success)
                {
                    throw new XBeeException($"Delivery failed with status code '{response.Status}'.");
                }
            }
        }

        public override Task TransmitDataAsync(byte[] data, bool enableAck = true)
        {
            return TransmitDataAsync(data, CancellationToken.None, enableAck);
        }
    }
}