using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Playables;
public class CustomPlayableBehaviour  : PlayableBehaviour
{
	public float data = 2;
	public override void ProcessFrame(Playable playable, FrameData info, object playerData)
	{
		Debug.Log(playerData);
		base.ProcessFrame(playable, info, playerData);
	}
}