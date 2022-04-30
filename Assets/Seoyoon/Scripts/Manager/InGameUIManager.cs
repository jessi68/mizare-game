using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Seoyoon
{
    public class InGameUIManager : YS.Singleton<InGameUIManager>
    {
        enum STATE
        {
            MENU,
            SAVE,
            LOAD,
            SETTING,
            LOG,
            EXIT
        }
        #region Field
        public Button menuBtn;
        public Button saveBtn;
        public Button loadBtn;
        public Button settingBtn;
        public Button logBtn;
        public Button exitBtn;

        [Space(10.0f)]

        public YS.SlideEffect menuPanel;
        public YS.SlideEffect savePanel;
        public GameObject logPanel;

        // UI 상태 변수
        private Stack<STATE> stateStack = new Stack<STATE>();
        #endregion

        #region Unity Methods
        private void Start()
        {
            // 버튼들에 이벤트 등록
            menuBtn.onClick.AddListener(() => { OnPushState(STATE.MENU); });
            saveBtn.onClick.AddListener(() => { OnPushState(STATE.SAVE); });
            loadBtn.onClick.AddListener(() => { OnPushState(STATE.LOAD); });
            settingBtn.onClick.AddListener(() => { OnPushState(STATE.SETTING); });
            logBtn.onClick.AddListener(() => { OnPushState(STATE.LOG); });
            exitBtn.onClick.AddListener(() => { OnPushState(STATE.EXIT); });
        }

        private void Update()
        {
            // esc키 눌리면 UI Pop
            if (stateStack.Count > 0 && Input.GetKeyDown(KeyCode.Escape))
                PopState(stateStack.Pop());
        }
        #endregion

        #region Methods
        public static bool IsGameState()
        {
            return Instance.stateStack.Count == 0;
        }
        /// <summary>
        /// UI상태 전단계로 가기
        /// </summary>
        /// <param name="popState">제거된 상태</param>
        private void PopState(STATE popState)
        {
            switch (popState)
            {
                case STATE.MENU:
                    menuPanel.SetSlide(new Vector2(-500.0f, 0.0f), true);
                    break;
                case STATE.SAVE:
                case STATE.LOAD:
                    savePanel.SetSlide(new Vector2(-500.0f, -340.0f), true);
                    break;
                case STATE.SETTING:
                    break;
                case STATE.LOG:
                    logPanel.SetActive(false);
                    break;
                case STATE.EXIT:
                    SceneManager.LoadScene("Seoyoon/Scenes/SeoyoonScene");
                    break;

            }
        }
        #region State Methods
        /// <summary>
        /// UI 상태 추가
        /// </summary>
        /// <param name="pushState">추가될 상태</param>
        private void OnPushState(STATE pushState)
        {
            if (stateStack.Count > 1)
            {
                STATE preState = stateStack.Pop();
                PopState(preState);
                if (preState == pushState)
                    return;
            }
            stateStack.Push(pushState);

            switch (pushState)
            {
                case STATE.MENU:
                    if (stateStack.Count > 1)
                    {
                        stateStack.Pop();
                        while (stateStack.Count > 0)
                            PopState(stateStack.Pop());
                    }
                    else
                    {
                        menuPanel.gameObject.SetActive(true);
                        menuPanel.SetSlide(Vector3.zero, false);
                    }
                    break;
                case STATE.SAVE:
                case STATE.LOAD:
                    savePanel.gameObject.SetActive(true);
                    savePanel.SetSlide(new Vector2(0.0f, -340.0f), false);
                    break;
                case STATE.SETTING:
                    break;
                case STATE.LOG:
                    logPanel.SetActive(true);
                    break;
                case STATE.EXIT:
                    ExitGame();
                    break;
            }
        }
        public void ExitGame()
        {
            SceneManager.LoadScene("Seoyoon/Scenes/TitleProto");
            // 타이틀 씬으로 돌아가게 구현
            // 필요하다면 저장할건지 물어보는것도 생각해야할듯
        }
        #endregion
        #endregion
    }
}