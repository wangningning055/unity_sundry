using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
[Serializable]
[TrackClipType(typeof(CustomClip))]
[TrackBindingType(typeof(GameObject))]
public class CustomTrack : TrackAsset
{
	public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
	{
		var mix = ScriptPlayable<CustomMixer>.Create(graph);
		mix.SetInputCount(inputCount);
		return mix;
	}


}