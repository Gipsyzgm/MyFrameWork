using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 双击返回键推出程序。需要就挂载。
/// </summary>
public class QuitGame : MonoBehaviour {
    public float i1 = 0;
    public float i2 = 0;
    public int CiShu = 0;//次数
                         // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))//按手机：返回键
        {
            CiShu += 1;//按一次就加一次
            if (CiShu == 1) { i1 = Time.time; }//记录第一次按下返回键的时间

            if (CiShu == 2)
            {
                i2 = Time.time;//记录第二次按下返回键的时间

                if (i2 - i1 <= 2f)
                {//第二次 减 第一次的时间在2秒内，就执行
                    Application.Quit();//退出
                }
                else//否则次数归还为0
                {
                    CiShu = 0;
                }
            }


        }

    }
}
