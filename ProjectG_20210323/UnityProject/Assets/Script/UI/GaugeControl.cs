using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GaugeControl : MonoBehaviour
{
    [SerializeField] private Image myContent;
    [SerializeField] private Text myPercentage;

    private float lerpSpeed = 20f;
    private float currentFill;

    void Update()
    {
        if (currentFill != myContent.fillAmount)
            myContent.fillAmount = Mathf.Lerp(myContent.fillAmount, currentFill, Time.deltaTime * lerpSpeed);
    }

    public void Initialize(float rate)
    {
        currentFill = rate;

        myPercentage.text = string.Format("{0:P1}", currentFill);
    }

    public void ResetGauge()
    {
        myContent.fillAmount = 1.0f;

        myPercentage.text = string.Format("{0:P1}", currentFill);
    }
}