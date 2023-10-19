using UnityEngine;
public class ClipAnimationForBlend : ClipBase
{
	ClipAnimationForBlendData data;
	AnimationClip _clip;
	Vector3 pos;
	HumanPoseHandler humanPoseHandler;
	public override void Init<T, TT>(T _sourceObj, ClipType _clipType, float clipLength, TT _data)
	{
		sourceObj = _sourceObj;
		clipType = _clipType;
		length = clipLength;
		_clip = _sourceObj as AnimationClip;
		data = _data as ClipAnimationForBlendData;
		humanPoseHandler = new HumanPoseHandler(data.avatar, data.player);
	}

	public override HumanPose GetHumanPoseByNormalize(float normaizeTime)
	{
		return GetHumanPoseByTime(normaizeTime * length);
	}
	public override HumanPose GetHumanPoseByTime(float time)
	{
		HumanPose oldPos = new HumanPose();
		HumanPose newPos = new HumanPose();

		humanPoseHandler.GetHumanPose(ref oldPos);
		pos = data.player.localPosition;

		_clip.SampleAnimation(data.player.gameObject, time);
		humanPoseHandler.GetHumanPose(ref newPos);
		humanPoseHandler.SetHumanPose(ref oldPos);
		data.player.localPosition = pos;
		return newPos;
	}
	public override void Reset()
	{
	}
	public override void Play()
	{
		
	}


}
