using UnityEngine;

namespace YS
{
    [CreateAssetMenu(fileName = "ItemData", menuName = ("AddData/ItemData"))]
    public class ItemData : ScriptableObject
    {
        [SerializeField]
        private ItemInfo[] items;

        public ItemInfo this[ITEM_INDEX index]
        {
            get { return items[(int)index]; }
        }
    }
    [System.Serializable]
    public struct InferenceChoiceInfo
    {
        public string choiceStr;
        public string resultStr;
    }
    [System.Serializable]
    public struct ItemInfo
    {
        public Sprite img;

        public string name;
        public string desc;
        public InferenceChoiceInfo[] choicesInfo;
        public uint correctIndex;
    }
}