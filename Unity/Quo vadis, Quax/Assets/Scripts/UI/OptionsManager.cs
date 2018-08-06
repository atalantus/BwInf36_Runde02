using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the Options GUI
/// </summary>
public class OptionsManager : MonoBehaviour
{
    /// <summary>
    /// Is the options panel currently open
    /// </summary>
    private bool _isOpen;
    /// <summary>
    /// The world position of the option panel when closed
    /// </summary>
    private Vector3 _closedPos;
    [SerializeField] private GameObject _toggleIcon;

    private void Awake()
    {
        _closedPos = transform.position;
    }

    /// <summary>
    /// Toggles the options GUI
    /// </summary>
    public void ToggleGUI()
    {
        var offset = 0;
        if (!_isOpen) offset = 400;
        var target = new Vector3(_closedPos.x + offset, _closedPos.y, _closedPos.z);
        iTween.MoveTo(gameObject, target, .5f);
        iTween.RotateBy(_toggleIcon, new Vector3(0, 0, .5f), .5f);
        _isOpen = !_isOpen;
    }
}
