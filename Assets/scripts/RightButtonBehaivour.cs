using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RightButtonBehaivour : MonoBehaviour
{
    #region Field;
    public Button[] controlledButtons;
    const int M_WILL_ACTIVE_BUTTON_MAX_INDEX = 4;

    bool m_isMenuShown = false;
    #endregion

    #region Unity Methods
    void Start()
    {
        setActiveAtButtons(false, controlledButtons.Length - 1);
    }
    #endregion

    #region Methods
    private void setActiveAtButtons(bool activeValue, int maxIndex)
    {
        for(int i = 0; i <= maxIndex; i++)
        {
            controlledButtons[i].gameObject.SetActive(activeValue);
        }
    }

    public void onClick()
    {
        if(m_isMenuShown == true)
        {
            setActiveAtButtons(!m_isMenuShown, controlledButtons.Length - 1);
        } else
        {
            setActiveAtButtons(!m_isMenuShown, M_WILL_ACTIVE_BUTTON_MAX_INDEX);
        }
        
        m_isMenuShown = !m_isMenuShown;
    }
    #endregion

}

