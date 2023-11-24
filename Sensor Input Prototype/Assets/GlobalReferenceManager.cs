using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Runtime.CompilerServices;
using SensorInputPrototype.MixinInterfaces;
using NUnit.Framework;
using System;

public static class GlobalReferenceManager 
{

    [SerializeField] public static ConditionalWeakTable<IGlobalReferenceManager, Fields> testCWT;
    [SerializeField] public static List<Tuple<int, UnityEngine.Component>> MixinPairs;
    private static ComicManagerTemplate activeComicTemplate = null;
    private static ChapterManagerTemplate activeChapterTemplate = null;
    private static PageManagerTemplate activePageTemplate = null;
    private static PanelManagerTemplate activePanelTemplate = null;
    static GlobalReferenceManager()
    {
         testCWT = new ConditionalWeakTable<IGlobalReferenceManager, Fields>();
        MixinPairs = new List<Tuple<int, UnityEngine.Component>>();
        

    }
    public sealed class Fields : Editor, IGlobalReferenceManager
    {
        internal int stuff;
        

    }
    [ExecuteInEditMode]
    public sealed class Fields2 : MonoBehaviour, IGlobalReferenceManager
    {

        void Update()
        {
            testCWT.GetOrCreateValue(this).stuff = 10000;
            testCWT.AddOrUpdate(this,testCWT.GetOrCreateValue(this));
            
            
            

        }


    }
    public static void SetActiveComic(ComicManagerTemplate comicManagerTemplate)
    {
        
            GlobalReferenceManager.activeComicTemplate = comicManagerTemplate;
        
    }
    public static UnityEngine.Component GetActiveComicTemplate()
    {

        return GlobalReferenceManager.activeComicTemplate;

    }
    public static void SetActiveChapter(ChapterManagerTemplate chapterManagerTemplate)
    {

        GlobalReferenceManager.activeChapterTemplate = chapterManagerTemplate;

    }
    public static UnityEngine.Component GetActiveChapterTemplate()
    {

        return GlobalReferenceManager.activeChapterTemplate;

    }
    public static void SetActivePage(PageManagerTemplate pageManagerTemplate)
    {

        GlobalReferenceManager.activePageTemplate = pageManagerTemplate;

    }
    public static UnityEngine.Component GetActivePageTemplate()
    {

        return GlobalReferenceManager.activePageTemplate;

    }
    public static void SetActivePanel(PanelManagerTemplate panelManagerTemplate)
    {

        GlobalReferenceManager.activePanelTemplate = panelManagerTemplate;

    }
    public static UnityEngine.Component GetActivePanelTemplate()
    {

        return GlobalReferenceManager.activePanelTemplate;

    }


}

public interface IGlobalReferenceManager
{



}
