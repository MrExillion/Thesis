using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SensorInputPrototype.InspectorReadOnlyCode;

using static System.TimeZoneInfo;
using System.Net.Security;
using SensorInputPrototype.MixinInterfaces;
public class PanelManagerMixin : MonoBehaviour, MPanelManager, IGlobalReferenceManager
{


    //public GameObject[] panelOrder;
    //public int currentPanel = 1;
    //public int nextPanel = 2;
    //public static PanelManagerMixin panelManager;
    //public static PanelManagerTemplate mixin;
    public bool canTransition = false;
    //private static int transitionToDo = 0;

    //private void Awake()
    //{
    //    if (panelManager == null)
    //        panelManager = this;
    //    else
    //        return;
    //}
    //public static ComicManagerTemplate mixin;

    /*
     PanelManagerTemplate Has the following Fields:
        TemplateId (int) inherited from Template.cs > PanelSystemTemplateType.cs accessible as protected in PanelMangerTemplate
            
    public abstract class PanelSystemTemplateType : Templates
    {
        protected int panelId;
            constructor => panelId = base.templateId. This is an incorrect use of templateId.
    }
     
     
     
     */
    

    private void Start()
    {

        



    }

    public void Update()
    {

        //transitionToDo = panelOrder[currentPanel].GetComponent<UniversalPanel>().GetTransitionType(); // this should be a return type function from an interface that handles both panels and pages



         //CheckForTransition();


    }
    //public bool CheckForTransition()
    //{
    //    Debug.Log("TransitionType: " + transitionToDo + ",\tgameObject: " + this.gameObject + ",\tPanelId: " + currentPanel, this);
    //    switch (transitionToDo)
    //    {
    //        case 0:
    //            //if(gameObject.GetComponent<HRotateMixin>() == null)
    //            //{
    //            //Debug.Assert(GetComponent<HRotateMixin>() == null, "Universal Panel, with ID: " + gameObject.GetComponent<UniversalPanel>().PanelId.ToString() + ", has evaluated: \"GetComponent<HRotateMixin>() == null\" ",gameObject.GetComponent<UniversalPanel>());
    //            // goto default;
    //            //}
    //            // else
    //            // {
    //            //Debug.Log("CanTransition:\t"+GetComponent<HRotateMixin>().canTransition+",\t"+this.PanelId);
    //            try { return panelOrder[currentPanel].GetComponent<HRotateMixin>().canTransition; }
    //            catch
    //            {
    //                Debug.LogError("Somthing bad happened", gameObject);
    //                return false;
    //            }
    //        // }


    //        case 1:


    //        case 2:

    //        case 3:

    //        case 4:

    //        default:
    //            Debug.Log("Transition not found, defaulting");
    //            return false;
    //    }
    //}


    //public bool CanTransition()
    //{

    //    return canTransition;
    //}
    public float NextPanelAnchor(string axisLetter)
    {

        if (Camera.main.GetComponent<CameraSequencer>().GetPanelFocus() + 1 < this.GetNumberOfPanels(this, false))
        {


            if (axisLetter == "y")
            {
                return GetComponent<PanelManagerTemplate>().panelOrder[Camera.main.GetComponent<CameraSequencer>().GetPanelFocus() + 1].transform.position.y;
            }
            else
            {
                Debug.Log(Camera.main.GetComponent<CameraSequencer>().GetPanelFocus() + 1);
                return GetComponent<PanelManagerTemplate>().panelOrder[Camera.main.GetComponent<CameraSequencer>().GetPanelFocus() + 1].transform.position.x;
            }
            //should throw exception here if bad.
        }
        else // next page instead, currently just loops. Should it loop if no more pages? => In the beginning yes.
        {
            if (axisLetter == "y")
            {
                return GetComponent<PanelManagerTemplate>().panelOrder[0].transform.position.y;
            }
            else
            {
                Debug.Log(0);
                return GetComponent<PanelManagerTemplate>().panelOrder[0].transform.position.x;
            }

        }
    }



}
