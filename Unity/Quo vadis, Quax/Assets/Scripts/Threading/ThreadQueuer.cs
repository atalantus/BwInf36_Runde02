using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System;

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

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else if (_instance != this)
            Destroy(gameObject);

        _mainThreadActions = new List<Action>();
    }

    private void Update()
    {
        if (_mainThreadActions.Count > 0)
        {
            var a = _mainThreadActions[0];
            _mainThreadActions.RemoveAt(0);

            a();
        }
    }

    public void StartThreadedAction(Action threadedAction)
    {
        var t = new Thread(new ThreadStart(threadedAction));
        t.Start();
    }

    public void QueueMainThreadAction(Action mainThreadAction)
    {
        _mainThreadActions.Add(mainThreadAction);
    }
}
