using System;
using System.Net;
using System.Net.Sockets;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using XLuaTest;

public class SocketTest:MonoBehaviour
{
	IPAddress iPAddress = new IPAddress(new byte[]{ 10, 6, 12, 233 });
	float time = 0;
	Socket udpSocket;
	bool isReciveData = false;
	byte[] dataRecive = new byte[1024];
	void Start()
	{
		Debug.Log("开始建立");
		CreatUDPSocket();
		ReceiveBegin();
	
	}
	public async Task<int> AsyncTest2()
	{
		return await Task.Run(()=>AsyncTest());
	}
	public int AsyncTest()
	{
		Thread.Sleep(10000);
		return 10;
	}

	public void CreatSocket()
	{
		Socket tcpClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		IPAddress iPAddress = new IPAddress(new byte[]{ 10, 6, 12, 233 });

		IPEndPoint iPEndPoint = new IPEndPoint(iPAddress, 1407);
		tcpClient.Connect(iPEndPoint);
		string aaa = "2123123";
		Debug.Log("已经建立连接");
		tcpClient.Send(Encoding.UTF8.GetBytes(aaa));

		byte[] data = new byte[1024];

		int Length = tcpClient.Receive(data);
		Debug.Log("接收到消息" + Encoding.UTF8.GetString(data, 0, Length));
	}
	public void CreatUDPSocket()
	{
		udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

		int usefulPort = GetPort();

		byte[] data = Encoding.UTF8.GetBytes(usefulPort.ToString());

		IPEndPoint iPEndPointClient = new IPEndPoint(iPAddress, usefulPort);
		udpSocket.Bind(iPEndPointClient);
		Debug.Log("建立完毕：" + usefulPort);
		
		IPEndPoint iPEndPoint = new IPEndPoint(iPAddress, 1407);
		udpSocket.SendTo(data, iPEndPoint);	
	
	}
	public async Task<int> AasyncRecive()
	{
		return await Task.Run(() => ReciveUDPData());
	}
	public int ReciveUDPData()
	{
		byte[] dataRecive = new byte[1024];
		EndPoint ipEndpointFrom = new IPEndPoint(IPAddress.Any, 0);
		int length = udpSocket.ReceiveFrom(dataRecive, ref ipEndpointFrom);
		isReciveData = true;
		Debug.Log("接收到数据 : " + Encoding.UTF8.GetString(dataRecive, 0, length));
		return length;
	}

	public void ReceiveBegin()
	{
		EndPoint ipEndpointFrom = new IPEndPoint(IPAddress.Any, 0);
		Debug.Log("建立第一个连接");

		IAsyncResult ar = udpSocket.BeginReceiveFrom(dataRecive, 0, 1024, SocketFlags.None, ref ipEndpointFrom, ReciveUDPDataAsync, udpSocket);
	}
	public void ReciveUDPDataAsync(IAsyncResult ar)
	{
		
		EndPoint ipEndpointFrom = new IPEndPoint(IPAddress.Any, 0);
		Socket curSocket = (Socket)ar.AsyncState;
		int length = udpSocket.EndReceiveFrom(ar, ref ipEndpointFrom);
		Debug.Log("接收到数据， 建立一个新的连接");
		// ar.AsyncWaitHandle.Close();
		Debug.Log("接收到数据 : " + Encoding.UTF8.GetString(dataRecive, 0, length));

		// udpSocket.BeginReceive(dataRecive, 0, 1024, SocketFlags.None, ReciveUDPDataAsync, udpSocket);
		udpSocket.BeginReceiveFrom(dataRecive, 0, 1024, SocketFlags.None, ref ipEndpointFrom, ReciveUDPDataAsync, udpSocket);

		isReciveData = true;
	}


	public int GetPort()
	{
		TcpListener tcpListener = new TcpListener(iPAddress, 0);
		tcpListener.Start();
		int port = ((IPEndPoint)tcpListener.LocalEndpoint).Port;
		tcpListener.Stop();
		return port;
	}
	void OnDestroy()
	{
		Debug.Log("********断开连接**********");
		udpSocket.Shutdown(SocketShutdown.Both);
		udpSocket.Close();
	}

}