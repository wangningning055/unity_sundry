using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class BvhClipNormal : BvhClip{
	
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
		frame = 0;
		timing = 0;
		bvhParser = new BVHParser(fileStr);
		worldPosForBvh.Clear();

		tPoseData.SetTpose();
		CreateBVHPos(bvhParser.root, null, null);
		SetPlayerPosToBvh(bvhParser.root, null);
		GetUnityBoneOldRotate();
		GetUnityOldRoot();
		tPoseData.SetTpose();

		// PlayPlayer(bvhParser.root, null);
		// isPlaying = true;
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
	public HumanPose GetHumanPoseByNormalizeTime(float normalizeTime)
	{
		tPoseData.SetTpose();
		float time = normalizeTime * bvhParser.frameTime * bvhParser.frames;
		PlayStepTime(time);
		return GetHumanPose();
	}
	// public override void Reset()
	// {
	// 	base.Reset();
	// 	frame = 0;
	// 	timing = 0;
	// 	worldPosForBvh.Clear();
	// 	tPoseData.SetTpose();
	// }

	public override void SetBVHByParser(BVHParser _parser)
	{
		if(!isInit) return;
		frame = 0;
		timing = 0;
		bvhParser = _parser;
		worldPosForBvh.Clear();

		tPoseData.SetTpose();
		CreateBVHPos(bvhParser.root, null, null);
		SetPlayerPosToBvh(bvhParser.root, null);
		GetUnityBoneOldRotate();
		GetUnityOldRoot();
		tPoseData.SetTpose();

		// PlayPlayer(bvhParser.root, null);
		// isPlaying = true;
	}

	

	//播放
	public override void PlayPlayer(BVHParser.BVHBone bvhBone, BVHParser.BVHBone bvhBoneParent)
	{
		Transform prefabBone = GetUnityBoneTemp(bvhBone);
		
		if(bvhBone == bvhParser.root)
		{

			if(isOpenRootMotion) prefabBone.position += bvhBone.worldPos[frame] - bvhBone.offset;
			// if(isOpenRootMotion) prefabBone.position += bvhBone.worldPos[frame] - bvhBone.offset;
		}
		// else
		// {
		// 	Vector3 parentWorldPos = bvhBoneParent.worldPositionForPose;
		// 	Quaternion parentRotate = bvhBoneParent.worldRotate[frame];
		// 	Vector3 offset = bvhBone.offset;
		// 	Vector3 worldPos = parentRotate * offset + parentWorldPos;
		// 	bvhBone.worldPositionForPose = worldPos;

		// 	worldPosForBvh.Remove(bvhBone.name);
		// 	worldPosForBvh.Add(bvhBone.name, worldPos);
		// }

		if(prefabBone == null || bvhBone == bvhParser.root){
			foreach(BVHParser.BVHBone child in bvhBone.children)
			{
				PlayPlayer(child, bvhBone);
			}
			return;
		}
		oldRotate.TryGetValue(BoneMapDic.Instance.boneNameMapping[bvhBone.name], out Quaternion oldQua);

		prefabBone.rotation = bvhBone.worldRotate[frame] * oldQua;

		foreach(BVHParser.BVHBone child in bvhBone.children)
		{
			PlayPlayer(child, bvhBone);
		}
	}

	//设置unityPrefab基础姿势
	public override void SetPlayerPosToBvh(BVHParser.BVHBone bvhBone, BVHParser.BVHBone bvhBoneParent)
	{
		worldPosForBvh.TryGetValue(bvhBone.name, out Vector3 bvhBoneWorldPos);
		Transform prefabBone = GetUnityBoneTemp(bvhBone);
	
		if(prefabBone == null || bvhBone == bvhParser.root){
			foreach(BVHParser.BVHBone child in bvhBone.children)
			{
				SetPlayerPosToBvh(child, bvhBone);
			}
			return;
		}
		Vector3 bvhTend = bvhBone.worldPositionForPose - bvhBoneParent.worldPositionForPose;
		Vector3 unitBoneTend = prefabBone.position - prefabBone.parent.position;
		Quaternion sampleRot = Quaternion.FromToRotation(unitBoneTend, bvhTend);

		if(BoneMapDic.Instance.special.TryGetValue(prefabBone.parent.name, out string selfName))
		{
			if(selfName == prefabBone.name)
			{
				prefabBone.parent.rotation = sampleRot * prefabBone.parent.rotation;
			}
		}
		else
		{
			prefabBone.parent.rotation = sampleRot * prefabBone.parent.rotation;
		}
		foreach(BVHParser.BVHBone child in bvhBone.children)
		{
			SetPlayerPosToBvh(child, bvhBone);
		}
	}

	Transform GetUnityBoneTemp(BVHParser.BVHBone bvhBone)
	{
		if(!BoneMapDic.Instance.boneNameMapping.TryGetValue(bvhBone.name, out string unityBoneName)) return null;
		if(unityBoneDic.TryGetValue(unityBoneName, out Transform child))
			return child;
		return null;
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