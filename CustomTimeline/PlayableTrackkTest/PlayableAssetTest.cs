using System;
using UnityEngine;
using UnityEngine.Playables;
[Serializable]
public class PlayableAssetTest : PlayableAsset
{
	public float data = 1;
	public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
	{
		var playable = ScriptPlayable<PlayableTrackkTest>.Create(graph, 1);
		playable.GetBehaviour().data = data;
		return playable;
	}
}