using UnityEngine;

public class AnimatorTest
{
	public Animator animator;
	public AnimationClip clip1, clip2;
	int bvhPlayerIndex1 = -1;
	int bvhPlayerIndex2 = -1;
	public BVHParser fileBvh1, fileBvh2;

	public Transform BvhRoot;
	public Transform Root;
	public int bvhIndex1, bvhIndex2, animIndex1, animIndex2;
	public BlendAnimator _blendAnimator;
	public void Start()
	{
		bvhPlayerIndex1 = BVHPlayerManager.Instance.InitPlayer(BvhRoot, false);
		bvhPlayerIndex2 = BVHPlayerManager.Instance.InitPlayer(BvhRoot, false);
		InitSampleAnimator();
		_blendAnimator.Play();
	}
	StateBase CreatBvh(BVHParser bVHParser, ref int index, int bvhIndex)
	{

		BvhClip bvhClip = BVHPlayerManager.Instance.GetPlayer(bvhIndex);
        ClipBvhForBlendData clipBvhForBlendData = new ClipBvhForBlendData
        {
            bVHParser = bVHParser,
            root = BvhRoot
        };
        ClipBVHForBlend clipBVHForBlend = (ClipBVHForBlend)ClipManager.GetClip(bvhClip, ClipType.BVH,bVHParser.frames * bVHParser.frameTime, clipBvhForBlendData);
		index = _blendAnimator.AddState(clipBVHForBlend);
		StateBase state1 = _blendAnimator.GetState(index);
		state1.isLoop = false;
		return state1;
	}
	StateBase CreatAnim(AnimationClip clip, ref int index)
	{
		ClipAnimationForBlendData clipAnimationForBlendData = new ClipAnimationForBlendData()
		{
			avatar = animator.avatar,
			player = animator.transform
		};
		ClipAnimationForBlend clipAnimationForBlend = (ClipAnimationForBlend)ClipManager.GetClip(clip, ClipType.AnimationClip, clip1.length, clipAnimationForBlendData);
		index = _blendAnimator.AddState(clipAnimationForBlend);
		StateBase state2 = _blendAnimator.GetState(index);
		state2.isLoop = false;
		return state2;
	}
	StateBase CreatUnityAnimtor(Animator clip, ref int index)
	{
		ClipAnimatorForBlendData clipAnimationForBlendData = new ClipAnimatorForBlendData()
		{
			avatar = animator.avatar,
			player = animator.transform,
			stateName = "New Animation"
		};
		ClipAnimatorForBlend clipAnimationForBlend = (ClipAnimatorForBlend)ClipManager.GetClip(clip, ClipType.Animator, clip1.length, clipAnimationForBlendData);
		index = _blendAnimator.AddState(clipAnimationForBlend);
		StateBase state2 = _blendAnimator.GetState(index);
		state2.isLoop = false;
		return state2;
	}
	void InitSampleAnimator()
	{
		_blendAnimator = BlendAnimatorManager.Instance.CreatAnimator(animator.transform, animator.avatar, BvhRoot);
		_blendAnimator.isRootMotion = false;
		StateBase state1 = CreatBvh(fileBvh1, ref bvhIndex1, bvhPlayerIndex1);	
		StateBase state2 = CreatBvh(fileBvh2, ref bvhIndex2, bvhPlayerIndex2);
		StateBase state3 = CreatUnityAnimtor(animator, ref animIndex1);
		// StateBase state4 = CreatUnityAnimtor(clip2, ref animIndex2);
		_blendAnimator.AddCondition("isAnim");
	}


	
}
