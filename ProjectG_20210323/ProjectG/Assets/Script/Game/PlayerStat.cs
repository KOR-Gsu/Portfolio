using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStat : LivingEntity
{
    public float level { get; private set; }
    public float maxMP { get; private set; }
    public float maxEXP { get; private set; }
    public float attackDamage { get; private set; }
    public float defense { get; private set; }
    public float attackRange { get; private set; }
    public float moveSpeed { get; private set; }
    public float rotateSpeed { get; private set; }
    public float intvlAttackTime { get; private set; }
    public float gold { get; private set; }
    public InfoWindow info { get; private set; }
    public InventoryWindow inventory { get; private set; }
    public float finalAttackDamage { get; private set; }
    public float finalAttackRange { get; private set; }
    public float finalIntvlAttackTime { get; private set; }
    public float finalDefense { get; private set; }

    private PlayerInput playerInput;

    private float _currentMP;
    public float currentMP
    {
        get { return _currentMP; }
        set
        {
            if (value > maxMP)
                _currentMP = maxMP;
            else if (value < 0)
                _currentMP = 0;
            else
                _currentMP = value;

            UIManager.instance.UpdateGaugeRate(Define.Gauge.MP, _currentMP / maxMP);
        } 
    }

    private float _currentEXP;
    public float currentEXP
    {
        get { return _currentEXP; }
        set
        {
            if (value >= maxEXP)
            {
                _currentEXP = value;
                while (_currentEXP > maxEXP)
                {
                    _currentEXP -= maxEXP;
                    UpLevel();
                }
            }
            else if (value < 0)
                _currentEXP = 0;
            else
                _currentEXP = value;

            UIManager.instance.UpdateGaugeRate(Define.Gauge.EXP, _currentEXP / maxEXP);
        } 
    }

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        damagedTextColor = Color.red;

        Managers.Input.keyAction -= OnItemKeyPress;
        Managers.Input.keyAction += OnItemKeyPress;
    }

    private void Start()
    {
        info = (InfoWindow)UIManager.instance.gameWindowList[(int)Define.InGameWindowType.InfoWindow];
        inventory = (InventoryWindow)UIManager.instance.gameWindowList[(int)Define.InGameWindowType.InventoryWindow];

        Initializing(Managers.Game.LoadPlayerStat());
    }

    public void Initializing(PlayerData data)
    {
        data.dataDictionary.TryGetValue("Level", out float _level);
        data.dataDictionary.TryGetValue("maxHP", out float _maxHP);
        data.dataDictionary.TryGetValue("maxMP", out float _maxMP);
        data.dataDictionary.TryGetValue("maxEXP", out float _maxEXP);
        data.dataDictionary.TryGetValue("currentHP", out float _currentHP);
        data.dataDictionary.TryGetValue("currentMP", out float _currentMP);
        data.dataDictionary.TryGetValue("currentEXP", out float _currentEXP);
        level = _level;
        maxHP = _maxHP;
        maxMP = _maxMP;
        maxEXP = _maxEXP;
        currentHP = _currentHP;
        currentMP = _currentMP;
        currentEXP = _currentEXP;

        data.dataDictionary.TryGetValue("attackDamage", out float _attackDamage);
        data.dataDictionary.TryGetValue("defense", out float _defense);
        data.dataDictionary.TryGetValue("attackRange", out float _attackRange);
        data.dataDictionary.TryGetValue("moveSpeed", out float _moveSpeed);
        data.dataDictionary.TryGetValue("rotateSpeed", out float _rotateSpeed);
        data.dataDictionary.TryGetValue("intvlAttackTime", out float _intvlAttackTime);
        data.dataDictionary.TryGetValue("gold", out float _gold);
        attackDamage = _attackDamage;
        defense = _defense;
        attackRange = _attackRange;
        moveSpeed = _moveSpeed;
        rotateSpeed = _rotateSpeed;
        intvlAttackTime = _intvlAttackTime;
        gold = _gold;

        info.Initializing(data);
        inventory.Initializing(data);

        UIManager.instance.UpdateLevelText(level);
        UIManager.instance.UpdateGaugeRate(Define.Gauge.HP, currentHP / maxHP);
        UIManager.instance.UpdateGaugeRate(Define.Gauge.MP, currentMP / maxMP);
        UIManager.instance.UpdateGaugeRate(Define.Gauge.EXP, currentEXP / maxEXP);
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        currentMP = maxMP;
    }

    private void Update()
    {
        finalAttackDamage = attackDamage;
        finalAttackRange = attackRange;
        finalIntvlAttackTime = intvlAttackTime;

        finalDefense = defense;

        if (info.weaponSlot.item != null)
        {
            WeaponItemData weapon = (WeaponItemData)info.weaponSlot.item;

            finalAttackDamage += weapon.attackDamageValue;
            finalAttackRange += weapon.attackRangeValue;
            finalIntvlAttackTime += weapon.intvlAttackValue;
        }
        if(info.armorSlot.item != null)
        {
            ArmorItemData armor = (ArmorItemData)info.armorSlot.item;

            finalDefense += armor.defenseValue;
        }
    }

    public void GetExp(int newExp, int newGold)
    {
        currentEXP += newExp;
        gold += newGold;
    }

    public void GetItem(DropAbleItem dropItem)
    {
        Item newItem = Managers.Item.FindItem(dropItem.itemSort, dropItem.itemID);

        inventory.AcquireItem(newItem);
    }

    public void Buy(Item newItem)
    {
        if (gold - newItem.price < 0)
            return;

        gold -= newItem.price;

        inventory.AcquireItem(newItem);
    }

    public void Sell(Item item)
    {
        gold += item.price;

        inventory.UpdateItemCount(-1);
    }

    public Item EquipItem(Item newItem)
    {
        if (newItem.itemSort == Define.ItemSort.Weapon)
        {
            Item oldWeapon = info.weaponSlot.item;
            info.weaponSlot.AddItem(newItem);

            return oldWeapon;
        }
        else if(newItem.itemSort == Define.ItemSort.Armor)
        {
            Item oldArmor = info.armorSlot.item;
            info.armorSlot.AddItem(newItem);

            return oldArmor;
        }

        return null;
    }

    public void GetGold(int newGold)
    {
        gold += newGold;
    }

    private void UpLevel()
    {
        level++;

        maxHP += 10f;
        maxMP += 10f;

        currentHP = maxHP;
        currentMP = maxMP;

        UIManager.instance.UpdateLevelText(level);
        UIManager.instance.UpdateGaugeRate((int)Define.Gauge.HP, currentHP / maxHP);
    }

    private void OnItemKeyPress()
    {
        if (playerInput.item1)
        {
            if (inventory.itemSlot1.item != null && inventory.itemSlot1.item.itemSort == Define.ItemSort.Consume)
            {
                if (inventory.itemSlot1.itemCount > 0)
                {
                    if (UIManager.instance.UseQuickSlot(Define.QuckSlot.Item_1))
                    {
                        ConsumeItemData item = (ConsumeItemData)inventory.itemSlot1.item;

                        if (item.id == 0)
                            RestoreHP(item.value);
                        else if (item.id == 1)
                            RestoreMP(item.value);

                        inventory.itemSlot1.UpdateItemCount(-1);
                    }
                }
            }
        }
        if (playerInput.item2)
        {
            if (inventory.itemSlot2.item != null && inventory.itemSlot2.item.itemSort == Define.ItemSort.Consume)
            {
                if (inventory.itemSlot2.itemCount > 0)
                {
                    if (UIManager.instance.UseQuickSlot(Define.QuckSlot.Item_2))
                    {
                        ConsumeItemData item = (ConsumeItemData)inventory.itemSlot2.item;

                        if (item.id == 0)
                            RestoreHP(item.value);
                        else if (item.id == 1)
                            RestoreMP(item.value);

                        inventory.itemSlot2.UpdateItemCount(-1);
                    }
                }
            }
        }
    }

    public override void RestoreHP(float newHP)
    {
        base.RestoreHP(newHP);
        UIManager.instance.UpdateGaugeRate((int)Define.Gauge.HP, currentHP / maxHP);
    }

    public void RestoreMP(float newMP)
    {
        if (!dead)
            currentMP += newMP;
    }

    public override void OnDamage(float damage)
    {
        float finalDamage = Mathf.Max(0, damage - finalDefense);

        base.OnDamage(finalDamage);

        if(!dead)
            UIManager.instance.UpdateGaugeRate((int)Define.Gauge.HP, currentHP / maxHP);
    }

    public override void Die()
    {
        base.Die();
        UIManager.instance.UpdateGaugeRate((int)Define.Gauge.HP, 0);

        PlayerMove playerMove = GetComponent<PlayerMove>();
        playerMove.playerState = Define.PlayerState.Die;

        currentHP = maxHP;

        Invoke(nameof(GameOver), 4f);
    }

    private void GameOver()
    {
        Managers.Game.RestartGame();
    }
}
