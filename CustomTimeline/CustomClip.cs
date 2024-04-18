using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
[Serializable]
public class CustomClip : PlayableAsset
{
	public ClipCaps clipCaps
	{
		get{return ClipCaps.None;}
	}

	public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
	{
		var playable = ScriptPlayable<CustomPlayableBehaviour>.Create(graph);
		var behaviour = playable.GetBehaviour();

		return playable;
	}
}