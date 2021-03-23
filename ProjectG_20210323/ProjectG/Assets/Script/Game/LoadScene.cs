using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{    
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if (SceneManager.GetActiveScene().name == "TownScene")
            {
                Managers.Game.SavePlayerData();
                SceneManager.LoadScene("DungeonScene");
            }
            if (SceneManager.GetActiveScene().name == "DungeonScene")
            {
                Managers.Game.SavePlayerData();
                SceneManager.LoadScene("TownScene");
            }
        }
    }
}
