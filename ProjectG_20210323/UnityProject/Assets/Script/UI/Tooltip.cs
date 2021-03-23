using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    [SerializeField] private Image tooltipImage;
    [SerializeField] private Text nameText;
    [SerializeField] private Text gradeText;
    [SerializeField] private Text infoText;
    [SerializeField] private Text detailText;
    [SerializeField] private Text priceText;

    private Rect myRect;

    public void Start()
    {
        myRect = tooltipImage.GetComponent<RectTransform>().rect;
        gameObject.SetActive(false);
    }

    public void SetPosition(Vector3 pos)
    {
        Vector3 offset = new Vector3(myRect.width * 0.675f, -myRect.height * 0.675f, 0f);
        transform.position = pos + offset;
    }

    public void SetTooltip(Item item)
    {
        nameText.text = string.Format("{0}", item.name);
        infoText.text = string.Format("{0}", item.info);
        gradeText.text = string.Format("  Grade: {0}", item.grade + 1);

        switch (item.itemSort)
        {
            case Define.ItemSort.Consume:
                {
                    ConsumeItemData consumeItem = (ConsumeItemData)Managers.Item.FindItem(Define.ItemSort.Consume, item.id);
                    detailText.text = string.Format("Value: {0} \n Cooldown: {1}", consumeItem.value, consumeItem.cooldown);
                }
                break;
            case Define.ItemSort.Weapon:
                {
                    WeaponItemData weaponItem = (WeaponItemData)Managers.Item.FindItem(Define.ItemSort.Weapon, item.id);
                    detailText.text = string.Format("ATK: {0}", weaponItem.attackDamageValue);
                }
                break;
            case Define.ItemSort.Armor:
                {
                    ArmorItemData armorItem = (ArmorItemData)Managers.Item.FindItem(Define.ItemSort.Armor, item.id);
                    detailText.text = string.Format("DEF: {0}", armorItem.defenseValue);
                }
                break;
        }

        priceText.text = string.Format("Price: {0} ", item.price);
    }
}
