using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Data;
public class GenAB
{
	[MenuItem("打包/批量设置AB名字")]
	public static void NameAB()
	{
		Debug.Log(Application.dataPath);
		SetABName("Image", "image");
		SetABName("Material", "material");
		SetABName("Prefab", "prefab");
		SetABName("UI", "ui");
		SetLuaABName("Lua", ".txt");
	}
	[MenuItem("打包/构建AB")]
	public static void GenABs()
	{
		DirectoryInfo folder = new DirectoryInfo(Application.dataPath + "/AssetBundle");
		foreach(FileInfo fileInfo in folder.GetFiles("*.*", SearchOption.AllDirectories))
		{
			File.Delete(fileInfo.FullName);
		}
		BuildPipeline.BuildAssetBundles(Application.dataPath + "/AssetBundle", BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.StandaloneWindows);
		AssetDatabase.Refresh();
		Debug.Log("生成完毕");
	}
	[MenuItem("打包/复制LUA文件夹")]
	public static void CopyLUA()
	{
		if(Directory.Exists(Application.dataPath + "/Lua"))
		{
			Directory.Delete(Application.dataPath + "/Lua", true);
			File.Delete(Application.dataPath + "/Lua.meta");
		}
		AssetDatabase.Refresh();
		CopyLuaFile("Lua");
		AssetDatabase.Refresh();
		Debug.Log("复制完毕");
	}
	public static void CopyLuaFile(string path)
	{
		string sourcePath = Application.dataPath + "/../" + path;
		string targetPath = Application.dataPath + "/" + path;
		if(!Directory.Exists(targetPath))
		{
			Directory.CreateDirectory(targetPath);
		}
		DirectoryInfo folder = new DirectoryInfo(sourcePath);
		foreach(FileInfo fileInfo in folder.GetFiles("*.lua", SearchOption.TopDirectoryOnly))
		{
			string baseName = fileInfo.FullName;
			string targetName = targetPath + "/" + fileInfo.Name + ".txt";
			File.Copy(baseName, targetName);
		}
		foreach(DirectoryInfo fileInfo1 in folder.GetDirectories("*.*", SearchOption.TopDirectoryOnly))
		{
			CopyLuaFile(path + "/" + fileInfo1.Name);
		}
	}
	public static void SetLuaABName(string path, string behind)
	{
		string sourcePath = Application.dataPath + "/" + path;
		Debug.Log(sourcePath);
		DirectoryInfo folder = new DirectoryInfo(sourcePath);
		foreach(FileInfo fileInfo in folder.GetFiles("*.txt", SearchOption.TopDirectoryOnly))
		{
			AssetImporter aaa = AssetImporter.GetAtPath("Assets/" + path + "/" + fileInfo.Name);
			Debug.Log("Assets/../" + path + "/" + fileInfo.Name);
			aaa.SetAssetBundleNameAndVariant("lua","assetBundle");
		}
		foreach(DirectoryInfo fileInfo1 in folder.GetDirectories("*.*", SearchOption.TopDirectoryOnly))
		{
			SetLuaABName(path + "/" + fileInfo1.Name, behind);
		}
	}
	public static void SetABName(string directoryName, string bundleName)
	{
		string sourcePath = Application.dataPath + "/" + "Raw/" + directoryName;
		Debug.Log(sourcePath);

		DirectoryInfo folder = new DirectoryInfo(sourcePath);
		foreach(FileInfo fileInfo in folder.GetFiles("*.*", SearchOption.AllDirectories))
		{
			if(!fileInfo.FullName.EndsWith(".meta"))
			{
				AssetImporter aaa = AssetImporter.GetAtPath("Assets/Raw/" + directoryName + "/" + fileInfo.Name);
				aaa.SetAssetBundleNameAndVariant(bundleName,"assetBundle");
				Debug.Log(aaa);
			}
		}
	
	}
}
