using UnityEngine;
using UnityEngine.SceneManagement;

namespace Seoyoon
{
    public struct SceneMeta
    {
        public SceneMeta(int saveIndex)
        {
            this.saveIndex = saveIndex;
        }

        int saveIndex { get; }
    }
    public class TitleManager : MonoBehaviour
    {
        #region Field
        public GameObject newGameUI;
        public GameObject loadGameUI;
        public GameObject gameSettingUI;

        private Coroutine cTextPreview;
        #endregion

        #region Unity Methods
        private void Start()
        {
            YS.SettingManager.Initialize();
        }
        #endregion

        #region Methods
        #region UIEvent
        public void ShowNewGamePanel()
        {
            newGameUI.SetActive(true);
        }
        public void CloseNewGamePanel()
        {
            newGameUI.SetActive(false);
        }
        public void ShowLoadGamePanel()
        {
            loadGameUI.SetActive(true);
        }
        public void CloseLoadGamePanel()
        {
            loadGameUI.SetActive(false);
        }
        public void ShowGameSettingUIPanel()
        {
            gameSettingUI.SetActive(true);
            cTextPreview = StartCoroutine(YS.SettingManager.TextPreview());
        }
        public void CloseGameSettingUIPanel()
        {
            StopCoroutine(cTextPreview);
            gameSettingUI.SetActive(false);
        }
        #endregion


        /// <summary>
        /// 새로운 게임을 시작한다.
        /// </summary>
        /// <param name="saveIndex">새로운 게임을 저장할 저장슬롯</param>
        public void OnNewGame(int saveIndex)
        {
            // 새로운 게임이므로, saveIndex에 해당하는 세이브데이터를 지우고 새 게임으로 덮어쓴다 (아직 어떻게 새로운 게임에 대한 세이브데이터를 설정할지 못정함)
            OnOverwriteGame(saveIndex, new YS.SaveData());

            // 빈 세이브파일에 새로운 게임을 시작하는것은 로드게임과 같은 기능을 하므로 로드게임시 호출되는 함수와 같은 함수 호출
            OnStartGame(saveIndex);
        }
        /// <summary>
        /// newData를 saveIndex슬롯에 덮어쓴다
        /// </summary>
        /// <param name="saveIndex">저장할 save파일 번호</param>
        /// <param name="newData">새롭게 저장할 데이터</param>
        public void OnOverwriteGame(int saveIndex, YS.SaveData newData)
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
            
            SceneManager.LoadScene("Seoyoon/Scenes/SeoyoonScene");
            // 씬 전환
        }

        #endregion
    }
}