using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    [CreateAssetMenu(fileName = "ScriptData", menuName = ("AddData/ScriptData"))]
    public class ScriptData : ScriptableObject
    {
        [SerializeReference]
        private BaseScriptEvent[] scripts;
        private BaseScriptEvent curScript;

#if UNITY_EDITOR
        [SerializeField, PropertyRange(0, "@scripts.Length - 1")]
        [LabelText("������ �ε��� ��ġ")]
        private uint insertIndex;
        [Button(Name = "��ũ��Ʈ ����")]
        private void InsertScript()
        {
        }
#endif

        public BaseScriptEvent CurrentScript => curScript;

        public void SetScript(uint index)
        {
            curScript?.OnExit();
            curScript = scripts[index];
            curScript.OnEnter();
        }
    }
}