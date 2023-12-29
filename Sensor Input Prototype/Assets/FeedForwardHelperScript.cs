using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class FeedForwardHelperScript : MonoBehaviour
{


    //SpriteLibrary sprites;
    //private IEnumerable<string> strings;
    //private string[] strs;
    SpriteRenderer spriteRenderer;
    public List<Sprite> sprites;
    public List<string> lables;



    // Start is called before the first frame update
    void Start()
    {
        //sprites = GetComponent<SpriteLibrary>();
        //strings = sprites.spriteLibraryAsset.GetCategoryLabelNames("HelperUi");
        //strs = new string[strings.Count()];
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //strs = strings.ToArray();

        //spriteRenderer.sprite = sprites.GetSprite("HelperUi", strs[GlobalReferenceManager.GetCurrentUniversalPanel().transitionType]);
        spriteRenderer.sprite = sprites[GlobalReferenceManager.GetCurrentUniversalPanel().transitionType];
        GetComponentInChildren<TextMeshPro>().text = lables[GlobalReferenceManager.GetCurrentUniversalPanel().transitionType];
    }
}
