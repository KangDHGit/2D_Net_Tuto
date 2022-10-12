using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager I;
    [SerializeField] Slider _silder_R;
    [SerializeField] Slider _silder_G;
    [SerializeField] Slider _silder_B;
    [SerializeField] Image _img_Color;
    private void Awake()
    {
        I = this;   
    }

    private void Start()
    {
        _silder_R = transform.Find("InputColor/Slider_R").GetComponent<Slider>();
        _silder_G = transform.Find("InputColor/Slider_G").GetComponent<Slider>();
        _silder_B = transform.Find("InputColor/Slider_B").GetComponent<Slider>();
        _img_Color = transform.Find("InputColor/Img_Color").GetComponent<Image>();
    }

    public Color GetColor()
    {
        return new Color(_silder_R.value, _silder_G.value, _silder_B.value);
    }
    public void SetColor()
    {
        _img_Color.color = new Color(_silder_R.value, _silder_G.value, _silder_B.value);
    }
}
