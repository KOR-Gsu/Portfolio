using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<UIManager>();

            return _instance;
        }
    }

    public Canvas myCanvas { get; private set; }
    public List<Window> gameWindowList { get; private set; } = new List<Window>();
    public DragSlot dragSlotScript { get; private set; }

    public Slot[] quickSlotList;

    private Tooltip tooltipScript;

    public bool isWindowOpen
    {
        get
        {
            bool result = false;
            for(int i = 0; i < gameWindowList.Count;i++)
                result = result || gameWindowList[i].isOpen;

            return result;
        }
    }

    [SerializeField] private Text levelText;
    [SerializeField] private GaugeControl[] gaugesList;

    private void Awake()
    {
        if (instance != this)
            Destroy(gameObject);
        
        myCanvas = GetComponent<Canvas>();

        GameObject exit = Managers.Resource.Instantiate(string.Format("{0}/{1}", nameof(Define.ResourcePath.UI), nameof(Define.InGameWindowType.ExitWindow)), myCanvas.transform);
        GameObject info = Managers.Resource.Instantiate(string.Format("{0}/{1}", nameof(Define.ResourcePath.UI), nameof(Define.InGameWindowType.InfoWindow)), myCanvas.transform);
        GameObject inven = Managers.Resource.Instantiate(string.Format("{0}/{1}", nameof(Define.ResourcePath.UI), nameof(Define.InGameWindowType.InventoryWindow)), myCanvas.transform);
        GameObject shop = Managers.Resource.Instantiate(string.Format("{0}/{1}", nameof(Define.ResourcePath.UI), nameof(Define.InGameWindowType.ShopWindow)), myCanvas.transform);

        gameWindowList.Add(exit.GetComponent<ExitWindow>());
        gameWindowList.Add(info.GetComponent<InfoWindow>());
        gameWindowList.Add(inven.GetComponent<InventoryWindow>());
        gameWindowList.Add(shop.GetComponent<ShopWindow>());

        GameObject tooltip = Managers.Resource.Instantiate(string.Format("{0}/{1}", nameof(Define.ResourcePath.UI), "Tooltip"), myCanvas.transform);
        tooltipScript = tooltip.GetComponent<Tooltip>();

        GameObject dragSlot = Managers.Resource.Instantiate(string.Format("{0}/{1}", nameof(Define.ResourcePath.UI), "DragSlot"), myCanvas.transform);
        dragSlotScript = dragSlot.GetComponent<DragSlot>();
    }

    public void UpdateLevelText(float level)
    {
        levelText.text = ((int)level).ToString();
    }

    public void UpdateGaugeRate(Define.Gauge index, float rate)
    {
        gaugesList[(int)index].Initialize(rate);
    }

    public bool UseQuickSlot(Define.QuckSlot index)
    {
        if (!quickSlotList[(int)index].isCooldown)
        {
            quickSlotList[(int)index].StartCooldown();
            return true;
        }
        return false;
    }

    public void ShowTooltip(Item item, Vector3 pos)
    {
        if (!tooltipScript.gameObject.activeSelf)
        {
            tooltipScript.SetTooltip(item);
            tooltipScript.gameObject.SetActive(true);
            tooltipScript.SetPosition(pos);
        }
    }

    public void HideTooltip()
    {
        tooltipScript.gameObject.SetActive(false);
    }

    public void OpenWindow(int index)
    {
        gameWindowList[index].ShowWindow();
    }
}
