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

public static class PageManager
{
    private static ConditionalWeakTable<MPageManager, Fields> table;
    private static List<Tuple<GameObject, PageManagerTemplate, int, List<Tuple<GameObject, PanelManagerTemplate, int, List<Tuple<GameObject, UniversalPanel, int, int>>>>>> pages;
    
    static PageManager()
    {

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





    public static void InitializeComicStructure_pages(this MChapterManager map, GameObject gameObject, PageManagerTemplate pageManagerTemplate, int pageID,
        System.Collections.Generic.List<Tuple<GameObject, PanelManagerTemplate, int, List<Tuple<GameObject, UniversalPanel, int, int>>>> pagesTupleList)
    {
        Tuple<GameObject, PageManagerTemplate, int, List<Tuple<GameObject, PanelManagerTemplate, int, List<Tuple<GameObject, UniversalPanel, int, int>>>>> pagesTuple;

        pagesTuple = new Tuple<GameObject, PageManagerTemplate, int, List<Tuple<GameObject, PanelManagerTemplate, int, List<Tuple<GameObject, UniversalPanel, int, int>>>>>(gameObject, pageManagerTemplate, pageID, pagesTupleList);
        pages.Add(pagesTuple);
    }

}

