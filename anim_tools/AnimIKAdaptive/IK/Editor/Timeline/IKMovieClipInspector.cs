// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor.Animations;
using UnityEditor.SceneManagement;
using vw_animation_ik_runtime;

namespace vw_animation_ik_editor
{
	[CustomEditor(typeof(IKMovieClip), true)]
	public class IKMovieClipInspector : Editor
	{
		string TestSceneName = "IKSampleTest";
		public AnimationClip AC;
		GenerateNewCurveType[] _generateNewCurveTypes = new GenerateNewCurveType[]{
			GenerateNewCurveType.GenerateFindEnd, GenerateNewCurveType.GenerateFindEnd,
			GenerateNewCurveType.GenerateFindEnd, GenerateNewCurveType.GenerateInverse
		};
		TimeSampleTimeType  _timeSampleTimeTypeStart = TimeSampleTimeType.inSit;


		SamplePresetType _samplePresetType = SamplePresetType.ForSit;
		float[,] startSampleTimeList = new float[,]{{0,0}, {0,0}, {0,0}, {0, 0}};
		Vector2 curveStartScrollPos = Vector2.zero;
		Vector2 curveEndScrollPos = Vector2.zero;
		Vector2 curveLoopScrollPos = Vector2.zero;
		bool IsHaveHandLPoint = false;
		bool IsHaveHandRPoint = false;
		bool IsHaveRootPoint = true;
		bool IsHaveSpine = false;
		bool IsRecordHandLPoint = false;
		bool IsRecordHandRPoint = false;
		bool IsRecordRootPoint = true;
		bool IsRecordSpine = false;
		bool IsSpineStraight = false;
		bool IsNeedRevert = false;

		float OriginalPosTime = 0;
		Vector3 TestChange = Vector3.one * 0.5f;
		IKSampleData _iKSampleData;
		SerializedProperty test;
		AnimationCurve[][] TotalNewCurves = new AnimationCurve[3][];
		public void OnEnable()
		{
			test = serializedObject.FindProperty("_iKSampleData");
			_iKSampleData = (target as IKMovieClip).Data;
			if(_iKSampleData == null||_iKSampleData._totalData == null|| _iKSampleData._totalData.Length==0) {Debug.Log("面板数据为空"); return;}
			bool[] isNeeds = _iKSampleData.GetIsNeed();
			IsHaveHandLPoint = isNeeds[1];
			IsHaveHandRPoint = isNeeds[2];
			IsHaveSpine = isNeeds[3];
			IsHaveRootPoint = isNeeds[0];
			if(IsHaveRootPoint)
			_generateNewCurveTypes[0] = _iKSampleData.GetPartData(HandleTargetType.Root)._generateNewCurveType;
			if(IsHaveHandLPoint)
			_generateNewCurveTypes[1] = _iKSampleData.GetPartData(HandleTargetType.HandL)._generateNewCurveType;
			if(IsHaveHandRPoint)
			_generateNewCurveTypes[2] = _iKSampleData.GetPartData(HandleTargetType.HandR)._generateNewCurveType;
			if(IsHaveSpine)
			_generateNewCurveTypes[3] = _iKSampleData.GetPartData(HandleTargetType.Spine)._generateNewCurveType;
			
			IsSpineStraight = _generateNewCurveTypes[3] == GenerateNewCurveType.GenerateStraight;
			_samplePresetType = _iKSampleData._samplePresetType;
			startSampleTimeList = _iKSampleData.GetTime();
			OriginalPosTime = _iKSampleData.OriginalPosTime;
			IsNeedRevert = _iKSampleData.isNeedRevert;
		}
		void OnValidate()
		{

		}
		public override void OnInspectorGUI()
		{
			
			if(GUILayout.Button("打开编辑测试场景"))
			{
				EditorSceneManager.OpenScene(NameConst.TestScenePath);
			}
					// 绘制预设
			GUILayout.BeginHorizontal();
			GUILayout.Label("动画预设");
			_samplePresetType = (SamplePresetType)EditorGUILayout.EnumPopup(_samplePresetType);
			GUILayout.Label(NameConst.GetExplainByPresetEnu(_samplePresetType));
			GUILayout.EndHorizontal();
			IKMovieClip t = (IKMovieClip)target;

			EditorGUILayout.ObjectField("Target graphic", t.Clip.defaultValue, typeof(AnimationClip), false);
			AC =  t.Clip.defaultValue as AnimationClip;
			GUILayout.BeginHorizontal();
			GUILayout.Label("设置目标点时间");
			OriginalPosTime = EditorGUILayout.DelayedFloatField(OriginalPosTime, GUILayout.Width(50));
			GUILayout.Label("（动画需要叠加到目标点的时间点）默认为动画最后一帧，反向叠加为第一帧");
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			IsHaveHandLPoint = GUILayout.Toggle(IsHaveHandLPoint, "是否需要左手挂点");
			IsHaveHandRPoint = GUILayout.Toggle(IsHaveHandRPoint, "是否需要右手挂点");
			IsHaveSpine = GUILayout.Toggle(IsHaveSpine, "是否需要颈椎挂点");
			IsHaveRootPoint = GUILayout.Toggle(IsHaveRootPoint, "是否需要根节点挂点") || _samplePresetType == SamplePresetType.ForSit;
			IsNeedRevert = GUILayout.Toggle(IsNeedRevert, "是否需要反向叠加（一般用于处理站起动画）");
			DrawDis();
			GUILayout.Space(30);
			GUILayout.Space(20);
			GUILayout.Label("动画设置:");

			if(AC != null)
				DrawCurve(AC, ref curveStartScrollPos);

			// if(GUILayout.Button("重新生成新曲线数据"))
			// {
			// 	UpdateTotalCurve(AC);
				
			// }
			// GUILayout.Label(SetUpPath);
			if(GUILayout.Button("保存并预览"))
			{
				if(AC == null) return;
				{
					GetGeneralData();
					EditorApplication.isPlaying = true;
				}
			}
			if(GUILayout.Button("仅预览"))
			{
				if(AC == null) return;
				{
					EditorApplication.isPlaying = true;
				}
			}
			if(GUILayout.Button("保存"))
			{
				if(AC == null) return;
				if(EditorSceneManager.GetActiveScene().name != TestSceneName){Debug.LogError("未打开测试场景，请打开编辑测试场景再保存预览"); return;}
			
               _iKSampleData = GetGeneralData();
                t.SetIKSampleData(_iKSampleData);
				EditorUtility.SetDirty(target);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

            }
        }
		void SingleSetBox(string curveName, int num, TimeSampleTimeType _timeSampleTimeType)
		{

			GUILayout.BeginHorizontal();
			GenerateNewCurveType useType;

			_generateNewCurveTypes[num] = (GenerateNewCurveType)EditorGUILayout.EnumPopup("新曲线生成方式", _generateNewCurveTypes[num]);
			useType = _generateNewCurveTypes[num];
			GUILayout.Label(NameConst.GetExplainByCurveEnu(useType));
			GUILayout.FlexibleSpace();

			GUILayout.EndHorizontal();

			if(useType == GenerateNewCurveType.GenerateBySetTimeHole || useType == GenerateNewCurveType.GenerateBySetTimeGoAndBack)
			{
				GUILayout.BeginHorizontal();
				// GUILayout.Space(20);
				// _timeSampleTimeType = (TimeSampleTimeType)EditorGUILayout.EnumPopup("采样时间段类型", _timeSampleTimeType);
				GUILayout.Label(NameConst.GetExplainByTimeEnu(_timeSampleTimeType));
				GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal();
				GUILayout.Space(20);
				GUILayout.Label("叠加开始时间");
				startSampleTimeList[num, 0] = EditorGUILayout.DelayedFloatField(startSampleTimeList[num, 0], GUILayout.Width(50));
				GUILayout.Label("叠加结束时间");
				startSampleTimeList[num, 1] = EditorGUILayout.DelayedFloatField(startSampleTimeList[num, 1], GUILayout.Width(50));
				GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();
				
			}
			GUILayout.FlexibleSpace();
		}
		void DrawDis()
		{
			Func<string, Vector3, Vector3> change = (name, dis) =>
			{
				GUILayout.BeginHorizontal();
				GUILayout.Label(name);
				dis.x = EditorGUILayout.DelayedFloatField(dis.x, GUILayout.Width(80));
				dis.y = EditorGUILayout.DelayedFloatField(dis.y, GUILayout.Width(80));
				dis.z = EditorGUILayout.DelayedFloatField(dis.z, GUILayout.Width(80));
				GUILayout.EndHorizontal();
				return dis;
			};
			// change("测试偏移值", TestChange);
			
		}

		void DrawCurve(AnimationClip oldAnim, ref Vector2 scrollPos)
		{
			if(AC == null) return;
			Action<AnimationCurve[], Color, string, bool, int> drawSingleCurve = (curves, color, name, isNeed, num) =>
			{
				if(curves == null) return;
				GUILayout.Space(10);
				GUILayout.BeginHorizontal();
				GUILayout.Label(name);
				SingleSetBox("新曲线设置", num, _timeSampleTimeTypeStart);
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal();
				if(isNeed)
				{
					for(int i = 0; i < curves.Length; i++)
					{
						if(curves[i] == null) continue;
						GUILayout.BeginVertical();
						string axis = i == 0? "X:":i==1? "Y: ":"Z: ";
						GUILayout.Label(axis);

						float[] rectData = ResolveCurveMgr.Instance.GetCurveEdgeValue(curves[i]);
						Rect rect = new Rect(0, rectData[1], rectData[2] + 1, rectData[0] - rectData[1]);
						EditorGUILayout.CurveField(curves[i], color, rect, GUILayout.Width(300), GUILayout.Height(100));
						GUILayout.EndVertical();
					}
				}
				GUILayout.EndHorizontal();
			};

			AnimationCurve[] rootCurvesOld = GenerateAnimCurveInfo.GetCurveInfo(oldAnim, HandleTargetType.Root);
			AnimationCurve[] handLCurvesOld = GenerateAnimCurveInfo.GetCurveInfo(oldAnim, HandleTargetType.HandL);
			AnimationCurve[] handRCurvesOld = GenerateAnimCurveInfo.GetCurveInfo(oldAnim, HandleTargetType.HandR);

			AnimationCurve[] rootCurvesNew = new AnimationCurve[1];
			AnimationCurve[] handLCurvesNew = new AnimationCurve[1];;
			AnimationCurve[] handRCurvesNew = new AnimationCurve[1];;

			rootCurvesNew = TotalNewCurves[0];
			handLCurvesNew = TotalNewCurves[1];
			handRCurvesNew= TotalNewCurves[2];

			scrollPos = GUILayout.BeginScrollView(scrollPos);
			if(IsHaveRootPoint)
			{
				drawSingleCurve(rootCurvesOld, Color.green, "原Root曲线", true, 0);
				drawSingleCurve(rootCurvesNew, Color.red, "新Root曲线", true, 0);
			}


			if(IsHaveHandLPoint && _samplePresetType != SamplePresetType.ForSit)
			{
				drawSingleCurve(handLCurvesOld, Color.green, "原HandL曲线", true, 1);
				drawSingleCurve(handLCurvesNew, Color.red, "新HandL曲线", true, 1);
			}
			if(IsHaveHandRPoint && _samplePresetType != SamplePresetType.ForSit)
			{
				drawSingleCurve(handRCurvesOld, Color.green, "原HandR曲线", true, 2);
				drawSingleCurve(handRCurvesNew, Color.red, "新HandR曲线", true, 2);
			}
			if(IsHaveSpine && _samplePresetType != SamplePresetType.ForSit)
			{
				GUILayout.Space(10);
				GUILayout.BeginHorizontal();
				GUILayout.Label("颈椎设置：");
				IsSpineStraight = GUILayout.Toggle(IsSpineStraight, "是否直线叠加");
				if(!IsSpineStraight)
				{
					GUILayout.Label("叠加开始时间");
					startSampleTimeList[3, 0] = EditorGUILayout.DelayedFloatField(startSampleTimeList[3, 0], GUILayout.Width(50));
					GUILayout.Label("叠加结束时间");
					startSampleTimeList[3, 1] = EditorGUILayout.DelayedFloatField(startSampleTimeList[3, 1], GUILayout.Width(50));
				}
				GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();
			}
			GUILayout.EndScrollView();
		}

		void UpdateTotalCurve(AnimationClip newAc)
		{
			// if(newAc == null) return;
			// bool isNeedRevert = IsNeedRevert;
			// List<Keyframe>[][] startList = GenerateAnimCurveInfo.GetKeyframeListByAnim(newAc);
			// {
			// 	float time = 0;
			// 	AnimationCurve[] curves = ResolveCurveMgr.Instance.GetAnimNewCurveByGeneralSets(HandleTargetType.Root, TestChange, _iKSampleData, out time);
			// 	AnimationCurve[] lCurves = ResolveCurveMgr.Instance.GetAnimNewCurveByGeneralSets(HandleTargetType.Root, TestChange, _iKSampleData, out time);
			// 	AnimationCurve[] rCurves = ResolveCurveMgr.Instance.GetAnimNewCurveByGeneralSets(HandleTargetType.Root, TestChange, _iKSampleData, out time);

			// 	TotalNewCurves = new AnimationCurve[][]{
			// 		curves, lCurves, rCurves
			// 	};
			// }

		}

		void SetRecorder()
		{
			IsRecordHandLPoint = IsHaveHandLPoint;
			IsRecordHandRPoint = IsHaveHandRPoint;
			IsRecordRootPoint = IsHaveRootPoint || IsHaveSpine;
			IsRecordSpine = IsHaveSpine;
			if(_samplePresetType == SamplePresetType.ForTake)
			{
				IsRecordRootPoint = IsHaveHandLPoint || IsHaveHandRPoint;
				IsRecordSpine = IsRecordRootPoint;
			}
			if(IsSpineStraight)
			{
				_generateNewCurveTypes[(int)HandleTargetType.Spine] = GenerateNewCurveType.GenerateStraight;
			}
		}
		//根据面板设置生成数据
		IKSampleData GetGeneralData(bool isNeedPack = true)
		{
			SetRecorder();
			Func<bool, bool, bool, AnimationClip, SamplePresetType, GenerateNewCurveType[], float[,], IKSampleData> getData =
			(isNeedHandL, isNeedHandR, isNeedRoot, anim, samplePresetType,generateNewCurveTypes, timeList) =>
			{
				IKSampleData data = new IKSampleData();
				data.SetAnim(anim);
				data.InitHandPoint(IsRecordHandLPoint, IsRecordHandRPoint, IsRecordRootPoint, IsRecordSpine);
				data.SetIsNeed(isNeedHandL, isNeedHandR, isNeedRoot, IsHaveSpine);
				data.SetOriginalPos(GetOriginalPos(AC, OriginalPosTime, IsNeedRevert));
				data.SetType(samplePresetType, generateNewCurveTypes);
				data.SetTime(timeList, OriginalPosTime);
				data.SetCurveData();
				return data;
			};
			IKSampleData startData = getData(IsHaveHandLPoint, IsHaveHandRPoint, IsHaveRootPoint, AC, _samplePresetType, _generateNewCurveTypes, startSampleTimeList);
			startData.isNeedRevert = IsNeedRevert;

			return startData;
		}
		public static Vector3[] GetOriginalPos(AnimationClip anim, float recorderTime, bool isNeedRevert)
		{
			if (null == anim)
			{
				Debug.Log("未找到start动画信息，需在Assets面板获取对应动画信息");
				return new Vector3[]{Vector3.zero, Vector3.zero, Vector3.zero};
			}
			float normalizedTime = isNeedRevert? 0:1;
			float animTotalTime = isNeedRevert? 0:anim.length;
			float animTime =  recorderTime == 0?animTotalTime : recorderTime;
			normalizedTime = recorderTime == 0? normalizedTime: recorderTime / anim.length;
			GameObject select = GameObject.Find("female_body_001_IKSample");
			foreach(var obj in Resources.FindObjectsOfTypeAll(typeof(GameObject)))
			{
				if(obj.name == "female_body_001_IKSample")
					select = obj as GameObject;

			}
			select.SetActive(true);
			GameObject temp = GameObject.Instantiate(select, Vector3.zero, Quaternion.identity);
			Animator tempAnimator = temp.GetComponent<Animator>();

			Selection.activeGameObject = temp;

			AnimatorOverrideController animatorOverrideController = new AnimatorOverrideController(tempAnimator.runtimeAnimatorController);

			AnimationClip[] clips = animatorOverrideController.animationClips;
			animatorOverrideController[clips[0].name] = anim;
			tempAnimator.runtimeAnimatorController = animatorOverrideController;

			clips = tempAnimator.runtimeAnimatorController.animationClips;
			tempAnimator.Rebind();
			tempAnimator.Update(0);

			tempAnimator.applyRootMotion = true;
			tempAnimator.Play("UseState", 0, normalizedTime);
			clips[0].SampleAnimation(temp, animTime);
			tempAnimator.applyRootMotion = true;
			tempAnimator.Update(0);
			Transform root = temp.transform.Find(NameConst.rootObjPath);
			Transform handL = temp.transform.Find(NameConst.handLObjPath);
			Transform handR = temp.transform.Find(NameConst.handRObjPath);
			Transform neck = temp.transform.Find(NameConst.spinePath);
			Vector3 rootPos = root.position;
			Vector3 handLPos = handL.position;
			Vector3 handRPos = handR.position;
			Vector3 spinePos = neck.position;
			GameObject.DestroyImmediate(temp);
			select.SetActive(false);
			return new Vector3[]{rootPos, handLPos, handRPos, spinePos};

		}
	}
}
