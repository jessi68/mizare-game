using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace YS
{
    [RequireComponent(typeof(Image))]
    public class Item : MonoBehaviour, IPointerClickHandler
    {
        #region Fields
        public ITEM_INDEX index;
        public Image image;
        #endregion

        public void OnPointerClick(PointerEventData eventData)
        {
            if (InGameUIManager.Instance.IsShowingInventory)
                GameManager.Instance.invenComp.SetItemInfo(index);
            else
                GameManager.Instance.OnFindItem(this);
        }
    }
}