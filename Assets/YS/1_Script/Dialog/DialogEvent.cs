using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace YS
{
    [System.Serializable]
    public abstract class DialogEvent : BaseScriptEvent
    {
        public SCREEN_EFFECT screenEffect;

        [Space(10.0f)]

        public CHARACTER_IMAGE_INDEX leftImage;
        public bool leftHighlight;
        public CHARACTER_EFFECT_INDEX leftEffect;
        public CHARACTER_IMAGE_INDEX rightImage;
        public bool rightHighlight;
        public CHARACTER_EFFECT_INDEX rightEffect;

        [Space(10.0f)]

        public string name;
        public string script;
        public uint nextIdx;

        public override void OnEnter()
        {
            base.OnEnter();

            gm.SetDialog(this);
        }

        protected override void OnUpdate()
        {
            if (IsKeyDownForDialogEvent())
                gm.OnDialogEvent(this);
        }
        public override void OnExit()
        {
            base.OnExit();

        }

        /// <summary>
        /// ���̾�α� �̺�Ʈ�� �߻��ߴ°�
        /// </summary>
        /// <returns>�߻��ߴٸ� true</returns>
        private bool IsKeyDownForDialogEvent()
        {
            bool result;

            // GameState�̰�
            result = InGameUIManager.IsGameState() &&
                     // �����̽� Ű�� ���Ȱų�
                     Input.GetKeyDown(KeyCode.Space) ||
                     // UI�� �ƴѰ��� ���콺 Ŭ�� �̺�Ʈ�� �߻�������
                     (Input.GetKeyDown(KeyCode.Mouse0) && !EventSystem.current.IsPointerOverGameObject());

            return result;
        }
    }
}
