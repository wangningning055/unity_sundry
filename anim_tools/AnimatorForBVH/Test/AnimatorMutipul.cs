using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AnimatorMutipul : MonoBehaviour
{
	public GameObject playerOri;
	List<AnimatorTest> animatorList = new List<AnimatorTest>();

	public AnimationClip clip1, clip2;

	BVHParser fileBvh1, fileBvh2;
	public Transform bvhRoot;
	public Transform root;
	public float blendTime = 1;
	void Start()
	{
		// fileBvh1 = new BVHParser(File.ReadAllText(Application.dataPath + "/../BVHFile" + $"/009_Sad_3_x_1_0.bvh"));
		// fileBvh2 = new BVHParser(File.ReadAllText(Application.dataPath + "/../BVHFile" + $"/065_Speech_0_x_1_0.bvh"));
		playerOri.SetActive(false);
		fileBvh1 = new BVHParser(File.ReadAllText("F:\\aaaaaaa\\clean\\009_Sad_3_x_1_0.bvh"));
		fileBvh2 = new BVHParser(File.ReadAllText("F:\\aaaaaaa\\clean\\065_Speech_0_x_1_0.bvh"));

	}
	void CreatPlayer()
	{
		float x = UnityEngine.Random.Range(-5, 5);
		float z = UnityEngine.Random.Range(-5, 5);

		GameObject player = Instantiate<GameObject>(playerOri);
		player.SetActive(true);
		player.transform.position = new Vector3(x, 0, z);
		AnimatorTest animatorTest = new AnimatorTest();
		animatorTest.clip1 = clip1;
		animatorTest.clip2 = clip2;
		animatorTest.fileBvh1 = fileBvh1;
		animatorTest.fileBvh2 = fileBvh2;
		Transform body = player.transform.Find("female_body_001_lod0");
		animatorTest.animator = body.GetComponent<Animator>();

		foreach(var trans in body.GetComponentsInChildren<Transform>())
		{
			if(trans.name == bvhRoot.name)
			{
				animatorTest.BvhRoot = trans;
				break;
			}
		}
		foreach(var trans in body.GetComponentsInChildren<Transform>())
		{
			if(trans.name == root.name)
			{
				animatorTest.Root = trans;
				break;
			}
		}
		animatorTest.Start();
		animatorList.Add(animatorTest);
	}

	
	private void OnGUI() {

		if(GUILayout.Button("增加"))
		{
			CreatPlayer();
		}

		if(GUILayout.Button("播放"))
		{
			foreach(AnimatorTest animator in animatorList)
			{
				animator._blendAnimator.Play();
			}
		}

		if(GUILayout.Button("暂停"))
		{
			foreach(AnimatorTest animator in animatorList)
			{
				animator._blendAnimator.Puse();
			}
			
		}
		if(GUILayout.Button("停止"))
		{
			foreach(AnimatorTest animator in animatorList)
			{
				animator._blendAnimator.Stop();
			}

		}
		if(GUILayout.Button("ResetCondition"))
		{
			foreach(AnimatorTest animator in animatorList)
			{
				animator._blendAnimator.SetCondition("isAnim", !animator._blendAnimator.GetCondition("isAnim"));
			}
		}
		if(GUILayout.Button("切换到BVHClip1"))
		{
			foreach(AnimatorTest animator in animatorList)
			{
				animator._blendAnimator.JumpState(animator.bvhIndex1, blendTime, 0);
			}
		}
		if(GUILayout.Button("切换到BVHClip2"))
		{
			foreach(AnimatorTest animator in animatorList)
			{
				animator._blendAnimator.JumpState(animator.bvhIndex2, blendTime, 0);
			}
		}
		if(GUILayout.Button("切换到Animatior, clip1"))
		{
			foreach(AnimatorTest animator in animatorList)
			{
				AnimatorReBind.Instance.ReBindFuc(animator.animator, clip1, "New Animation");
				animator.animator.Play("New Animation", 0, 0);
				animator._blendAnimator.JumpState(animator.animIndex1, blendTime, 0);
			}
		}
		if(GUILayout.Button("切换到Animatior, clip2"))
		{
			foreach(AnimatorTest animator in animatorList)
			{
				AnimatorReBind.Instance.ReBindFuc(animator.animator, clip1, "New Animation");
				animator.animator.Play("New Animation", 0, 0);
				animator._blendAnimator.JumpState(animator.animIndex1, blendTime, 0);
			}
		}
	}
	
}