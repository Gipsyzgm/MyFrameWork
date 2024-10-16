using System;
using UnityEngine;
public class GameBasicInfoEditor : ScriptableObject
{
    [SerializeField]
    public string LoginServer;
    [SerializeField]
    public bool remoteResCheck;
    [SerializeField]
    public string remoteResPath;
    [SerializeField, Min(0f)]
    public float minConnectElapseTime = 3f;
    [SerializeField]
    public bool fcm;
    [SerializeField]
    public string AppVersion;
    
}
