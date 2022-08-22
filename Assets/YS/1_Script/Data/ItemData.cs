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
        [LabelText("������ ����")]
        public string choiceStr;
        [LabelText("�������� ���� ���")]
        public string resultStr;
    }
    [System.Serializable]
    public struct ItemInfo
    {
        [LabelText("�̹���")]
        public Sprite img;
        [LabelText("�̸�")]
        public string name;
        [LabelText("����"), TextArea]
        public string desc;
        [LabelText("������ ����")]
        public InferenceChoiceInfo[] choicesInfo;
        [LabelText("�ùٸ� ������")]
        public uint correctIndex;
    }
}