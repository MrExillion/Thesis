//using System.Collections;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Runtime.InteropServices;
//using System.Xml.Serialization;
using Unity.VisualScripting;
//using UnityEditor.UIElements;
using UnityEngine;
using SensorInputPrototype.InspectorReadOnlyCode;
public class CameraSequencer : MonoBehaviour
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


    public int GetPanelFocus()
    {
        return panelFocus;
    }
    private void Update()
    {
        panelFocus = PageReadOrder.panelManager.currentPanel;
        panelTransition = PageReadOrder.panelManager.CanTransition();


        if(panelTransition)
        {
            panelFocus = PageReadOrder.panelManager.nextPanel;
            Debug.Log("PanelTransitioned: " + GetPanelFocus());
            gameObject.transform.position = new Vector3(PageReadOrder.panelManager.NextPanelAnchor("x"), PageReadOrder.panelManager.NextPanelAnchor("y"), gameObject.transform.position.z);
            panelTransition = false; //should be a callback instead
            PageReadOrder.panelManager.currentPanel = PageReadOrder.panelManager.nextPanel;
            PageReadOrder.panelManager.nextPanel = PageReadOrder.panelManager.nextPanel + 1;
            if(PageReadOrder.panelManager.panelOrder.Length -1 == PageReadOrder.panelManager.currentPanel)
            {
                PageReadOrder.panelManager.nextPanel = 1;
            }
        }
        else
        {

        }
    }

}
