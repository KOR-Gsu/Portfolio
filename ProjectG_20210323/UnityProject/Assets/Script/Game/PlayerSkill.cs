using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    public LayerMask targetLayer;
    public Skill[] skillList;

    public void UseSkill1(float damage, Enemy target, Transform transform)
    {
        Enemy[] enemies = new Enemy[1];
        enemies[0] = target;

        skillList[0].CreateSkill(Define.Skill.MeteorStrike, damage, enemies, transform.position);
        StartCoroutine(skillList[0].UpdateSkill());
    }

    public void UseSkill2(float damage, Transform transform)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 5f, targetLayer);
        Enemy[] enemies = new Enemy[colliders.Length];

        for(int i = 0; i<colliders.Length;i++)
        {
            Enemy newEnemy = colliders[i].GetComponent<Enemy>();

            if (newEnemy != null && !newEnemy.dead)
                enemies[i] = newEnemy;
        }

        skillList[1].CreateSkill(Define.Skill.Explosion, damage, enemies, transform.position);
        StartCoroutine(skillList[1].UpdateSkill());
    }
}
