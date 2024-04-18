using UnityEngine.Playables;
using UnityEngine;
using System;
[Serializable]
public class PlayableTrackkTest : PlayableBehaviour
{
	public float data;
	public override void ProcessFrame(Playable playable, FrameData info, object playerData)
	{
		Debug.Log(data);
		base.ProcessFrame(playable, info, playerData);
	}
}