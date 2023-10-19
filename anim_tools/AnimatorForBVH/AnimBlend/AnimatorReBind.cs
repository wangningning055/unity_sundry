using UnityEngine;
using System.Collections.Generic;
public class AnimatorReBind
{
	static AnimatorReBind s_animatorReBind;
	Animator _animator;
	AnimationClipOverrides clipOverride;
	AnimatorOverrideController animatorOverrideController;
	public static AnimatorReBind Instance{
		get{
			if(s_animatorReBind == null)
			{
				s_animatorReBind = new AnimatorReBind();
			}
			return s_animatorReBind;
		}
	}
	public void ReBindFuc(Animator animator, AnimationClip clip, string replaceAnimName)
	{
		if(animator != _animator)
		{
			_animator = animator;
			animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
			animator.runtimeAnimatorController = animatorOverrideController;
			clipOverride = new AnimationClipOverrides(animatorOverrideController.overridesCount);
			animatorOverrideController.GetOverrides(clipOverride);

		}
		clipOverride[replaceAnimName] = clip;
		animatorOverrideController.ApplyOverrides(clipOverride);
		// Debug.Log($"{replaceAnimName},        {clip.length}");

	}
}

public class AnimationClipOverrides : List<KeyValuePair<AnimationClip, AnimationClip>>
{
	public AnimationClipOverrides(int capacity):base(capacity){}
	public AnimationClip this[string name]
	{
		get{return this.Find(x => x.Key.name.Equals(name)).Value;}
		set{
			int index = this.FindIndex(x => x.Key.name.Equals(name));
			if(index != -1)
				this[index] = new KeyValuePair<AnimationClip, AnimationClip>(this[index].Key, value);
		}
	}
}