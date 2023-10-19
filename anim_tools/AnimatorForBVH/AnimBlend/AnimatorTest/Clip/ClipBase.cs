using UnityEngine;

public class ClipBase{
	public float length;
	protected ClipType clipType;
	protected object sourceObj;
	public virtual void Init<T, TT>(T _sourceObj, ClipType _clipType, float clipLength, TT clipData)
	{
		sourceObj = _sourceObj;
		clipType = _clipType;
		length = clipLength;
	}

	public virtual HumanPose GetHumanPoseByNormalize(float normaizeTime)
	{
		return new HumanPose();
	}
	public virtual HumanPose GetHumanPoseByTime(float time)
	{
		return new HumanPose();
	}
	public virtual void Reset()
	{

	}
	public virtual void Play()
	{
	}
}