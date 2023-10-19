// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using System;
// namespace vw_animation_ik_runtime
// {
// 	public class ResolveCurveData
// 	{
	
// 		//初始化原曲线
// 		public static AnimationCurve[] InitOldCurve(List<Keyframe>[] totalAnimKeyList)
// 		{
// 			Func<List<Keyframe>, AnimationCurve> getCurve = (list) =>
// 			{
// 				AnimationCurve oldCurve = new AnimationCurve();
// 				foreach(Keyframe key in list)
// 				{
// 					oldCurve.AddKey(key);
// 				}
// 				return oldCurve;
// 			};
// 			AnimationCurve[] oldCurve = new AnimationCurve[3];
		
// 			for(int i = 0; i < totalAnimKeyList.Length; i++)
// 			{
	
// 				oldCurve[i] = getCurve(totalAnimKeyList[i]);
// 			}

// 			return oldCurve;
// 		}
// 		//在time之前叠加线性函数，生成新曲线,用于手部移动或根节点的横向移动
// 		public static AnimationCurve InitNewCurveByYTangent(float Ytime, List<Keyframe> animKeyList, float changeDis, bool isRevert)
// 		{
// 			// int smoothStartIndexY = GetEndSmoothIndex(animKeyListY, isRevert);

// 			// float smoothStartTime = animKeyListY[smoothStartIndexY].time;
// 			float singleChangeDis = changeDis / Ytime;
// 			float totalTime = animKeyList[animKeyList.Count - 1].time;
// 			AnimationCurve newCurve = new AnimationCurve();
// 			List<Keyframe> newList = new List<Keyframe>();
// 			float startTime = Ytime == 0? 0:Ytime - 0.5f;
// 			if(Ytime != 0)
// 				return InitNewCurveByInverse(animKeyList, changeDis, isRevert,startTime, Ytime, out newList);
// 			else
// 				return InitNewCurveByHole(animKeyList, changeDis, isRevert,startTime, Ytime);

// 			//新建曲线
// 			// for(int i = 0; i < animKeyList.Count; i++)
// 			// {
// 			// 	Keyframe oldKey = animKeyList[i];
// 			// 	float change = changeDis;
// 			// 	bool condition = isRevert? oldKey.time >= Ytime : oldKey.time <= Ytime;
// 			// 	if(condition && Ytime > 0)
// 			// 	{
// 			// 		change = isRevert? changeDis *((totalTime - oldKey.time) / (totalTime - Ytime)) : changeDis * (oldKey.time / Ytime);
// 			// 	}
// 			// 	float inTangent = oldKey.inTangent;
// 			// 	float outTangent = oldKey.outTangent;
// 			// 	float time = oldKey.time;
// 			// 	float value = oldKey.value + change;
// 			// 	float inWeight = oldKey.inWeight;
// 			// 	float outWeight = oldKey.outTangent;
// 			// 	// if(time >= Ytime + 0.5f )
// 			// 	// {
// 			// 	// 	value = oldKey.value + changeDis;
// 			// 	// }
// 			// 	Keyframe newKey = new Keyframe(time, value, inTangent, outTangent, inWeight, outWeight);
// 			// 	newKey.weightedMode = oldKey.weightedMode;
// 			// 	newCurve.AddKey(newKey);
// 			// }
// 			// return newCurve;
// 		}

// 		//在曲线上简单叠加线性函数；生成新曲线
// 		public static AnimationCurve InitNewCurveByStraight(List<Keyframe> animKeyList, float changeDis, bool isRevert, float startTime, float endTime)
// 		{
// 			AnimationCurve newCurve = new AnimationCurve();


// 			int index = 0;
// 			//新建曲线
// 			for(int i = 0; i < animKeyList.Count; i++)
// 			{
// 				if(!isRevert && animKeyList[i].time > startTime)
// 				{
// 					index = i; break;
// 				}
// 				if(isRevert && animKeyList[animKeyList.Count - i - 1].time < startTime)
// 				{
// 					index = i; break;
// 				}
// 			}
// 			List<Keyframe> list = new List<Keyframe>();
// 			return GenerateNewCurve(animKeyList, changeDis, index, isRevert, out list);
			
// 			// return GenerateNewCurveNoLerp(animKeyList, changeDis, index, isRevert);
// 			// return newCurve;
			
// 		}

// 		//整体抬高曲线；生成新曲线，用于loop
// 		public static AnimationCurve InitNewCurveByHole(List<Keyframe> animKeyList, float changeDis, bool isRevert, float startTime, float endTime)
// 		{
// 			AnimationCurve newCurve = new AnimationCurve();
// 			endTime = endTime <= 0? animKeyList[animKeyList.Count - 1].time:endTime;
// 			float totalTime = endTime - startTime;
// 			float singleChangeDis = changeDis / totalTime;
// 			int startIndex = -1;
// 			for(int i = 0; i < animKeyList.Count; i++)
// 			{
// 				Keyframe oldKey = animKeyList[i];
// 				float inTangent = oldKey.inTangent;
// 				float outTangent = oldKey.outTangent;
// 				float time = oldKey.time;
// 				// float dis = time >= startTime && time <= endTime? singleChangeDis * (time - startTime): time < startTime? 0:changeDis;
// 				// if(isRevert) dis = time >= startTime && time <= endTime? changeDis - singleChangeDis * (time - startTime): time < startTime? 0:changeDis;
// 				// float value = oldKey.value + dis;
// 				float value = oldKey.value + changeDis;
// 				float inWeight = oldKey.inWeight;
// 				float outWeight = oldKey.outTangent;
// 				Keyframe newKey = new Keyframe(time, value, inTangent, outTangent, inWeight, outWeight);
// 				newKey.weightedMode = oldKey.weightedMode;
// 				newCurve.AddKey(newKey);
// 				if(time >= startTime && startIndex < 0)
// 					startIndex = i;
// 			}

// 			List<Keyframe> newList;
// 			return GenerateNewCurve(animKeyList, changeDis, startIndex, isRevert, out newList);
// 			// return newCurve;
			
// 		}

// 		//在曲线上简单叠加反比例函数；生成新曲线
// 		public static AnimationCurve InitNewCurveByInverse(List<Keyframe> animKeyList, float changeDis, bool isRevert, float startTime, float endTime, out List<Keyframe> newList)
// 		{
// 			AnimationCurve newCurve = new AnimationCurve();
// 			float powData = 1.2f;
// 			endTime = endTime <= 0? animKeyList[animKeyList.Count - 1].time:endTime;
// 			float totalTime = endTime - startTime;
// 			float singleChangeDis = changeDis / totalTime;
// 			float littleChange = (changeDis + Mathf.Pow(powData, startTime) - Mathf.Pow(powData, endTime) ) / totalTime;

// 			float powStart = Mathf.Pow(powData, startTime);
// 			float powEnd = Mathf.Pow(powData, endTime);
// 			newList = new List<Keyframe>();
// 			//新建曲线
// 			for(int i = 0; i < animKeyList.Count; i++)
// 			{
// 				Keyframe oldKey = animKeyList[i];
// 				float inTangent = oldKey.inTangent;
// 				float outTangent = oldKey.outTangent;
// 				float time = oldKey.time;



// 				float dif = (Mathf.Pow(powData, time) - powStart) * changeDis / (powEnd - powStart);
		

// 				// float difValue = Mathf.Pow(2, time) - Mathf.Pow(2, startTime) + (time - startTime) * littleChange;

// 				float value =oldKey.value + (time >= startTime && time <= endTime? dif : time < startTime? 0: changeDis);

// 				if(isRevert)
// 					value =oldKey.value + (time >= startTime && time <= endTime? changeDis - dif : time < startTime? changeDis: 0);

// 				float inWeight = oldKey.inWeight;
// 				float outWeight = oldKey.outTangent;
				

// 				Keyframe newKey = new Keyframe(time, value, inTangent, outTangent, inWeight, outWeight);
// 				newKey.weightedMode = oldKey.weightedMode;
// 				newList.Add(newKey);
// 			}
// 			for(int i = 1; i < newList.Count - 1 ; i++)
// 			{
// 				// float change = animKeyList[i].value - newStartList[i].value;
// 				// if(startTime != 0)
// 				// Debug.Log($"顶顶顶顶顶顶顶顶顶顶顶顶顶{change},  {changeDis}  ,    {i},   {startIndex}");
// 				Keyframe key = newList[i];
// 				float tangent =  (newList[i + 1].value - newList[i - 1].value)/(newList[i + 1].time - newList[i - 1].time);
// 				key.inTangent = tangent;
// 				key.outTangent = tangent;
// 				newList[i] = key;
// 			}
// 			for(int i = 0; i < newList.Count ; i++)
// 			{
// 				newCurve.AddKey(newList[i]);
// 			}
// 			return newCurve;
			
// 		}

// 		//处理末端逐渐减少并趋于平滑的原曲线（末端反比例函数）；抬高平滑的尾端；生成Y轴方向的根节点新曲线,用于一般坐姿
// 		public static AnimationCurve GetCurveSmoothEnd(List<Keyframe> animKeyList, float changeDis, bool isRevert, out float smoothTime ,HandleTargetType type)
// 		{
// 			int smoothStartIndex = 0;
// 			float time = 0;
// 			smoothStartIndex = GetEndSmoothIndex(animKeyList, isRevert, out time);
// 			smoothTime = time;
// 			List<Keyframe> list = new List<Keyframe>();
// 			return GenerateNewCurve(animKeyList, changeDis, smoothStartIndex, isRevert, out list ,  type);
// 		}
		
// 		//于首次身体下降时就开始修正，抬高首次下降之后的所有尾端，特用于马桶
// 		public static AnimationCurve GetCurveToilet(List<Keyframe> animKeyList, float changeDis, bool isRevert, out float smoothTime)
// 		{
// 			int smoothMiddleIndex = 0;
// 			// if(isHaveTime)
// 			// {
// 			// 	smoothMiddleIndex = GetIndexByTime(animKeyList, time, isRevert);
// 			// }
// 			// else
// 			// {
// 				float time = 0;
// 				smoothMiddleIndex = GetMiddleSmoothIndex(animKeyList, isRevert, out time);
// 				smoothTime = animKeyList[smoothMiddleIndex].time;
// 			// }

// 			// for(int i = 0; i < animKeyList.Count - 1; i++)3 6
// 			// {
// 			// 	if(i >= smoothMiddleIndex[0] && i <= smoothMiddleIndex[1])
// 			// 	{
// 			// 		Keyframe oldKey = animKeyList[i];
// 			// 		oldKey.value += changeDis;
// 			// 		animKeyList[i] = oldKey; 
// 			// 	}
// 			// }
// 			List<Keyframe> list = new List<Keyframe>();
// 			return GenerateNewCurve(animKeyList, changeDis, smoothMiddleIndex, isRevert, out list);
// 		}

// 		public static int GetEndSmoothIndex(List<Keyframe> animKeyList, bool isRevert, out float time)
// 		{

// 			float largeValue = 0;
// 			float minimumValue = Mathf.Infinity;
// 			int smoothStartIndex = 0;
// 			int largeValueIndex = 0;
// 			int minimumValueIndex = 0;
// 			//查找最高点和最低点
// 			for(int j = 0;j < animKeyList.Count; j++)
// 			{
// 				Keyframe key = animKeyList[j];
// 				if(key.value > largeValue)
// 				{
// 					largeValue = key.value;
// 					largeValueIndex = j;
// 				}
// 				if(key.value < minimumValue)
// 				{
// 					minimumValue = key.value;
// 					minimumValueIndex = j;
// 				}
// 			}
// 			//末端向前查找，变化量低于1/5记录为平滑点
// 			float endDif = (largeValue - minimumValue) / 5;
// 			for(int j = animKeyList.Count - 1 ; j > 0; j--)
// 			{
// 				float curDif = Mathf.Abs(animKeyList[animKeyList.Count - 1].value - animKeyList[j].value);
// 				if(curDif > endDif)
// 				{
// 					break;
// 				}
// 				smoothStartIndex = j;
// 			}
// 			//从前向后查找，变化量开始大于1/5记录为平滑点
// 			if(isRevert)
// 			{
// 				for(int j = 0 ; j < animKeyList.Count  - 1 ; j++)
// 				{
// 					float curDif = Mathf.Abs(animKeyList[0].value - animKeyList[j].value);
// 					smoothStartIndex = j;

// 					if(curDif > endDif)
// 					{
// 						break;
// 					}
// 				}
// 			}
// 			time = animKeyList[smoothStartIndex].time;
// 			return smoothStartIndex;
// 		}

// 		//获取首次开始下降点，用于马桶坐姿
// 		public static int GetMiddleSmoothIndex(List<Keyframe> animKeyList, bool isRevert, out float time)
// 		{
			
// 			int startDownIndex = 0;
// 			int startUpIndex = 0;

// 			for(int j = 0;j < animKeyList.Count; j++)
// 			{
// 				Keyframe key = animKeyList[j];
// 				if(key.inTangent < -0.2)
// 				{
// 					startDownIndex = j;
// 					break;
// 				}
// 			}
// 			for(int j = startDownIndex;j < animKeyList.Count; j++)
// 			{
// 				Keyframe key = animKeyList[j];
// 				if(key.inTangent > 0)
// 				{
// 					startUpIndex = j;
// 					break;
// 				}
// 			}

// 			if(isRevert)
// 			{
// 				for(int j = animKeyList.Count - 1;j >= 0; j--)
// 				{
// 					Keyframe key = animKeyList[j];
// 					if(key.inTangent < -0.2)
// 					{
// 						startDownIndex = j;
// 						break;
// 					}
// 				}
// 				for(int j = startDownIndex;j >= 0; j--)
// 				{
// 					Keyframe key = animKeyList[j];
// 					if(key.inTangent > 0)
// 					{
// 						startUpIndex = j;
// 						break;
// 					}
// 				}
// 			}
// 			time = animKeyList[startDownIndex].time;
// 			return startDownIndex;
// 		}
// 		public static AnimationCurve GenerateGoAndBack(List<Keyframe> animKeyList, float changeDis, float startTime, float endTime)
// 		{
// 			// startTime = 1.2f;
// 			AnimationCurve newCurve = new AnimationCurve();
// 			int startIndex = GetIndexByTime(animKeyList, startTime, false);
// 			int endIndex = GetIndexByTime(animKeyList, endTime, false);
// 			List<Keyframe> startList = new List<Keyframe>();
// 			List<Keyframe> endList = new List<Keyframe>();
// 			for(int i = 0; i < animKeyList.Count; i++)
// 			{
// 				if(i <= startIndex)
// 				{
// 					startList.Add(animKeyList[i]);
// 				}
// 				else
// 				{
// 					endList.Add(animKeyList[i]);
// 				}
// 			}
// 			List<Keyframe> newStartList = new List<Keyframe>();
// 			List<Keyframe> newEndList = new List<Keyframe>();


// 			// AnimationCurve startCurve = GenerateNewCurve(startList, changeDis, startList.Count - 1, false, out newStartList);
// 			// AnimationCurve endCurve = GenerateNewCurve(endList, changeDis, 0, true, out newEndList);

// 			InitNewCurveByInverse(startList, changeDis, false, 0, 0, out newStartList);
// 			InitNewCurveByInverse(endList, changeDis, true, endList[0].time, endList[0].time + 0.5f, out newEndList);

// 			float startChange = startList[startList.Count - 1].value - newStartList[newStartList.Count - 1].value;
// 			float endChange = endList[0].value - newEndList[0].value;

// 			float startChange1 = startList[0].value - newStartList[0].value;
// 			float endChange1 = endList[endList.Count - 1].value - newEndList[newEndList.Count - 1].value;
// 			newStartList.AddRange(newEndList);
			
// 			for(int i = 1; i < newStartList.Count - 1 ; i++)
// 			{
// 				// float change = animKeyList[i].value - newStartList[i].value;
// 				// if(startTime != 0)
// 				// Debug.Log($"顶顶顶顶顶顶顶顶顶顶顶顶顶{change},  {changeDis}  ,    {i},   {startIndex}");
// 				Keyframe key = newStartList[i];
// 				float tangent =  (newStartList[i + 1].value - newStartList[i - 1].value)/(newStartList[i + 1].time - newStartList[i - 1].time);
// 				key.inTangent = tangent;
// 				key.outTangent = tangent;
// 				newStartList[i] = key;

// 			}
// 			for(int i = 0; i < newStartList.Count ; i++)
// 			{
// 				newCurve.AddKey(newStartList[i]);
// 			}

	
// 			return newCurve;
// 		}
// 		public static AnimationCurve GenerateNewCurve(List<Keyframe> animKeyList, float changeDis, int smoothStartIndex, bool isRevert,
// 			out List<Keyframe> newList, HandleTargetType type = HandleTargetType.HandL)
// 		{
			
// 			//从平滑点开始向前查找，按点的高度判断，平滑超出高度导致斜率不合理的点,构建新曲线
// 			AnimationCurve newCurve = new AnimationCurve();
// 			float oldSmoothValue = animKeyList[smoothStartIndex].value;
// 			float newSmoothValue = oldSmoothValue + changeDis;
// 			int unSafeIndex = smoothStartIndex;
// 			newList = new List<Keyframe>();
// 			for(int j = smoothStartIndex - 1 ; j > 0; j--)
// 			{
// 				float curValue = animKeyList[j].value;
// 				if((oldSmoothValue < curValue && newSmoothValue < curValue) || (oldSmoothValue > curValue && newSmoothValue > curValue))
// 				{
// 					break;
// 				}
// 				if(j > 0) j = j - 1;
// 				unSafeIndex = j;

// 			}
// 			//从平滑点开始向后查找，按点的高度判断，平滑超出高度导致斜率不合理的点,构建新曲线
// 			if(isRevert)
// 			{
// 				for(int j = smoothStartIndex + 1 ; j < animKeyList.Count; j++)
// 				{
// 					float curValue = animKeyList[j].value;
// 					if((oldSmoothValue < curValue && newSmoothValue < curValue) || (oldSmoothValue > curValue && newSmoothValue > curValue))
// 					{
// 						break;
// 					}
// 					if(j < animKeyList.Count - 1) j = j + 1;
// 					unSafeIndex = j;
// 				}
// 			}
// 			float unSafeValue = animKeyList[unSafeIndex].value;
// 			for(int i = 0; i < animKeyList.Count; i++)
// 			{
// 				Keyframe oldKey = animKeyList[i];
// 				float inTangent = oldKey.inTangent;
// 				float outTangent = oldKey.outTangent;

// 				float curChangeDis = i >= smoothStartIndex? changeDis:0;
// 				if(isRevert)
// 				{
// 					curChangeDis = i <= smoothStartIndex? changeDis:0;	
					
// 				}


// 				float time = oldKey.time;
// 				float value = oldKey.value + curChangeDis;
// 				float inWeight = oldKey.inWeight;
// 				float outWeight = oldKey.outTangent;
// 				if(type == HandleTargetType.Root)
// 				{
// 					// Debug.Log($"{unSafeIndex} , {smoothStartIndex}");
// 				}
// 				if((i > unSafeIndex && i < smoothStartIndex)||(i < unSafeIndex && i > smoothStartIndex))
// 				{
// 					float dis = unSafeValue - newSmoothValue;
// 					float count = Mathf.Abs(unSafeIndex - smoothStartIndex) + 2;
// 					value = unSafeValue -  dis * Mathf.Abs(i - unSafeIndex) / count;
// 					// float newTangent = (animKeyList[smoothStartIndex].inTangent + animKeyList[unSafeIndex].inTangent) / 2;
// 					float newTangent = dis / (unSafeIndex - smoothStartIndex);
// 					inTangent = newTangent;
// 					outTangent = newTangent;
// 				}

// 				// if(unSafeIndex != smoothStartIndex - 1 && (i > unSafeIndex && i < smoothStartIndex))
// 				// {
// 				// 	float scale = (i - unSafeIndex) / (smoothStartIndex - unSafeIndex);
// 				// 	value =  Mathf.Lerp(unSafeValue, newSmoothValue, scale);
// 				// 	inTangent = Mathf.Lerp(animKeyList[unSafeIndex].inTangent, animKeyList[smoothStartIndex].inTangent, scale);
// 				// 	outTangent = Mathf.Lerp(animKeyList[unSafeIndex].outTangent, animKeyList[smoothStartIndex].outTangent, scale);
					
// 				// }
// 				Keyframe newKey = new Keyframe(time, value, inTangent, outTangent, inWeight, outWeight);
// 				// if(type == HandleTargetType.Root)
// 				// 	Debug.Log(curChangeDis);
// 				newKey.weightedMode = oldKey.weightedMode;
// 				newCurve.AddKey(newKey);
// 				newList.Add(newKey);
// 			}
// 			return newCurve;
// 		}
// 		//生成没有平滑的新曲线
// 		public static AnimationCurve GenerateNewCurveNoLerp(List<Keyframe> animKeyList, float changeDis, int smoothStartIndex, bool isRevert, HandleTargetType type = HandleTargetType.HandL)
// 		{
			
// 			//从平滑点开始向前查找，按点的高度判断，平滑超出高度导致斜率不合理的点,构建新曲线
// 			AnimationCurve newCurve = new AnimationCurve();
// 			float oldSmoothValue = animKeyList[smoothStartIndex].value;
// 			float newSmoothValue = oldSmoothValue + changeDis;
			
			
// 			for(int i = 0; i < animKeyList.Count; i++)
// 			{
// 				Keyframe oldKey = animKeyList[i];
// 				float inTangent = oldKey.inTangent;
// 				float outTangent = oldKey.outTangent;

// 				float curChangeDis = i >= smoothStartIndex? changeDis:0;
// 				if(isRevert)
// 				curChangeDis = i <= smoothStartIndex? changeDis:0;	
// 				float time = oldKey.time;
// 				float value = oldKey.value + curChangeDis;
// 				float inWeight = oldKey.inWeight;
// 				float outWeight = oldKey.outTangent;
				
// 				Keyframe newKey = new Keyframe(time, value, inTangent, outTangent, inWeight, outWeight);
			
// 				newKey.weightedMode = oldKey.weightedMode;
// 				newCurve.AddKey(newKey);
// 			}
// 			return newCurve;
// 		}
		
// 		public static int GetIndexByTime(List<Keyframe> animKeyList, float time, bool isRevert)
// 		{
// 			int middleIndex = 0;
// 			for(int i = 0; i < animKeyList.Count; i++)
// 			{
// 				if(animKeyList[i].time > time)
// 				{
// 					middleIndex = i;
// 					break;
// 				}
// 			}

// 			return middleIndex;
// 		}


// 		public static float[] GetCurveEdgeValue(AnimationCurve curve)
// 		{

// 			float max = -1000;
// 			float min = 1000;
// 			float time = curve[curve.length - 1].time;
// 			for(int i = 0; i < curve.length; i++)
// 			{
// 				if(curve[i].value > max)
// 					max = curve[i].value;
// 				if(curve[i].value < min)
// 					min = curve[i].value;
// 			}
// 			return new float[]{max, min, time};
// 		}
// 		public static float GetRootYTime(GenerateNewCurveType _generateNewCurveType, List<Keyframe> animKeyList, bool isRevert, float startTime)
// 		{
// 			if(_generateNewCurveType == GenerateNewCurveType.GenerateFindEnd)
// 			{
// 				float time = 0;
// 				GetEndSmoothIndex(animKeyList, isRevert, out time);
// 				return time;
// 			}
// 			// else if(_generateNewCurveType == GenerateNewCurveType.GenerateFindFirstDown)
// 			// {
// 			// 	float time = 0;
// 			// 	GetMiddleSmoothIndex(animKeyList, isRevert, out time);
// 			// 	return time;
// 			// }
// 			else if(_generateNewCurveType == GenerateNewCurveType.GenerateStraight)
// 			{
// 				return 0;
// 			}
// 			return 0;
// 		}

// 		public static AnimationCurve[] GeneralGenerateCurve(List<Keyframe>[] animKeyLists, 
// 			bool isRevert, Vector3 changeDis, HandleTargetType type, GenerateNewCurveType _generateNewCurveType, float startTime = 0, float endTime = 0)
// 		{
// 			// if(type == HandleTargetType.HandL)
// 			// 	Debug.Log(_generateNewCurveType);
// 			AnimationCurve[] newAc = new AnimationCurve[3];
// 			// if(_generateNewCurveType == GenerateNewCurveType.GenerateBySetTimeStraight)
// 			// {

// 			// 	newAc[1] = InitNewCurveByStraight(animKeyLists[1],changeDis.y, isRevert, startTime, endTime);
// 			// 	newAc[0] = InitNewCurveByYTangent(startTime, animKeyLists[0], changeDis.x, isRevert);
// 			// 	newAc[2] = InitNewCurveByYTangent(startTime, animKeyLists[2], changeDis.z, isRevert);
// 			// 	//根节点y与手不同
// 			// 	if(type != HandleTargetType.Root)
// 			// 	{
// 			// 		newAc[2] = InitNewCurveByStraight(animKeyLists[2],changeDis.z, isRevert, startTime, endTime);
// 			// 		newAc[0] = InitNewCurveByStraight(animKeyLists[0],changeDis.x, isRevert, startTime, endTime);
// 			// 	}
// 			// }
// 			if(_generateNewCurveType == GenerateNewCurveType.GenerateBySetTimeHole)
// 			{
// 				newAc[0] = InitNewCurveByHole(animKeyLists[0],changeDis.x, isRevert, startTime, endTime);
// 				newAc[1] = InitNewCurveByHole(animKeyLists[1],changeDis.y, isRevert, startTime, endTime);
// 				newAc[2] = InitNewCurveByHole(animKeyLists[2],changeDis.z, isRevert, startTime, endTime);
// 			}
// 			else if(_generateNewCurveType == GenerateNewCurveType.GenerateFindEnd)
// 			{

// 				float time = 0;
// 				newAc[1] = GetCurveSmoothEnd(animKeyLists[1], changeDis.y, isRevert, out time, type);
// 				newAc[0] = InitNewCurveByYTangent(time, animKeyLists[0], changeDis.x, isRevert);
// 				newAc[2] = InitNewCurveByYTangent(time, animKeyLists[2], changeDis.z, isRevert);

// 			}
// 			// else if(_generateNewCurveType == GenerateNewCurveType.GenerateFindFirstDown)
// 			// {
// 			// 	float time = 0;
// 			// 	newAc[1] = GetCurveToilet(animKeyLists[1], changeDis.y, isRevert, out time);
// 			// 	newAc[0] = InitNewCurveByYTangent(time, animKeyLists[0], changeDis.x, isRevert);
// 			// 	newAc[2] = InitNewCurveByYTangent(time, animKeyLists[2], changeDis.z, isRevert);
				
// 			// }		
// 			else if(_generateNewCurveType == GenerateNewCurveType.GenerateInverse)
// 			{

// 				float time = animKeyLists[1][animKeyLists[1].Count - 1].time;
// 				List<Keyframe> newList = new List<Keyframe>();
// 				newAc[1] = InitNewCurveByInverse(animKeyLists[1], changeDis.y, isRevert, 0, time, out newList);
// 				newAc[0] = InitNewCurveByInverse(animKeyLists[0], changeDis.x, isRevert, 0, time, out newList);
// 				newAc[2] = InitNewCurveByInverse(animKeyLists[2], changeDis.z, isRevert, 0, time, out newList);
// 			}		
// 			else if(_generateNewCurveType == GenerateNewCurveType.GenerateStraight)
// 			{
	
// 				float time = 0;//animKeyLists[1][animKeyLists[1].Count - 1].time;
// 				newAc[0] = InitNewCurveByHole(animKeyLists[0],changeDis.x, isRevert, 0, time);
// 				newAc[1] = InitNewCurveByHole(animKeyLists[1],changeDis.y, isRevert, 0, time);
// 				newAc[2] = InitNewCurveByHole(animKeyLists[2],changeDis.z, isRevert, 0, time);

// 			}
// 			// else if(_generateNewCurveType == GenerateNewCurveType.GenerateBySetTimeGoAndBack)
// 			// {
// 			// 	float time = animKeyLists[1][animKeyLists[1].Count - 1].time;

// 			// 	// newAc[0] = InitNewCurveByHole(animKeyLists[0],changeDis.x, isRevert, 0, time);
// 			// 	// newAc[1] = InitNewCurveByHole(animKeyLists[1],changeDis.y, isRevert, 0, time);
				

// 			// 	newAc[0] = GenerateGoAndBack(animKeyLists[0],changeDis.x, startTime, time);
// 			// 	newAc[1] = GenerateGoAndBack(animKeyLists[1],changeDis.y, startTime, time);
// 			// 	newAc[2] = GenerateGoAndBack(animKeyLists[2],changeDis.z, startTime, time);
// 			// }
// 			return newAc;
// 		}

// 		public static AnimationCurve[] GetAnimNewCurveByCustom(HandleTargetType type, Vector3 changes, IKSampleData _ikSampleData)
// 		{
// 			 List<Keyframe>[] keyFrameList = _ikSampleData.GetCurveData(type).GetKeyframeList();
// 			 IKPartsSampleData partData = _ikSampleData.GetPartData(type);
// 			 AnimationCurve[] newCurves = ResolveCurveData.GeneralGenerateCurve(keyFrameList, _ikSampleData.isNeedRevert, changes, type,
// 					partData._generateNewCurveType, partData.time[0], partData.time[1]);
// 			return newCurves;
// 		}
// 		public static AnimationCurve[] GetAnimNewCurveByGeneralSets(HandleTargetType type, Vector3 changes, IKSampleData _ikSampleData)
// 		{
// 			if(_ikSampleData._samplePresetType == SamplePresetType.Customize)
// 				return GetAnimNewCurveByCustom(type, changes, _ikSampleData);
// 			else
// 				return GetAnimNewCurveByForSit(type, changes, _ikSampleData);
// 		}
// 		public static AnimationCurve[] GetAnimNewCurveByForSit(HandleTargetType type, Vector3 changes, IKSampleData _ikSampleData)
// 		{
// 			float time = 0;
// 			bool isEnd = _ikSampleData.isNeedRevert;
// 			GenerateNewCurveType _generateNewCurveTypes = _ikSampleData.GetPartData(type)._generateNewCurveType;
// 			float[] partTime = _ikSampleData.GetPartData(type).time;
// 			List<Keyframe>[] keyFrameList = _ikSampleData.GetCurveData(type).GetKeyframeList();

// 			GenerateNewCurveType _rootGenerateNewCurveTypes = _ikSampleData.GetPartData(HandleTargetType.Root)._generateNewCurveType;
// 			float[] rootPartTime = _ikSampleData.GetPartData(HandleTargetType.Root).time;
// 			List<Keyframe>[] rootKeyFrameList = _ikSampleData.GetCurveData(HandleTargetType.Root).GetKeyframeList();
// 			AnimationCurve[] newRoots = ResolveCurveData.GeneralGenerateCurve(rootKeyFrameList, isEnd,changes, HandleTargetType.Root,
// 					_rootGenerateNewCurveTypes, rootPartTime[0], rootPartTime[1]);
// 			time = GetRootYTime(_rootGenerateNewCurveTypes, rootKeyFrameList[1], isEnd, rootPartTime[0]);

// 			if(type == HandleTargetType.Root)
// 			{
// 				return newRoots;
// 			}			
// 			else
// 			{
// 				AnimationCurve[] newHands = new AnimationCurve[]{InitNewCurveByYTangent(time, keyFrameList[0], changes.x, isEnd),
// 					InitNewCurveByYTangent(time, keyFrameList[1], changes.y, isEnd),
// 					InitNewCurveByYTangent(time, keyFrameList[2], changes.z, isEnd),
// 				};
// 				return newHands;
// 			}
			
// 		}
// 	}
// }