using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LogData
{
    public string id;
    public string pw;

    public LogData(string _id, string _pw)
    {
        id = _id;
        pw = _pw;
    }
}

public class SlotData
{
    public int sort;
    public int id;
    public int count;
}

[System.Serializable]
public class PlayerData
{
    public LogData log;
    public Dictionary<string, float> dataDictionary = new Dictionary<string, float>();
    public Dictionary<string, SlotData> invenDataDictionary = new Dictionary<string, SlotData>();
}

[System.Serializable]
public class LogDataJson
{
    public Dictionary<string, LogData> logDataDictionary = new Dictionary<string, LogData>();

    public void Add(LogData data)
    {
        logDataDictionary.Add(data.id, data);
    }

    public bool IsData(string id)
    {
        return logDataDictionary.ContainsKey(id);
    }

    public LogData FindLogData(string id)
    {
        logDataDictionary.TryGetValue(id, out LogData logData);

        return logData;
    }
}

[System.Serializable]
public class PlayerDataJson
{
    public Dictionary<string, PlayerData> playerDataDictionary = new Dictionary<string, PlayerData>();

    public void Add(PlayerData data)
    {
        playerDataDictionary.Add(data.log.id, data);
    }

    public bool IsData(string id)
    {
        return playerDataDictionary.ContainsKey(id);
    }

    public PlayerData FindPlayerData(string id)
    {
        playerDataDictionary.TryGetValue(id, out PlayerData playerData);

        return playerData;
    }
}

[System.Serializable]
public class MonsterData
{
    public float maxHP;
    public float attackDamage;
    public float defense;
    public float scanRange;
    public float attackRange;
    public float moveSpeed;
    public float rotateSpeed;
    public float intvlAttackTime;
    public float exp;
    public float gold;
}

[System.Serializable]
public class ItemDataJson
{
    public Dictionary<int, ConsumeItemData> consumeItemDataDictionary = new Dictionary<int, ConsumeItemData>();
    public Dictionary<int, WeaponItemData> weaponItemDataDictionary = new Dictionary<int, WeaponItemData>();
    public Dictionary<int, ArmorItemData> armorItemDataDictionary = new Dictionary<int, ArmorItemData>();
}

[System.Serializable]
public class MonsterDataJson
{
    public Dictionary<string, MonsterData> monsterDataDictionary = new Dictionary<string, MonsterData>();
}