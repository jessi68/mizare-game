using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;

namespace YS
{
    public class TitleManager : MonoBehaviour
    {
        public enum TITLE_UI_STATE
        {
            TOUCH_TO_START,
            MENU,
            SETTING,
            GAMESTART,
            LOAD,
            GALLERY
        }
        #region Field
        [BoxGroup("패널", true, true)]
        [LabelText("터치시작 패널")]
        public GameObject clickToStartPanel;
        [BoxGroup("패널")]
        [LabelText("메뉴 패널"), Tooltip("모든 메뉴들의 상위 패널")]
        public GameObject menuPanel;
        [BoxGroup("패널")]
        [LabelText("설정 패널")]
        public GameObject settingPanel;
        [BoxGroup("패널")]
        [LabelText("메인 패널")]
        public GameObject mainPanel;
        [BoxGroup("패널")]
        [LabelText("게임시작 패널")]
        public GameObject startPanel;
        [BoxGroup("패널")]
        [LabelText("불러오기 패널")]
        public GameObject loadPanel;
        [BoxGroup("패널")]
        [LabelText("사진첩 패널")]
        public GameObject galleryPanel;

        [BoxGroup("메인 메뉴", true, true)]
        [LabelText("설정 버튼")]
        public Button settingBtn;
        [BoxGroup("메인 메뉴")]
        [LabelText("뒤로가기 버튼")]
        public Button backBtn;
        [BoxGroup("메인 메뉴")]
        [LabelText("게임시작 버튼")]
        public Button startBtn;
        [BoxGroup("메인 메뉴")]
        [LabelText("불러오기 버튼")]
        public Button loadBtn;
        [BoxGroup("메인 메뉴")]
        [LabelText("사진첩 버튼")]
        public Button galleryBtn;

        [BoxGroup("메인 메뉴/설정", true, true)]
        [LabelText("볼륨 슬라이더")]
        public Slider volumeSlider;
        [BoxGroup("메인 메뉴/설정")]
        [LabelText("타이핑 슬라이더")]
        public Slider typingSlider;
        [BoxGroup("메인 메뉴/설정")]
        [LabelText("타이핑 미리보기")]
        public CustomTMPEffect previewTMP;

        [BoxGroup("메인 메뉴/게임시작", true, true)]
        [LabelText("슬롯1 버튼")]
        public Button slot1StartBtn;
        [BoxGroup("메인 메뉴/게임시작")]
        [LabelText("슬롯2 버튼")]
        public Button slot2StartBtn;
        [BoxGroup("메인 메뉴/게임시작")]
        [LabelText("슬롯3 버튼")]
        public Button slot3StartBtn;

        [BoxGroup("메인 메뉴/불러오기", true, true)]
        [LabelText("퀵슬롯 버튼")]
        public Button quickSlotLoadBtn;
        [BoxGroup("메인 메뉴/불러오기")]
        [LabelText("슬롯1 버튼")]
        public Button slot1LoadBtn;
        [BoxGroup("메인 메뉴/불러오기")]
        [LabelText("슬롯2 버튼")]
        public Button slot2LoadBtn;
        [BoxGroup("메인 메뉴/불러오기")]
        [LabelText("슬롯3 버튼")]
        public Button slot3LoadBtn;

        private Coroutine coroutineTextPreview;
        // UI 상태 변수
        private Stack<TITLE_UI_STATE> stateStack = new Stack<TITLE_UI_STATE>();
        #endregion

        #region Properties
        public TITLE_UI_STATE CurrentState => stateStack.Peek();
        #endregion

        #region Unity Methods
        private void Start()
        {
            stateStack.Push(TITLE_UI_STATE.TOUCH_TO_START);

            settingBtn.onClick.AddListener(() => { PushState(TITLE_UI_STATE.SETTING); });
            backBtn.onClick.AddListener(() => { PopState(); });
            startBtn.onClick.AddListener(() => { PushState(TITLE_UI_STATE.GAMESTART); });
            loadBtn.onClick.AddListener(() => { PushState(TITLE_UI_STATE.LOAD); });
            galleryBtn.onClick.AddListener(() => { PushState(TITLE_UI_STATE.GALLERY); });

            volumeSlider.onValueChanged.AddListener((float value) => { AudioManager.BaseBGMVolume = value; });
            typingSlider.onValueChanged.AddListener((float value) => { SettingManager.ChangeTypingSpeed((TYPING_SPEED)value); });
            volumeSlider.value = AudioManager.BaseBGMVolume;
            typingSlider.value = (float)SettingManager.GetTypingSpeed();

            slot1StartBtn.onClick.AddListener(() => { OnNewGame(1); });
            slot2StartBtn.onClick.AddListener(() => { OnNewGame(2); });
            slot3StartBtn.onClick.AddListener(() => { OnNewGame(3); });

            quickSlotLoadBtn.onClick.AddListener(() => { OnStartGame(0); });
            slot1LoadBtn.onClick.AddListener(() => { OnStartGame(1); });
            slot2LoadBtn.onClick.AddListener(() => { OnStartGame(2); });
            slot3LoadBtn.onClick.AddListener(() => { OnStartGame(3); });
        }
        private void Update()
        {
            if (clickToStartPanel.activeInHierarchy && Input.anyKeyDown)
            {
                PopState();
                PushState(TITLE_UI_STATE.MENU);
            }
        }
        #endregion

        #region Methods
        #region UIEvent
        public void PushState(TITLE_UI_STATE pushState)
        {
            stateStack.Push(pushState);

            switch (pushState)
            {
                case TITLE_UI_STATE.MENU:
                    menuPanel.SetActive(true);
                    break;
                case TITLE_UI_STATE.SETTING:
                    settingPanel.SetActive(true);
                    coroutineTextPreview = StartCoroutine(SettingManager.TextPreview(previewTMP));
                    break;
                case TITLE_UI_STATE.GAMESTART:
                    mainPanel.SetActive(false);
                    startPanel.SetActive(true);
                    break;
                case TITLE_UI_STATE.LOAD:
                    mainPanel.SetActive(false);
                    loadPanel.SetActive(true);
                    break;
                case TITLE_UI_STATE.GALLERY:
                    mainPanel.SetActive(false);
                    galleryPanel.SetActive(true);
                    break;
            }
        }
        public void PopState()
        {
            switch (CurrentState)
            {
                case TITLE_UI_STATE.TOUCH_TO_START:
                    clickToStartPanel.SetActive(false);
                    break;
                case TITLE_UI_STATE.MENU:
                    // 게임 종료
                    // ...
                    break;
                case TITLE_UI_STATE.SETTING:
                    StopCoroutine(coroutineTextPreview);
                    previewTMP.SkipTyping();
                    settingPanel.SetActive(false);
                    break;
                case TITLE_UI_STATE.GAMESTART:
                    startPanel.SetActive(false);
                    mainPanel.SetActive(true);
                    break;
                case TITLE_UI_STATE.LOAD:
                    loadPanel.SetActive(false);
                    mainPanel.SetActive(true);
                    break;
                case TITLE_UI_STATE.GALLERY:
                    galleryPanel.SetActive(false);
                    mainPanel.SetActive(true);
                    break;
            }
        }
        #endregion


        /// <summary>
        /// 새로운 게임을 시작한다.
        /// </summary>
        /// <param name="saveIndex">새로운 게임을 저장할 저장슬롯</param>
        public void OnNewGame(int saveIndex)
        {
            // 새로운 게임이므로, saveIndex에 해당하는 세이브데이터를 지우고 새 게임으로 덮어쓴다 (아직 어떻게 새로운 게임에 대한 세이브데이터를 설정할지 못정함)
            OnOverwriteGame(saveIndex, new SaveData());

            // 빈 세이브파일에 새로운 게임을 시작하는것은 로드게임과 같은 기능을 하므로 로드게임시 호출되는 함수와 같은 함수 호출
            OnStartGame(saveIndex);
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
            
            SceneManager.LoadScene(1);
            // 씬 전환
        }
        #endregion
    }
}