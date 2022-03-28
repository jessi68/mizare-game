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

        #region Field
        public SlideEffect menuPanel;
        public SlideEffect savePanel;

        public Image leftSideImg;
        public Image rightSideImg;

        public TMP_Text nameTMP;
        public CustomTMPEffect scriptTMP;
        public GameObject logUI;
        public TMP_Text logTMP;

        private static SaveData[] saveDatas = new SaveData[3];
        // UI 상태 변수
        private STATE state;
        private int scriptIndex;
        private string log;
        #endregion

        #region Unity Methods
        void Start()
        {
            state = STATE.GAME;
            SetDialog();
        }
        void Update()
        {
            // UI가 켜져있지 않고 UI가 아닌곳에 마우스 클릭이 일어났을때
            if (state == STATE.GAME && Input.GetKeyDown(KeyCode.Mouse0) && !EventSystem.current.IsPointerOverGameObject())
            {
                // 마우스 클릭시 타이핑이 안끝났다면 타이핑 끝내고, 타이핑이 다 되어있는 상태라면 다음 다이얼로그 설정
                if (!scriptTMP.IsDoneTyping)
                    scriptTMP.SkipTyping();
                else
                    SetDialog();
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
        /// (스크립트 인덱스를 하나씩 올리는데 분기에 따라 변화가 필요하다면, 이부분을 이벤트함수에서 처리해야할듯. 만약 파싱해서 사용한다면 파싱할때 다음 인덱스번호를 확인해야할 필요가 있을꺼같습니다)
        /// </summary>
        private void SetDialog()
        {
            log += "<b>" + ScriptData.scripts[scriptIndex].Name + "</b>\n<size=40>" + ScriptData.scripts[scriptIndex].Script + "</size>\n";
            nameTMP.SetText(ScriptData.scripts[scriptIndex].Name);
            scriptTMP.SetText(ScriptData.scripts[scriptIndex].Script);
            ScriptData.scripts[scriptIndex].OnScriptStart?.Invoke();
            ++scriptIndex;
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