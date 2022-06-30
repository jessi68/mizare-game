using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace YS
{
    [System.Serializable]
    public class ChoiceEvent : BaseScriptEvent
    {
        [Tooltip("������")]
        public ChoiceData[] choices;

        public override void OnEnter()
        {
            base.OnEnter();

            gm.choiceStruct.choiceUI.SetActive(true);

            gm.choiceStruct.SetChoice(choices);
        }
        protected override void OnUpdate() { }
        public override void OnExit()
        {
            gm.choiceStruct.choiceUI.SetActive(false);

            base.OnExit();
        }
        public void OnChooseChoice(int index)
        {
            // ��� �������� ��Ȱ��ȭ�ϰ�
            for (int i = 0; i < choices.Length; ++i)
                gm.choiceStruct.choiceBtns[i].gameObject.SetActive(false);

            gm.scriptData.SetScript(choices[index].nextIdx);
        }
    }
}
