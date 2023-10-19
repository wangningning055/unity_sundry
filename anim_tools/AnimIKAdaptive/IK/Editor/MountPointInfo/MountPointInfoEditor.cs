// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using vw_animation_ik_runtime;

namespace MountPoint
{
    [CustomEditor(typeof(MountPointInfo))]
    public class MountPointInfoEditor : Editor
    {
        private MountPointInfo t;
        private static bool s_inEditor = false;

        private static GameObject[] s_prefabs = new GameObject[3];

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI(); //显示
            t = (MountPointInfo)target;

            if (GUILayout.Button("增加挂点"))
            {
                Save();
                AddMountPoint();
                Editor();
            }

            if (GUILayout.Button("进入编辑模式"))
            {
                Save();
                Editor();
            }

            if (GUILayout.Button("保存数据"))
            {
                Save();
            }
        }

        public void Save()
        {
            if (s_inEditor)
            {
                for (int i = 0; i < t.mountPoints.Count; i++)
                {
                    MountPoint mountPoint = t.mountPoints[i];
                    var prefab = s_prefabs[(int)mountPoint.Type];
                    if (prefab == null)
                    {
                        t.mountPoints.RemoveAt(i);
                        i -= 1;
                    }
                    else
                    {
                        mountPoint.Position = prefab.transform.localPosition;
                        DestroyImmediate(prefab);
                    }
                }

                s_inEditor = false;
            }
        }

        public void Editor()
        {
            if (!s_inEditor)
            {
                s_prefabs = new GameObject[3];
                if (t.mountPoints.Count != 0)
                {
                    for (int i = 0; i < t.mountPoints.Count; i++)
                    {
                        var data = t.mountPoints[i];
                        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        go.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                        go.name = data.Type.ToString();
                        int temp = (int)data.Type;
                        s_prefabs[temp] = go;
                        go.transform.SetParent(t.transform.gameObject.transform);
                        go.transform.localPosition = data.Position;
                    }
                }
                //没有被初始化的模型
                else
                {
                    for (int i = 0; i < 3; i++)
                    {
                        var mountPoint = new MountPoint();
                        mountPoint.Type = (HandleTargetType)i;
                        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        go.name = mountPoint.Type.ToString();
                        go.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                        s_prefabs[i] = go;
                        go.transform.SetParent(t.transform.gameObject.transform);
                        go.transform.localPosition = Vector3.zero;
                        t.mountPoints.Add(mountPoint);
                    }
                }

                s_inEditor = true;
            }
        }

        public void AddMountPoint()
        {
            Dictionary<int, bool> mapping = new Dictionary<int, bool>() { { 0, true }, { 1, true }, { 2, true } };

            for (int i = 0; i < t.mountPoints.Count; i++)
            {
                var data = t.mountPoints[i];
                mapping[(int)data.Type] = false;
            }

            foreach (var iter in mapping)
            {
                if (iter.Value)
                {
                    MountPoint mountPoint = new MountPoint();
                    mountPoint.Type = (HandleTargetType)iter.Key;
                    t.mountPoints.Add(mountPoint);
                    return;
                }
            }
        }
    }
}
