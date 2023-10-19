using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;

public class AnimSampleBlend{

	HumanPose startHumanPose = new HumanPose();
	HumanPose targetHumanPose = new HumanPose();
	HumanPoseHandler humanPoseHandler;
	public Animator animator;
	int bvhPlayerIndex, bvhIndex1, bvhIndex2;
	float fromeTime, toTime;
	BlendBVHType type;
	float targetTime;
	float duration = 1f;
	float totalTime = 0;
	bool isInit = false;
	bool isComplete = false;
	string stateName;
	Action completeCallBack;
	Action updateAc, completeUpdate;
	HumanPose animatorPos;
	bool isExcuteBefore = false;
	public void Init(int fromBvhIndex, int toBvhIndex, float _toTime, float blendTime, Action callback = null)
	{
		bvhIndex1 = fromBvhIndex;
		bvhIndex2 = toBvhIndex;
		// fromeTime = _fromTime;
		toTime = _toTime;
		duration = blendTime;
		BvhClip play1 = BVHPlayerManager.Instance.GetPlayer(bvhIndex1);
		BvhClip play2 = BVHPlayerManager.Instance.GetPlayer(bvhIndex2);
		// play1.Play(fromeTime);
		play2.Play(toTime);
		completeCallBack = callback;
		humanPoseHandler = play1.humanPoseHandler;
		isInit = true;
		updateAc = BvhToBvh;
		completeUpdate = () =>{play2.Update();};
	}
	public void Init(int fromBvhIndex, Animator toAnimator, string _stateNmae, float _toTime, float blendTime, Action callback = null)
	{
		bvhIndex1 = fromBvhIndex;
		animator = toAnimator;
		// fromeTime = _fromTime;
		toTime = _toTime;
		stateName = _stateNmae;
		duration = blendTime;
		BvhClip play1 = BVHPlayerManager.Instance.GetPlayer(bvhIndex1);
		// play1.Play(fromeTime);
		animator.Play(stateName, 0, 0);
		animator.Play(stateName, 0, toTime / animator.GetCurrentAnimatorClipInfo(0).Length);
		completeCallBack = callback;
		humanPoseHandler = play1.humanPoseHandler;
		isInit = true;
		updateAc = BvhToAnimator;
		completeUpdate = () =>{animator.Update(Time.deltaTime);};
		
	}

	public void Init(Animator fromAnimator, int toBvhIndex, float _toTime, float blendTime, Action callback = null)
	{
		bvhIndex1 = toBvhIndex;
		animator = fromAnimator;
		// fromeTime = _fromTime;
		toTime = _toTime;
		duration = blendTime;
		BvhClip play1 = BVHPlayerManager.Instance.GetPlayer(bvhIndex1);
		play1.Play(toTime);
		// animator.Play(stateName, 0, 0);
		// animator.Play(stateName, 0, fromeTime / animator.GetCurrentAnimatorClipInfo(0).Length);
		completeCallBack = callback;
		humanPoseHandler = play1.humanPoseHandler;
		isInit = true;
		updateAc = AnimatorToBvh;
		completeUpdate = () =>{play1.Update();};
	}

	public void BvhToBvh()
	{
		BvhClip play1 = BVHPlayerManager.Instance.GetPlayer(bvhIndex1);
		BvhClip play2 = BVHPlayerManager.Instance.GetPlayer(bvhIndex2);
		
		startHumanPose = play1.GetHumanPose();
		targetHumanPose = play2.GetHumanPose();
	}
	public void BvhToAnimator()
	{
		BvhClip play1 = BVHPlayerManager.Instance.GetPlayer(bvhIndex1);
		startHumanPose = play1.GetHumanPose();
		targetHumanPose = animatorPos;
	}
	public void AnimatorToBvh()
	{
		BvhClip play1 = BVHPlayerManager.Instance.GetPlayer(bvhIndex1);
		startHumanPose = animatorPos;
		targetHumanPose =  play1.GetHumanPose();
	}
	void SimulateAnimComplete()
	{
		completeCallBack?.Invoke();
		completeUpdate?.Invoke();
		SampleBlendManager.Instance.RemoveList(this);
	}
	void SimulateAnim(float normaizeTime)
	{
		if(normaizeTime >= 1 || !isExcuteBefore) return;
		HumanPose newPos = new HumanPose();
		newPos.bodyPosition = Vector3.Lerp(startHumanPose.bodyPosition, targetHumanPose.bodyPosition, normaizeTime);
		newPos.bodyRotation = Quaternion.Lerp(startHumanPose.bodyRotation, targetHumanPose.bodyRotation, normaizeTime);

		newPos.muscles = new float[startHumanPose.muscles.Length];

		for(int i = 0; i < startHumanPose.muscles.Length; i++)
		{
			float temp = Mathf.Lerp(startHumanPose.muscles[i], targetHumanPose.muscles[i], normaizeTime);
			newPos.muscles[i] = temp;
		}
		humanPoseHandler.SetHumanPose(ref newPos);
	
	}
	
	public void Update()
	{
		
		if(isInit && !isComplete)
		{
			totalTime += Time.deltaTime;
			if(totalTime >= duration)
			{
				isComplete = true;
				SimulateAnimComplete();
				return;
			}
			updateAc?.Invoke();
			SimulateAnim(1 - (Mathf.Cos(Mathf.Clamp01(totalTime / duration) * Mathf.PI) + 1) / 2);
		}
	}
	public void BeforBvhUpdate()
	{
		isExcuteBefore = true;
		if(animator != null)
		{
			humanPoseHandler.GetHumanPose(ref animatorPos);
		}
	}
}