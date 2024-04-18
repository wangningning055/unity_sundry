using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
public class GenAssetSourceJson
{
	[MenuItem("打包/生成Assetjson")]
	public static void GenData()
	{
		Debug.Log("生成完毕");
		CollectAssetData();
	}


	static void CollectAssetData()
	{
		string dataPath = Application.dataPath + "/Raw";
		List<AssetData> assetDatas = new List<AssetData>();
		DirectoryInfo directoryInfo = new DirectoryInfo(dataPath);
		foreach(DirectoryInfo childdirInfo in directoryInfo.GetDirectories("*.*", SearchOption.TopDirectoryOnly))
		{
			SourceType type = GetPathBySourceType.getType(childdirInfo.Name);
			foreach(FileInfo fileInfo in childdirInfo.GetFiles("*.*", SearchOption.AllDirectories))
			{
				if(!fileInfo.FullName.EndsWith(".meta"))
				{
					int aa = fileInfo.FullName.IndexOf("Assets");
					string path = fileInfo.FullName.Substring(aa);
					AssetData assetData = new AssetData();
					assetData.sourceName = fileInfo.Name.Split('.')[0];
					assetData.sourceType = type;
					assetData.path = path;
					assetDatas.Add(assetData);
				}
			}
		}
		AssetDataList assetDataList = new AssetDataList();
		assetDataList.datas = assetDatas;
		string data = JsonUtility.ToJson(assetDataList);
		Debug.Log(assetDatas.Count);
		Debug.Log(data);
		string jsonPath = AssetDataList.jsonPath;
		if(!File.Exists(jsonPath))
		{
			File.Create(jsonPath);
		}
		File.WriteAllText(jsonPath, data);

		AssetDataList temp = JsonUtility.FromJson<AssetDataList>(File.ReadAllText(jsonPath));
		Debug.Log(temp.datas.Count);
		// Debug.Log(jsonPath);
	}
}
