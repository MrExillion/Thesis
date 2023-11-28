using NUnit.Framework;
using NUnit.Framework.Constraints;
using SensorInputPrototype.InspectorReadOnlyCode;
using SensorInputPrototype.MixinInterfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows;

public static class ComicManager

{
#if UNITY_EDITOR
    [ShowOnly]
#endif
    [SerializeField]
    public static ConditionalWeakTable<MTransition, TransitionFields> transitionsTable;

    private enum comicStructureContainerClassName
    {
        UniversalPanel = 0, PanelManagerTemplate = 1, PanelManagerMixin = 2, PanelManager = 3, PageManagerTemplate = 4, PageManagerMixin = 5, PageManager = 6,
        ChapterManagerTemplate = 7, ChapterManagerMixin = 8, ChapterManager = 9, ComicManagerTemplate = 10, ComicManagerMixin = 11, ComicManager = 12
    };
    private static ConditionalWeakTable<MComicManager, Fields> table;
    private static List<Tuple<GameObject, ComicManagerTemplate, int, List<Tuple<GameObject, ChapterManagerTemplate, int,
        List<Tuple<GameObject, PageManagerTemplate, int,
            List<Tuple<GameObject, PanelManagerTemplate, int,
                List<Tuple<GameObject, UniversalPanel, int,
                    int>>>>>>>>>> comicHierachy;
    public static ComicManagerTemplate PrimaryComic;
    static ComicManager()
    {
        comicHierachy = new List<Tuple<GameObject, ComicManagerTemplate, int, List<Tuple<GameObject, ChapterManagerTemplate, int, List<Tuple<GameObject, PageManagerTemplate, int, List<Tuple<GameObject, PanelManagerTemplate, int, List<Tuple<GameObject, UniversalPanel, int, int>>>>>>>>>>();


        table = new ConditionalWeakTable<MComicManager, Fields>();
        transitionsTable = new ConditionalWeakTable<MTransition, TransitionFields>();


    }
    private sealed class Fields : MonoBehaviour, MComicManager, MChapterManager, MPageManager, MPanelManager, MTransition, MThesisAPI, IGlobalReferenceManager
    {
        internal List<GameObject> panelObjects;
        internal List<GameObject> pageObjects;
        internal List<GameObject> chapterObjects;
        internal List<GameObject> comicObjects;

        internal List<PanelManagerMixin> PageList;
        internal List<PageManagerMixin> ChapterList;
        internal List<ChapterManagerMixin> ComicList;
        internal ComicManagerMixin mixin;

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


    }
    public sealed class TransitionFields
    {
        internal int transitionToDo = 0;
        internal int currentPanel = 1;

    }

    #region MTransitionsMixinFunctions
    //public static void UpdateTransitionTypeLine(this MComicManager map, object keywordThis)
    //{
    //    List<GameObject> panelOrder = table.GetOrCreateValue(map).GetAllOfSameType(keywordThis, false);
    //    UniversalPanel component = table.GetOrCreateValue(map).GetPanel(table.GetOrCreateValue(map).currentPanel);

    //    foreach (GameObject panel in panelOrder)
    //    {
    //        if (table.GetOrCreateValue(map).GetPanel(table.GetOrCreateValue(map).currentPanel) == panel)
    //        {
    //            transitionsTable.GetOrCreateValue(map).transitionToDo = panel.GetComponent<UniversalPanel>().GetTransitionType(panelOrder[ComicManagerMixin.mixin.currentPanel].GetComponent(typeof(comicStructureContainerClassName).GetEnumName(ClassCaller(keywordThis)))) // this should be a return type function from an interface that handles both panels and pages


    //        }
    //    }

    //}

    public static void SetPrimaryComic<T>(this MComicManager map) where T : ComicManagerTemplate
    {
        PrimaryComic = table.GetOrCreateValue(map).GetComponent<T>();
        table.GetOrCreateValue(map).mixin.GetOrAddComponent<T>();
    }


    public static int ClassCaller(object self)
    {


        for (int i = 0; typeof(comicStructureContainerClassName).GetEnumValues().Length > i; i++)
        {
            if ((string)TypeDescriptor.GetClassName(self) == typeof(comicStructureContainerClassName).GetEnumName(i))
            {
                return i;
            }
        }
        return -1; // return -1 if nothing is found throw exception if unusable.

    }

    public static bool CheckForTransition(this MComicManager map)
    {
        int nextInLineTransitionType = transitionsTable.GetOrCreateValue(map).transitionToDo;
        int currentPanel = transitionsTable.GetOrCreateValue(map).currentPanel;
        Debug.Log("TransitionType: " + nextInLineTransitionType + ",\tgameObject: " + map + ",\tPanelId: " + currentPanel, table.GetOrCreateValue(map).gameObject);
        switch (nextInLineTransitionType)
        {
            case 0:
                //if(gameObject.GetComponent<HRotateMixin>() == null)
                //{
                //Debug.Assert(GetComponent<HRotateMixin>() == null, "Universal Panel, with ID: " + gameObject.GetComponent<UniversalPanel>().PanelId.ToString() + ", has evaluated: \"GetComponent<HRotateMixin>() == null\" ",gameObject.GetComponent<UniversalPanel>());
                // goto default;
                //}
                // else
                // {
                //Debug.Log("CanTransition:\t"+GetComponent<HRotateMixin>().canTransition+",\t"+this.PanelId);
                try { return map.GetAllOfSameType(map, false)[currentPanel].GetComponent<HRotateMixin>().canTransition; }
                catch
                {
                    //Debug.LogError("Somthing bad happened", gameObject);
                    return false;
                }
            // }


            case 1:


            case 2:

            case 3:

            case 4:

            default:
                Debug.Log("Transition not found, defaulting");
                return false;
        }
    }

    /// <summary>
    /// Only looks for panel within context, cannot reference panels higher in the hierarchy, returns GameObject, use GetUniversalPanel to get the script reference.
    /// </summary>
    /// <param name="map"></param>
    /// <param name="type"></param>
    /// <param name="idInput"></param>
    /// <returns> GameObject or null reference </returns>
    public static GameObject GetPanel(this MComicManager map, object type, int idInput)
    {

        switch (ClassCaller(type))
        {
            case (int)comicStructureContainerClassName.ComicManager:
                {
                    for (int i = 0; i < comicHierachy.Count; i++)
                    {
                        if (comicHierachy[i].Item3 == idInput)
                        {
                            return comicHierachy[i].Item1;

                        }

                    }

                    goto default;

                }
            case (int)comicStructureContainerClassName.ComicManagerMixin:
                {
                    for (int i = 0; i < comicHierachy.Count; i++)
                    {
                        if (comicHierachy[i].Item3 == idInput)
                        {
                            return comicHierachy[i].Item1;

                        }

                    }

                    goto default;
                }
            case (int)comicStructureContainerClassName.ComicManagerTemplate:
                {
                    for (int i = 0; i < comicHierachy.Count; i++)
                    {
                        if (comicHierachy[i].Item3 == idInput)
                        {
                            return comicHierachy[i].Item1;

                        }

                    }

                    goto default;
                }
            case (int)comicStructureContainerClassName.ChapterManager:
                {

                    for (int i = 0; i < comicHierachy.Count; i++)
                    {
                        for (int j = 0; j < comicHierachy[i].Item4.Count; j++)
                        {
                            if (comicHierachy[i].Item4[j].Item3 == idInput)
                            {
                                return comicHierachy[i].Item4[j].Item1;

                            }

                        }

                    }
                    goto default;
                }
            case (int)comicStructureContainerClassName.ChapterManagerMixin:
                {

                    for (int i = 0; i < comicHierachy.Count; i++)
                    {
                        for (int j = 0; j < comicHierachy[i].Item4.Count; j++)
                        {
                            if (comicHierachy[i].Item4[j].Item3 == idInput)
                            {
                                return comicHierachy[i].Item4[j].Item1;

                            }

                        }

                    }
                    goto default;
                }
            case (int)comicStructureContainerClassName.ChapterManagerTemplate:
                {

                    for (int i = 0; i < comicHierachy.Count; i++)
                    {
                        for (int j = 0; j < comicHierachy[i].Item4.Count; j++)
                        {
                            if (comicHierachy[i].Item4[j].Item3 == idInput)
                            {
                                return comicHierachy[i].Item4[j].Item1;

                            }

                        }

                    }
                    goto default;
                }
            case (int)comicStructureContainerClassName.PageManager:
                {

                    for (int i = 0; i < comicHierachy.Count; i++)
                    {
                        for (int j = 0; j < comicHierachy[i].Item4.Count; j++)
                        {
                            for (int k = 0; k < comicHierachy[i].Item4[j].Item4.Count; k++)
                            {
                                if (comicHierachy[i].Item4[j].Item4[k].Item3 == idInput)
                                {
                                    return comicHierachy[i].Item4[j].Item4[k].Item1;

                                }
                            }

                        }

                    }
                    goto default;
                }
            case (int)comicStructureContainerClassName.PageManagerMixin:
                {
                    for (int i = 0; i < comicHierachy.Count; i++)
                    {
                        for (int j = 0; j < comicHierachy[i].Item4.Count; j++)
                        {
                            for (int k = 0; k < comicHierachy[i].Item4[j].Item4.Count; k++)
                            {
                                if (comicHierachy[i].Item4[j].Item4[k].Item3 == idInput)
                                {
                                    return comicHierachy[i].Item4[j].Item4[k].Item1;

                                }
                            }

                        }

                    }
                    goto default;
                }
            case (int)comicStructureContainerClassName.PageManagerTemplate:
                {

                    for (int i = 0; i < comicHierachy.Count; i++)
                    {
                        for (int j = 0; j < comicHierachy[i].Item4.Count; j++)
                        {
                            for (int k = 0; k < comicHierachy[i].Item4[j].Item4.Count; k++)
                            {
                                if (comicHierachy[i].Item4[j].Item4[k].Item3 == idInput)
                                {
                                    return comicHierachy[i].Item4[j].Item4[k].Item1;

                                }
                            }

                        }

                    }
                    goto default;
                }
            case (int)comicStructureContainerClassName.PanelManager:
                {

                    for (int i = 0; i < comicHierachy.Count; i++)
                    {
                        for (int j = 0; j < comicHierachy[i].Item4.Count; j++)
                        {
                            for (int k = 0; k < comicHierachy[i].Item4[j].Item4.Count; k++)
                            {
                                for (int v = 0; v < comicHierachy[i].Item4[j].Item4[k].Item4.Count; v++)
                                {


                                    if (comicHierachy[i].Item4[j].Item4[k].Item3 == idInput)
                                    {
                                        return comicHierachy[i].Item4[j].Item4[k].Item4[v].Item1;
                                    }
                                }
                            }

                        }

                    }
                    goto default;
                }
            case (int)comicStructureContainerClassName.PanelManagerMixin:
                {

                    for (int i = 0; i < comicHierachy.Count; i++)
                    {
                        for (int j = 0; j < comicHierachy[i].Item4.Count; j++)
                        {
                            for (int k = 0; k < comicHierachy[i].Item4[j].Item4.Count; k++)
                            {
                                for (int v = 0; v < comicHierachy[i].Item4[j].Item4[k].Item4.Count; v++)
                                {


                                    if (comicHierachy[i].Item4[j].Item4[k].Item3 == idInput)
                                    {
                                        return comicHierachy[i].Item4[j].Item4[k].Item4[v].Item1;
                                    }
                                }
                            }

                        }

                    }
                    goto default;
                }
            case (int)comicStructureContainerClassName.PanelManagerTemplate:
                {

                    for (int i = 0; i < comicHierachy.Count; i++)
                    {
                        for (int j = 0; j < comicHierachy[i].Item4.Count; j++)
                        {
                            for (int k = 0; k < comicHierachy[i].Item4[j].Item4.Count; k++)
                            {
                                for (int v = 0; v < comicHierachy[i].Item4[j].Item4[k].Item4.Count; v++)
                                {


                                    if (comicHierachy[i].Item4[j].Item4[k].Item3 == idInput)
                                    {
                                        return comicHierachy[i].Item4[j].Item4[k].Item4[v].Item1;
                                    }
                                }
                            }

                        }

                    }
                    goto default;
                }
            case (int)comicStructureContainerClassName.UniversalPanel:
                {
                    Debug.LogAssertion("Unnessecary Use of GetPanel From UniversalPanel, avoid", (UnityEngine.Component) type);
                    for (int i = 0; i < comicHierachy.Count; i++)
                    {
                        for (int j = 0; j < comicHierachy[i].Item4.Count; j++)
                        {
                            for (int k = 0; k < comicHierachy[i].Item4[j].Item4.Count; k++)
                            {
                                for (int v = 0; v < comicHierachy[i].Item4[j].Item4[k].Item4.Count; v++)
                                {
                                    for (int n = 0; n < comicHierachy[i].Item4[j].Item4[k].Item4[v].Item4.Count; n++)
                                    {
                                        foreach (System.Tuple<GameObject, UniversalPanel, int, int> tuple in comicHierachy[i].Item4[j].Item4[k].Item4[v].Item4)
                                        {
                                            if (tuple.Item2.GetInstanceID() == ((UnityEngine.Component)type).GetInstanceID())
                                            {
                                                //accumulative = (accumulative != 0) ? accumulative *= comicHierachy[i].Item4[j].Item4[k].Item4[v].Item4.Count : accumulative = comicHierachy[i].Item4[j].Item4[k].Item4[v].Item4.Count;
                                                return comicHierachy[i].Item4[j].Item4[k].Item4[v].Item1;

                                                

                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    break;
                }
            default:
                Debug.Log("Unnexpected fallthrough in switch case, called by: ", (UnityEngine.Component)type);
                return null;
        }
        return null;
    }


    






    public static List<GameObject> GetAllOfSameType(this MComicManager map, object type, bool getTotalNumberOfPanels)
    {
        List<GameObject> accumulative;
        accumulative = new List<GameObject>();
        switch (ClassCaller(type))
        {
            case (int)comicStructureContainerClassName.ComicManager:
                {
                    for (int i = 0; i < comicHierachy.Count; i++)
                    {
                        accumulative.Add(comicHierachy[i].Item1);
                    }
                    if (!getTotalNumberOfPanels)
                        return accumulative;
                    else
                        goto case (int)comicStructureContainerClassName.ChapterManager;
                }
            case (int)comicStructureContainerClassName.ComicManagerMixin:
                {
                    for (int i = 0; i < comicHierachy.Count; i++)
                    {
                        accumulative.Add(comicHierachy[i].Item1);
                    }

                    if (!getTotalNumberOfPanels)
                        return accumulative;
                    else
                        goto case (int)comicStructureContainerClassName.ChapterManager;
                }
            case (int)comicStructureContainerClassName.ComicManagerTemplate:
                {
                    for (int i = 0; i < comicHierachy.Count; i++)
                    {
                        accumulative.Add(comicHierachy[i].Item1);
                    }

                    if (!getTotalNumberOfPanels)
                        return accumulative;
                    else
                        goto case (int)comicStructureContainerClassName.ChapterManager;
                }
            case (int)comicStructureContainerClassName.ChapterManager:
                {

                    for (int i = 0; i < comicHierachy.Count; i++)
                    {
                        for (int j = 0; j < comicHierachy[i].Item4.Count; j++)
                        {
                            accumulative.Add(comicHierachy[i].Item4[j].Item1);
                            //accumulative = (accumulative != null) ? accumulative.Add(comicHierachy[i].Item4[j].Item1) : accumulative = comicHierachy[i].Item4.Count;
                        }
                        if (!getTotalNumberOfPanels)
                            return accumulative;
                        else
                            goto case (int)comicStructureContainerClassName.PageManager;
                    }
                    break;
                }
            case (int)comicStructureContainerClassName.ChapterManagerMixin:
                {

                    for (int i = 0; i < comicHierachy.Count; i++)
                    {
                        for (int j = 0; j < comicHierachy[i].Item4.Count; j++)
                        {
                            accumulative.Add(comicHierachy[i].Item4[j].Item1);
                            //accumulative = (accumulative != null) ? accumulative.Add(comicHierachy[i].Item4[j].Item1) : accumulative = comicHierachy[i].Item4.Count;
                        }
                        if (!getTotalNumberOfPanels)
                            return accumulative;
                        else
                            goto case (int)comicStructureContainerClassName.PageManager;
                    }
                    break;
                }
            case (int)comicStructureContainerClassName.ChapterManagerTemplate:
                {

                    for (int i = 0; i < comicHierachy.Count; i++)
                    {
                        for (int j = 0; j < comicHierachy[i].Item4.Count; j++)
                        {
                            accumulative.Add(comicHierachy[i].Item4[j].Item1);
                            //accumulative = (accumulative != null) ? accumulative.Add(comicHierachy[i].Item4[j].Item1) : accumulative = comicHierachy[i].Item4.Count;
                        }
                        if (!getTotalNumberOfPanels)
                            return accumulative;
                        else
                            goto case (int)comicStructureContainerClassName.PageManager;
                    }
                    break;
                }
            case (int)comicStructureContainerClassName.PageManager:
                {

                    for (int i = 0; i < comicHierachy.Count; i++)
                    {
                        for (int j = 0; j < comicHierachy[i].Item4.Count; j++)
                        {
                            accumulative.Add(comicHierachy[i].Item4[j].Item1);
                            //accumulative = (accumulative != null) ? accumulative.Add(comicHierachy[i].Item4[j].Item1) : accumulative = comicHierachy[i].Item4.Count;
                        }
                        if (!getTotalNumberOfPanels)
                            return accumulative;
                        else
                            goto case (int)comicStructureContainerClassName.PageManager;
                    }
                    break;
                }
            case (int)comicStructureContainerClassName.PageManagerMixin:
                {
                    for (int i = 0; i < comicHierachy.Count; i++)
                    {
                        for (int j = 0; j < comicHierachy[i].Item4.Count; j++)
                        {
                            for (int k = 0; k < comicHierachy[i].Item4[j].Item4.Count; k++)
                            {

                                accumulative.Add(comicHierachy[i].Item4[j].Item4[k].Item1);
                                //accumulative = (accumulative != 0) ? accumulative *= comicHierachy[i].Item4[j].Item4.Count : accumulative = comicHierachy[i].Item4[j].Item4.Count;

                                if (!getTotalNumberOfPanels)
                                    return accumulative;
                                else
                                    goto case (int)comicStructureContainerClassName.PanelManager;
                            }
                        }
                    }
                    break;
                }
            case (int)comicStructureContainerClassName.PageManagerTemplate:
                {

                    for (int i = 0; i < comicHierachy.Count; i++)
                    {
                        for (int j = 0; j < comicHierachy[i].Item4.Count; j++)
                        {
                            for (int k = 0; k < comicHierachy[i].Item4[j].Item4.Count; k++)
                            {

                                accumulative.Add(comicHierachy[i].Item4[j].Item4[k].Item1);
                                //accumulative = (accumulative != 0) ? accumulative *= comicHierachy[i].Item4[j].Item4.Count : accumulative = comicHierachy[i].Item4[j].Item4.Count;

                                if (!getTotalNumberOfPanels)
                                    return accumulative;
                                else
                                    goto case (int)comicStructureContainerClassName.PanelManager;
                            }
                        }
                    }
                    break;
                }
            case (int)comicStructureContainerClassName.PanelManager:
                {

                    for (int i = 0; i < comicHierachy.Count; i++)
                    {
                        for (int j = 0; j < comicHierachy[i].Item4.Count; j++)
                        {
                            for (int k = 0; k < comicHierachy[i].Item4[j].Item4.Count; k++)
                            {
                                for (int v = 0; v < comicHierachy[i].Item4[j].Item4[k].Item4.Count; v++)
                                {
                                    accumulative.Add(comicHierachy[i].Item4[j].Item4[k].Item4[v].Item1);
                                    //accumulative = (accumulative != 0) ? accumulative *= comicHierachy[i].Item4[j].Item4.Count : accumulative = comicHierachy[i].Item4[j].Item4.Count;

                                    if (!getTotalNumberOfPanels)
                                        return accumulative;
                                    else
                                        goto case (int)comicStructureContainerClassName.PanelManager;
                                }
                            }
                        }
                    }
                    break;
                }
            case (int)comicStructureContainerClassName.PanelManagerMixin:
                {

                    for (int i = 0; i < comicHierachy.Count; i++)
                    {
                        for (int j = 0; j < comicHierachy[i].Item4.Count; j++)
                        {
                            for (int k = 0; k < comicHierachy[i].Item4[j].Item4.Count; k++)
                            {
                                for (int v = 0; v < comicHierachy[i].Item4[j].Item4[k].Item4.Count; v++)
                                {
                                    accumulative.Add(comicHierachy[i].Item4[j].Item4[k].Item4[v].Item1);
                                    //accumulative = (accumulative != 0) ? accumulative *= comicHierachy[i].Item4[j].Item4.Count : accumulative = comicHierachy[i].Item4[j].Item4.Count;

                                    if (!getTotalNumberOfPanels)
                                        return accumulative;
                                    else
                                        goto case (int)comicStructureContainerClassName.PanelManager;
                                }
                            }
                        }
                    }
                    break;
                }
            case (int)comicStructureContainerClassName.PanelManagerTemplate:
                {

                    for (int i = 0; i < comicHierachy.Count; i++)
                    {
                        for (int j = 0; j < comicHierachy[i].Item4.Count; j++)
                        {
                            for (int k = 0; k < comicHierachy[i].Item4[j].Item4.Count; k++)
                            {
                                for (int v = 0; v < comicHierachy[i].Item4[j].Item4[k].Item4.Count; v++)
                                {
                                    accumulative.Add(comicHierachy[i].Item4[j].Item4[k].Item4[v].Item1);
                                    //accumulative = (accumulative != 0) ? accumulative *= comicHierachy[i].Item4[j].Item4.Count : accumulative = comicHierachy[i].Item4[j].Item4.Count;

                                    if (!getTotalNumberOfPanels)
                                        return accumulative;
                                    else
                                        goto case (int)comicStructureContainerClassName.PanelManager;
                                }
                            }
                        }
                    }
                    break;
                }
            case (int)comicStructureContainerClassName.UniversalPanel:
                {

                    for (int i = 0; i < comicHierachy.Count; i++)
                    {
                        for (int j = 0; j < comicHierachy[i].Item4.Count; j++)
                        {
                            for (int k = 0; k < comicHierachy[i].Item4[j].Item4.Count; k++)
                            {
                                for (int v = 0; v < comicHierachy[i].Item4[j].Item4[k].Item4.Count; v++)
                                {
                                    for (int n = 0; n < comicHierachy[i].Item4[j].Item4[k].Item4[v].Item4.Count; n++)
                                    {
                                        foreach (System.Tuple<GameObject, UniversalPanel, int, int> tuple in comicHierachy[i].Item4[j].Item4[k].Item4[v].Item4)
                                        {
                                            if (tuple.Item2.GetInstanceID() == ((UnityEngine.Component)type).GetInstanceID())
                                            {
                                                //accumulative = (accumulative != 0) ? accumulative *= comicHierachy[i].Item4[j].Item4[k].Item4[v].Item4.Count : accumulative = comicHierachy[i].Item4[j].Item4[k].Item4[v].Item4.Count;
                                                accumulative.Add(comicHierachy[i].Item4[j].Item4[k].Item4[v].Item4[n].Item1);

                                                return accumulative;

                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    break;
                }
            default:
                return null;
        }
        return null;
    }

    public static int GetNumberOfPanels(this MTransition map, object type, bool getTotalNumberOfPanels)
    {
        int accumulative = 0;
        switch (ClassCaller(type))
        {
            case (int)comicStructureContainerClassName.ComicManager:
                {
                    accumulative = comicHierachy.Count;

                    if (!getTotalNumberOfPanels)
                        return accumulative;
                    else
                        goto case (int)comicStructureContainerClassName.ChapterManager;


                }
            case (int)comicStructureContainerClassName.ComicManagerMixin:
                {
                    accumulative = comicHierachy.Count;

                    if (!getTotalNumberOfPanels)
                        return accumulative;
                    else
                        goto case (int)comicStructureContainerClassName.ChapterManager;
                }
            case (int)comicStructureContainerClassName.ComicManagerTemplate:
                {
                    accumulative = comicHierachy.Count;

                    if (!getTotalNumberOfPanels)
                        return accumulative;
                    else
                        goto case (int)comicStructureContainerClassName.ChapterManager;
                }
            case (int)comicStructureContainerClassName.ChapterManager:
                {

                    for (int i = 0; i < comicHierachy.Count; i++)
                    {
                        accumulative = (accumulative != 0) ? accumulative *= comicHierachy[i].Item4.Count : accumulative = comicHierachy[i].Item4.Count;

                        if (!getTotalNumberOfPanels)
                            return accumulative;
                        else
                            goto case (int)comicStructureContainerClassName.PageManager;
                    }
                    goto default;
                }
            case (int)comicStructureContainerClassName.ChapterManagerMixin:
                {

                    for (int i = 0; i < comicHierachy.Count; i++)
                    {
                        accumulative = (accumulative != 0) ? accumulative *= comicHierachy[i].Item4.Count : accumulative = comicHierachy[i].Item4.Count;

                        if (!getTotalNumberOfPanels)
                            return accumulative;
                        else
                            goto case (int)comicStructureContainerClassName.PageManager;
                    }
                    goto default;
                }
            case (int)comicStructureContainerClassName.ChapterManagerTemplate:
                {

                    for (int i = 0; i < comicHierachy.Count; i++)
                    {
                        accumulative = (accumulative != 0) ? accumulative *= comicHierachy[i].Item4.Count : accumulative = comicHierachy[i].Item4.Count;

                        if (!getTotalNumberOfPanels)
                            return accumulative;
                        else
                            goto case (int)comicStructureContainerClassName.PageManager;
                    }
                    goto default;
                }
            case (int)comicStructureContainerClassName.PageManager:
                {

                    for (int i = 0; i < comicHierachy.Count; i++)
                    {
                        for (int j = 0; j < comicHierachy[i].Item4.Count; j++)
                        {
                            accumulative = (accumulative != 0) ? accumulative *= comicHierachy[i].Item4[j].Item4.Count : accumulative = comicHierachy[i].Item4[j].Item4.Count;

                            if (!getTotalNumberOfPanels)
                                return accumulative;
                            else
                                goto case (int)comicStructureContainerClassName.PanelManager;
                        }
                    }
                    goto default;
                }
            case (int)comicStructureContainerClassName.PageManagerMixin:
                {
                    for (int i = 0; i < comicHierachy.Count; i++)
                    {
                        for (int j = 0; j < comicHierachy[i].Item4.Count; j++)
                        {
                            accumulative = (accumulative != 0) ? accumulative *= comicHierachy[i].Item4[j].Item4.Count : accumulative = comicHierachy[i].Item4[j].Item4.Count;

                            if (!getTotalNumberOfPanels)
                                return accumulative;
                            else
                                goto case (int)comicStructureContainerClassName.PanelManager;
                        }
                    }
                    goto default;
                }
            case (int)comicStructureContainerClassName.PageManagerTemplate:
                {

                    for (int i = 0; i < comicHierachy.Count; i++)
                    {
                        for (int j = 0; j < comicHierachy[i].Item4.Count; j++)
                        {
                            accumulative = (accumulative != 0) ? accumulative *= comicHierachy[i].Item4[j].Item4.Count : accumulative = comicHierachy[i].Item4[j].Item4.Count;

                            if (!getTotalNumberOfPanels)
                                return accumulative;
                            else
                                goto case (int)comicStructureContainerClassName.PanelManager;
                        }
                    }
                    goto default;
                }
            case (int)comicStructureContainerClassName.PanelManager:
                {

                    for (int i = 0; i < comicHierachy.Count; i++)
                    {
                        for (int j = 0; j < comicHierachy[i].Item4.Count; j++)
                        {
                            for (int k = 0; k < comicHierachy[i].Item4[j].Item4.Count; k++)
                            {
                                accumulative = (accumulative != 0) ? accumulative *= comicHierachy[i].Item4[j].Item4[k].Item4.Count : accumulative = comicHierachy[i].Item4[j].Item4[k].Item4.Count;

                                if (!getTotalNumberOfPanels)
                                    return accumulative;
                                else
                                    goto case (int)comicStructureContainerClassName.UniversalPanel;
                            }

                        }
                    }
                    goto default;
                }
            case (int)comicStructureContainerClassName.PanelManagerMixin:
                {

                    for (int i = 0; i < comicHierachy.Count; i++)
                    {
                        for (int j = 0; j < comicHierachy[i].Item4.Count; j++)
                        {
                            for (int k = 0; k < comicHierachy[i].Item4[j].Item4.Count; k++)
                            {
                                accumulative = (accumulative != 0) ? accumulative *= comicHierachy[i].Item4[j].Item4[k].Item4.Count : accumulative = comicHierachy[i].Item4[j].Item4[k].Item4.Count;

                                if (!getTotalNumberOfPanels)
                                    return accumulative;
                                else
                                    goto case (int)comicStructureContainerClassName.UniversalPanel;
                            }

                        }
                    }
                    goto default;
                }
            case (int)comicStructureContainerClassName.PanelManagerTemplate:
                {

                    for (int i = 0; i < comicHierachy.Count; i++)
                    {
                        for (int j = 0; j < comicHierachy[i].Item4.Count; j++)
                        {
                            for (int k = 0; k < comicHierachy[i].Item4[j].Item4.Count; k++)
                            {
                                accumulative = (accumulative != 0) ? accumulative *= comicHierachy[i].Item4[j].Item4[k].Item4.Count : accumulative = comicHierachy[i].Item4[j].Item4[k].Item4.Count;

                                if (!getTotalNumberOfPanels)
                                    return accumulative;
                                else
                                    goto case (int)comicStructureContainerClassName.UniversalPanel;
                            }

                        }
                    }
                    goto default;
                }
            case (int)comicStructureContainerClassName.UniversalPanel:
                {

                    for (int i = 0; i < comicHierachy.Count; i++)
                    {
                        for (int j = 0; j < comicHierachy[i].Item4.Count; j++)
                        {
                            for (int k = 0; k < comicHierachy[i].Item4[j].Item4.Count; k++)
                            {
                                for (int v = 0; v < comicHierachy[i].Item4[j].Item4[k].Item4.Count; v++)
                                {
                                    foreach (System.Tuple<GameObject, UniversalPanel, int, int> tuple in comicHierachy[i].Item4[j].Item4[k].Item4[v].Item4)
                                    {
                                        if (tuple.Item2.GetInstanceID() == ((UnityEngine.Component)type).GetInstanceID())
                                        {
                                            accumulative = (accumulative != 0) ? accumulative *= comicHierachy[i].Item4[j].Item4[k].Item4[v].Item4.Count : accumulative = comicHierachy[i].Item4[j].Item4[k].Item4[v].Item4.Count;

                                            if (!getTotalNumberOfPanels)
                                                return accumulative;
                                            else
                                                return comicHierachy[i].Item4[j].Item4[k].Item4[v].Item4.Count;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    goto default;
                }
            default:
                return 0;
        }
    }




    #endregion

    #region ComicManagerMixinFunctions
    //public static void NewChapter(this MComicManager map, GameObject gameObject)
    //{

    //    GameObject[] chapterPages = gameObject.GetComponent<PageManagerMixin>().GetPageOrder();
    //    for (int i = 0; i < chapterPages.Length; i++)
    //    {
    //        table.GetOrCreateValue(map).chapterList.SetValue(chapterPages, table.GetOrCreateValue(map).chapterList.Length);
    //    }


    //}




    public static void InitializeComicManager(this MComicManager map)
    {
        //table.GetOrCreateValue(map).GetComponentOrAdd<PageManagerMixin>();
        if (table.GetOrCreateValue(map).mixin == null)
        {
            table.GetOrCreateValue(map).mixin = table.GetOrCreateValue(map).GetComponentOrAdd<ComicManagerMixin>();
        }
    }




    //public static UniversalPanel GetPanel(this MComicManager map, int id)
    //{
    //    List<GameObject> panels = table.GetOrCreateValue(map).GetAllOfSameType(table.GetOrCreateValue(map).GetComponent<T>())

    //    return panels[id].GetComponent<UniversalPanel>();
    //}

    public static void InitializeComicStructure_comics(this MComicManager map, GameObject gameObject, ComicManagerTemplate comicManagerTemplate, int comicID,
        System.Collections.Generic.List<Tuple<GameObject, ChapterManagerTemplate, int, List<
            Tuple<GameObject, PageManagerTemplate, int, List<
                Tuple<GameObject, PanelManagerTemplate, int, List<
                    Tuple<GameObject, UniversalPanel, int, int>>>>>>>> chapterTupleList)
    {
        Tuple<GameObject, ComicManagerTemplate, int, List<Tuple<GameObject, ChapterManagerTemplate, int, List<Tuple<GameObject, PageManagerTemplate, int, List<Tuple<GameObject, PanelManagerTemplate, int, List<Tuple<GameObject, UniversalPanel, int, int>>>>>>>>> comicTuple;

        comicTuple = new Tuple<GameObject, ComicManagerTemplate, int, List<Tuple<GameObject, ChapterManagerTemplate, int, List<Tuple<GameObject, PageManagerTemplate, int, List<Tuple<GameObject, PanelManagerTemplate, int, List<Tuple<GameObject, UniversalPanel, int, int>>>>>>>>>(gameObject, comicManagerTemplate, comicID, chapterTupleList);
        comicHierachy.Add(comicTuple);
    }


    public static ComicManagerMixin GetPrimaryMixin(this MComicManager map)
    {

        return table.GetOrCreateValue(map).mixin;
    }



    #endregion

    #region ComicManagerGlobalfunctions_Get_Set_Other
    public static
    List<Tuple<GameObject, ComicManagerTemplate,int,
    List<Tuple<GameObject, ChapterManagerTemplate, int,
      List<Tuple<GameObject, PageManagerTemplate, int,
         List<Tuple<GameObject, PanelManagerTemplate, int,
            List<Tuple<GameObject, UniversalPanel, int, int
                >>>>>>>>>>
    getComicsList()
    {

        return comicHierachy;
    }


    #endregion




}