using UnityEngine;
using UnityEngine.UI;

namespace YS
{
    public class AudioManager : Singleton<AudioManager>
    {
        #region Field
        private AudioSource audioBGM;
        private AudioSource audioFX;

        private float audioBGMVolume;
        private float audioFXVolume;
        private float audioBGMTempVolume;
        private float audioFXTempVolume;
        #endregion

        #region Unity Methods
        protected override void Awake()
        {
            base.Awake();

            audioBGM = transform.GetChild(0).GetComponent<AudioSource>();
            audioFX = transform.GetChild(1).GetComponent<AudioSource>();
            DontDestroyOnLoad(gameObject);

            // 볼륨도 저장데이터에 포함된다면 여기서 초기화.
            //Instance.audioBGMVolume = saveData.volume;
            //Instance.audioFXVolume = saveData.volumeFX;
            audioBGMVolume = 1.0f;
            audioFXVolume = 1.0f;
        }
        #endregion

        #region Properties
        public static float TotalBGMVolume => Instance.audioBGM.volume;
        public static float TotalFXVolume => Instance.audioFX.volume;
        public static float BaseBGMVolume
        {
            get => Instance.audioBGMVolume;
            set
            {
                Instance.audioBGMVolume = value;
                Instance.audioBGM.volume = value * TempBGMVolume;
            }
        }
        private static float TempBGMVolume
        {
            get => Instance.audioBGMTempVolume;
            set
            {
                Instance.audioBGMTempVolume = value;
                Instance.audioBGM.volume = value * BaseBGMVolume;
            }
        }
        public static float BaseFXVolume
        {
            get => Instance.audioFXVolume;
            set
            {
                Instance.audioFXVolume = value;
                Instance.audioFX.volume = value * TempFXVolume;
            }
        }       
        private static float TempFXVolume
        {
            get => Instance.audioFXTempVolume;
            set
            {
                Instance.audioFXTempVolume = value;
                Instance.audioFX.volume = value * BaseFXVolume;
            }
        }
        #endregion

        #region Methods
        public static void PlayBGM(AudioClip newBGM, float tempVolume = 1.0f)
        {
            var am = Instance;

            am.audioBGM.Stop();
            TempBGMVolume = tempVolume;
            am.audioBGM.clip = newBGM;
            am.audioBGM.Play();
        }
        public static void PlayFX(AudioClip newFX, float tempVolume = 1.0f, float delay = 0.0f)
        {
            var am = Instance;

            am.audioFX.Stop();
            TempFXVolume = tempVolume;
            am.audioFX.clip = newFX;
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