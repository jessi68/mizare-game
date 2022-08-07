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
        [BoxGroup("스크립트 삽입", true, true)]
        [SerializeField, Min(0), MaxValue("@scripts.Count - 1")]
        [LabelText("삽입할 인덱스 위치")]
        private int insertIndex;
        [BoxGroup("스크립트 삽입")]
        [Button(Name = "스크립트 삽입")]
        private void InsertScript()
        {
            scripts.Insert(insertIndex, null);
        }

        [BoxGroup("스크립트 위치 바꾸기", true, true)]
        [SerializeField, Min(0), MaxValue("@scripts.Count - 1")]
        [LabelText("바꿀 인덱스 위치")]
        private Vector2Int swapIndex;
        [BoxGroup("스크립트 위치 바꾸기")]
        [Button(Name = "스크립트 위치 바꾸기")]
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