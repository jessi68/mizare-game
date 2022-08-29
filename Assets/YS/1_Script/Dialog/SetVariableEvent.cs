using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    [System.Serializable]
    public class SetVariableEvent : BaseScriptEvent
    {
        [SerializeField, LabelText("������ ������")]
        private ChangeVariableDataInTable[] changeVars = new ChangeVariableDataInTable[0];

        public override void OnEnter()
        {
            base.OnEnter();

            var table = gm.scriptData.VariablesTable;

            foreach (var changeVar in changeVars)
            {
                if (!table.ContainsKey(changeVar.varName))
                    throw new UnityException("�������� �ʴ� �������Դϴ�.");

                switch (changeVar.type)
                {
                    case ADDABLE_TYPE.BOOL:
                        table[changeVar.varName] = changeVar.valueBool;
                        break;
                    case ADDABLE_TYPE.INT:
                        table[changeVar.varName] = changeVar.valueInt;
                        break;
                    case ADDABLE_TYPE.FLOAT:
                        table[changeVar.varName] = changeVar.valueFloat;
                        break;
                }
            }

            gm.scriptData.SetScript(gm.scriptData.CurrentIndex + 1);
        }
        protected override void OnUpdate() { }
    }
}