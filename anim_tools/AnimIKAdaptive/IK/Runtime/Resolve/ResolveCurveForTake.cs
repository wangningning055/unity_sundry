using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace vw_animation_ik_runtime
{
	public class ResolveCurveForTake:ResolveCurveBase
	{
		public static AnimationCurve[] GetAnimNewCurveByForTake(HandleTargetType type, Vector3 changes, IKSampleData _ikSampleData, out float completeTime)
		{
			return 	ResolveCurveMgr.Instance.GeneralGenerateCurve(type, changes, _ikSampleData, out completeTime);
		}

		public static void GetRootRotate(Vector3 changes, IKSampleData _ikSampleData)
		{

		}
		public static void GetRootPos(Vector3 changes, IKSampleData _ikSampleData)
		{
			
		}

	}
}
