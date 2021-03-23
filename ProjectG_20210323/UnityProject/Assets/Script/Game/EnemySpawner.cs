using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Define.EnemyType enemyType;
    [SerializeField] private int maxCount;
    [SerializeField] private int keepSpawnCount;
    [SerializeField] private int curSpawnCount;
    [SerializeField] private float spawnRadius;
    [SerializeField] private float spawnTime;
    private int _keepSpawnCount = 0;

    private List<GameObject> enemyPrefabList = new List<GameObject>();
    private int spawnIndex = 0;

    private void Start()
    {
        Managers.Game.OnSpawnEvent -= CheckEnemyDead;
        Managers.Game.OnSpawnEvent += CheckEnemyDead;

        SetEnemy();

        for (int i = 0; i < keepSpawnCount; i++)
        {
            enemyPrefabList[i].SetActive(true);
            curSpawnCount++;
        }
    }

    private void OnDisable()
    {
        if (Managers.instance != null)
            Managers.Game.OnSpawnEvent -= CheckEnemyDead;
    }

    void Update()
    {
        while (_keepSpawnCount + curSpawnCount < keepSpawnCount)
        {
            StartCoroutine(Spawn());
        }
    }

    private void SetEnemy()
    {
        MonsterDataJson monsterDataJson = Managers.Data.JsonToData<MonsterDataJson>(nameof(Define.FileName.Monster_Data));

        monsterDataJson.monsterDataDictionary.TryGetValue(enemyType.ToString(), out MonsterData data);

        for (int i = 0; i < maxCount; i++)
        {
            string str = string.Format("{0}/{1}", nameof(Define.ResourcePath.Prefab), enemyType.ToString());
            GameObject newEnemyPrefab = Managers.Resource.Instantiate(str, transform);
            Enemy newEnemy = newEnemyPrefab.GetComponent<Enemy>();

            newEnemyPrefab.SetActive(false);
            newEnemy.Initializing(data);
            enemyPrefabList.Add(newEnemyPrefab);
        }
    }

    public void CheckEnemyDead()
    {
        for(int i = 0; i < enemyPrefabList.Count;i++)
        {
            if(enemyPrefabList[i].GetComponent<Enemy>().dead)
            {
                curSpawnCount--;

                spawnIndex += 1;
                if (spawnIndex >= maxCount)
                    spawnIndex = 0;
            }
        }
    }

    private IEnumerator Spawn()
    {
        _keepSpawnCount++;
        yield return new WaitForSeconds(Random.Range(1f, spawnTime));

        Vector3 randDir = Random.insideUnitSphere * Random.Range(0, spawnRadius);
        randDir.y = transform.position.y;
        Vector3 randSpawnPoint = transform.position + randDir;

        enemyPrefabList[spawnIndex].transform.position = randSpawnPoint;
        enemyPrefabList[spawnIndex].SetActive(true);

        curSpawnCount++;
        _keepSpawnCount--;
    }
}
