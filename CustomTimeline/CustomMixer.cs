using UnityEngine;
using UnityEngine.Playables;

public class CustomMixer : PlayableBehaviour
{
	public override void OnGraphStart(Playable playable)
	{
		base.OnGraphStart(playable);
	}
	public override void OnBehaviourPlay(Playable playable, FrameData info)
	{
		base.OnBehaviourPlay(playable, info);
	}

	public override void PrepareFrame(Playable playable, FrameData info)
	{
		base.PrepareFrame(playable, info);
	}
	public override void ProcessFrame(Playable playable, FrameData info, object playerData)
	{
		// (playerData as GameObject).transform.position += Vector3.up * 0.1f;
		for(int i = 0; i < playable.GetInputCount(); i++)
		{
			float weight = playable.GetInputWeight(i);
			var clip = playable.GetInput(i);

		}
		base.ProcessFrame(playable, info, playerData);
	}
	public override void OnPlayableDestroy(Playable playable)
	{
		base.OnPlayableDestroy(playable);
	}
}