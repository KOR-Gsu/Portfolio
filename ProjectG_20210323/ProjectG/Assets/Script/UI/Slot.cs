using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public bool isSkill;
    public Define.SlotTpye slotTpye;

    public Item item;
    public Image itemImage;
    public int itemCount;
    [SerializeField] private Image itemGradeImage;
    [SerializeField] private Text countText;

    public bool isCooldown { get; private set; } = false;
    public float cooldown;
    [SerializeField] private Image cooldownImage;

    private PlayerStat playerStat;

    private void Start()
    {
        playerStat = GameObject.Find("Player").GetComponent<PlayerStat>();
    }

    private void SetItemSprite(Sprite newSprite)
    {
        itemImage.sprite = newSprite;

        if (newSprite != null)
            itemImage.gameObject.SetActive(true);
        else
            itemImage.gameObject.SetActive(false);
    }

    private void SetGradeColor(bool active)
    {
        if (active)
        {
            itemGradeImage.gameObject.SetActive(true);
            switch (item.grade)
            {
                case 0:
                    itemGradeImage.color = Color.white;
                    break;
                case 1:
                    itemGradeImage.color = Color.blue;
                    break;
                case 2:
                    itemGradeImage.color = Color.red;
                    break;
                default:
                    break;
            }
        }
        else
            itemGradeImage.gameObject.SetActive(false);
    }

    private void SetCountText(bool active)
    {
        if(active)
        {
            countText.gameObject.SetActive(true);
            countText.text = itemCount.ToString();
        }
        else
        {
            countText.gameObject.SetActive(false);
            countText.text = "0";
        }
    }

    public void AddItem(Item newItem, int count = 1)
    {
        if (isSkill)
            return;

        newItem.SetImage();
        item = newItem;
        itemCount = count;
        SetItemSprite(item.itemImage);
        SetGradeColor(true);

        if (item.itemSort == Define.ItemSort.Consume)
            SetCountText(true);
        else
            SetCountText(false);
    }

    public void RemoveItem()
    {
        if (isSkill)
            return;

        item = null;
        SetItemSprite(null);
        SetGradeColor(false);
        SetCountText(false);
    }

    public void UpdateItemCount(int count)
    {
        if (isSkill)
            return;

        itemCount += count;
        countText.text = itemCount.ToString();

        if (itemCount <= 0)
            RemoveItem();
    }

    public void StartCooldown()
    {
        if (item != null)
        {
            if (item.itemSort == Define.ItemSort.Consume)
            {
                ConsumeItemData UsingItem = (ConsumeItemData)item;

                cooldownImage.gameObject.SetActive(true);
                StartCoroutine(UpdateCooldown(UsingItem.cooldown));
            }
        }
        else
        {
            cooldownImage.gameObject.SetActive(true);
            StartCoroutine(UpdateCooldown(cooldown));
        }
    }

    private IEnumerator UpdateCooldown(float cooldown)
    {
        isCooldown = true;

        float nowDelayTime = 0;
        while (nowDelayTime < cooldown)
        {
            nowDelayTime += Time.deltaTime;
            cooldownImage.fillAmount = (1.0f - nowDelayTime / cooldown);

            yield return new WaitForFixedUpdate();
        }

        isCooldown = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(item != null)
            UIManager.instance.ShowTooltip(item, transform.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.instance.HideTooltip();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isSkill)
            return;

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (item != null)
            {
                switch (slotTpye)
                {
                    case Define.SlotTpye.Info:
                        {
                            playerStat.inventory.AcquireItem(item);
                            RemoveItem();
                            UIManager.instance.HideTooltip();
                        }
                        break;
                    case Define.SlotTpye.Inventory:
                        {
                            if (UIManager.instance.gameWindowList[(int)Define.InGameWindowType.ShopWindow].isOpen)
                            {
                                playerStat.Sell(item);

                                if(item.itemSort == Define.ItemSort.Consume)
                                    UpdateItemCount(-1);
                                else
                                    RemoveItem();
                            }
                            else if (UIManager.instance.gameWindowList[(int)Define.InGameWindowType.InfoWindow].isOpen)
                            {
                                Item oldItem = playerStat.EquipItem(Managers.Item.FindItem(item.itemSort, item.id));

                                if (oldItem != null)
                                {
                                    AddItem(oldItem);
                                    UIManager.instance.HideTooltip();
                                    UIManager.instance.ShowTooltip(oldItem, transform.position);
                                }
                                else
                                {
                                    RemoveItem();
                                    UIManager.instance.HideTooltip();
                                }
                            }
                        }
                        break;
                    case Define.SlotTpye.Shop:
                        {
                            playerStat.Buy(item);
                        }
                        break;
                }
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isSkill)
            return;

        if (item != null)
        {
            UIManager.instance.dragSlotScript.dragSlot = this;
            UIManager.instance.dragSlotScript.SetDragImage(itemImage);
            UIManager.instance.dragSlotScript.transform.position = eventData.position;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isSkill)
            return;

        if (item != null)
            UIManager.instance.dragSlotScript.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isSkill)
            return;

        UIManager.instance.dragSlotScript.SetColor(0);
        UIManager.instance.dragSlotScript.dragSlot = null;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (isSkill)
            return;

        if (UIManager.instance.dragSlotScript.dragSlot != null)
            ChangeSlot();
    }

    private void ChangeSlot()
    {
        Item _tempItem = item;
        int _tempCount = itemCount;

        AddItem(UIManager.instance.dragSlotScript.dragSlot.item, UIManager.instance.dragSlotScript.dragSlot.itemCount);

        if (_tempItem != null)
            UIManager.instance.dragSlotScript.dragSlot.AddItem(_tempItem, _tempCount);
        else
            UIManager.instance.dragSlotScript.dragSlot.RemoveItem();
    }
}
