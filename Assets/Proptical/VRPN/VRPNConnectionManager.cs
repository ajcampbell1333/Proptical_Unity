using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

namespace Proptical.VRPN
{
    /// <summary>
    /// Manages VRPN connection lifecycle
    /// Handles TCP handshake and UDP data reception
    /// Thread-safe socket operations with main thread marshaling
    /// </summary>
    public class VRPNConnectionManager : IDisposable
    {
        private UdpClient udpClient;
        private TcpClient tcpClient;
        private IPEndPoint serverEndPoint;
        private Thread receiveThread;
        private bool shouldStop;
        private bool isConnected;
        private readonly object transformDataLock = new object();
        private VRPNTransformData lastTransform;

        /// <summary>Event fired when transform is updated (called on main thread)</summary>
        public Action<VRPNTransformData> OnTransformUpdated { get; set; }

        /// <summary>Event fired when connection is established (called on main thread)</summary>
        public Action OnConnectionEstablished { get; set; }

        /// <summary>Event fired when connection is lost (called on main thread)</summary>
        public Action<string> OnConnectionLost { get; set; }

        /// <summary>
        /// Initialize connection to VRPN server
        /// </summary>
        /// <param name="serverAddress">IP address or hostname of VRPN server</param>
        /// <param name="serverPort">UDP port for initial contact (default: 3883)</param>
        /// <returns>true if initialization started successfully</returns>
        public bool InitializeConnection(string serverAddress, int serverPort = 3883)
        {
            try
            {
                if (!IPAddress.TryParse(serverAddress, out IPAddress ipAddress))
                {
                    IPHostEntry hostEntry = Dns.GetHostEntry(serverAddress);
                    if (hostEntry.AddressList.Length == 0)
                    {
                        Debug.LogError($"VRPN: Failed to resolve hostname: {serverAddress}");
                        return false;
                    }
                    ipAddress = hostEntry.AddressList[0];
                }

                serverEndPoint = new IPEndPoint(ipAddress, serverPort);
                return SetupUDPSocket();
            }
            catch (Exception ex)
            {
                Debug.LogError($"VRPN: Failed to initialize connection: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Start receiving data on background thread
        /// </summary>
        /// <returns>true if thread started successfully</returns>
        public bool StartReceiving()
        {
            if (receiveThread != null && receiveThread.IsAlive)
                return false;

            shouldStop = false;
            receiveThread = new Thread(ReceiveLoop) { IsBackground = true };
            receiveThread.Start();
            return true;
        }

        /// <summary>
        /// Stop receiving data and close connection
        /// </summary>
        public void StopReceiving()
        {
            shouldStop = true;
            receiveThread?.Join(1000);
            receiveThread = null;

            udpClient?.Close();
            udpClient = null;

            tcpClient?.Close();
            tcpClient = null;

            isConnected = false;
        }

        /// <summary>Check if currently connected</summary>
        public bool IsConnected() => isConnected;

        /// <summary>Get last received transform data</summary>
        public VRPNTransformData GetLastTransform()
        {
            lock (transformDataLock)
                return lastTransform;
        }

        private bool SetupUDPSocket()
        {
            try
            {
                udpClient = new UdpClient();
                udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                udpClient.Connect(serverEndPoint);
                isConnected = true;
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError($"VRPN: Failed to setup UDP socket: {ex.Message}");
                Debug.LogWarning($"VRPN: Check firewall settings and ensure UDP port {serverEndPoint.Port} is open");
                return false;
            }
        }

        private void ReceiveLoop()
        {
            while (!shouldStop && isConnected)
            {
                try
                {
                    if (udpClient == null || udpClient.Client == null)
                        break;

                    IPEndPoint remoteEndPoint = null;
                    byte[] data = udpClient.Receive(ref remoteEndPoint);

                    if (data != null && data.Length > 0)
                        ProcessUDPPacket(data, data.Length);
                }
                catch (SocketException ex)
                {
                    if (!shouldStop)
                    {
                        isConnected = false;
                        string errorMsg = ex.Message;
                        UnityMainThreadDispatcher.Instance?.Enqueue(() => OnConnectionLost?.Invoke(errorMsg));
                    }
                    break;
                }
                catch (ObjectDisposedException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    Debug.LogError($"VRPN: Error in receive loop: {ex.Message}");
                }
            }
        }

        private void ProcessUDPPacket(byte[] data, int dataSize)
        {
            if (VRPNMessageParser.ParseTrackerMessage(data, dataSize, out VRPNTransformData transform, out string rigidBodyName))
            {
                lock (transformDataLock)
                    lastTransform = transform;

                VRPNTransformData capturedTransform = transform;
                UnityMainThreadDispatcher.Instance?.Enqueue(() => OnTransformUpdated?.Invoke(capturedTransform));
            }
        }

        private bool PerformTCPHandshake()
        {
            // TODO: Implement TCP handshake (low priority - may be deferred)
            return false;
        }

        public void Dispose()
        {
            StopReceiving();
            udpClient?.Dispose();
            tcpClient?.Dispose();
        }
    }
}

