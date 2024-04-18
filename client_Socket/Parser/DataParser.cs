// using System;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using UnityEngine;

public class DataParser
{
	//[1 + 1 + 1 + 2 + 255 + 1024]
	//包体序号         | 包体数量     |    数据长度       | 数据头 | 数据
	//[8位（0-255）]   [8位（0-255）]    [16位（0-65535]   [512]    [1024]
	//1+1+512+1024 = 1538
	static int packHeadLength = 4;
	static int dataHeadLength = 512;
	static int dataLength = 1024;

	//序列化数据为多个包
	public static List<byte[]> GetPackList(HeadData headData, BaseNetData data)
	{

		int totalLength = packHeadLength + dataHeadLength + dataLength;
		byte[] dataByte = SerializeData(data);
		byte[] headDataByte = SerializeData(headData);
		Debug.Log("发送数据长度: " + dataByte.Length);
		Debug.Log("头长度: " + headDataByte.Length);

		if(headDataByte.Length > dataHeadLength - 1) Debug.Log("数据头过大： "  + headDataByte.Length);
		int num = dataByte.Length / dataLength + 1;
		if(dataByte.Length > 65535) Debug.Log("数据体过大");
		if(num > 64) Debug.Log("数据体过大");
		byte packNums = (byte)num;
		Debug.Log("包体数量： " + packNums);
		List<byte[]> finalDatas = new List<byte[]>();
		for(int i = 0; i < packNums; i++)
		{
			byte[] singleBytes = new byte[totalLength];
			byte packNum = (byte)i;
			singleBytes[0] = packNum;
			singleBytes[1] = packNums;
			singleBytes[2] = BitConverter.GetBytes(dataByte.Length)[0];
			singleBytes[3] = BitConverter.GetBytes(dataByte.Length)[1];

			Array.Copy(headDataByte, 0, singleBytes, packHeadLength, headDataByte.Length);
			int copyLength = i * dataLength + dataLength > dataByte.Length? dataByte.Length % dataLength: dataLength;
			Debug.Log("复制长度： " + copyLength);
			Array.Copy(dataByte, i * dataLength, singleBytes, packHeadLength + dataHeadLength, copyLength);
			finalDatas.Add(singleBytes);
		}
		return finalDatas;
	}
	//将多个包解析为数据
	public static BaseNetData ResolvePackList(List<byte[]> packList)
	{
		int packNums = packList[0][1];

		int dataTotalLength = BitConverter.ToInt16(new byte[] { packList[0][2], packList[0][3] }, 0);
		HeadData headData = ResolveHead(packList[0]);
		Debug.Log("解析头: " + packNums);
		Debug.Log("头端口: " + headData.port);
		Debug.Log("解析数据长度: " + dataTotalLength);

		byte[] totalDataBytes = new byte[dataTotalLength];
		for(int i = 0; i < packList.Count; i++)
		{
			int length = (i + 1) * dataLength > dataTotalLength? dataTotalLength - i * dataLength: dataLength;
			Array.Copy(packList[i], packHeadLength + dataHeadLength, totalDataBytes, i * dataLength, length);
		}
		BaseNetData baseNetData = ResolveBody(headData.dataType, totalDataBytes, dataTotalLength);
		Debug.Log(baseNetData == null);
		Debug.Log("解析数据体");
		return baseNetData;

	}

	public static HeadData ResolveHead(byte[] pack)
	{
		return DeSerializeData<HeadData>(pack, packHeadLength, dataHeadLength);
	}
	public static BaseNetData ResolveBody(NetDataType netDataType, byte[] pack, int length)
	{
		return DeSerializeData(pack, netDataType, length);
	}



	public static byte[] SerializeData(object data)
	{
		if(data == null) return new byte[0];
		string str = JsonUtility.ToJson(data);
		Debug.Log("序列化的str：" + str);
		return Encoding.UTF8.GetBytes(str);
	}
	public static T DeSerializeData<T>(byte[] data, int index = 0, int length = -1)
	{
		length = length < 0? data.Length : length;
		string str = Encoding.UTF8.GetString(data, index, length);
		Debug.Log("解析的str：" + str);
		return JsonUtility.FromJson<T>(str);
	}
	public static BaseNetData DeSerializeData(byte[] data, NetDataType netDataType, int length = -1)
	{
		string str = "";
		if (length < 0)
			str = Encoding.UTF8.GetString(data);
		else
			str = Encoding.UTF8.GetString(data, 0, length);
		Debug.Log("解析的str：" + str);

		BaseNetData baseNetData = JsonUtility.FromJson<BaseNetData>(str);
		if (baseNetData != null)
		{
			if (netDataType == NetDataType.Base)
			{
				return baseNetData;
			}
			if (netDataType == NetDataType.LoginHeart)
			{
				return JsonUtility.FromJson<LoginHeart>(str);
			}
			if (netDataType == NetDataType.LoginNetData)
			{
				return JsonUtility.FromJson<LoginNetData>(str);
			}
		}
		return baseNetData;
	}
	public static int GetRandomInt()
	{
		return UnityEngine.Random.Range(0, 65535);
	}
}