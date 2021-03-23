using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoWindow : Window
{
    private PlayerStat playerStat;

    public Slot weaponSlot;
    public Slot armorSlot;
    public Text[] stats;

    private void Start()
    {
        playerStat = GameObject.Find("Player").GetComponent<PlayerStat>();

        weaponSlot.slotTpye = Define.SlotTpye.Info;
        armorSlot.slotTpye = Define.SlotTpye.Info;
    }

    void Update()
     {
        stats[(int)Define.Stat.LV].text = string.Format("{0:G0}", playerStat.level);
        stats[(int)Define.Stat.HP].text = string.Format("{0:F0} / {1:G0}", playerStat.currentHP, playerStat.maxHP);
        stats[(int)Define.Stat.MP].text = string.Format("{0:F0} / {1:G0}", playerStat.currentMP, playerStat.maxMP);
        stats[(int)Define.Stat.EXP].text = string.Format("{0:G0} / {1:G0}", playerStat.currentEXP, playerStat.maxEXP);
        stats[(int)Define.Stat.ATK].text = string.Format("{0:G0}", playerStat.finalAttackDamage);
        stats[(int)Define.Stat.DEF].text = string.Format("{0:G0}", playerStat.finalDefense);
        stats[(int)Define.Stat.RNG].text = string.Format("{0:F1}", playerStat.finalAttackRange);
        stats[(int)Define.Stat.SPD].text = string.Format("{0:F1}", playerStat.moveSpeed);
        stats[(int)Define.Stat.APS].text = string.Format("{0:F1}", playerStat.finalIntvlAttackTime);
    }

    public void Initializing(PlayerData data)
    {
        data.dataDictionary.TryGetValue("equipedWeapon", out float _weapon);
        data.dataDictionary.TryGetValue("equipedArmor", out float _armor);
        if (_weapon != -1)
            weaponSlot.AddItem(Managers.Item.FindItem(Define.ItemSort.Weapon, (int)_weapon));
        if (_armor != -1)
            armorSlot.AddItem(Managers.Item.FindItem(Define.ItemSort.Armor, (int)_armor));
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
