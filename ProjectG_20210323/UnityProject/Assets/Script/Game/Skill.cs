using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public float[] onDamageTime;
    public float[] magnification;
    public float cooldown;
    public float mpCost;
    public float duration;

    private Enemy[] targetEnemies;
    private GameObject skillObject;
    private float playerDamage;
    private int onDamageIndex;

    public void CreateSkill(Define.Skill skill, float damage, Enemy[] target, Vector3 position)
    {
        targetEnemies = target;

        skillObject = Managers.Resource.Instantiate(string.Format("{0}/{1}", nameof(Define.ResourcePath.Prefab), skill.ToString()), position);
        playerDamage = damage;
        onDamageIndex = 0;
    }

    public IEnumerator UpdateSkill()
    {
        Invoke("Destroy", duration);

        float time = 0;
        while (onDamageIndex < onDamageTime.Length)
        {
            time += Time.deltaTime;
            if (time >= onDamageTime[onDamageIndex])
            {
                for (int i = 0; i < targetEnemies.Length; i++)
                {
                    if (targetEnemies[i] == null || targetEnemies[i].dead)
                        continue;

                    targetEnemies[i].OnDamage(playerDamage * magnification[onDamageIndex]);
                }

                onDamageIndex++;
            }

            yield return new WaitForFixedUpdate();
        }
    }

    public void Destroy()
    {
        targetEnemies = null;
        Destroy(skillObject);
    }
}
