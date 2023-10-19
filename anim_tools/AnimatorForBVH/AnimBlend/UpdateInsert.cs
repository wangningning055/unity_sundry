using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;
public enum InsertType{
	EarlyUpdate = 0,
	AfterRunBehaviourFixUpdate = 1,
	AfterScriptUpdate = 2,
	AfterScriptLateUpdate = 3,
	BeforFixUpdateAnim = 4,
	BeforDirectorUpdateAnimBegin = 5,
	AfterDirectorUpdateAnimEnd = 7

}
public class UpdateInsert
{
	static UpdateInsert _instance;
	List<Type> _typeList = new List<Type>();
	public static UpdateInsert Instance
	{
		get{
			if(_instance == null)
			{
				_instance = new UpdateInsert();
				Application.quitting += _instance.RemoveAll;
			}
			return _instance;
		}
	}
	public void InsertUpdate(Action action, Type actionType, InsertType insertType)
	{
		PlayerLoopSystem playerLoop = PlayerLoop.GetCurrentPlayerLoop();


		PlayerLoopSystem insertUpdate = new PlayerLoopSystem()
		{
			type = actionType,
			updateDelegate = () =>
			{
				action?.Invoke();
			}
		};
		if(insertType == InsertType.EarlyUpdate)
		{
			int fixIndex = Array.FindIndex(playerLoop.subSystemList, (sub) => sub.type.Name == "EarlyUpdate");
			PlayerLoopSystem playerFixSystem = playerLoop.subSystemList[fixIndex];
			List<PlayerLoopSystem> playerFixLoopSubList = new List<PlayerLoopSystem>(playerFixSystem.subSystemList);
			// int fixSceIndex = playerFixLoopSubList.FindIndex(m => m.type.Name.Contains("ScriptRunBehaviourFixedUpdate"));
			playerFixLoopSubList.Add(insertUpdate);
			playerFixSystem.subSystemList = playerFixLoopSubList.ToArray();
			playerLoop.subSystemList[fixIndex] = playerFixSystem;
		}

		if(insertType == InsertType.AfterRunBehaviourFixUpdate)
		{
			int fixIndex = Array.FindIndex(playerLoop.subSystemList, (sub) => sub.type.Name == "FixedUpdate");
			PlayerLoopSystem playerFixSystem = playerLoop.subSystemList[fixIndex];
			List<PlayerLoopSystem> playerFixLoopSubList = new List<PlayerLoopSystem>(playerFixSystem.subSystemList);
			int fixSceIndex = playerFixLoopSubList.FindIndex(m => m.type.Name.Contains("ScriptRunBehaviourFixedUpdate"));
			playerFixLoopSubList.Insert(fixSceIndex + 1, insertUpdate);
			playerFixSystem.subSystemList = playerFixLoopSubList.ToArray();
			playerLoop.subSystemList[fixIndex] = playerFixSystem;
		}

		if(insertType == InsertType.AfterScriptUpdate)
		{
			int updIndex = Array.FindIndex(playerLoop.subSystemList, (sub) => sub.type.Name == "Update");
			PlayerLoopSystem updateLoop = playerLoop.subSystemList[updIndex];
			List<PlayerLoopSystem> updateSubLoop = new List<PlayerLoopSystem>(updateLoop.subSystemList);
			int updateSecIndex = updateSubLoop.FindIndex((m) => m.type.Name.Contains("ScriptRunDelayedDynamicFrameRate"));
			updateSubLoop.Insert(updateSecIndex, insertUpdate);
			updateLoop.subSystemList = updateSubLoop.ToArray();
			playerLoop.subSystemList[updIndex] = updateLoop;
		}
	
		if(insertType == InsertType.AfterScriptLateUpdate)
		{
			int lateIndex = Array.FindIndex(playerLoop.subSystemList, (sub) => sub.type.Name == "PostLateUpdate");
			PlayerLoopSystem lateLoop = playerLoop.subSystemList[lateIndex];
			List<PlayerLoopSystem> lateSubLoop = new List<PlayerLoopSystem>(lateLoop.subSystemList);
			int lateSecIndex = lateSubLoop.FindIndex((m) => m.type.Name.Contains("ScriptRunDelayedDynamicFrameRate"));
			lateSubLoop.Insert(lateSecIndex, insertUpdate);
			lateLoop.subSystemList = lateSubLoop.ToArray();
			playerLoop.subSystemList[lateIndex] = lateLoop;
		}
					// FixedUpdate,    LegacyFixedAnimationUpdate
		if(insertType == InsertType.BeforFixUpdateAnim)
		{
			int lateIndex = Array.FindIndex(playerLoop.subSystemList, (sub) => sub.type.Name == "FixedUpdate");
			PlayerLoopSystem lateLoop = playerLoop.subSystemList[lateIndex];
			List<PlayerLoopSystem> lateSubLoop = new List<PlayerLoopSystem>(lateLoop.subSystemList);
			int lateSecIndex = lateSubLoop.FindIndex((m) => m.type.Name.Contains("LegacyFixedAnimationUpdate"));
			lateSubLoop.Insert(lateSecIndex, insertUpdate);
			lateLoop.subSystemList = lateSubLoop.ToArray();
			playerLoop.subSystemList[lateIndex] = lateLoop;
		}
		if(insertType == InsertType.BeforDirectorUpdateAnimBegin)
		{
			int lateIndex = Array.FindIndex(playerLoop.subSystemList, (sub) => sub.type.Name == "PreLateUpdate");
			PlayerLoopSystem lateLoop = playerLoop.subSystemList[lateIndex];
			List<PlayerLoopSystem> lateSubLoop = new List<PlayerLoopSystem>(lateLoop.subSystemList);
			int lateSecIndex = lateSubLoop.FindIndex((m) => m.type.Name.Contains("DirectorUpdateAnimationBegin"));
			lateSubLoop.Insert(lateSecIndex, insertUpdate);
			lateLoop.subSystemList = lateSubLoop.ToArray();
			playerLoop.subSystemList[lateIndex] = lateLoop;
		}


		if(insertType == InsertType.AfterDirectorUpdateAnimEnd)
		{
			int lateIndex = Array.FindIndex(playerLoop.subSystemList, (sub) => sub.type.Name == "PreLateUpdate");
			PlayerLoopSystem lateLoop = playerLoop.subSystemList[lateIndex];
			List<PlayerLoopSystem> lateSubLoop = new List<PlayerLoopSystem>(lateLoop.subSystemList);
			int lateSecIndex = lateSubLoop.FindIndex((m) => m.type.Name.Contains("DirectorUpdateAnimationEnd"));
			lateSubLoop.Insert(lateSecIndex + 1, insertUpdate);
			lateLoop.subSystemList = lateSubLoop.ToArray();
			playerLoop.subSystemList[lateIndex] = lateLoop;
		}

		// foreach(PlayerLoopSystem loop in playerLoop.subSystemList)
		// {
		// 	foreach(PlayerLoopSystem subLoop in loop.subSystemList)
		// 	{
		// 		Debug.Log($"{loop.type.Name},    {subLoop.type.Name}");

		// 	}
		// }
		// 	Debug.Log("?????????????????????????_______________________________________________________");

		_typeList.Add(actionType);
		PlayerLoop.SetPlayerLoop(playerLoop);
	}

	public void RemoveUpdate(Type actionType)
	{
		Type subType = null;
		Type subsubType = null;
		PlayerLoopSystem playerLoop = PlayerLoop.GetCurrentPlayerLoop();
		foreach(PlayerLoopSystem subsLoop in playerLoop.subSystemList)
		{
			foreach(PlayerLoopSystem subSubloop in subsLoop.subSystemList)
			{
				if(subSubloop.type == actionType)
				{
					subType = subsLoop.type;
					subsubType = subSubloop.type;
					break;
				}
			}
		}
		if(subType == null || subsubType == null) return;
		int firIndex = Array.FindIndex(playerLoop.subSystemList, (s) => s.type == subType);
		PlayerLoopSystem subLoop = playerLoop.subSystemList[firIndex];
		List<PlayerLoopSystem> subsubList = new List<PlayerLoopSystem>(subLoop.subSystemList);
		int secIndex = subsubList.FindIndex((m) => m.type == subsubType);
		subsubList.RemoveAt(secIndex);
		subLoop.subSystemList = subsubList.ToArray();
		playerLoop.subSystemList[firIndex] = subLoop;

		// foreach(PlayerLoopSystem loop in playerLoop.subSystemList)
		// {
		// 	foreach(PlayerLoopSystem subsLoop in loop.subSystemList)
		// 	{
		// 		// FixedUpdate,    LegacyFixedAnimationUpdate
		// 		// PreLateUpdate,    DirectorUpdateAnimationBegin
		// 		// PreLateUpdate,    LegacyAnimationUpdate
		// 		// PreLateUpdate,    DirectorUpdateAnimationEnd
		// 		Debug.Log($"{loop.type.Name},    {subsLoop.type.Name}");

		// 	}
		// }
		PlayerLoop.SetPlayerLoop(playerLoop);
		_typeList.Remove(actionType);
		
	}
	void RemoveAll()
	{
		List<Type> temp = new List<Type>(_typeList);
		foreach(Type type in temp)
		{
			RemoveUpdate(type);
		}
	}

}