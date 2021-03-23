using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class LivingEntity : MonoBehaviour, IDamageable
{
    [SerializeField] private string damagedTextPrefabName;
    [SerializeField] protected Transform hudPos;
    [SerializeField] private Outline outline;

    public bool dead { get; protected set; }
    public bool isTarget { get; private set; }
    public Color damagedTextColor { get; set; }
    public float maxHP { get; protected set; }

    private float _currentHP = 1;
    public float currentHP 
    {
        get{ return _currentHP; }
        set
        {
            if (value > maxHP)
                _currentHP = maxHP;
            else if (value < 0)
            {
                _currentHP = 0;
                dead = true;
                Die();
            }
            else
                _currentHP = value;
        } 
    }
    
    protected virtual void OnEnable()
    {
        dead = false;
        isTarget = false;
        currentHP = maxHP;
    }

    public virtual void OnDamage(float damage)
    {
        ShowDamaged(damage, damagedTextColor);

        currentHP -= damage;
    }

    public virtual void RestoreHP(float newHP)
    {
        if (!dead)
            currentHP += newHP;
    }

    public virtual void Die()
    {
        _currentHP = 0;

        UnTargetting();
        dead = true;
    }

    public virtual void Targetting()
    {
        isTarget = true;
        outline.OutlineMode = Outline.Mode.OutlineVisible;
    }

    public virtual void UnTargetting()
    {
        isTarget = false;
        outline.OutlineMode = Outline.Mode.SilhouetteOnly;
    }

    public void ShowDamaged(float damage, Color color)
    {
        GameObject hudText = Managers.Resource.Instantiate(string.Format("{0}/{1}", Define.ResourcePath.UI, damagedTextPrefabName), UIManager.instance.myCanvas.transform);
        hudText.GetComponent<DamageText>().targetTransform = hudPos;
        hudText.GetComponent<DamageText>().damage = damage;
        hudText.GetComponent<DamageText>().textColor = color;
    }
}