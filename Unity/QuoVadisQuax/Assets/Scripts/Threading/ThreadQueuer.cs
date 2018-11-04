using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ThreadQueuer : MonoBehaviour
{
    private readonly int _mainThreadActionsMultipleMaxSize = 100;
    private List<Action> _mainThreadActions;
    private List<Action> _mainThreadActionsMultiple;

    /// <summary>
    ///     The Singleton Instance
    /// </summary>
    public static ThreadQueuer Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);

        _mainThreadActions = new List<Action>();
        _mainThreadActionsMultiple = new List<Action>();
    }

    private void Update()
    {
        if (_mainThreadActionsMultiple.Count > 0)
        {
            var clearList = true;

            var mainThreadActions = _mainThreadActionsMultiple;

            if (_mainThreadActionsMultiple.Count > _mainThreadActionsMultipleMaxSize)
            {
                mainThreadActions = _mainThreadActionsMultiple.GetRange(0, _mainThreadActionsMultipleMaxSize);
                clearList = false;
            }

            //Debug.LogWarning("MainThreadActionsMultiple: " + mainThreadActions.Count);

            for (var i = 0; i < mainThreadActions.Count; i++)
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
        if (callback != null) action += () => { callback(); };

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