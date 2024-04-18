using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AssetData
{
	public SourceType sourceType;
	public string sourceName;
	public string path;
}
[Serializable]
public class AssetDataList:IEnumerable
{
	public static string jsonPath = Application.dataPath + "/AssetData/data";
	public List<AssetData> datas;
	public Dictionary<int, AssetData>aaa;
	public void ass()
	{
		aaa.Remove(1);
	}

    public IEnumerator GetEnumerator()
    {
        throw new NotImplementedException();
    }
}
public class test
{
	public void aaa()
	{
		AssetDataList aaa = new AssetDataList();
		foreach(int a in aaa)
		{

		}
	}
}
