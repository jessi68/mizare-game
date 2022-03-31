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
            // 볼륨도 저장데이터에 포함된다면 여기서 초기화.
            //Instance.audioSource.volume = saveData.volume;
            // 슬라이더도 볼륨에 맞게 조정
            //Instance.volumeSlider.value = Instance.audioSource.volume;
        }
        public static void ChangeMusic()
        {
            // 추후에 음악 변경할 일있다면 구현
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