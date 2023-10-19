using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;
using MountPoint;
namespace vw_animation_ik_runtime
{
	public class IKSampleRunTimeMgr
	{
		public static IKSampleRunTimeMgr _ikMgr;
		public static IKSampleRunTimeMgr Instance{
			get{
				if(_ikMgr == null){
					_ikMgr = new IKSampleRunTimeMgr();
				}
				return _ikMgr;
			}
		}
		IKSampleMono _iKSampleMono;
		Dictionary<Transform, int> IKDic = new Dictionary<Transform, int>();
		Dictionary<Transform, TrackBindingData> sitDic = new Dictionary<Transform, TrackBindingData>();
		IKSampleMono IKSampleMono{
			get{
				if(_iKSampleMono == null)
				{
					var ikTotal = new GameObject("IKTotal");
					_iKSampleMono = ikTotal.AddComponent<IKSampleMono>();
				}
				return _iKSampleMono;
			}
		}
		public void SetSit(Transform Player, TrackBindingData Sit)
		{
			TrackBindingData sit;
			if(!sitDic.TryGetValue(Player, out sit))
			sitDic.Add(Player, Sit);
		}
		public int PlayIK(IKSampleData data, Transform player)
		{
			TrackBindingData sit;
			if(!sitDic.TryGetValue(player, out sit))
				{Debug.LogWarning("未找到座椅");return -1;}
			if(data == null || data.isCurveDataNull())
				{Debug.LogWarning($"没有相关动画采样数据"); return -1;}
			int index;
			if(!IKDic.TryGetValue(player,out index))
			{
				index = -1;
			}
			if(index == -1)
			{
				SingleIKMono ik = new SingleIKMono();
				ik.InitTotalData(data, sit, player);
				index = IKSampleMono.AddIK(ik);
				IKDic.Add(player, index);
				// Debug.Log("添加IK");
				return index;
			}
			// Debug.Log("更新IK");
			IKSampleMono.UpdateIK(index, data);
			return index;
		}

		public void RemoveIK(Transform player)
		{
			int index = 0;
			if(!IKDic.TryGetValue(player,out index))
			{
				return;
			}
			IKDic.Remove(player);
			if(sitDic.TryGetValue(player, out TrackBindingData sit))
			{
				// Debug.Log("移除座椅");
				sitDic.Remove(player);
			}
			IKSampleMono.RemoveIK(index);
			// Debug.Log("移除ik");
		}
		public SingleIKMono GetIK(int index)
		{
			return IKSampleMono.GetIK(index);
		}

	
	}
}
