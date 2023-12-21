//using System.Collections;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Runtime.InteropServices;
//using System.Xml.Serialization;
using Unity.VisualScripting;
//using UnityEditor.UIElements;
using UnityEngine;
using SensorInputPrototype.InspectorReadOnlyCode;
using SensorInputPrototype.MixinInterfaces;

public class CameraSequencer : MonoBehaviour, MTransition, MHRotate
{
    #if UNITY_EDITOR 
    [ShowOnly] 
    #endif
    [SerializeField]
    public int panelFocus = 1;
    #if UNITY_EDITOR 
    [ShowOnly] 
    #endif
    [SerializeField]
    private bool panelTransition = false;
    [SerializeField] public ComicManagerMixin currentComicManagerMixin;
    [SerializeField] public ComicManagerTemplate comicManager;
    private bool blockChainReaction = false;
    private int previousTransitionType = -1;

    private void Awake()
    {
        
    }

    public int GetPanelFocus()
    {
        return panelFocus;
    }

    private void OnEnable()
    {
        //currentComicManagerMixin = ComicManagerMixin.mixin;
        //ComicManager.PrimaryComic = ComicManagerMixin.mixin;
        //GlobalReferenceManager.SetActiveComicContainer(comicManager);

    }
    private void Start()
    {
        currentComicManagerMixin = ComicManagerMixin.mixin;
        if(ComicManager.PrimaryComic == null)
        {
            //ComicManager.PrimaryComic = ComicManager.getComicsList()[0].Item2;
            ComicManager.PrimaryComic = comicManager;
        }
        
        
    }

    private void Update()
    {
        //GlobalReferenceManager.SetActiveComicContainer(comicManager);
        //comicManager = (GlobalReferenceManager.GetActiveComicTemplate() as ComicManagerTemplate);
        //foreach( System.Tuple<int, Component> tuples in GlobalReferenceManager.MixinPairs)
        //{
        //    //Debug.Log(tuples.Item1, tuples.Item2);

        //}
        //GlobalReferenceManager.AddNewMixin<ComicManagerMixin>(this.comicManager,comicManager.gameObject);




        if (currentComicManagerMixin == null)
        { currentComicManagerMixin = (GlobalReferenceManager.MixinPairs.Find(x => x.Item1 == comicManager.GetComponent<ComicManagerMixin>().GetInstanceID()).Item2 as ComicManagerMixin);
        }
        if (previousTransitionType == (int)GlobalReferenceManager.GetCurrentUniversalPanel().transitionType)
        {
            blockChainReaction = true;
        }

        //currentComicManagerMixin = ComicManager.PrimaryComic.GetPrimaryMixin();
        //currentPanelManagerMixin = (GlobalReferenceManager.MixinPairs.Find(x => x.Item1 == .GetInstanceID()).Item2 as ComicManagerMixin);
        panelFocus = currentComicManagerMixin.currentPanel;
        panelTransition = GlobalReferenceManager.GetCurrentUniversalPanel().CanTransition();
        Debug.Log("Can Transition?   " + panelTransition);

        if(panelTransition && !blockChainReaction)
        {
            GlobalReferenceManager.GetCurrentUniversalPanel().ResetCanTransition();
            Debug.Log("PanelTransitioned from: " + GetPanelFocus() +" to: "+comicManager.nextPanel);
            panelFocus = comicManager.nextPanel;
            Debug.Log("PanelTransitioned: " + GetPanelFocus());
            gameObject.transform.position = new Vector3(
                (GlobalReferenceManager.GetActivePanelTemplate() as PanelManagerTemplate).panelOrder[panelFocus].GetComponentInParent<PanelManagerMixin>().NextPanelAnchor("x"),
                (GlobalReferenceManager.GetActivePanelTemplate() as PanelManagerTemplate).panelOrder[panelFocus].GetComponentInParent<PanelManagerMixin>().NextPanelAnchor("y"),
                gameObject.transform.position.z);
            panelTransition = false;
            previousTransitionType = GlobalReferenceManager.GetCurrentUniversalPanel().transitionType;

            //Transition.Next(currentComicManagerMixin);



            //panelTransition = false; //should be a callback instead
            //panelFocus = ;
            //comicManager.GetPrimaryMixin().nextPanel = comicManager.GetPrimaryMixin().nextPanel + 1;
            //if(comicManager.GetPrimaryMixin().panelOrder.Length -1 == comicManager.GetPrimaryMixin().currentPanel)
            //{
            //comicManager.GetPrimaryMixin().nextPanel = 1;
            //}
        }
        else if(blockChainReaction && UniversalPanel.ResetConditions(GlobalReferenceManager.GetCurrentUniversalPanel()))
        {
            blockChainReaction = false;
        }
    }

}
