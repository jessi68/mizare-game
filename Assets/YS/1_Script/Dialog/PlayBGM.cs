using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    [System.Serializable]
    public class PlayBGM : BaseScriptEvent
    {
        [SerializeField, LabelText("�������"), Tooltip("������� ����\nnull�� �� ������� ����� ����ϴ�.")]
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
