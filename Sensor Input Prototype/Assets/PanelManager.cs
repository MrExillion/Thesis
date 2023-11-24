//using System.Text;
//using System.Threading.Tasks;
//using Unity.VisualScripting;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using SensorInputPrototype.InspectorReadOnlyCode;
using SensorInputPrototype.MixinInterfaces;
using System.Collections.Generic;
using System;
using Unity.Properties;
using UnityEditor;
using System.Reflection;

public static class PanelManager
{
#if UNITY_EDITOR
    [ShowOnly]
#endif
    [SerializeField]
    private static ConditionalWeakTable<MPanelManager, Fields> table;
    private static List<Tuple<GameObject, PanelManagerTemplate, int, List<Tuple<GameObject, UniversalPanel, int, int>>>> panels;
    private static List<Tuple<GameObject, UniversalPanel, int, int>> tempPanels;
    public delegate void TempListReadyForInit(MPanelManager map, GameObject gameObject, PanelManagerTemplate panelManagerTemplate, int panelID,
    System.Collections.Generic.List<Tuple<GameObject, UniversalPanel, int, int>> panelsTupleList);
    
    static PanelManager()
    {

        table = new ConditionalWeakTable<MPanelManager, Fields>();
        

    }
    private sealed class Fields : ComicManagerTemplate, MTransition
    {
        internal GameObject[] panelObjects = { };
        internal ComicManagerMixin mixin;
        internal TempListReadyForInit TempListReadyDelegate;
    }

    [Obsolete("Handled via GetComponentOrAdd")]
    public static void InitializePanelManger(this MPanelManager map)
    {

        table.GetOrCreateValue(map).GetComponentOrAdd<PanelManagerMixin>();
        //if (PanelManagerTemplate.mixin == null)
        //{
        //    PanelManagerTemplate.mixin = table.GetOrCreateValue(map).GetComponentOrAdd<PanelManagerMixin>();
        //}
        //else
        //{ return; }
    


    }
    public static int GetTransitionType(this MPanelManager map, int panelId)
    {
        //(GlobalReferenceManager.MixinPairs.Find(x => x.Item1 == (GlobalReferenceManager.GetActiveComicTemplate()).GetInstanceID()).Item2 as ComicManagerMixin)
        //if((panels.Find(x=> x.Item4.Find(z=> z.Item1) == panels.Find(x=> x.Item3[panelId]))
       //{

        //}

            
            
            return panels.Find(x=> x.Item2 == (map as Component)).Item4.Find(x=> x.Item3 == panelId).Item4;
            //GetPanelTransition(map, panelId);
    }


    public static void InitializeComicStructure_panels(this MPanelManager map, GameObject gameObject, PanelManagerTemplate panelManagerTemplate, int panelID, 
    System.Collections.Generic.List<Tuple<GameObject, UniversalPanel, int, int>> panelsTupleList)
    {
        Tuple<GameObject, PanelManagerTemplate, int, List<Tuple<GameObject, UniversalPanel, int, int>>> panelsTuple;

        panelsTuple = new Tuple<GameObject, PanelManagerTemplate, int, List<Tuple<GameObject, UniversalPanel, int, int>>>(gameObject, panelManagerTemplate, panelID, panelsTupleList);
        panels.Add(panelsTuple);
        table.GetOrCreateValue(map).TempListReadyDelegate = null;

    }
    public static void TrackPanel(this MPanelManager map, GameObject gameObject, UniversalPanel universalPanel, int panelId, int transitionTypeIndex)
    {
        
        
        Tuple<GameObject, UniversalPanel, int, int>  tupleOut;
        tupleOut = new Tuple<GameObject, UniversalPanel, int, int>(gameObject, universalPanel, panelId,transitionTypeIndex);
        tempPanels.Add(tupleOut);
        if (universalPanel.GetComponentInParent<PanelManagerTemplate>().panelOrder.Count == tempPanels.Count)
        {
            SortListToReflectPanelOrder(map);
            table.GetOrCreateValue(map).TempListReadyDelegate = InitializeComicStructure_panels;
        }



    }
    
    private static void SortListToReflectPanelOrder(this MPanelManager map)
    {
        List<Tuple<GameObject, UniversalPanel, int, int>> tupleListOut;
        tupleListOut = new List<Tuple<GameObject, UniversalPanel, int, int>>();
        foreach (Tuple<GameObject, UniversalPanel, int, int> tuple in tempPanels)
        {
            //I am not sure if Tuples only hold pointers or values when replaced, use the commented code if problems occur and null reference exceptions cause doubt
            /*
            Tuple<GameObject, UniversalPanel, int, int> tempTuple1 = new Tuple<GameObject, UniversalPanel, int, int>(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4);
            tupleListOut.Insert(tuple.Item3, tempTuple1);
            */
            tupleListOut.Insert(tuple.Item3, tuple);
        }
        // Clear the tempPanels to avoid unused memory reservations. The GC behaviour in this regard is unknown.
        tempPanels.Clear();
        // set the sorted List of Tuples as the tempPanels, Fire Event for Hierachy building.
            tempPanels = tupleListOut;
    }

    public static void InvokeTempListReadyDelegate(this MPanelManager map, GameObject gameObject, PanelManagerTemplate panelManagerTemplate, int pageID)
    {
        
        while (table.GetOrCreateValue(map).TempListReadyDelegate != null)
        {
            table.GetOrCreateValue(map).TempListReadyDelegate.Invoke(map,gameObject,panelManagerTemplate,pageID, tempPanels);
            
        }
        
    }

}
