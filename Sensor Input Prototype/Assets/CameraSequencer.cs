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
    private int panelFocus = 1;
    #if UNITY_EDITOR 
    [ShowOnly] 
    #endif
    [SerializeField]
    private bool panelTransition = false;
    private static ComicManagerMixin currentComicManagerMixin;
    private static ComicManagerTemplate comicManager;
    private void Awake()
    {
        comicManager = ComicManager.PrimaryComic;
    }

    public int GetPanelFocus()
    {
        return panelFocus;
    }
    private void Update()
    {
        
        comicManager = (GlobalReferenceManager.GetActiveComicTemplate() as ComicManagerTemplate);
        currentComicManagerMixin = (GlobalReferenceManager.MixinPairs.Find(x => x.Item1 == comicManager.GetInstanceID()).Item2 as ComicManagerMixin);
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
