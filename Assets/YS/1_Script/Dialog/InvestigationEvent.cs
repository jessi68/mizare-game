using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YS
{
    public class InvestigationEvent : BaseScriptEvent
    {
        public GameObject bgPrefab;
        public uint nextIndex;

        public override void OnEnter()
        {
            base.OnEnter();

            gm.ChangeBackground(GameObject.Instantiate(bgPrefab));

            gm.ivStruct.items = new Item[gm.bgUI.transform.childCount];
            for (int i = 0; i < gm.ivStruct.items.Length; ++i)
            {
                gm.ivStruct.items[i] = gm.bgUI.transform.GetChild(i).GetComponent<Item>();
                gm.ivStruct.items[i].image.raycastTarget = true;
            }

            gm.ivStruct.findCount = gm.ivStruct.items.Length;

            gm.bgMtrl.SetFloat("_IsIn", 0.0f);

            

            gm.ivStruct.investigationUI.SetActive(true);
            gm.ivStruct.SetInvestigationMode();
        }
        protected override void OnUpdate()
        {
            if (gm.IsKeyDownForDialogEvent())
            {
                if (gm.ivStruct.isInChooseResult)
                {
                    if (gm.ivStruct.inferenceDialogTMP.IsDoneTyping)
                    {
                        if (gm.itemData[gm.ivStruct.items[gm.ivStruct.curItemIndex].index].correctIndex == gm.ivStruct.choicedIndex || --gm.ivStruct.retryCount == 0)
                        {
                            if (gm.ivStruct.items.Length == ++gm.ivStruct.curItemIndex)
                                gm.scriptData.SetScript();
                            else
                                gm.ivStruct.SetChoicePanel();
                        }
                    }
                    else
                        gm.ivStruct.inferenceDialogTMP.SkipTyping();
                }
                else if (!gm.ivStruct.choiceItemDescTMP.IsDoneTyping)
                        gm.ivStruct.choiceItemDescTMP.SkipTyping();
            }
        }
        public override void OnExit()
        {
            gm.ivStruct.investigationUI.SetActive(false);

            base.OnExit();
        }
    }
}