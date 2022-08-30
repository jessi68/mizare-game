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

        [FoldoutGroup("���� ĳ����"), SerializeField]
        [HideLabel]
        private CharacterStruct leftCharacter;
        [FoldoutGroup("�߾� ĳ����"), SerializeField]
        [HideLabel]
        private CharacterStruct centerCharacter;
        [FoldoutGroup("������ ĳ����"), SerializeField]
        [HideLabel]
        private CharacterStruct rightCharacter;

        [BoxGroup("���̾�α� UI", true, true), SerializeField]
        [LabelText("����"), Tooltip("��ȭ ������ �̸�\n��ĭ�� �� �̸� ĭ UI ����")]
        private string name;
        [BoxGroup("���̾�α� UI"), SerializeField, TextArea]
        [LabelText("��ũ��Ʈ ����"), Tooltip("��ȭ ������ ����\n�̸��� ���� ��� ��ĭ�� �� ��ȭ ���� ����")]
        private string script;

        #region Properties
        public SCREEN_EFFECT ScreenEffect => screenEffect;
        public CharacterStruct LeftCharacter => leftCharacter;
        public CharacterStruct CenterCharacter => centerCharacter;
        public CharacterStruct RightCharacter => rightCharacter;
        public string Name => name;
        public string Script => script;
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
        [System.Serializable]
        public struct CharacterStruct
        {
            [SerializeField]
            [LabelText("�̹���"), Tooltip("ĳ���� �̹���")]
            public Sprite image;
            [SerializeField, DisableIf("@image == null")]
            [LabelText("�¿� ����"), Tooltip("ĳ���� �̹����� �¿� ���� ��ų���ΰ�")]
            public bool isMirror;
            [SerializeField, DisableIf("@image == null")]
            [LabelText("���� ����"), Tooltip("ĳ���Ͱ� ȭ���ΰ�")]
            public bool isHighlight;
            [SerializeField, DisableIf("@image == null")]
            [LabelText("ȿ��"), Tooltip("�̹��� ȿ��\nNONE : ȿ�� ����\nSHAKE_VERTICAL : �̹��� ���� ����\nSHAKE_HORIZONTAL : �̹��� �¿� ����\nSHAKE_RANDOM : �̹��� ������ �������� ����\nBOUNCE : �پ������")]
            public CHARACTER_EFFECT_INDEX effect;
        }
    }
}