using System.Net.Sockets;
using System.Net;
using System.Text;
using UnityEngine;
using System;
using Packages.Rider.Editor.UnitTesting;
using System.Collections.Generic;
public class SocketManager
{
	static SocketManager socketManager;
	public static SocketManager Instance
	{
		get{
			if(socketManager == null)
			{
				socketManager = new SocketManager();
			}	
			return socketManager;
		}
	}
	public IPAddress serverIPAdress;
	public IPEndPoint serverIPEndPoint;
	public int serverPort = 1407;

	public int curPort = -1;
	HeartSocket loginHeartSocket;
	Socket mainSocket;
	byte[] dataRecive;
	Dictionary<int, Action> rpcDic;
	public void Init()
	{
		rpcDic = new Dictionary<int, Action>();
		dataRecive = new byte[1024];
		serverIPAdress = new IPAddress(new byte[]{ 10, 6, 12, 233 });
		curPort = GetPort();
		serverIPEndPoint = GetServerIPEndPoint();
		mainSocket = CreatUDPSocket();
		BeginContinueReciveData();
		//尝试建立连接
		// Login();
		//建立成功


		// 建立失败
	}

	//建立一个可用的UDPSocket
	public Socket CreatUDPSocket()
	{
		Socket udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
		
		IPEndPoint iPEndPointClient = new IPEndPoint(serverIPAdress, curPort);
		udpSocket.Bind(iPEndPointClient);
		Debug.Log("建立一个socket：" + curPort);
		return udpSocket;
	}

	//获取一个可用的端口号
	public int GetPort()
	{
		TcpListener tcpListener = new TcpListener(serverIPAdress, 0);
		tcpListener.Start();
		int port = ((IPEndPoint)tcpListener.LocalEndpoint).Port;
		tcpListener.Stop();
		return port;
	}
	IPEndPoint GetServerIPEndPoint()
	{
		return new IPEndPoint(serverIPAdress, serverPort);
		
	}
	public void SendData(byte[] data)
	{
		EndPoint ipEndpointFrom = new IPEndPoint(serverIPAdress, 1407);
		mainSocket.SendTo(data, ipEndpointFrom);
	}

	//开启一次监听
	public void ReciveData()
	{
		EndPoint ipEndpointFrom = new IPEndPoint(serverIPAdress, 0);
		IAsyncResult ar = mainSocket.BeginReceiveFrom(dataRecive, 0, 1024, SocketFlags.None, ref ipEndpointFrom, ReceiveDataAsync, mainSocket);
	}
	public void ReceiveDataAsync(IAsyncResult ar)
	{
		EndPoint ipEndpointFrom = new IPEndPoint(serverIPAdress, 0);
		int length = mainSocket.EndReceiveFrom(ar, ref ipEndpointFrom);
		Debug.Log("接收到数据 : " + Encoding.UTF8.GetString(dataRecive, 0, length));
		ProtocalHandler.Instance.TriggerRpcCallBack(dataRecive, length);
		
	}


	//开启连续不断的监听
	public void BeginContinueReciveData()
	{
		EndPoint ipEndpointFrom = new IPEndPoint(serverIPAdress, 0);
		Debug.Log("建立第一个监听");

		IAsyncResult ar = mainSocket.BeginReceiveFrom(dataRecive, 0, 1024, SocketFlags.None, ref ipEndpointFrom, ReciveUDPDataAsync, mainSocket);
	}
	public void ReciveUDPDataAsync(IAsyncResult ar)
	{
		
		EndPoint ipEndpointFrom = new IPEndPoint(serverIPAdress, 0);
		Socket curSocket = (Socket)ar.AsyncState;
		int length = mainSocket.EndReceiveFrom(ar, ref ipEndpointFrom);

		ProtocalHandler.Instance.TriggerRpcCallBack(dataRecive, length);
		ProtocalHandler.Instance.HandleProtocal(dataRecive, length);
		Debug.Log("接收到数据， 同时建立一个新的监听");
		Debug.Log("接收到数据 : " + Encoding.UTF8.GetString(dataRecive, 0, length));

		mainSocket.BeginReceiveFrom(dataRecive, 0, 1024, SocketFlags.None, ref ipEndpointFrom, ReciveUDPDataAsync, mainSocket);

	}

	public void Destroy()
	{
		
		NetForClient.Instance.SendProtocal(ProtocalType.Quit);
		mainSocket.Close();
		mainSocket.Dispose();
		mainSocket = null;
		dataRecive =null;
		serverIPAdress = null;
		serverIPEndPoint = null;
		socketManager = null;
	}

}