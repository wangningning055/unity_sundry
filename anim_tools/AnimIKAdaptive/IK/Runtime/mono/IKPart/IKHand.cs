using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;
using System;
using MountPoint;
using Athena;

namespace vw_animation_ik_runtime
{
	public class IKHand:BasePart
	{
		bool isSetFixMiddle = false;
		public override void UpdatePos(float passedTime)
		{
			if(type == HandleTargetType.HandR)
				IKSolverHandRB.PositionWeight = 1;
			if(type == HandleTargetType.HandL)
				IKSolverHandLB.PositionWeight = 1;

			Transform hang = GetHangTargetByType(type);
			Transform targetBone = GetTargetBoneByType(type);
			Vector3 dif = GetCurveDif(passedTime);
			animPos += dif;
			targetBone.position = animPos;

		}
		public override void FixUpdatePos(float passedTime)
		{
			Action fixGoAndBack = () =>
			{
				float middleTime = iKSampleData.OriginalPosTime;
				bool isRevert = passedTime >= middleTime;

				float completeTimes = isRevert? completeTime:middleTime;
				if(!isRevert)
				{
					bool isNeedFix = passedTime - (completeTimes - fixDuration) >= 0;
					if(isNeedFix)
					{
						float normalize = Mathf.Clamp((passedTime - (completeTimes - fixDuration)) / fixDuration, 0, 1);
						Vector3 hangPos = GetHangTargetByType(type).position;
						Transform targetBone = GetTargetBoneByType(type);
						Vector3 dif = (targetBone.position - hangPos) * normalize;
						targetBone.position -= dif;
					}
				}
				else
				{
					bool isNeedFix = passedTime - (completeTimes + fixDuration) <= 0;
					if(isNeedFix)
					{
						float normalize = Mathf.Clamp((completeTimes + fixDuration) - passedTime / fixDuration, 0, 1);
						Vector3 hangPos = GetHangTargetByType(type).position;
						Transform targetBone = GetTargetBoneByType(type);
						if(!isSetFixMiddle)
						{
							stablePos = targetBone.position;
							isSetFixMiddle = true;
						}
						Vector3 dif = (stablePos - hangPos) * normalize;
						targetBone.position -= dif;
					}

				}
			};
			if(iKSampleData.GetPartData(type)._generateNewCurveType == GenerateNewCurveType.GenerateBySetTimeGoAndBack)
			{
				fixGoAndBack();
				return;
			}
			base.FixUpdatePos(passedTime);
		}
		public override void UpdateRotate(float passedTime)
		{
			if(iKSampleData._samplePresetType != SamplePresetType.ForTake || iKSampleData.GetIsNeed()[3]) return;
			
			Quaternion change = ResolveRotate.GetRootRotateForTake(GetAnimTargetOriginalValue(HandleTargetType.Root),
				GetAnimTargetOriginalValue(type), GetHangTargetByType(type).position, player);
			ResolveRotate.UpdateRotateBySet(change, iKSampleData, passedTime, spines, player, type);
		}
	}
}
	