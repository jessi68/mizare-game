using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadingManager : MonoBehaviour
{
    // Start is called before the first frame update

    string loadingMessage = "Loading";

    [SerializeField]
    TextMeshProUGUI loadingText;

    void Start()
    {
        
        Debug.Log(loadingText);
        showLoadingMessage();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void showLoadingMessage()
    {
        StartCoroutine(graduallyLoadingMessage());
    }

    private IEnumerator graduallyLoadingMessage()
    {
        for (int i = 0; i < loadingMessage.Length; i++)
        {
            loadingText.text += loadingMessage[i];
            yield return new WaitForSeconds(0.1f);
        }
    }
}
