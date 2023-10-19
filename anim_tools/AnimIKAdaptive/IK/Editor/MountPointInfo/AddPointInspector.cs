using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor.Animations;
using UnityEditor.SceneManagement;
using vw_animation_ik_runtime;

[CanEditMultipleObjects, CustomEditor(typeof(IkSampleTest))]
public class EditorForPoint:Editor
{
    public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		if(GUILayout.Button("生成交互物挂点"))
		{
			IkSampleTest test = target as IkSampleTest;
			// Debug.Log(test.DownTimeLine);
			test.GetData();
		}
		
	}
}