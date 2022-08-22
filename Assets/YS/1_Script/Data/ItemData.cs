using UnityEngine;
using Sirenix.OdinInspector;

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
        [LabelText("선택지 내용")]
        public string choiceStr;
        [LabelText("선택지에 대한 대답")]
        public string resultStr;
    }
    [System.Serializable]
    public struct ItemInfo
    {
        [LabelText("이미지")]
        public Sprite img;
        [LabelText("이름")]
        public string name;
        [LabelText("설명"), TextArea]
        public string desc;
        [LabelText("선택지 정보")]
        public InferenceChoiceInfo[] choicesInfo;
        [LabelText("올바른 선택지")]
        public uint correctIndex;
    }
}