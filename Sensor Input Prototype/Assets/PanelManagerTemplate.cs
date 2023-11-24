using SensorInputPrototype.MixinInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.Runtime.CompilerServices;
using UnityEngine.InputSystem;
using System.Collections;

public class PanelManagerTemplate : PanelSystemTemplateType, MThesisAPI
{
    [SerializeField] public int pageId;


    public override int panelId
    {
        get { return panelId; }
        set { panelId = pageId; }
    }
    public List<GameObject> panelOrder;
    public bool canTransition = false;
    // private static List<Tuple<int,PanelManagerMixin>> mixin;



    private void Awake()
    {


        GlobalReferenceManager.MixinPairs.Add(new Tuple<int, UnityEngine.Component>(this.GetInstanceID(), this.GetComponentOrAdd<PanelManagerMixin>()));

    }

    private void Start()
    {


        (GlobalReferenceManager.MixinPairs.Find(x => x.Item1 == this.GetInstanceID()).Item2 as PanelManagerMixin).InvokeTempListReadyDelegate(gameObject, this, panelId);
    }


}

