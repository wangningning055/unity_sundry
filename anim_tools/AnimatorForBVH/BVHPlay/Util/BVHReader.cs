#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
public class BVHReader
{
	
    public static string OpenFile()
	{
		string path = EditorUtility.OpenFilePanel("打开bvh", Application.dataPath, "bvh");
		if(path != null)
		return File.ReadAllText(path);
		// return File.ReadAllText("F:\\aaaaaaa\\dancess\\audio_test_label_001_Neutral_0_x_1_0.bvh");
		return "";
	}

}
#endif

