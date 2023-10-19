using System;
using UnityEngine;
public class BVHPlayerTest : MonoBehaviour
{
	public Transform root, specialRoot;
	BvhClip player;
	public bool isShowUI = false;
	public bool isSpecial = false;
	int playerIndex = -1;
	private void OnDrawGizmos() {
	if(player != null && player.isInit)
		DrawBvh(player.bvhParser.root);
	}

	void DrawBvh(BVHParser.BVHBone bone)
	{
		if(!isShowUI) return;
		if(player == null || !player.isInit) return;
		if(bone == null) return;
		player.GetWorldPosDic().TryGetValue(bone.name, out Vector3 unityForBvhPos);
		Gizmos.color = Color.blue;
		Gizmos.DrawSphere(unityForBvhPos, 0.01f);
		// worldPosForBvhObj.TryGetValue(bone.name, out Transform unityForBvhPosobj);
		// unityForBvhPosobj.position = unityForBvhPos;
		foreach(BVHParser.BVHBone child in bone.children)
		{
			player.GetWorldPosDic().TryGetValue(child.name, out Vector3 unityForBvhChild);
			Gizmos.color = Color.blue;
			Gizmos.DrawSphere(unityForBvhChild, 0.01f);
			Gizmos.color = Color.white;
			Gizmos.DrawLine(unityForBvhChild, unityForBvhPos);
			DrawBvh(child);
		}
	}

	void DrawBvhObj(BVHParser.BVHBone bone)
	{
		if(!isShowUI) return;
		if(player == null || !player.isInit) return;
		if(bone == null) return;
		// player = BVHPlayer.GetPlayer();
		player.GetWorldPosObjDic().TryGetValue(bone.name, out Transform unityForBvhPosObj);
		Gizmos.color = Color.blue;
		Gizmos.DrawSphere(unityForBvhPosObj.position, 0.01f);
		foreach(BVHParser.BVHBone child in bone.children)
		{
			player.GetWorldPosObjDic().TryGetValue(child.name, out Transform unityForBvhChildObj);
			Gizmos.color = Color.blue;
			// GUI.TextField(unityForBvhChildObj.position, child.name);
			Gizmos.DrawSphere(unityForBvhChildObj.position, 0.01f);
			Gizmos.color = Color.white;
			Gizmos.DrawLine(unityForBvhChildObj.position, unityForBvhPosObj.position);
			DrawBvhObj(child);
		}
	}


	void OnGUI()
	{
		if(!isShowUI) return;
		if(GUILayout.Button("加载"))
		{
            string fileStr = BVHReader.OpenFile();
			LoadBVH(fileStr);
		}
		if(GUILayout.Button("播放"))
		{
			BVHPlayerManager.Instance.Play(playerIndex);
		}
		if(GUILayout.Button("暂停"))
		{
			BVHPlayerManager.Instance.Pause(playerIndex);
		}
		if(GUILayout.Button("播第0帧"))
		{
			BVHPlayerManager.Instance.GetPlayer(playerIndex).PlayStepTime(0);
		}
		if(GUILayout.Button("重置"))
		{
			BVHPlayerManager.Instance.GetPlayer(playerIndex).ResetPos();
		}
		if(player == null || (!player.isInit)) return;
		// player = BVHPlayer.GetPlayer();
		int frame = player.frame;
		BVHParser bvhParser = player.bvhParser;

		float percent = (frame * bvhParser.frameTime) / (bvhParser.frames * bvhParser.frameTime);
		float newPercent = GUILayout.HorizontalSlider(percent, 0, 1, GUILayout.Width(200), GUILayout.Height(50));
		if(newPercent != percent)
		{
			frame = (int)Math.Floor(bvhParser.frames * newPercent);
			if(frame >= bvhParser.frames) frame = bvhParser.frames - 1;
			if(frame <= 0) frame = 0;
			player.frame = frame;
			player.timing = frame * bvhParser.frameTime;
		}

	
	}

	void LoadBVH(string fileStr)
	{
		// playerIndex = BVHPlayerManager.Instance.InitPlayer(root, false);
		if(playerIndex != -1)
		{
			// player.Reset();
			player.Stop();
		}

		playerIndex = BVHPlayerManager.Instance.InitPlayer(specialRoot, true);
		BVHPlayerManager.Instance.SetBVHStr(fileStr, playerIndex);
		player = BVHPlayerManager.Instance.GetPlayer(playerIndex);
	}
    void Update()
    {
		if(Input.GetKeyDown(KeyCode.R))
		{
#if UNITY_EDITOR
            string fileStr = BVHReader.OpenFile();
			if(isSpecial)
			{
				playerIndex = BVHPlayerManager.Instance.InitPlayer(specialRoot, true);
			}
			else
			{
				playerIndex = BVHPlayerManager.Instance.InitPlayer(root, false);
			}
			BVHPlayerManager.Instance.SetBVHStr(fileStr, playerIndex);
			player = BVHPlayerManager.Instance.GetPlayer(playerIndex);
#endif
		}
		if(Input.GetKeyDown(KeyCode.T))
		{
			BVHPlayerManager.Instance.Play(playerIndex);
		}

		if(Input.GetKeyDown(KeyCode.Y))
		{
			BVHPlayerManager.Instance.Pause(playerIndex);
		}
		if(Input.GetKeyDown(KeyCode.U))
		{
			BVHPlayerManager.Instance.Stop(playerIndex);
		}
		if(Input.GetKeyDown(KeyCode.G))
		{
			BVHPlayerManager.Instance.Beh(playerIndex);
		}
		if(Input.GetKeyDown(KeyCode.H))
		{
			BVHPlayerManager.Instance.Fow(playerIndex);
		}
    }
}
