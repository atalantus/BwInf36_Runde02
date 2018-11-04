using System.Collections.Generic;
using System.Diagnostics;
using Algorithm.Pathfinding;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Algorithm.Quadtree
{
    /// <inheritdoc />
    /// <summary>
    ///     General Quadtree Manager
    /// </summary>
    public class QuadtreeManager : MonoBehaviour
    {
        #region Properties

        /// <summary>
        ///     The Singleton Instance
        /// </summary>
        public static QuadtreeManager Instance { get; private set; }

        /// <summary>
        ///     Updated Quadtree delegate
        /// </summary>
        /// <param name="updatedMapSquares">List of the updated <see cref="MapSquare" />s</param>
        /// <param name="point">The point that was checked for</param>
        public delegate void UpdatedQuadtreeEventHandler(List<MapSquare> updatedMapSquares, Vector2Int point);

        /// <summary>
        ///     Checked Special Square delegate
        /// </summary>
        /// <param name="isWalkable">Is the special square walkable</param>
        /// <param name="sw_point">The South-West (Bottom-Left) point of the square</param>
        public delegate void CheckedSpecialSquareEventHandler(bool isWalkable, Vector2Int sw_point);

        /// <summary>
        ///     Created Node delegate
        /// </summary>
        /// <param name="mapSquare">The <see cref="MapSquare" /> that was created</param>
        public delegate void CreatedNodeEventHandler(MapSquare mapSquare);

        /// <summary>
        ///     UpdatedQuadtree event
        /// </summary>
        public event UpdatedQuadtreeEventHandler UpdatedQuadtree;

        /// <summary>
        ///     CheckedSpecialSquare event
        /// </summary>
        public event CheckedSpecialSquareEventHandler CheckedSpecialSquare;

        /// <summary>
        ///     CreatedNode event
        /// </summary>
        public event CreatedNodeEventHandler CreatedNode;

        /// <summary>
        ///     The root of the Quadtree
        /// </summary>
        public Node RootNode;

        /// <summary>
        ///     The execution time of the quadtree methods
        /// </summary>
        public double QuadtreeTime;

        #endregion

        #region Methods

        /// <summary>
        ///     Unity Awake
        /// </summary>
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else if (Instance != this)
                Destroy(gameObject);
        }

        /// <summary>
        ///     Unity Start
        /// </summary>
        private void Start()
        {
            /**
             * Subscribe to events
             */
            PathfindingManager.Instance.RequestedMapTile += SearchForPoint;

            PathfindingManager.Instance.RequestSpecialSquare += CheckSpecialSquare;
        }

        /// <summary>
        ///     Setup Quadtree
        /// </summary>
        public void SetupQuadtree()
        {
            RootNode = null;
            RootNode = new Node(new Vector2Int(0, 0), MapDataManager.Instance.Dimensions.x);
            QuadtreeTime = 0;
        }

        /// <summary>
        ///     Search Quadtree for 2x2 square
        /// </summary>
        /// <param name="sw_point">The South-West (Bottom-Left) point of the square</param>
        private void SearchForPoint(Vector2Int sw_point)
        {
            // Execute from main thread
            ThreadQueuer.Instance.QueueMainThreadAction(() =>
            {
                var s = new Stopwatch();
                s.Start();

                var updatedSquares = new List<MapSquare>();

                for (var x = sw_point.x; x < sw_point.x + 2; x++)
                for (var y = sw_point.y; y < sw_point.y + 2; y++)
                {
                    var point = new Vector2Int(x, y);
                    var isChecked = false;

                    foreach (var updatedSquare in updatedSquares)
                        if (updatedSquare.ContainsPoint(point))
                        {
                            isChecked = true;
                            break;
                        }

                    if (isChecked) continue;

                    var square = RootNode.FindPoint(point);

                    updatedSquares.Add(square);
                }

                s.Stop();
                Debug.LogWarning("Quadtree Search took: " + s.Elapsed.TotalMilliseconds + "ms");
                QuadtreeTime += s.Elapsed.TotalMilliseconds;

                if (UpdatedQuadtree != null)
                    UpdatedQuadtree.Invoke(updatedSquares, sw_point);
            });
        }

        /// <summary>
        ///     Checks a 2x2 <see cref="MapSquare" /> outside of the Quadtree
        /// </summary>
        /// <param name="sw_point">The South-West (Bottom-Left) point of the square</param>
        private void CheckSpecialSquare(Vector2Int sw_point)
        {
            // Execute from main thread
            ThreadQueuer.Instance.QueueMainThreadAction(() =>
            {
                var isWalkable = true;

                var containsWater = false;
                var containsLand = false;

                var pixels = new List<Color>();

                for (var x = sw_point.x; x < sw_point.x + 2; x++)
                for (var y = sw_point.y; y < sw_point.y + 2; y++)
                {
                    Color pixel;
                    if (MapDataManager.Instance.MapTexture.width > x &&
                        MapDataManager.Instance.MapTexture.height > y)
                        pixel = MapDataManager.Instance.MapTexture.GetPixel(x, y);
                    else
                        pixel = new Color(1, 1, 1, 1);

                    pixels.Add(pixel);
                }

                var specialSquare = new MapSquare(sw_point, 2) {MapType = MapTypes.Ground};

                foreach (var pixel in pixels)
                {
                    var pixelType = pixel.GetMapType();

                    if (pixelType == MapTypes.Water && !containsWater)
                        containsWater = true;
                    else if (pixelType != MapTypes.Water && !containsLand)
                        containsLand = true;
                    if (containsWater && containsLand)
                        break;
                }

                if (!containsLand && containsWater)
                {
                    specialSquare.MapType = MapTypes.Water;
                    isWalkable = false;
                }

                RegisterNewNode(specialSquare);

                if (CheckedSpecialSquare != null)
                    CheckedSpecialSquare.Invoke(isWalkable, sw_point);
            });
        }

        /// <summary>
        ///     Invokes the <see cref="CreatedNode" /> event
        /// </summary>
        /// <param name="mapSquare">The <see cref="CreatedNode" />'s parameter</param>
        public void RegisterNewNode(MapSquare mapSquare)
        {
            if (CreatedNode != null)
                CreatedNode.Invoke(mapSquare);
        }

        #endregion
    }
}