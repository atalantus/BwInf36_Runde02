using System;
using Algorithm;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;

#endif

/// <summary>
///     Manages the Options GUI
/// </summary>
public class OptionsManager : MonoBehaviour
{
    #region Properties

    public delegate void StartedAlgorithmEventHandler(Vector2Int quaxPos, Vector2Int cityPos);

    public event StartedAlgorithmEventHandler StartedAlgorithm;

    [SerializeField] private AlgorithmManager _algorithmManager;
    [SerializeField] private Text[] _algorithmResults;

    /// <summary>
    ///     The world position of the option panel when closed
    /// </summary>
    private Vector3 _closedPos;

    [SerializeField] private Text[] _guiCoordinates;
    [SerializeField] private LoadImageManager _loadImage;
    [SerializeField] private Dropdown _quaxPosDropdown;
    [SerializeField] private GameObject _quaxPosMap;
    [SerializeField] private GameObject _quaxPosOverlay;

    private Texture2D _quaxPosOverlayTexture;

    private Vector2Int _selectedQuax;
    [SerializeField] private GameObject _toggleIcon;

    /// <summary>
    ///     Is the options panel currently open
    /// </summary>
    public bool IsOpen { private get; set; }

    public bool ShowNodes { get; set; }

    #endregion

    #region Methods

    private void Awake()
    {
        _closedPos = transform.position;
    }

    private void Start()
    {
        _loadImage.UpdatedLoadingState += OnLoadingState_Changed;
        _algorithmManager.FinishedAlgorithm += (foundPath, flights, time) =>
        {
            UpdateAlgorithmResults(foundPath ? "YES" : "NO", flights.ToString(),
                time.Minutes + "m " + time.Seconds + "s " + time.Milliseconds + "ms");
        };
    }

    private void OnLoadingState_Changed(LoadImageManager.LoadingState state)
    {
        if (state == LoadImageManager.LoadingState.Done) SetUpOptionsGUI();
    }

    private void SetUpOptionsGUI()
    {
        _quaxPosDropdown.ClearOptions();
        for (var i = 0; i < MapDataManager.Instance.QuaxPositions.Count; i++)
            _quaxPosDropdown.options.Add(new Dropdown.OptionData("Quax " + (i + 1)));

        _quaxPosOverlayTexture =
            new Texture2D(MapDataManager.Instance.Dimensions.X, MapDataManager.Instance.Dimensions.Y,
                TextureFormat.ARGB32, false) {filterMode = FilterMode.Point};
        _quaxPosOverlay.GetComponent<RawImage>().texture = _quaxPosOverlayTexture;

        _quaxPosDropdown.value = 0;
        SelectQuaxPos(0);
        _quaxPosDropdown.RefreshShownValue();

        _quaxPosMap.GetComponent<RawImage>().texture = _loadImage.MapTexture;
        var aspectRatio = MapDataManager.Instance.Dimensions.X / (float) MapDataManager.Instance.Dimensions.Y;
        _quaxPosMap.GetComponent<AspectRatioFitter>().aspectRatio = aspectRatio;
        _quaxPosOverlay.GetComponent<AspectRatioFitter>().aspectRatio = aspectRatio;

        _guiCoordinates[2].text = MapDataManager.Instance.CityPosition.X.ToString();
        _guiCoordinates[3].text = MapDataManager.Instance.CityPosition.Y.ToString();

        UpdateAlgorithmResults("-", "-", "-");
    }

    private void UpdateAlgorithmResults(string foundPath, string flights, string time)
    {
        _algorithmResults[0].text = foundPath;
        _algorithmResults[1].text = flights;
        _algorithmResults[2].text = time;
    }

    public void SelectQuaxPos(int index)
    {
        _selectedQuax = MapDataManager.Instance.QuaxPositions[index];
        _guiCoordinates[0].text = _selectedQuax.X.ToString();
        _guiCoordinates[1].text = _selectedQuax.Y.ToString();

        Action markQuax = () =>
        {
            var size = Mathf.Min(MapDataManager.Instance.Dimensions.X, MapDataManager.Instance.Dimensions.Y) / 5;
            if (size % 2 == 0) size++;
            _quaxPosOverlayTexture.DrawSquare(new Vector2Int(_selectedQuax.X - size / 2, _selectedQuax.Y - size / 2),
                size, Color.magenta);
        };

        _quaxPosOverlayTexture.ClearTexture(markQuax);

        UpdateAlgorithmResults("-", "-", "-");
    }

    public void StartAlgorithm()
    {
        if (StartedAlgorithm != null)
        {
            StartedAlgorithm.Invoke(_selectedQuax, MapDataManager.Instance.CityPosition);
            UpdateAlgorithmResults("...", "...", "...");
        }
    }

    /// <summary>
    ///     Toggles the options GUI
    /// </summary>
    public void ToggleGUI()
    {
        var offset = 0;
        if (!IsOpen) offset = 400;
        var target = new Vector3(_closedPos.x + offset, _closedPos.y, _closedPos.z);
        iTween.MoveTo(gameObject, target, .5f);
        iTween.RotateBy(_toggleIcon, new Vector3(0, 0, .5f), .5f);
        IsOpen = !IsOpen;
    }

    /// <summary>
    ///     Quits the game
    /// </summary>
    public void Quit()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    #endregion
}