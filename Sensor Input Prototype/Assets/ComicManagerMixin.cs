using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SensorInputPrototype.MixinInterfaces;
public class ComicManagerMixin : MonoBehaviour, MComicManager, IGlobalReferenceManager
{
    public int currentPanel = 0;
    public int currentChapter = 0;
    public int currentPage = 0;
    public int currentComic = 0;
    public int nextPanel = 1;
    public int nextChapter = 1;
    public int nextPage = 1;
    public int nextComic = 1;
    public int previousPanel = -1;
    public int previousChapter = -1;
    public int previousPage =  -1;
    public int previousComic = -1;

    public static ComicManagerMixin mixin;
    private void Start()
    {
        if (mixin == null)
            mixin = this;
            
        else
            return;
    }

    private void Update()
    {
        //mixin.UpdateTransitionTypeLine(mixin);
        

    }

    public void TemporaryComicManagerPropGet()
    {



    }
    
}
