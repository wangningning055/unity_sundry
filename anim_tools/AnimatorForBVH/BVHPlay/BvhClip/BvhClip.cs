using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml.Serialization;

public class BvhClip{

	public bool isInit = false;
	public int frame = 0;
	public float timing = 0;
	public BVHParser bvhParser;
	public bool isPlaying = false;
	public Dictionary<string, Transform> unityBoneDic = new Dictionary<string, Transform>();
	public Vector3 rootPosRecorder;
	public Vector3 prefabPosRecorder;
	public Transform prefabRoot;
	public Dictionary<string, Quaternion> oldRotate = new Dictionary<string, Quaternion>();

	public Dictionary<string, Vector3> worldPosForBvh = new Dictionary<string, Vector3>();
	public bool isOpenRootMotion = false;
	// public Animator animator;
	public HumanPoseHandler humanPoseHandler;
	public Vector3 animatorLocalPos;
	public TPoseData tPoseData;
	int currentEventTimeIndex = -1;
	float currentEventTime = -1;
	public BvhEvent bvhEventCom;
	public virtual void Update()
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
			EventAct();

			if(frame >= 0 && frame < bvhParser.frames)
			{
				ResetPos();
				ResetUnityBonePos();
				PlayPlayer(bvhParser.root, null);
			}
		}
	}
	public virtual void SetBVH(string fileStr)
	{
	}

	public virtual void SetBVHByParser(BVHParser _parser)
	{

	}
	public void EventAct()
	{
		if(bvhEventCom.ISEmpty()) return;
		if(timing > currentEventTime)
		{
			if(currentEventTimeIndex != -1)
				bvhEventCom.GetEventByIndex(currentEventTimeIndex)?.Invoke();
			currentEventTime = bvhEventCom.UpdateNextEventTime(timing, out currentEventTimeIndex);
		}
	}

	public virtual void Init(Transform playerRoot)
	{
		frame = 0;
		timing = 0;
		worldPosForBvh.Clear();
		unityBoneDic.Clear();
		prefabRoot = playerRoot;
		FindTposeData(playerRoot);
		InitUnityBoneDic();
		humanPoseHandler = new HumanPoseHandler(tPoseData.avatar, tPoseData.transform);
		isInit = true;
		bvhEventCom = new BvhEvent();
	}
		//查找animator
	public void FindTposeData(Transform parent)
	{
		tPoseData = parent.GetComponent<TPoseData>();
		if(tPoseData != null || parent.parent == null)
		{
			return;
		}
		FindTposeData(parent.parent);
	}
	public virtual void Reset()
	{
		frame = 0;
		timing = 0;
		currentEventTime = -1;
		currentEventTimeIndex = -1;
		worldPosForBvh.Clear();
		tPoseData.SetTpose();
	}

    public float GetLength()
    {
        return bvhParser.GetLength();
    }

    public virtual void Play()
	{
		if(bvhParser == null){Debug.LogError("没有bvh解析数据，无法播放");return;}
		isPlaying = true;
	}
	public virtual void Play(float time = 0)
	{
		if(bvhParser == null){Debug.LogError("没有bvh解析数据，无法播放");return;}
		isPlaying = true;
		timing = time;
		frame = (int)Mathf.Ceil(timing / bvhParser.frameTime);
		if(frame > bvhParser.frames - 1) frame = bvhParser.frames - 1;
		currentEventTime = bvhEventCom.UpdateNextEventTime(timing, out currentEventTimeIndex);

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
		currentEventTime = -1;
		currentEventTimeIndex = -1;
		// tPoseData.SetTpose();
		// PlayPlayer(bvhParser.root, null);
	}
	public virtual void PlayStepFow()
	{
		frame++;
		timing = frame * bvhParser.frameTime;
		ResetUnityBonePos();
		PlayPlayer(bvhParser.root, null);
	}
	public virtual void PlayStepBeh()
	{
		frame--;
		timing = frame * bvhParser.frameTime;
		ResetUnityBonePos();
		PlayPlayer(bvhParser.root, null);
	}
	public virtual void PlayStepTime(float time)
	{
		timing = time;
		frame = (int)Mathf.Ceil(timing / bvhParser.frameTime);
		if(frame > bvhParser.frames - 1) frame = bvhParser.frames - 1;
		ResetUnityBonePos();
		PlayPlayer(bvhParser.root, null);
		currentEventTime = bvhEventCom.UpdateNextEventTime(timing, out currentEventTimeIndex);
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
		prefabPosRecorder = tPoseData.transform.localPosition;
	}
	//重置unity模型的位置
	public virtual void ResetUnityBonePos()
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
		// if(bone.isEnd){bone.endVec.x = -bone.endVec.x;  bone.endVec = bone.endVec + bone.worldPositionForPose;}
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


	//获取当前姿势，用于动画混合
	public Dictionary<Transform, Quaternion> GetCurrentPose()
	{
		Dictionary<Transform, Quaternion> currentRotate = new Dictionary<Transform, Quaternion>();
		foreach(Transform child in unityBoneDic.Values)
		{
			currentRotate.Add(child, child.rotation);
		}
		return currentRotate;
	}
	//设置当前姿势，用于动画混合
	public void SetCurrentPose(Dictionary<Transform, Quaternion> poseDic)
	{
		foreach(Transform child in unityBoneDic.Values)
		{
			child.rotation = poseDic[child];
		}
	}
	public void SetHumanPose(HumanPose humanPose)
	{
		HumanPose _humanPose = new HumanPose();
		_humanPose = humanPose;
		humanPoseHandler.SetHumanPose(ref _humanPose);
	}
	public HumanPose GetHumanPose()
	{
		ResetPos();
		ResetUnityBonePos();
		PlayPlayer(bvhParser.root, null);
		HumanPose humanPose = new HumanPose();
		humanPoseHandler.GetHumanPose(ref humanPose);
		return humanPose;
	}
	public float GetCurrentTime()
	{
		return timing;
	}

	public virtual void Destory()
	{
		ResetPos();
		// tPoseData.SetTpose();
		unityBoneDic.Clear();
		oldRotate.Clear();
		worldPosForBvh.Clear();
	}
	public virtual void SetPlayerPosToBvh(BVHParser.BVHBone bvhBone, BVHParser.BVHBone bvhBoneParent)
	{

	}
	public virtual void ResetPos()
	{
		tPoseData.SetTpose();
	}

}
