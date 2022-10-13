using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class UI_Chat : NetworkBehaviour
{
    Text _txt_Temp;
    public InputField _inputChat;
    public NetPlayer _player;

    [SyncVar (hook = nameof(OnStatusTextChanged))]
    public string _str_status;

    private void Awake()
    {
        _inputChat = transform.Find("InputChat").GetComponent<InputField>();
    }
    void OnStatusTextChanged(string oldStr, string newStr)
    {
        if(_txt_Temp == null)
            _txt_Temp = transform.Find("Scroll View/Viewport/Content/Txt_Template").GetComponent<Text>();
        if(_txt_Temp != null)
        {
            _txt_Temp.text = _str_status;
        }
    }

    // inputField의 이벤트 함수(submit, 즉 입력후 엔터 눌렀을때)
    public void OnChatSubmit()
    {
        Debug.Log("OnChatSubmit !!" + _inputChat.text);
        // 커맨드 함수를 호출하여서 서버에서 동기화 변수인 _str_status 수정
        _player.CmdSendChatMessage(_inputChat.text);
    }
}
