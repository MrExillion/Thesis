using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Runtime.CompilerServices;
using SensorInputPrototype.MixinInterfaces;
using NUnit.Framework;
using System;
using Unity.VisualScripting;
using System.ComponentModel;

public static class GlobalReferenceManager
{

    private static ConditionalWeakTable<IGlobalReferenceManager, Fields> table;
    [SerializeField] public static List<Tuple<int, UnityEngine.Component>> MixinPairs;
    private static ComicManagerTemplate activeComicTemplate = null;
    private static ChapterManagerTemplate activeChapterTemplate = null;
    private static PageManagerTemplate activePageTemplate = null;
    private static PanelManagerTemplate activePanelTemplate = null;
    private static ComicManagerMixin comicManagerMixin = null;
    private delegate void GlobalRefInitCallBack();
    

    //private static IDictionary dictionary; Dictionary still has a mix type issue in my use case, i want a single list though i could proably do Dictionary<int, UnityEngine.Component>> MixinPairs with the same style of implementation. But getin the mixin in question could be either more coplex computation or less. LookUp would looklike this as Dictionary: GlobalReferenceManager.MixinPairs[instance ID i am looking to match].Foobar() || (...) ].property || (...)].fields etc. On a code readability level this is easier. However I do not know if Add works nicely with this.


    static GlobalReferenceManager()
    {
        //testCWT = new ConditionalWeakTable<IGlobalReferenceManager, Fields>();
        MixinPairs = new List<Tuple<int, UnityEngine.Component>>();
        table = new ConditionalWeakTable<IGlobalReferenceManager, Fields>();
        // dictionary = new Dictionary<int, ComicManagerTemplate>();
        
    }
    private sealed class Fields : MonoBehaviour, IGlobalReferenceManager, MThesisAPI
    {
        //internal int stuff;
        internal ComicManagerTemplate comicManagerTemplate1 = null;
        internal List<GameObject> defactoPanelList = new List<GameObject>();
        internal bool _rayHitFinalPanel = false;

    }
    //[ExecuteInEditMode]
    //public sealed class Fields2 : MonoBehaviour, IGlobalReferenceManager, MThesisAPI
    //{

    //    void Update()
    //    {
    //        //testCWT.GetOrCreateValue(this).stuff = 10000;
    //        //testCWT.AddOrUpdate(this,testCWT.GetOrCreateValue(this));




    //    }


    //}

    #region GlobalReferenceManagerComponent
    /// <summary>
    /// Initiates GlobalReferenceManager as a Usable Interface, Must run AFTER the comics has been built. Using AddNewMixin etc. And Prior to CamaraSequencer.
    /// </summary>
    /// <param name="map"></param>
    /// <param name="gameObject"></param>
    public static void GlobalReferenceManagerInit(this IGlobalReferenceManager map,GameObject gameObject)
    {
        table.GetOrCreateValue(map).comicManagerTemplate1 = gameObject.GetComponent<GlobalRefManagerComponent>().primaryComic.GetComponent<ComicManagerTemplate>();

        SetActiveComicContainer(table.GetOrCreateValue(map).comicManagerTemplate1); 
        SetActiveChapterContainer((GetActiveComicTemplate() as ComicManagerTemplate).comicsList[0].GetComponent<ChapterManagerTemplate>());
        SetActivePageContainer((GetActiveChapterTemplate() as ChapterManagerTemplate).chapterOrder[0].GetComponent<PageManagerTemplate>());
        SetActivePanelContainer((GetActivePageTemplate() as PageManagerTemplate).pageOrder[0].GetComponent<PanelManagerTemplate>());
        SetGlobalReferenceManagerComicManagerMixinPvt();
    }
    #endregion



    #region MixinHandlers
    /// <summary>
    /// This is most likely completely unusefull but its added as a precaution.
    /// </summary>
    private static void SetGlobalReferenceManagerComicManagerMixinPvt()
    {
        GlobalReferenceManager.comicManagerMixin = GetActiveComicTemplate().GetComponent<ComicManagerMixin>();
    }

    public static void AddNewMixin<T>(this IGlobalReferenceManager mixinHost, GameObject g) where T : UnityEngine.Component
    {

        //Debug.Log(table.GetOrCreateValue(mixinHost).GetComponentOrAdd_pseudoOverride1<T>());

        T makeSureComponentExistFirst = GetComponentOrAdd_pseudoOverride1<T>(g);
        GlobalReferenceManager.MixinPairs.Add(new Tuple<int, UnityEngine.Component>(makeSureComponentExistFirst.GetInstanceID(), makeSureComponentExistFirst));

    }






    #endregion
    /// <summary>
    /// Sets the Comic Container for later use, This should NEVER be instantiated as more than 1 per scene.
    /// It holds a List of comics which can be itterated through when "reading" comics in a public build of the program.
    /// </summary>
    /// <param name="comicManagerTemplate"></param>
    public static void SetActiveComicContainer(ComicManagerTemplate comicManagerTemplate)
    {

        GlobalReferenceManager.activeComicTemplate = comicManagerTemplate;

    }
    /// <summary>
    /// Gets The Active Comic Container which has a list of Comics or ComicManagerTemplates.
    /// Use Casting to type if you need access to functions implemented by this template
    /// </summary>
    /// <returns> UnityEngine.Component</returns>
    public static UnityEngine.Component GetActiveComicTemplate()
    {

        return GlobalReferenceManager.activeComicTemplate;

    }
    /// <summary>
    /// Sets the Active Comic, which holds a list of chapters.
    /// </summary>
    /// <param name="chapterManagerTemplate"></param>
    public static void SetActiveChapterContainer(ChapterManagerTemplate chapterManagerTemplate)
    {

        GlobalReferenceManager.activeChapterTemplate = chapterManagerTemplate;

    }
    /// <summary>
    /// Gets The Active Comic which has a list of Chapters or ChapterManagerTemplates.
    /// Use Casting to type if you need access to functions implemented by this template
    /// </summary>
    /// <returns> UnityEngine.Component</returns>
    public static UnityEngine.Component GetActiveChapterTemplate()
    {

        return GlobalReferenceManager.activeChapterTemplate;

    }
    /// <summary>
    /// Sets the Active Chapter, which holds a list of pages. Class name for this is PageManagerTemplate Because it Manages Pages in the Chapter, which it itself is an instance of.
    /// </summary>
    /// <param name="pageManagerTemplate"></param>
    public static void SetActivePageContainer(PageManagerTemplate pageManagerTemplate)
    {

        GlobalReferenceManager.activePageTemplate = pageManagerTemplate;

    }
    /// <summary>
    /// Gets The Active Chapter which has a list of Pages or PageManagerTemplates.
    /// Use Casting to type if you need access to functions implemented by this template
    /// </summary>
    /// <returns> UnityEngine.Component</returns>
    public static UnityEngine.Component GetActivePageTemplate()
    {

        return GlobalReferenceManager.activePageTemplate;

    }
    /// <summary>
    /// Sets Active Page, Which holds a list of Panels or PanelManagerTemplates which holds a list of GameObjects with the UniversalPanel Component, The PanelManagerTemplate is a Page Container.
    /// </summary>
    /// <param name="panelManagerTemplate"></param>
    public static void SetActivePanelContainer(PanelManagerTemplate panelManagerTemplate)
    {

        GlobalReferenceManager.activePanelTemplate = panelManagerTemplate;

    }
    /// <summary>
    /// Gets The Active Page which has a list of PanelManagerTemplates.
    /// Use Casting to type if you need access to functions implemented by this template
    /// </summary>
    /// <returns> UnityEngine.Component</returns>
    public static UnityEngine.Component GetActivePanelTemplate()
    {

        return GlobalReferenceManager.activePanelTemplate;

    }

    /// <summary>
    /// Pseudo Override of GetComponentOrAdd which handles MixinIn Registration poorly, hence the reason for an override that takes 1 argument also, but of a different type aka GameObject not an Interface Map.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="gameObject"></param>
    /// <returns> T as UnityEngine.Component </returns>
    public static T GetComponentOrAdd_pseudoOverride1<T>(GameObject gameObject) where T : UnityEngine.Component 
        //where S : UnityEngine.GameObject
    {

        //T component = table.GetOrCreateValue(
        //(MThesisAPI)host.GetType().GetInterface("MThesisAPI")).gameObject.GetComponent<T>() as T;
        //T componentout;
        gameObject.TryGetComponent(out T cpnt);
        
        
        if (cpnt == false)
        {

            T component = gameObject.AddComponent<T>();
                return component;
        }

        return cpnt;

    }

    /// <summary>
    /// Gets The Active Panel as in the UniversalPanel Not as Component, but as the UniversalPanel class, no casting nessecary.
    /// </summary>
    /// <returns> UniversalPanel as Class/Type  </returns>
    public static UniversalPanel GetCurrentUniversalPanel()
    {
        GameObject gameObjectOfPage = GetActivePanelTemplate().gameObject;
        return gameObjectOfPage.GetComponent<PanelManagerTemplate>().panelOrder[
            (GlobalReferenceManager.MixinPairs.Find(x => x.Item1 == GetActiveComicTemplate().GetComponent<ComicManagerMixin>().GetInstanceID()).Item2 as ComicManagerMixin).currentPanel
            ].GetComponent<UniversalPanel>();

    }


    [System.Obsolete]public static int GetDefactoPanelId(UniversalPanel component) 
    {
        int currentPanelNum = 0;

        PanelManagerTemplate page = component.GetComponentInParent<PanelManagerTemplate>();
        PageManagerTemplate chapter = page.GetComponentInParent<PageManagerTemplate>();
        ChapterManagerTemplate comic = chapter.GetComponentInParent<ChapterManagerTemplate>();
        ComicManagerTemplate container = comic.GetComponentInParent<ComicManagerTemplate>();

        for (int i = container.containerId; i >= 0; i--)
        {
            if (i < container.containerId)
            {
                for (int j = container.comicsList[i].GetComponent<ChapterManagerTemplate>().chapterOrder.Count; j >= 0; j--)
                {
                    if (j < chapter.chapterId)
                    {
                        for (int k = chapter.pageOrder[j].GetComponent<PageManagerTemplate>().pageOrder.Count; k >= 0; k--)
                        {
                            if (k < page.pageId)
                            {
                                currentPanelNum += container.comicsList[i].GetComponent<ChapterManagerTemplate>().chapterOrder[j].GetComponent<PageManagerTemplate>().pageOrder[k].GetComponent<PanelManagerTemplate>().panelOrder.Count;
                            }
                            else
                            {
                                currentPanelNum += component.PanelId;
                            }
                        }
                    }
                    else
                    {
                        for (int k = page.pageId; k >= 0; k--)
                        {
                            if (k < page.pageId)
                            {
                                currentPanelNum += container.comicsList[i].GetComponent<ChapterManagerTemplate>().chapterOrder[j].GetComponent<PageManagerTemplate>().pageOrder[k].GetComponent<PanelManagerTemplate>().panelOrder.Count;
                            }
                            else
                            {

                                currentPanelNum += component.PanelId;
                            }
                        }
                    }
                }
            }
            else
            {
                for (int j = chapter.chapterId; j >= 0; j--)
                {
                    if (j < chapter.chapterId)
                    {
                        for (int k = chapter.pageOrder[j].GetComponent<PageManagerTemplate>().pageOrder.Count; k >= 0; k--)
                        {
                            if (k < page.pageId)
                            {
                                currentPanelNum += chapter.pageOrder[k].GetComponent<PanelManagerTemplate>().panelOrder.Count;

                            }
                            else
                            {
                                currentPanelNum += component.PanelId;
                            }
                        }

                    }
                    else
                    {
                        for (int k = page.pageId; k >= 0; k--)
                        {
                            if (k < page.pageId)
                            {
                                currentPanelNum += chapter.pageOrder[k].GetComponent<PanelManagerTemplate>().panelOrder.Count;
                            }
                            else
                            {
                                currentPanelNum += component.PanelId;
                            }

                        }
                    }
                }

            }


        }
        return currentPanelNum;
    }

    public static int GetDefactoPanelId(this IGlobalReferenceManager globalRef, UniversalPanel component)
    {

        return table.GetOrCreateValue(globalRef).defactoPanelList.IndexOf(component.gameObject);
    }
    public static void SetDefactorPanelIds(this IGlobalReferenceManager globalRef, ComicManagerTemplate comicManagerTemplate)
    {
 
        List<GameObject> defactoPanelList = new List<GameObject>();

        List<GameObject> comics = comicManagerTemplate.comicsList;
        //List<GameObject> chapterGObjs = comicManagerTemplate.comicsList[comicManagerTemplate.comicsList.Count - 1].GetComponent<ChapterManagerTemplate>().chapterOrder;
        //List<GameObject> pageGObjs = chapterGObjs[chapterGObjs.Count-1].GetComponent<PageManagerTemplate>().pageOrder;
        //UniversalPanel lastPanel = pageGObjs[pageGObjs.Count-1].GetComponent<UniversalPanel>();
     

        for(int i = 0; i < comics.Count; i++)
        {
            for (int j = 0; j < comics[i].GetComponent<ChapterManagerTemplate>().chapterOrder.Count; j++)
            {
                for(int k = 0; k < comics[i].GetComponent<ChapterManagerTemplate>().chapterOrder[j].GetComponent<PageManagerTemplate>().pageOrder.Count; k++)
                {
                    for(int l = 0; l < comics[i].GetComponent<ChapterManagerTemplate>().chapterOrder[j].GetComponent<PageManagerTemplate>().pageOrder[k].GetComponent<PanelManagerTemplate>().panelOrder.Count;l++) 
                    {
                        defactoPanelList.Add(comics[i].GetComponent<ChapterManagerTemplate>().chapterOrder[j].GetComponent<PageManagerTemplate>().pageOrder[k].GetComponent<PanelManagerTemplate>().panelOrder[l]);
                    }
                }
            }
        }
         table.GetOrCreateValue(globalRef).defactoPanelList = new(defactoPanelList);
    }

    public static bool rayHitFinalPanel(this IGlobalReferenceManager m)
    {

        return table.GetOrCreateValue(m)._rayHitFinalPanel;
    }
    public static void rayHitFinalPanel(this IGlobalReferenceManager m,bool value)
    {

         table.GetOrCreateValue(m)._rayHitFinalPanel = value;
    }


}




namespace SensorInputPrototype.MixinInterfaces{
    public interface IGlobalReferenceManager : MThesisAPI 
    {
        

    }
}