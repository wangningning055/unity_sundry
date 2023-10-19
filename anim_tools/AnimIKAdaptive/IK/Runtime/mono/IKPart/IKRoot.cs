using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;
using System;
using MountPoint;
using Athena;

namespace vw_animation_ik_runtime
{
	public class IKRoot:BasePart
	{
		bool isNeedRoot, isNeedHandL, isNeedHandR, isNeedSpine;
		Quaternion handLRotate, handRRotate;
		public override void InitPart(IKSampleData _ikSampleData, TrackBindingData _sit,
			 Transform _player, HandleTargetType _type)
		{
			iKSampleData = _ikSampleData;
			type = _type;
			player = _player;
			sit = _sit;
			GetIsNeed();
			if(isNeedRoot)
			UpdateCurveDataByType(type);

		}
		void GetIsNeed()
		{
			isNeedRoot = iKSampleData.GetIsNeed()[0];
			isNeedHandL = iKSampleData.GetIsNeed()[1];
			isNeedHandR = iKSampleData.GetIsNeed()[2];
			isNeedSpine = iKSampleData.GetIsNeed()[3];
		}
		public override void RecordAnimPos()
		{
			animPos = GetAnimPosByType(type);
			handLRotate = boneHandL.transform.rotation;
			handRRotate = boneHandR.transform.rotation;
		}
		public override void FixUpdateRotate(float passedTime)
		{
			if(!isNeedHandL) boneHandL.transform.rotation = handLRotate;
			if(!isNeedHandR) boneHandR.transform.rotation = handRRotate;
		}
		public override void UpdatePos(float passedTime)
		{
			if(!isNeedRoot) return;
			Vector3 handLAnimPos = boneHandL.transform.position;
			Vector3 handRAnimPos = boneHandR.transform.position;


			Transform targetBone = GetTargetBoneByType(type);
			Vector3 dif = GetCurveDif(passedTime);
			animPos += dif;
			targetBone.position = animPos;
			//不使用手部ik则在处理root时同步处理手部位置
			if(type == HandleTargetType.Root)
			{
				handLAnimPos += dif;
				handRAnimPos += dif;

				if(dif.y < 0)
				{
					handLAnimPos.y -= dif.y / 2;
					handRAnimPos.y -= dif.y / 2;
				}
				else
				{
					handLAnimPos.y -= dif.y / 2;
					handRAnimPos.y -= dif.y / 2;
				}
				if(!isNeedHandL)
					targetHandL.transform.position = handLAnimPos;
				if(!isNeedHandR)
					targetHandR.transform.position = handRAnimPos;
			}
		}

		public override void UpdateRotate(float passedTime)
		{
			if(!isNeedSpine) return;
			FixHandForSpine();
			Vector3 dis = isNeedRoot? GetTargetDif(HandleTargetType.Root): Vector3.zero;
			Quaternion change = ResolveRotate.GetRootRotateForSpineHang(GetAnimTargetOriginalValue(HandleTargetType.Root),
				GetAnimTargetOriginalValue(HandleTargetType.Spine), GetHangTargetByType(HandleTargetType.Spine).position, dis);
			ResolveRotate.UpdateRotateBySet(change, iKSampleData, passedTime, spines, player, type);
		}
		public override void FixUpdatePos(float passedTime)
		{
			if(type == HandleTargetType.Spine || !isNeedRoot) return;
			base.FixUpdatePos(passedTime);
		}
		void FixHandForSpine()
		{
			IKSolverHandRB.PositionWeight = 1;
			IKSolverHandLB.PositionWeight = 1;
		}
	}
}
	