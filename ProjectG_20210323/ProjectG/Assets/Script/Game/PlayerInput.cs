using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private string item1Btn = nameof(Define.PlayerInput.Item1);
    private string item2Btn = nameof(Define.PlayerInput.Item2);
    private string skill1Btn = nameof(Define.PlayerInput.Skill1);
    private string skill2Btn = nameof(Define.PlayerInput.Skill2);

    public bool item1 { get; private set; }
    public bool item2 { get; private set; }
    public bool skill1 { get; private set; }
    public bool skill2 { get; private set; }

    void Update()
    { 
        if(Managers.instance != null && Managers.Game.isGameOver)
        {
            item1 = false;
            item2 = false;
            skill1 = false;
            skill2 = false;

            return;
        }

        item1 = Input.GetButtonDown(item1Btn);
        item2 = Input.GetButtonDown(item2Btn);
        skill1 = Input.GetButtonDown(skill1Btn);
        skill2 = Input.GetButtonDown(skill2Btn);
    }
}
