using SensorInputPrototype.MixinInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class PageManagerTemplate : PanelSystemTemplateType, MPageManager
{
    //public GameObject[] pageOrder;
    [SerializeField] public int chapterId;
    public List<GameObject> pageOrder;

    public override int panelId
    {
        get { return panelId; }
        set { panelId = chapterId; }
    }

    private void Awake()
    {
        this.InitializePageManager();
    }
    



}

