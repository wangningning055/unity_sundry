using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(CustomClip), true)]
public class CustomClipWindow:Editor
{
	public float data;

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		GUILayout.Label("asdasdas");
	}
}