using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] private Define.ItemSort itemSort;

    public void OpenShop()
    {
        UIManager.instance.OpenWindow((int)Define.InGameWindowType.ShopWindow);
        UIManager.instance.gameWindowList[(int)Define.InGameWindowType.ShopWindow].GetComponent<ShopWindow>().SetItem(itemSort);

        UIManager.instance.OpenWindow((int)Define.InGameWindowType.InventoryWindow);
    }
}
