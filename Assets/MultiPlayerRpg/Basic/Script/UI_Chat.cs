using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class UI_Chat : NetworkBehaviour
{
    Text _txt_Temp;

    [SyncVar (hook = nameof(OnStatusTextChanged))]
    public string _str_status;

    void OnStatusTextChanged(string oldStr, string newStr)
    {
        if(_txt_Temp == null)
            _txt_Temp = transform.Find("Scroll View/Viewport/Content/Txt_Template").GetComponent<Text>();
        if(_txt_Temp != null)
        {
            _txt_Temp.text = _str_status;
        }

    }
}
