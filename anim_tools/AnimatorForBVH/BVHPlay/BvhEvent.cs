	
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml.Serialization;

public class BvhEvent{
	
	List<Action> eventList = new List<Action>();
	List<float> eventTimeList = new List<float>();
	public void AddEvent(float time, Action act)
	{
		if(eventTimeList.Count == 0)
		{
			eventList.Add(act);
			eventTimeList.Add(time);
			return;
		}
		for(int i = 0; i < eventTimeList.Count; i++)
		{
			if(eventTimeList[i] > time)
			{
				eventTimeList.Insert(i, time);
				eventList.Insert(i, act);
				return;
			}
			if(eventTimeList[i] == time)
			{
				eventList.RemoveAt(i);
				eventList.Insert(i, act);
				return;
			}
		}
		eventTimeList.Add(time);
		eventList.Add(act);

	}
	public bool ISEmpty()
	{
		return eventTimeList.Count == 0;
	}
	public void RemoveEvent(float time)
	{
		for(int i = 0; i < eventTimeList.Count; i++)
		{
			if(eventTimeList[i] == time)
			{
				eventTimeList.Remove(time);
				eventList.RemoveAt(i);
				return;
			}
		}
	}
	public float UpdateNextEventTime(float time, out int index)
	{
		index = -1;
		for(int i = 0; i < eventTimeList.Count; i++)
		{
			// Debug.Log(eventTimeList[i]);
		}
		for(int i = 0; i < eventTimeList.Count; i++)
		{

			if(eventTimeList[i] > time)
			{
				index = i;
				return eventTimeList[i];
			}
		}
		return -1;
	}
	public Action GetEventByIndex(int index)
	{
		return eventList[index];
	}
	public Action GetEventByTime(float time)
	{
		for(int i = 0; i < eventTimeList.Count; i++)
		{
			if(time == eventTimeList[i])
			{
				return eventList[i];
			}
		}
		return null;
	}
}
