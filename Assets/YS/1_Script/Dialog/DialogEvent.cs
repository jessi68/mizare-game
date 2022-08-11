using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    [System.Serializable]
    public class DialogEvent : BaseScriptEvent
    {
        [SerializeField]
        [LabelText("ȭ�� ȿ��"), Tooltip("ȭ�� ȿ��\nNONE : ȭ�� ȿ�� ����\nFADE_IN : ���� ȭ�鿡�� ���� ��� ȭ������ ��ȯ\nFADE_OUT : ��� ȭ�鿡�� ���� ���� ȭ������ ��ȯ\nRED_FLASH : ȭ�� ���������� ������")]
        private SCREEN_EFFECT screenEffect;

        [Space(10.0f)]

        [BoxGroup("���� ĳ����", true, true), SerializeField]
        [LabelText("�̹���"), Tooltip("ĳ���� �̹���")]
        private CHARACTER_IMAGE_INDEX leftImage;
        [BoxGroup("���� ĳ����"), SerializeField]
        [LabelText("���� ����"), Tooltip("ĳ���Ͱ� ȭ���ΰ�")]
        private bool leftHighlight;
        [BoxGroup("���� ĳ����"), SerializeField]
        [LabelText("ȿ��"), Tooltip("�̹��� ȿ��\nNONE : ȿ�� ����\nSHAKE_VERTICAL : �̹��� ���� ����\nSHAKE_HORIZONTAL : �̹��� �¿� ����\nSHAKE_RANDOM : �̹��� ������ �������� ����\nBOUNCE : �پ������")]
        private CHARACTER_EFFECT_INDEX leftEffect;
        [BoxGroup("������ ĳ����", true, true), SerializeField]
        [LabelText("�̹���"), Tooltip("ĳ���� �̹���")]
        private CHARACTER_IMAGE_INDEX rightImage;
        [BoxGroup("������ ĳ����"), SerializeField]
        [LabelText("���� ����"), Tooltip("ĳ���Ͱ� ȭ���ΰ�")]
        private bool rightHighlight;
        [BoxGroup("������ ĳ����"), SerializeField]
        [LabelText("ȿ��"), Tooltip("�̹��� ȿ��\nNONE : ȿ�� ����\nSHAKE_VERTICAL : �̹��� ���� ����\nSHAKE_HORIZONTAL : �̹��� �¿� ����\nSHAKE_RANDOM : �̹��� ������ �������� ����\nBOUNCE : �پ������")]
        private CHARACTER_EFFECT_INDEX rightEffect;

        [BoxGroup("���̾�α� UI", true, true), SerializeField]
        [LabelText("����"), Tooltip("��ȭ ������ �̸�\n��ĭ�� �� �̸� ĭ UI ����")]
        private string name;
        [BoxGroup("���̾�α� UI"), SerializeField, TextArea]
        [LabelText("��ũ��Ʈ ����"), Tooltip("��ȭ ������ ����\n�̸��� ���� ��� ��ĭ�� �� ��ȭ ���� ����")]
        private string script;
        [SerializeField, ShowIf("isEvent")]
        [LabelText("�̵��� �̺�Ʈ ��ġ"), Tooltip("��ȭ �̺�Ʈ�� ���� �� �̵��� �̺�Ʈ ��ȣ")]
        private int nextIdx;

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
        public int NextIdx => nextIdx;
        #endregion

        public override void OnEnter()
        {
            base.OnEnter();

            gm.dialogStruct.Setup(this);
        }

        protected override void OnUpdate()
        {
            if (gm.IsKeyDown())
                gm.dialogStruct.OnDialogEvent(this);
        }

        public override void OnExit()
        {
            gm.dialogStruct.Release();

            base.OnExit();
        }

#if UNITY_EDITOR
        private bool isEvent = true;
        public DialogEvent()
        {
            isEvent = true;
        }
        public DialogEvent(bool isEvent)
        {
            this.isEvent = isEvent;
        }
#endif
    }
}
