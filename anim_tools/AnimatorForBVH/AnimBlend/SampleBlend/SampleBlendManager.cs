using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;
public enum BlendBVHType
{
	BvhToAnimator = 0,
	AnimatorToBvh = 1,
	AnimatorToBvhByParser = 2,
	// AnimatorToAnimator = 3,
	BvhToBvh = 4,
	BvhToBvhByParser = 5
}

public class SampleBlendManager{
	static SampleBlendManager _instance;
	List<AnimSampleBlend> blendList = new List<AnimSampleBlend>();
	public static SampleBlendManager Instance
	{
		get{
			if(_instance == null)
			{
				_instance = new SampleBlendManager();
				BVHUpdataManager.Instance.Init();
			}
			return _instance;
		}
	}

	public void RemoveList(AnimSampleBlend blend)
	{
		blendList.Remove(blend);
	}
	/// <summary>
	/// bvh混合bvh
	/// </summary>
	/// <param name="fromBvhIndex">原始bvhindex</param>
	/// <param name="toBvhIndex">目标bvhIndex</param>
	/// <param name="toTime">混合到目标bvh的时间位置</param>
	/// <param name="blendTime">混合花费时间</param>
	/// <param name="callBack">混合完成的回调，一般决定开关对应动画状态机并强制更新动画</param>
	public void BlendBvhToBvh(int fromBvhIndex, int toBvhIndex, float toTime, float blendTime, Action callBack)
	{
		AnimSampleBlend animSampleBlend = new AnimSampleBlend();
		
		animSampleBlend.Init(fromBvhIndex, toBvhIndex, toTime, blendTime, callBack);
		blendList.Add(animSampleBlend);
	}
	/// <summary>
	/// bvh混合到animator
	/// </summary>
	/// <param name="fromBvhIndex">原始bvhIndex</param>
	/// <param name="toAnimator">目标animator</param>
	/// <param name="stateName">目标状态名称</param>
	/// <param name="toTime">混合到目标动画的时间位置</param>
	/// <param name="blendTime">混合花费时间</param>
	/// <param name="callBack">混合完成的回调，一般决定开关对应动画状态机并强制更新动画</param>
	public void BlendBvhToAnimator(int fromBvhIndex, Animator toAnimator, string stateName, float toTime, float blendTime, Action callBack)
	{
		AnimSampleBlend animSampleBlend = new AnimSampleBlend();
		
		animSampleBlend.Init(fromBvhIndex, toAnimator, stateName, toTime, blendTime, callBack);
		blendList.Add(animSampleBlend);
	}
	/// <summary>
	/// animator混合到bvh
	/// </summary>
	/// <param name="fromAnimator">原始animator</param>
	/// <param name="toBvhIndex">目标bvhIndex</param>
	/// <param name="toTime">混合到的目标动画的时间位置</param>
	/// <param name="blendTime">混合花费时间</param>
	/// <param name="callBack">混合完成的回调，一般决定开关对应动画状态机并强制更新动画</param>
	public void BlendAnimatorToBvh(Animator fromAnimator, int toBvhIndex, float toTime, float blendTime, Action callBack)
	{
		AnimSampleBlend animSampleBlend = new AnimSampleBlend();
		animSampleBlend.Init(fromAnimator, toBvhIndex, toTime, blendTime, callBack);
		blendList.Add(animSampleBlend);
	}

	public void BeforBvhUpdate()
	{
		for(int i = 0; i < blendList.Count; i++)
		{
			blendList[i].BeforBvhUpdate();
		}
	}
	public void Update()
	{
		// if(blendList.Count > 1)
		// {
		// 	Debug.Log($"aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa   {blendList.Count}");
		// 	Debug.Log($"{blendList[0].animator.transform.parent.name},             {blendList[1].animator.transform.parent.name}"); 
		// }
		for(int i = 0; i < blendList.Count; i++)
		{
			blendList[i].Update();
		}
	}

}