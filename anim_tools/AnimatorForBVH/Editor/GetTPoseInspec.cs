
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TPoseData))]
public class GetTposeInspec : Editor
{
	override public void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		if(GUILayout.Button("GetTPos"))
		{
			TPoseData tPoseData = target as TPoseData;
			Transform root = tPoseData.FindRoot();
			if(root != null)
			GetTpose(root, tPoseData);
		}
		// if(GUILayout.Button("SetPos"))
		// {
		// 	Tesssss tesssss = target as Tesssss;
		// 	Animator animator = tesssss.GetComponent<Animator>();
		// 	if(animator == null || tesssss.root == null) return;
			
		// 	SetTpose(tesssss.root, tesssss);
		// }
	
	}

	public void GetTpose(Transform parent, TPoseData tPoseData)
	{
		Transform[] allTrans = parent.GetComponentsInChildren<Transform>();
		tPoseData.TposeRotDic = new Quaternion[allTrans.Length];
		tPoseData.TposeBoneLocalPos = new Vector3[allTrans.Length];
		for(int i = 0; i < allTrans.Length; i++)
		{
			tPoseData.TposeRotDic[i] = allTrans[i].rotation;
			tPoseData.TposeBoneLocalPos[i] = allTrans[i].localPosition;
		}
		EditorUtility.SetDirty(tPoseData);
	}
	
}