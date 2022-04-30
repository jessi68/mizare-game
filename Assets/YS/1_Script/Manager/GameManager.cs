using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace YS
{
    public struct SaveData
    {
        public int scriptIndex;
    }

    public class GameManager : Singleton<GameManager>
    {
        enum SIDE_IMAGE
        {
            LEFT_SIDE,
            RIGHT_SIDE
        }

        #region Field
        public RectTransform[] choices;
        private TMP_Text[] choiceTMPs;
        public Material bgMtrl;

        public Image[] sideImg = new Image[2];

        public TMP_Text nameTMP;
        public CustomTMPEffect scriptTMP;
        public TMP_Text logTMP;

        public ScriptData scripts;

        
        private Sprite[] charImgs = new Sprite[(int)CHARACTER_IMAGE_INDEX.MAX];
        // sideImg�� �ʱ� ��ġ (FX�ʱ�ȭ �� �� ���)
        private Vector3[] sidePos = new Vector3[2];
        // sideFX �ڷ�ƾ ���� (��ŵ �� ���)
        private Coroutine[] sideFXCoroutine = new Coroutine[2];
        private Coroutine bgFXCoroutine;

        private uint scriptIndex;
        private StringBuilder log = new StringBuilder();
        #endregion

        #region Unity Methods
        protected override void Awake()
        {
            base.Awake();

            // �ʱ�ȭ�� ���� ó���� Left, Right ���̵� �̹����� ��ġ ���
            for (int i = 0; i < 2; ++i)
                sidePos[i] = sideImg[i].transform.position;

            choiceTMPs = new TMP_Text[choices.Length];
            for (int i = 0; i < choices.Length; ++i)
                choiceTMPs[i] = choices[i].GetChild(0).GetComponent<TMP_Text>();

            // ����� ĳ���͵� �̹��� �ε�
            charImgs[(int)CHARACTER_IMAGE_INDEX.MIZAR] = ResourceManager.GetResource<Sprite>("image001");
            charImgs[(int)CHARACTER_IMAGE_INDEX.ALCOR] = ResourceManager.GetResource<Sprite>("image018");
        }
        void Start()
        {
            // ���߿� �ε�� �ε��� index������ ����
            SetDialog(0);
        }
        void Update()
        {
            if (IsKeyDownForDialogEvent())
                OnDialogEvent();
        }
        #endregion

        #region Methods
        /// <summary>
        /// ���� ����
        /// </summary>
        private void SaveGame()
        {

        }
        /// <summary>
        /// ���� �ҷ�����
        /// </summary>
        private void LoadGame()
        {

        }
        /// <summary>
        /// ���̾�α� �̺�Ʈ�� �߻��ߴ°�
        /// </summary>
        /// <returns>�߻��ߴٸ� true</returns>
        private bool IsKeyDownForDialogEvent()
        {
            bool result;

                     // GameState�̰�
            result = InGameUIManager.IsGameState() &&
                     // �����̽� Ű�� ���Ȱų�
                     Input.GetKeyDown(KeyCode.Space) ||
                     // UI�� �ƴѰ��� ���콺 Ŭ�� �̺�Ʈ�� �߻�������
                     (Input.GetKeyDown(KeyCode.Mouse0) && !EventSystem.current.IsPointerOverGameObject());

            return result;
        }
        /// <summary>
        /// ���̾�α� �̺�Ʈ �߻��� ȣ��Ǵ� �Լ�
        /// </summary>
        private void OnDialogEvent()
        {
            // ���콺 Ŭ���� Ÿ������ �ȳ����ٸ� Ÿ���� ������, Ÿ������ �� �Ǿ��ִ� ���¶�� ���� ���̾�α� ����
            if (!scriptTMP.IsDoneTyping)
            {
                ResetEffects();
                scriptTMP.SkipTyping();
            }
            else
            {
                if (scripts[scriptIndex].choices.Length == 0)
                    SetDialog(scripts[scriptIndex].nextIdx);
                else
                {
                    float padding = (1 - (scripts[scriptIndex].choices.Length * 0.15f)) / (scripts[scriptIndex].choices.Length + 1);
                    float height = 1.0f;
                    for (int i = 0; i < scripts[scriptIndex].choices.Length; ++i)
                    {
                        choiceTMPs[i].SetText(scripts[scriptIndex].choices[i].str);
                        choices[i].gameObject.SetActive(true);
                        height -= padding;
                        choices[i].anchorMax = new Vector2(1.0f, height);
                        height -= 0.15f;
                        choices[i].anchorMin = new Vector2(0.0f, height);
                    }
                }
            }
        }
        /// <summary>
        /// ���̾�α� ����
        /// </summary>
        private void SetDialog(uint index)
        {
            scriptIndex = index;

            DialogScript data = scripts[scriptIndex];

            ResetEffects();

            log.Append("<b>");
            log.Append(data.name);
            log.Append("</b>\n<size=40>");
            log.Append(data.script);
            log.Append("</size>\n");

            logTMP.SetText(log);
            nameTMP.SetText(data.name);
            scriptTMP.SetText(data.script);

            ScreenEffect(data.screenEffect);
            SetCharSetting(SIDE_IMAGE.LEFT_SIDE, data.leftImage, data.leftHighlight, data.leftEffect);
            SetCharSetting(SIDE_IMAGE.RIGHT_SIDE, data.rightImage, data.rightHighlight, data.rightEffect);
        }
        /// <summary>
        /// ������ ���� ȣ��Ǵ� �̺�Ʈ �Լ�
        /// </summary>
        /// <param name="index">������ ��ȣ</param>
        public void OnChooseChoice(int index)
        {
            // ��� �������� ��Ȱ��ȭ�ϰ�
            for (int i = 0; i < scripts[scriptIndex].choices.Length; ++i)
                choices[i].gameObject.SetActive(false);

            // ���õ� �������� index�� Dialog�̵�
            SetDialog(scripts[scriptIndex].choices[index].nextIdx);
        }
        /// <summary>
        /// ȭ�� ȿ��
        /// </summary>
        /// <param name="screenFX">������ ȿ��</param>
        private void ScreenEffect(SCREEN_EFFECT screenFX)
        {
            switch (screenFX)
            {
                case SCREEN_EFFECT.FADE_IN:
                    bgFXCoroutine = StartCoroutine(FadeEffect(true, 1.0f));
                    break;
                case SCREEN_EFFECT.FADE_OUT:
                    bgFXCoroutine = StartCoroutine(FadeEffect(false, 1.0f));
                    break;
                case SCREEN_EFFECT.RED_FLASH:
                    bgMtrl.SetColor("_AddColor", Color.red);
                    Invoke("ResetFlash", 0.25f);
                    break;
            }
        }
        /// <summary>
        /// ĳ���� �̹��� ����
        /// </summary>
        /// <param name="side">���� ���̵����� ������ ���̵����� ����</param>
        /// <param name="charImgIdx">������ �̹���</param>
        /// <param name="isHighlight">���̶���Ʈ ����</param>
        /// <param name="charFX">���̵� �̹����� �� ȿ��</param>
        private void SetCharSetting(SIDE_IMAGE side, CHARACTER_IMAGE_INDEX charImgIdx, bool isHighlight, CHARACTER_EFFECT_INDEX charFX)
        {
            sideImg[(int)side].sprite = charImgs[(int)charImgIdx];
            sideImg[(int)side].color = isHighlight ? Color.white : Color.gray;

            switch (charFX)
            {
                case CHARACTER_EFFECT_INDEX.SHAKE_HORIZONTAL:
                case CHARACTER_EFFECT_INDEX.SHAKE_VERTICAL:
                case CHARACTER_EFFECT_INDEX.SHAKE_RANDOM:
                    sideFXCoroutine[(int)side] = StartCoroutine(ShakeEffect(sideImg[(int)side].gameObject.transform, 5, 0.5f, 0.01f, charFX));
                    break;

                case CHARACTER_EFFECT_INDEX.BOUNCE:
                    sideFXCoroutine[(int)side] = StartCoroutine(BounceEffect(sideImg[(int)side].gameObject.transform, 3.0f));
                    break;
            }
        }
        private IEnumerator ShakeEffect(Transform target, float intensity, float time, float intervalTime, CHARACTER_EFFECT_INDEX type)
        {
            WaitForSeconds interval = CachedWaitForSeconds.Get(intervalTime);

            Vector3 curShakeVector = Vector3.zero;
            Vector3 dir = new Vector3(0.0f, 0.0f, 0.0f);
            float remainingTime = time;
            float curIntensity;

            switch (type)
            {
                case CHARACTER_EFFECT_INDEX.SHAKE_VERTICAL:
                    dir.x = 1.0f;
                    break;
                case CHARACTER_EFFECT_INDEX.SHAKE_HORIZONTAL:
                    dir.y = 1.0f;
                    break;
            }

            while (remainingTime > 0.0f)
            {
                if (type == CHARACTER_EFFECT_INDEX.SHAKE_RANDOM)
                    dir = Quaternion.AngleAxis(Random.Range(0.0f, 360.0f), Vector3.forward) * Vector3.right;
                else
                    dir = -dir;

                curIntensity = intensity * (remainingTime / time);

                target.position -= curShakeVector;
                curShakeVector = dir * curIntensity;
                target.position += curShakeVector;

                remainingTime -= intervalTime;
                yield return interval;
            }

            target.position -= curShakeVector;
        }
        private IEnumerator BounceEffect(Transform target, float time)
        {
            float t = 0.0f;
            WaitForSeconds wf = CachedWaitForSeconds.Get(0.01f);
            Bezier bezier = new Bezier();
            bezier.bezierPos = new Vector3[3]
            {
                target.position,
                target.position + Vector3.up * 100.0f,
                target.position
            };

            while (t <= 1.0f)
            {
                t += time * 0.01f;
                target.position = bezier.GetBezierPosition(t);
                yield return wf;
            }
        }
        private IEnumerator FadeEffect(bool isIn, float time)
        {
            WaitForSeconds wf = CachedWaitForSeconds.Get(0.01f);
            float curTime = 0.0f;

            bgMtrl.SetFloat("_IsIn", isIn ? 1.0f : 0.0f);
            
            while (curTime < time)
            {
                bgMtrl.SetFloat("_CurTime", curTime / time);
                yield return wf;
                curTime += 0.01f;
            }
        }
        private void ResetFlash()
        {
            bgMtrl.SetColor("_AddColor", Vector4.zero);
        }
        /// <summary>
        /// ȿ�� �߰��� ��ŵ�� �� �����Ƿ�, ȿ������ ������̶�� �����Ű�� �̹������� ��ġ �����·� �����ϴ� �Լ�
        /// </summary>
        private void ResetEffects()
        {
            // �۵����� ȿ�� �ڷ�ƾ ���߱�
            if (sideFXCoroutine[0] != null) StopCoroutine(sideFXCoroutine[0]);
            if (sideFXCoroutine[1] != null) StopCoroutine(sideFXCoroutine[1]);
            if (bgFXCoroutine != null)      StopCoroutine(bgFXCoroutine);

            for (int i = 0; i < 2; ++i)
                sideImg[i].transform.position = sidePos[i];

            bgMtrl.SetFloat("_CurTime", 1.0f);
            ResetFlash();
        }
        #endregion
    }
}