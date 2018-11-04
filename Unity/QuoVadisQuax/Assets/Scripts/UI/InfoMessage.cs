using UnityEngine;
using UnityEngine.UI;

public class InfoMessage : MonoBehaviour
{
    public delegate void DestroyingMsgEventHandler(string id);

    private string _msg;
    [SerializeField] private GameObject _spinner;
    [SerializeField] private Text _text;

    public string ID { get; private set; }

    public string Msg
    {
        get { return _msg; }
        set { DisplayMsg(value); }
    }

    public event DestroyingMsgEventHandler DestroyingMsg;

    public void Setup(string msg, string id, bool spinnerIcon = false, float lifetime = -1f)
    {
        Msg = msg;
        ID = id;

        _spinner.SetActive(spinnerIcon);

        if (lifetime != -1f)
            Destroy(gameObject, lifetime);
    }

    private void OnDestroy()
    {
        if (DestroyingMsg != null) DestroyingMsg.Invoke(ID);
    }

    private void DisplayMsg(string msg)
    {
        _msg = msg;
        _text.text = msg;
    }
}