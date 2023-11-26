using SensorInputPrototype.MixinInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UIElements;

public static class PageManager
{
    private static ConditionalWeakTable<MPageManager, Fields> table;
    private static List<Tuple<GameObject, PageManagerTemplate, int,
        List<Tuple<GameObject, PanelManagerTemplate, int,
            List<Tuple<GameObject, UniversalPanel, int, int
    >>>>>> pages;

    private static List<Tuple<GameObject, PanelManagerTemplate, int,
        List<Tuple<GameObject, UniversalPanel, int, int
    >>>> panelsListTobeSaved;
    static PageManager()
    {
        pages = new List<Tuple<GameObject, PageManagerTemplate, int, List<Tuple<GameObject, PanelManagerTemplate, int, List<Tuple<GameObject, UniversalPanel, int, int>>>>>>();

        panelsListTobeSaved = new List<Tuple<GameObject, PanelManagerTemplate, int, List<Tuple<GameObject, UniversalPanel, int, int>>>>();




        table = new ConditionalWeakTable<MPageManager, Fields>();

    }
    private sealed class Fields : ComicManagerTemplate, MTransition
    {
        internal PageManagerMixin mixin;
    }
    public static void InitializePageManager(this MPageManager map)
    {

        //table.GetOrCreateValue(map).GetComponentOrAdd<PageManagerMixin>();
        if (table.GetOrCreateValue(map).mixin == null)
        {
            table.GetOrCreateValue(map).mixin = table.GetOrCreateValue(map).GetComponentOrAdd<PageManagerMixin>();
        }
        
    }

    public static 
        List<Tuple<GameObject, PanelManagerTemplate, int,
            List<Tuple<GameObject, UniversalPanel, int, int
        >>>>
        TrackPages(this MPageManager map, List<GameObject> pageObjects)
    {
        foreach(GameObject gameObject in pageObjects)
        {
            panelsListTobeSaved.Add(PanelManager.getPanelList().Find(x => x.Item2.GetInstanceID()
            == gameObject.GetComponent<PanelManagerTemplate>().GetInstanceID()));
        }
        return panelsListTobeSaved;
    }

    public static 
        List<Tuple<GameObject, PageManagerTemplate, int,
            List<Tuple<GameObject, PanelManagerTemplate, int,
                List<Tuple<GameObject, UniversalPanel, int, int
        >>>>>> 
        getPageList()
    {

        return pages;
    }

    public static void InitializeComicStructure_pages(this MChapterManager map,
        GameObject gameObject,
        PageManagerTemplate pageManagerTemplate,
        int pageID,
        
        List<Tuple<GameObject, PanelManagerTemplate, int,
            List<Tuple<GameObject, UniversalPanel, int, int
         >>>> pagesTupleList)
    {
        Tuple<GameObject, PageManagerTemplate, int,
            List<Tuple<GameObject, PanelManagerTemplate, int,
            List<Tuple<GameObject, UniversalPanel, int, int
        >>>>> pagesTuple;

        pagesTuple = new Tuple<GameObject, PageManagerTemplate, int,
            List<Tuple<GameObject, PanelManagerTemplate, int,
            List<Tuple<GameObject, UniversalPanel, int, int
        >>>>>(gameObject, pageManagerTemplate, pageID, pagesTupleList);

        pages.Add(pagesTuple);
    }



}

