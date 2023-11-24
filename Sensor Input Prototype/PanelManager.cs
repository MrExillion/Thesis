using SensorInputPrototype.InspectorReadOnlyCode;
using SensorInputPrototype.MixinInterfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
//using Unity.VisualScripting;
using UnityEngine;

public static class PanelManager
{
#if UNITY_EDITOR
    [ShowOnly]
#endif
    [SerializeField]
    private static ConditionalWeakTable<MPanelManager, Fields> table;
    
    static PanelManager()
    {

        table = new ConditionalWeakTable<MPanelManager, Fields>();
        

    }
    private sealed class Fields : ComicManagerTemplate, MTransition
    {
        internal GameObject[] panelObjects = { };
        
    }


    public static void InitializePanelManger(this MPanelManager map)
    {

        table.GetOrCreateValue(map).GetOrAddComponent<PanelManagerMixin>();
        if (PanelManagerMixin.template == null)
        {
            PanelManagerMixin.template = table.GetOrCreateValue(map).GetOrAddComponent<PanelManagerTemplate>();
        }
        else
        { return; }
    


    }
    public static int GetTransitionType(this MPanelManager map, int panelId)
    {

        return table.GetOrCreateValue(map).GetPanel(panelId).GetTransitionType();
    }




}
