using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    [CreateAssetMenu(fileName = "ScriptData", menuName = ("AddData/ScriptData"))]
    public class ScriptData : SerializedScriptableObject
    {
        [SerializeReference]
        [ListDrawerSettings(ShowIndexLabels = true, NumberOfItemsPerPage = 1), Searchable]
        private List<BaseScriptEvent> scripts;
        private BaseScriptEvent curScript;
        private int curIndex;
        [LabelText("���� �߰��� ������")]
        public Dictionary<string, object> varTable = new Dictionary<string, object>();

#if UNITY_EDITOR
        [HorizontalGroup("���� ���̺� ����", LabelWidth = 75)]
        [BoxGroup("���� ���̺� ����/���� �߰�"), SerializeField]
        [LabelText("���� �̸�")]
        private string addKey;
        [BoxGroup("���� ���̺� ����/���� �߰�"), SerializeField]
        [LabelText("���� Ÿ��")]
        private ADDABLE_TYPE addType;
        [BoxGroup("���� ���̺� ����/���� �߰�"), SerializeField]
        [Button(Name = "���� �߰�")]
        private void AddVarInTable()
        {
            object value = null;
            switch (addType)
            {
                case ADDABLE_TYPE.BOOL:
                    value = new bool();
                    break;
                case ADDABLE_TYPE.INT:
                    value = new int();
                    break;
                case ADDABLE_TYPE.FLOAT:
                    value = new float();
                    break;
            }
            if (varTable.ContainsKey(addKey))
                throw new UnityException("�̹� �����ϴ� �������Դϴ�.");

            varTable.Add(addKey, value);
        }
        [HorizontalGroup("���� ���̺� ����")]
        [BoxGroup("���� ���̺� ����/���� ����"), SerializeField]
        [LabelText("���� �̸�")]
        private string removeKey;
        [BoxGroup("���� ���̺� ����/���� ����"), SerializeField]
        [Button(Name = "���� ����")]
        private void RemoveVarInTable()
        {
            if (varTable.ContainsKey(removeKey))
                varTable.Remove(removeKey);
            else
                throw new UnityException("�������� �ʴ� �������Դϴ�.");
        }
        [BoxGroup("��ũ��Ʈ ����", true, true)]
        [SerializeField, Min(0), MaxValue("@scripts.Count - 1")]
        [LabelText("������ �ε��� ��ġ")]
        private int insertIndex;
        [BoxGroup("��ũ��Ʈ ����")]
        [Button(Name = "��ũ��Ʈ ����")]
        private void InsertScript()
        {
            scripts.Insert(insertIndex, null);
        }

        [BoxGroup("��ũ��Ʈ ��ġ �ٲٱ�", true, true)]
        [SerializeField, Min(0), MaxValue("@scripts.Count - 1")]
        [LabelText("�ٲ� �ε��� ��ġ")]
        private Vector2Int swapIndex;
        [BoxGroup("��ũ��Ʈ ��ġ �ٲٱ�")]
        [Button(Name = "��ũ��Ʈ ��ġ �ٲٱ�")]
        private void SwapScript()
        {
            (scripts[swapIndex.x], scripts[swapIndex.y]) = (scripts[swapIndex.y], scripts[swapIndex.x]);
        }
#endif

        public BaseScriptEvent CurrentScript => curScript;
        public int CurrentIndex => curIndex;
        public Dictionary<string, object> VariablesTable => varTable;

        public void SetScript(int index)
        {
            curScript?.OnExit();
            curScript = scripts[curIndex = index];
            curScript.OnEnter();
        }
        public void Clear()
        {
            curScript = null;
        }
    }
}