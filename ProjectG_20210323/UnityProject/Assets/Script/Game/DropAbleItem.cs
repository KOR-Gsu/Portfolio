using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropAbleItem : MonoBehaviour
{
    public Define.ItemSort itemSort;
    
    public int itemID { get; private set; }

    private Rigidbody itemRigidbody;
    private bool isRanding;

    public void SetItem(Define.ItemSort sort, Vector3 pos)
    {
        transform.position = pos + Vector3.up;
        isRanding = false;

        itemSort = sort;
        switch (itemSort)
        {
            case Define.ItemSort.Weapon:
                itemID = Random.Range(0, Managers.Item.weaponItemDataList.Count);
                break;
            case Define.ItemSort.Armor:
                itemID = Random.Range(0, Managers.Item.armorItemDataList.Count);
                break;
        }

        Vector3 randPos = Random.insideUnitSphere * Random.Range(0, 3f);
        randPos.y = 3f;

        itemRigidbody = GetComponent<Rigidbody>();
        itemRigidbody.AddForce(randPos.normalized * 25, ForceMode.Impulse);
        itemRigidbody.AddRelativeTorque(Vector3.one * 30, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Terrain")
        {
            isRanding = true;
            itemRigidbody.velocity = Vector3.zero;
        }
        if (collision.gameObject.tag == "Player")
        {
            if (!isRanding)
                return;

            collision.gameObject.GetComponent<PlayerStat>().GetItem(this);

            Destroy(gameObject);
        }
    }
}
