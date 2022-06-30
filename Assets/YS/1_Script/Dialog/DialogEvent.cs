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
        [Tooltip("ȭ�� ȿ��\nNONE : ȭ�� ȿ�� ����\nFADE_IN : ���� ȭ�鿡�� ���� ��� ȭ������ ��ȯ\nFADE_OUT : ��� ȭ�鿡�� ���� ���� ȭ������ ��ȯ\nRED_FLASH : ȭ�� ���������� ������")]
        private SCREEN_EFFECT screenEffect;

        [Space(10.0f)]

        [SerializeField]
        [Tooltip("���ʿ� ��Ÿ�� ĳ���� �̹���")]
        private CHARACTER_IMAGE_INDEX leftImage;
        [SerializeField]
        [Tooltip("���� ĳ���Ͱ� ȭ���ΰ�")]
        private bool leftHighlight;
        [SerializeField]
        [Tooltip("���� ĳ���� �̹��� ȿ��\nNONE : ȿ�� ����\nSHAKE_VERTICAL : �̹��� ���� ����\nSHAKE_HORIZONTAL : �̹��� �¿� ����\nSHAKE_RANDOM : �̹��� ������ �������� ����\nBOUNCE : �پ������")]
        private CHARACTER_EFFECT_INDEX leftEffect;
        [SerializeField]
        [Tooltip("�����ʿ� ��Ÿ�� ĳ���� �̹���")]
        private CHARACTER_IMAGE_INDEX rightImage;
        [SerializeField]
        [Tooltip("������ ĳ���Ͱ� ȭ���ΰ�")]
        private bool rightHighlight;
        [SerializeField]
        [Tooltip("������ ĳ���� �̹��� ȿ��\nNONE : ȿ�� ����\nSHAKE_VERTICAL : �̹��� ���� ����\nSHAKE_HORIZONTAL : �̹��� �¿� ����\nSHAKE_RANDOM : �̹��� ������ �������� ����\nBOUNCE : �پ������")]
        private CHARACTER_EFFECT_INDEX rightEffect;

        [Space(10.0f)]

        [SerializeField]
        [Tooltip("��ȭ ������ �̸�\n��ĭ�� �� �̸� ĭ UI ����")]
        private string name;
        [SerializeField]
        [Tooltip("��ȭ ������ ����\n�̸��� ���� ��� ��ĭ�� �� ��ȭ ���� ����")]
        private string script;
        [SerializeField]
        [Tooltip("��ȭ �̺�Ʈ�� ���� �� �̵��� �̺�Ʈ ��ȣ")]
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
