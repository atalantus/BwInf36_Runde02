using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsManager : MonoBehaviour
{
    private bool _isOpen;
    private Vector3 _closedPos;
    [SerializeField] private GameObject _toggleIcon;

    private void Awake()
    {
        _closedPos = transform.position;
    }

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
