using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using UnityStandardAssets._2D;

public class NetPlayer : NetworkBehaviour
{
    public float _speed = 5.0f;
    Rigidbody2D _rigid;
    SpriteRenderer _spriteR;

    // 캐릭터 머리 위 이름(3D텍스트)
    [SerializeField] TextMesh _txt_Name;

    //동기화 변수(SyncVar) 대괄호[]를 어트리뷰트 라고 한다
    [SyncVar(hook = nameof(OnNameChanged))]
    public string _playerName;
    [SyncVar(hook = nameof(OnColorChanged))]
    public Color _playerColor;

    void OnNameChanged(string oldName, string newName)
    {
        if (_txt_Name == null)
            _txt_Name = transform.Find("Info/Txt_Name").GetComponent<TextMesh>();
        _txt_Name.text = newName;
        Debug.Log("OnNameChanged " + _playerName);
    }
    void OnColorChanged(Color oldColor, Color newColor)
    {
        if (_spriteR == null)
            _spriteR = GetComponent<SpriteRenderer>();
        _spriteR.color = newColor;
    }
    [Command]
    void CmdSetUpPlayer(string name, Color color)
    {
        Debug.Log("CmdSetUpPlayer " + _playerName);
        _playerName = name;
        _playerColor = color;

        // Canvas 아래 UI_Chat에 있는 동기화 변수, _str_status에 메세지를 넣기
        UIManager.I._UI_Chat._str_status = "플레이어 " + _playerName + " 님이 입장하셨습니다.";
    }
    [Command]
    public void CmdSendChatMessage(string msg)
    {
        Debug.Log("서버가 챗 메세지를 받았습니다" + msg);
        UIManager.I._UI_Chat._str_status = msg;
    }    

    private void Start()
    {
        if (!TryGetComponent(out _rigid))
            Debug.LogError("_rigid is Null");
        if (!transform.Find("Info/Txt_Name").TryGetComponent(out _txt_Name))
            Debug.LogError("_txt_Name is Null");
        if (!TryGetComponent(out _spriteR))
            Debug.LogError("_spriteR is Null");
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        UIManager.I._UI_Chat._player = this;

        // 카메라 팔로우 스크립트를 붙이기
        NetCamera2DFollow camFollow = Camera.main.gameObject.AddComponent<NetCamera2DFollow>();
        camFollow.target = transform;

        // 로컬플레이어 생성시점에 랜덤으로 이름 생성
        //string name = "Player" + Random.Range(100, 999);

        InputField inputName = UIManager.I.transform.Find("InputName").GetComponent<InputField>();
        
        // 서버(호스트)에 생성된 이름을 알리기, 커맨드함수 호출
        CmdSetUpPlayer(inputName.text, UIManager.I.GetColor());
    }
    public override void OnStopLocalPlayer()
    {
        NetCamera2DFollow camFollow = Camera.main.gameObject.GetComponent<NetCamera2DFollow>();
        if (camFollow != null)
        {
            if (camFollow.target.Equals(transform))
            {
                Destroy(camFollow);
            }
        }
    }

    private void Update()
    {
        if (isLocalPlayer)
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            Vector2 dir = new Vector2(h, v);
            _rigid.velocity = dir * Time.fixedDeltaTime * _speed * 100.0f;
        }
    }
}
