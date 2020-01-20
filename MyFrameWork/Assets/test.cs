using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour {

    void Start()
    {



    }
    /// <summary>
    /// 无参构造函数函数。继承MonoBehaviour。
    /// 状况1：当挂载到场景中的就会执行一次。
    /// 状况2：开始运行的时候会自动执行两次。
    /// 状况3：关闭运行的时候会自动执行一次。
    /// 结论：MonoBehaviour有两个生命周期，一个是作为C#对象的周期，
    /// 一个是作为Component的周期。构造函数代表第一个，Awake代表第二个。
    /// 即你的代码既可以作为Unity的脚本，也可以作为C#的脚本执行。
    /// 在不同的生命周期执行会表现的跟Player环境下不一样。
    /// 所以尽量不要用构造函数（很多文档都提到过这个），设计上需要用也要保证初始化代码尽可能简单。
    /// </summary>
    public test()
    {
        Debug.LogError("无参构造函数");
    }

    /// <summary>
    /// 有参构造函数。使用this关键字
    /// 状况：会先执行this后面串联的构造函数然后再执行自己的方法。
    /// </summary>
    /// <param name="text"></param>
    public test(string text) : this(text, 0)
    {
        Debug.LogError("1个参数的构造函数：");
        Debug.LogError("text：" + text);
    }
    /// <summary>
    /// 有参构造函数。不使用this关键字
    /// 状况：必须用包含对应参数的实例化方法才能执行。
    /// </summary>
    /// <param name="text"></param>
    /// <param name="x"></param>
    public test(string text,int x)
    {
        Debug.LogError("2个参数的构造函数：");
        Debug.LogError("text：" + text);
        Debug.LogError("x："+x);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
