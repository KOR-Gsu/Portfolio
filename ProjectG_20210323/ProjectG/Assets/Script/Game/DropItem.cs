using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    [SerializeField] private int maxItems;

    public void DropItems()
    {
        StartCoroutine(GenerateItems());
    }

    private IEnumerator GenerateItems()
    {
        for (int i = 0; i < maxItems; i++)
        {
            yield return new WaitForSeconds(0.3f);

            Define.DropItem dropItem = (Define.DropItem)Random.Range(1, 3);
            GameObject item = Managers.Resource.Instantiate(string.Format("{0}/{1}", nameof(Define.ResourcePath.Prefab), dropItem.ToString()), transform.position);
            item.GetComponent<DropAbleItem>().SetItem((Define.ItemSort)dropItem, transform.position);
        }
    }
}
