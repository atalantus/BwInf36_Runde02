using UnityEngine;
using UnityEngine.UI;

/// <summary>
///     Wrapper class for info massages
/// </summary>
public class InfoMessage : MonoBehaviour
{
    #region Properties

    public delegate void DestroyingMsgEventHandler(string id);

    public event DestroyingMsgEventHandler DestroyingMsg;

    [SerializeField] private GameObject _spinner;
    [SerializeField] private Text _text;

    public string Id { get; private set; }

    public string Msg
    {
        set { DisplayMsg(value); }
    }

    #endregion

    #region Methods

    public void Setup(string msg, string id, bool spinnerIcon = false, float lifetime = -1f)
    {
        Msg = msg;
        Id = id;

        _spinner.SetActive(spinnerIcon);

        if (lifetime != -1f)
            Destroy(gameObject, lifetime);
    }

    private void OnDestroy()
    {
        if (DestroyingMsg != null) DestroyingMsg.Invoke(Id);
    }

    private void DisplayMsg(string msg)
    {
        _text.text = msg;
    }

    #endregion
}