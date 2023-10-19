using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;
using System;
using UnityEngine.PlayerLoop;
using UnityEditorInternal;

public class BlendBase : AnimatorStateBase
{
	float targetNormalizeTime;
	bool isInit = false;
	public bool isHasExitTime;
	public float exitTime;
	bool isComplete = false;
	Action completeCallback;
	AnimatorStateBase fromState, toState;
	public BlendBase(AnimatorStateBase _fromState, AnimatorStateBase _toState, BlendAnimator blendAnimator, Action callBack = null):base(blendAnimator)
	{
		fromState = _fromState;
		toState = _toState;
		animator = blendAnimator;
		completeCallback = callBack;
	}
	public void InitTimeData(float _exitTime, bool _isHasExitTime, float _targetTime = 0)
	{
		isHasExitTime = _isHasExitTime;
		exitTime = _exitTime;
		isInit = true;
		targetNormalizeTime = _targetTime;
		length = exitTime;
	}
	
	
	public override AnimatorStateBase GetNextState()
	{
		return toState;
	}
	public override bool CheckIsEnd()
	{
		return isComplete;
	}
	public override void Reset()
	{
		curNormalizeTime = 0;
		curTime = 0;
		lastStateNormalizeTime = 0;
		isComplete = false;
	}
	HumanPose LerpHumanPos(HumanPose humanPose1, HumanPose humanPose2, float t)
	{
		HumanPose targetPos = humanPose1;
		
		targetPos.bodyPosition = Vector3.Lerp(humanPose1.bodyPosition, humanPose2.bodyPosition, t);
		targetPos.bodyRotation = Quaternion.Lerp(humanPose1.bodyRotation, humanPose2.bodyRotation, t);
		for(int i = 0; i < humanPose1.muscles.Length; i++)
		{
			targetPos.muscles[i] = Mathf.Lerp(humanPose1.muscles[i], humanPose2.muscles[i], t);
		}
		return targetPos;

	}
	public override HumanPose GetHumanPoseByNormalizeTime(float normaizeTime)
	{

		float fromNormalizeTime = lastStateNormalizeTime;
		float toNormalizeTime = targetNormalizeTime + curTime / toState.length;
		fromNormalizeTime = Mathf.Clamp01(fromNormalizeTime);
		toNormalizeTime = Mathf.Clamp01(toNormalizeTime);

		HumanPose fromPos = fromState.GetHumanPoseByNormalizeTime(fromNormalizeTime);
		HumanPose toPos = toState.GetHumanPoseByNormalizeTime(toNormalizeTime);


		normaizeTime = 1 - (Mathf.Cos(Mathf.Clamp01(normaizeTime) * Mathf.PI) + 1) / 2;

		return LerpHumanPos(fromPos, toPos, normaizeTime);
	}

	public override HumanPose UpdateHumanPose()
	{
		curTime += Time.deltaTime;
		curNormalizeTime = curTime / exitTime;
		float toNormalizeTime = targetNormalizeTime + curTime / toState.length;

		HumanPose targetPos = GetHumanPoseByNormalizeTime(curNormalizeTime);
		if(curNormalizeTime >= 1)
		{
			isComplete = true;
			toState.curNormalizeTime = toNormalizeTime;
			toState.curTime = toState.curNormalizeTime * toState.length;
			completeCallback?.Invoke();
		}
		return targetPos;
	}
	public override void Play()
	{
		Debug.Log("???");
		animator.SetAvataPos(UpdateHumanPose());
	}

}