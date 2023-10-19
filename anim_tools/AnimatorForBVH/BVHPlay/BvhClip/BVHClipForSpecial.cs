using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class BVHClipForSpecial : BvhClip{

	Dictionary<string, Vector3> basePosDic = new Dictionary<string, Vector3>();
	public override void Update()
	{
		if(isPlaying)
		{
			timing += Time.deltaTime;
			if(timing > bvhParser.frameTime * frame)
			{
				frame++;
				if(frame >= bvhParser.frames)
				Stop();
			}
			if(frame >= 0 && frame < bvhParser.frames)
			{
				ResetPos();
				ResetUnityBonePos();
				PlayPlayer(bvhParser.root, null);
			}
		}
	}
	// public void SetHumanPose(HumanPose humanPose)
	// {
	// 	HumanPose _humanPose = new HumanPose();
	// 	_humanPose = humanPose;
	// 	humanPoseHandler.SetHumanPose(ref _humanPose); 
	// }
	// public HumanPose GetHumanPose()
	// {
	// 	HumanPose humanPose = new HumanPose();
	// 	humanPoseHandler.GetHumanPose(ref humanPose);
	// 	return humanPose;
	// }
	// public override void Init(Transform playerRoot)
	// {
	// 	frame = 0;
	// 	timing = 0;
	// 	worldPosForBvh.Clear();
	// 	unityBoneDic.Clear();
	// 	prefabRoot = playerRoot;
	// 	FindTposeData(playerRoot);
	// 	InitUnityBoneDic();
	// 	humanPoseHandler = new HumanPoseHandler(tPoseData.avatar, tPoseData.transform);
	// 	isInit = true;
	// }


	
	public override void SetBVH(string fileStr)
	{
		if(!isInit) return;
		bvhParser = new BVHParser(fileStr);
		worldPosForBvh.Clear();
		tPoseData.SetTpose();

		CreateBVHPos(bvhParser.root, null, null);
		SetPlayerPosToBvh(bvhParser.root, null);
		GetUnityBoneOldRotate();
		
		GetUnityOldRoot();
		PlayPlayer(bvhParser.root, null);
		// isPlaying = true;
	}
	//设置unityPrefab基础姿势
	public override void SetPlayerPosToBvh(BVHParser.BVHBone bvhBone, BVHParser.BVHBone bvhBoneParent)
	{
		if(!unityBoneDic.TryGetValue(bvhBone.name, out Transform pre))
		{
			return;
		}
		// Transform prefabBone = unityBoneDic[bvhBone.name];
	
		// if(prefabBone == null || bvhBone == bvhParser.root){
		// 	foreach(BVHParser.BVHBone child in bvhBone.children)
		// 	{
		// 		SetPlayerPosToBvh(child, bvhBone);
		// 	}
		// 	return;
		// }
		// Vector3 unitBoneTend = prefabBone.position - prefabBone.parent.position;
		
		// 	Vector3 bvhTend = bvhBone.worldPositionForPose - bvhBoneParent.worldPositionForPose;
		// 	Quaternion sampleRot = Quaternion.FromToRotation(unitBoneTend, bvhTend);
		// 	prefabBone.parent.rotation = sampleRot * prefabBone.parent.rotation;
		// 	// if(bvhBone.isEnd)
		// 	// {
		// 	// 	bvhBone.endVec.x = -bvhBone.endVec.x;
		// 	// 	Vector3 endWorldPos = bvhBone.endVec + bvhBone.worldPositionForPose;
		// 	// 	Quaternion selfsampleRot = Quaternion.FromToRotation(endWorldPos - bvhBone.worldPositionForPose, unitBoneTend);
		// 	// 	prefabBone.rotation = selfsampleRot * prefabBone.rotation;
		// 	// 	return; 
		// 	// }
		// 	foreach(BVHParser.BVHBone child in bvhBone.children)
		// 	{
		// 		SetPlayerPosToBvh(child, bvhBone);
		// 	}

	}
	//播放
	public override void PlayPlayer(BVHParser.BVHBone bvhBone, BVHParser.BVHBone bvhBoneParent)
	{
		if(!unityBoneDic.TryGetValue(bvhBone.name, out Transform pre))
		{
			// Debug.Log(bvhBone.name);
			return;
		}
		Transform prefabBone = unityBoneDic[bvhBone.name];
		
		if(bvhBone == bvhParser.root)
		{
			if(isOpenRootMotion) prefabBone.position += bvhBone.worldPos[frame] - bvhBone.offset;
		}
		else
		{
			Vector3 parentWorldPos = bvhBoneParent.worldPositionForPose;
			Quaternion parentRotate = bvhBoneParent.worldRotate[frame];
			Vector3 offset = bvhBone.offset;
			Vector3 worldPos = parentRotate * offset + parentWorldPos;
			bvhBone.worldPositionForPose = worldPos;

			worldPosForBvh.Remove(bvhBone.name);
			worldPosForBvh.Add(bvhBone.name, worldPos);
		}

		if(bvhBone == bvhParser.root){
			foreach(BVHParser.BVHBone child in bvhBone.children)
			{
				PlayPlayer(child, bvhBone);
			}
			return;
		}
	
		prefabBone.rotation = bvhBone.worldRotate[frame] * oldRotate[bvhBone.name];
		// prefabBone.rotation = bvhBone.worldRotate[frame];
 
		prefabBone.position += bvhBone.worldPos[frame] - bvhBone.offset;


		foreach(BVHParser.BVHBone child in bvhBone.children)
		{
			PlayPlayer(child, bvhBone);
		}
	}

	//记录unity模型处于bvh初始姿态下的旋转
	void GetUnityBoneOldRotate()
	{
		if(oldRotate.Keys.Count > 0) return;
		foreach(Transform child in unityBoneDic.Values)
		{
			if(!oldRotate.TryGetValue(child.name, out Quaternion rot))
			oldRotate.Add(child.name, child.rotation);
		}
		if(basePosDic.Keys.Count > 0) return;
		foreach(Transform child in unityBoneDic.Values)
		{
			if(!basePosDic.TryGetValue(child.name, out Vector3 pos))
			basePosDic.Add(child.name, child.localPosition);
		}
	}
	//重置unity模型的位置
	public override void ResetUnityBonePos()
	{
		prefabRoot.localPosition = rootPosRecorder;
		tPoseData.transform.localPosition = prefabPosRecorder;
	}
	public override void ResetPos()
	{
		tPoseData.SetTpose();
	}
}