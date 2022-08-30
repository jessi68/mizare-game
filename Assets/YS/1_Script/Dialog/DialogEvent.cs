using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    [System.Serializable]
    public class DialogEvent : BaseScriptEvent
    {
        [SerializeField]
        [LabelText("화면 효과"), Tooltip("화면 효과\nNONE : 화면 효과 없음\nFADE_IN : 검은 화면에서 점차 배경 화면으로 전환\nFADE_OUT : 배경 화면에서 점차 검은 화면으로 전환\nRED_FLASH : 화면 빨간색으로 깜빡임")]
        private SCREEN_EFFECT screenEffect;

        [FoldoutGroup("왼쪽 캐릭터"), SerializeField]
        [HideLabel]
        private CharacterStruct leftCharacter;
        [FoldoutGroup("중앙 캐릭터"), SerializeField]
        [HideLabel]
        private CharacterStruct centerCharacter;
        [FoldoutGroup("오른쪽 캐릭터"), SerializeField]
        [HideLabel]
        private CharacterStruct rightCharacter;

        [BoxGroup("다이얼로그 UI", true, true), SerializeField]
        [LabelText("제목"), Tooltip("대화 상자의 이름\n빈칸일 시 이름 칸 UI 숨김")]
        private string name;
        [BoxGroup("다이얼로그 UI"), SerializeField, TextArea]
        [LabelText("스크립트 내용"), Tooltip("대화 상자의 내용\n이름과 내용 모두 빈칸일 시 대화 상자 숨김")]
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
            [LabelText("이미지"), Tooltip("캐릭터 이미지")]
            public Sprite image;
            [SerializeField, DisableIf("@image == null")]
            [LabelText("좌우 반전"), Tooltip("캐릭터 이미지를 좌우 반전 시킬것인가")]
            public bool isMirror;
            [SerializeField, DisableIf("@image == null")]
            [LabelText("강조 여부"), Tooltip("캐릭터가 화자인가")]
            public bool isHighlight;
            [SerializeField, DisableIf("@image == null")]
            [LabelText("효과"), Tooltip("이미지 효과\nNONE : 효과 없음\nSHAKE_VERTICAL : 이미지 상하 흔들기\nSHAKE_HORIZONTAL : 이미지 좌우 흔들기\nSHAKE_RANDOM : 이미지 무작위 방향으로 흔들기\nBOUNCE : 뛰어오르기")]
            public CHARACTER_EFFECT_INDEX effect;
        }
    }
}