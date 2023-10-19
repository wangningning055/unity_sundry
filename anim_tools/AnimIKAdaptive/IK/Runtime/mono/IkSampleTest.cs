using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;
using System.Text.RegularExpressions;
using System;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using UnityEngine.Animations;
namespace vw_animation_ik_runtime
{
public class IkSampleTest : MonoBehaviour
{

	int curAnimIndex = 0;
	int animNum = 4;
	public Animator _oriAnimator,_animator;
	public Transform sit;
	TrackBindingData bindData;
	Vector3 firstOriPos, firstRunPos;
	Quaternion firstOriRotate, firstRunRotate;
	int useIKIndex = -1;
	string curName;
	public GameObject CurveObj;
	PlayableDirector oldDirect;
	Dictionary<string, PlayableBinding> direct = new Dictionary<string, PlayableBinding>();
	public PlayableAsset DownTimeLine;
	public PlayableAsset UpTimeLine;
	PlayableBinding pbTemp;
	Vector3 oriPos = Vector3.zero;
	string animTrackTypeName = "UnityEngine.Timeline.AnimationTrack";
	string ikTrackTypeName = "IKMovieClipTrack";
	string animTrackName = "AnimationTrack";
	string ikTrackName = "IKMovieClipTrack";
	void Start()
	{
		firstOriPos = _oriAnimator.gameObject.transform.position;
		firstOriRotate = _oriAnimator.gameObject.transform.rotation;
		firstRunPos = _animator.gameObject.transform.position;
		firstRunRotate = _animator.gameObject.transform.rotation;
		CurveObj.SetActive(false);

		// LoadAnim(_oriAnimator);
		LoadAnim(_animator);
		oldDirect = _animator.gameObject.AddComponent<PlayableDirector>();
		
		bindData = ScriptableObject.CreateInstance<TrackBindingData>();
		bindData.SetData(sit, null);
		// bindData = UnityEngine.Object.Instantiate<TrackBindingData>(new TrackBindingData(sit, null));
		IKSampleRunTimeMgr.Instance.SetSit(_animator.gameObject.transform, bindData);
		oriPos = _animator.transform.position;
	}
	void LoadAnim(Animator animator)
	{
#if UNITY_EDITOR
		// EditorData data = EditorData.ReadData();
		AnimatorOverrideController animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
		AnimationClip[] clips = animatorOverrideController.animationClips;
		Action<string, int> initAnim = (path, index) =>
		{
			if(path == "") return;
			AnimationClip anim = UnityEditor.AssetDatabase.LoadAssetAtPath<AnimationClip>(path);
			animatorOverrideController[clips[index].name] = anim;
		};
		Action<AnimationClip, int> initAnimClip = (clip, index) =>
		{
			animatorOverrideController[clips[index].name] = clip;
		};
		animator.runtimeAnimatorController = animatorOverrideController;
		animator.Rebind();


#endif
	}
	public void GetData()
	{
		// GameObject lHand, rHand, root, spine;
		Action<string> getHandPoint = (name) =>
		{
			if(!sit.Find(name))
			{
				GameObject hang = new GameObject(name);
				hang.transform.parent = sit;
			}
		};
		getHandPoint(NameConst.rootPointName);
		getHandPoint(NameConst.lHandPointName);
		getHandPoint(NameConst.rHandPointName);
		getHandPoint(NameConst.spinePointName);
	}
	void PlayTimeLine(PlayableAsset timeLine)
	{
		oldDirect.Stop();
		direct = new Dictionary<string, PlayableBinding>();
		oldDirect.playableAsset = timeLine;

		var timelineAssest = oldDirect.playableAsset as TimelineAsset;
		foreach(var track in timelineAssest.GetOutputTracks())
		{
			string type = track.GetType().ToString();
			if(type == animTrackTypeName)
			{
				animTrackName = track.name;
				break;
			}
		}
		foreach(var track in timelineAssest.GetOutputTracks())
		{
			string type = track.GetType().ToString();
			if(type == ikTrackTypeName)
			{
				ikTrackName = track.name;
				break;
			}
		}
		foreach(var at in oldDirect.playableAsset.outputs)
		{

			if(at.streamName == animTrackName || at.streamName == ikTrackName)
			{
				direct.Add(at.streamName, at);
			}
		}
		var timelineAsset = oldDirect.playableAsset as TimelineAsset;
		foreach(var track in timelineAsset.GetOutputTracks())
		{
			var animTrack = track as AnimationTrack;
			if(animTrack == null) continue;
			animTrack.trackOffset = TrackOffset.ApplySceneOffsets;
		}
	
		if(direct.TryGetValue(animTrackName, out pbTemp))
		oldDirect.SetGenericBinding(direct[animTrackName].sourceObject, _animator);

		if(direct.TryGetValue(ikTrackName, out pbTemp))
		oldDirect.SetGenericBinding(direct[ikTrackName].sourceObject, bindData);

		oldDirect.Play();
	}
	private void Update() {
		if(Input.GetKeyDown(KeyCode.O))
		{
			PlayTimeLine(DownTimeLine);

		}
		if(Input.GetKeyDown(KeyCode.P))
		{
			PlayTimeLine(UpTimeLine);
			// oldDirect.Stop();
			// direct = new Dictionary<string, PlayableBinding>();
			// oldDirect.playableAsset = UpTimeLine;
			// // oldDirect.SetGenericBinding();
			// foreach(var at in oldDirect.playableAsset.outputs)
			// {
			// 	if(at.streamName == "PlayerTrack" || at.streamName == "IK Movie Clip Track")
			// 	{
			// 		direct.Add(at.streamName, at);
			// 	}
			// }
			// var timelineAsset = oldDirect.playableAsset as TimelineAsset;
			// foreach(var track in timelineAsset.GetOutputTracks())
			// {
			// 	var animTrack = track as AnimationTrack;
			// 	if(animTrack == null) continue;
			// 	animTrack.trackOffset = TrackOffset.ApplySceneOffsets;
			// }
			// if(direct.TryGetValue("PlayerTrack", out pbTemp))
			// oldDirect.SetGenericBinding(direct["PlayerTrack"].sourceObject, _animator);
			// if(direct.TryGetValue("IK Movie Clip Track", out pbTemp))
			// oldDirect.SetGenericBinding(direct["IK Movie Clip Track"].sourceObject, sit);
			// oldDirect.Play();
		}


		//进入站立idle状态就删除ik
		AnimatorStateInfo animInfo  = _animator.GetCurrentAnimatorStateInfo(0);
		if(animInfo.IsName("idle_loop") && useIKIndex != -1)
		{
			// IKSampleRunTimeMgr.RemoveIK(useIKIndex);
			useIKIndex = -1;
		}
		string cueName = _animator.GetCurrentAnimatorClipInfo(0).Length > 0 ? _animator.GetCurrentAnimatorClipInfo(0)[0].clip.name: "idle_loop";
		if(cueName != "idle_loop")
		{
			// AnimaChangeForIK(cueName);
		}
	}

	void AnimaChangeForIK(string animName)
	{
		if(animName == curName) return;
		IKSampleData data;
		if(!IKSampleFileUtil.ReadDataByeAnimName(animName, out data))
		{
			Debug.Log("找不到动画数据，删除ik");
			// IKSampleRunTimeMgr.RemoveIK(useIKIndex);
			useIKIndex = -1;
			curName = animName;

			return;
		}
		// useIKIndex = IKSampleRunTimeMgr.PlayIK(animName, _animator.transform, sit, useIKIndex);
		curName = animName;
		Debug.Log($"当前动画名称为：{curName}");
	}

}
}