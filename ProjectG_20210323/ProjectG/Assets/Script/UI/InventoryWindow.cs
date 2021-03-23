using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class InventoryWindow : Window
{
    private PlayerStat playerStat;
    private int curItemCount = 0;
    private int maxItemCount = 24;
    public Slot itemSlot1 { get; private set; }
    public Slot itemSlot2 { get; private set; }
    public Slot[] slots { get; private set; }

    [SerializeField] private Text goldText;
    [SerializeField] private GridLayoutGroup slotGridLayoutGroup;

    void Awake()
    {
        slots = slotGridLayoutGroup.GetComponentsInChildren<Slot>();
        for (int i = 0; i < slots.Length; i++)
            slots[i].slotTpye = Define.SlotTpye.Inventory;

        itemSlot1 = UIManager.instance.quickSlotList[0].GetComponent<Slot>();
        itemSlot1.slotTpye = Define.SlotTpye.Inventory;
        itemSlot2 = UIManager.instance.quickSlotList[1].GetComponent<Slot>();
        itemSlot2.slotTpye = Define.SlotTpye.Inventory;

        gameObject.SetActive(false);
    }

    private void Start()
    {
        playerStat = GameObject.Find("Player").GetComponent<PlayerStat>();
    }

    void Update()
    {
        goldText.text = string.Format("{0:N0}", playerStat.gold);
    }

    public void Initializing(PlayerData data)
    {
        data.invenDataDictionary.TryGetValue("itemSlot1", out SlotData _itemSlot1);
        data.invenDataDictionary.TryGetValue("itemSlot2", out SlotData _itemSlot2);
        if (_itemSlot1 != null)
            itemSlot1.AddItem(Managers.Item.FindItem((Define.ItemSort)_itemSlot1.sort, _itemSlot1.id), _itemSlot1.count);
        if (_itemSlot2 != null)
            itemSlot2.AddItem(Managers.Item.FindItem((Define.ItemSort)_itemSlot2.sort, _itemSlot2.id), _itemSlot2.count);

        for (int i = 0; i < slots.Length; i++)
        {
            data.invenDataDictionary.TryGetValue(string.Format("inven{0:D2}", i), out SlotData _invenSlot);

            if (_invenSlot != null)
                AcquireItem(Managers.Item.FindItem((Define.ItemSort)_invenSlot.sort, _invenSlot.id), _invenSlot.count);
        }
    }

    public void AcquireItem(Item newItem, int count = 1)
    {
        if (newItem.itemSort == Define.ItemSort.Consume)
        {
            if(itemSlot1.item.name == newItem.name)
            {
                itemSlot1.UpdateItemCount(count);
                return;
            }
            if(itemSlot2.item.name == newItem.name)
            {
                itemSlot2.UpdateItemCount(count);
                return;
            }

            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item != null)
                {
                    if (slots[i].item.name == newItem.name)
                    {
                        slots[i].UpdateItemCount(count);
                        return;
                    }
                }
            }
        }
        if (curItemCount < maxItemCount)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item == null)
                {
                    slots[i].AddItem(newItem, count);
                    curItemCount += 1;
                    return;
                }
            }
        }
    }

    public void Sort()
    {
        for(int i = 0; slots[i].item != null ;i++)
        {
            for(int j = i+1; slots[j].item != null; j++)
            {
                if(slots[i].item.itemSort > slots[j].item.itemSort)
                {
                    Item tempItem = slots[i].item;
                    int tempCount = slots[i].itemCount;

                    slots[i].AddItem(slots[j].item, slots[j].itemCount);
                    slots[j].AddItem(tempItem, tempCount);
                }
                else if(slots[i].item.itemSort == slots[j].item.itemSort)
                {
                    if (slots[i].item.id > slots[j].item.id)
                    {
                        Item tempItem = slots[i].item;
                        int tempCount = slots[i].itemCount;

                        slots[i].AddItem(slots[j].item, slots[j].itemCount);
                        slots[j].AddItem(tempItem, tempCount);
                    }
                }
            }
        }
    }

    public void UpdateItemCount(int count)
    {
        curItemCount += count;
    }

    public override void ShowWindow()
    {
        base.ShowWindow();
    }

    public override void CloseWindow()
    {
        base.CloseWindow();
    }
}
