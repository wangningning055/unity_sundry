using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using XLua;

[LuaCallCSharp]
public static class LuaUtils
{
	public static GameObject FindGameObject(Transform transform, string path)
	{
		try
		{
			return transform.Find(path).gameObject;
		}
		catch(Exception exp)
		{
			Debug.LogException(exp);
			return null;
		}
	}

	public static Text FindText(Transform transform, string path)
	{
		try
		{
			return transform.Find(path).GetComponent<Text>();
		}
		catch(Exception exp)
		{
			Debug.LogException(exp);
			return null;
		}
	}
	public static Button FindButton(Transform transform, string path)
	{
		try
		{
			return transform.Find(path).GetComponent<Button>();
		}
		catch(Exception exp)
		{
			Debug.LogException(exp);
			return null;
		}
	}
	public static Image FindImage(Transform transform, string path)
	{
		try
		{
			return transform.Find(path).GetComponent<Image>();
		}
		catch(Exception exp)
		{
			Debug.LogException(exp);
			return null;
		}
	}
}