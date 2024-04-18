using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
public class NetForClient
{
	static NetForClient netForClient;
	public static NetForClient Instance
	{
		get{
			if(netForClient == null)
			{
				netForClient = new NetForClient();
			}
			return netForClient;
		}
	}
	public void SendProtocal(ProtocalType protocalType, BaseNetData data = null)
	{
		data ??= new BaseNetData();
		// data.protocalType = protocalType;
		// data.isRpc = false;
		// data.port = SocketManager.Instance.curPort;
		// byte[] bytes = JsonSerlized.SerizedData(data);
		LoginHeart loginHeart = new LoginHeart();
		loginHeart.playerID = 1001;
		loginHeart.heartData = 1024;
		loginHeart.heartFrame = new int[1000];
		loginHeart.heartFrame[3] = 23;
		loginHeart.heartFrame[0] = 11;
		loginHeart.heartFrame[99] = 1111;
		loginHeart.heartFrame[999] = 12684;

		// HeadData headData = DataUtil.GetHeadData(false, protocalType);
		// List<byte[]> dataList = JsonSerlized.GetPackList(headData, loginHeart);
		// BaseNetData baseNetData = JsonSerlized.ResolvePackList(dataList);
		// LoginHeart newGetData = baseNetData as LoginHeart;
		// Debug.Log(newGetData.heartFrame[999]);
		// Debug.Log(newGetData.playerID);
		
		// SocketManager.Instance.SendData(bytes);

	}
	public void SendRpc(ProtocalType protocalType, Action<BaseNetData> callBack, BaseNetData data = null)
	{
		// data ??= new BaseNetData();
		// data.protocalType = protocalType;
		// data.isRpc = true;
		// data.port = SocketManager.Instance.curPort;
		// data.rpcTarget = ProtocalHandler.Instance.AddRpcCall(callBack);
		// byte[] bytes = JsonSerlized.SerizedData(data);
		// Debug.Log("发送长度为: " + bytes.Length);
		// SocketManager.Instance.SendData(bytes);
	}
	
}