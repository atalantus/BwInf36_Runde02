using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     Holds information about the Map
///     SINGLETON
/// </summary>
public class MapDataManager : MonoBehaviour
{
    /// <summary>
    ///     The Singleton Instance
    /// </summary>
    public static MapDataManager Instance { get; private set; }

    /// <summary>
    ///     The dimensions of the loaded image
    /// </summary>
    public Vector2Int Dimensions { get; set; }

    public List<Vector2Int> QuaxPositions { get; set; }
    public Vector2Int CityPosition { get; set; }
    public Texture2D MapTexture { get; set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);

        QuaxPositions = new List<Vector2Int>();
    }
}