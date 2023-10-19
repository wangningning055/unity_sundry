using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Text.RegularExpressions;
using Object = UnityEngine.Object;

namespace vw_animation_ik_runtime
{

    [Serializable]
	public class IKSampleCurveData
	{
		public string X, Y, Z;
		public List<Keyframe>[] _keyFrameList;
		public void SetData(string[] CurveText)
		{
			if(_keyFrameList != null && _keyFrameList[0].Count > 0) return;
			X = CurveText[0];
			Y = CurveText[1];
			Z = CurveText[2];
			if(_keyFrameList == null)
			{
				_keyFrameList = new List<Keyframe>[]{
					IKSampleFileUtil.ReadKeyFrameData(X),
					IKSampleFileUtil.ReadKeyFrameData(Y),
					IKSampleFileUtil.ReadKeyFrameData(Z),
				};
			}
			else
			{
				_keyFrameList[0] = IKSampleFileUtil.ReadKeyFrameData(X);
				_keyFrameList[1] = IKSampleFileUtil.ReadKeyFrameData(Y);
				_keyFrameList[2] = IKSampleFileUtil.ReadKeyFrameData(Z);
			}
	
		}
		public List<Keyframe>[] GetKeyframeList()
		{
			// if(_keyFrameList != null) return _keyFrameList;
			// _keyFrameList = new List<Keyframe>[]{
			// 	IKSampleFileUtil.ReadKeyFrameData(X),
			// 	IKSampleFileUtil.ReadKeyFrameData(Y),
			// 	IKSampleFileUtil.ReadKeyFrameData(Z),
			// };
			return _keyFrameList;
		}
		public void ReleaseKeyFrame()
		{
			if(_keyFrameList == null) return;
			Action<int> releaseSingle = (index) =>
			{
				if(_keyFrameList[index] == null) return;
				foreach(Keyframe key in _keyFrameList[index])
				{
					ResolveCurveMgr.Instance._keyFramePool.ReturnKey(key);
				}
				int listLength = _keyFrameList[index].Count;
				for(int i = 0; i <listLength; i++)
				{
					_keyFrameList[index].Remove(_keyFrameList[index][0]);
				}
			};
			releaseSingle(0);
			releaseSingle(1);
			releaseSingle(2);
		}
	}
	[Serializable]
	public class IKPartsSampleData
	{
		public GenerateNewCurveType _generateNewCurveType;//root, handL, handR曲线类型
		public float[] time = new float[]{0,0};
		public Vector3 originalPos;//原点
		public string[] CurveText;
		public float _totalTime = -1;
		public IKSampleCurveData _curveData;

	}

	[Serializable]
	public class IKSampleData
	{
		AnimationClip anim;
		public IKPartsSampleData[] _totalData;
		public float OriginalPosTime = 0;
		public bool isNeedRevert;//是否开启反向（用于end动画）
		public bool[] isNeed;
		public SamplePresetType _samplePresetType;//动画预设
		public void InitHandPoint(bool handL, bool handR, bool root, bool spine)
		{
			Action<bool, int> initData = (isNeed, index) =>
			{
				if(isNeed)
				{
					_totalData[index] = new IKPartsSampleData();
				}
			};
			initData(root, (int)HandleTargetType.Root);
			initData(handL, (int)HandleTargetType.HandL);
			initData(handR, (int)HandleTargetType.HandR);
			initData(spine, (int)HandleTargetType.Spine);
		}
		public void SetIsNeed(bool handL, bool handR, bool root, bool spine)
		{
			isNeed = new bool[(int)HandleTargetType.Max];
			isNeed[(int)HandleTargetType.HandL] = handL;
			isNeed[(int)HandleTargetType.HandR] = handR;
			isNeed[(int)HandleTargetType.Root] = root;
			isNeed[(int)HandleTargetType.Spine] = spine;

		}
		
		public void SetAnim(AnimationClip _anim)
		{
			anim = _anim;
			_totalData = new IKPartsSampleData[(int)HandleTargetType.Max];
		}
		public AnimationClip GetAnim()
		{
			return anim;

		}
		public void SetOriginalPos(Vector3[] totalPos)
		{
			Action<int> setPos = (index) =>
			{
				if(_totalData[index] != null)
				{
					_totalData[index].originalPos = totalPos[index];
				}
			};
			setPos((int)HandleTargetType.Root);
			setPos((int)HandleTargetType.HandL);
			setPos((int)HandleTargetType.HandR);
			setPos((int)HandleTargetType.Spine);

		}
		public void SetType(SamplePresetType samplePresetType, GenerateNewCurveType[] generateNewCurveTypes)
		{
			_samplePresetType = samplePresetType;
			Action<int> setType = (index) =>
			{
				if(_totalData[index] != null)
				{
					_totalData[index]._generateNewCurveType = generateNewCurveTypes[index];
				}
			};
			setType((int)HandleTargetType.Root);
			setType((int)HandleTargetType.HandL);
			setType((int)HandleTargetType.HandR);
			setType((int)HandleTargetType.Spine);

		}
		public void SetTime(float[,] startTime, float _originalPosTime)
		{
			Action<int> setTime = (index) =>
			{
				if(_totalData[index] != null)
				{
					_totalData[index].time= new float[]{startTime[index, 0], startTime[index, 1]};
				}
			};
			setTime((int)HandleTargetType.Root);
			setTime((int)HandleTargetType.HandL);
			setTime((int)HandleTargetType.HandR);
			setTime((int)HandleTargetType.Spine);
			OriginalPosTime = _originalPosTime;

		}
		public float[,] GetTime()
		{
			float[,] time = new float[4,2];
		
			time[0, 0] = _totalData[(int)HandleTargetType.Root] != null? _totalData[0].time[0]:0;
			time[0, 1] = _totalData[(int)HandleTargetType.Root] != null? _totalData[0].time[1]:0;

			time[1, 0] = _totalData[(int)HandleTargetType.HandL] != null? _totalData[1].time[0]:0;
			time[1, 1] = _totalData[(int)HandleTargetType.HandL] != null? _totalData[1].time[1]:0;

			time[2, 0] =  _totalData[(int)HandleTargetType.HandR] != null? _totalData[2].time[0]:0;
			time[2, 1] =  _totalData[(int)HandleTargetType.HandR] != null? _totalData[2].time[1]:0;

			time[3, 0] =  _totalData[(int)HandleTargetType.Spine] != null? _totalData[3].time[0]:0;
			time[3, 1] =  _totalData[(int)HandleTargetType.Spine] != null? _totalData[3].time[1]:0;
			return time;
		}
		public float GetTotalTime(HandleTargetType type)
		{
			IKPartsSampleData data = _totalData[(int)type];
			if(data._totalTime < 0)
			{
				List<Keyframe>[] keyFrameList = GetCurveData(type).GetKeyframeList();
				data._totalTime = keyFrameList[0][keyFrameList[0].Count - 1].time;
			}
			return data._totalTime;
		}
		public void SetCurveData()
		{
#if UNITY_EDITOR
			string[][] allStartCurveText = GenerateAnimCurveInfo.GetAnimCurveInfoText(anim);
			Action<int> setCurveData = (index) =>
			{
				if(_totalData[index] != null)
				{
					_totalData[index].CurveText = allStartCurveText[index];
				}
			};
			setCurveData((int)HandleTargetType.Root);
			setCurveData((int)HandleTargetType.HandL);
			setCurveData((int)HandleTargetType.HandR);
#endif
		}
		public IKSampleCurveData GetCurveData(HandleTargetType type)
		{
			int index = (int)type;
			IKSampleCurveData curCurveData = _totalData[index]._curveData;
			if(curCurveData != null && curCurveData._keyFrameList != null)
			{
				_totalData[index]._curveData.SetData(_totalData[index].CurveText);
				return _totalData[index]._curveData;
			}
			IKSampleCurveData curveData = new IKSampleCurveData();
			curveData.SetData(_totalData[index].CurveText);
			_totalData[index]._curveData = curveData;
			return curveData;
		}
		public IKPartsSampleData GetPartData(HandleTargetType type)
		{
			return _totalData[(int)type];
		}
		public bool isCurveDataNull()
		{
			Func<int, bool> isHaveCurvedata = (index) =>
			{
				if(_totalData[index] != null)
				{
					return _totalData[index].CurveText == null;
				}
				return false;
			};
			return isHaveCurvedata((int)HandleTargetType.Root) || isHaveCurvedata((int)HandleTargetType.HandL) || isHaveCurvedata((int)HandleTargetType.HandR);
		}
		public bool[] GetIsNeed()
		{
			if(isNeed == null || isNeed.Length <=0) return new bool[]{false, false, false,false};
			return isNeed;
		}
		public void ReleaseCurveData()
		{
			// Debug.Log($"还数据1,   {ResolveCurveMgr.Instance._keyFramePool.keys.Count},    {ResolveCurveMgr.Instance._keyFramePool.newInt}");

			_totalData[(int)HandleTargetType.Root]._curveData.ReleaseKeyFrame();
			_totalData[(int)HandleTargetType.HandL]._curveData.ReleaseKeyFrame();
			_totalData[(int)HandleTargetType.HandR]._curveData.ReleaseKeyFrame();

			// _totalData[(int)HandleTargetType.Root]._curveData = null;
			// _totalData[(int)HandleTargetType.HandL]._curveData = null;
			// _totalData[(int)HandleTargetType.HandR]._curveData = null;
			// Debug.Log($"还数据2,   {ResolveCurveMgr.Instance._keyFramePool.keys.Count},    {ResolveCurveMgr.Instance._keyFramePool.newInt}");
		}
	}
}
