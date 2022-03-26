using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RightButtonBehaivour : MonoBehaviour
{
    #region Field;
    public Button[] willActivateButtons;
    bool m_isMenuShown = false;
    #endregion

    // Start is called before the first frame update
    #region Unity Methods
    void Start()
    {
        for(int i = 0; i < willActivateButtons.Length; i++)
        {
            willActivateButtons[i].gameObject.SetActive(false);
        }
    }
    #endregion

    #region Methods
    public void onClick()
    {
        for (int i = 0; i < willActivateButtons.Length; i++)
        {
            willActivateButtons[i].gameObject.SetActive(!m_isMenuShown);
        }
        m_isMenuShown = !m_isMenuShown;
    }
    #endregion

}

