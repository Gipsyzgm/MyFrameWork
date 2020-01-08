using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IApplication : MonoBehaviour {


    void OnApplicationPause(bool pause)
    {
        if(pause)
        {
            ITime.OfflineTime();
        }
        else
        {

        }
    }

    void OnApplicationQuit()
    {
        ITime.OfflineTime();
    }
}
