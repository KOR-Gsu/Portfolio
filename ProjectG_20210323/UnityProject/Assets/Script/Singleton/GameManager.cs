using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Linq;

public class GameManager
{
    public bool isGameOver { get; private set; }
    public LogData logData { get; private set; }

    public Action OnSpawnEvent;

    private void BackToTown() 
    { 
        SceneManager.LoadScene("TownScene");

        isGameOver = false;
    }

    public void Init()
    {
        isGameOver = false;

        logData = Managers.Data.currentLog;
        /*
#if UNITY_EDITOR
        logData = new LogData("test", "123");
#endif*/
    }

    public void RestartGame()
    {
        isGameOver = true;

        SavePlayerData();
        BackToTown();
    }

    public void SavePlayerData()
    {
        PlayerData savingPlayerData = CreatePlayerData();
        PlayerDataJson playerDataJson = Managers.Data.JsonToData<PlayerDataJson>(nameof(Define.FileName.Player_Saved_Data));

        if (playerDataJson.playerDataDictionary.ContainsKey(logData.id))
        {
            foreach (var key in playerDataJson.playerDataDictionary.Keys.ToList())
            {
                if (key == logData.id)
                {
                    playerDataJson.playerDataDictionary[key] = savingPlayerData;
                    break;
                }
            }
        }
        else
            playerDataJson.Add(savingPlayerData);

        Managers.Data.DataToJson(nameof(Define.FileName.Player_Saved_Data), playerDataJson);
    }

    private PlayerData CreatePlayerData()
    {
        PlayerStat playerStat = GameObject.Find("Player").GetComponent<PlayerStat>();
        PlayerData playerData = new PlayerData()
        {
            log = logData
        };

        playerData.dataDictionary.Add("Level", playerStat.level);
        playerData.dataDictionary.Add("currentHP", playerStat.currentHP);
        playerData.dataDictionary.Add("currentMP", playerStat.currentMP);
        playerData.dataDictionary.Add("currentEXP", playerStat.currentEXP);
        playerData.dataDictionary.Add("maxHP", playerStat.maxHP);
        playerData.dataDictionary.Add("maxMP", playerStat.maxMP);
        playerData.dataDictionary.Add("maxEXP", playerStat.maxEXP);
        playerData.dataDictionary.Add("attackDamage", playerStat.attackDamage);
        playerData.dataDictionary.Add("defense", playerStat.defense);
        playerData.dataDictionary.Add("attackRange", playerStat.attackRange);
        playerData.dataDictionary.Add("moveSpeed", playerStat.moveSpeed);
        playerData.dataDictionary.Add("rotateSpeed", playerStat.rotateSpeed);
        playerData.dataDictionary.Add("intvlAttackTime", playerStat.intvlAttackTime);
        playerData.dataDictionary.Add("gold", playerStat.gold);

        if (playerStat.info.weaponSlot.item == null)
            playerData.dataDictionary.Add("equipedWeapon", -1);
        else
            playerData.dataDictionary.Add("equipedWeapon", playerStat.info.weaponSlot.item.id);

        if (playerStat.info.armorSlot.item == null)
            playerData.dataDictionary.Add("equipedArmor", -1);
        else
            playerData.dataDictionary.Add("equipedArmor", playerStat.info.armorSlot.item.id);

        SlotData slotData1 = null;
        if (playerStat.inventory.itemSlot1.item != null)
        {
            slotData1 = new SlotData()
            {
                sort = (int)playerStat.inventory.itemSlot1.item.itemSort,
                id = playerStat.inventory.itemSlot1.item.id,
                count = playerStat.inventory.itemSlot1.itemCount
            };
        }
        playerData.invenDataDictionary.Add("itemSlot1", slotData1);

        SlotData slotData2 = null;
        if (playerStat.inventory.itemSlot2.item != null)
        {
            slotData2 = new SlotData()
            {
                sort = (int)playerStat.inventory.itemSlot2.item.itemSort,
                id = playerStat.inventory.itemSlot2.item.id,
                count = playerStat.inventory.itemSlot2.itemCount
            };
        }
        playerData.invenDataDictionary.Add("itemSlot2", slotData2);

        for (int i = 0; i < playerStat.inventory.slots.Length; i++)
        {
            SlotData invenData = null;

            if (playerStat.inventory.slots[i].item != null)
            {
                invenData = new SlotData()
                {
                    sort = (int)playerStat.inventory.slots[i].item.itemSort,
                    id = playerStat.inventory.slots[i].item.id,
                    count = playerStat.inventory.slots[i].itemCount
                };
            }

            playerData.invenDataDictionary.Add(string.Format("inven{0:D2}", i), invenData);
        }

        return playerData;
    }

    public PlayerData LoadPlayerStat()
    {
        PlayerDataJson playerDataJson = Managers.Data.JsonToData<PlayerDataJson>(nameof(Define.FileName.Player_Saved_Data));

        if (!playerDataJson.playerDataDictionary.ContainsKey(logData.id))
            CreateNewPlayerData(playerDataJson);

        playerDataJson.playerDataDictionary.TryGetValue(logData.id, out PlayerData playerData);

        return playerData;
    }

    private void CreateNewPlayerData(PlayerDataJson playerDataJson)
    {
        PlayerData newPlayerData = Managers.Data.JsonToData<PlayerData>(nameof(Define.FileName.Player_Default_Data));

        newPlayerData.log = logData;
        playerDataJson.Add(newPlayerData);

        Managers.Data.DataToJson(nameof(Define.FileName.Player_Saved_Data), playerDataJson);
    }
}