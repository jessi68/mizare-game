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

        [Space(10.0f)]

        [BoxGroup("왼쪽 캐릭터", true, true), SerializeField]
        [LabelText("이미지"), Tooltip("캐릭터 이미지")]
        private CHARACTER_IMAGE_INDEX leftImage;
        [BoxGroup("왼쪽 캐릭터"), SerializeField]
        [LabelText("강조 여부"), Tooltip("캐릭터가 화자인가")]
        private bool leftHighlight;
        [BoxGroup("왼쪽 캐릭터"), SerializeField]
        [LabelText("효과"), Tooltip("이미지 효과\nNONE : 효과 없음\nSHAKE_VERTICAL : 이미지 상하 흔들기\nSHAKE_HORIZONTAL : 이미지 좌우 흔들기\nSHAKE_RANDOM : 이미지 무작위 방향으로 흔들기\nBOUNCE : 뛰어오르기")]
        private CHARACTER_EFFECT_INDEX leftEffect;
        [BoxGroup("오른쪽 캐릭터", true, true), SerializeField]
        [LabelText("이미지"), Tooltip("캐릭터 이미지")]
        private CHARACTER_IMAGE_INDEX rightImage;
        [BoxGroup("오른쪽 캐릭터"), SerializeField]
        [LabelText("강조 여부"), Tooltip("캐릭터가 화자인가")]
        private bool rightHighlight;
        [BoxGroup("오른쪽 캐릭터"), SerializeField]
        [LabelText("효과"), Tooltip("이미지 효과\nNONE : 효과 없음\nSHAKE_VERTICAL : 이미지 상하 흔들기\nSHAKE_HORIZONTAL : 이미지 좌우 흔들기\nSHAKE_RANDOM : 이미지 무작위 방향으로 흔들기\nBOUNCE : 뛰어오르기")]
        private CHARACTER_EFFECT_INDEX rightEffect;

        [BoxGroup("다이얼로그 UI", true, true), SerializeField]
        [LabelText("제목"), Tooltip("대화 상자의 이름\n빈칸일 시 이름 칸 UI 숨김")]
        private string name;
        [BoxGroup("다이얼로그 UI"), SerializeField, TextArea]
        [LabelText("스크립트 내용"), Tooltip("대화 상자의 내용\n이름과 내용 모두 빈칸일 시 대화 상자 숨김")]
        private string script;
        [SerializeField, ShowIf("isEvent")]
        [LabelText("이동할 이벤트 위치"), Tooltip("대화 이벤트가 끝난 후 이동할 이벤트 번호")]
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
