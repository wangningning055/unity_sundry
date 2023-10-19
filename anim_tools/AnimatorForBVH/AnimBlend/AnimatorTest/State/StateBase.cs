using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;
using System;
using UnityEngine.PlayerLoop;
using UnityEditorInternal;

public class StateBase :AnimatorStateBase
{
	public BlendBase blend;
	StateBase nextState;
	public bool isLoop;
	Dictionary<string, bool> conditionNameList = new Dictionary<string, bool>();
	public ClipBase clip;
	
	public bool isComplete = true;

    public StateBase(BlendAnimator _animator) : base(_animator)
    {
		animator = _animator;
    }

    public void SetClip<T>(T _clip) where T : ClipBase
	{
		clip = _clip;
		length = clip.length;
	}

	public void SetNextState(StateBase _nextState, float blendTime, bool isHasExitTime, float targetStateNormalizeTime = 0, Action callBack = null)
	{
		nextState = _nextState;
		BlendBase curblend = new BlendBase(this, nextState, animator, callBack);
		curblend.InitTimeData(blendTime, isHasExitTime, targetStateNormalizeTime);
		blend = curblend;
	}
	public void AddNextStateOverCondition(string name, bool targetBool)
	{
		if(animator.CheckCondition(name))
		{
			conditionNameList.Add(name, targetBool);
		}
	}

	public void RemoveNextStateOverCondition(string name)
	{
		conditionNameList.Remove(name);
	}

	bool CheckStateCondition()
	{
		bool con = true;
		foreach(string val in conditionNameList.Keys)
		{
			con = con && (animator.GetCondition(val) == conditionNameList[val]);
		}
		return con;
	}
	public override AnimatorStateBase GetNextState()
	{
		return blend;
	}
	public override bool CheckIsEnd()
	{
		if(blend == null)
		{
			return false;
		}
		if(!blend.isHasExitTime)
			return CheckStateCondition();
		else
			return CheckStateCondition() && isComplete;
	}
	public override void Reset()
	{
		curNormalizeTime = 0;
		curTime = 0;
		clip.Reset();
	}
	public override HumanPose GetHumanPoseByNormalizeTime(float normalizeTime)
	{
		normalizeTime = Mathf.Clamp01(normalizeTime);
		return clip.GetHumanPoseByNormalize(normalizeTime);
	}
	public override HumanPose UpdateHumanPose()
	{
		curTime += Time.deltaTime;
		curNormalizeTime = curTime / clip.length;
		if(curTime >= clip.length && isLoop){curTime = 0; isComplete = true;}
		curNormalizeTime = curTime / clip.length;
		return clip.GetHumanPoseByNormalize(curNormalizeTime);
	}
	public override void Play()
	{
		clip.Play();
	}

}