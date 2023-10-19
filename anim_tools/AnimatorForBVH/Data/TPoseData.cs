using UnityEngine;
public class TPoseData : MonoBehaviour
{
	[SerializeField]
	public Quaternion[] TposeRotDic;
	public Vector3[] TposeBoneLocalPos;
	public Avatar avatar;
	public Transform PelvisBone;
	public void SetTpose()
	{
		Transform root = FindRoot();
		if(TposeRotDic.Length <= 0) {Debug.LogWarning("没有Tpose数据"); return;}
		Transform[] allTrans = root.GetComponentsInChildren<Transform>();
		for(int i = 0; i < allTrans.Length; i++)
		{
			allTrans[i].rotation = TposeRotDic[i];
			allTrans[i].localPosition = TposeBoneLocalPos[i];
		}
	}
	public Transform FindRoot()
	{
		Transform root = transform.Find("Root");
		if(root == null) {Debug.LogWarning("没有找到Root，脚本需要挂在Root骨骼的父节点上"); return null;}
		return root;
	}
}
