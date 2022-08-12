using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;

namespace YS
{
    public class InGameUIManager : Singleton<InGameUIManager>
    {
        public enum INGAME_UI_STATE
        {
            GAME,
            INVEN,
            MENU,
            SAVE,
            LOAD,
            SETTING,
            LOG,
            EXIT
        }

        #region Field
        [BoxGroup("패널", true, true)]
        [LabelText("인벤토리 UI")]
        public InventoryComponent invenComp;
        [BoxGroup("패널")]
        [LabelText("메인메뉴 UI")]
        public GameObject ui;
        [BoxGroup("패널")]
        [LabelText("메뉴 패널 UI")]
        public SlideEffect menuPanel;
        [BoxGroup("패널")]
        [LabelText("세이브 패널 UI")]
        public SlideEffect savePanel;
        [BoxGroup("패널")]
        [LabelText("로그 패널 UI")]
        public GameObject logPanel;
        [BoxGroup("패널")]
        [LabelText("설정 패널 UI")]
        public GameObject settingPanel;

        [BoxGroup("UI", true, true)]
        [LabelText("메뉴 버튼")]
        public Button menuBtn;
        [BoxGroup("UI")]
        [LabelText("인벤토리 버튼")]
        public Button invenBtn;
        [BoxGroup("UI/인벤토리", true, true)]
        [LabelText("인벤토리 닫기 버튼")]
        public Button invenExitBtn;
        [BoxGroup("UI/메뉴", true, true)]
        [LabelText("저장 버튼")]
        public Button saveBtn;
        [BoxGroup("UI/메뉴/저장", true, true)]
        [LabelText("슬롯1")]
        public Button slot1SaveBtn;
        [BoxGroup("UI/메뉴/저장")]
        [LabelText("슬롯2")]
        public Button slot2SaveBtn;
        [BoxGroup("UI/메뉴/저장")]
        [LabelText("슬롯3")]
        public Button slot3SaveBtn;
        [BoxGroup("UI/메뉴")]
        [LabelText("불러오기 버튼")]
        public Button loadBtn;
        [BoxGroup("UI/메뉴/불러오기", true, true)]
        [LabelText("퀵슬롯")]
        public Button quickSlotLoadBtn;
        [BoxGroup("UI/메뉴/불러오기")]
        [LabelText("슬롯1")]
        public Button slot1LoadBtn;
        [BoxGroup("UI/메뉴/불러오기")]
        [LabelText("슬롯2")]
        public Button slot2LoadBtn;
        [BoxGroup("UI/메뉴/불러오기")]
        [LabelText("슬롯3")]
        public Button slot3LoadBtn;
        [BoxGroup("UI/메뉴")]
        [LabelText("설정 버튼")]
        public Button settingBtn;
        [BoxGroup("UI/메뉴/설정", true, true)]
        [LabelText("설정 닫기 버튼")]
        public Button closeSettingBtn;
        [BoxGroup("UI/메뉴/설정")]
        [LabelText("볼륨 슬라이더")]
        public Slider volumeSlider;
        [BoxGroup("UI/메뉴/설정")]
        [LabelText("타이핑 속도 슬라이더")]
        public Slider typingSlider;
        [BoxGroup("UI/메뉴/설정")]
        [LabelText("타이핑 미리보기")]
        public CustomTMPEffect previewTMP;
        [BoxGroup("UI/메뉴")]
        [LabelText("로그 버튼")]
        public Button logBtn;
        [BoxGroup("UI/메뉴")]
        [LabelText("종료 버튼")]
        public Button exitBtn;

        // UI 상태 변수
        private Stack<INGAME_UI_STATE> stateStack = new Stack<INGAME_UI_STATE>();
        private Coroutine coroutineTextPreview;
        #endregion

        #region Properties
        public bool IsShowingInventory => stateStack.Count != 0 && stateStack.Peek() == INGAME_UI_STATE.INVEN;
        [ShowInInspector]
        public INGAME_UI_STATE CurrentState => stateStack.Count == 0 ? INGAME_UI_STATE.GAME : stateStack.Peek();
        public Stack<INGAME_UI_STATE> StateStack => stateStack;
        #endregion

        #region Unity Methods
        private void Start()
        {
            // 버튼들에 이벤트 등록
            menuBtn.onClick.AddListener(() => { OnPushState(INGAME_UI_STATE.MENU); });
            
            invenBtn.onClick.AddListener(() => { OnPushState(INGAME_UI_STATE.INVEN); });
            invenExitBtn.onClick.AddListener(() => { PopState(stateStack.Pop()); });
            
            saveBtn.onClick.AddListener(() => { OnPushState(INGAME_UI_STATE.SAVE); });
            slot1SaveBtn.onClick.AddListener(() => { OnOverwriteGame(1, GameManager.Instance.CurrentData); });
            slot2SaveBtn.onClick.AddListener(() => { OnOverwriteGame(2, GameManager.Instance.CurrentData); });
            slot3SaveBtn.onClick.AddListener(() => { OnOverwriteGame(3, GameManager.Instance.CurrentData); });
            
            loadBtn.onClick.AddListener(() => { OnPushState(INGAME_UI_STATE.LOAD); });
            quickSlotLoadBtn.onClick.AddListener(() => { OnStartGame(0); });
            slot1LoadBtn.onClick.AddListener(() => { OnStartGame(1); });
            slot2LoadBtn.onClick.AddListener(() => { OnStartGame(2); });
            slot3LoadBtn.onClick.AddListener(() => { OnStartGame(3); });
            
            settingBtn.onClick.AddListener(() => { OnPushState(INGAME_UI_STATE.SETTING); });
            closeSettingBtn.onClick.AddListener(() => { PopState(stateStack.Pop()); });
            volumeSlider.onValueChanged.AddListener((float value) => { AudioManager.BGMVolume = value; });
            typingSlider.onValueChanged.AddListener((float value) => { SettingManager.ChangeTypingSpeed((TYPING_SPEED)value); });
            volumeSlider.value = AudioManager.BGMVolume;
            typingSlider.value = (float)SettingManager.GetTypingSpeed();
            
            logBtn.onClick.AddListener(() => { OnPushState(INGAME_UI_STATE.LOG); });
            exitBtn.onClick.AddListener(() => { OnPushState(INGAME_UI_STATE.EXIT); });
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
        public void PopState(INGAME_UI_STATE popState)
        {
            switch (popState)
            {
                case INGAME_UI_STATE.INVEN:
                    invenComp.CloseInventory();
                    ui.SetActive(true);
                    break;
                case INGAME_UI_STATE.MENU:
                    menuPanel.SetSlide(new Vector2(-500.0f, 0.0f), true);
                    break;
                case INGAME_UI_STATE.SAVE:
                case INGAME_UI_STATE.LOAD:
                    savePanel.SetSlide(new Vector2(-500.0f, -340.0f), true);
                    break;
                case INGAME_UI_STATE.SETTING:
                    StopCoroutine(coroutineTextPreview);
                    settingPanel.SetActive(false);
                    break;
                case INGAME_UI_STATE.LOG:
                    logPanel.SetActive(false);
                    break;
                case INGAME_UI_STATE.EXIT:
                    SceneManager.LoadScene("Seoyoon/Scenes/SeoyoonScene");
                    break;
            }
        }
        #region State Methods
        /// <summary>
        /// UI 상태 추가
        /// </summary>
        /// <param name="pushState">추가될 상태</param>
        private void OnPushState(INGAME_UI_STATE pushState)
        {
            if (stateStack.Count > 1)
            {
                INGAME_UI_STATE preState = stateStack.Pop();
                PopState(preState);
                if (preState == pushState)
                    return;
            }
            stateStack.Push(pushState);

            switch (pushState)
            {
                case INGAME_UI_STATE.INVEN:
                    ui.SetActive(false);
                    invenComp.OpenInventory();
                    break;
                case INGAME_UI_STATE.MENU:
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
                case INGAME_UI_STATE.SAVE:
                case INGAME_UI_STATE.LOAD:
                    savePanel.gameObject.SetActive(true);
                    savePanel.SetSlide(new Vector2(0.0f, -340.0f), false);
                    break;
                case INGAME_UI_STATE.SETTING:
                    settingPanel.SetActive(true);
                    coroutineTextPreview = StartCoroutine(SettingManager.TextPreview(previewTMP));
                    break;
                case INGAME_UI_STATE.LOG:
                    logPanel.SetActive(true);
                    break;
                case INGAME_UI_STATE.EXIT:
                    ExitGame();
                    break;
            }
        }
        public void ExitGame()
        {
            SceneManager.LoadScene("YS/0_Scene/TitleProto");
            // 타이틀 씬으로 돌아가게 구현
            // 필요하다면 저장할건지 물어보는것도 생각해야할듯
        }

        /// <summary>
        /// newData를 saveIndex슬롯에 덮어쓴다
        /// </summary>
        /// <param name="saveIndex">저장할 save파일 번호</param>
        /// <param name="newData">새롭게 저장할 데이터</param>
        public void OnOverwriteGame(int saveIndex, SaveData newData)
        {
            // saveIndex로 해당 세이브 슬롯을 얻어와서 newData로 처리
            // ...
        }

        /// <summary>
        /// 선택된 save파일의 정보를 읽고 해당 정보를 바탕으로 게임을 시작한다
        /// </summary>
        /// <param name="saveIndex">save파일 번호</param>
        public void OnStartGame(int saveIndex)
        {
            // 씬 전환하기 전 몇번 세이브를 호출했는지 인게임씬에서 알아야하므로 특정 객체에 정보를 담아 DontDestroyOnLoad(정보가 담긴 객체); 함수 호출
            // ...

            SceneManager.LoadScene("YS/0_Scene/YSTestScene");
            // 씬 전환
        }
        #endregion
        #endregion
    }
}