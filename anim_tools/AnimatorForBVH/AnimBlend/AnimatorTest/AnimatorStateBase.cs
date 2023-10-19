using UnityEngine;

public class AnimatorStateBase
{
	public StateType stateType;
	public BlendAnimator animator;
	public int stateIndex;
	public float curNormalizeTime;
	public float curTime;
	public float lastStateNormalizeTime = 1;
	public float length;
	public AnimatorStateBase( BlendAnimator _animator)
	{
		animator = _animator;
	}
	public virtual HumanPose GetHumanPoseByNormalizeTime(float normalizeTime)
	{
		return new HumanPose();
	}
	public virtual HumanPose UpdateHumanPose()
	{
		return new HumanPose();
	}
	public virtual bool CheckIsEnd()
	{
		return true;
	}
	public virtual void Reset()
	{
		curNormalizeTime = 0;
		curTime = 0;
		lastStateNormalizeTime = 0;
	}
	public virtual AnimatorStateBase GetNextState()
	{
		return null;
	}
	public virtual void Play()
	{
		// tPoseData.SetTpose();
		// if(!isRootMotion)
		// {
		// 	humanPose.bodyPosition = avatarRoot.parent.localPosition;
		// }
		// humanPoseHandler.SetHumanPose(ref humanPose);
	}

}