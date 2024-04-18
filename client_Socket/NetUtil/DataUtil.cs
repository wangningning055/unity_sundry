using System;

public class DataUtil
{
	public static HeadData GetHeadData( bool isRpc, ProtocalType protocalType,NetDataType dataType = NetDataType.Base, Action<BaseNetData> callback = null)
	{
		HeadData headData = new HeadData();
		headData.port = SocketManager.Instance.curPort;
		headData.dataType = GetDataTypeByProtocal(protocalType);
		headData.isRpc = isRpc;
		headData.protocalType = protocalType;
		if(isRpc)
		{
			headData.rpcTarget = ProtocalHandler.Instance.AddRpcCall(callback);
		}
		return headData;
	}
	public static NetDataType GetDataTypeByProtocal(ProtocalType protocalType)
	{
		if(protocalType == ProtocalType.LoginHeart)
		{
			return NetDataType.LoginHeart;
		}
		return NetDataType.Base;
	}
}