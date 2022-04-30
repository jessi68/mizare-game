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
        /// ���ο� ������ �����Ѵ�.
        /// </summary>
        /// <param name="saveIndex">���ο� ������ ������ ���彽��</param>
        public void OnNewGame(int saveIndex)
        {
            // ���ο� �����̹Ƿ�, saveIndex�� �ش��ϴ� ���̺굥���͸� ����� �� �������� ����� (���� ��� ���ο� ���ӿ� ���� ���̺굥���͸� �������� ������)
            OnOverwriteGame(saveIndex, new YS.SaveData());

            // �� ���̺����Ͽ� ���ο� ������ �����ϴ°��� �ε���Ӱ� ���� ����� �ϹǷ� �ε���ӽ� ȣ��Ǵ� �Լ��� ���� �Լ� ȣ��
            OnStartGame(saveIndex);
        }
        /// <summary>
        /// newData�� saveIndex���Կ� �����
        /// </summary>
        /// <param name="saveIndex">������ save���� ��ȣ</param>
        /// <param name="newData">���Ӱ� ������ ������</param>
        public void OnOverwriteGame(int saveIndex, YS.SaveData newData)
        {
            // saveIndex�� �ش� ���̺� ������ ���ͼ� newData�� ó��
            // ...
        }

        /// <summary>
        /// ���õ� save������ ������ �а� �ش� ������ �������� ������ �����Ѵ�
        /// </summary>
        /// <param name="saveIndex">save���� ��ȣ</param>
        public void OnStartGame(int saveIndex)
        {
            // �� ��ȯ�ϱ� �� ��� ���̺긦 ȣ���ߴ��� �ΰ��Ӿ����� �˾ƾ��ϹǷ� Ư�� ��ü�� ������ ��� DontDestroyOnLoad(������ ��� ��ü); �Լ� ȣ��
            // ...
            
            SceneManager.LoadScene("Seoyoon/Scenes/SeoyoonScene");
            // �� ��ȯ
        }

        #endregion
    }
}