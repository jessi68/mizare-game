using System.Collections;
using System.Collections.Generic;
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
        enum STATE
        {
            GAME,
            MENU,
            SAVE,
            LOAD,
            GALLERY,
            LOG
        }
        enum SIDE_IMAGE
        {
            LEFT_SIDE,
            RIGHT_SIDE
        }

        #region Field
        public SlideEffect menuPanel;
        public SlideEffect savePanel;
        public GameObject logUI;
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

        // UI 상태 변수
        private STATE state;
        private uint scriptIndex;
        private string log;
        #endregion

        #region Unity Methods
        protected override void Awake()
        {
            base.Awake();

            for (int i = 0; i < 2; ++i)
                sidePos[i] = sideImg[i].transform.position;

            charImgs[(int)CHARACTER_IMAGE_INDEX.MIZAR] = ResourceManager.GetResource<Sprite>("image001");
            charImgs[(int)CHARACTER_IMAGE_INDEX.ALCOR] = ResourceManager.GetResource<Sprite>("image018");
        }
        void Start()
        {
            state = STATE.GAME;
            SetDialog(0);
        }
        void Update()
        {
            // UI가 켜져있지 않고 UI가 아닌곳에 마우스 클릭이 일어났을때
            if (state == STATE.GAME && Input.GetKeyDown(KeyCode.Mouse0) && !EventSystem.current.IsPointerOverGameObject())
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

                    }
                }
            }

            // esc키 눌리면 UI 뒤로가기
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                switch (state)
                {
                    case STATE.GAME:
                        ShowMenu();
                        break;
                    case STATE.MENU:
                        CloseMenu();
                        break;
                    case STATE.SAVE:
                        CloseSave();
                        break;
                    case STATE.LOAD:
                        CloseLoad();
                        break;
                    case STATE.GALLERY:
                        CloseGallery();
                        break;
                    case STATE.LOG:
                        CloseLog();
                        break;
                }
            }
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
        /// 다이얼로그 설정
        /// </summary>
        private void SetDialog(uint index)
        {
            scriptIndex = index;

            DialogScript data = scripts[scriptIndex];

            ResetEffects();

            log += "<b>" + data.name + "</b>\n<size=40>" + data.script + "</size>\n";
            nameTMP.SetText(data.name);
            scriptTMP.SetText(data.script);

            ScreenEffect(data.screenEffect);
            SetCharSetting(SIDE_IMAGE.LEFT_SIDE, data.leftImage, data.leftHighlight, data.leftEffect);
            SetCharSetting(SIDE_IMAGE.RIGHT_SIDE, data.rightImage, data.rightHighlight, data.rightEffect);
        }
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
        /// <summary>
        /// UI상태 전단계로 가기
        /// (UI 스택이 최대 2개일거같아서 그런 경우 전 상태로 되돌아가게끔 작성했는데, 추후에 더 깊고 복잡하게 들어가서 저장해야하는경우 스택자료구조 사용해서 복구해야할거 같습니다)
        /// </summary>
        private void PopUIState()
        {
            switch (state)
            {
                case STATE.MENU:
                    CloseMenu();
                    break;
                case STATE.SAVE:
                    CloseSave();
                    break;
                case STATE.LOAD:
                    CloseLoad();
                    break;
                case STATE.GALLERY:
                    CloseGallery();
                    break;
                case STATE.LOG:
                    CloseLog();
                    break;
            }
        }
        #region State Methods
        /// <summary>
        /// 메뉴 버튼 클릭시 호출
        /// </summary>
        public void OnClickMenuBtn()
        {
            switch (state)
            {
                case STATE.GAME:
                    ShowMenu();
                    return;
                case STATE.SAVE:
                    CloseSave();
                    break;
                case STATE.LOAD:
                    CloseLoad();
                    break;
                case STATE.GALLERY:
                    CloseGallery();
                    break;
                case STATE.LOG:
                    CloseLog();
                    break;
            }

            CloseMenu();
        }
        // 아래 전부 버튼 클릭시 해당 버튼에 해당하는 상호작용함수들
        public void ShowMenu()
        {
            state = STATE.MENU;
            menuPanel.SetSlide(Vector3.zero);
        }
        public void CloseMenu()
        {
            state = STATE.GAME;
            menuPanel.SetSlide(new Vector2(-500.0f, 0.0f));
        }
        public void ShowSave()
        {
            if (state != STATE.MENU)
                PopUIState();
            state = STATE.SAVE;
            savePanel.SetSlide(new Vector2(0.0f, -340.0f));
        }
        public void CloseSave()
        {
            state = STATE.MENU;
            savePanel.SetSlide(new Vector2(-500.0f, -340.0f));
        }
        public void ShowLoad()
        {
            if (state != STATE.MENU)
                PopUIState();
            state = STATE.LOAD;
            savePanel.SetSlide(new Vector2(0.0f, -340.0f));
        }
        public void CloseLoad()
        {
            state = STATE.MENU;
            savePanel.SetSlide(new Vector2(-500.0f, -340.0f));
        }
        public void ShowGallery()
        {
            if (state != STATE.MENU)
                PopUIState();
            state = STATE.GALLERY;
        }
        public void CloseGallery()
        {
            state = STATE.MENU;
        }
        public void ShowLog()
        {
            if (state != STATE.MENU)
                PopUIState();
            state = STATE.LOG;
            logTMP.SetText(log);
            logUI.SetActive(true);
        }
        public void CloseLog()
        {
            state = STATE.MENU;
            logUI.SetActive(false);
        }
        public void ExitGame()
        {
            // 타이틀 씬으로 돌아가게 구현
            // 필요하다면 저장할건지 물어보는것도 생각해야할듯
        }
        #endregion
        #endregion
    }
}