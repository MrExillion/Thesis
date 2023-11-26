using SensorInputPrototype.InspectorReadOnlyCode;
using SensorInputPrototype.MixinInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Unity.VisualScripting;
using System.ComponentModel;

public static class ChapterManager

{
#if UNITY_EDITOR
    [ShowOnly]
#endif
    [SerializeField]
    private static ConditionalWeakTable<MChapterManager, Fields> table;
    private static List<Tuple<GameObject, ChapterManagerTemplate, int, List<Tuple<GameObject, PageManagerTemplate, int, List<Tuple<GameObject, PanelManagerTemplate, int, List<Tuple<GameObject, UniversalPanel, int, int>>>>>>>> chapters;
    private static List<Tuple<GameObject, PageManagerTemplate, int, List<Tuple<GameObject, PanelManagerTemplate, int, List<Tuple<GameObject, UniversalPanel, int, int>>>>>> pageListToBeSaved;
    static ChapterManager()
    {

        table = new ConditionalWeakTable<MChapterManager, Fields>();
        //table.NewChapter();
        chapters = new List<Tuple<GameObject, ChapterManagerTemplate, int, List<Tuple<GameObject, PageManagerTemplate, int, List<Tuple<GameObject, PanelManagerTemplate, int, List<Tuple<GameObject, UniversalPanel, int, int>>>>>>>>();
        pageListToBeSaved = new List<Tuple<GameObject, PageManagerTemplate, int, List<Tuple<GameObject, PanelManagerTemplate, int, List<Tuple<GameObject, UniversalPanel, int, int>>>>>>();

    }
    private sealed class Fields : MonoBehaviour, MThesisAPI
    {
        internal GameObject[] chapterObjects = { };
        internal ChapterManagerMixin mixin;
    }
    //public static void NewChapter(this MChapterManager map, ChapterManagerMixin chapterManagerMixin)
    //{

    //    GameObject[] ChapterManagerMixins = chapterManagerMixin.GetChapterOrder();
    //    for (int i = 0; i < ChapterManagerMixins.Length; i++)
    //    {




    //            //GetPages(gameObject.GetComponentInParent<PageManagerMixin>().Get()).SetValue(panelObjects, table.GetOrCreateValue(map).pageList.Length);
    //    }
    //    chapterManagerMixin.gameObject.GetComponentInParent<ComicManagerMixin>().UpdateChapterList(ChapterManagerMixins);


    //}
    public static void InitializeChapterManager(this MChapterManager map)
    {

        if (table.GetOrCreateValue(map).mixin == null)
        {
            table.GetOrCreateValue(map).mixin = table.GetOrCreateValue(map).GetComponentOrAdd<ChapterManagerMixin>();
        }

    }


    //public static Array GetChapterOrder(this MChapterManager map)
    //{
    //    return table.GetOrCreateValue(map).chapterOrder;
    //}


    public static void InitializeComicStructure_chapters(this MComicManager map, GameObject gameObject, ChapterManagerTemplate chapterManagerTemplate, int chapterID,
        System.Collections.Generic.List<Tuple<GameObject, PageManagerTemplate, int, List<Tuple<GameObject, PanelManagerTemplate, int, List<Tuple<GameObject, UniversalPanel, int, int>>>>>> pageTupleList)
    {
        Tuple<GameObject, ChapterManagerTemplate, int, List<Tuple<GameObject, PageManagerTemplate, int, List<Tuple<GameObject, PanelManagerTemplate, int, List<Tuple<GameObject, UniversalPanel, int, int>>>>>>> pageTuple;

        pageTuple = new Tuple<GameObject, ChapterManagerTemplate, int, List<Tuple<GameObject, PageManagerTemplate, int, List<Tuple<GameObject, PanelManagerTemplate, int, List<Tuple<GameObject, UniversalPanel, int, int>>>>>>>(gameObject, chapterManagerTemplate, chapterID, pageTupleList);
        chapters.Add(pageTuple);
    }

    public static List<Tuple<GameObject, PageManagerTemplate, int, List<Tuple<GameObject, PanelManagerTemplate, int, List<Tuple<GameObject, UniversalPanel, int, int>>>>>> TrackChapters(this MChapterManager map, List<GameObject> chapterObjects)
    {

        foreach (GameObject gameObject in chapterObjects)
        {
            //    PageManagerTemplate pageManagerTemplate = gameObject.GetComponent<PageManagerTemplate>();
            //    //pageListToBeSaved.Add(gameObject, pageManagerTemplate, pageManagerTemplate.panelId, panelManagerTemplate.)
            Debug.Log(PageManager.getPageList().Count);
            pageListToBeSaved.Add(PageManager.getPageList().Find(x => x.Item2.GetInstanceID() == gameObject.GetComponent<PageManagerTemplate>().GetInstanceID()));
            //    //GlobalReferenceManager.MixinPairs[(GlobalReferenceManager.MixinPairs.LastIndexOf(GlobalReferenceManager.MixinPairs.FindLast(x=> TypeDescriptor.GetClassName(x.Item2) == "PageManagerMixin")) - 1)].Item2.GetComponent<PageManagerTemplate>()
            //    pageManagerTemplate.
        }
        //for(int i = 0; i < chapterObjects.Count; i++)
        //{
        //    pageListToBeSaved.Add(chapterObjects[i].GetComponent<PageManagerTemplate>().GetPage());
        //}


        return pageListToBeSaved;
    }
    public static
        List<Tuple<GameObject, ChapterManagerTemplate, int,
          List<Tuple<GameObject, PageManagerTemplate, int,
             List<Tuple<GameObject, PanelManagerTemplate, int,
                List<Tuple<GameObject, UniversalPanel, int, int
                    >>>>>>>>
        getChapterList()
    {

        return chapters;
    }




}