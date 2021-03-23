using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageText : MonoBehaviour
{
    public Transform targetTransform;

    private Camera worldCamera;
    private RectTransform rectParent;
    private RectTransform rectDamageText;
    private Vector3 offSet;

    private float moveSpeed;
    private float alphaSpeed;
    private float destroyTime;
    private float deltaY;
    private Text damageText;
    private Color alpha;

    public float damage;
    public Color textColor;

    void Start()
    {
        worldCamera = UIManager.instance.myCanvas.worldCamera;
        rectParent = UIManager.instance.myCanvas.GetComponent<RectTransform>();
        rectDamageText = GetComponent<RectTransform>();
        offSet = new Vector3(0f, 0f, 0f);
        moveSpeed = 3.0f;
        alphaSpeed = 1.2f;
        destroyTime = 1.0f;

        damageText = GetComponent<Text>();
        alpha = textColor;
        damageText.text = ((int)damage).ToString();

        Invoke("DestroyObject", destroyTime);
    }
    
    void Update()
    {
        deltaY += moveSpeed * Time.deltaTime;
        offSet.y = deltaY;

        alpha.a = Mathf.Lerp(alpha.a, 0, Time.deltaTime * alphaSpeed);
        damageText.color = alpha;

        rectDamageText.transform.position = Camera.main.WorldToScreenPoint(targetTransform.position + offSet);
    }

    private void DestroyObject()
    {
        Destroy(gameObject);
    }
}
