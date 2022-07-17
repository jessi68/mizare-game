using UnityEngine;
using UnityEngine.UI;

namespace YS
{
    public class AudioManager : Singleton<AudioManager>
    {
        #region Field
        public Slider volumeSlider;
        
        private AudioSource audioBGM;
        private AudioSource audioFX;
        #endregion

        #region Unity Methods
        protected override void Awake()
        {
            base.Awake();

            audioBGM = transform.GetChild(0).GetComponent<AudioSource>();
            audioFX = transform.GetChild(1).GetComponent<AudioSource>();
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
        public static void PlayBGM(AudioClip newBGM)
        {
            var am = Instance;

            am.audioBGM.Stop();
            am.audioBGM.clip = newBGM;
            am.audioBGM.Play();
        }
        public static void PlayFX(AudioClip newBGM)
        {
            var am = Instance;

            am.audioFX.Stop();
            am.audioFX.clip = newBGM;
            am.audioFX.Play();
        }
        public static void StopBGM()
        {
            Instance.audioBGM.Stop();
        }
        public static void StopFX()
        {
            Instance.audioFX.Stop();
        }
        public static void SetBGMVolume(float volume)
        {
            Instance.audioBGM.volume = volume;
        }
        public static void SetFXVolume(float volume)
        {
            Instance.audioFX.volume = volume;
        }
        public static void OnChangeBGMVolumeSlider()
        {
            SetBGMVolume(Instance.volumeSlider.value);
        }
        #endregion
    }
}