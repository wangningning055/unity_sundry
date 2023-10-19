using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace vw_animation_ik_runtime
{
	public class ResolveCurveForSit:ResolveCurveBase
	{
		public static float GetRootYTime(GenerateNewCurveType _generateNewCurveType, List<Keyframe> animKeyList, bool isRevert, float startTime)
		{
			if(_generateNewCurveType == GenerateNewCurveType.GenerateFindEnd)
			{
				float time = 0;
				GetEndSmoothIndex(animKeyList, isRevert, out time);
				return time;
			}
			else if(_generateNewCurveType == GenerateNewCurveType.GenerateStraight)
			{
				return 0;
			}
			return 0;
		}

		//处理末端逐渐减少并趋于平滑的原曲线（末端反比例函数）；抬高平滑的尾端；生成Y轴方向的根节点新曲线,用于一般坐姿
		public static AnimationCurve GetCurveSmoothEnd(List<Keyframe> animKeyList, float changeDis, bool isRevert, out float smoothTime ,HandleTargetType type)
		{
			int smoothStartIndex = 0;
			float time = 0;
			smoothStartIndex = GetEndSmoothIndex(animKeyList, isRevert, out time);
			smoothTime = time;
			return GenerateNewCurve(animKeyList, changeDis, smoothStartIndex, isRevert, type);
		}
		public static int GetEndSmoothIndex(List<Keyframe> animKeyList, bool isRevert, out float time)
		{

			float largeValue = 0;
			float minimumValue = Mathf.Infinity;
			int smoothStartIndex = 0;
			int largeValueIndex = 0;
			int minimumValueIndex = 0;
			//查找最高点和最低点
			for(int j = 0;j < animKeyList.Count; j++)
			{
				Keyframe key = animKeyList[j];
				if(key.value > largeValue)
				{
					largeValue = key.value;
					largeValueIndex = j;
				}
				if(key.value < minimumValue)
				{
					minimumValue = key.value;
					minimumValueIndex = j;
				}
			}
			//末端向前查找，变化量低于1/5记录为平滑点
			float endDif = (largeValue - minimumValue) / 5;
			for(int j = animKeyList.Count - 1 ; j > 0; j--)
			{
				float curDif = Mathf.Abs(animKeyList[animKeyList.Count - 1].value - animKeyList[j].value);
				if(curDif > endDif)
				{
					break;
				}
				smoothStartIndex = j;
			}
			//从前向后查找，变化量开始大于1/5记录为平滑点
			if(isRevert)
			{
				for(int j = 0 ; j < animKeyList.Count  - 1 ; j++)
				{
					float curDif = Mathf.Abs(animKeyList[0].value - animKeyList[j].value);
					smoothStartIndex = j;

					if(curDif > endDif)
					{
						break;
					}
				}
			}
			time = animKeyList[smoothStartIndex].time;
			return smoothStartIndex;
		}

		public static AnimationCurve[] GetAnimNewCurveByForSit(HandleTargetType type, Vector3 changes, IKSampleData _ikSampleData, out float completeTime)
		{
			float time = 0;
			bool isEnd = _ikSampleData.isNeedRevert;
			// GenerateNewCurveType _generateNewCurveTypes = _ikSampleData.GetPartData(type)._generateNewCurveType;
			// float[] partTime = _ikSampleData.GetPartData(type).time;
			List<Keyframe>[] keyFrameList = _ikSampleData.GetCurveData(type).GetKeyframeList();
			
			// GenerateNewCurveType _rootGenerateNewCurveTypes = _ikSampleData.GetPartData(HandleTargetType.Root)._generateNewCurveType;
			float[] rootPartTime = _ikSampleData.GetPartData(HandleTargetType.Root).time;
			// List<Keyframe>[] rootKeyFrameList = _ikSampleData.GetCurveData(HandleTargetType.Root).GetKeyframeList();
			AnimationCurve[] newRoots = ResolveCurveMgr.Instance.GeneralGenerateCurve(HandleTargetType.Root, changes, _ikSampleData, out time);

			if(type == HandleTargetType.Root)
			{
				_ikSampleData.GetPartData(HandleTargetType.Root).time[1] = time;
				completeTime = time;
				return newRoots;
			}
			else
			{
				AnimationCurve[] newHands = new AnimationCurve[]{InitNewCurveByYTangent(time, keyFrameList[0], changes.x, isEnd),
					InitNewCurveByYTangent(time, keyFrameList[1], changes.y, isEnd),
					InitNewCurveByYTangent(time, keyFrameList[2], changes.z, isEnd),
				};
				completeTime = rootPartTime[1];
				return newHands;
			}
		}

	}
}
