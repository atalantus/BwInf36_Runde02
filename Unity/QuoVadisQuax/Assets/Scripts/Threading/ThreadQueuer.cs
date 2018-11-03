using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System;
using System.Linq;

public class ThreadQueuer : MonoBehaviour
{
    private static ThreadQueuer _instance;
    /// <summary>
    /// The Singleton Instance
    /// </summary>
    public static ThreadQueuer Instance
    {
        get { return _instance; }
    }

    private List<Action> _mainThreadActions;
    private List<Action> _mainThreadActionsMultiple;
    private int _mainThreadActionsMultipleMaxSize = 100;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else if (_instance != this)
            Destroy(gameObject);

        _mainThreadActions = new List<Action>();
        _mainThreadActionsMultiple = new List<Action>();
    }

    private void Update()
    {
        if (_mainThreadActionsMultiple.Count > 0)
        {
            var clearList = true;
            
            List<Action> mainThreadActions = _mainThreadActionsMultiple;

            if (_mainThreadActionsMultiple.Count > _mainThreadActionsMultipleMaxSize)
            {
                mainThreadActions = _mainThreadActionsMultiple.GetRange(0, _mainThreadActionsMultipleMaxSize);
                clearList = false;
            }
            
            //Debug.LogWarning("MainThreadActionsMultiple: " + mainThreadActions.Count);
            
            for (int i = 0; i < mainThreadActions.Count; i++)
            {
                var a = mainThreadActions[i];
                a();
            }

            if (clearList) _mainThreadActionsMultiple.Clear();
            else _mainThreadActionsMultiple.RemoveRange(0, _mainThreadActionsMultipleMaxSize);
            
            //Debug.LogWarning(1f/Time.deltaTime);
        }
        
        if (_mainThreadActions.Count > 0)
        {
            var a = _mainThreadActions[0];
            _mainThreadActions.RemoveAt(0);

            a();
        }
    }

    public void StartThreadedAction(Action threadedAction, Action callback = null)
    {
        var action = new ThreadStart(threadedAction);
        if (callback != null)
        {
            action += () => { callback(); };
        }

        var t = new Thread(new ThreadStart(threadedAction));
        t.Start();
    }

    public void QueueMainThreadAction(Action mainThreadAction, Action callback = null)
    {
        _mainThreadActions.Add(mainThreadAction);
        if (callback != null)
            _mainThreadActions.Add(callback);
    }
    
    public void QueueMainThreadActionMultiple(Action mainThreadAction)
    {
        _mainThreadActionsMultiple.Add(mainThreadAction);
    }
}
