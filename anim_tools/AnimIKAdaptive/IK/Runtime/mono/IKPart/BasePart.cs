using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;
using System;
using MountPoint;
using Athena;
using InverseKinematic.Foot;

namespace vw_animation_ik_runtime
{
	public class BasePart
	{
		public IKSampleData iKSampleData;
		public Transform boneHandL, boneHandR, boneRoot, boneFootL, boneFootR, player, boneSpine1, boneSpine2, boneSpine3;
		public Transform[] spines;
		public Transform targetHandL, targetHandR, targetRoot, targetFootL, targetFootR;
		public TrackBindingData sit;
		public LimbIK IKSolverFootL, IKSolverFootR, IKSolverHandL, IKSolverHandR;
		public TwoBoneIK IKSolverFootLB, IKSolverFootRB, IKSolverHandLB, IKSolverHandRB;

		public AnimationCurve[] oldCurves;
		public AnimationCurve[] newCurves;
		public HandleTargetType type;
		public float completeTime; // 叠加完成时间
		public Vector3 stablePos;//坐姿稳定位置;
		public float fixDuration = 1.5f; //修正时常
		public Vector3 fixDif = Vector3.zero;//针对不同体型导致的差值的修正
		public Vector3 animPos = Vector3.zero;
		public static Vector3 rootDif = Vector3.zero;
		public Vector3 targetPos = Vector3.zero;//记录挂点位置，位置更改时更新dif，用于调试时
		public virtual void InitPart(IKSampleData _ikSampleData, TrackBindingData _sit,
			 Transform _player, HandleTargetType _type)
		{
			iKSampleData = _ikSampleData;
			type = _type;
			player = _player;
			sit = _sit;
			UpdateCurveDataByType(type);
		}
		public void SetBone(Transform root, Transform handL, Transform handR, Transform footL, Transform footR,
			Transform spine1, Transform spine2, Transform spine3, Transform neck)
		{
			boneRoot = root;
			boneHandL = handL;
			boneHandR = handR;
			boneFootL = footL;
			boneFootR = footR;
			boneSpine1 = spine1;
			boneSpine2 = spine2;
			boneSpine3 = spine3;
			spines = new Transform[]{boneRoot, boneSpine1, boneSpine2, boneSpine3, neck};
		}
		public void SetTarget(Transform root, Transform handL, Transform handR, Transform footL, Transform footR)
		{
			targetRoot = root;
			targetHandL = handL;
			targetHandR = handR;
			targetFootL = footL;
			targetFootR = footR;
		}
		public void SetIKSolver(LimbIK handL, LimbIK handR, LimbIK footL, LimbIK footR)
		{
			IKSolverHandL = handL;
			IKSolverHandR = handR;
			IKSolverFootL = footL;
			IKSolverFootR = footR;
		}
		public void SetIKSolverBone(TwoBoneIK handL, TwoBoneIK handR, TwoBoneIK footL, TwoBoneIK footR)
		{
			IKSolverHandLB = handL;
			IKSolverHandRB = handR;
			IKSolverFootLB = footL;
			IKSolverFootRB = footR;
		}
		public virtual void RecordAnimPos()
		{
			animPos = GetAnimPosByType(type);
		}
		public virtual void UpdatePos(float passedTime)
		{

			if(type == HandleTargetType.HandR)
				IKSolverHandRB.PositionWeight = 1;
			if(type == HandleTargetType.HandL)
				IKSolverHandLB.PositionWeight = 1;

			Transform targetBone = GetTargetBoneByType(type);
			Vector3 dif = GetCurveDif(passedTime);

			animPos += dif;
			targetBone.position = animPos;
		}
		//不同体型的位置修正
		public virtual void FixUpdatePos(float passedTime)
		{
			
			bool isRevert = iKSampleData.isNeedRevert;
			Transform targetBone = GetTargetBoneByType(type);
			float tempDur =1f;
			if(isRevert)
			{
				bool isNeedFix = passedTime - (completeTime + tempDur) <= 0;
				if(isNeedFix)
				{
					float normalize = Mathf.Clamp((completeTime + tempDur) - passedTime / tempDur, 0, 1);
					Vector3 hangPos = GetHangTargetByType(type).position;
					if(passedTime == 0) stablePos = targetBone.position;
					Vector3 dif = (stablePos - hangPos) * normalize;
					targetBone.position -= dif;
				}
				
			}
			else
			{
				bool isNeedFix = completeTime - passedTime <= 0;
				if(isNeedFix)
				{
					float normalize = Mathf.Clamp((passedTime - completeTime) / fixDuration, 0, 1);
					normalize = completeTime <= fixDuration? 1:normalize;
					Transform hangTrans = GetHangTargetByType(type);
					Vector3 dif = (targetBone.position - hangTrans.position) * normalize;
					if(passedTime <= completeTime + fixDuration || hangTrans.position != targetPos)
					{fixDif = dif; targetPos = hangTrans.position;}
					targetBone.position -= fixDif;
					// if(type == HandleTargetType.Root)
					// Debug.Log($"{normalize} ,{passedTime < completeTime + fixDuration}, {completeTime}");
				}
			}
		}
		public virtual void UpdateRotate(float passedTime)
		{

		}
		public virtual void FixUpdateRotate(float passedTime)
		{

		}
		
		public Transform GetTargetBoneByType(HandleTargetType _type)
		{
			if(_type == HandleTargetType.Root)
				return targetRoot;
			if(_type == HandleTargetType.HandL)
				return targetHandL;
			if(_type == HandleTargetType.HandR)
				return targetHandR;
			if(_type == HandleTargetType.FootL)
				return targetFootL;
			if(_type == HandleTargetType.FootR)
				return targetFootR;
			return targetRoot;
		}
		public void UpdateCurveDataByType(HandleTargetType type)
		{
			Vector3 change = Vector3.zero;
			// AnimationCurve[] curves = new AnimationCurve[]{
			// 	new AnimationCurve(),
			// 	new AnimationCurve(),
			// 	new AnimationCurve()
			// };
			change = GetTargetDif(type);

			// newCurves = curves;
			// oldCurves = curves;

			oldCurves = ResolveCurveMgr.Instance.InitOldCurve(iKSampleData.GetCurveData(type).GetKeyframeList());
			newCurves = ResolveCurveMgr.Instance.GetAnimNewCurveByGeneralSets(type, change, iKSampleData, out completeTime);
		}
		//获取target与原始动画终点位置的差值，用以插入新的曲线中
		public Vector3 GetTargetDif(HandleTargetType type)
		{
			Vector3 dif = Vector3.zero;
			Vector3 originalPos = GetAnimTargetOriginalValue(type);
			Vector3 target = Vector3.zero;
			target = GetHangTargetByType(type).position;
			dif = target - originalPos;
			if(type == HandleTargetType.Root)
			{
				rootDif = dif;
			}
			return dif;
		}
		//获取当前人物位置下在动画终点时的目标的原始位置
		public Vector3 GetAnimTargetOriginalValue(HandleTargetType type)
		{
			Vector3 oriPos = Vector3.zero;
			oriPos = iKSampleData.GetPartData(type).originalPos;
			Vector3 oriPosByCurrent = player.transform.rotation *oriPos + player.transform.position;
			if(type == HandleTargetType.HandL || type == HandleTargetType.HandR)
			{
				oriPosByCurrent += rootDif;
			}
			return oriPosByCurrent;
		}
	
		public Transform GetHangTargetByType(HandleTargetType type)
		{
			if(type == HandleTargetType.Root)
				return sit.ikPrepareTrans[0].Find(NameConst.rootPointName);
			else if(type == HandleTargetType.HandL)
				return sit.ikPrepareTrans[0].Find(NameConst.lHandPointName);
			else if(type == HandleTargetType.HandR)
				return sit.ikPrepareTrans[0].Find(NameConst.rHandPointName);
			else
				return sit.ikPrepareTrans[0].Find(NameConst.spinePointName);
		}
		//获取两曲线在时间轴上的的差值
		public Vector3 GetCurveDif(double passTime)
		{
			float passedTime = (float)passTime;
			float difX = newCurves[0].Evaluate(passedTime) - oldCurves[0].Evaluate(passedTime);
			float difY = newCurves[1].Evaluate(passedTime) - oldCurves[1].Evaluate(passedTime);
			float difZ = newCurves[2].Evaluate(passedTime) - oldCurves[2].Evaluate(passedTime);
			Vector3 dif = new Vector3(difX, difY, difZ);
			return dif;
		}
		public Vector3 GetAnimPosByType(HandleTargetType type)
		{
			Vector3 rootAnimPos = boneRoot.transform.position;
			Vector3 handLAnimPos = boneHandL.transform.position;
			Vector3 handRAnimPos = boneHandR.transform.position;
			Vector3 footLAnimPos = boneFootL.transform.position;
			Vector3 footRAnimPos = boneFootR.transform.position;
			Vector3 animPos = Vector3.zero;
			if(type == HandleTargetType.Root) animPos = rootAnimPos;
			if(type == HandleTargetType.HandL) animPos = handLAnimPos;
			if(type == HandleTargetType.HandR) animPos = handRAnimPos;
			if(type == HandleTargetType.FootL) animPos = footLAnimPos;
			if(type == HandleTargetType.FootR) animPos = footRAnimPos;
			return animPos;
		}
	}
}
	