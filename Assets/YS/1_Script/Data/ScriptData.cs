using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    [CreateAssetMenu(fileName = "ScriptData", menuName = ("AddData/ScriptData"))]
    public class ScriptData : ScriptableObject
    {
        [SerializeReference]
        [ListDrawerSettings(ShowIndexLabels = true)]
        private List<BaseScriptEvent> scripts;
        private BaseScriptEvent curScript;
        private int curIndex;

#if UNITY_EDITOR
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

        public void SetScript(int index)
        {
            curScript?.OnExit();
            curScript = scripts[curIndex = index];
            curScript.OnEnter();
        }
    }
}