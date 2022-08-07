using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    [System.Serializable]
    public class PlayBGM : BaseScriptEvent
    {
        [SerializeField, LabelText("πË∞Ê¿Ωæ«"), Tooltip("πË∞Ê¿Ωæ« º±≈√\nnull¿œ Ω√ πË∞Ê¿Ωæ« ¿Áª˝¿ª ∏ÿ√‰¥œ¥Ÿ.")]
        private AudioClip audioBGM;

        public override void OnEnter()
        {
            base.OnEnter();

            if (audioBGM != null)
                AudioManager.PlayBGM(audioBGM);
            else
                AudioManager.StopBGM();

            gm.scriptData.SetScript(gm.scriptData.CurrentIndex + 1);
        }
        protected override void OnUpdate() { }
    }
}
