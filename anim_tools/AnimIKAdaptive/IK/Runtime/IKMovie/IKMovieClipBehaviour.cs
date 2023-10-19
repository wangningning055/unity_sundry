// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using vw_animation_ik_runtime;
[Serializable]
public class IKMovieClipBehaviour : PlayableBehaviour
{
	public ExposedReference<AnimationClip> Component;
	TimelineAsset _timelineAsset;
    public IKSampleData _iKSampleData;
	TrackAsset trackAsset;
    public AnimationClip animationClip;
	PlayableDirector direct;
	Animator _animator;
	// public Transform sit;
	public TrackBindingData preparedData;
	bool isInStart = false;
	SingleIKMono ik;
	static bool ikOpen = true;
	void GetAnimator(Playable playable)
	{
		if(direct == null)
		{
			direct = playable.GetGraph().GetResolver()as PlayableDirector;
		}


		// _timelineAsset = direct.playableAsset as TimelineAsset;
		// foreach(var asset in _timelineAsset.GetOutputTracks())
		// {
		// 	if(asset.name == "PlayerTrack")
		// 	{
		// 		_animator = direct.GetGenericBinding(asset) as Animator;
		// 	}
		// }
	}
	void GetSit(Playable playable)
	{
		if(direct == null)
		{
			direct = playable.GetGraph().GetResolver()as PlayableDirector;
		}
		foreach(var ass in direct.playableAsset.outputs)
		{
			if(ass.streamName == "IK Movie Clip Track")
			{
				// sit = direct.GetGenericBinding(ass.sourceObject) as Transform;
				preparedData = direct.GetGenericBinding(ass.sourceObject) as TrackBindingData;

			}
			
		}
	}

	public override void OnGraphStart(Playable playable)
	{
// local furniture = module.smartObjMgr.GetObject(action._param.enterArgs.itemId)
// 			local dataClass = CS.vw_animation_ik_runtime.TrackBindingData
// 			local clothBone = nil
// 			local interActivetrans = furniture:GetInteractObjByIndex(action._param.enterArgs.posIndex)
// 			local bindingData = CS.UnityEngine.ScriptableObject.CreateInstance(typeof(dataClass))
// 			bindingData:SetData(interActivetrans, clothBone);
//             director:SetGenericBinding(output.sourceObject, bindingData)
	}
	 public override void OnGraphStop(Playable playable)
	{
		if(!ikOpen) return;
		if(!isInStart) return;
		isInStart = false;
		// Debug.Log("结束TimeLine");
		IKSampleRunTimeMgr.Instance.RemoveIK(direct.gameObject.transform);

	}
	public override void OnBehaviourPlay(Playable playable, FrameData info)
	{
		if(!ikOpen) return;
		if(isInStart) return;
		isInStart = true;
		GetAnimator(playable);
		GetSit(playable);
		if(preparedData == null)
		{Debug.LogWarning("timeLine未设置座椅"); return;}
		if(_iKSampleData == null)
		{Debug.LogWarning("无设置数据"); return;}
		if(_iKSampleData._totalData == null || _iKSampleData._totalData.Length <= 0)
		{Debug.LogWarning("无设置数据"); return;}
		Debug.Log("??????????????????????AAAA");
		IKSampleRunTimeMgr.Instance.SetSit( direct.gameObject.transform, preparedData);
		int index = IKSampleRunTimeMgr.Instance.PlayIK(_iKSampleData, direct.gameObject.transform);
		ik = IKSampleRunTimeMgr.Instance.GetIK(index);
	}
	public override void OnBehaviourPause(Playable playable, FrameData info)
	{
	}
	public override void ProcessFrame(Playable playable, FrameData info, object playerData)
	{
		if(ik != null)
		{
			ik.SetIKPassedTimeTime(playable.GetTime());
		}
		
	}

	
}
