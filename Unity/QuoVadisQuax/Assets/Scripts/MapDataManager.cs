using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds information about the Map
/// SINGLETON
/// </summary>
public class MapDataManager : MonoBehaviour
{
    private static MapDataManager _instance;
    /// <summary>
    /// The Singleton Instance
    /// </summary>
    public static MapDataManager Instance
    {
        get { return _instance; }
    }

    /// <summary>
    /// The dimensions of the loaded image
    /// </summary>
    public Vector2Int Dimensions { get; set; }
    public List<Vector2Int> QuaxPositions { get; set; }
    public Vector2Int CityPosition { get; set; }

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else if (_instance != this)
            Destroy(gameObject);

        QuaxPositions = new List<Vector2Int>();
    }
}
