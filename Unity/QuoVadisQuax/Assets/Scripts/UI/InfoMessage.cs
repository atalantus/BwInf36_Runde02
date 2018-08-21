using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoMessage : MonoBehaviour
{
    [SerializeField] private Text _text;

    public delegate void DestroyingMsgEventHandler(string id);

    public string ID { get; private set; }
    private string _msg;
    public string Msg
    {
        get { return _msg; }
        set { DisplayMsg(value); }
    }
    public event DestroyingMsgEventHandler DestroyingMsg;

    public void Setup(string msg, string id, float lifetime = -1f)
    {
        Msg = msg;
        ID = id;

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
