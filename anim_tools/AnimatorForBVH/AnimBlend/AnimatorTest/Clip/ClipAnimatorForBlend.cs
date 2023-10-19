using UnityEngine;
public class ClipAnimatorForBlend : ClipBase
{
	ClipAnimatorForBlendData data;
	Animator _clip;
	Vector3 pos;
	HumanPoseHandler humanPoseHandler;
	public override void Init<T, TT>(T _sourceObj, ClipType _clipType, float clipLength, TT _data)
	{
		sourceObj = _sourceObj;
		clipType = _clipType;
		length = clipLength;
		_clip = _sourceObj as Animator;
		data = _data as ClipAnimatorForBlendData;
		humanPoseHandler = new HumanPoseHandler(data.avatar, data.player);
	}

	public override HumanPose GetHumanPoseByNormalize(float normaizeTime)
	{
		return GetHumanPoseByTime(normaizeTime * length);
	}
	public override HumanPose GetHumanPoseByTime(float time)
	{
		HumanPose oldPos = new HumanPose();
		float normaizeTime = time / _clip.runtimeAnimatorController.animationClips[0].length;
		
		_clip.Play(data.stateName, 0, normaizeTime);
		_clip.Update(normaizeTime);
		humanPoseHandler.GetHumanPose(ref oldPos);
		return oldPos;
	}
	public override void Reset()
	{
	}
	public override void Play()
	{
		// _clip.is
	}
}
