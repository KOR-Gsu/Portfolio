using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TileUIScript : MonoBehaviour
{
    [SerializeField] private InputField idInputField;
    [SerializeField] private InputField pwInputField;

    private List<Window> titleWindowList = new List<Window>();

    private Canvas myCanvas;
    private Color alertAlpha;
    private Color orignalAlpha;
    private float alphaSpeed = 2.0f;
    private int charMin = 2;
    private int charMax = 8;

    void Start()
    {
        myCanvas = GetComponent<Canvas>();

        orignalAlpha = idInputField.image.color;

        GameObject register = Managers.Resource.Instantiate(string.Format("{0}/{1}", nameof(Define.ResourcePath.UI), nameof(Define.WindowTpye.RegisterWindow)), myCanvas.transform);
        GameObject exit = Managers.Resource.Instantiate(string.Format("{0}/{1}", nameof(Define.ResourcePath.UI), nameof(Define.WindowTpye.ExitWindow)), myCanvas.transform);

        titleWindowList.Add(register.GetComponent<RegisterWindow>());
        titleWindowList.Add(exit.GetComponent<ExitWindow>());
    }

    void Update()
    {
        alertAlpha.r = Mathf.Lerp(alertAlpha.r, orignalAlpha.r, Time.deltaTime * alphaSpeed);
        alertAlpha.g = Mathf.Lerp(alertAlpha.g, orignalAlpha.g, Time.deltaTime * alphaSpeed);
        alertAlpha.b = Mathf.Lerp(alertAlpha.b, orignalAlpha.b, Time.deltaTime * alphaSpeed);

        if (idInputField.image.color != orignalAlpha)
            idInputField.image.color = alertAlpha;

        if (pwInputField.image.color != orignalAlpha)
            pwInputField.image.color = alertAlpha;
    }

    public void LogIn()
    {
        if (CheckInputField())
            return;

        LogData currentLogData = new LogData(idInputField.GetComponentInChildren<Text>().text, pwInputField.text);

        if (CheckLogData(currentLogData))
            return;

        Managers.Data.currentLog = currentLogData;
        Managers.Game.Init();

        SceneManager.LoadSceneAsync("TownScene");
    }

    private bool CheckInputField()
    {
        bool idCheck = false;
        bool pwCheck = false;

        if (idInputField.GetComponentInChildren<Text>().text.Length < charMin || idInputField.GetComponentInChildren<Text>().text.Length > charMax)
        {
            AlertInputField(idInputField);
            idCheck = true;
        }
        if (pwInputField.text.Length < charMin || pwInputField.text.Length > charMax)
        {
            AlertInputField(pwInputField);
            pwCheck = true;
        }

        return idCheck || pwCheck;
    }

    private bool CheckLogData(LogData logData)
    {
        LogDataJson logDataJson = Managers.Data.JsonToData<LogDataJson>(nameof(Define.FileName.Log_Data));
        if (logDataJson.IsData(logData.id))
        {
            logDataJson.logDataDictionary.TryGetValue(logData.id, out LogData logtmp);

            if (logData.pw != logtmp.pw)
            {
                AlertInputField(pwInputField);
                return true;
            }
        }
        else
        {
            AlertInputField(idInputField);
            return true;
        }

        return false;
    }

    private void AlertInputField(InputField inputField)
    {
        alertAlpha = Color.red;
        inputField.image.color = alertAlpha;
    }

    public void OpenWindow(int index)
    {
        titleWindowList[index].ShowWindow();
    }
}
