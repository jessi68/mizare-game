using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using Sirenix.OdinInspector;

namespace YS
{
    public class GameManager : Singleton<GameManager>
    {
        #region Field
        /// <summary>
        /// ���̾�α� ������ ����
        /// </summary>
        [HideLabel]
        public DialogStruct dialogStruct;
        /// <summary>
        /// ������ ������ ����
        /// </summary>
        [HideLabel]
        public ChoiceStruct choiceStruct;
        /// <summary>
        /// ���� ������ ����
        /// </summary>
        [HideLabel]
        public InvestigationStruct ivStruct;
        /// <summary>
        /// �߸� ������ ����
        /// </summary>
        [HideLabel]
        public InferenceStruct ifStruct;
        /// <summary>
        /// ���� ������ ����
        /// </summary>
        [HideLabel]
        public ArrangeStruct arStruct;

        [LabelText("�α� TMP")]
        public TMP_Text logTMP;

        [LabelText("��ũ��Ʈ ������")]
        public ScriptData scriptData;
        [LabelText("������ ������")]
        public ItemData itemData;
        [LabelText("�κ��丮 ������Ʈ")]
        public InventoryComponent invenComp;
        [LabelText("��������Ʈ")]
        public GameObject bgUI;
        private Material bgMtrl;

        [HideInInspector]
        public Sprite[] charImgs = new Sprite[(int)CHARACTER_IMAGE_INDEX.MAX];
        [HideInInspector]
        public Coroutine bgFXCoroutine;

        private SaveData currentData;
        private InGameUIManager um;
        private StringBuilder log = new StringBuilder();

        public delegate void OnUpdate();
        public event OnUpdate OnUpdateEvent;
        #endregion

        #region Properties
        public int ItemCount => bgUI.transform.childCount;
        public SaveData CurrentData => currentData;
        #endregion

        #region Unity Methods
        protected override void Awake()
        {
            base.Awake();

            bgMtrl = ResourceManager.GetResource<Material>("BGFXShader");

            // ����� ĳ���͵� �̹��� �ε�
            charImgs[(int)CHARACTER_IMAGE_INDEX.NONE] = null;
            charImgs[(int)CHARACTER_IMAGE_INDEX.MIZAR] = ResourceManager.GetResource<Sprite>("Characters/Mizar/Mizar_Normal");
            charImgs[(int)CHARACTER_IMAGE_INDEX.ALCOR] = ResourceManager.GetResource<Sprite>("Characters/Alcor/Alcor_Normal");
            charImgs[(int)CHARACTER_IMAGE_INDEX.SENIOR] = ResourceManager.GetResource<Sprite>("Characters/Senior/Senior");
            charImgs[(int)CHARACTER_IMAGE_INDEX.SCHOLAR] = ResourceManager.GetResource<Sprite>("Characters/Scholar/Scholar");
            charImgs[(int)CHARACTER_IMAGE_INDEX.BLACKROBE] = ResourceManager.GetResource<Sprite>("Characters/BlackRobe/BlackRobe");
            //charImgs[(int)CHARACTER_IMAGE_INDEX.MIZAR_NORMAL] = ResourceManager.GetResource<Sprite>("Characters/Mizar/Mizar_Normal");
            //charImgs[(int)CHARACTER_IMAGE_INDEX.MIZAR_SMILE1] = ResourceManager.GetResource<Sprite>("Characters/Mizar/Mizar_Smile1");
            //charImgs[(int)CHARACTER_IMAGE_INDEX.MIZAR_SMILE2] = ResourceManager.GetResource<Sprite>("Characters/Mizar/Mizar_Smile2");
            //charImgs[(int)CHARACTER_IMAGE_INDEX.MIZAR_WINK] = ResourceManager.GetResource<Sprite>("Characters/Mizar/Mizar_Wink");
            //charImgs[(int)CHARACTER_IMAGE_INDEX.MIZAR_SURPRISED] = ResourceManager.GetResource<Sprite>("Characters/Mizar/Mizar_Surprised");
            //charImgs[(int)CHARACTER_IMAGE_INDEX.ALCOR_NORMAL] = ResourceManager.GetResource<Sprite>("Characters/Alcor/Alcor_Normal");
            //charImgs[(int)CHARACTER_IMAGE_INDEX.ALCOR_ANGRY] = ResourceManager.GetResource<Sprite>("Characters/Alcor/Alcor_Angry");
            //charImgs[(int)CHARACTER_IMAGE_INDEX.SENIOR] = ResourceManager.GetResource<Sprite>("Characters/Senior/Senior");
            //charImgs[(int)CHARACTER_IMAGE_INDEX.CHLID] = ResourceManager.GetResource<Sprite>("Characters/Child/Child");
            //charImgs[(int)CHARACTER_IMAGE_INDEX.WOMAN] = ResourceManager.GetResource<Sprite>("Characters/Woman/Woman");
            //charImgs[(int)CHARACTER_IMAGE_INDEX.SCHOLAR] = ResourceManager.GetResource<Sprite>("Characters/Scholar/Scholar");
            //charImgs[(int)CHARACTER_IMAGE_INDEX.BLACKROBE] = ResourceManager.GetResource<Sprite>("Characters/BlackRobe/BlackRobe");
        }
        void Start()
        {
            um = InGameUIManager.Instance;

            SetBGFadeInOut(true);
            SetBGCurTime(0.0f);
            dialogStruct.Initialize();
            choiceStruct.Initialize();
            ivStruct.Initialize();
            ifStruct.Initialize();

            // ���߿� �ε�� �ε��� index������ ����
            scriptData.SetScript(0);
        }
        void Update()
        {
            if (um.CurrentState != InGameUIManager.INGAME_UI_STATE.GAME && IsKeyDown())
                um.PopState(um.StateStack.Pop());
            else
                OnUpdateEvent?.Invoke();
        }
        private void OnDestroy()
        {
            scriptData.Clear();
        }
        #endregion

        #region Methods
        public bool IsKeyDown()
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
        public Item GetItem(int index)
        {
            return bgUI.transform.GetChild(index).GetComponent<Item>();
        }
        /// <summary>
        /// ��� ���̵�ȿ�� ���൵ ����
        /// </summary>
        /// <param name="curTime">���̵� ȿ�� ���൵ ���� (0 ~ 1)</param>
        public void SetBGCurTime(float curTime)
        {
            bgMtrl.SetFloat("_CurTime", curTime);
        }
        /// <summary>
        /// ��� ���̵� ����
        /// </summary>
        /// <param name="fadeCondition">true�� FadeIn, false�� FadeOut</param>
        public void SetBGFadeInOut(bool fadeCondition)
        {
            bgMtrl.SetFloat("_IsIn", fadeCondition ? 1.0f : 0.0f);
        }
        /// <summary>
        /// �α� �����
        /// </summary>
        public void Logging(string str)
        {
            log.Append(str);
            logTMP.SetText(log);
        }
        public void ChangeBackground(GameObject newBG)
        {
            Transform canvasTr = bgUI.transform.parent;
            RectTransform newBGTr = newBG.GetComponent<RectTransform>();

            newBGTr.SetParent(canvasTr, true);
            newBGTr.SetAsFirstSibling();
            newBGTr.anchoredPosition = Vector2.zero;
            newBGTr.anchorMin = Vector2.zero;
            newBGTr.anchorMax = Vector2.one;
            newBGTr.sizeDelta = Vector2.zero;

            Destroy(bgUI);

            bgUI = newBG;
        }

        #region FX
        /// <summary>
        /// ȭ�� ȿ��
        /// </summary>
        /// <param name="screenFX">������ ȿ��</param>
        public void ScreenEffect(SCREEN_EFFECT screenFX)
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
                    Invoke(nameof(ResetFlash), 0.25f);
                    break;
            }
        }
        public IEnumerator ShakeEffect(Transform target, float intensity, float time, float intervalTime, CHARACTER_EFFECT_INDEX type)
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
        public IEnumerator BounceEffect(Transform target, float time)
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
        public IEnumerator FadeEffect(bool isIn, float time)
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
        public void ResetFlash()
        {
            bgMtrl.SetColor("_AddColor", Color.black);
        }
        #endregion
        #endregion
    }
}