using SensorInputPrototype.MixinInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class PageManagerTemplate : PanelSystemTemplateType, MPageManager, IGlobalReferenceManager
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
        //panelId = chapterId;
        this.AddNewMixin<PanelManagerMixin>(gameObject); // this is the new was of dealing with the thing i saw below.
        //GlobalReferenceManager.MixinPairs.Add(new Tuple<int, UnityEngine.Component>(this.GetInstanceID(), this.GetComponentOrAdd<PanelManagerMixin>()));
        //this.InitializePageManager(); Unneeded as Component Addition Is being done in the above line 
        this.InitializeComicStructure_pages(gameObject, this, chapterId, this.TrackPages(pageOrder));
    }
    



}

