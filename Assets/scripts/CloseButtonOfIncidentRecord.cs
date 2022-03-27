using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseButtonOfIncidentRecord : MonoBehaviour
{
    #region Field
    public GameObject recordIncidentParent;
    #endregion

    #region Methods
    public void onClick()
    {
        for(int i = 0; i < recordIncidentParent.transform.childCount; i++)
        {
            recordIncidentParent.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
    #endregion
    // Start is called before the first frame update

}
