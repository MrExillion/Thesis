using SensorInputPrototype.MixinInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ComicManagerTemplate : PanelSystemTemplateType, MTransition, MComicManager
{
    [SerializeField] public int containerId;
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

        this.InitializeComicManager();

        // DO NOT CALL Templates SetPanelID() -- need a fix or this.
        // SetUp All Properties Shared across hierachy
        this.SetPrimaryComic<ComicManagerTemplate>();

    }
}

