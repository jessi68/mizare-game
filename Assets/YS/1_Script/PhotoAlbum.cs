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
        this.Add(Resources.Load<Sprite>("dumiPhotos/one"));
        this.Add(Resources.Load<Sprite>("dumiPhotos/two"));
        this.Add(Resources.Load<Sprite>("dumiPhotos/three"));
        this.Add(Resources.Load<Sprite>("dumiPhotos/four"));
        this.Add(Resources.Load<Sprite>("dumiPhotos/five"));

        leftButton.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {

    }
    #endregion

    #region Methods

    bool IsPreiviousPageExist()
    {
        return currentFirstIndex >= shownPictures.Length;
    }

    bool IsNextPageExist()
    {
        return currentFirstIndex + shownPictures.Length < sprites.Count;
    }

    void Add(Sprite sprite)
    {
        if(sprites.Count < shownPictures.Length)
        {
            shownPictures[sprites.Count].GetComponent<Image>().sprite = sprite;
        }
        sprites.Add(sprite);
    }

    private void ChangeShownPictures()
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
    
    public void GoToPreivousPage()
    {
        finalIndex = currentFirstIndex - 1;
        currentFirstIndex -= shownPictures.Length;
        ChangeShownPictures();

        if (!IsPreiviousPageExist())
        {
            leftButton.enabled = false;
        }

        if(IsNextPageExist())
        {
            rightButton.enabled = true;
        }
       
    }

    public void GoToNextPage()
    {
        currentFirstIndex += shownPictures.Length;
        finalIndex = currentFirstIndex + shownPictures.Length - 1;
        ChangeShownPictures();

        if(!IsNextPageExist())
        {
            rightButton.enabled = false;
        }

        if(IsPreiviousPageExist())
        {
            leftButton.enabled = true;
        }
    }
    #endregion

}
