using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace vw_animation_ik_runtime
{
	public class ResolveRotate:ResolveCurveBase
	{
		public static void UpdateRotateBySit(Quaternion tend,IKSampleData _ikSampleData, float passedTime, Transform[] bones, Transform player)
		{
			IKPartsSampleData partData = _ikSampleData.GetPartData(HandleTargetType.Spine);
			IKPartsSampleData rootPartData = _ikSampleData.GetPartData(HandleTargetType.Root);
			bool isRevert = _ikSampleData.isNeedRevert;
			float startTime = partData.time[0];
			float endTime = partData.time[1];

			GenerateNewCurveType _rootGenerateNewCurveTypes =rootPartData._generateNewCurveType;
			float[] rootPartTime = rootPartData.time;
			float normalizedTime = passedTime / _ikSampleData.GetTotalTime(HandleTargetType.Root);
			if(_rootGenerateNewCurveTypes == GenerateNewCurveType.GenerateStraight)
			{
				normalizedTime = 1;
			}
			if(isRevert)
			{
				normalizedTime = 1 - normalizedTime;
			}
			// Vector3 oldNeck = bones[4].position;
			Quaternion dif = Quaternion.Lerp(Quaternion.identity, tend, normalizedTime);
			bones[0].rotation = dif * bones[0].rotation;

			Debug.DrawRay(bones[0].position, bones[1].position - bones[0].position, Color.blue);
			Debug.DrawRay(bones[1].position, bones[2].position - bones[1].position, Color.blue);
			Debug.DrawRay(bones[2].position, bones[3].position - bones[2].position, Color.blue);
			Debug.DrawRay(bones[3].position, bones[4].position - bones[3].position, Color.blue);

			Vector3 neckPos = bones[4].position;
			
			float spineLength = 0.5f;
			Vector3 neckTend = (bones[4].position - bones[0].position).normalized;
			// Vector3 oldNeckTend = (oldNeck - bones[0].position).normalized;
			// float clampOld = Vector3.Dot(oldNeckTend, -player.forward);
			float clampNew = Mathf.Clamp(Vector3.Dot(neckTend, -player.forward), 0, 1);
			float module = clampNew < 0.5? clampNew: 1- clampNew;
			// float middleNormalized = 0.5f;
			// float module = clampNew < middleNormalized? clampNew / middleNormalized: 1 - (clampNew - middleNormalized) / (1 - middleNormalized);
			float middleLength = module * spineLength;
			Vector3 spineTend =(bones[2].position - bones[0].position).normalized;
			Vector3 normal = (-player.forward - Vector3.Dot(-player.forward, spineTend) * spineTend).normalized;
			Vector3 normalPos = normal * middleLength + bones[2].position;
			Quaternion rootTend = Quaternion.FromToRotation(bones[4].position - bones[0].position, normalPos - bones[0].position);
			Quaternion spinesTend = Quaternion.FromToRotation(normalPos - bones[0].position, bones[4].position - bones[0].position);

			normalPos.y = normalPos.y < bones[0].position.y? bones[0].position.y : normalPos.y;

			Debug.DrawRay(bones[0].position, -player.forward, Color.blue);
			Debug.DrawRay(bones[0].position, spineTend * 1000, Color.red);
			Debug.DrawRay(bones[2].position, normal * 1000, Color.green);
			Debug.DrawRay(bones[0].position,  (normalPos - bones[0].position) * 1000, Color.yellow);

			Quaternion difRoot = Quaternion.Lerp(Quaternion.identity, rootTend, normalizedTime);
			Quaternion difSpine1 = Quaternion.Lerp(Quaternion.identity, spinesTend, 0.5f * normalizedTime);
			Quaternion difSpine2 = Quaternion.Lerp(Quaternion.identity, spinesTend, 0.5f * normalizedTime);
			Quaternion difSpine3 = Quaternion.Lerp(Quaternion.identity, spinesTend, 0.5f * normalizedTime);


			bones[0].rotation = difRoot * bones[0].rotation;
			bones[1].rotation = difSpine1 * bones[1].rotation;
			bones[2].rotation = difSpine2 * bones[2].rotation;
			// bones[3].rotation = difSpine3 * bones[3].rotation;
			// bones[4].rotation = difSpine3 * bones[4].rotation;
			Debug.DrawRay(bones[0].position, bones[1].position - bones[0].position, Color.white);
			Debug.DrawRay(bones[1].position, bones[2].position - bones[1].position, Color.white);
			Debug.DrawRay(bones[2].position, bones[3].position - bones[2].position, Color.white);
			Debug.DrawRay(bones[3].position, bones[4].position - bones[3].position, Color.white);

		}

		public static void UpdateRotateByLie(Quaternion tend,IKSampleData _ikSampleData, float passedTime, Transform[] bones, Transform player)
		{
			IKPartsSampleData partData = _ikSampleData.GetPartData(HandleTargetType.Spine);
			bool isRevert = _ikSampleData.isNeedRevert;
			float startTime = partData.time[0];
			float endTime = partData.time[1];

			GenerateNewCurveType _rootGenerateNewCurveTypes =partData._generateNewCurveType;
			float normalizedTime = Mathf.Clamp((passedTime - startTime)	/ (endTime - startTime), 0, 1);
			if(_rootGenerateNewCurveTypes == GenerateNewCurveType.GenerateStraight)
			{
				normalizedTime = 1;
			}
			if(isRevert)
			{
				normalizedTime = 1 - normalizedTime;
			}
			Quaternion dif1 = Quaternion.Lerp(Quaternion.identity, tend, normalizedTime * 0.2f);
			Quaternion dif2 = Quaternion.Lerp(Quaternion.identity, tend, normalizedTime * 0.1f);
			Quaternion dif3 = Quaternion.Lerp(Quaternion.identity, tend, normalizedTime * 0.3f);
			Quaternion dif4 = Quaternion.Lerp(Quaternion.identity, tend, normalizedTime * 0.5f);

			bones[0].rotation = dif1 * bones[0].rotation;
			bones[1].rotation = dif2 * bones[1].rotation;
			bones[2].rotation = dif3 * bones[2].rotation;
			bones[3].rotation = dif4 * bones[3].rotation;
			bones[4].rotation = dif4 * bones[4].rotation;


			Debug.DrawRay(bones[0].position, (bones[4].position - bones[0].position) * 100, Color.red);
			Debug.DrawRay(bones[0].position, bones[1].position - bones[0].position, Color.blue);
			Debug.DrawRay(bones[1].position, bones[2].position - bones[1].position, Color.blue);
			Debug.DrawRay(bones[2].position, bones[3].position - bones[2].position, Color.blue);
			Debug.DrawRay(bones[3].position, bones[4].position - bones[3].position, Color.blue);


		}

		public static void UpdateRotateByTake(Quaternion tend,IKSampleData _ikSampleData, float passedTime, Transform[] bones, HandleTargetType type)
		{
			IKPartsSampleData partData = _ikSampleData.GetPartData(type);
			float startTime = partData.time[0];
			float endTime = partData.time[1];
			float middleTime = _ikSampleData.OriginalPosTime;
			float totalTime =  _ikSampleData.GetTotalTime(type);
			endTime = endTime <= middleTime? totalTime:endTime;
			middleTime = middleTime != 0? middleTime: _ikSampleData.isNeedRevert? totalTime:0; 
			float normalizedTime = 0;
			if(passedTime < middleTime  && passedTime >= startTime)
			{
				normalizedTime = (passedTime - startTime) / (middleTime - startTime);
			}
			else if(passedTime > middleTime)
			{
				normalizedTime = 1 - (passedTime - middleTime) / (endTime - middleTime);
			}
			if(partData._generateNewCurveType == GenerateNewCurveType.GenerateStraight)
			{
				normalizedTime = 1;
			}
			normalizedTime = Mathf.Clamp(normalizedTime, 0, 1);
			float normalized = (Mathf.Sin(normalizedTime * (3.14f) - 3.14f / 2) + 1) / 2;
			Quaternion dif = Quaternion.Lerp(Quaternion.identity, tend, normalized / 4);
			bones[0].rotation = dif * bones[0].rotation;
			bones[1].rotation = dif * bones[1].rotation;
			bones[2].rotation = dif * bones[2].rotation;
			bones[3].rotation = dif * bones[3].rotation;
		}

		public static void UpdateRotateByCustom(Quaternion tend,IKSampleData _ikSampleData, float passedTime, Transform[] bones)
		{
			
		}

		//获取颈椎挂点对应根节点的旋转量
		public static Quaternion GetRootRotateForSpineHang(Vector3 root, Vector3 neck, Vector3 spinePoint, Vector3 dis)
		{
			root = root + dis;
			neck = neck + dis;
			Vector3 oldDir = (neck - root).normalized;
			Vector3 newDir = (spinePoint - root).normalized;
			return Quaternion.FromToRotation(oldDir, newDir);
		}

		//获取手部抓取时对应的根节点旋转量
		public static Quaternion GetRootRotateForTake(Vector3 root, Vector3 originalHandPoint, Vector3 handTargetPos, Transform player)
		{
			if(handTargetPos.y > originalHandPoint.y)//禁用根节点上扬
				handTargetPos.y = originalHandPoint.y;
			Vector3 pointForXY = originalHandPoint - player.forward;

			Vector3 oldDir = originalHandPoint - pointForXY;
			Vector3 newDir = handTargetPos - pointForXY;
			Debug.DrawRay(pointForXY, oldDir * 100, Color.blue);
			Debug.DrawRay(pointForXY, newDir * 100, Color.green);

			Vector3 pointForZ = originalHandPoint + player.up * 0.8f;

			if(Vector3.Dot(player.forward, handTargetPos - originalHandPoint) <= 0) //禁用根节点上扬
			{
				Vector3 qwe = player.InverseTransformPoint(handTargetPos);
				qwe.z = player.InverseTransformPoint(originalHandPoint).z;
				handTargetPos = player.TransformPoint(qwe);
			}

			// Vector3 targetPoint = Vector3.Dot(player.forward, handTargetPos - originalHandPoint) * (player.forward) + originalHandPoint;
			Vector3 oldZDir = originalHandPoint - pointForZ;
			// Vector3 newZDir = targetPoint - pointForZ;
			Vector3 newZDir = handTargetPos - pointForZ;

	
			Debug.DrawRay(originalHandPoint, player.right * 100, Color.white);
			Debug.DrawRay(originalHandPoint, player.right * -100, Color.white);

			Debug.DrawRay(pointForZ, oldZDir * 100, Color.red);
			Debug.DrawRay(pointForZ, newZDir * 100, Color.yellow);

			Quaternion zRotate = Quaternion.FromToRotation(newZDir, oldZDir);
			Quaternion xyRotate = Quaternion.FromToRotation(oldDir, newDir);
	
			return zRotate * xyRotate;
		}
		public static void UpdateRotateBySet(Quaternion tend,IKSampleData _ikSampleData, float passedTime, Transform[] bones, Transform player, HandleTargetType type)
		{
			if(_ikSampleData._samplePresetType == SamplePresetType.Customize)
				UpdateRotateBySit(tend, _ikSampleData, passedTime, bones, player);
			if(_ikSampleData._samplePresetType == SamplePresetType.ForSit)
				UpdateRotateBySit(tend, _ikSampleData, passedTime, bones, player);
			if(_ikSampleData._samplePresetType == SamplePresetType.ForTake)
				UpdateRotateByTake(tend, _ikSampleData, passedTime, bones, type);
			if(_ikSampleData._samplePresetType == SamplePresetType.ForLie)
				UpdateRotateByLie(tend, _ikSampleData, passedTime, bones, player);
		}

	}

}
