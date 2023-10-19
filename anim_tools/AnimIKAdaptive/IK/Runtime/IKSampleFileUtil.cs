using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using MountPoint;
namespace vw_animation_ik_runtime
{
	public class IKSampleFileUtil
	{
		Dictionary<string, IKSampleData> fileDataStack;
		//读取原始曲线数据
		public static List<Keyframe> ReadKeyFrameData(string text)
		{
			List<Keyframe> animKeyList = new List<Keyframe>();
			string[] frameData = text.Split(';');
			for(int i = 0; i < frameData.Length; i++)
			{
				if(frameData[i] == "") continue;
				Keyframe key = UnPackKeyframe(frameData[i]);
				animKeyList.Add(key);
			}

			return animKeyList;
		}
		//读取原始骨骼终点位置
		public static Vector3 ReadOriginalData(string path, HandleTargetType type)
		{
			Vector3 rootPos = Vector3.zero;
			Vector3 LHandPos = Vector3.zero;
			Vector3 RHandPos = Vector3.zero;

			string text = File.ReadAllText(path);
			string[] frameData = text.Split(';');
			for(int i = 0; i < frameData.Length; i++)
			{
				if(frameData[i] == "") continue;
				string[] posData = frameData[i].Split(',');
				float x = float.Parse(posData[0]);
				float y = float.Parse(posData[1]);
				float z = float.Parse(posData[2]);
				if(i == 0)
				rootPos = new Vector3(x, y , z);
				if(i == 1)
				LHandPos = new Vector3(x, y , z);
				if(i == 2)
				RHandPos = new Vector3(x, y , z);
			}
			if(type == HandleTargetType.Root)
			return rootPos;
			else if(type == HandleTargetType.HandL)
			return LHandPos;
			else
			return RHandPos;
		}

		public static string PackKeyframe(Keyframe key)
		{
			float inTangent = key.inTangent;
			float inWeight = key.inWeight;
			float outTangent = key.outTangent;
			float outWeight = key.outWeight;
			float time = key.time;
			float value = key.value;
			int weightedMode = (int)key.weightedMode;
			string all = $"{inTangent}, {inWeight}, {outTangent}, {outWeight}, {time}, {value}, {weightedMode};";
			return all;
		}

		public static Keyframe UnPackKeyframe(string data)
		{
			// Keyframe key = new Keyframe();
			Keyframe key = ResolveCurveMgr.Instance._keyFramePool.GetKey();
			string[] keyData = data.Split(',');
			key.inTangent = float.Parse(keyData[0]);
			key.inWeight = float.Parse(keyData[1]);
			key.outTangent = float.Parse(keyData[2]);
			key.outWeight = float.Parse(keyData[3]);
			key.time = float.Parse(keyData[4]);
			key.value = float.Parse(keyData[5]);
			int weightedModeInt = int.Parse(keyData[6]);
			key.weightedMode = (WeightedMode)weightedModeInt;
			return key;
		}


		public static bool IsHaveTime(string animName, out float time)
		{
			string dirPath = Application.dataPath + "/.." + "/TempAnimData" + $"/{animName}";
			string filePath = dirPath + "/" + "Time";
			bool isHaveTime = File.Exists(filePath);
			time = 0;
			if(isHaveTime)
				time = float.Parse(File.ReadAllText(filePath));
			return isHaveTime;
		}

		public static string GetPath(string name)
		{
			string dirPath = Application.dataPath + "/.." + "/TempAnimData" + $"/{name}";
			string filePath = dirPath + "/" + "data.json";
			return filePath;
		}
		public static void SaveJson(IKSampleData data, string animName)
		{
			// animName = animName.Replace("_start", "");
			string jsonData = JsonUtility.ToJson(data);
			SaveDataInFile(animName, jsonData);
		}
		public static string GetJsonStr(string path)
		{
			return File.ReadAllText(path);
		}
		public static void SaveDataInFile(string animName, string allText)
		{
			string dirPath = Application.dataPath + "/.." + "/TempAnimData" + $"/{animName}";
			string filePath = dirPath + "/" + "data.json";
			if(!Directory.Exists(dirPath))
			{
				Directory.CreateDirectory(dirPath);
			}
			File.WriteAllText(filePath, allText);
			Debug.Log("写入完毕");
		}
		public static bool ReadDataByeAnimName(string animName, out IKSampleData data)
		{
			// string dirPath = NameConst.GetSavePath() + "/" + animName;
			// if(Directory.Exists(dirPath))
			// {
			// 	data = IKSampleData.ResolveJson(dirPath + "/" + "data.json");
			// 	return true;
			// }
			data = null;
			// Debug.LogWarning($"没有找到对应动画曲线烘培数据:{dirPath}");
			return false;
		}
		

		
	}
}