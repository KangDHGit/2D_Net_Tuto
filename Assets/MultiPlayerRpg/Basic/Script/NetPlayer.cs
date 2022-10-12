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

    // 캐릭터 머리 위 이름(3D텍스트)
    [SerializeField] TextMesh _txt_Name;

    //동기화 변수(SyncVar) 대괄호[]를 어트리뷰트 라고 한다
    [SyncVar(hook = nameof(OnNameChanged))]
    public string _playerName;

    void OnNameChanged(string oldName, string newName)
    {
        if (_txt_Name == null)
            _txt_Name = transform.Find("Info/Txt_Name").GetComponent<TextMesh>();
        _txt_Name.text = newName;
        Debug.Log("OnNameChanged " + _playerName);
    }
    [Command]
    void CmdSetUpPlayer(string name)
    {
        Debug.Log("CmdSetUpPlayer " + _playerName);
        _playerName = name;
    }

    private void Start()
    {
        if (!TryGetComponent(out _rigid))
            Debug.LogError("_rigid is Null");
        if (!transform.Find("Info/Txt_Name").TryGetComponent(out _txt_Name))
            Debug.LogError("_txt_Name is Null");
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        // 카메라 팔로우 스크립트를 붙이기
        NetCamera2DFollow camFollow = Camera.main.gameObject.AddComponent<NetCamera2DFollow>();
        camFollow.target = transform;

        // 로컬플레이어 생성시점에 랜덤으로 이름 생성
        //string name = "Player" + Random.Range(100, 999);

        InputField inputName = UIManager.I.transform.Find("InputName").GetComponent<InputField>();
        
        // 서버(호스트)에 생성된 이름을 알리기, 커맨드함수 호출
        CmdSetUpPlayer(inputName.text);
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
