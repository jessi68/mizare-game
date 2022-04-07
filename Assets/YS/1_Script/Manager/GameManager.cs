using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace YS
{
    public struct SaveData
    {
        public int scriptIndex;
    }

    public class GameManager : Singleton<GameManager>
    {
        enum SIDE_IMAGE
        {
            LEFT_SIDE,
            RIGHT_SIDE
        }

        #region Field
        public RectTransform[] choices;
        private TMP_Text[] choiceTMPs;
        public Material bgMtrl;

        public Image[] sideImg = new Image[2];

        public TMP_Text nameTMP;
        public CustomTMPEffect scriptTMP;
        public TMP_Text logTMP;

        public ScriptData scripts;

        
        private Sprite[] charImgs = new Sprite[(int)CHARACTER_IMAGE_INDEX.MAX];
        // sideImg의 초기 위치 (FX초기화 할 때 사용)
        private Vector3[] sidePos = new Vector3[2];
        // sideFX 코루틴 정보 (스킵 시 사용)
        private Coroutine[] sideFXCoroutine = new Coroutine[2];
        private Coroutine bgFXCoroutine;

        private uint scriptIndex;
        private StringBuilder log = new StringBuilder();
        #endregion

        #region Unity Methods
        protected override void Awake()
        {
            base.Awake();

            // 초기화를 위해 처음의 Left, Right 사이드 이미지의 위치 얻기
            for (int i = 0; i < 2; ++i)
                sidePos[i] = sideImg[i].transform.position;

            choiceTMPs = new TMP_Text[choices.Length];
            for (int i = 0; i < choices.Length; ++i)
                choiceTMPs[i] = choices[i].GetChild(0).GetComponent<TMP_Text>();

            // 사용할 캐릭터들 이미지 로딩
            charImgs[(int)CHARACTER_IMAGE_INDEX.MIZAR] = ResourceManager.GetResource<Sprite>("image001");
            charImgs[(int)CHARACTER_IMAGE_INDEX.ALCOR] = ResourceManager.GetResource<Sprite>("image018");
        }
        void Start()
        {
            // 나중에 로드시 로드한 index값으로 설정
            SetDialog(0);
        }
        void Update()
        {
            if (IsKeyDownForDialogEvent())
                OnDialogEvent();
        }
        #endregion

        #region Methods
        /// <summary>
        /// 게임 저장
        /// </summary>
        private void SaveGame()
        {

        }
        /// <summary>
        /// 게임 불러오기
        /// </summary>
        private void LoadGame()
        {

        }
        /// <summary>
        /// 다이얼로그 이벤트가 발생했는가
        /// </summary>
        /// <returns>발생했다면 true</returns>
        private bool IsKeyDownForDialogEvent()
        {
            bool result;

                     // GameState이고
            result = InGameUIManager.IsGameState() &&
                     // 스페이스 키가 눌렸거나
                     Input.GetKeyDown(KeyCode.Space) ||
                     // UI가 아닌곳에 마우스 클릭 이벤트가 발생했을때
                     (Input.GetKeyDown(KeyCode.Mouse0) && !EventSystem.current.IsPointerOverGameObject());

            return result;
        }
        /// <summary>
        /// 다이얼로그 이벤트 발생시 호출되는 함수
        /// </summary>
        private void OnDialogEvent()
        {
            // 마우스 클릭시 타이핑이 안끝났다면 타이핑 끝내고, 타이핑이 다 되어있는 상태라면 다음 다이얼로그 설정
            if (!scriptTMP.IsDoneTyping)
            {
                ResetEffects();
                scriptTMP.SkipTyping();
            }
            else
            {
                if (scripts[scriptIndex].choices.Length == 0)
                    SetDialog(scripts[scriptIndex].nextIdx);
                else
                {
                    float padding = (1 - (scripts[scriptIndex].choices.Length * 0.15f)) / (scripts[scriptIndex].choices.Length + 1);
                    float height = 1.0f;
                    for (int i = 0; i < scripts[scriptIndex].choices.Length; ++i)
                    {
                        choiceTMPs[i].SetText(scripts[scriptIndex].choices[i].str);
                        choices[i].gameObject.SetActive(true);
                        height -= padding;
                        choices[i].anchorMax = new Vector2(1.0f, height);
                        height -= 0.15f;
                        choices[i].anchorMin = new Vector2(0.0f, height);
                    }
                }
            }
        }
        /// <summary>
        /// 다이얼로그 설정
        /// </summary>
        private void SetDialog(uint index)
        {
            scriptIndex = index;

            DialogScript data = scripts[scriptIndex];

            ResetEffects();

            log.Append("<b>");
            log.Append(data.name);
            log.Append("</b>\n<size=40>");
            log.Append(data.script);
            log.Append("</size>\n");

            logTMP.SetText(log);
            nameTMP.SetText(data.name);
            scriptTMP.SetText(data.script);

            ScreenEffect(data.screenEffect);
            SetCharSetting(SIDE_IMAGE.LEFT_SIDE, data.leftImage, data.leftHighlight, data.leftEffect);
            SetCharSetting(SIDE_IMAGE.RIGHT_SIDE, data.rightImage, data.rightHighlight, data.rightEffect);
        }
        /// <summary>
        /// 선택지 고르면 호출되는 이벤트 함수
        /// </summary>
        /// <param name="index">선택지 번호</param>
        public void OnChooseChoice(int index)
        {
            // 모든 선택지들 비활성화하고
            for (int i = 0; i < scripts[scriptIndex].choices.Length; ++i)
                choices[i].gameObject.SetActive(false);

            // 선택된 선택지의 index로 Dialog이동
            SetDialog(scripts[scriptIndex].choices[index].nextIdx);
        }
        /// <summary>
        /// 화면 효과
        /// </summary>
        /// <param name="screenFX">적용할 효과</param>
        private void ScreenEffect(SCREEN_EFFECT screenFX)
        {
            switch (screenFX)
            {
                case SCREEN_EFFECT.FADE_IN:
                    bgFXCoroutine = StartCoroutine(FadeEffect(true, 1.0f));
                    break;
                case SCREEN_EFFECT.FADE_OUT:
                    bgFXCoroutine = StartCoroutine(FadeEffect(false, 1.0f));
                    break;
                case SCREEN_EFFECT.RED_FLASH:
                    bgMtrl.SetColor("_AddColor", Color.red);
                    Invoke("ResetFlash", 0.25f);
                    break;
            }
        }
        /// <summary>
        /// 캐릭터 이미지 설정
        /// </summary>
        /// <param name="side">왼쪽 사이드인지 오른쪽 사이드인지 설정</param>
        /// <param name="charImgIdx">보여줄 이미지</param>
        /// <param name="isHighlight">하이라이트 여부</param>
        /// <param name="charFX">사이드 이미지에 줄 효과</param>
        private void SetCharSetting(SIDE_IMAGE side, CHARACTER_IMAGE_INDEX charImgIdx, bool isHighlight, CHARACTER_EFFECT_INDEX charFX)
        {
            sideImg[(int)side].sprite = charImgs[(int)charImgIdx];
            sideImg[(int)side].color = isHighlight ? Color.white : Color.gray;

            switch (charFX)
            {
                case CHARACTER_EFFECT_INDEX.SHAKE_HORIZONTAL:
                case CHARACTER_EFFECT_INDEX.SHAKE_VERTICAL:
                case CHARACTER_EFFECT_INDEX.SHAKE_RANDOM:
                    sideFXCoroutine[(int)side] = StartCoroutine(ShakeEffect(sideImg[(int)side].gameObject.transform, 5, 0.5f, 0.01f, charFX));
                    break;

                case CHARACTER_EFFECT_INDEX.BOUNCE:
                    sideFXCoroutine[(int)side] = StartCoroutine(BounceEffect(sideImg[(int)side].gameObject.transform, 3.0f));
                    break;
            }
        }
        private IEnumerator ShakeEffect(Transform target, float intensity, float time, float intervalTime, CHARACTER_EFFECT_INDEX type)
        {
            WaitForSeconds interval = CachedWaitForSeconds.Get(intervalTime);

            Vector3 curShakeVector = Vector3.zero;
            Vector3 dir = new Vector3(0.0f, 0.0f, 0.0f);
            float remainingTime = time;
            float curIntensity;

            switch (type)
            {
                case CHARACTER_EFFECT_INDEX.SHAKE_VERTICAL:
                    dir.x = 1.0f;
                    break;
                case CHARACTER_EFFECT_INDEX.SHAKE_HORIZONTAL:
                    dir.y = 1.0f;
                    break;
            }

            while (remainingTime > 0.0f)
            {
                if (type == CHARACTER_EFFECT_INDEX.SHAKE_RANDOM)
                    dir = Quaternion.AngleAxis(Random.Range(0.0f, 360.0f), Vector3.forward) * Vector3.right;
                else
                    dir = -dir;

                curIntensity = intensity * (remainingTime / time);

                target.position -= curShakeVector;
                curShakeVector = dir * curIntensity;
                target.position += curShakeVector;

                remainingTime -= intervalTime;
                yield return interval;
            }

            target.position -= curShakeVector;
        }
        private IEnumerator BounceEffect(Transform target, float time)
        {
            float t = 0.0f;
            WaitForSeconds wf = CachedWaitForSeconds.Get(0.01f);
            Bezier bezier = new Bezier();
            bezier.bezierPos = new Vector3[3]
            {
                target.position,
                target.position + Vector3.up * 100.0f,
                target.position
            };

            while (t <= 1.0f)
            {
                t += time * 0.01f;
                target.position = bezier.GetBezierPosition(t);
                yield return wf;
            }
        }
        private IEnumerator FadeEffect(bool isIn, float time)
        {
            WaitForSeconds wf = CachedWaitForSeconds.Get(0.01f);
            float curTime = 0.0f;

            bgMtrl.SetFloat("_IsIn", isIn ? 1.0f : 0.0f);
            
            while (curTime < time)
            {
                bgMtrl.SetFloat("_CurTime", curTime / time);
                yield return wf;
                curTime += 0.01f;
            }
        }
        private void ResetFlash()
        {
            bgMtrl.SetColor("_AddColor", Vector4.zero);
        }
        /// <summary>
        /// 효과 중간에 스킵될 수 있으므로, 효과들이 재생중이라면 종료시키고 이미지들의 위치 원상태로 복구하는 함수
        /// </summary>
        private void ResetEffects()
        {
            // 작동중인 효과 코루틴 멈추기
            if (sideFXCoroutine[0] != null) StopCoroutine(sideFXCoroutine[0]);
            if (sideFXCoroutine[1] != null) StopCoroutine(sideFXCoroutine[1]);
            if (bgFXCoroutine != null)      StopCoroutine(bgFXCoroutine);

            for (int i = 0; i < 2; ++i)
                sideImg[i].transform.position = sidePos[i];

            bgMtrl.SetFloat("_CurTime", 1.0f);
            ResetFlash();
        }
        #endregion
    }
}