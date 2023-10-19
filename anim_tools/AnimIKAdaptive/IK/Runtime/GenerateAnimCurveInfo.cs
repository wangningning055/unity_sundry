using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace vw_animation_ik_runtime
{
public class GenerateAnimCurveInfo:MonoBehaviour
{
#if UNITY_EDITOR
	public static AnimationCurve[] GetCurveInfo(AnimationClip animationClip, HandleTargetType type)
	{
		
		string[] propertyNameList = NameConst.GetPropertyNameByType(type);
		string propertyNameX = propertyNameList[0];
		string propertyNameY = propertyNameList[1];
		string propertyNameZ = propertyNameList[2];
		AnimationCurve[] curves = new AnimationCurve[3];
		EditorCurveBinding[] eb = AnimationUtility.GetCurveBindings(animationClip);
		foreach(EditorCurveBinding si in eb)
		{
			if(si.propertyName == propertyNameX)
			{
				curves[0] = AnimationUtility.GetEditorCurve(animationClip, si);
			}
			if(si.propertyName == propertyNameY)
			{
				curves[1] = AnimationUtility.GetEditorCurve(animationClip, si);
			}
			if(si.propertyName == propertyNameZ)
			{
				curves[2] = AnimationUtility.GetEditorCurve(animationClip, si);
			}
		}
		return curves;
	}

	public static string[][] GetAnimCurveInfoText(AnimationClip anim)
	 {

		AnimationCurve[] rootCurves = GetCurveInfo(anim, HandleTargetType.Root);
		AnimationCurve[] handLCurves = GetCurveInfo(anim, HandleTargetType.HandL);
		AnimationCurve[] handRCurves = GetCurveInfo(anim, HandleTargetType.HandR);

		Func<AnimationCurve[], string[]> getCurveInfo = (curves) =>{
			string allTextY = "";
			string allTextX = "";
			string allTextZ = "";
			for(int i = 0; i < curves[0].length; i++)
			{
				allTextX += IKSampleFileUtil.PackKeyframe(curves[0][i]);
			}

			for(int i = 0; i < curves[1].length; i++)
			{
				allTextY += IKSampleFileUtil.PackKeyframe(curves[1][i]);
			}


			for(int i = 0; i < curves[2].length; i++)
			{
				allTextZ += IKSampleFileUtil.PackKeyframe(curves[2][i]);
			}
			string[] allText = new string[]{allTextX, allTextY, allTextZ};
			return allText;
		};
		string[] rootText = getCurveInfo(rootCurves);
		string[] handLText = getCurveInfo(handLCurves);
		string[] handRText = getCurveInfo(handRCurves);
		string[][] allText = new String[][]{rootText, handLText, handRText};
		return allText;
	 }
	public static Keyframe[][][] GetKeyframeListByAnim(AnimationClip anim)
	{
		AnimationCurve[] rootCurves = GetCurveInfo(anim, HandleTargetType.Root);
		AnimationCurve[] handLCurves = GetCurveInfo(anim, HandleTargetType.HandL);
		AnimationCurve[] handRCurves = GetCurveInfo(anim, HandleTargetType.HandR);
		Keyframe[][] rootList = new Keyframe[3][];
		Keyframe[][] handLList = new Keyframe[3][];
		Keyframe[][] handRList = new Keyframe[3][];

		Action<AnimationCurve[], Keyframe[][]> getCurveInfo = (curves, list) =>{
			list[0] = new Keyframe[curves[0].length];
			list[1] = new Keyframe[curves[1].length];
			list[2] = new Keyframe[curves[2].length];
			for(int i = 0; i < curves[0].length; i++)
			{
				list[0][i] = curves[0][i];
			}

			for(int i = 0; i < curves[1].length; i++)
			{
				list[1][i] = curves[1][i];
			}
			for(int i = 0; i < curves[2].length; i++)
			{
				list[2][i] = curves[2][i];
			}
		};
		getCurveInfo(rootCurves, rootList);
		getCurveInfo(handLCurves, handLList);
		getCurveInfo(handRCurves, handRList);

		return new Keyframe[][][]{
			rootList,
			handLList,
			handRList,
		};
	}
	
#endif
}
}
