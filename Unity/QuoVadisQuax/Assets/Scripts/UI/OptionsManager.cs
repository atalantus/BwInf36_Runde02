using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the Options GUI
/// </summary>
public class OptionsManager : MonoBehaviour
{
    /// <summary>
    /// Is the options panel currently open
    /// </summary>
    public bool IsOpen { get; set; }
    /// <summary>
    /// The world position of the option panel when closed
    /// </summary>
    private Vector3 _closedPos;

    [SerializeField] private LoadImageManager _loadImage;
    [SerializeField] private Dropdown _quaxPosDropdown;
    [SerializeField] private GameObject _quaxPosMap;
    [SerializeField] private GameObject _quaxPosOverlay;
    [SerializeField] private Text[] _quaxPosCoordinates;
    [SerializeField] private GameObject _toggleIcon;

    private void Awake()
    {
        _closedPos = transform.position;
    }

    private void Start()
    {
        _loadImage.UpdatedLoadingState += OnLoadingState_Changed;
    }

    private void OnLoadingState_Changed(LoadImageManager.LoadingState state)
    {
        if (state == LoadImageManager.LoadingState.DONE)
        {
            SetUpOptionsGUI();
        }
    }

    private void SetUpOptionsGUI()
    {
        _quaxPosDropdown.ClearOptions();
        for (var i = 0; i < MapDataManager.Instance.QuaxPositions.Count; i++)
        {
            _quaxPosDropdown.options.Add(new Dropdown.OptionData("Quax " + (i + 1)));
        }

        _quaxPosDropdown.value = 0;
        SelectQuaxPos(0);
        _quaxPosDropdown.RefreshShownValue();

        _quaxPosMap.GetComponent<RawImage>().texture = _loadImage.MapTexture;
        var aspectRatio = MapDataManager.Instance.Dimensions.x / MapDataManager.Instance.Dimensions.y;
        _quaxPosMap.GetComponent<AspectRatioFitter>().aspectRatio = aspectRatio;
    }

    public void SelectQuaxPos(int index)
    {
        var pos = MapDataManager.Instance.QuaxPositions[index];
        _quaxPosCoordinates[0].text = pos.x.ToString();
        _quaxPosCoordinates[1].text = (MapDataManager.Instance.Dimensions.y - pos.y).ToString();
    }

    /// <summary>
    /// Toggles the options GUI
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
    /// Quits the game
    /// </summary>
    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
