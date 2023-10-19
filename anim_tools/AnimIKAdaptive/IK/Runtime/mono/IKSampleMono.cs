using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using RootMotion.FinalIK;
using System;
using MountPoint;
using Athena;
using InverseKinematic.Foot;
namespace vw_animation_ik_runtime
{
public class SingleIKMono
{

	public Transform handL,armL1, armL2, handR, armR1, armR2, footL, legL1, legL2, footR, legR1, legR2, neck, spine1, spine2, spine3;
	public Transform handLTarget, handRTarget, footLTarget, footRTarget;

	// public LimbIK IKSolverFootL, IKSolverFootR, IKSolverHandL, IKSolverHandR;
	public TwoBoneIK IKSolverFootLB, IKSolverFootRB, IKSolverHandLB, IKSolverHandRB;

	IKSampleData _iKSampleData;
	TrackBindingData _sit;
	Transform root, _player;
	AnimationCurve[][] oldCurves;
	AnimationCurve[][] newCurves;
	double passedTime;
	// List<BasePart> partList = new List<BasePart>();
	Dictionary<int, BasePart> partList = new Dictionary<int, BasePart>();

	bool isNeedRoot, isNeedLHand, isNeedRHand, isNeedSpine;
	// bool isUseLimb = false;
	public void InitIKSampleData()
	{
		partList.Clear();
		if(GetHangTargetByType(HandleTargetType.Root) == null) {Debug.LogWarning("Root家具挂点找不到");_iKSampleData.GetIsNeed()[0] = false;}
		if(GetHangTargetByType(HandleTargetType.HandL) == null) {Debug.LogWarning("HandL家具挂点找不到");_iKSampleData.GetIsNeed()[1] = false;}
		if(GetHangTargetByType(HandleTargetType.HandR) == null) {Debug.LogWarning("HandR家具挂点找不到");_iKSampleData.GetIsNeed()[2] = false;}
		if(GetHangTargetByType(HandleTargetType.Spine) == null) {Debug.LogWarning("Spine家具挂点找不到");_iKSampleData.GetIsNeed()[3] = false;}

		isNeedRoot =  _iKSampleData.GetIsNeed()[0];
		isNeedLHand = _iKSampleData.GetIsNeed()[1];
		isNeedRHand = _iKSampleData.GetIsNeed()[2];
		isNeedSpine = _iKSampleData.GetIsNeed()[3];

		Func<HandleTargetType, BasePart> init = (type) =>
		{
			BasePart part;
			if(type == HandleTargetType.Root || type == HandleTargetType.Spine)
				part = new IKRoot();
			else if(type == HandleTargetType.HandL || type == HandleTargetType.HandR)
				part = new IKHand();
			else
				part = new IKFoot();
			part.InitPart(_iKSampleData, _sit, _player, type);
			part.SetBone(root, handL, handR, footL, footR, spine1, spine2, spine3, neck);
			part.SetTarget(root, handLTarget, handRTarget, footLTarget, footRTarget);
			part.SetIKSolverBone(IKSolverHandLB, IKSolverHandRB, IKSolverFootLB, IKSolverFootRB);
			partList.Add((int)type, part);
			return part;
		};

		if(isNeedRoot || isNeedSpine)
			init(HandleTargetType.Root);

		if(isNeedLHand)
			init(HandleTargetType.HandL);

		if(isNeedRHand)
			init(HandleTargetType.HandR);

		init(HandleTargetType.FootR);
		init(HandleTargetType.FootL);




	}
	public void InitTotalData(IKSampleData iKSampleData, TrackBindingData sit, Transform player)
	{
		_iKSampleData = iKSampleData;

		// totalKeyframeList = _iKSampleData.GetCurveData().GetKeyframeList();
		_sit = sit;
		;
		Debug.Log(_sit.ikPrepareTrans[0]);
		Debug.Log($"{_sit.ikPrepareTrans[0].name}");
		_player = player;
		if(_player.GetComponent<CharacterCustomization>() == null)
			getBoneInTest();
		else
			getBoneInRunTime();
		InitIk();
		InitSolverAndTarget();
		InitIKSampleData();

	}
	void getBoneInTest()
	{
		Transform[] childList = _player.GetComponentsInChildren<Transform>();
		Func<string, Transform, Transform> getBone = (name, trans) =>
		{
			if(trans.name == name)
			{
				return trans;
			}
			return null;
		};
		foreach(Transform child in childList)
		{
			armL1 = armL1 == null? getBone(NameConst.armL1, child) : armL1;
			armL2 = armL2 == null? getBone(NameConst.armL2, child) : armL2;
			handL = handL == null? getBone(NameConst.handLName, child) : handL;
			armR1 = armR1 == null? getBone(NameConst.armR1, child) : armR1;
			armR2 = armR2 == null? getBone(NameConst.armR2, child) : armR2;
			handR = handR == null? getBone(NameConst.handRName, child) : handR;
			legL1 = legL1 == null? getBone(NameConst.legL1Name, child) : legL1;
			legL2 = legL2 == null? getBone(NameConst.legL2Name, child) : legL2;
			footL = footL == null? getBone(NameConst.footLName, child) : footL;
			legR1 = legR1 == null? getBone(NameConst.legR1Name, child) : legR1;
			legR2 = legR2 == null? getBone(NameConst.legR2Name, child) : legR2;
			footR = footR == null? getBone(NameConst.footRName, child) : footR;
			root = root == null? getBone(NameConst.rootName, child) : root;
			neck = neck == null? getBone(NameConst.neck, child) : neck;
			spine1 = spine1 == null? getBone(NameConst.spine1, child) : spine1;
			spine2 = spine2 == null? getBone(NameConst.spine2, child) : spine2;
			spine3 = spine3 == null? getBone(NameConst.spine3, child) : spine3;
		}
	}

	void getBoneInRunTime()
	{
		Func<string,Transform> getBone = (name) =>
		{
			return _player.GetComponent<CharacterCustomization>().GetTransformByName(name);
		};
		armL1 = getBone(NameConst.armL1);
		armL2 = getBone(NameConst.armL2);
		handL = getBone(NameConst.handLName);
		armR1 = getBone(NameConst.armR1);
		armR2 = getBone(NameConst.armR2);
		handR = getBone(NameConst.handRName);
		legL1 = getBone(NameConst.legL1Name);
		legL2 = getBone(NameConst.legL2Name);
		footL = getBone(NameConst.footLName);
		legR1 = getBone(NameConst.legR1Name);
		legR2 = getBone(NameConst.legR2Name);
		footR = getBone(NameConst.footRName);
		root = getBone(NameConst.rootName);
		neck = getBone(NameConst.neck);
		spine1 = getBone(NameConst.spine1);
		spine2 = getBone(NameConst.spine2);
		spine3 = getBone(NameConst.spine3);
	}
	void InitIk()
	{
		// Debug.Log("IK初始化");

		// Func<Transform, Transform, Transform, Transform, LimbIK> addIK = (bone1, bone2, bone3, target) =>
		// {

		// 	var limbIK = root.gameObject.AddComponent<LimbIK>();
		// 	limbIK.solver.SetChain(bone1, bone2, bone3, root);
		// 	limbIK.solver.target = target;
		// 	return limbIK;
		// };
		Func<Transform, Transform, Transform, Transform, TwoBoneIK> addIKBone = (bone1, bone2, bone3, target) =>
		{
			var ikCom = new TwoBoneIK();
            ikCom.Target = target;
			ikCom.First = bone1; ikCom.Second = bone2; ikCom.Third = bone3;
			ikCom.InitBone();
			return ikCom;
		};
		handLTarget = new GameObject("handLtarget").transform;
		handRTarget = new GameObject("handRTarget").transform;
		footLTarget = new GameObject("footLTarget").transform;
		footRTarget = new GameObject("footRTarget").transform;

		handLTarget.SetParent(_player);
		handRTarget.SetParent(_player);
		footLTarget.SetParent(_player);
		footRTarget.SetParent(_player);

		IKSolverHandLB = addIKBone(armL1, armL2, handL, handLTarget);
		IKSolverHandRB = addIKBone(armR1, armR2, handR, handRTarget);
		IKSolverFootLB = addIKBone(legL1, legL2, footL, footLTarget);
		IKSolverFootRB = addIKBone(legR1, legR2, footR, footRTarget);

	}

	void InitSolverAndTarget()
	{
		IKSolverFootLB.PositionWeight = 0;
		IKSolverFootRB.PositionWeight = 0;
		IKSolverHandLB.PositionWeight = 0;
		IKSolverHandRB.PositionWeight = 0;

	}
	public void SwitchAnim(IKSampleData iKSampleData)
	{
		_iKSampleData.ReleaseCurveData();
		_iKSampleData = iKSampleData;
		InitIKSampleData();
	}
	
	public void LateUpdate()
	{
		UpdateGeneralPos();
		RecPartAnimPos();
		UpdatePartPos((float)passedTime);
		UpdatePartFixPos((float)passedTime);
		UpdatePartRotate((float)passedTime);
		UpdateIK();

		//旋转修正放在最后（手脚旋转保持）
		UpdatePartFixRotate((float)passedTime);

		// TestLegRotate tes = _player.GetComponent<TestLegRotate>();
		// if(tes != null)
		// {
		// 	tes.UpdatePos();
		// }
		UpdateCloth();
	}
	public void UpdateCloth()
	{		
		if(_sit.ikPrepareTrans[1] == null)return;
	
	}
	public void UpdateIK()
	{
		IKSolverFootLB.OnFixUpdate();
		IKSolverFootRB.OnFixUpdate();
		IKSolverHandLB.OnFixUpdate();
		IKSolverHandRB.OnFixUpdate();

		IKSolverFootLB.OnLateUpdate();
		IKSolverFootRB.OnLateUpdate();
		IKSolverHandLB.OnLateUpdate();
		IKSolverHandRB.OnLateUpdate();
	}
	public void RecPartAnimPos()
	{
		Action<HandleTargetType> update = (type) =>
		{
			BasePart part;
			if(partList.TryGetValue((int)type, out part)) part.RecordAnimPos();
		};
		update(HandleTargetType.Root);
		update(HandleTargetType.HandL);
		update(HandleTargetType.HandR);
		update(HandleTargetType.FootL);
		update(HandleTargetType.FootR);
	}
	public void UpdatePartPos(float passedTime)
	{
		Action<HandleTargetType> update = (type) =>
		{
			BasePart part;
			if(partList.TryGetValue((int)type, out part)) part.UpdatePos(passedTime);
		};
		update(HandleTargetType.FootL);
		update(HandleTargetType.FootR);

		update(HandleTargetType.Root);
		
		BasePart part;
		if(partList.TryGetValue((int)HandleTargetType.HandL, out part)) part.RecordAnimPos();
		if(partList.TryGetValue((int)HandleTargetType.HandR, out part)) part.RecordAnimPos();

		update(HandleTargetType.HandL);
		update(HandleTargetType.HandR);


	}
	public void UpdatePartFixPos(float passedTime)
	{
		Action<HandleTargetType> update = (type) =>
		{
			BasePart part;
			if(partList.TryGetValue((int)type, out part)) part.FixUpdatePos(passedTime);
		};
		update(HandleTargetType.FootL);
		update(HandleTargetType.FootR);
		update(HandleTargetType.Root);


		BasePart part;
		if(partList.TryGetValue((int)HandleTargetType.HandL, out part)) part.RecordAnimPos();
		if(partList.TryGetValue((int)HandleTargetType.HandR, out part)) part.RecordAnimPos();


		update(HandleTargetType.HandL);
		update(HandleTargetType.HandR);

	}
	public void UpdatePartRotate(float passedTime)
	{
		Action<HandleTargetType> update = (type) =>
		{
			BasePart part;
			if(partList.TryGetValue((int)type, out part)) part.UpdateRotate(passedTime);
		};
		update(HandleTargetType.Root);
		update(HandleTargetType.HandL);
		update(HandleTargetType.HandR);
		update(HandleTargetType.FootL);
		update(HandleTargetType.FootR);
	}
	public void UpdatePartFixRotate(float passedTime)
	{
		Action<HandleTargetType> update = (type) =>
		{
			BasePart part;
			if(partList.TryGetValue((int)type, out part)) part.FixUpdateRotate(passedTime);
		};
		update(HandleTargetType.Root);
		update(HandleTargetType.HandL);
		update(HandleTargetType.HandR);
		update(HandleTargetType.FootL);
		update(HandleTargetType.FootR);
	}
	public void SetIKPassedTimeTime(double time)
	{
		passedTime = time;
	}


	void UpdateGeneralPos()
	{
		if(isNeedRoot) return;
		Vector3 handLAnimPos = handL.transform.position;
		Vector3 handRAnimPos = handR.transform.position;
		if(!isNeedLHand)
			handLTarget.transform.position = handLAnimPos;
		if(!isNeedRHand)
			handRTarget.transform.position = handRAnimPos;
	}

	Transform GetHangTargetByType(HandleTargetType type)
	{
		if(type == HandleTargetType.Root)
			return _sit.ikPrepareTrans[0].Find(NameConst.rootPointName);
		else if(type == HandleTargetType.HandL)
			return _sit.ikPrepareTrans[0].Find(NameConst.lHandPointName);
		else if(type == HandleTargetType.HandR)
			return _sit.ikPrepareTrans[0].Find(NameConst.rHandPointName);
		else
			return _sit.ikPrepareTrans[0].Find(NameConst.spinePointName);

	}


	public void Destroy()
	{
		if(_iKSampleData != null)
		_iKSampleData.ReleaseCurveData();
		IKSolverFootLB.OnFixUpdate();
		IKSolverFootRB.OnFixUpdate();
		IKSolverHandLB.OnFixUpdate();
		IKSolverHandRB.OnFixUpdate();

		GameObject.Destroy(handLTarget.gameObject);
		GameObject.Destroy(handRTarget.gameObject);
		GameObject.Destroy(footLTarget.gameObject);
		GameObject.Destroy(footRTarget.gameObject);
		partList.Clear();

	}
}


public class IKSampleMono : MonoBehaviour
{
	public Dictionary<int, SingleIKMono> totalDic = new Dictionary<int, SingleIKMono>();

	int totalIndex = 0;
	public int AddIK(SingleIKMono ik)
	{
		totalIndex++;
		totalDic.Add(totalIndex, ik);
		return totalIndex;
	}

	public void RemoveIK(int index)
	{
		SingleIKMono ik;
		if(!totalDic.TryGetValue(index, out ik))
		return;
		totalDic.Remove(index);
		// Debug.Log("销毁IK");
		ik.Destroy();
	}
	public SingleIKMono GetIK(int index)
	{
		SingleIKMono ik;
		totalDic.TryGetValue(index, out ik);
		return ik;
	}

	public void UpdateIK(int index, IKSampleData data)
	{
		SingleIKMono ik;
		if(!totalDic.TryGetValue(index, out ik))
		return;
		ik.SwitchAnim(data);
	}

	void LateUpdate()
	{
		foreach(SingleIKMono ik in totalDic.Values)
		{
			ik.LateUpdate();
		}
	}
	public void Destroy()
	{
		foreach(SingleIKMono ik in totalDic.Values)
		{
			ik.Destroy();
		}
	}

}
}
