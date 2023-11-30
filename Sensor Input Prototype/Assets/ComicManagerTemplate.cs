using SensorInputPrototype.MixinInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ComicManagerTemplate : PanelSystemTemplateType, MTransition, MComicManager, IGlobalReferenceManager
{
    [SerializeField] public int containerId;
    public List<GameObject> comicsList;
    public override int panelId
    {
        get { return panelId; }
        set { panelId = containerId; }
    }

    public int currentPanel = 1;
    public int nextPanel = 2;
    public int previousPanel = 1;
   
    private void Awake()
    {
        //panelId = containerId;
        GlobalReferenceManager.AddNewMixin<ComicManagerMixin>(this,gameObject);
        // this.InitializeComicManager();

        // DO NOT CALL Templates SetPanelID() -- need a fix or this.
        // SetUp All Properties Shared across hierachy
        //this.SetPrimaryComic<ComicManagerTemplate>(); // I think this should be discontinued now that i have a GloablReferenceSingleton
        //GlobalReferenceManager.SetActiveComicContainer(this);
        

    }
    void Update()
    {

        previousPanel = GetComponent<ComicManagerMixin>().nextPanel;
        currentPanel = GetComponent<ComicManagerMixin>().currentPanel;
        nextPanel = GetComponent<ComicManagerMixin>().nextPanel;
    }

}

