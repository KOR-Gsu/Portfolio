using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterHPGauge : MonoBehaviour
{
    [SerializeField] private Image myContent;
    [SerializeField] private Text myPercentage;

    [HideInInspector] public Transform targetTransform;

    private Camera worldCamera;
    private RectTransform rectParent;
    private RectTransform rectHPBar;
    private Vector3 offSet;
    private float lerpSpeed = 10f;
    private float currentFill;

    void Awake()
    {
        worldCamera = UIManager.instance.myCanvas.worldCamera;
        rectParent = UIManager.instance.myCanvas.GetComponent<RectTransform>();
        rectHPBar = GetComponent<RectTransform>();
        offSet = new Vector3(0f, 2f, 0f);
    }
    
    void Update()
    {
        if (currentFill != myContent.fillAmount)
            myContent.fillAmount = Mathf.Lerp(myContent.fillAmount, currentFill, Time.deltaTime * lerpSpeed);

        rectHPBar.transform.position = Camera.main.WorldToScreenPoint(targetTransform.position + offSet);
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
