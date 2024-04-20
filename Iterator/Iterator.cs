using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//c#自定义迭代器

//定义要遍历的基础的数据
public class Email
{
	public string name;
	public Email(string na)
	{
		name = na;
	}
}

//定义可以被遍历的集合，需要继承IEnumerable接口，实现GetEnumerator接口，用来返回对应的迭代器
public class EmailList : IEnumerable
{
	List<Email> list = new List<Email>();

    public IEnumerator GetEnumerator()
    {
		return new MailListor(list);
    }
	public void Add(Email email)
	{
		list.Add(email);
	}
}

//定义对应的自定义集合的迭代器，实现IWnumerate<T>的接口，内容有moveNext用来判断是否结束，Current用来返回当前迭代器的数据，Dispose是迭代结束调用，reset重置
//自定义的迭代器可以用来实现特殊的输出顺序的集合
public class MailListor : IEnumerator<Email>
{
	List<Email> emailList;
	public MailListor(List<Email> emails)
	{
		emailList = emails;
	}
	int curIndex = -1;
	public Email Current {
		get{
			return emailList[curIndex];
		}
	}
	object IEnumerator.Current => emailList[curIndex];

    public void Dispose()
    {
        Debug.Log("遍历完毕");
    }

    public bool MoveNext()
    {
		Debug.Log("moveNext");
        curIndex++;
		Debug.Log($"{curIndex},   {curIndex < emailList.Count}");

		return curIndex < emailList.Count;
    }

    public void Reset()
    {
        curIndex = -1;
    }
}

//使用yield关键字语法糖将不再考虑IEnumerator，会自动给生成
public class EmailList2:IEnumerable
{
	List<Email> list = new List<Email>();

    public IEnumerator GetEnumerator()
    {
		for(int i = 0; i < list.Count; i++)
		{

			yield return list[0];

		}
    }
	public void Add(Email email)
	{
		list.Add(email);
	}
}

public class EmailMgr
{
	Email email1 = new Email("11");
	Email emai2 = new Email("21");
	Email email3 = new Email("31");
	Email email4 = new Email("41");
	Email email5 = new Email("51");


	// EmailList emailList = new EmailList();
	// public void Forech()
	// {
	// 	emailList.Add(email1);
	// 	emailList.Add(emai2);
	// 	emailList.Add(email3);
	// 	emailList.Add(email4);
	// 	emailList.Add(email5);

	// 	foreach(Email email in emailList)
	// 	{
	// 		Debug.Log(email.name);
	// 	}
	// }
	EmailList2 emailList2 = new EmailList2();
	public void Forech2()
	{
		emailList2.Add(email1);
		emailList2.Add(emai2);
		emailList2.Add(email3);
		emailList2.Add(email4);
		emailList2.Add(email5);

		foreach(Email email in emailList2)
		{
			Debug.Log(email.name);
		}
	}
}


public class CoroutineTest2 : MonoBehaviour
{
	bool isOk = true;
	EmailMgr emailMgr;
	void Start()
	{
		emailMgr = new EmailMgr();
		emailMgr.Forech2();
	
	}

	// void Update()
	// {
	// 	emailMgr.Forech2();
	// 	if(Input.GetKeyDown(KeyCode.A))
	// 	{

	// 	}
	// }

}