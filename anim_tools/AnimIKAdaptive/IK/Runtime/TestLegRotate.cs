// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using RootMotion.FinalIK;
// using System;
// using MountPoint;
// using Athena;
// using InverseKinematic.Foot;
// public class TestLegRotate:MonoBehaviour
// {
// 	public Transform[] trans;
// 	public Transform playerTrans, qunziTrans;
// 	public bool synchronous = false;
// 	public bool isNeedRotate = false;
// 	public void getBone()
// 	{
// 		SkinnedMeshRenderer sk = GetComponent<SkinnedMeshRenderer>();
// 		trans = sk.bones;
// 	}

// 	public void UpdatePos()
// 	{
// 		if(!synchronous || qunziTrans == null || qunziTrans.gameObject.activeSelf == false) return;
// 		foreach(Transform playerChild in playerTrans.GetComponentsInChildren<Transform>())
// 		{
// 			foreach(Transform qunziTransChild in playerTrans.GetComponentsInChildren<Transform>())
// 			{
// 				if(qunziTransChild.name == playerChild.name)
// 				{
// 					qunziTransChild.position = playerChild.position;
// 					qunziTransChild.rotation = playerChild.rotation;
// 				}
// 			}
// 		}
// 		// qunziTrans.position = playerTrans.position;
// 		// if(isNeedRotate)
// 		// qunziTrans.rotation = playerTrans.rotation;
// 	}
// }