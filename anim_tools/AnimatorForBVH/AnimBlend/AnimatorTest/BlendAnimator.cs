using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;
using System;
using UnityEngine.PlayerLoop;
using UnityEditorInternal;

public class BlendAnimator
{
	TPoseData tPoseData;
	public Transform player;
	public Avatar avatar;
	public Transform avatarRoot;
	public bool isRootMotion;
	Dictionary<int, StateBase> stateDic = new Dictionary<int, StateBase>();
	Dictionary<string, bool> conditionDic = new Dictionary<string, bool>();
	public bool isInit = false;
	AnimatorStateBase curState;
	StateBase startState;
	int totalStateIndex = -1;
	public bool isPlaying = false;
	HumanPoseHandler humanPoseHandler;
	public float cueStateNormalizeTime{
		get{
			return curState.curNormalizeTime;
		}
	}
	public BlendAnimator(Transform _player, Avatar _avatar, Transform _avatarRoot)
	{
		player = _player;
		tPoseData = player.GetComponent<TPoseData>();
		avatar = _avatar;
		avatarRoot = _avatarRoot;
		humanPoseHandler = new HumanPoseHandler(avatar, player.transform);

		isInit = true;
	}
	public void SetStartState(int stateIndex)
	{
		if(stateDic.TryGetValue(stateIndex, out StateBase stateBase))
		{
			startState = stateBase;
		}
	}
	public int AddState<T>(T clip)where T : ClipBase
	{
		StateBase stateBase = new StateBase(this);
		stateBase.SetClip(clip);
		totalStateIndex++;
		stateDic.Add(totalStateIndex, stateBase);
		stateBase.stateIndex = totalStateIndex;
		if(totalStateIndex == 0)
		startState = stateBase;
		return totalStateIndex;
	}
	public void RemoveState(int stateIndex)
	{
		stateDic.Remove(stateIndex);
	}
	public StateBase GetState(int index)
	{
		stateDic.TryGetValue(index, out StateBase stateBase);
		return stateBase;
	}
	public void SetTransition(StateBase from, StateBase to, float blendTime, bool isHasExitTime)
	{
		from.SetNextState(to, blendTime, isHasExitTime);
	}
	public void AddCondition(string name)
	{
		conditionDic.Add(name, false);
	}
	public void RemoveCondition(string name)
	{
		conditionDic.Remove(name);
	}
	public void SetCondition(string name, bool value)
	{
		conditionDic[name] = value;
	}
	public bool GetCondition(string name)
	{
		return conditionDic[name];
	}
	public bool CheckCondition(string name)
	{
		return conditionDic.TryGetValue(name, out bool val);
	}
	public void ChangeCurState(AnimatorStateBase animatorStateBase)
	{
		curState.Reset();
		curState = animatorStateBase;
	}
	public void ChangeCurState(int index)
	{
		curState.Reset();
		curState = stateDic[index];
	}
	public void JumpNextState()
	{
		curState.GetNextState().lastStateNormalizeTime = curState.curNormalizeTime;
		curState.Reset();
		curState = curState.GetNextState();
	}
	public void JumpState(int stateIndex, float blendTime, float targetTime, Action calllBack = null)
	{
		StateBase state = stateDic[stateIndex];
		// state.lastStateNormalizeTime = curState.curNormalizeTime;
		BlendBase tempBlend = new BlendBase(curState, state, this, calllBack);
		tempBlend.lastStateNormalizeTime = curState.curNormalizeTime;
		tempBlend.InitTimeData(blendTime, false, targetTime);
		curState = tempBlend;
	}
	public void Play()
	{
		isPlaying = true;
		if(curState == null && stateDic.Values.Count > 0)
		curState = startState;
	}

	public void Puse()
	{
		isPlaying = false;
	}

	public void Stop()
	{
		curState = startState;
		isPlaying = false;
		foreach(StateBase stateBase in stateDic.Values)
		{
			stateBase.Reset();
		}
	}
	public void SetAvataPos(HumanPose humanPose)
	{
		tPoseData.SetTpose();
		if(!isRootMotion)
		{
			humanPose.bodyPosition = avatarRoot.parent.localPosition;
		}
		humanPoseHandler.SetHumanPose(ref humanPose);
	
	}
	public void Updata()
	{
		if(isPlaying && curState != null)
		{
			if(curState.CheckIsEnd())
			{
				Debug.Log("change");
				JumpNextState();
			}
			// curState.Play();
			SetAvataPos(curState.UpdateHumanPose());
		}
	}
	public void Destory()
	{
		isPlaying = false;
	}
}