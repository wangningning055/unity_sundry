using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using XLua;
namespace vw_animation_ik_runtime
{
	[LuaCallCSharp]
	public class TrackBindingData : ScriptableObject
	{
		public Transform[] ikPrepareTrans;
		public void SetData(Transform sit, Transform suitRoot)
		{
			ikPrepareTrans = new Transform[]{sit, suitRoot};
		}
	}
	public class KeyFramePool{
		public List<Keyframe> keys;
		int MaxLength = 200;
		public int newInt = 0;
		public Keyframe GetKey()
		{
			if(keys == null) keys = new List<Keyframe>();
			if(keys.Count <= 0)
			{
				newInt++;
				return new Keyframe();
			}
			Keyframe newKey = keys[0];
			keys.Remove(newKey);
			return newKey;
		}
		public void ReturnKey(Keyframe key)
		{
			// if(keys.Count < MaxLength)
			keys.Add(key);
		}
	}
	public class ResolveCurveMgr
	{
		static ResolveCurveMgr _resolveCurveMgr;
		public KeyFramePool _keyFramePool;
		public static ResolveCurveMgr Instance {
			get
			{
				if(_resolveCurveMgr == null)
				{
					_resolveCurveMgr = new ResolveCurveMgr();
					_resolveCurveMgr._keyFramePool = new KeyFramePool();
				}
				return _resolveCurveMgr;
			}
		}


		public AnimationCurve[] GeneralGenerateCurve(HandleTargetType type, Vector3 changeDis, IKSampleData _ikSampleData, out float time)
		{

			AnimationCurve[] newAc = new AnimationCurve[3];
			List<Keyframe>[] animKeyLists = _ikSampleData.GetCurveData(type).GetKeyframeList();
			 IKPartsSampleData partData = _ikSampleData.GetPartData(type);

			bool isRevert = _ikSampleData.isNeedRevert;
			GenerateNewCurveType _generateNewCurveType = partData._generateNewCurveType;
			float startTime = partData.time[0];
			float endTime = partData.time[1];
			float middleTime = _ikSampleData.OriginalPosTime;
			time = 0;
			// if(_generateNewCurveType == GenerateNewCurveType.GenerateBySetTimeStraight)
			// {

			// 	newAc[1] = InitNewCurveByStraight(animKeyLists[1],changeDis.y, isRevert, startTime, endTime);
			// 	newAc[0] = InitNewCurveByYTangent(startTime, animKeyLists[0], changeDis.x, isRevert);
			// 	newAc[2] = InitNewCurveByYTangent(startTime, animKeyLists[2], changeDis.z, isRevert);
			// 	//根节点y与手不同
			// 	if(type != HandleTargetType.Root)
			// 	{
			// 		newAc[2] = InitNewCurveByStraight(animKeyLists[2],changeDis.z, isRevert, startTime, endTime);
			// 		newAc[0] = InitNewCurveByStraight(animKeyLists[0],changeDis.x, isRevert, startTime, endTime);
			// 	}
			// }
			if(_generateNewCurveType == GenerateNewCurveType.GenerateBySetTimeHole)
			{
				
				newAc[0] = ResolveCurveBase.InitNewCurveByHole(animKeyLists[0],changeDis.x, isRevert, startTime, endTime);
				newAc[1] = ResolveCurveBase.InitNewCurveByHole(animKeyLists[1],changeDis.y, isRevert, startTime, endTime);
				newAc[2] = ResolveCurveBase.InitNewCurveByHole(animKeyLists[2],changeDis.z, isRevert, startTime, endTime);
				time = endTime;
			}
			else if(_generateNewCurveType == GenerateNewCurveType.GenerateFindEnd)
			{
				newAc[1] = ResolveCurveForSit.GetCurveSmoothEnd(animKeyLists[1], changeDis.y, isRevert, out time, type);
				newAc[0] = ResolveCurveBase.InitNewCurveByYTangent(time, animKeyLists[0], changeDis.x, isRevert);
				newAc[2] = ResolveCurveBase.InitNewCurveByYTangent(time, animKeyLists[2], changeDis.z, isRevert);
				// Debug.Log($"{type},  {time}");
			}
			// else if(_generateNewCurveType == GenerateNewCurveType.GenerateFindFirstDown)
			// {
			// 	float time = 0;
			// 	newAc[1] = GetCurveToilet(animKeyLists[1], changeDis.y, isRevert, out time);
			// 	newAc[0] = InitNewCurveByYTangent(time, animKeyLists[0], changeDis.x, isRevert);
			// 	newAc[2] = InitNewCurveByYTangent(time, animKeyLists[2], changeDis.z, isRevert);
				
			// }		
			else if(_generateNewCurveType == GenerateNewCurveType.GenerateInverse)
			{

				time = animKeyLists[1][animKeyLists[1].Count - 1].time;
				newAc[1] = ResolveCurveBase.InitNewCurveByInverse(animKeyLists[1], changeDis.y, isRevert, 0, time);
				newAc[0] = ResolveCurveBase.InitNewCurveByInverse(animKeyLists[0], changeDis.x, isRevert, 0, time);
				newAc[2] = ResolveCurveBase.InitNewCurveByInverse(animKeyLists[2], changeDis.z, isRevert, 0, time);
			}		
			else if(_generateNewCurveType == GenerateNewCurveType.GenerateStraight)
			{
	
				time = 0;//animKeyLists[1][animKeyLists[1].Count - 1].time;
				newAc[0] = ResolveCurveBase.InitNewCurveByHole(animKeyLists[0],changeDis.x, isRevert, 0, time);
				newAc[1] = ResolveCurveBase.InitNewCurveByHole(animKeyLists[1],changeDis.y, isRevert, 0, time);
				newAc[2] = ResolveCurveBase.InitNewCurveByHole(animKeyLists[2],changeDis.z, isRevert, 0, time);

			}
			else if(_generateNewCurveType == GenerateNewCurveType.GenerateBySetTimeGoAndBack)
			{
				time = endTime;
				// Debug.Log($"asdasdasdasdad{startTime},   {endTime},  {middleTime}");
				newAc[0] = ResolveCurveBase.GenerateGoAndBack(animKeyLists[0],changeDis.x, startTime, endTime, middleTime);
				newAc[1] = ResolveCurveBase.GenerateGoAndBack(animKeyLists[1],changeDis.y, startTime, endTime, middleTime);
				newAc[2] = ResolveCurveBase.GenerateGoAndBack(animKeyLists[2],changeDis.z, startTime, endTime, middleTime);
			}
			return newAc;
		}

	
		public AnimationCurve[] GetAnimNewCurveByGeneralSets(HandleTargetType type, Vector3 changes, IKSampleData _ikSampleData, out float time)
		{
			if(_ikSampleData._samplePresetType == SamplePresetType.Customize)
				return GeneralGenerateCurve(type, changes, _ikSampleData, out time);
			if(_ikSampleData._samplePresetType == SamplePresetType.ForSit)
				return ResolveCurveForSit.GetAnimNewCurveByForSit(type, changes, _ikSampleData, out time);
			if(_ikSampleData._samplePresetType == SamplePresetType.ForTake)
				return ResolveCurveForTake.GetAnimNewCurveByForTake(type, changes, _ikSampleData, out time);
			if(_ikSampleData._samplePresetType == SamplePresetType.ForLie)
				return GeneralGenerateCurve(type, changes, _ikSampleData, out time);
			time = 0;
			return new AnimationCurve[1];
		}

		public float[] GetCurveEdgeValue(AnimationCurve curve)
		{

			float max = -1000;
			float min = 1000;
			float time = curve[curve.length - 1].time;
			for(int i = 0; i < curve.length; i++)
			{
				if(curve[i].value > max)
					max = curve[i].value;
				if(curve[i].value < min)
					min = curve[i].value;
			}
			return new float[]{max, min, time};
		}

		//初始化原曲线
		public AnimationCurve[] InitOldCurve(List<Keyframe>[] totalAnimKeyList)
		{
			// Func<List<Keyframe>, AnimationCurve> getCurve = (list) =>
			// {
			// 	AnimationCurve oldCurve = new AnimationCurve();
		
			// 	return oldCurve;
			// };
			AnimationCurve[] oldCurve = new AnimationCurve[3];
		
			for(int i = 0; i < totalAnimKeyList.Length; i++)
			{
				oldCurve[i] = new AnimationCurve();
				foreach(Keyframe key in totalAnimKeyList[i])
				{
					oldCurve[i].AddKey(key);
				}
			}
			return oldCurve;
		}

	}
}