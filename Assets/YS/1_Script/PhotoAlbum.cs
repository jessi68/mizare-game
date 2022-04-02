using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotoAlbum : MonoBehaviour
{
    #region Field
    private List<Sprite> sprites;
    public GameObject[] shownPictures;
    int currentFirstIndex = 0;
    int finalIndex = 2;

    public Button leftButton;
    public Button rightButton;
    #endregion

    // Start is called before the first frame update

    #region Unity methods


    void Start()
    {

        sprites = new List<Sprite>();

        // dumi photo 넣는 코드
        this.add(Resources.Load<Sprite>("dumiPhotos/one"));
        this.add(Resources.Load<Sprite>("dumiPhotos/two"));
        this.add(Resources.Load<Sprite>("dumiPhotos/three"));
        this.add(Resources.Load<Sprite>("dumiPhotos/four"));
        this.add(Resources.Load<Sprite>("dumiPhotos/five"));

        // 처음에는 비활성화된 상태 
        this.transform.gameObject.SetActive(false);

        leftButton.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {

    }
    #endregion

    #region Methods

    bool isPreiviousPageExist()
    {
        return currentFirstIndex >= shownPictures.Length;
    }

    bool isNextPageExist()
    {
        return currentFirstIndex + shownPictures.Length < sprites.Count;
    }

    void add(Sprite sprite)
    {
        if(sprites.Count < shownPictures.Length)
        {
            shownPictures[sprites.Count].GetComponent<Image>().sprite = sprite;
        }
        sprites.Add(sprite);
    }

    private void changeShownPictures()
    {
        for (int i = currentFirstIndex; i <= finalIndex; i++)
        {
            if(i >= sprites.Count) 
            {
                shownPictures[i - currentFirstIndex].GetComponent<Image>().sprite = null;
                continue;
            }
            shownPictures[i - currentFirstIndex].GetComponent<Image>().sprite = sprites[i];
        }
    }
    
    public void goToPreivousPage()
    {
        finalIndex = currentFirstIndex - 1;
        currentFirstIndex -= shownPictures.Length;
        changeShownPictures();

        if (!isPreiviousPageExist())
        {
            leftButton.enabled = false;
        }

        if(isNextPageExist())
        {
            rightButton.enabled = true;
        }
       
    }

    public void goToNextPage()
    {
        currentFirstIndex += shownPictures.Length;
        finalIndex = currentFirstIndex + shownPictures.Length - 1;
        changeShownPictures();

        if(!isNextPageExist())
        {
            rightButton.enabled = false;
        }

        if(isPreiviousPageExist())
        {
            leftButton.enabled = true;
        }
    }
    #endregion

}
