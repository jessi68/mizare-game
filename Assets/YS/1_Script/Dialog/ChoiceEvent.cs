using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    [System.Serializable]
    public class ChoiceEvent : BaseScriptEvent
    {
        [LabelText("선택지"), Tooltip("선택지")]
        public ChoiceData[] choices;

        public override void OnEnter()
        {
            base.OnEnter();

            gm.choiceStruct.Setup(choices);
        }
        protected override void OnUpdate() { }
        public override void OnExit()
        {
            gm.choiceStruct.Release();

            base.OnExit();
        }
    }
}