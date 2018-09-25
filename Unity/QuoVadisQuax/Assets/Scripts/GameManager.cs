using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private OptionsManager _optionsManager;
    
    private static GameManager _instance;
    /// <summary>
    /// The Singleton Instance
    /// </summary>
    public static GameManager Instance
    {
        get { return _instance; }
    }
    
    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else if (_instance != this)
            Destroy(gameObject);
    }

    private void Start()
    {
        _optionsManager.StartedAlgorithm += OnStartAlgorithm;
    }

    private void OnStartAlgorithm(Vector2Int quaxPos, Vector2Int cityPos)
    {
        Debug.Log(quaxPos + " " + cityPos);
    }
}
