using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityStandardAssets._2D;

public class NetPlayer : NetworkBehaviour
{
    public float _speed = 5.0f;
    Rigidbody2D _rigid;

    private void Start()
    {
        if (!TryGetComponent(out _rigid))
            Debug.LogError("_rigid is Null");
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        // 카메라 팔로우 스크립트를 붙이기
        NetCamera2DFollow camFollow = Camera.main.gameObject.AddComponent<NetCamera2DFollow>();
        camFollow.target = transform;
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
