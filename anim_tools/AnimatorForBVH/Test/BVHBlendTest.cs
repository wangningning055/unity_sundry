using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class BVHBlendTest : MonoBehaviour
{
	public Animator animator;
	public AnimationClip clip1, clip2, currentClip;
	BVHParser fileBvh1, fileBvh2;
	public float animTime = 0; 
	int bvhIndex1 = -1;
	int bvhIndex2 = -1;
	int currentIndex = -1;
	bool isAnim = false;
	BvhClip bvhClip1, bvhClip2;
	public Transform root;
	public List<string> asd = new List<string>();
	void Start()
	{
		fileBvh1 = new BVHParser(File.ReadAllText("F:\\aaaaaaa\\dancess\\audio_那也祝你好运啦_label_Neutral.bvh"));
		fileBvh2 = new BVHParser(File.ReadAllText("F:\\aaaaaaa\\dancess\\22222.bvh"));
		// fileBvh1 = new BVHParser(File.ReadAllText(Application.dataPath + "/../BVHFile" + $"/009_Sad_3_x_1_0.bvh"));
		// fileBvh2 = new BVHParser(File.ReadAllText(Application.dataPath + "/../BVHFile" + $"/065_Speech_0_x_1_0.bvh"));
		bvhIndex1 = BVHPlayerManager.Instance.InitPlayer(root, false);
		bvhIndex2 = BVHPlayerManager.Instance.InitPlayer(root, false);
		bvhClip1 = BVHPlayerManager.Instance.GetPlayer(bvhIndex1);
		bvhClip2 = BVHPlayerManager.Instance.GetPlayer(bvhIndex2);
		bvhClip1.SetBVHByParser(fileBvh1);
		bvhClip2.SetBVHByParser(fileBvh2);
		bvhClip1.Play();
		currentIndex = bvhIndex1;
		// //添加event
		// bvhClip1.bvhEventCom.AddEvent(0.5f, () =>{
		// 	Debug.Log("触发0.5秒时的事件");
		// });
		// //移除event
		// bvhClip1.bvhEventCom.RemoveEvent(0.5f);
		// bvhClip1.bvhEventCom.AddEvent(1, () =>{
		// 	Debug.Log("触发1秒时的事件");
		// });
		// bvhClip1.bvhEventCom.AddEvent(2, () =>{
		// 	Debug.Log("触发2秒时的事件");
		// });
		// bvhClip1.bvhEventCom.AddEvent(3, () =>{
		// 	Debug.Log("触发3秒时的事件");
		// });
		// bvhClip1.bvhEventCom.AddEvent(4, () =>{
		// 	Debug.Log("触发4秒时的事件");
		// });
		// bvhClip1.bvhEventCom.AddEvent(5, () =>{
		// 	Debug.Log("触发5秒时的事件");
		// });
		// bvhClip1.bvhEventCom.AddEvent(1.5f, () =>{
		// 	Debug.Log("触发1.5秒时的事件");
		// });

	}
	void OnGUI()
	{
		if(GUILayout.Button("播放bvh1"))
		{
			bvhClip1.Play();
			bvhClip2.Stop();
			currentIndex = bvhIndex1;
			isAnim = false;
		}
		if(GUILayout.Button("播放bvh2"))
		{
			bvhClip1.Stop();
			bvhClip2.Play();
			currentIndex = bvhIndex2;
			isAnim = false;
		}
		if(GUILayout.Button("播放AnimatorClip1"))
		{
			bvhClip1.Stop();
			bvhClip2.Stop();
			isAnim = true;

			AnimatorReBind.Instance.ReBindFuc(animator, clip1, "New Animation");
			animator.Play("New Animation", 0, 0);
			currentClip = clip1;
		}
		if(GUILayout.Button("播放AnimatorClip2"))
		{
			bvhClip1.Stop();
			bvhClip2.Stop();
			isAnim = true;
			AnimatorReBind.Instance.ReBindFuc(animator, clip2, "New Animation");
			animator.Play("New Animation", 0, 0);
			currentClip = clip2;
		}
		if(GUILayout.Button("bvh混合到bvh"))
		{
			if(isAnim == true) return;
			int fromBvhIndex = currentIndex;
			int toBvhIndex = currentIndex == bvhIndex1? bvhIndex2:bvhIndex1;
			BvhClip play1 = BVHPlayerManager.Instance.GetPlayer(fromBvhIndex);
			BvhClip play2 = BVHPlayerManager.Instance.GetPlayer(toBvhIndex);

			SampleBlendManager.Instance.BlendBvhToBvh(fromBvhIndex, toBvhIndex, 0, 1,() =>{
				
				currentIndex = toBvhIndex;
				play1.Stop();
				play2.Update();
			});
		}
		if(GUILayout.Button("bvh混合到animator"))
		{
			if(isAnim == true) return;
			int fromBvhIndex = currentIndex;
			BvhClip play1 = BVHPlayerManager.Instance.GetPlayer(fromBvhIndex);
			AnimatorReBind.Instance.ReBindFuc(animator, currentClip, "New Animation");
			animator.Play("New Animation", 0, 0);
			SampleBlendManager.Instance.BlendBvhToAnimator(fromBvhIndex, animator, "New Animation", 0, 1,() =>{
				bvhClip1.Stop();
				bvhClip2.Stop();
				animator.Update(Time.deltaTime);
				isAnim = true;
			});
		}
		if(GUILayout.Button("animator混合到bvh"))
		{
			if(isAnim == false) return;
			bvhClip1.Stop();
			bvhClip2.Stop();
			int toBvhIndex = currentIndex;
			BvhClip play1 = BVHPlayerManager.Instance.GetPlayer(toBvhIndex);
			play1.Play();
			SampleBlendManager.Instance.BlendAnimatorToBvh(animator, toBvhIndex, 0, 1,() =>{
				isAnim = false;
			});
		}
		// if(GUILayout.Button("继续播bvh"))
		// {
		// 	BvhClip play1 = BVHPlayerManager.Instance.GetPlayer(currentIndex);
		// 	play1.Play();
		// 	isAnim = false;
		// }
		// if(GUILayout.Button("暂停bvh"))
		// {
		// 	BvhClip play1 = BVHPlayerManager.Instance.GetPlayer(currentIndex);
		// 	play1.Pause();
		// 	isAnim = false;
		// }
		// if(GUILayout.Button("重播bvh"))
		// {
		// 	BvhClip play1 = BVHPlayerManager.Instance.GetPlayer(currentIndex);
		// 	play1.Stop();
		// 	play1.Play();
		// 	isAnim = false;
		// }
		// if(GUILayout.Button("删除事件"))
		// {
		// 	bvhClip1.bvhEventCom.RemoveEvent(3);
		// }
		// if(GUILayout.Button("添加事件"))
		// {
		// 	bvhClip1.bvhEventCom.AddEvent(4.5f, () =>{
		// 		Debug.Log("触发4.5秒时的事件");
		// 	});
		// }
	}

}
