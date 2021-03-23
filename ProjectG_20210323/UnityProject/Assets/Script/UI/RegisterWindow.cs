using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegisterWindow : Window
{
    [SerializeField] private InputField idInputField;
    [SerializeField] private InputField pwInputField;
    
    private Color alertAlpha;
    private Color orignalAlpha;
    private float alphaSpeed = 2.0f;
    private int charMin = 2;
    private int charMax = 8;

    public override void ShowWindow()
    {
        base.ShowWindow();
    }

    public override void CloseWindow()
    {
        base.CloseWindow();
    }

    private void Start()
    {
        orignalAlpha = idInputField.image.color;
    }

    private void Update()
    {
        if(idInputField.image.color != orignalAlpha)
        {
            alertAlpha.r = Mathf.Lerp(alertAlpha.r, orignalAlpha.r, Time.deltaTime * alphaSpeed);
            alertAlpha.g = Mathf.Lerp(alertAlpha.g, orignalAlpha.g, Time.deltaTime * alphaSpeed);
            alertAlpha.b = Mathf.Lerp(alertAlpha.b, orignalAlpha.b, Time.deltaTime * alphaSpeed);

            idInputField.image.color = alertAlpha;
        }
        if (pwInputField.image.color != orignalAlpha)
        {
            alertAlpha.r = Mathf.Lerp(alertAlpha.r, orignalAlpha.r, Time.deltaTime * alphaSpeed);
            alertAlpha.g = Mathf.Lerp(alertAlpha.g, orignalAlpha.g, Time.deltaTime * alphaSpeed);
            alertAlpha.b = Mathf.Lerp(alertAlpha.b, orignalAlpha.b, Time.deltaTime * alphaSpeed);

            pwInputField.image.color = alertAlpha;
        }
    }

    public void RegisterPlayer()
    {
        if (CheckInputField())
            return;

        LogData currentLogData = new LogData(idInputField.GetComponentInChildren<Text>().text, pwInputField.text);
        LogDataJson logDataJson = Managers.Data.JsonToData<LogDataJson>(nameof(Define.FileName.Log_Data));

        if (IsSameLogData(logDataJson, currentLogData))
            return;

        logDataJson.Add(currentLogData);
        Managers.Data.DataToJson<LogDataJson>(nameof(Define.FileName.Log_Data), logDataJson);

        CloseWindow();
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

    private bool IsSameLogData(LogDataJson logDataJson, LogData logData)
    {
        if (logDataJson.IsData(logData.id))
        {
            AlertInputField(idInputField);
            AlertInputField(pwInputField);

            return true;
        }

        return false;
    }

    private void AlertInputField(InputField inputField)
    {
        alertAlpha = Color.red;
        inputField.image.color = alertAlpha;
    }
}
