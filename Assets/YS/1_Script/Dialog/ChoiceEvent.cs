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
        [System.Serializable]
        public struct ChoiceStruct
        {
            public string str;
            public uint nextIdx;
        }

        public ChoiceStruct[] choices;
        private RectTransform[] choicesUI;
        private TMP_Text[] choiceTMPs;

        public override void OnEnter()
        {
            choicesUI = gm.choices;
            choiceTMPs = gm.choiceTMPs;

            float padding = (1 - (choices.Length * 0.15f)) / (choices.Length + 1);
            float height = 1.0f;
            for (int i = 0; i < choices.Length; ++i)
            {
                choiceTMPs[i].SetText(choices[i].str);
                choicesUI[i].gameObject.SetActive(true);
                height -= padding;
                choicesUI[i].anchorMax = new Vector2(1.0f, height);
                height -= 0.15f;
                choicesUI[i].anchorMin = new Vector2(0.0f, height);
            }
        }
        public void OnChooseChoice(int index)
        {
            // 모든 선택지들 비활성화하고
            for (int i = 0; i < choices.Length; ++i)
                choicesUI[i].gameObject.SetActive(false);

            gm.scripts.SetScript(choices[index].nextIdx);
        }
    }
}
