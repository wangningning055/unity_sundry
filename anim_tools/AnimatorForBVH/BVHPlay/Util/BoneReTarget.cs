using System.Collections.Generic;
using System;
using UnityEngine;
public class BoneReTarget : MonoBehaviour
{
	//衣服列表
	public GameObject[] suits;
	//角色根骨骼
	public Transform root;
	void Start()
	{
		ResetBone();
	}
	void ResetBone()
	{
		Dictionary<string, Transform> playerBones = new Dictionary<string, Transform>();
		foreach(Transform trans in root.GetComponentsInChildren<Transform>())
		{
			if(!playerBones.TryGetValue(trans.name, out Transform bone))
			playerBones.Add(trans.name, trans);
		}
		Action<GameObject> reSet =(obj) =>
		{
			SkinnedMeshRenderer[] mashs = obj.GetComponentsInChildren<SkinnedMeshRenderer>();
			foreach(SkinnedMeshRenderer suitMesh in mashs)
			{
				Transform[] boneList = new Transform[suitMesh.bones.Length];
				playerBones.TryGetValue(suitMesh.rootBone.name, out Transform bone);

				suitMesh.rootBone = bone;
				
				for(int i = 0; i < suitMesh.bones.Length; i++)
				{
					if(suitMesh.bones[i] != null)
					playerBones.TryGetValue(suitMesh.bones[i].name, out boneList[i]);
			
				}
				suitMesh.bones = boneList;
			}
		};

		foreach(GameObject obj in suits)
		{
			reSet(obj);
		}

		
	}	
 
}
