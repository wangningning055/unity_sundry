using System.Net;
using System.Net.Sockets;
using UnityEngine;
public class HeartSocket
{
	Socket heartSocket;
	//失去心跳次数
	int timeOutCount = 5;
	SocketManager manager;

	public void Init(SocketManager socketManager)
	{
		manager = socketManager;
	}
	public void Begin()
	{
		heartSocket = SocketManager.Instance.CreatUDPSocket();
		
		// LoginHeart loginHeart = new LoginHeart();
		// loginHeart.playerID = 1;
		// loginHeart.port = manager.curPort;
		// heartSocket.SendTo(JsonSerlized.SerizedData(loginHeart), manager.serverIPEndPoint);
		Debug.Log("发送登录信息");
	}
	public void TriggerHeart()
	{
		// LoginHeart loginHeart = new LoginHeart();
		// loginHeart.playerID = 1;
		// loginHeart.port = manager.curPort;
		// heartSocket.SendTo(JsonSerlized.SerizedData(loginHeart), manager.serverIPEndPoint);
		Debug.Log("发送登录信息");
	}

	void DisConnectCallback()
	{

	}
}