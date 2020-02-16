using PcapDotNet.Packets;
using PcapDotNet.Packets.Ethernet;
using PcapDotNet.Packets.IpV4;
using PcapDotNet.Packets.Transport;
using PcapPacketModifier.Logic.Layers.Interfaces;

namespace PcapPacketModifier.Logic.Layers
{
    /// <summary>
    /// Mapper, changes values from one layer to another if this value is not default and is some user value
    /// Is used to automatically inject user values in intercepted packets, not to ask every time to modify same modules
    /// </summary>
    public class LayerExchanger : ILayerExchanger
    {
        /// <summary>
        /// Assigns user values from any supported layer to other layer, maps values
        /// </summary>
        /// <param name="layerToCopyFrom">Layer to copy from</param>
        /// <param name="layerToCopyTo">Layer to copy to</param>
        /// <returns>Modified layer with user values</returns>
        public T AssignUserValuesFromFilledLayerToOtherLayer<T>(T layerToCopyFrom, T layerToCopyTo) where T : Layer
        {
            switch (layerToCopyFrom)
            {
                case TcpLayer tcpLayer:
                    return AssignUserValuesFromFilledTcpLayerToOtherTcpLayer(layerToCopyFrom as TcpLayer, layerToCopyTo as TcpLayer) as T;
                case EthernetLayer ethernetLayer:
                    return AssignUserValuesFromFilledEthernetLayerToOtherEthernetLayer(layerToCopyFrom as EthernetLayer, layerToCopyTo as EthernetLayer) as T;
                case IpV4Layer ipV4Layer:
                    return AssignUserValuesFromFilledIpV4LayerToOtherIpV4Layer(layerToCopyFrom as IpV4Layer, layerToCopyTo as IpV4Layer) as T;
                case UdpLayer udpLayer:
                    return AssignUserValuesFromFilledUdpLayerToOtherUdpLayer(layerToCopyFrom as UdpLayer, layerToCopyTo as UdpLayer) as T;
                case PayloadLayer payloadLayer:
                    return AssignUserValuesFromFilledPayloadLayerToOtherPayloadLayer(layerToCopyFrom as PayloadLayer, layerToCopyTo as PayloadLayer) as T;
            }

            return null;
        }

        /// <summary>
        /// Assigns from prefilled tcp layer (with user values) to other tcp layer (assigns only user values, if they are not default)
        /// </summary>
        /// <param name="layerToCopyFrom">Tcp layer with user filled values</param>
        /// <param name="layerToCopyTo">Tcp layer to assign values to</param>
        /// <returns>Modified Tcp layer with newly assigned user values</returns>
        private Layer AssignUserValuesFromFilledTcpLayerToOtherTcpLayer(TcpLayer layerToCopyFrom, TcpLayer layerToCopyTo)
        {
            layerToCopyTo.AcknowledgmentNumber = (layerToCopyFrom.AcknowledgmentNumber != default) ? layerToCopyFrom.AcknowledgmentNumber : layerToCopyTo.AcknowledgmentNumber;
            layerToCopyTo.ControlBits = (layerToCopyFrom.ControlBits != default) ? layerToCopyFrom.ControlBits : layerToCopyTo.ControlBits;
            layerToCopyTo.SequenceNumber = (layerToCopyFrom.SequenceNumber != default) ? layerToCopyFrom.SequenceNumber : layerToCopyTo.SequenceNumber;
            layerToCopyTo.DestinationPort = (layerToCopyFrom.DestinationPort != default) ? layerToCopyFrom.DestinationPort : layerToCopyTo.DestinationPort;
            layerToCopyTo.SequenceNumber = (layerToCopyFrom.SequenceNumber != default) ? layerToCopyFrom.SequenceNumber : layerToCopyTo.SequenceNumber;
            layerToCopyTo.SourcePort = (layerToCopyFrom.SourcePort != default) ? layerToCopyFrom.SourcePort : layerToCopyTo.SourcePort;
            layerToCopyTo.UrgentPointer = (layerToCopyFrom.UrgentPointer != default) ? layerToCopyFrom.UrgentPointer : layerToCopyTo.UrgentPointer;
            layerToCopyTo.Window = (layerToCopyFrom.Window != default) ? layerToCopyFrom.Window : layerToCopyTo.Window;

            return layerToCopyTo;
        }

        /// <summary>
        /// Assigns from prefilled IpV4 layer (with user values) to other tcp layer (assigns only user values, if they are not default)
        /// </summary>
        /// <param name="layerToCopyFrom">IpV4 layer with user filled values</param>
        /// <param name="layerToCopyTo">IpV4 layer to assign values to</param>
        /// <returns>Modified IpV4 layer with newly assigned user values</returns>
        private Layer AssignUserValuesFromFilledIpV4LayerToOtherIpV4Layer(IpV4Layer layerToCopyFrom, IpV4Layer layerToCopyTo)
        {
            layerToCopyTo.CurrentDestination = (layerToCopyFrom.CurrentDestination != default) ? layerToCopyFrom.CurrentDestination : layerToCopyTo.CurrentDestination;
            layerToCopyTo.Fragmentation = (layerToCopyFrom.Fragmentation != default) ? layerToCopyFrom.Fragmentation : layerToCopyTo.Fragmentation;
            layerToCopyTo.HeaderChecksum = (layerToCopyFrom.HeaderChecksum != default) ? layerToCopyFrom.HeaderChecksum : layerToCopyTo.HeaderChecksum;
            layerToCopyTo.Identification = (layerToCopyFrom.Identification != default) ? layerToCopyFrom.Identification : layerToCopyTo.Identification;
            layerToCopyTo.Protocol = (layerToCopyFrom.Protocol != default) ? layerToCopyFrom.Protocol : layerToCopyTo.Protocol;
            layerToCopyTo.Source = (layerToCopyFrom.Source != default) ? layerToCopyFrom.Source : layerToCopyTo.Source;
            layerToCopyTo.Ttl = (layerToCopyFrom.Ttl != default) ? layerToCopyFrom.Ttl : layerToCopyTo.Ttl;
            layerToCopyTo.TypeOfService = (layerToCopyFrom.TypeOfService != default) ? layerToCopyFrom.TypeOfService : layerToCopyTo.TypeOfService;

            return layerToCopyTo;
        }

        /// <summary>
        /// Assigns from prefilled ethernet layer (with user values) to other tcp layer (assigns only user values, if they are not default)
        /// </summary>
        /// <param name="layerToCopyFrom">Ethernet layer with user filled values</param>
        /// <param name="layerToCopyTo">Ethernet layer to assign values to</param>
        /// <returns>Modified Ethernet layer with newly assigned user values</returns>
        private Layer AssignUserValuesFromFilledEthernetLayerToOtherEthernetLayer(EthernetLayer layerToCopyFrom, EthernetLayer layerToCopyTo)
        {
            layerToCopyTo.Destination = (layerToCopyFrom.Destination != default) ? layerToCopyFrom.Destination : layerToCopyTo.Destination;
            layerToCopyTo.EtherType = (layerToCopyFrom.EtherType != default) ? layerToCopyFrom.EtherType : layerToCopyTo.EtherType;
            layerToCopyTo.Source = (layerToCopyFrom.Source != default) ? layerToCopyFrom.Source : layerToCopyTo.Source;

            return layerToCopyTo;
        }

        /// <summary>
        /// Assigns from prefilled Udp layer (with user values) to other tcp layer (assigns only user values, if they are not default)
        /// </summary>
        /// <param name="layerToCopyFrom">Udp layer with user filled values</param>
        /// <param name="layerToCopyTo">Udp layer to assign values to</param>
        /// <returns>Modified Udp layer with newly assigned user values</returns>
        private Layer AssignUserValuesFromFilledUdpLayerToOtherUdpLayer(UdpLayer layerToCopyFrom, UdpLayer layerToCopyTo)
        {
            layerToCopyTo.CalculateChecksumValue = (layerToCopyFrom.CalculateChecksumValue != default) ? layerToCopyFrom.CalculateChecksumValue : layerToCopyTo.CalculateChecksumValue;
            layerToCopyTo.DestinationPort = (layerToCopyFrom.DestinationPort != default) ? layerToCopyFrom.DestinationPort : layerToCopyTo.DestinationPort;
            layerToCopyTo.SourcePort = (layerToCopyFrom.SourcePort != default) ? layerToCopyFrom.SourcePort : layerToCopyTo.SourcePort;

            return layerToCopyTo;
        }

        /// <summary>
        /// Assigns from prefilled payload layer (with user values) to other tcp layer (assigns only user values, if they are not default)
        /// </summary>
        /// <param name="layerToCopyFrom">Payload layer with user filled values</param>
        /// <param name="layerToCopyTo">Payload layer to assign values to</param>
        /// <returns>Modified Payload layer with newly assigned user values</returns>
        private Layer AssignUserValuesFromFilledPayloadLayerToOtherPayloadLayer(PayloadLayer layerToCopyFrom, PayloadLayer layerToCopyTo)
        {
            layerToCopyTo.Data = (layerToCopyFrom.Data != default) ? layerToCopyFrom.Data : layerToCopyTo.Data;

            return layerToCopyTo;
        }
    }
}
