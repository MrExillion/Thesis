using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SensorInputPrototype.MixinInterfaces;
public class ComicManagerMixin : MonoBehaviour, MComicManager, IGlobalReferenceManager
{
    internal int currentPanel;
    internal int currentChapter;
    internal int currentPage;
    internal int currentComic;
    internal int nextPanel;
    internal int nextChapter;
    internal int nextPage;
    internal int nextComic;
    internal int previousPanel;
    internal int previousChapter;
    internal int previousPage;
    internal int previousComic;

    //public static ComicManagerTemplate template;
    private void Start()
    {
        //if (template == null)
        //    template = gameObject.GetComponent<ComicManagerTemplate>();
        //else
        //    return;        
    }

    private void Update()
    {
        //template.UpdateTransitionTypeLine(template);
    }

    public void TemporaryComicManagerPropGet()
    {



    }
    
}
