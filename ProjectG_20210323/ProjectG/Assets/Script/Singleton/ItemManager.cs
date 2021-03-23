using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager
{
    public List<ConsumeItemData> consumeItemDataList { get; private set; } = new List<ConsumeItemData>();
    public List<WeaponItemData> weaponItemDataList { get; private set; } = new List<WeaponItemData>();
    public List<ArmorItemData> armorItemDataList { get; private set; } = new List<ArmorItemData>();

    public void Init()
    {
        ItemDataJson itemDataJson = Managers.Data.JsonToData<ItemDataJson>(nameof(Define.FileName.Item_Data));

        for(int i = 0; i < itemDataJson.consumeItemDataDictionary.Keys.Count; i++)
            consumeItemDataList.Add(itemDataJson.consumeItemDataDictionary[i]);

        for (int i = 0; i < itemDataJson.weaponItemDataDictionary.Keys.Count; i++)
            weaponItemDataList.Add(itemDataJson.weaponItemDataDictionary[i]);

        for (int i = 0; i < itemDataJson.armorItemDataDictionary.Keys.Count; i++)
            armorItemDataList.Add(itemDataJson.armorItemDataDictionary[i]);
    }

    public Item FindItem(Define.ItemSort sort, int id)
    {
        switch (sort)
        {
            case Define.ItemSort.Consume:
                return consumeItemDataList[id];
            case Define.ItemSort.Weapon:
                return weaponItemDataList[id];
            case Define.ItemSort.Armor:
                return armorItemDataList[id];
            default:
                return null;
        }
    }
}
