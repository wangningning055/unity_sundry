public class ClipManager
{
	public static ClipBase GetClip<T, TT>(T sourceObj, ClipType clipType, float clipLength, TT sourceObjData)
	{
		if(clipType == ClipType.BVH)
		{
			ClipBVHForBlend bVHClipForBlend = new ClipBVHForBlend();
			bVHClipForBlend.Init(sourceObj, clipType, clipLength, sourceObjData);
			return bVHClipForBlend;
		}
			if(clipType == ClipType.AnimationClip)
		{
			ClipAnimationForBlend bVHClipForBlend = new ClipAnimationForBlend();
			bVHClipForBlend.Init(sourceObj, clipType, clipLength, sourceObjData);
			return bVHClipForBlend;
		}
			if(clipType == ClipType.Animator)
		{
			ClipAnimatorForBlend bVHClipForBlend = new ClipAnimatorForBlend();
			bVHClipForBlend.Init(sourceObj, clipType, clipLength, sourceObjData);
			return bVHClipForBlend;
		}
		return default;
	}
}