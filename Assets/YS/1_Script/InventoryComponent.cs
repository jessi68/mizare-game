using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace YS
{
    public class InventoryComponent : MonoBehaviour
    {
        public Transform rootItemTr;
        public Image invenItemImg;
        public TMP_Text invenItemName;
        public TMP_Text invenItemDesc;
        public GameObject invenItemPrefab;

        private List<ITEM_INDEX> inven = new List<ITEM_INDEX>();

        public void OpenInventory()
        {
            gameObject.SetActive(true);
            invenItemImg.sprite = null;
            invenItemName.text = "";
            invenItemDesc.text = "";
        }
        public void CloseInventory()
        {
            gameObject.SetActive(false);
        }

        public void AddItem(ITEM_INDEX itemIndex)
        {
            inven.Add(itemIndex);
            Item item = Instantiate(invenItemPrefab, rootItemTr).GetComponent<Item>();
            item.index = itemIndex;
            item.image.sprite = GameManager.Instance.itemData[itemIndex].img;
        }
        public void SetItemInfo(ITEM_INDEX itemIndex)
        {
            var itemData = GameManager.Instance.itemData;

            invenItemImg.sprite = itemData[itemIndex].img;
            invenItemName.text = itemData[itemIndex].name;
            invenItemDesc.text = itemData[itemIndex].desc;
        }
    }
}