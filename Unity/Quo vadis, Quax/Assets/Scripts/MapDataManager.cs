﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDataManager : MonoBehaviour
{
    private static MapDataManager _instance;
    public static MapDataManager Instance
    {
        get { return _instance; }
    }

    public Vector2 Dimensions { get; set; }

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else if (_instance != this)
            Destroy(gameObject);
    }
}
