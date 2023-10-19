using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;
using System;
using UnityEngine.PlayerLoop;

public class BVHPlayerManager
{
	Dictionary<int, BvhClip> playerDic = new Dictionary<int, BvhClip>();
	static BVHPlayerManager _bVHPlayer;
	int playerIndex = 0;
	public static BVHPlayerManager Instance
	{
		get{
			if(_bVHPlayer == null)
			{
				_bVHPlayer = new BVHPlayerManager();
				BVHUpdataManager.Instance.Init();
			}
			return _bVHPlayer;
		}
	}
	// public TPoseData GetTposeData(Transform parent)
	// {
	// 	TPoseData poseData = parent.GetComponent<TPoseData>();
	// 	if(poseData != null || parent.parent == null)
	// 		return poseData;
	// 	return GetTposeData(parent.parent);
	// }
	public int InitPlayer(Transform playerRoot, bool isSpecial)
	{
		BvhClip player;
		if(isSpecial)
		{
			player = new BVHClipForSpecial();
		}
		else
		{
			player = new BvhClipNormal();
		}
		if(playerRoot == null) {Debug.LogError("没有Root"); return -1;}
		// TPoseData tPosData = GetTposeData(playerRoot);
		// if(tPosData == null){Debug.LogError("没有Tpose数据组件"); return -1;}
		player.Init(playerRoot);
		playerIndex++;
		playerDic.Add(playerIndex, player);
		Debug.Log("initBVH");
		return playerIndex;
	}

	public void SetBVHStr(string fileStr, int index)
	{
		if(playerDic.TryGetValue(index, out BvhClip playBvh))
		{
			playBvh.SetBVH(fileStr);
		}
	}

	public void RemovePlayer(int index)
	{
		if(playerDic.TryGetValue(index, out BvhClip playBvh))
		{
			playBvh.Destory();
			playerDic.Remove(index);
		}
	}

	public BvhClip GetPlayer(int index)
	{
		if(playerDic.TryGetValue(index, out BvhClip playBvh))
			return playBvh;
		return null;
	}
	public void Play(int index)
	{
		if(playerDic.TryGetValue(index, out BvhClip playBvh))
			playBvh.Play();
	
	}

	public void Pause(int index)
	{
		if(playerDic.TryGetValue(index, out BvhClip playBvh))
			playBvh.Pause();
	}

	public void Stop(int index)
	{
		if(playerDic.TryGetValue(index, out BvhClip playBvh))
			playBvh.Stop();
	}
	public void Fow(int index)
	{
		if(playerDic.TryGetValue(index, out BvhClip playBvh))
			playBvh.PlayStepFow();
	}
	public void Beh(int index)
	{
		if(playerDic.TryGetValue(index, out BvhClip playBvh))
			playBvh.PlayStepBeh();
	}

	public void Update()
	{
		foreach(BvhClip bvh in playerDic.Values)
		{
			bvh.Update();
		}
	}
	
}
