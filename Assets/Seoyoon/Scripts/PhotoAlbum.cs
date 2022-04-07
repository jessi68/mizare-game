using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotoAlbum : MonoBehaviour
{
    #region Field
    private List<Sprite> sprites;
    public Image[] shownPictures;
    int currentPageIndex = 0;
    int lastPageIndex;

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

        lastPageIndex = (int) Mathf.Ceil((float) sprites.Count / shownPictures.Length) - 1;
   
        leftButton.interactable = false;

        if(!IsNextPageExist())
        {
            rightButton.interactable = false;
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
    #endregion

    #region Methods

    bool IsPreiviousPageExist()
    {
        return currentPageIndex >= 1;
    }

    bool IsNextPageExist()
    {
        return currentPageIndex < lastPageIndex;
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
        int firstIndex = currentPageIndex * shownPictures.Length;
        int finalIndex = firstIndex + shownPictures.Length - 1;

        for (int i = firstIndex; i <= finalIndex; i++)
        {
            if(i >= sprites.Count) 
            {
                shownPictures[i - firstIndex].sprite = null;
                continue;
            }
            shownPictures[i - firstIndex].sprite = sprites[i];
        }
    }
    
    public void GoToPreivousPage()
    {
        currentPageIndex--;
        
        ChangeShownPictures();

        if (!IsPreiviousPageExist())
        {
            leftButton.interactable = false;
        }

        if(IsNextPageExist())
        {
            rightButton.interactable = true;
        }
       
    }

    public void GoToNextPage()
    {
        currentPageIndex++;

        ChangeShownPictures();

        if(!IsNextPageExist())
        {
            rightButton.interactable = false;
        }

        if(IsPreiviousPageExist())
        {
            leftButton.interactable = true;
        }
    }
    #endregion

}
