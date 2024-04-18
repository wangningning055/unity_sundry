using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using XLua;

public class LoadMgr
{
	private static LoadMgr loadMgr;
	public static LoadMgr Instance
	{
		get{
			if(loadMgr == null)
			{
				loadMgr = new LoadMgr();
			}
			return loadMgr;
		}
	}
	Dictionary<string, AssetData> assetDatas;
	bool isLoadForAB = false;
	public void Init()
	{
		LoadAssetData();
	}

	void LoadAssetData()
	{
		assetDatas = new Dictionary<string, AssetData>();
		AssetDataList assetDataList = JsonUtility.FromJson<AssetDataList>(File.ReadAllText(AssetDataList.jsonPath));
		if(assetDataList != null)
		{
			foreach(AssetData data in assetDataList.datas)
			{
				assetDatas.Add(data.sourceName, data);
			}
		}
	}
	[LuaCallCSharp]
	public UnityEngine.Object Load(string resName, Type type)
	{
		if(!assetDatas.Keys.Contains(resName))
		{
			Debug.LogError("不存在该资源");
			return null;
		}
		AssetData data = assetDatas[resName];
		if(isLoadForAB)
		{
			return null;
		}
		else
		{
			return LoadForAsset.Instance.LoadImmdiatly(data.path, type, data.sourceType);
		}
	}
}
