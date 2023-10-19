using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GetPic : MonoBehaviour
{
	public Camera camera;
	 WaitForEndOfFrame wd = new WaitForEndOfFrame();
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.S))
		{
			StartCoroutine(CatchPic2());
			// File.OpenFile(Application.dataPath + "/aaaaaaaaaaa" + "/wawaaw.png", FileMode.Create);
		}
    }


	IEnumerator CatchPic()
	{
		yield return wd;
		RenderTexture rt = new RenderTexture(Screen.width, Screen.height, 32);
		RenderTexture currentActive = RenderTexture.active;
		RenderTexture.active = rt;
		Texture2D tex = new Texture2D(rt.width, rt.height, TextureFormat.ARGB32, false);
		Camera.main.targetTexture = rt;

		tex.ReadPixels(new Rect(0, 0, tex.width, tex.height), 0, 0);
		tex.Apply();
		RenderTexture.active = currentActive;

		var bytes = tex.EncodeToPNG();
		SaveInFile("first", bytes);
	}

	IEnumerator CatchPic2()
	{
		yield return wd;
		RenderTexture rt = new RenderTexture(Screen.width, Screen.height, 24);
		Camera.main.targetTexture = rt;
		Texture2D tex = new Texture2D(rt.width, rt.height);
		tex.ReadPixels(new Rect(0, 0, tex.width, tex.height), 0, 0);
		var bytes = tex.EncodeToPNG();
		SaveInFile("secone", bytes);

	}

	IEnumerator CatchPic3()
	{
		yield return wd;
		RenderTexture rt = new RenderTexture(Screen.width, Screen.height, 16);
		Texture2D tex = ScreenCapture.CaptureScreenshotAsTexture();
		tex.ReadPixels(new Rect(0, 0, tex.width, tex.height), 0, 0);
		var bytes = tex.EncodeToPNG();
		SaveInFile("third", bytes);
	}

	IEnumerator CatchPic4()
	{
		yield return wd;
		Texture2D tex = new Texture2D(Screen.width, Screen.height);
		for(int i = 0; i < tex.width; i++)
		{
			for(int j = 0; j < tex.height; j++)
			{
				tex.SetPixel(i, j ,new Color(0, 0, 0, 0));
			}
		}
		tex.ReadPixels(new Rect(0, 0, tex.width, tex.height), 0, 0);
		tex.Apply();
		var bytes = tex.EncodeToPNG();
		SaveInFile("fourth", bytes);
	}

	void SaveInFile(string fileName, byte [] bytes)
	{
		var file = File.Open(Application.dataPath + "/../aaaaaaaaaaa" + $"/{fileName}.png", FileMode.Create);
		var binary = new BinaryWriter(file);
		binary.Write(bytes);
		file.Close();
		Debug.Log($"保存完毕 : {fileName}");
		
	}
}
