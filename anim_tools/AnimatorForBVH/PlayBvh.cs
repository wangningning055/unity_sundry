using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class PlayBvh : MonoBehaviour{

	public bool isInit = false;
	public int frame = 0;
	public float timing = 0;
	public BVHParser bvhParser;
	public bool isPlaying = false;
	public Dictionary<string, Transform> unityBoneDic = new Dictionary<string, Transform>();
	public Vector3 rootPosRecorder;
	public Transform prefabRoot;
	public Dictionary<string, Quaternion> oldRotate = new Dictionary<string, Quaternion>();
	public Dictionary<string, Vector3> worldPosForBvh = new Dictionary<string, Vector3>();
	public bool isOpenRootMotion = false;
	void Update()
	{
		if(isPlaying)
		{

			timing += Time.deltaTime;

			if(timing > bvhParser.frameTime * frame)
            {
                frame = (int)(timing / bvhParser.frameTime);
				if(frame >= bvhParser.frames)
				    Stop();
			}
			if(frame >= 0 && frame < bvhParser.frames)
			{
				ResetUnityBoneData();
				PlayPlayer(bvhParser.root, null);
			}
		}

	}
	public virtual void SetBVH(string fileStr)
	{
	}

	public virtual void Init(Transform playerRoot)
	{
		frame = 0;
		timing = 0;
		worldPosForBvh.Clear();
		unityBoneDic.Clear();
		prefabRoot = playerRoot;
		InitUnityBoneDic();
		isInit = true;
	}

	public virtual void Play()
	{
		isPlaying = true;
	}
	public virtual void Pause()
	{
		isPlaying = false;
	}
	public virtual void Stop()
	{
		isPlaying = false;
		frame = 0;
		timing = 0;
		PlayPlayer(bvhParser.root, null);
	}
	public virtual void PlayStepFow()
	{
		frame++;
		ResetUnityBoneData();
		PlayPlayer(bvhParser.root, null);
	}
	public virtual void PlayStepBeh()
	{
		frame--;
		ResetUnityBoneData();
		PlayPlayer(bvhParser.root, null);
	}
	public virtual  Dictionary<string, Vector3> GetWorldPosDic()
	{
		return worldPosForBvh;
	}
	public virtual  Dictionary<string, Transform> GetWorldPosObjDic()
	{
		return null;
	}
	public void GetUnityOldRoot()
	{
		rootPosRecorder = prefabRoot.localPosition ;

	}
	//重置unity模型的旋转以及位置
	public virtual void ResetUnityBoneData()
	{

	}
	public virtual void InitUnityBoneDic()
	{
		foreach(Transform child in prefabRoot.GetComponentsInChildren<Transform>())
		{
			if(!unityBoneDic.TryGetValue(child.name, out Transform obj))
			unityBoneDic.Add(child.name, child);
		}
	}
	//获取bvh初始姿态字典
	public virtual void CreateBVHPos(BVHParser.BVHBone bone,  BVHParser.BVHBone bvhParent, Transform parent)
	{

		Vector3 offset = new Vector3(-bone.offsetX, bone.offsetY, bone.offsetZ) / 100;
		if(bone == bvhParser.root)
		{offset.x = 0; offset.z = 0;}

		bone.worldPositionForPose = bvhParent == null? offset : offset + bvhParent.worldPositionForPose;
		bone.offset = offset;
		worldPosForBvh.Add(bone.name, bone.worldPositionForPose);

		if(bone.children.Count <=0 ) return;
		foreach(BVHParser.BVHBone child in bone.children)
		{
			CreateBVHPos(child, bone, null);
		}
	}
	//播放
	public virtual void PlayPlayer(BVHParser.BVHBone bvhBone, BVHParser.BVHBone bvhBoneParent)
	{

	}

}
