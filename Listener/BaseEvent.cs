using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public delegate void baseEvent();
public class BaseEvent
{
	EventType type;
	event baseEvent trigger;
	
	public void Regist(baseEvent act)
	{
		trigger += act;
	}

	public void UnRegist(baseEvent act)
	{
		trigger -= act;
	}
	public void Trigger()
	{
		trigger?.Invoke();
	}
}