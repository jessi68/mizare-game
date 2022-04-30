using UnityEngine;
using UnityEngine.UI;

namespace YS
{
    public class AudioManager : Singleton<AudioManager>
    {
        #region Field
        public Slider volumeSlider;
        
        private AudioSource audioSource;
        #endregion

        #region Unity Methods
        protected override void Awake()
        {
            base.Awake();

            audioSource = GetComponent<AudioSource>();
            DontDestroyOnLoad(gameObject);
        }
        #endregion

        #region Methods
        public static void Initialize()
        {
            // ������ ���嵥���Ϳ� ���Եȴٸ� ���⼭ �ʱ�ȭ.
            //Instance.audioSource.volume = saveData.volume;
            // �����̴��� ������ �°� ����
            //Instance.volumeSlider.value = Instance.audioSource.volume;
        }
        public static void ChangeMusic()
        {
            // ���Ŀ� ���� ������ ���ִٸ� ����
        }
        public static void PlayMusic()
        {
            if (Instance.audioSource.isPlaying) return;
            Instance.audioSource.Play();
        }
        public static void StopMusic()
        {
            Instance.audioSource.Stop();
        }
        public static void SetVolume(float volume)
        {
            Instance.audioSource.volume = volume;
        }
        public static void OnChangeBGMVolumeSlider()
        {
            SetVolume(Instance.volumeSlider.value);
        }
        #endregion
    }
}