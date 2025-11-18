using UnityEngine;

namespace Proptical.VRPN
{
    /// <summary>
    /// VRPN Client
    /// MonoBehaviour for connecting to VRPN server and receiving tracking data
    /// 
    /// Network Configuration:
    /// - UDP is primary for real-time tracking data
    /// - TCP handshake is low priority (may be deferred)
    /// - If UDP connection fails, check firewall settings and ensure UDP port is open
    /// </summary>
    [AddComponentMenu("Proptical/VRPN Client")]
    public class VRPNClient : MonoBehaviour
    {
        [Tooltip("VRPN server IP address or hostname (e.g., 127.0.0.1 or 192.168.1.100)")]
        [SerializeField] private string serverAddress = "127.0.0.1";

        [Tooltip("VRPN server UDP port. Default is 3883. Ensure this port is open in your firewall.")]
        [SerializeField] private int serverPort = 3883;

        [Tooltip("Name of the rigid body to track. Leave empty to track all rigid bodies.")]
        [SerializeField] private string rigidBodyName = string.Empty;

        [Tooltip("Auto-connect on Start")]
        [SerializeField] private bool autoConnect = false;

        [Tooltip("Enable smooth interpolation of transforms")]
        [SerializeField] private bool smoothInterpolation = true;

        [Tooltip("Interpolation speed (0-1, higher = faster)")]
        [Range(0f, 1f)]
        [SerializeField] private float interpolationSpeed = 0.1f;

        private VRPNConnectionManager connectionManager;
        private VRPNTransformData currentTransform;

        /// <summary>Event fired when transform is updated</summary>
        public System.Action<VRPNTransformData> OnTransformUpdated;

        /// <summary>Event fired when connection is established</summary>
        public System.Action OnConnectionEstablished;

        /// <summary>Event fired when connection is lost</summary>
        public System.Action<string> OnConnectionLost;

        private void Start()
        {
            // Ensure UnityMainThreadDispatcher exists
            _ = UnityMainThreadDispatcher.Instance;

            if (autoConnect)
                ConnectToServer(serverAddress, serverPort, rigidBodyName);
        }

        private void Update()
        {
            if (connectionManager != null && connectionManager.IsConnected())
            {
                VRPNTransformData latestTransform = connectionManager.GetLastTransform();
                if (latestTransform.IsValid())
                {
                    if (smoothInterpolation)
                    {
                        currentTransform.Position = Vector3.Lerp(currentTransform.Position, latestTransform.Position, interpolationSpeed);
                        currentTransform.Rotation = Quaternion.Slerp(currentTransform.Rotation, latestTransform.Rotation, interpolationSpeed);
                        currentTransform.Timestamp = latestTransform.Timestamp;
                    }
                    else
                    {
                        currentTransform = latestTransform;
                    }

                    transform.SetPositionAndRotation(currentTransform.Position, currentTransform.Rotation);
                }
            }
        }

        private void OnDestroy()
        {
            DisconnectFromServer();
        }

        /// <summary>
        /// Connect to VRPN server
        /// Server Address: IP or hostname (e.g., 127.0.0.1)
        /// Server Port: UDP port (default 3883)
        /// If connection fails, check firewall settings and ensure UDP port is open.
        /// </summary>
        public void ConnectToServer(string inServerAddress = "127.0.0.1", int inServerPort = 3883, string inRigidBodyName = "")
        {
            DisconnectFromServer();

            serverAddress = inServerAddress;
            serverPort = inServerPort;
            rigidBodyName = inRigidBodyName;

            connectionManager = new VRPNConnectionManager();
            connectionManager.OnConnectionEstablished += HandleConnectionEstablished;
            connectionManager.OnConnectionLost += HandleConnectionLost;
            connectionManager.OnTransformUpdated += HandleTransformUpdated;

            if (!connectionManager.InitializeConnection(serverAddress, serverPort))
            {
                Debug.LogError($"VRPN: Failed to initialize connection to {serverAddress}:{serverPort}");
                Debug.LogWarning($"VRPN: Check firewall settings and ensure UDP port {serverPort} is open");
                connectionManager = null;
                return;
            }

            if (!connectionManager.StartReceiving())
            {
                Debug.LogError("VRPN: Failed to start receiving thread");
                connectionManager = null;
                return;
            }
        }

        /// <summary>Disconnect from VRPN server</summary>
        public void DisconnectFromServer()
        {
            if (connectionManager != null)
            {
                connectionManager.OnConnectionEstablished -= HandleConnectionEstablished;
                connectionManager.OnConnectionLost -= HandleConnectionLost;
                connectionManager.OnTransformUpdated -= HandleTransformUpdated;
                connectionManager.StopReceiving();
                connectionManager.Dispose();
                connectionManager = null;
            }
        }

        /// <summary>Check if currently connected to server</summary>
        public bool IsConnected() => connectionManager != null && connectionManager.IsConnected();

        /// <summary>Get last received transform data</summary>
        public VRPNTransformData GetLastTransform() => connectionManager?.GetLastTransform() ?? default;

        private void HandleConnectionEstablished()
        {
            OnConnectionEstablished?.Invoke();
        }

        private void HandleConnectionLost(string errorMessage)
        {
            Debug.LogWarning($"VRPN: Connection lost: {errorMessage}");
            OnConnectionLost?.Invoke(errorMessage);
        }

        private void HandleTransformUpdated(VRPNTransformData transform)
        {
            OnTransformUpdated?.Invoke(transform);
        }
    }
}

