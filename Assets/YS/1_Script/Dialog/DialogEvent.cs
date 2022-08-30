using UnityEngine;
using UnityEngine.UI;
using TMPro;
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
    [System.Serializable]
    public struct DialogStruct
    {
        #region Fields
        [FoldoutGroup("다이얼로그 UI", false)]
        [LabelText("다이얼로그 패널 UI"), Tooltip("다이얼로그 루트 게임오브젝트")]
        public GameObject dialogUI;
        [FoldoutGroup("다이얼로그 UI"), ListDrawerSettings(HideAddButton = true, HideRemoveButton = true)]
        [LabelText("이미지 위치"), Tooltip("다이얼로그 왼쪽/중앙/오른쪽 Image 컴포넌트")]
        public Image[] sideImg;
        [FoldoutGroup("다이얼로그 UI")]
        [LabelText("이름 TMP"), Tooltip("다이얼로그 창 이름 TMP")]
        public TMP_Text nameTMP;
        [FoldoutGroup("다이얼로그 UI")]
        [LabelText("스크립트 TMP"), Tooltip("다이얼로그 창 스크립트 TMP")]
        public CustomTMPEffect scriptTMP;
        [FoldoutGroup("다이얼로그 UI/캐릭터 떨림 효과", false)]
        [LabelText("강도"), Tooltip("캐릭터 떨림의 강도")]
        public float shakeIntensity;
        [FoldoutGroup("다이얼로그 UI/캐릭터 떨림 효과", false)]
        [LabelText("지속 시간"), Tooltip("캐릭터 떨림 효과 시간")]
        public float shakeTime;
        [FoldoutGroup("다이얼로그 UI/캐릭터 떨림 효과", false)]
        [LabelText("떨림 간격"), Tooltip("캐릭터가 얼만큼의 시간간격으로 떨릴지에 대한 수치")]
        public float shakeIntervalTime;
        [FoldoutGroup("다이얼로그 UI/캐릭터 바운스 효과", false)]
        [LabelText("지속 시간"), Tooltip("캐릭터가 뛰는 시간\n'1 / 지속시간'sec\nex)1 = 1sec, 2 = 0.5sec, 0.5 = 2sec")]
        public float bounceTime;
        [FoldoutGroup("다이얼로그 UI/캐릭터 바운스 효과", false)]
        [LabelText("높이"), Tooltip("캐릭터가 뛰어오르는 높이")]
        public float bounceHeight;

        // sideImg의 초기 위치 (FX초기화 할 때 사용)
        private Vector3[] sidePos;
        // sideFX 코루틴 정보 (스킵 시 사용)
        private Coroutine[] sideFXCoroutine;
        private GameManager gm;
        #endregion

        #region Properties
        public Vector3[] SidePosition => sidePos;
        #endregion

        #region Methods
        public void Initialize()
        {
            gm = GameManager.Instance;

            sidePos = new Vector3[3];
            // 초기화를 위해 처음의 Left, Right 사이드 이미지의 위치 얻기
            for (int i = 0; i < 3; ++i)
                sidePos[i] = sideImg[i].transform.position;

            sideFXCoroutine = new Coroutine[2];
        }
        /// <summary>
        /// 다이얼로그 설정
        /// </summary>
        public void Setup(DialogEvent de)
        {
            dialogUI.SetActive(true);
            gm.ScreenEffect(de.ScreenEffect);

            if (de.Name == null || de.Name == "")
            {
                if (de.Script == null || de.Script == "")
                {
                    dialogUI.SetActive(false);
                    return;
                }

                nameTMP.transform.parent.gameObject.SetActive(false);
            }
            else
                nameTMP.transform.parent.gameObject.SetActive(true);

            gm.Logging($"<b>{de.Name}</b>\n<size=40>{de.Script}</size>\n");

            nameTMP.SetText(de.Name);
            scriptTMP.SetText(de.Script);

            SetCharSetting(SIDE_IMAGE.LEFT_SIDE, de.LeftCharacter);
            SetCharSetting(SIDE_IMAGE.CENTER_SIDE, de.CenterCharacter);
            SetCharSetting(SIDE_IMAGE.RIGHT_SIDE, de.RightCharacter);
        }
        public void Release()
        {
            dialogUI.SetActive(false);
            ResetEffects();
        }
        /// <summary>
        /// 다이얼로그 이벤트 발생시 호출되는 함수
        /// </summary>
        public void OnDialogEvent(DialogEvent de)
        {
            // 마우스 클릭시 타이핑이 안끝났다면 타이핑 끝내고, 타이핑이 다 되어있는 상태라면 다음 다이얼로그 설정
            if (!scriptTMP.IsDoneTyping)
                scriptTMP.SkipTyping();
            else
                gm.scriptData.SetScript(gm.scriptData.CurrentIndex + 1);
        }
        /// <summary>
        /// 캐릭터 이미지 설정
        /// </summary>
        /// <param name="side">왼쪽 사이드인지 오른쪽 사이드인지 설정</param>
        /// <param name="charImgIdx">보여줄 이미지</param>
        /// <param name="isHighlight">하이라이트 여부</param>
        /// <param name="charFX">사이드 이미지에 줄 효과</param>
        private void SetCharSetting(SIDE_IMAGE side, DialogEvent.CharacterStruct character)
        {
            sideImg[(int)side].sprite = character.image;
            sideImg[(int)side].transform.localScale = new Vector3(character.isMirror ? -1.0f : 1.0f, 1.0f, 1.0f);

            if (character.image == null)
                sideImg[(int)side].color = Color.clear;
            else
            {
                sideImg[(int)side].color = character.isHighlight ? Color.white : Color.gray;
                sideImg[(int)side].SetNativeSize();
            }

            switch (character.effect)
            {
                case CHARACTER_EFFECT_INDEX.SHAKE_HORIZONTAL:
                case CHARACTER_EFFECT_INDEX.SHAKE_VERTICAL:
                case CHARACTER_EFFECT_INDEX.SHAKE_RANDOM:
                    sideFXCoroutine[(int)side] = gm.StartCoroutine(gm.ShakeEffect(sideImg[(int)side].gameObject.transform, shakeIntensity, shakeTime, shakeIntervalTime, character.effect));
                    break;

                case CHARACTER_EFFECT_INDEX.BOUNCE:
                    sideFXCoroutine[(int)side] = gm.StartCoroutine(gm.BounceEffect(sideImg[(int)side].gameObject.transform, bounceTime, bounceHeight));
                    break;
            }
        }
        /// <summary>
        /// 효과 중간에 스킵될 수 있으므로, 효과들이 재생중이라면 종료시키고 이미지들의 위치 원상태로 복구하는 함수
        /// </summary>
        private void ResetEffects()
        {
            // 작동중인 효과 코루틴 멈추기
            if (sideFXCoroutine[0] != null) gm.StopCoroutine(sideFXCoroutine[0]);
            if (sideFXCoroutine[1] != null) gm.StopCoroutine(sideFXCoroutine[1]);
            if (gm.bgFXCoroutine != null) gm.StopCoroutine(gm.bgFXCoroutine);

            for (int i = 0; i < 3; ++i)
                sideImg[i].transform.position = SidePosition[i];

            gm.SetBGCurTime(1.0f);
            gm.ResetFlash();
        }
        #endregion
    }
    public enum SIDE_IMAGE
    {
        LEFT_SIDE,
        CENTER_SIDE,
        RIGHT_SIDE
    }
    public enum SCREEN_EFFECT
    {
        NONE,
        FADE_IN,
        FADE_OUT,
        RED_FLASH,
    }
    public enum CHARACTER_EFFECT_INDEX
    {
        NONE,
        SHAKE_VERTICAL,
        SHAKE_HORIZONTAL,
        SHAKE_RANDOM,
        BOUNCE
    }
}