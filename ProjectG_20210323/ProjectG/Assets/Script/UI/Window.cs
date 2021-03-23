using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : MonoBehaviour
{
    [SerializeField] private Define.WindowTpye windowTpye;

    public bool isOpen { get; private set; }

    private void Awake()
    {
        isOpen = false;

        gameObject.SetActive(false);
    }

    virtual public void ShowWindow()
    {
        isOpen = true;

        gameObject.SetActive(true);
    }

    virtual public void CloseWindow()
    {
        isOpen = false;

        gameObject.SetActive(false);
    }
}