using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;
using System;
using UnityEngine.PlayerLoop;
public class BVHUpdataManager
{

	static BVHUpdataManager _bVHUpdataManager;
	int playerIndex = 0;
	public static BVHUpdataManager Instance
	{
		get{
			if(_bVHUpdataManager == null)
			{
				_bVHUpdataManager = new BVHUpdataManager();
				UpdateInsert.Instance.InsertUpdate(_bVHUpdataManager.Update, typeof(BVHUpdataManager), InsertType.AfterDirectorUpdateAnimEnd);
			}
			return _bVHUpdataManager;
		}
	}
	public void Init()
	{
		
	}

	public void Update()
	{
		SampleBlendManager.Instance.BeforBvhUpdate();
		BVHPlayerManager.Instance.Update();
		SampleBlendManager.Instance.Update();
	}
}