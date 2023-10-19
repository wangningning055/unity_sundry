using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace vw_animation_ik_runtime
{
	public class IKSample:MonoBehaviour
	{
		void Start()
		{
			int[][][] aaa = new int[][][]
			{
				new int[][]{
					new int[]{1,2,3},
					new int[]{11,22,33},
					new int[]{111,222},
					new int[]{1111},
				},

				new int[][]{
					new int[]{5,6,7},
					new int[]{234,567,789},
				},
			};
			for(int i = 0; i < aaa.Length; i++)
			{
				for(int j = 0; j < aaa[i].Length; j++)
				{
					for(int k = 0; k<aaa[i][j].Length; k++)
					{
						Debug.Log(aaa[i][j][k]);
					}
				}
			}
		}
	}
}