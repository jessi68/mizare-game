using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace YS
{
    public class SlideEffect : MonoBehaviour
    {
        public float speed = 7.5f;

        private RectTransform rt;
        private Vector2 dest;
        private bool isInDest = true;

        void Start()
        {
            rt = transform as RectTransform;
        }

        // Update is called once per frame
        void Update()
        {
            if (!isInDest)
            {
                rt.anchoredPosition = Vector2.Lerp(rt.anchoredPosition, dest, speed * Time.deltaTime);
                if ((rt.anchoredPosition - dest).sqrMagnitude < 1.0f)
                {
                    rt.anchoredPosition = dest;
                    isInDest = true;
                }
            }
        }

        /// <summary>
        /// dest 위치로 이동
        /// </summary>
        /// <param name="dest">이동할 위치</param>
        public void SetSlide(Vector2 dest)
        {
            this.dest = dest;
            isInDest = false;
        }
    }
}