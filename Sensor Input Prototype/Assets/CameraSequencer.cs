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

public class CameraSequencer : MonoBehaviour, MTransition
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
        //GlobalReferenceManager.SetActiveComic(comicManager);

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
        //GlobalReferenceManager.SetActiveComic(comicManager);
        //comicManager = (GlobalReferenceManager.GetActiveComicTemplate() as ComicManagerTemplate);
        foreach( System.Tuple<int, Component> tuples in GlobalReferenceManager.MixinPairs)
        {
            //Debug.Log(tuples.Item1, tuples.Item2);

        }
        //GlobalReferenceManager.AddNewMixin<ComicManagerMixin>(this.comicManager,comicManager.gameObject);

        currentComicManagerMixin = (GlobalReferenceManager.MixinPairs.Find(x => x.Item1 == comicManager.GetComponent<ComicManagerMixin>().GetInstanceID()).Item2 as ComicManagerMixin);
        //currentComicManagerMixin = ComicManager.PrimaryComic.GetPrimaryMixin();
        //currentPanelManagerMixin = (GlobalReferenceManager.MixinPairs.Find(x => x.Item1 == .GetInstanceID()).Item2 as ComicManagerMixin);
        panelFocus = currentComicManagerMixin.currentPanel;
        panelTransition = this.CanTransition();



        if(panelTransition)
        {
            
            panelFocus = comicManager.nextPanel;
            Debug.Log("PanelTransitioned: " + GetPanelFocus());
            gameObject.transform.position = new Vector3(
                (currentComicManagerMixin.GetPanel(
                GlobalReferenceManager.GetActivePanelTemplate(),panelFocus).GetComponent<PanelManagerMixin>()).NextPanelAnchor("x"),
                (currentComicManagerMixin.GetPanel(
                GlobalReferenceManager.GetActivePanelTemplate(), panelFocus).GetComponent<PanelManagerMixin>()).NextPanelAnchor("y"),
                    gameObject.transform.position.z);
            Transition.Next(currentComicManagerMixin);


            //panelTransition = false; //should be a callback instead
            //panelFocus = ;
            //comicManager.GetPrimaryMixin().nextPanel = comicManager.GetPrimaryMixin().nextPanel + 1;
            //if(comicManager.GetPrimaryMixin().panelOrder.Length -1 == comicManager.GetPrimaryMixin().currentPanel)
            //{
                //comicManager.GetPrimaryMixin().nextPanel = 1;
            //}
        }
        else
        {

        }
    }

}
