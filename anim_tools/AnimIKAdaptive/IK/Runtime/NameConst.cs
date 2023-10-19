using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace vw_animation_ik_runtime
{
	public class NameConst
	{
		public static string rootName = "Bip001_Pelvis";
		public static string neck = "Neck";

		public static string footLName = "Bip001_L_Foot", footRName = "Bip001_R_Foot";
		public static string legL1Name = "Bip001_L_Thigh", legR1Name = "Bip001_R_Thigh";
		public static string legL2Name = "Bip001_L_Calf", legR2Name = "Bip001_R_Calf";

		public static string handLName = "Bip001_L_Hand",handRName = "Bip001_R_Hand";
		public static string armL1 = "Bip001_L_UpperArm",armR1 = "Bip001_R_UpperArm";
		public static string armL2 = "Bip001_L_Forearm",armR2 = "Bip001_R_Forearm";
		public static string spine1 = "Spine";
		public static string spine2 = "Spine1";
		public static string spine3 = "Spine2";
		public static string prefabName = "female_body_001_lod1";
		
		public static string rootObjPath = "Root/Bip001/Bip001_Pelvis";
		public static string handLObjPath = "Root/Bip001/Bip001_Pelvis/Spine/Spine1/Spine2/Bip001_L_Clavicle/Bip001_L_UpperArm/Bip001_L_Forearm/Bip001_L_Hand";
		public static string handRObjPath = "Root/Bip001/Bip001_Pelvis/Spine/Spine1/Spine2/Bip001_R_Clavicle/Bip001_R_UpperArm/Bip001_R_Forearm/Bip001_R_Hand";
		public static string spinePath = "Root/Bip001/Bip001_Pelvis/Spine/Spine1/Spine2/Neck";

		public static string rootPropertyNameY = "RootT.y";
		public static string rootPropertyNameX = "RootT.x";
		public static string rootPropertyNameZ = "RootT.z";
		public static string handLPropertyNameY = "LeftHandT.y";
		public static string handLPropertyNameX = "LeftHandT.x";
		public static string handLPropertyNameZ = "LeftHandT.z";
		public static string handRPropertyNameY = "RightHandT.y";
		public static string handRPropertyNameX = "RightHandT.x";
		public static string handRPropertyNameZ = "RightHandT.z";
		public static string lHandPointName = "lefthandpoint";
		public static string rHandPointName = "righthandpoint";
		public static string spinePointName = "spinepoint";
		public static string rootPointName = "rootpoint";

		public static string TestScenePath = "Assets/Scene/Prog/IKSampleTest.unity";
		
		public static string GetOriginalFilePath(string animName)
		{
			return Application.dataPath + "/.." + "/TempAnimData" + "/" + animName + "/OriginalPos";
		}
		public static string[] GetPropertyNameByType(HandleTargetType type)
		{
			string[] nameList = new string[3];
			if(type == HandleTargetType.Root)
			{ nameList[0] = rootPropertyNameX; nameList[1] = rootPropertyNameY; nameList[2] = rootPropertyNameZ;}
			else if(type == HandleTargetType.HandL)
			{ nameList[0] = handLPropertyNameX; nameList[1] = handLPropertyNameY; nameList[2] = handLPropertyNameZ;}
			else
			{ nameList[0] = handRPropertyNameX; nameList[1] = handRPropertyNameY; nameList[2] = handRPropertyNameZ;}
			return nameList;
		}
		static string GetPropertyCurvePath(string animName, string propertyName)
		{
			return Application.dataPath +  "/.." + "/TempAnimData" + $"/{animName}"+ "/" + propertyName;;
		}
		public static string GetRootYCurvePath(string animName)
		{
			return GetPropertyCurvePath(animName, rootPropertyNameY);
		}
		public static string[] GetAllCurvePathByType(HandleTargetType type, string animName)
		{
			string[] pathList = new string[3];
			string pathRootX = GetPropertyCurvePath(animName, rootPropertyNameX);
			string pathRootY = GetPropertyCurvePath(animName, rootPropertyNameY);
			string pathRootZ = GetPropertyCurvePath(animName, rootPropertyNameZ);

			string pathHandLX = GetPropertyCurvePath(animName, handLPropertyNameX);
			string pathHandLY = GetPropertyCurvePath(animName, handLPropertyNameY);
			string pathHandLZ = GetPropertyCurvePath(animName, handLPropertyNameZ);

			string pathHandRX = GetPropertyCurvePath(animName, handRPropertyNameX);
			string pathHandRY = GetPropertyCurvePath(animName, handRPropertyNameY);
			string pathHandRZ = GetPropertyCurvePath(animName, handRPropertyNameZ);

			if(type == HandleTargetType.Root)
			{ pathList[0] = pathRootX; pathList[1] = pathRootY; pathList[2] = pathRootZ;}
			else if(type == HandleTargetType.HandL)
			{ pathList[0] = pathHandLX; pathList[1] = pathHandLY; pathList[2] = pathHandLZ;}
			else
			{ pathList[0] = pathHandRX; pathList[1] = pathHandRY; pathList[2] = pathHandRZ;}
			return pathList;
		}

		public static string GetExplainByCurveEnu(GenerateNewCurveType type)
		{
			if(type == GenerateNewCurveType.GenerateStraight)
			{
				return "整体直接叠加直线";
			}
			else if(type == GenerateNewCurveType.GenerateInverse)
			{
				return "整体叠加反比例曲线";
			}
			else if(type == GenerateNewCurveType.GenerateFindEnd)
			{
				return "尾端曲线平缓处叠加";
			}
			else if(type == GenerateNewCurveType.GenerateBySetTimeGoAndBack)
			{
				return "先叠加，再反向叠加";
			}
			// else if(type == GenerateNewCurveType.GenerateBySetTimeStraight)
			// {
			// 	return "通过时间设置叠加直线";
			// }
			else if(type == GenerateNewCurveType.GenerateBySetTimeHole)
			{
				return "通过时间设置将时间段内整体叠加";
			}
			return "";

		}
		
		public static string GetExplainByTimeEnu(TimeSampleTimeType type)
		{
			// if(type == TimeSampleTimeType.startSit)
			// {
			// 	return "曲线开始叠加的时间段";
			// }
			if(type == TimeSampleTimeType.inSit)
			{
				return "Y轴曲线或手从开始叠加到基本完成的时间段";
			}
			// else if(type == TimeSampleTimeType.endSit)
			// {
			// 	return "最后完成的时间段";
			// }
			return "";
		}
		public static string GetExplainByPresetEnu(SamplePresetType type)
		{
			if(type == SamplePresetType.ForSit)
			{
				return "仅记录root动画，hand变化跟随root, 坐姿解决方案";
			}
			if(type == SamplePresetType.ForTake)
			{
				return "伸手探物解决方案，配置手部挂点将会带有颈椎旋转";
			}
			if(type == SamplePresetType.ForLie)
			{
				return "躺姿解决方案，一般适用于仅配置spine";
			}
			if(type == SamplePresetType.Customize)
			{
				return "全部单独自定义记录";
			}
			return "";
		}
		public static string GetSavePath()
		{
			return Application.dataPath + "/.." + "/TempAnimData";
		}
	}
	public enum HandleTargetType
	{
		Root = 0,
		HandL = 1,
		HandR = 2,
		Spine = 3,
		FootL = 11,//不需记录的类型
		FootR = 12,//不需记录的类型
		Max = 4,
	}
	public enum AnimStateType
	{
		Start = 0,
		Loop = 1,
		End = 2
	}
	public enum GenNewCurveType
	{
		GenToilet = 1,
		GenNormalSit = 2,
		GenHalf = 3,
		GenBySet = 4
	}

	public enum SamplePresetType
	{
		ForSit = 0,//仅记录Root，end变化为start的反向
		ForTake = 1,//伸手解决方案
		ForLie = 2,//躺下解决方案
		Customize = 10,//全部单独记录
	}
	public enum TimeSampleTimeType
	{
		// startSit = 1, //曲线开始叠加的时间段(准备坐下)
		inSit = 2, //Y轴曲线或手从开始叠加到基本完成的时间段(正在坐下)
		// endSit = 3 //最后完成的时间段(坐下完成)
	}
	public enum GenerateNewCurveType
	{
		GenerateStraight = 1, //整体直接叠加直线

		GenerateInverse = 2, //整体叠加反比例曲线

		GenerateFindEnd = 3, //尾端曲线平缓处叠加

		// GenerateFindFirstDown = 4, //从曲线首个最低点开始叠加

		// GenerateBySetTimeStraight = 5,//通过时间设置将时间段内线性叠加
		GenerateBySetTimeHole = 6,//通过时间设置将时间段内整体叠加
		GenerateBySetTimeGoAndBack = 7
	}
	public enum AnimNumType
	{
		Single = 0,
		Twice = 1,
		Third = 2
	}
}