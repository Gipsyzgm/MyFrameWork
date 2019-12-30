using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Test : MonoBehaviour {

    public Image image;
	// Use this for initialization
	void Start () {

        string Any = "@EndMark@";

        MyLog.LogWithColor("Sbfdasgdasgdgfdsafdsssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssss",Color.yellow);
      
    }
	
	// Update is called once per frame
	void Update () {
        

    }
    public void back()
    {
        SceneManager.LoadScene("test_ui");

    }
  
}
