using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RightButtonBehaivour : MonoBehaviour
{
    #region Field;
    public Button[] inactiveButtons;
    const int M_WILL_ACTIVE_BUTTON_MAX_INDEX = 4;

    bool m_isMenuShown = false;
    #endregion

    // Start is called before the first frame update
    #region Unity Methods
    void Start()
    {
        for(int i = 0; i < inactiveButtons.Length; i++)
        {
            inactiveButtons[i].gameObject.SetActive(false);
        }
    }
    #endregion

    #region Methods
    public void onClick()
    {
        for (int i = 0; i <= M_WILL_ACTIVE_BUTTON_MAX_INDEX; i++)
        {
            inactiveButtons[i].gameObject.SetActive(!m_isMenuShown);
        }
        m_isMenuShown = !m_isMenuShown;
    }
    #endregion

}

