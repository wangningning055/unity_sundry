using UnityEngine;
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
public class NetMono:MonoBehaviour
{
	void Start()
	{
		SocketManager.Instance.Init();
		ProtocalHandler.Instance.AddProtocalCall(ProtocalType.Test, Test1);
		ProtocalHandler.Instance.AddProtocalCall(ProtocalType.Test, Test2);
		ProtocalHandler.Instance.AddProtocalCall(ProtocalType.TestNoData, Test3);
		//rpc
		//拆包粘包

	}
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.A))
		{
			print("发送消息");
			SendTesss();
		}
	}
	void OnDestroy()
	{
		print("销毁");
		SocketManager.Instance.Destroy();
	}
	void SendTesss()
	{
		// byte[] aaa = Encoding.UTF8.GetBytes("qqqqwwwwwee啊");
		// for(int i = 0; i < aaa.Length; i++)
		// {
		// 	print(aaa[i]);
		// }
		// print("??????????????????????????");
		// byte[] bbb = new byte[aaa.Length * 2];

		// for(int i = 0; i < bbb.Length; i++)
		// {
		// 	print(bbb[i]);
		// }
		// Array.Copy(aaa, 0, bbb, 0, aaa.Length);
		// print("??????????????????????????");
		// for(int i = 0; i < bbb.Length; i++)
		// {
		// 	print(bbb[i]);
		// }
		// string ddd = Encoding.UTF8.GetString(bbb);
		// print(ddd);
		// byte[] tes = BitConverter.GetBytes(65535);
		// for(int i = 0; i < tes.Length; i++)
		// {
		// 	print(tes[i]);
		// }
		NetForClient.Instance.SendProtocal(ProtocalType.LoginHeart);

	}
	void rpcConfirm(BaseNetData data)
	{
		print("rpc回调确认 ");
	}
	void callBack()
	{
		Console.WriteLine("哇哇哇叽叽叽叽");
	}
	void Test1(BaseNetData baseNetData)
	{
		print("触发Test绑定消息！！！！！！");
	}
	void Test2(BaseNetData baseNetData)
	{
		print("触发Test绑定消息??????");

		
	}
	void Test3(BaseNetData baseNetData)
	{
		print("触发2222222222222222");
		
	}
}