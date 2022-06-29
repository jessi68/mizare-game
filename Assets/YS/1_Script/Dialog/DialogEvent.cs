using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace YS
{
    [System.Serializable]
    public class DialogEvent : BaseScriptEvent
    {
        [SerializeField]
        private SCREEN_EFFECT screenEffect;

        [Space(10.0f)]

        [SerializeField]
        private CHARACTER_IMAGE_INDEX leftImage;
        [SerializeField]
        private bool leftHighlight;
        [SerializeField]
        private CHARACTER_EFFECT_INDEX leftEffect;
        [SerializeField]
        private CHARACTER_IMAGE_INDEX rightImage;
        [SerializeField]
        private bool rightHighlight;
        [SerializeField]
        private CHARACTER_EFFECT_INDEX rightEffect;

        [Space(10.0f)]

        [SerializeField]
        private string name;
        [SerializeField]
        private string script;
        [SerializeField]
        private uint nextIdx;

        #region Properties
        public SCREEN_EFFECT ScreenEffect => screenEffect;
        public CHARACTER_IMAGE_INDEX LeftImage => leftImage;
        public bool LeftHighlight => leftHighlight;
        public CHARACTER_EFFECT_INDEX LeftEffect => leftEffect;
        public CHARACTER_IMAGE_INDEX RightImage => rightImage;
        public bool RightHighlight => rightHighlight;
        public CHARACTER_EFFECT_INDEX RightEffect => rightEffect;
        public string Name => name;
        public string Script => script;
        public uint NextIdx => nextIdx;
        #endregion

        public override void OnEnter()
        {
            base.OnEnter();

            gm.dialogStruct.dialogUI.SetActive(true);
            gm.SetDialog(this);
        }

        protected override void OnUpdate()
        {
            if (gm.IsKeyDownForDialogEvent())
                gm.OnDialogEvent(this);
        }

        public override void OnExit()
        {
            gm.dialogStruct.dialogUI.SetActive(false);
            gm.ResetEffects();

            base.OnExit();
        }
    }
}
