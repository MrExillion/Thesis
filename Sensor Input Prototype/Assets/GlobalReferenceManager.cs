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


    #region MixinHandlers
    public static void AddNewMixin<T>(this IGlobalReferenceManager mixinHost, GameObject g) where T : UnityEngine.Component
    {

        //Debug.Log(table.GetOrCreateValue(mixinHost).GetComponentOrAdd_pseudoOverride1<T>());

        T makeSureComponentExistFirst = GetComponentOrAdd_pseudoOverride1<T>(g);
        GlobalReferenceManager.MixinPairs.Add(new Tuple<int, UnityEngine.Component>(makeSureComponentExistFirst.GetInstanceID(), makeSureComponentExistFirst));

    }






    #endregion
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

}

namespace SensorInputPrototype.MixinInterfaces{
    public interface IGlobalReferenceManager : MThesisAPI 
    {
        

    }
}