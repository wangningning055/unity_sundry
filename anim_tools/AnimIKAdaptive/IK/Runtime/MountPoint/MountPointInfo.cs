using System;
using System.Collections.Generic;
using UnityEngine;
using vw_animation_ik_runtime;

namespace MountPoint
{
    [Serializable]
    public class MountPoint
    {
        public HandleTargetType Type;
        public Vector3 Position;
    }

    /// <summary>
    /// 挂点信息
    /// </summary>
    public class MountPointInfo : MonoBehaviour
    {
        [HideInInspector] public List<MountPoint> mountPoints = new List<MountPoint>(3);
    }
}
