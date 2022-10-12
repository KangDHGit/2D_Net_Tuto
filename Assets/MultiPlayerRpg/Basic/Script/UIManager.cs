using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager I;
    private void Awake()
    {
        I = this;   
    }
}
