using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetMob : NetworkBehaviour
{
    SpriteRenderer _spRen;

    Vector3 _startPos;
    Color _orignColor;
    private void Start()
    {
        if(TryGetComponent(out _spRen))
        {
            _spRen.sortingOrder = 3;
            _orignColor = _spRen.color;
        }
        _startPos = transform.position;
    }

    private void Update()
    {
        if(isServer)
        {
            Vector3 pos = transform.position;

            pos.x = _startPos.x + Mathf.PingPong(Time.time, 2.0f);

            transform.position = pos;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isServer)
        {
            if (collision.gameObject.TryGetComponent(out NetPlayer player))
                Debug.Log(player._playerName);

            Rpc_DoDamageEffect();
        }
    }

    [ClientRpc]// 서버에서 클라이언트로 뿌리는 어트리뷰트
    void Rpc_DoDamageEffect()
    {
        StartCoroutine(ChangeColor());
    }

    IEnumerator ChangeColor()
    {
        int count = 0;
        while(count < 5)
        {
            _spRen.color = Color.red;
            yield return new WaitForSecondsRealtime(0.1f);

            _spRen.color = _orignColor;
            count++;
            yield return new WaitForSecondsRealtime(0.1f);
        }
        yield return null;
    }
}
