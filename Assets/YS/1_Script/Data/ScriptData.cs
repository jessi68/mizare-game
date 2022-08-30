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
        [LabelText("현재 추가된 변수들")]
        public Dictionary<string, object> varTable = new Dictionary<string, object>();

#if UNITY_EDITOR
        [HorizontalGroup("변수 테이블 조작", LabelWidth = 75)]
        [BoxGroup("변수 테이블 조작/변수 추가"), SerializeField]
        [LabelText("변수 이름")]
        private string addKey;
        [BoxGroup("변수 테이블 조작/변수 추가"), SerializeField]
        [LabelText("변수 타입")]
        private ADDABLE_TYPE addType;
        [BoxGroup("변수 테이블 조작/변수 추가"), SerializeField]
        [Button(Name = "변수 추가")]
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
                throw new UnityException("이미 존재하는 변수명입니다.");

            varTable.Add(addKey, value);
        }
        [HorizontalGroup("변수 테이블 조작")]
        [BoxGroup("변수 테이블 조작/변수 제거"), SerializeField]
        [LabelText("변수 이름")]
        private string removeKey;
        [BoxGroup("변수 테이블 조작/변수 제거"), SerializeField]
        [Button(Name = "변수 제거")]
        private void RemoveVarInTable()
        {
            if (varTable.ContainsKey(removeKey))
                varTable.Remove(removeKey);
            else
                throw new UnityException("존재하지 않는 변수명입니다.");
        }
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