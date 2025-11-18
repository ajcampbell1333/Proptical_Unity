using UnityEngine;

namespace Proptical.VRPN
{
    /// <summary>
    /// Transform data received from VRPN server
    /// Contains position, rotation, and timestamp for a tracked rigid body
    /// </summary>
    [System.Serializable]
    public struct VRPNTransformData
    {
        /// <summary>Position in world space (X, Y, Z)</summary>
        public Vector3 Position;

        /// <summary>Rotation as quaternion (X, Y, Z, W)</summary>
        public Quaternion Rotation;

        /// <summary>Timestamp when this transform was received (in seconds since connection start)</summary>
        public float Timestamp;

        /// <summary>Default constructor</summary>
        public VRPNTransformData(Vector3 position, Quaternion rotation, float timestamp)
        {
            Position = position;
            Rotation = rotation;
            Timestamp = timestamp;
        }

        /// <summary>Check if transform data is valid</summary>
        public bool IsValid() => !float.IsNaN(Position.x) && !float.IsNaN(Rotation.x) && Quaternion.Dot(Rotation, Rotation) > 0.9f;
    }
}

