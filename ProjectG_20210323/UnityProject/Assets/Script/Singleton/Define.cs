using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum FileName
    {
        Log_Data,
        Player_Default_Data,
        Player_Saved_Data,
        Monster_Data,
        Item_Data
    }
    public enum ResourcePath
    {
        Prefab,
        Sprite,
        UI
    }
    public enum WindowTpye
    {
        RegisterWindow,
        ExitWindow,
        InfoWindow,
        InventoryWindow,
        ShopWindow,
    }
    public enum InGameWindowType
    {
        ExitWindow,
        InfoWindow,
        InventoryWindow,
        ShopWindow
    }
    public enum SlotTpye
    {
        Info,
        Inventory,
        Shop,
    }
    public enum Mouse
    {
        Mouse_0,
        Mouse_1,
        Mouse_2
    }
    public enum CursorType
    {
        None,
        Default,
        Attack
    }
    public enum MouseEvent
    {
        Press, 
        Down,
        Up,
        Click
    }
    public enum PlayerInput
    {
        Item1,
        Item2,
        Skill1,
        Skill2,
        Test1,
        Test2
    }
    public enum Layer
    {
        UI = 5,
        Window = 6,
        Ground = 7,
        Terrain = 8,
        Shop = 9,
        Enemy = 10,
        Player = 11
    }
    public enum PlayerState
    {
        Die,
        Idle,
        Moving,
        Attack,
        AttackIdle,
        Skill1,
        Skill2
    }
    public enum Skill
    { 
        MeteorStrike = 0,
        Explosion = 1
    }
    public enum EnemyType
    {
        Warrior,
        Archer
    }

    public enum EnemyState
    {
        Die,
        Idle,
        Moving,
        Attack,
        AttackIdle
    }

    public enum Gauge
    {
        HP,
        MP,
        EXP
    }

    public enum Menu
    {
        Info,
        Inventory,
        Eixt
    }
    public enum QuckSlot
    {
        Item_1,
        Item_2,
        Skill_1,
        Skill_2
    }
    public enum ItemSort
    {
        Consume,
        Weapon,
        Armor
    }
    public enum DropItem
    {
        Sword = 1,
        Shield
    }
    public enum Stat
    {
        LV,
        HP,
        MP,
        EXP,
        ATK,
        DEF,
        RNG,
        SPD,
        APS
    }
}
