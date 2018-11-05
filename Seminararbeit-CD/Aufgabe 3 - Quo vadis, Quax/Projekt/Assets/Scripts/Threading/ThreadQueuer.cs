using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

/// <summary>
///     Manages multithreading
/// </summary>
public class ThreadQueuer : MonoBehaviour
{
    #region Properties

    private const int MainThreadActionsMultipleMaxSize = 100;
    private List<Action> _mainThreadActions;
    private List<Action> _mainThreadActionsMultiple;

    /// <summary>
    ///     The Singleton Instance
    /// </summary>
    public static ThreadQueuer Instance { get; private set; }

    #endregion

    #region Methods

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

            if (_mainThreadActionsMultiple.Count > MainThreadActionsMultipleMaxSize)
            {
                mainThreadActions = _mainThreadActionsMultiple.GetRange(0, MainThreadActionsMultipleMaxSize);
                clearList = false;
            }

            foreach (var a in mainThreadActions) a();

            if (clearList) _mainThreadActionsMultiple.Clear();
            else _mainThreadActionsMultiple.RemoveRange(0, MainThreadActionsMultipleMaxSize);
        }

        if (_mainThreadActions.Count > 0)
        {
            var a = _mainThreadActions[0];
            _mainThreadActions.RemoveAt(0);

            a();
        }
    }

    /// <summary>
    ///     Execute an action on new thread
    /// </summary>
    /// <param name="threadedAction">Action to execute on new thread</param>
    /// <param name="callback">Callback method</param>
    public void StartThreadedAction(Action threadedAction, Action callback = null)
    {
        var action = new ThreadStart(threadedAction);
        if (callback != null) action += () => { callback(); };

        var t = new Thread(new ThreadStart(threadedAction));
        t.Start();
    }

    /// <summary>
    ///     Execute an action on the main thread
    /// </summary>
    /// <param name="mainThreadAction">Action to execute on the main thread</param>
    /// <param name="callback">Callback method</param>
    public void QueueMainThreadAction(Action mainThreadAction, Action callback = null)
    {
        _mainThreadActions.Add(mainThreadAction);
        if (callback != null)
            _mainThreadActions.Add(callback);
    }

    /// <summary>
    ///     Execute an action along with multiple others on the main thread
    /// </summary>
    /// <param name="mainThreadAction">Action to execute on the main thread</param>
    public void QueueMainThreadActionMultiple(Action mainThreadAction)
    {
        _mainThreadActionsMultiple.Add(mainThreadAction);
    }

    #endregion
}