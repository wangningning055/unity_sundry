using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XLua;
public class LoadForAsset
{
	private static LoadForAsset asset;
	public static LoadForAsset Instance{
		get{
			if(asset == null)
			{
				asset = new LoadForAsset();
			}
			return asset;
		}
	}

	public T LoadImmdiatly<T>(string sourceName)where T: UnityEngine.Object
	{
		T obj = AssetDatabase.LoadAssetAtPath<T>(sourceName);
		if(typeof(T) == typeof(GameObject))
		{
			return GameObject.Instantiate(obj);
		}
		return obj;
	}
	public UnityEngine.Object LoadImmdiatly(string path, Type type, SourceType sourceType)
	{
		// UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath(GetPathBySourceType.GetPath(path, sourceType), type);
		UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath(path, type);
		if(type == typeof(GameObject))
		{
			return GameObject.Instantiate(obj);
		}
		return obj;
	}
	public GameObject LoadPrefab(string sourceName, SourceType type)
	{
		GameObject gameObject = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Raw/" + GetPathBySourceType.getDirectorName(type) + "/" + sourceName + ".prefab");
		return GameObject.Instantiate(gameObject);
	}
	public void LoadAsyc(string sourceName, Action callback)
	{

	}
}
