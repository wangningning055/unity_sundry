 using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class BoneMapDic{
	public Dictionary<string, string> boneNameMapping = new Dictionary<string, string>();
	public Dictionary<string, string> special = new Dictionary<string, string>();
	public Dictionary<string, Quaternion> specialRotate = new Dictionary<string, Quaternion>();

	static BoneMapDic _instance;
	public static BoneMapDic Instance
	{
		get{
			if(_instance == null) 
			{
				_instance = new BoneMapDic();
				_instance.InitBoneNameMapping();
			}
			return _instance;
		}
	}
	private void InitBoneNameMapping()
	{
		// boneNameMapping.Clear();
		if(boneNameMapping.Keys.Count > 0)
		return;
		// ���ӹ�������ӳ���ϵ
		boneNameMapping.Add("Hips", "Bip001_Pelvis");
		boneNameMapping.Add("Spine", "Spine");
		boneNameMapping.Add("Spine1", "Spine1");
		boneNameMapping.Add("Spine3", "Spine2");
		// boneNameMapping.Add("Spine3", "");
		boneNameMapping.Add("Neck", "Neck");
		//boneNameMapping.Add("Neck1", "neck2");
		boneNameMapping.Add("Head", "Head");
		// boneNameMapping.Add("HeadEnd", "");
		boneNameMapping.Add("RightShoulder", "Bip001_R_Clavicle");
		boneNameMapping.Add("RightArm", "Bip001_R_UpperArm");
		boneNameMapping.Add("RightForeArm", "Bip001_R_Forearm");
		boneNameMapping.Add("RightHand", "Bip001_R_Hand");
		boneNameMapping.Add("RightHandThumb1", "Bip001_R_Finger0");
		boneNameMapping.Add("RightHandThumb2", "Bip001_R_Finger01");
		boneNameMapping.Add("RightHandThumb3", "Bip001_R_Finger02");
		// boneNameMapping.Add("RightHandThumb4", "");
		boneNameMapping.Add("RightHandIndex1", "Bip001_R_Finger1");
		boneNameMapping.Add("RightHandIndex2", "Bip001_R_Finger11");
		boneNameMapping.Add("RightHandIndex3", "Bip001_R_Finger12");
		// boneNameMapping.Add("RightHandIndex4", "");
		boneNameMapping.Add("RightHandMiddle1", "Bip001_R_Finger2");
		boneNameMapping.Add("RightHandMiddle2", "Bip001_R_Finger21");
		boneNameMapping.Add("RightHandMiddle3", "Bip001_R_Finger22");
		// boneNameMapping.Add("RightHandMiddle4", "");
		boneNameMapping.Add("RightHandRing1", "Bip001_R_Finger3");
		boneNameMapping.Add("RightHandRing2", "Bip001_R_Finger31");
		boneNameMapping.Add("RightHandRing3", "Bip001_R_Finger32");
		// boneNameMapping.Add("RightHandRing4", "");
		boneNameMapping.Add("RightHandPinky1", "Bip001_R_Finger4");
		boneNameMapping.Add("RightHandPinky2", "Bip001_R_Finger41");
		boneNameMapping.Add("RightHandPinky3", "Bip001_R_Finger42");
		// boneNameMapping.Add("RightHandPinky4", "");
		// boneNameMapping.Add("RightForeArmEnd", "");
		// boneNameMapping.Add("RightArmEnd", "");
		// left
		boneNameMapping.Add("LeftShoulder", "Bip001_L_Clavicle");
		boneNameMapping.Add("LeftArm", "Bip001_L_UpperArm");
		boneNameMapping.Add("LeftForeArm", "Bip001_L_Forearm");
		boneNameMapping.Add("LeftHand", "Bip001_L_Hand");
		boneNameMapping.Add("LeftHandThumb1", "Bip001_L_Finger0");
		boneNameMapping.Add("LeftHandThumb2", "Bip001_L_Finger01");
		boneNameMapping.Add("LeftHandThumb3", "Bip001_L_Finger02");
		// boneNameMapping.Add("LeftHandThumb4", "");
		boneNameMapping.Add("LeftHandIndex1", "Bip001_L_Finger1");
		boneNameMapping.Add("LeftHandIndex2", "Bip001_L_Finger11");
		boneNameMapping.Add("LeftHandIndex3", "Bip001_L_Finger12");
		// boneNameMapping.Add("LeftHandIndex4", "");
		boneNameMapping.Add("LeftHandMiddle1", "Bip001_L_Finger2");
		boneNameMapping.Add("LeftHandMiddle2", "Bip001_L_Finger21");
		boneNameMapping.Add("LeftHandMiddle3", "Bip001_L_Finger22");
		// boneNameMapping.Add("LeftHandMiddle4", "");
		boneNameMapping.Add("LeftHandRing1", "Bip001_L_Finger3");
		boneNameMapping.Add("LeftHandRing2", "Bip001_L_Finger31");
		boneNameMapping.Add("LeftHandRing3", "Bip001_L_Finger32");
		// boneNameMapping.Add("LeftHandRing4", "");
		boneNameMapping.Add("LeftHandPinky1", "Bip001_L_Finger4");
		boneNameMapping.Add("LeftHandPinky2", "Bip001_L_Finger41");
		boneNameMapping.Add("LeftHandPinky3", "Bip001_L_Finger42");
		// boneNameMapping.Add("LeftHandPinky4", "");
		// boneNameMapping.Add("LeftForeArmEnd", "");
		// boneNameMapping.Add("LeftArmEnd", "");
		// RightUpLeg
		boneNameMapping.Add("RightUpLeg", "Bip001_R_Thigh");
		boneNameMapping.Add("RightLeg", "Bip001_R_Calf");
		boneNameMapping.Add("RightFoot", "Bip001_R_Foot");
		boneNameMapping.Add("RightToeBase", "Bip001_R_Toe0");
		// boneNameMapping.Add("RightToeBaseEnd", "");
		// boneNameMapping.Add("RightLegEnd", "");
		// boneNameMapping.Add("RightUpLegEnd", "");
		// LeftUpLeg
		boneNameMapping.Add("LeftUpLeg", "Bip001_L_Thigh");
		boneNameMapping.Add("LeftLeg", "Bip001_L_Calf");
		boneNameMapping.Add("LeftFoot", "Bip001_L_Foot");
		boneNameMapping.Add("LeftToeBase", "Bip001_L_Toe0");
		// boneNameMapping.Add("LeftToeBaseEnd", "");
		// boneNameMapping.Add("LeftLegEnd", "");
		// boneNameMapping.Add("LeftUpLegEnd", "");
		InitSpecial();
	}
	void InitSpecial()
	{
		special.Add("Spine2", "Neck");
		special.Add("Bip001_Pelvis", "Spine");
		special.Add("Bip001_L_Hand", "Bip001_L_Finger2");
		special.Add("Bip001_R_Hand", "Bip001_R_Finger2");

	}
	void InitSpecialRotate()
	{
		// specialRotate.Add("H", "Neck");


	}
}