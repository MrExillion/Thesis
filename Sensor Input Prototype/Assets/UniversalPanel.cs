using SensorInputPrototype.MixinInterfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class UniversalPanel : MonoBehaviour, MTransition, MPanelManager, IGlobalReferenceManager
{
    
    public int PanelId = 0; // I am considering the UI system to be able to simply dictate the panel ID by order of execution as the list in the PageManager, but for now its not like that. 
    public int transitionType = 0; // I want this to maybe be some fancier solution in the future, either by automatically finding the transition through appliance, or by using Enums to be more inspector reader friendly
    private void Awake()
    {
        this.AddNewMixin<UniversalPanel>(this.gameObject);
        //GlobalReferenceManager.MixinPairs.Add(new Tuple<int, UnityEngine.Component>(this.GetInstanceID(), this.GetComponentOrAdd<UniversalPanel>()));
        this.TrackPanel(gameObject, this, PanelId, transitionType);
    }
    public int GetTransitionType()
    {
        return transitionType;

        //Debug.Log("TransitionType: " + this.transitionType + ",\tgameObject: " + this.gameObject+ ",\tPanelId: " + this.PanelId, this);
        //switch (transitionType)
        //{
        //    case 0:
        //        //if(gameObject.GetComponent<HRotateMixin>() == null)
        //        //{
        //            //Debug.Assert(GetComponent<HRotateMixin>() == null, "Universal Panel, with ID: " + gameObject.GetComponent<UniversalPanel>().PanelId.ToString() + ", has evaluated: \"GetComponent<HRotateMixin>() == null\" ",gameObject.GetComponent<UniversalPanel>());
        //           // goto default;
        //        //}
        //       // else
        //       // {
        //            //Debug.Log("CanTransition:\t"+GetComponent<HRotateMixin>().canTransition+",\t"+this.PanelId);
                    
        //       // }
                
                
        //    case 1:
                
                
        //    case 2:

        //    case 3:

        //    case 4:

        //    default:
        //        Debug.Log("Transition not found, defaulting");
        //        return false;
        //}

            

        
    }

}
