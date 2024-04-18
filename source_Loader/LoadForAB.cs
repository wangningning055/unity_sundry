using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadForAB
{
	private static LoadForAB loadForAB;
	private Dictionary<string, AssetBundle> abDic;
	private AssetBundleManifest assetBundleManifest;
	public Main mainObj;
	private List<string> loadingList;
	private string abUrl{
		get{
			return Application.dataPath + "/" + "AssetBundle" + "/";
		}
	}
	private string mainABName{
		get{
			return "AssetBundle";
		}
	}
	public static LoadForAB Instance
	{
		get{
			if(loadForAB == null)
			{
				loadForAB = new LoadForAB();
			}
			return loadForAB;
		}
	}
	public void Init(Main main)
	{

		abDic = new Dictionary<string, AssetBundle>();
		loadingList = new List<string>();
		AssetBundle abMain = AssetBundle.LoadFromFile(abUrl + mainABName);
		assetBundleManifest = abMain.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
		mainObj = main;

	}
	private AssetBundle LoadAB(string abName)
	{
		if(!abDic.ContainsKey(abName))
		{

			//加载依赖
			string[] dependices = assetBundleManifest.GetAllDependencies(abName);
			Debug.Log(dependices.Length);
			foreach(string dep in dependices)
			{
				if(!abDic.ContainsKey(dep))
				{
					Debug.Log(abUrl + dep);
					AssetBundle ab = AssetBundle.LoadFromFile(abUrl + dep);
					Debug.Log("Add Dic" + dep);

					abDic.Add(dep, ab);
				}
			}
			//加载目标包
			AssetBundle targetAB = AssetBundle.LoadFromFile(abUrl + abName);
			Debug.Log("Add Dic" + abName);
			abDic.Add(abName, targetAB);
			return targetAB;
		}

		return abDic[abName];
	}

	public UnityEngine.Object LoadImmdiatly(string abName, string resName)
	{
		AssetBundle targetAB = LoadAB(abName);
		UnityEngine.Object obj = targetAB.LoadAsset(resName);
		if(obj is GameObject)
		{
			return GameObject.Instantiate(obj);
		}
		return obj;
	}
	public T LoadImmdiatly<T>(string abName, string resName) where T: UnityEngine.Object
	{
		AssetBundle targetAB = LoadAB(abName);
		T obj = targetAB.LoadAsset(resName) as T;
		if(typeof(T) == typeof(GameObject))
		{
			return GameObject.Instantiate(obj as GameObject) as T;
		}
		return obj;
	}

	public UnityEngine.Object LoadImmdiatly(string abName, string resName, Type type)
	{
		AssetBundle targetAB = LoadAB(abName);
		UnityEngine.Object obj = targetAB.LoadAsset(resName, type);
		if(obj is GameObject)
		{
			return GameObject.Instantiate(obj);
		}
		return obj;
	}

	public void LoadAsync(string abName, string resName, Action<UnityEngine.Object> callback)
	{
		mainObj.StartCoroutine(loadAsync(abName, resName, loadingList.Contains(abName), callback));
		if(!loadingList.Contains(abName))
			loadingList.Add(abName);
	}

	public void LoadAsync<T>(string abName, string resName, Action<T> callback) where T: UnityEngine.Object
	{
		mainObj.StartCoroutine(loadAsync(abName, resName, loadingList.Contains(abName), callback));
		if(!loadingList.Contains(abName))
			loadingList.Add(abName);
	}
	public void LoadAsync(string abName, string resName, Type type, Action<UnityEngine.Object> callback)
	{
		mainObj.StartCoroutine(loadAsync(abName, resName, loadingList.Contains(abName), type, callback));
		if(!loadingList.Contains(abName))
			loadingList.Add(abName);
	}
	IEnumerator loadAsync<T>(string abName, string resName, bool isLoading, Action<T> callback)where T: UnityEngine.Object
	{	
		if(isLoading)
		{
			// Debug.Log("等待另一个加载完成");
			yield return new WaitWhile(() =>{
				return loadingList.Contains(abName);
			});
		}
	
		if(!abDic.ContainsKey(abName))
		{

			//加载依赖
			string[] dependices = assetBundleManifest.GetAllDependencies(abName);
			foreach(string dep in dependices)
			{
				if(!abDic.ContainsKey(dep) && !loadingList.Contains(dep))
				{
					loadingList.Add(dep);
					AssetBundleCreateRequest ab = AssetBundle.LoadFromFileAsync(abUrl + dep);
					yield return ab.assetBundle;
					// Debug.Log("Add Dic " + dep);
					abDic.Add(dep, ab.assetBundle);
					loadingList.Remove(dep);
				}
			}
			//加载目标包
			AssetBundleCreateRequest targetAB = AssetBundle.LoadFromFileAsync(abUrl + abName);
			yield return targetAB;
			
			abDic.Add(abName, targetAB.assetBundle);
		
		}
		loadingList.Remove(abName);
		// Debug.Log("加载完成");
		AssetBundleRequest abRequest = abDic[abName].LoadAssetAsync(resName);
		yield return abRequest;
		if(abRequest.asset is GameObject)
		{
			GameObject a = GameObject.Instantiate(abRequest.asset) as GameObject;
			callback?.Invoke(a as T);
			yield break;
		}


		callback?.Invoke(abRequest.asset as T);
	}
	//协程异步加载ab包
	IEnumerator loadAsync(string abName, string resName, bool isLoading, Type type, Action<UnityEngine.Object> callback)
	{	
		if(isLoading)
		{
			// Debug.Log("等待另一个加载完成");
			yield return new WaitWhile(() =>{
				return loadingList.Contains(abName);
			});
		}
		if(!abDic.ContainsKey(abName))
		{
			//加载依赖
			string[] dependices = assetBundleManifest.GetAllDependencies(abName);
			// Debug.Log(dependices.Length);
			foreach(string dep in dependices)
			{
				if(!abDic.ContainsKey(dep) && !loadingList.Contains(dep))
				{
					loadingList.Add(dep);
					AssetBundleCreateRequest ab = AssetBundle.LoadFromFileAsync(abUrl + dep);
					yield return ab.assetBundle;
					// Debug.Log("Add Dic " + dep);
					abDic.Add(dep, ab.assetBundle);
					loadingList.Remove(dep);
				}
			}
			//加载目标包
			AssetBundleCreateRequest targetAB = AssetBundle.LoadFromFileAsync(abUrl + abName);
			yield return targetAB;
			// Debug.Log("Add Dic" + abName);
			abDic.Add(abName, targetAB.assetBundle);
		
		}
		loadingList.Remove(abName);
		AssetBundleRequest abRequest = abDic[abName].LoadAssetAsync(resName, type);
		yield return abRequest;
		if(abRequest.asset is GameObject)
		{
			GameObject a = GameObject.Instantiate(abRequest.asset) as GameObject;
			callback?.Invoke(a);
			yield break;
		}
		callback?.Invoke(abRequest.asset);
	}
	public void UnLoadAB(string abName, bool isComplete)
	{
		if(abDic.ContainsKey(abName))
		{
			abDic[abName].Unload(isComplete);
		}
		abDic.Remove(abName);
	}
	public void UnLoadAll()
	{
		AssetBundle.UnloadAllAssetBundles(true);
		abDic.Clear();
	}

	// IEnumerator LoadAsync(string abName, string ObjName)
	// {
	// 	// AssetBundleCreateRequest ab = AssetBundle.LoadFromFileAsync(Application.dataPath + "/" + "AssetBundle" + "/" + abName + ".assetBundle");
	// 	// yield return ab;
	// 	// AssetBundleRequest abr = ab.assetBundle.LoadAssetAsync<GameObject>(ObjName);
	// 	// yield return abr;
	// 	// GameObject sphere = abr.asset as GameObject;
	// 	// Instantiate(sphere);

	// }

}
