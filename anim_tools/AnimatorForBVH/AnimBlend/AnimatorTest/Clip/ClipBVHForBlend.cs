using UnityEngine;
public class ClipBVHForBlend : ClipBase
{
	ClipBvhForBlendData data;
	BvhClipNormal _bvhClip;
	public override void Init<T, TT>(T _sourceObj, ClipType _clipType, float clipLength, TT _data)
	{
		sourceObj = _sourceObj;
		clipType = _clipType;
		length = clipLength;
		_bvhClip = sourceObj as BvhClipNormal;
		data = _data as ClipBvhForBlendData;
		_bvhClip.SetBVHByParser(data.bVHParser);
	}
	public override HumanPose GetHumanPoseByNormalize(float normaizeTime)
	{
		return _bvhClip.GetHumanPoseByNormalizeTime(normaizeTime);
	}
	public override HumanPose GetHumanPoseByTime(float time)
	{
		_bvhClip.PlayStepTime(time);
		return _bvhClip.GetHumanPose();
	}
	public override void Reset()
	{
		_bvhClip.Reset();
		_bvhClip.Stop();
	}
	public override void Play()
	{
		if(!_bvhClip.isPlaying)
		_bvhClip.Play();
	}

}
