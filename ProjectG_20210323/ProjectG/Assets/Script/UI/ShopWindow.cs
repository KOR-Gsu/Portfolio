using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopWindow : Window
{
    private Slot[] slots;

    [SerializeField] private GridLayoutGroup slotGridLayoutGroup;

    private void Awake()
    {
        slots = slotGridLayoutGroup.GetComponentsInChildren<Slot>();
        for (int i = 0; i < slots.Length; i++)
            slots[i].slotTpye = Define.SlotTpye.Shop;

        gameObject.SetActive(false);
    }

    public void SetItem(Define.ItemSort sort)
    {
        switch (sort)
        {
            case Define.ItemSort.Consume:
                {
                    for (int i = 0; i < Managers.Item.consumeItemDataList.Count; i++)
                    {
                        Item newItem = Managers.Item.consumeItemDataList[i];

                        slots[i].AddItem(newItem);
                    }
                }
                break;
            case Define.ItemSort.Weapon:
                {
                    for (int i = 0; i < Managers.Item.weaponItemDataList.Count; i++)
                    {
                        Item newItem = Managers.Item.weaponItemDataList[i];

                        slots[i].AddItem(newItem);
                    }
                }
                break;
            case Define.ItemSort.Armor:
                {
                    for (int i = 0; i < Managers.Item.armorItemDataList.Count; i++)
                    {
                        Item newItem = Managers.Item.armorItemDataList[i];

                        slots[i].AddItem(newItem);
                    }
                }
                break;
        }
    }

    public override void ShowWindow()
    {
        base.ShowWindow();
    }

    public override void CloseWindow()
    {
        for(int i = 0; i< slots.Length;i++)
            slots[i].RemoveItem();

        base.CloseWindow();
    }
}
