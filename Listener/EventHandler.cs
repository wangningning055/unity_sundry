using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EventHandler
{
	event baseEvent trigger;
	
	public baseEvent GetEvent(Action act)
	{
		if(trigger == null)
			trigger = () =>{act?.Invoke();};
		return trigger;
	}
}