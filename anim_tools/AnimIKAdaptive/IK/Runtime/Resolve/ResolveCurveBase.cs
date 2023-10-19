using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace vw_animation_ik_runtime
{
	public class ResolveCurveBase
	{

		public static AnimationCurve GenerateNewCurve(List<Keyframe> animKeyList, float changeDis, int smoothStartIndex, bool isRevert,
			HandleTargetType type = HandleTargetType.HandL)
		{
			
			//从平滑点开始向前查找，按点的高度判断，平滑超出高度导致斜率不合理的点,构建新曲线
			AnimationCurve newCurve = new AnimationCurve();
			float oldSmoothValue = animKeyList[smoothStartIndex].value;
			float newSmoothValue = oldSmoothValue + changeDis;
			int unSafeIndex = smoothStartIndex;
			for(int j = smoothStartIndex - 1 ; j > 0; j--)
			{
				float curValue = animKeyList[j].value;
				if((oldSmoothValue < curValue && newSmoothValue < curValue) || (oldSmoothValue > curValue && newSmoothValue > curValue))
				{
					break;
				}
				if(j > 0) j = j - 1;
				unSafeIndex = j;

			}
			//从平滑点开始向后查找，按点的高度判断，平滑超出高度导致斜率不合理的点,构建新曲线
			if(isRevert)
			{
				for(int j = smoothStartIndex + 1 ; j < animKeyList.Count; j++)
				{
					float curValue = animKeyList[j].value;
					if((oldSmoothValue < curValue && newSmoothValue < curValue) || (oldSmoothValue > curValue && newSmoothValue > curValue))
					{
						break;
					}
					if(j < animKeyList.Count - 1) j = j + 1;
					unSafeIndex = j;
				}
			}
			float unSafeValue = animKeyList[unSafeIndex].value;
			for(int i = 0; i < animKeyList.Count; i++)
			{
				Keyframe oldKey = animKeyList[i];
				// float inTangent = oldKey.inTangent;
				// float outTangent = oldKey.outTangent;

				float curChangeDis = i >= smoothStartIndex? changeDis:0;
				if(isRevert)
				{
					curChangeDis = i <= smoothStartIndex? changeDis:0;	
					
				}


				// float time = oldKey.time;
				oldKey.value = oldKey.value + curChangeDis;
				// float inWeight = oldKey.inWeight;
				// float outWeight = oldKey.outTangent;
				if(type == HandleTargetType.Root)
				{
					// Debug.Log($"{unSafeIndex} , {smoothStartIndex}");
				}
				if((i > unSafeIndex && i < smoothStartIndex)||(i < unSafeIndex && i > smoothStartIndex))
				{
					float dis = unSafeValue - newSmoothValue;
					float count = Mathf.Abs(unSafeIndex - smoothStartIndex) + 2;
					oldKey.value = unSafeValue -  dis * Mathf.Abs(i - unSafeIndex) / count;
					// float newTangent = (animKeyList[smoothStartIndex].inTangent + animKeyList[unSafeIndex].inTangent) / 2;
					float newTangent = dis / (unSafeIndex - smoothStartIndex);
					oldKey.inTangent = newTangent;
					oldKey.outTangent = newTangent;
				}

				// if(unSafeIndex != smoothStartIndex - 1 && (i > unSafeIndex && i < smoothStartIndex))
				// {
				// 	float scale = (i - unSafeIndex) / (smoothStartIndex - unSafeIndex);
				// 	value =  Mathf.Lerp(unSafeValue, newSmoothValue, scale);
				// 	inTangent = Mathf.Lerp(animKeyList[unSafeIndex].inTangent, animKeyList[smoothStartIndex].inTangent, scale);
				// 	outTangent = Mathf.Lerp(animKeyList[unSafeIndex].outTangent, animKeyList[smoothStartIndex].outTangent, scale);
					
				// }
				// oldKey.inTangent = inTangent;
				// oldKey.outTangent = outTangent;
				// Keyframe newKey = new Keyframe(time, value, inTangent, outTangent, inWeight, outWeight);
				// newKey.weightedMode = oldKey.weightedMode;
				// newCurve.AddKey(newKey);
				// newList.Add(newKey);
				animKeyList[i] = oldKey;
				newCurve.AddKey(oldKey);
			}
			return newCurve;
		}

		//生成没有平滑的新曲线
		public static AnimationCurve GenerateNewCurveNoLerp(List<Keyframe> animKeyList, float changeDis, int smoothStartIndex, bool isRevert, HandleTargetType type = HandleTargetType.HandL)
		{
			
			//从平滑点开始向前查找，按点的高度判断，平滑超出高度导致斜率不合理的点,构建新曲线
			AnimationCurve newCurve = new AnimationCurve();
			float oldSmoothValue = animKeyList[smoothStartIndex].value;
			float newSmoothValue = oldSmoothValue + changeDis;
			
			
			for(int i = 0; i < animKeyList.Count; i++)
			{
				Keyframe oldKey = animKeyList[i];

				float curChangeDis = i >= smoothStartIndex? changeDis:0;
				if(isRevert)
				curChangeDis = i <= smoothStartIndex? changeDis:0;	
				float time = oldKey.time;
				oldKey.value = oldKey.value + curChangeDis;

				newCurve.AddKey(oldKey);

				// Keyframe newKey = new Keyframe(time, value, inTangent, outTangent, inWeight, outWeight);
			
				// newKey.weightedMode = oldKey.weightedMode;
				// newCurve.AddKey(newKey);
			}
			return newCurve;
		}

				//在曲线上简单叠加反比例函数；生成新曲线
		public static AnimationCurve InitNewCurveByInverse(List<Keyframe> animKeyList, float changeDis, bool isRevert, float startTime, float endTime)
		{
			AnimationCurve newCurve = new AnimationCurve();
			float powData = 1.5f;
			endTime = endTime <= 0? animKeyList[animKeyList.Count - 1].time:endTime;
			float totalTime = endTime - startTime;
			float singleChangeDis = changeDis / totalTime;
			float littleChange = (changeDis + Mathf.Pow(powData, startTime) - Mathf.Pow(powData, endTime) ) / totalTime;
			endTime = endTime <= startTime? startTime + 0.1f:endTime;
			float powStart = Mathf.Pow(powData, startTime);
			float powEnd = Mathf.Pow(powData, endTime);
			//新建曲线
			for(int i = 0; i < animKeyList.Count; i++)
			{
				Keyframe oldKey = animKeyList[i];
				float time = animKeyList[i].time;

				float dif = (Mathf.Pow(powData, time) - powStart) * changeDis / (powEnd - powStart);

				float change = time > startTime && time < endTime? dif : time <= startTime? 0: changeDis;
				if(isRevert)
					change = time > startTime && time < endTime? changeDis - dif : time <= startTime? changeDis: 0;
				
				
				oldKey.value =oldKey.value + change;
				animKeyList[i] = oldKey;
				// newList.Add(oldKey);

				// Keyframe newKey = new Keyframe(time, value, inTangent, outTangent, inWeight, outWeight);
				// newKey.weightedMode = oldKey.weightedMode;
				// newList.Add(newKey);
			}
			for(int i = 1; i < animKeyList.Count - 1 ; i++)
			{
				Keyframe key = animKeyList[i];
				float tangent =  (animKeyList[i + 1].value - animKeyList[i - 1].value)/(animKeyList[i + 1].time - animKeyList[i - 1].time);
				key.inTangent = tangent;
				key.outTangent = tangent;
				animKeyList[i] = key;
			}
			for(int i = 0; i < animKeyList.Count ; i++)
			{
				newCurve.AddKey(animKeyList[i]);
			}
			return newCurve;
		}

		//整体抬高曲线；生成新曲线，用于loop
		public static AnimationCurve InitNewCurveByHole(List<Keyframe> animKeyList, float changeDis, bool isRevert, float startTime, float endTime)
		{
			AnimationCurve newCurve = new AnimationCurve();
			endTime = endTime <= 0? animKeyList[animKeyList.Count - 1].time:endTime;
			float totalTime = endTime - startTime;
			float singleChangeDis = changeDis / totalTime;
			int startIndex = -1;
			for(int i = 0; i < animKeyList.Count; i++)
			{
				Keyframe oldKey = animKeyList[i];
				float time = oldKey.time;
				// float dis = time >= startTime && time <= endTime? singleChangeDis * (time - startTime): time < startTime? 0:changeDis;
				// if(isRevert) dis = time >= startTime && time <= endTime? changeDis - singleChangeDis * (time - startTime): time < startTime? 0:changeDis;
				// float value = oldKey.value + dis;
				oldKey.value = oldKey.value + changeDis;
				newCurve.AddKey(oldKey);

				// Keyframe newKey = new Keyframe(time, value, inTangent, outTangent, inWeight, outWeight);
				// newKey.weightedMode = oldKey.weightedMode;
				// newCurve.AddKey(newKey);
				if(time >= startTime && startIndex < 0)
					startIndex = i;
			}
			return GenerateNewCurve(animKeyList, changeDis, startIndex, isRevert);
			// return newCurve;
			
		}
		//在曲线上简单叠加线性函数；生成新曲线
		public static AnimationCurve InitNewCurveByStraight(List<Keyframe> animKeyList, float changeDis, bool isRevert, float startTime, float endTime)
		{
			AnimationCurve newCurve = new AnimationCurve();


			int index = 0;
			//新建曲线
			for(int i = 0; i < animKeyList.Count; i++)
			{
				if(!isRevert && animKeyList[i].time > startTime)
				{
					index = i; break;
				}
				if(isRevert && animKeyList[animKeyList.Count - i - 1].time < startTime)
				{
					index = i; break;
				}
			}
			return GenerateNewCurve(animKeyList, changeDis, index, isRevert);
			
			// return GenerateNewCurveNoLerp(animKeyList, changeDis, index, isRevert);
			// return newCurve;
			
		}

		//在time之前叠加线性函数，生成新曲线,用于手部移动或根节点的横向移动
		public static AnimationCurve InitNewCurveByYTangent(float Ytime, List<Keyframe> animKeyList, float changeDis, bool isRevert)
		{
			// int smoothStartIndexY = GetEndSmoothIndex(animKeyListY, isRevert);

			// float smoothStartTime = animKeyListY[smoothStartIndexY].time;
			float singleChangeDis = changeDis / Ytime;
			float totalTime = animKeyList[animKeyList.Count - 1].time;
			AnimationCurve newCurve = new AnimationCurve();
			float startTime = Ytime == 0? 0:Ytime - 0.5f;
			if(Ytime != 0)
				return InitNewCurveByInverse(animKeyList, changeDis, isRevert,startTime, Ytime);
			else
				return InitNewCurveByHole(animKeyList, changeDis, isRevert,startTime, Ytime);


		}
		public static int GetIndexByTime(List<Keyframe> animKeyList, float time)
		{
			int middleIndex = 0;
			for(int i = 0; i < animKeyList.Count; i++)
			{
				if(animKeyList[i].time > time)
				{
					middleIndex = i;
					break;
				}
			}

			return middleIndex;
		}

		public static AnimationCurve GenerateGoAndBack(List<Keyframe> animKeyList, float changeDis, float startTime, float endTime, float middleTime)
		{
			AnimationCurve newCurve = new AnimationCurve();
			//开始叠加和叠加结束时间必须在目标点时间两侧
			if(middleTime == 0) middleTime = (startTime + endTime) / 2;
			endTime = endTime < middleTime? middleTime + 0.5f:endTime;
			startTime = startTime > middleTime? middleTime - 0.5f:startTime;
			int startIndex = GetIndexByTime(animKeyList, startTime);
			int endIndex = GetIndexByTime(animKeyList, endTime);
			int middleIndex = GetIndexByTime(animKeyList, middleTime);
			List<Keyframe> startList = new List<Keyframe>();
			List<Keyframe> endList = new List<Keyframe>();
			for(int i = 0; i < animKeyList.Count; i++)
			{
				if(i < middleIndex)
				{
					startList.Add(animKeyList[i]);
				}
				else
				{
					endList.Add(animKeyList[i]);
				}
			}
			InitNewCurveByInverse(startList, changeDis, false, startTime, startList[startList.Count - 1].time);
			InitNewCurveByInverse(endList, changeDis, true, endList[0].time, endTime);
	
			for(int i = 0; i < endList.Count - 1; i++)
			{
				startList.Add(endList[i]);
			}
			// for(int i = 0; i < endList.Count - 1; i++)
			// {
			// 	startList.Add(endList[i]);
			// }
			// startList.AddRange(newEndList);
		
			for(int i = 1; i < startList.Count - 1 ; i++)
			{
				Keyframe key = startList[i];
				float up = startList[i + 1].value - startList[i - 1].value;
				float down = startList[i + 1].time - startList[i - 1].time;
				float tangent =  up/down;
				key.inTangent = tangent;
				key.outTangent = tangent;
				startList[i] = key;

			}
			for(int i = 0; i < startList.Count ; i++)
			{
				newCurve.AddKey(startList[i]);
			}
			return newCurve;
		}


	}
}