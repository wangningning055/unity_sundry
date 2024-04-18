using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TestEvent;
public class ListenerManager
{
	private static ListenerManager instance;
	private Dictionary<TestEvent.EventType, BaseEvent> eventDic;
	public static ListenerManager Instance{
		get{
			if(instance == null)
			{
				instance = new ListenerManager();
				instance.Init();
			}
			return instance;
		}
	}
	public void Init()
	{
		eventDic = new Dictionary<TestEvent.EventType, BaseEvent>();
		foreach(TestEvent.EventType eventType in Enum.GetValues(typeof(TestEvent.EventType)))
		{
			eventDic.Add(eventType, new BaseEvent());
		}
	}

	public void RegistEvent(TestEvent.EventType type, baseEvent act)
	{
		if(eventDic.TryGetValue(type, out BaseEvent baseEvent))
		{
			baseEvent.Regist(act);
		}
	}
	public void UnRegistEvent(TestEvent.EventType type, baseEvent act)
	{
		if(eventDic.TryGetValue(type, out BaseEvent baseEvent))
		{
			baseEvent.UnRegist(act);
		}
	}
	public void TriggerEvent(TestEvent.EventType type)
	{
		if(eventDic.TryGetValue(type, out BaseEvent baseEvent))
		{
			baseEvent.Trigger();
		}
	}
	public void RegistAllEvent()
	{

	}
	public void UnRegistAllEvent()
	{
		
	}
}