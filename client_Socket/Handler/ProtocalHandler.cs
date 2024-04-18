using System.Collections.Generic;
using System;
using UnityEngine;
using System.Text;
using System.Linq;
using UnityEditor.Experimental.GraphView;
public class ProtocalHandler
{
	static ProtocalHandler protocalHandler;
	Dictionary<int, Action<BaseNetData>> rpcDic = new Dictionary<int, Action<BaseNetData>>();
	Dictionary<int, Action<BaseNetData>> protocalDic = new Dictionary<int, Action<BaseNetData>>();

	// Dictionary<ProtocalType, Action<BaseNetData>> protocalDic = new Dictionary<ProtocalType, Action<BaseNetData>>();
	public static ProtocalHandler Instance
	{
		get{
			if(protocalHandler == null)
			{
				protocalHandler = new ProtocalHandler();
				protocalHandler.Init();
			}
			return protocalHandler;
		}
	}
	public void Init()
	{
		InitProtocalDic();
	}
	void InitProtocalDic()
	{
		for(int i = 0; i < Enum.GetValues(typeof(ProtocalType)).Length; i++)
		{
			int key = (int)Enum.GetValues(typeof(ProtocalType)).GetValue(i);
			Action<BaseNetData> action = (BaseNetData) => {};
			protocalDic.Add(key, action);
		}
	}
	public void HandleProtocal(byte[] bytes, int length)
	{
		// BaseNetData baseNetData = JsonSerlized.DeSerializaData(bytes, length);
		// if(baseNetData == null) return;
		// ExcuteProtocalCall(baseNetData);
		// if(baseNetData.protocalType == ProtocalType.Quit)
		// {
		// 	Debug.Log("服务器退出");
		// }
	}

	public void AddProtocalCall(ProtocalType protocalType, Action<BaseNetData> callBack)
	{
		if(protocalDic.Keys.Contains((int)protocalType))
		{
			protocalDic[(int)protocalType] += callBack;
		}
	}
	public void RemoveProtocalCall(ProtocalType protocalType, Action<BaseNetData> callBack)
	{
		if(protocalDic.Keys.Contains((int)protocalType))
		{
			protocalDic[(int)protocalType] -= callBack;
		}
	}
	public void ExcuteProtocalCall(BaseNetData baseNetData)
	{
		// int key = (int)baseNetData.protocalType;
		// if(protocalDic.Keys.Contains(key))
		// {
		// 	protocalDic[key].Invoke(baseNetData);
		// }
	}
	public int AddRpcCall(Action<BaseNetData> callback)
	{
		// int target = JsonSerlized.GetRandomInt();
		// while(rpcDic.ContainsKey(target))
		// 	target = JsonSerlized.GetRandomInt();

		
		// rpcDic.Add(target, callback);
		// return target;
		return -1;
	}
	public void TriggerRpcCallBack(byte[] bytes, int length)
	{
		// Debug.Log("接收到消息，长度是" + length);
		// string data = Encoding.UTF8.GetString(bytes, 0, length);
		// BaseNetData getdata = JsonUtility.FromJson<BaseNetData>(data);
		// if(rpcDic.Keys.Contains(getdata.rpcTarget))
		// {
		// 	rpcDic[getdata.rpcTarget](getdata);
		// 	rpcDic.Remove(getdata.rpcTarget);
		// }
	}

}