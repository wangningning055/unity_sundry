using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using XLua;
public class Main : MonoBehaviour
{
	//将main中的更新映射到lua中
	public Delegate_Void_Void _luaStart;
	public Delegate_Void_Float _luaUpdate;
	public Delegate_Void_Float _luaLateUpdate;

	public Delegate_LuaTable_String luaTable_Delegate;
	LuaEnv luaEnv;
	LuaTable luaMian;
	bool isLoader = false;
	Event aaa;

	//是否从ab包中加载lua
	bool iaLoadLuaFromAB = false;

	//心跳监听
	int outHeartTime = 5;
	int curHeartTime = 0;
    void Start()
    {
		LoadForAB.Instance.Init(this);
		LoadMgr.Instance.Init();
		luaEnv = new LuaEnv();
        GameObject begin = new GameObject("begin");
		LuaEnv.CustomLoader loder = customLoder;
		LuaEnv.CustomLoader loderAB = customLoderFromAB;
		if(iaLoadLuaFromAB)
			luaEnv.AddLoader(loderAB);
		else
			luaEnv.AddLoader(loder);

		// luaEnv.DoString("local main = require('main.lua')  main.Start()");
		luaTable_Delegate = luaEnv.Global.GetInPath<Delegate_LuaTable_String>("require");
		luaMian = luaTable_Delegate("main");
		isLoader = luaMian != null && luaEnv != null;
		if(!isLoader)
		{
			Debug.Log("load lua miss");
			return;
		}
		_luaStart = luaMian.Get<Delegate_Void_Void>("Start");
		_luaStart?.Invoke();

		//事件注册模块测试
		// ListenerManager.Instance.RegistEvent(EventType.MainEvt, Act1);
		// ListenerManager.Instance.RegistEvent(EventType.Button1, Act3);
		// ListenerManager.Instance.RegistEvent(EventType.Button1, Act2);


		//资源加载模块测试
		// LoadForAB.Instance.LoadAsync("prefab.assetbundle", "cube", (obj) =>{
		// 	(obj as GameObject).transform.position = Vector3.up;
		// });
		// LoadForAB.Instance.LoadAsync("prefab.assetbundle", "cube", (obj) =>{
		// 	(obj as GameObject).transform.position = Vector3.up;
		// });
		// LoadForAB.Instance.LoadAsync<GameObject>("prefab.assetbundle", "sphere", (obj) => {
		// 	obj.transform.position = Vector3.down;
		// });
		// LoadForAB.Instance.LoadAsync<GameObject>("prefab.assetbundle", "sphere", (obj) => {
		// 	obj.transform.position = Vector3.down;
		// });
		// LoadForAB.Instance.LoadAsync("prefab.assetbundle", "cube", typeof(GameObject), (obj) =>{
		// 	(obj as GameObject).transform.position = Vector3.up;
		// });
		// LoadForAB.Instance.LoadAsync("prefab.assetbundle", "cube", typeof(GameObject), (obj) =>{
		// 	(obj as GameObject).transform.position = Vector3.up;
		// });
	
    }

	void Act1()
	{
		print("this is Act1");
	}
	void Act2()
	{
		print("this is Act2");
	}
	void Act3()
	{
		print("this is Act3");
	}

    void Update()
    {
        if(!isLoader) return;
		_luaUpdate = luaMian.Get<Delegate_Void_Float>("Update");
		_luaUpdate?.Invoke(Time.deltaTime);
		// GameObject player = GameObject.Find("RPG").transform.Find("RPG-Character").gameObject;
		// Animator animator = player.GetComponent<Animator>();
		// if(Input.GetKeyDown(KeyCode.A))
		// {
	
		// 	bool aa = animator.GetBool("isInMotion");
		// 	animator.SetBool("isInMotion" , !aa);
		// }
		// if(Input.GetKey(KeyCode.D))
		// {
		// 	animator.SetLayerWeight(1, 0.7f);
		// }
		// else
		// {
		// 	animator.SetLayerWeight(1, 0);

		// }
		// if(Input.GetKeyDown(KeyCode.S))
		// {
		// 	print("获取名字");
		// 	print(PlayerPrefs.GetInt("PlayerNamesss"));
		// }

		// if(Input.GetKeyDown(KeyCode.A))
		// {
		// 	ListenerManager.Instance.TriggerEvent(EventType.MainEvt);
		// }
		// if(Input.GetKeyDown(KeyCode.S))
		// {
		// 	ListenerManager.Instance.TriggerEvent(EventType.Button1);
		// }
		// if(Input.GetKeyDown(KeyCode.Q))
		// {
		// 	ListenerManager.Instance.UnRegistEvent(EventType.MainEvt, Act1);
		// }
    }
	void LateUpdate()
	{
        if(!isLoader) return;
		_luaLateUpdate = luaMian.Get<Delegate_Void_Float>("LateUpdate");
		_luaLateUpdate?.Invoke(Time.deltaTime);
		luaEnv.Tick();

	}
	byte[] customLoder(ref string fileName)
	{
		// Debug.Log("查找名称： " + fileName);
		fileName = fileName.Replace('.', '/');
		fileName = Application.dataPath + "/../Lua/" + fileName + ".lua";
		// Debug.Log("查找位置 ：" + fileName);
		if(File.Exists(fileName))
		{
			return File.ReadAllBytes(fileName);
		}
		return null;
	}
	byte[] customLoderFromAB(ref string fileName)
	{
		// Debug.Log("查找名称： " + fileName);
		string[] aaa = fileName.Split('.');
		TextAsset textAsset = LoadForAB.Instance.LoadImmdiatly("lua.assetbundle", aaa[aaa.Length - 1] + ".lua") as TextAsset;
		// Debug.Log("??????????????????????");
		// Debug.Log(textAsset.text);
		return System.Text.ASCIIEncoding.UTF8.GetBytes(textAsset.text);
		// fileName = fileName.Replace('.', '/');
		// Debug.Log("查找位置 ：" + fileName);
		// if(File.Exists(fileName))
		// {
		// 	return File.ReadAllBytes(fileName);
		// }
	}
}
