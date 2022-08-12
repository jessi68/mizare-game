using UnityEngine;
using UnityEngine.UI;

namespace YS
{
    public class AudioManager : Singleton<AudioManager>
    {
        #region Field
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

        #region Properties
        public static float BGMVolume
        {
            get => Instance.audioBGM.volume;
            set => Instance.audioBGM.volume = value;
        }
        public static float FXVolume
        {
            get => Instance.audioFX.volume;
            set => Instance.audioFX.volume = value;
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
        public static void PlayFX(AudioClip newBGM, float delay = 0.0f)
        {
            var am = Instance;

            am.audioFX.Stop();
            am.audioFX.clip = newBGM;
            if (delay == 0.0f)
                am.audioFX.Play();
            else
                am.audioFX.PlayDelayed(delay);
        }
        public static void StopBGM()
        {
            Instance.audioBGM.Stop();
        }
        public static void StopFX()
        {
            Instance.audioFX.Stop();
        }
        #endregion
    }
}