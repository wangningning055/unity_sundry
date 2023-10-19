using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class BlendAnimatorManager
{
	static BlendAnimatorManager _instance;
	List<BlendAnimator> _blendAnimators;
	public static BlendAnimatorManager Instance{
		get{
			if(_instance == null)
			{
				_instance = new BlendAnimatorManager();
				UpdateInsert.Instance.InsertUpdate(_instance.Update, typeof(BlendAnimatorManager), InsertType.AfterDirectorUpdateAnimEnd);
				_instance._blendAnimators = new List<BlendAnimator>();

			}
			return _instance;
		}
	}
	public void GetBVHClip(BVHParser parser, Transform root)
	{
		BvhClip bvhClip = new BvhClip();
		// bvhClip.Init(root);
		// bvhClip.SetBVHByParser(parser);
		ClipManager.GetClip(bvhClip, ClipType.BVH, parser.frames * parser.frameTime, parser);
	}
	public BlendAnimator CreatAnimator(Transform transform, Avatar avatar, Transform avatarRoot)
	{
		BlendAnimator animator = new BlendAnimator(transform, avatar, avatarRoot);
		_blendAnimators.Add(animator);
		return animator;
	}
	public void RemoveAnimator(BlendAnimator blendAnimator)
	{
		blendAnimator.Destory();
		_blendAnimators.Remove(blendAnimator);
	}
	void Update()
	{
		foreach(BlendAnimator animator in _blendAnimators)
		{
			animator.Updata();
		}
	}
}