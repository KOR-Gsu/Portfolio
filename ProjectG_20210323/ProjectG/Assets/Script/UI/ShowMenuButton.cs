using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowMenuButton : MonoBehaviour
{
    [SerializeField] private VerticalLayoutGroup myContent;
    private bool isMenu;

    private void Start()
    {
        myContent.gameObject.SetActive(false);

        isMenu = false;
    }

    public void Show()
    {
        if (!isMenu)
            myContent.gameObject.SetActive(true);
        else
            myContent.gameObject.SetActive(false);

        isMenu = !isMenu;
    }
}
