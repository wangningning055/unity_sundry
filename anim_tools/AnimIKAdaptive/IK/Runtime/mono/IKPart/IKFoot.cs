using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;
using System;
using MountPoint;
using Athena;

namespace vw_animation_ik_runtime
{
	public class IKFoot:BasePart
	{
		Quaternion footLRotate, footRRotate;
		public override void InitPart(IKSampleData _ikSampleData, TrackBindingData _sit,
			 Transform _player, HandleTargetType _type)
		{
			iKSampleData = _ikSampleData;
			type = _type;
			player = _player;
			sit = _sit;
			// isUseLimb = _isuse;
		}
		public override void UpdatePos(float passedTime)
		{

			footLRotate = boneFootL.rotation;
			footRRotate = boneFootR.rotation;

			IKSolverFootRB.PositionWeight = 1;
			IKSolverFootLB.PositionWeight = 1;

			Transform targetBone = GetTargetBoneByType(type);
			targetBone.position = animPos;

		}
		public override void FixUpdatePos(float passedTime)
		{
		}
		public override void FixUpdateRotate(float passedTime)
		{
			boneFootL.rotation = footLRotate;
			boneFootR.rotation = footRRotate;
		}
	}
}
	