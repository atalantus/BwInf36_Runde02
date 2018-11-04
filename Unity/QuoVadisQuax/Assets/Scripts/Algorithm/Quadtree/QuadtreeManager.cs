using System.Collections.Generic;
using System.Diagnostics;
using Algorithm.Pathfinding;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Algorithm.Quadtree
{
    public class QuadtreeManager : MonoBehaviour
    {
        #region Properties

        private static QuadtreeManager _instance;

        /// <summary>
        /// The Singleton Instance
        /// </summary>
        public static QuadtreeManager Instance
        {
            get { return _instance; }
        }

        [SerializeField] private OptionsManager _optionsManager;

        public delegate void UpdatedQuadtreeEventHandler(List<MapSquare> updatedMapSquares, Vector2Int sw_point);

        public delegate void CheckedSpecialSquareEventHandler(bool isWalkable, Vector2Int sw_point);

        public delegate void CreatedNodeEventHandler(MapSquare mapSquare);

        public event UpdatedQuadtreeEventHandler UpdatedQuadtree;
        public event CheckedSpecialSquareEventHandler CheckedSpecialSquare;
        public event CreatedNodeEventHandler CreatedNode;

        public NodeElement _rootNode;

        public double QuadtreeTime;

        #endregion

        #region Methods

        private void Awake()
        {
            if (_instance == null)
                _instance = this;
            else if (_instance != this)
                Destroy(gameObject);
        }

        private void Start()
        {
            PathfindingManager.Instance.RequestedMapTile += SearchForPoint;

            PathfindingManager.Instance.RequestSpecialSquare += CheckSpecialSquare;
        }

        public void SetupQuadtree()
        {
            _rootNode = null;
            _rootNode = new Node(new Vector2Int(0, 0), MapDataManager.Instance.Dimensions.x);
            QuadtreeTime = 0;
        }

        private void SearchForPoint(Vector2Int sw_point)
        {
            //Debug.Log("QuadtreeManager - FindPoint " + sw_point);

            // TODO: Performance: don't run whole quadtree on main thread! only GetPixel in GetMatType() in MapSquare.cs
            ThreadQueuer.Instance.QueueMainThreadAction(() =>
            {
                var s = new Stopwatch();
                s.Start();

                List<MapSquare> updatedSquares = new List<MapSquare>();

                for (int x = sw_point.x; x < sw_point.x + 2; x++)
                {
                    for (int y = sw_point.y; y < sw_point.y + 2; y++)
                    {
                        var point = new Vector2Int(x, y);
                        //Debug.Log("Loop point: " + point);
                        bool isChecked = false;

                        foreach (var updatedSquare in updatedSquares)
                        {
                            if (updatedSquare.TouchesPoint(point))
                            {
                                //Debug.Log(point + " already searched");
                                isChecked = true;
                                break;
                            }
                        }

                        if (isChecked) continue;

                        var square = _rootNode.FindPoint(point);
                        //Debug.Log("Added square SW " + square.SW_Point + " NE " + square.NE_Point);
                        updatedSquares.Add(square);
                    }
                }

                s.Stop();
                Debug.LogWarning("Quadtree Search took: " + s.Elapsed.TotalMilliseconds + "ms");
                QuadtreeTime += s.Elapsed.TotalMilliseconds;

                if (UpdatedQuadtree != null)
                    UpdatedQuadtree.Invoke(updatedSquares, sw_point);
            });
        }

        private void CheckSpecialSquare(Vector2Int sw_point)
        {
            ThreadQueuer.Instance.QueueMainThreadAction(() =>
            {
                var isWalkable = true;

                var containsWater = false;
                var containsLand = false;

                List<Color> pixels = new List<Color>();

                for (int x = sw_point.x; x < sw_point.x + 2; x++)
                {
                    for (int y = sw_point.y; y < sw_point.y + 2; y++)
                    {
                        Color pixel;
                        if (MapDataManager.Instance.MapTexture.width > x &&
                            MapDataManager.Instance.MapTexture.height > y)
                            pixel = MapDataManager.Instance.MapTexture.GetPixel(x, y);
                        else
                            pixel = new Color(1, 1, 1, 1);
                        
                        pixels.Add(pixel);
                    }
                }

                var specialSquare = new MapSquare(sw_point, 2) {MapType = MapTypes.GROUND};

                foreach (var pixel in pixels)
                {
                    var pixelType = pixel.GetMapType();

                    if (pixelType == MapTypes.WATER && !containsWater)
                        containsWater = true;
                    else if (pixelType != MapTypes.WATER && !containsLand)
                        containsLand = true;
                    if (containsWater && containsLand)
                        break;
                }

                if (!containsLand && containsWater)
                {
                    specialSquare.MapType = MapTypes.WATER;
                    isWalkable = false;
                }

                RegisterNewNode(specialSquare);

                if (CheckedSpecialSquare != null)
                    CheckedSpecialSquare.Invoke(isWalkable, sw_point);
            });
        }

        public void RegisterNewNode(MapSquare mapSquare)
        {
            if (CreatedNode != null)
                CreatedNode.Invoke(mapSquare);
        }

        #endregion
    }
}