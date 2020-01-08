using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class IUpdate : MonoBehaviour {

    public static IUpdate Instance;

    int updateId;
    List<int> id = new List<int>();
    List<float> timeMax = new List<float>();
    List<float> timeCount = new List<float>();
    List<Action> updateAction = new List<Action>();

    List<Action> update = new List<Action>();
    List<Action> lateUpdate = new List<Action>();
    List<Action> fixedUpdate = new List<Action>();

    void Awake()
    {
        Instance = this;
    }
    void Update()
    {
        for (int i = 0; i < update.Count; i++)
            if (update[i] != null)
                update[i]();

        for (int i=0;i< timeCount.Count;i++)
        {
            timeCount[i] += Time.deltaTime;
            if(timeCount[i] >= timeMax[i])
                if (updateAction[i] != null)
                    updateAction[i]();
        }
    }
    void LateUpdate()
    {
        for (int i = 0; i < lateUpdate.Count; i++)
            if (lateUpdate[i] != null)
                lateUpdate[i]();
    }
    void FixedUpdate()
    {
        for (int i = 0; i < lateUpdate.Count; i++)
            if (lateUpdate[i] != null)
                lateUpdate[i]();
    }


    //添加移除普通的Update
    public void AddUpdate(Action action)
    {
        update.Add(action);
    }
    public void AddLateUpdate(Action action)
    {
        lateUpdate.Add(action);
    }
    public void AddFixedUpdate(Action action)
    {
        fixedUpdate.Add(action);
    }

    public void RemoveUpdate(Action action)
    {
        update.Remove(action);
    }
    public void RemoveLateUpdate(Action action)
    {
        lateUpdate.Remove(action);
    }
    public void RemoveFixedUpdate(Action action)
    {
        fixedUpdate.Remove(action);
    }

    //添加移除Update计时
    public int AddUpdateHandheld(Action _action,float _timeMax)
    {
        updateId++;
        id.Add(updateId);
        timeMax.Add(_timeMax);
        timeCount.Add(0);
        updateAction.Add(_action);
        return updateId;
    }
    public void RemoveUpdateHandheld(int _updateId)
    {
        if(id.Contains(_updateId))
        {
            int index = id.IndexOf(_updateId);
            id.RemoveAt(index);
            timeMax.RemoveAt(index);
            timeCount.RemoveAt(index);
            updateAction.RemoveAt(index);
        }
    }
}
