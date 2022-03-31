using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotoAlbum : MonoBehaviour
{
    #region Field
    private List<Sprite> sprites;
    public GameObject[] shownPictures;
    int currentIndex = 0;
    #endregion

    // Start is called before the first frame update

    #region Unity methods


    void Start()
    {

        sprites = new List<Sprite>();
        this.add(Resources.Load<Sprite>("dumiPhotos/one"));
        this.add(Resources.Load<Sprite>("dumiPhotos/two"));
        this.add(Resources.Load<Sprite>("dumiPhotos/three"));
        this.add(Resources.Load<Sprite>("dumiPhotos/four"));
        this.add(Resources.Load<Sprite>("dumiPhotos/five"));
        this.transform.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
    #endregion

    #region Methods
    void add(Sprite sprite)
    {
        if(sprites.Count < shownPictures.Length)
        {
            shownPictures[sprites.Count].GetComponent<Image>().sprite = sprite;
        }
        sprites.Add(sprite);
    }

    
    public void goToPreivousPage()
    {
        currentIndex -= shownPictures.Length;
    }

    public void goToNextPage()
    {
        currentIndex += shownPictures.Length;
    }
    #endregion

}
