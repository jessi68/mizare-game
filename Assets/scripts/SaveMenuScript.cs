using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveMenuScript : MonoBehaviour
{
    #region Field
    public Button[] incidentRecordButtons;
    bool m_isMenuShown = false;
    #endregion
    // Start is called before the first frame update

    #region Methods
    public void onClick()
    {
       
        Debug.Log(m_isMenuShown);
        for (int i = 0; i < incidentRecordButtons.Length; i++)
        {
            incidentRecordButtons[i].gameObject.SetActive(!m_isMenuShown);
        }
        m_isMenuShown = !m_isMenuShown;
    }
    #endregion
}
