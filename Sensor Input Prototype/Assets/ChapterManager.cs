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
            pageListToBeSaved.Add(PageManager.getPageList().Find(x => x.Item2.GetInstanceID() == gameObject.GetComponent<PanelManagerTemplate>().GetInstanceID()));
        }
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