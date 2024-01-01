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
using System;
using Unity.Mathematics;

public class CameraSequencer : MonoBehaviour, MTransition, MHRotate, MClassicComicMixin
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
    private Quaternion cameraDefaultRotation;
    private Quaternion initialRotation;
    public bool classicMode = false;
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
        cameraDefaultRotation = Camera.main.transform.rotation;
        Quaternion quaternion;
        quaternion = GyroUpdateRoutine();
        initialRotation = quaternion;

    }
    private void Start()
    {
        currentComicManagerMixin = ComicManagerMixin.mixin;
        if (ComicManager.PrimaryComic == null)
        {
            //ComicManager.PrimaryComic = ComicManager.getComicsList()[0].Item2;
            ComicManager.PrimaryComic = comicManager;
        }
        if (Camera.main.scene.name == "ClassicComicBook")
        {
            this.ClassicMixin_Initialized(gameObject);
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
        {
            currentComicManagerMixin = (GlobalReferenceManager.MixinPairs.Find(x => x.Item1 == comicManager.GetComponent<ComicManagerMixin>().GetInstanceID()).Item2 as ComicManagerMixin);
        }
        if (previousTransitionType == (int)GlobalReferenceManager.GetCurrentUniversalPanel().transitionType)
        {
            blockChainReaction = true;
            previousTransitionType = -1;
        }

        //currentComicManagerMixin = ComicManager.PrimaryComic.GetPrimaryMixin();
        //currentPanelManagerMixin = (GlobalReferenceManager.MixinPairs.Find(x => x.Item1 == .GetInstanceID()).Item2 as ComicManagerMixin);
        panelFocus = currentComicManagerMixin.currentPanel;



        if (!blockChainReaction)
        {
            panelTransition = GlobalReferenceManager.GetCurrentUniversalPanel().CanTransition();
        }
        else
        {
            GlobalReferenceManager.GetCurrentUniversalPanel().ResetCanTransition();
        }
        if (classicMode)
        {

            this.ClassicMixin_Update();

        }
        else
        {


            if (panelTransition) { Debug.Log("Can Transition?   " + panelTransition + " ,  panelFocus = " + panelFocus + " , BlockChainReaction = " + blockChainReaction); }



            if (panelTransition && !blockChainReaction)
            {
                GlobalReferenceManager.GetCurrentUniversalPanel().ResetCanTransition();
                previousTransitionType = (int)GlobalReferenceManager.GetCurrentUniversalPanel().transitionType;
                Debug.Log("PanelTransitioned from: " + GetPanelFocus() + " to: " + comicManager.nextPanel);
                panelFocus = comicManager.nextPanel;
                Debug.Log("PanelTransitioned: " + GetPanelFocus());
                gameObject.transform.position = new Vector3(
                    (GlobalReferenceManager.GetActivePanelTemplate() as PanelManagerTemplate).panelOrder[panelFocus].GetComponentInParent<PanelManagerMixin>().NextPanelAnchor("x"),
                    (GlobalReferenceManager.GetActivePanelTemplate() as PanelManagerTemplate).panelOrder[panelFocus].GetComponentInParent<PanelManagerMixin>().NextPanelAnchor("y"),
                    gameObject.transform.position.z);
                //this.UpdateRotation(initialRotation.z, 0f);
                Camera.main.transform.rotation = cameraDefaultRotation;


                //if ((int)GlobalReferenceManager.GetCurrentUniversalPanel().transitionType == 0)
                //{
                //    Quaternion quaternion;
                //    quaternion = GyroUpdateRoutine();
                //    initialRotation = quaternion;
                //}






                //Transition.Next(currentComicManagerMixin);



                //panelTransition = false; //should be a callback instead
                //panelFocus = ;
                //comicManager.GetPrimaryMixin().nextPanel = comicManager.GetPrimaryMixin().nextPanel + 1;
                //if(comicManager.GetPrimaryMixin().panelOrder.Length -1 == comicManager.GetPrimaryMixin().currentPanel)
                //{
                //comicManager.GetPrimaryMixin().nextPanel = 1;
                //}
            }
            else if (blockChainReaction && (int)GlobalReferenceManager.GetCurrentUniversalPanel().transitionType == 0)
            {
                Quaternion quaternion = GyroUpdateRoutine();

                this.UpdateRotation(quaternion.z - initialRotation.z, quaternion.eulerAngles.z - initialRotation.eulerAngles.z);
                if (this.IsResetConditionMet())
                {
                    blockChainReaction = false;
                    this.ResetCanTransition();
                    panelTransition = false;
                }


            }
        }



    }
    private void LateUpdate()
    {
        //if(panelTransition && !blockChainReaction)
        //{

        //    Camera.main.transform.rotation = cameraDefaultRotation;
        //    panelTransition = false;
        //}

    }
    private Quaternion GyroUpdateRoutine()
    {
        Quaternion quaternion;

        int compassAdjust = -1;
        int heading = (int)MathF.Truncate(Input.compass.magneticHeading);
        if (MathF.Abs(heading) >= 360 && heading != 0)
        {
            heading = heading - ((heading % 360) * 360); // I can't find indications on weather or not this is needed, but this compensates the heading to always be between -360 and 360, while never dividing by 0.
        }


        if (heading < 0)
        {
            if (heading > -90)
            {
                compassAdjust = 1;
            }
            else if (heading > -270)
            {
                compassAdjust = -1;
            }
            else
            {
                compassAdjust = 1;
            }

        }
        else
        {


            if (heading < 90)
            {
                compassAdjust = -1;
            }
            else if (heading < 270)
            {
                compassAdjust = 1;
            }
            else
            {
                compassAdjust = -1;
            }

        }




        quaternion = new Quaternion(Input.gyro.attitude.x, Input.gyro.attitude.y, Input.gyro.attitude.z, compassAdjust * Input.gyro.attitude.w);

        return quaternion;
    }



}


