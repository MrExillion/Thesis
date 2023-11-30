using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Mathematics;
using SensorInputPrototype.InspectorReadOnlyCode;
using SensorInputPrototype.MixinInterfaces;
using Unity.VisualScripting;

using System.Runtime.InteropServices.WindowsRuntime;

public class HRotateMixin : MonoBehaviour, MHRotate, MTransition
{
    #if UNITY_EDITOR 
    [ShowOnly]
    #endif
    [SerializeField]
    private Camera camera;
    #if UNITY_EDITOR
    [ShowOnly]
    #endif
    [SerializeField]
    private int compassAdjust = -1;
    #if UNITY_EDITOR
    [ShowOnly]
    #endif
    [SerializeField]
    private Quaternion quaternion;
    private Quaternion initialRotation;
    public bool canTransition = false;
    private int previousPanel = 0;
    //[SerializeField] public GameObject t;
    #if UNITY_EDITOR
    [ShowOnly] 
    #endif
    [SerializeField]
    private static HRotateTemplate template;
    private bool isRunningDebug;
    private bool triggered = true;
    private static ComicManagerMixin comicManagerMixin;
    private void Awake()
    {
        Input.gyro.enabled = true;
        Input.compass.enabled = true;

        if (comicManagerMixin == null)
        { HRotateMixin.comicManagerMixin = Camera.main.GetComponent<CameraSequencer>().currentComicManagerMixin; }
        //if (gameObject.GetComponent<HRotateTemplate>() == t.GetComponent<HRotateTemplate>())
        if (template == null)
            template = gameObject.GetComponent<HRotateTemplate>();
        else
            return;
        //#if (PLATFORM_ANDROID == true && UNITY_EDITOR == false)

        //InputSystem.AddDevice<AndroidLightSensor>("AndroidLightSensor");
        //InputSystem.EnableDevice(InputSystem.GetDevice("AndroidLightSensor"));

        //#endif
    }

    // Start is called before the first frame update
    private void Start()
    {
        

        //camera.transform.localRotation

        int heading = (int)MathF.Truncate(Input.compass.magneticHeading);
        if (MathF.Abs(heading) >= 360 && heading != 0)
        {
            heading = heading - ((heading % 360)*360); // I can't find indications on weather or not this is needed, but this compensates the heading to always be between -360 and 360, while never dividing by 0.
        }


        if (heading < 0)
        {
            if (heading > -90)
            {
                compassAdjust = 1;
            }
            else if( heading > -270)
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


        initialRotation = quaternion;

        //Console.WriteLine("Name {0}, Age = {1}", h.Name, h.GetAge());
        //Console.ReadKey();
        //if (lightsensorref.enabled)
        //    Debug.Log("light sensor is enabled");
        //InputSystem.EnableDevice(Gyroscope.current);
        //InputSystem.EnableDevice(AttitudeSensor.current);
        //if (Gyroscope.current.enabled)
        //{
        //    Debug.Log("Gyroscope is enabled");
        //    //    Get sampling frequency of gyro.
        //    frequency = Gyroscope.current.samplingFrequency;

        //    //    Set sampling frequency of gyro to sample 16 times per second.
        //    Gyroscope.current.samplingFrequency = 16;
        //}
        //else
        //{
        //    return;
        //}



        //light = GameObject.Find("Directional Light").GetComponent<Light>();






    }


    public static Tuple<GameObject,UniversalPanel,int,int> GetTupleByIndex(int layer1,int layer2,int layer3,int layer4,int layer5)
    {
        return ComicManager.getComicsList()[layer1].Item4[layer2].Item4[layer3].Item4[layer4].Item4[layer5];

    }
    public static Tuple<GameObject, UniversalPanel, int, int> GetLastTuple()
    {
        int layer1 = ComicManager.getComicsList().Count;
        int layer2 = ComicManager.getComicsList()[layer1 ].Item4.Count;
        int layer3 = ComicManager.getComicsList()[layer1 ].Item4[layer2].Item4.Count;
        int layer4 = ComicManager.getComicsList()[layer1 ].Item4[layer2 ].Item4[layer3].Item4.Count;
        int layer5 = ComicManager.getComicsList()[layer1 ].Item4[layer2 ].Item4[layer3].Item4[layer4].Item4.Count;
        return ComicManager.getComicsList()[layer1].Item4[layer2].Item4[layer3].Item4[layer4].Item4[layer5];

    }
    public static Tuple<GameObject, UniversalPanel, int, int> GetLastTuple(int depth,int index)
    {
        int layer1 = ComicManager.getComicsList().Count;
        int layer2 = ComicManager.getComicsList()[layer1].Item4.Count;
        int layer3 = ComicManager.getComicsList()[layer1].Item4[layer2].Item4.Count;
        int layer4 = ComicManager.getComicsList()[layer1].Item4[layer2].Item4[layer3].Item4.Count;
        int layer5 = ComicManager.getComicsList()[layer1].Item4[layer2].Item4[layer1].Item4[layer2].Item4.Count;
        switch (depth)
        {
            case 0:
                {
                    layer1 = index;
                    layer2 = ComicManager.getComicsList()[layer1].Item4.Count;
                    layer3 = ComicManager.getComicsList()[layer1].Item4[layer2].Item4.Count;
                    layer4 = ComicManager.getComicsList()[layer1].Item4[layer2].Item4[layer3].Item4.Count;
                    layer5 = ComicManager.getComicsList()[layer1].Item4[layer2].Item4[layer3].Item4[layer4].Item4.Count;

                    return ComicManager.getComicsList()[layer1].Item4[layer2].Item4[layer3].Item4[layer4].Item4[layer5];
                }
            case 1:
                {

                    layer2 = index + 1;
                    layer3 = ComicManager.getComicsList()[layer1].Item4[layer2].Item4.Count;
                    layer4 = ComicManager.getComicsList()[layer1].Item4[layer2].Item4[layer3].Item4.Count;
                    layer5 = ComicManager.getComicsList()[layer1].Item4[layer2].Item4[layer1].Item4[layer2].Item4.Count;

                    return ComicManager.getComicsList()[layer1].Item4[layer2].Item4[layer3].Item4[layer4].Item4[layer5];

                }
            case 2:
                {
                    layer3 = index + 1;
                    layer4 = ComicManager.getComicsList()[layer1].Item4[layer2].Item4[layer3].Item4.Count;
                    layer5 = ComicManager.getComicsList()[layer1].Item4[layer1].Item4[layer2].Item4[layer4].Item4.Count;

                    return ComicManager.getComicsList()[layer1].Item4[layer2].Item4[layer3].Item4[layer4].Item4[layer5];
                }
            case 3:
                {

                    layer4 = index + 1;
                    layer5 = ComicManager.getComicsList()[layer1].Item4[layer2].Item4[layer3].Item4[layer4].Item4.Count;

                    return ComicManager.getComicsList()[layer1].Item4[layer2].Item4[layer3].Item4[layer4].Item4[layer5];
                }
            case 4:
                {
                    layer5 = index + 1;

                    return ComicManager.getComicsList()[layer1].Item4[layer2].Item4[layer3].Item4[layer4].Item4[layer5];
                }
            default: return ComicManager.getComicsList()[layer1].Item4[layer2].Item4[layer3].Item4[layer4].Item4[layer5];
        }



    }

    /// <summary>
    /// Compares Current, Previous wrapping orders, and outputs either int3 or int4 as either current wrapper or previous wrapper
    /// </summary>
    /// <param name="int1"></param>
    /// <param name="int2"></param>
    /// <param name="int3"></param>
    /// <param name="int4"></param>
    /// <returns> either int3 or int4</returns>
    public int BoolIntOut(int int1, int int2, int int3, int int4)
    {
        if(int1 == 0 && int2 >0) // prev > 0 cur == 0 => Retrun prev super
        {
            return int4;
        }
        else if(int1 > 0 && int2 < int1) // prev >= 0 cur >0 => Return current Super
        {
            return int3;
        }
        else if(int2 > int1) // 
        {
            return int4;
        }
        else if (int1 == int2)
        {
            return int3;
        }
        return 0; // should never be reached, should add exception throw.
    }


    // Update is called once per frame
    void Update()
    {
        HRotateMixin.comicManagerMixin = Camera.main.GetComponent<CameraSequencer>().currentComicManagerMixin;
        /*
         current comic> current ch> current page> current panel = 0,0,0,0 : LookAt => count, count, count, count.
        elseif = a,b,c,0 : LookAt a,b,c-1,count
        elseif = 0,0,0,d : LookAt a,b,c,d-1

        panel
            if prevPanelId > panel id
                page
                    check page 0 =>
                    if prevPage id > page id   else check
                                foreach chapter
                                    if prevChapter id > chapter id
                                            foreach comic
                                                if prevComicId > comicId
                                                   check comicId 0
                                                else 
                                                   check prevComicId


         
         */



        Debug.Log("PanelId: " + gameObject.GetComponent<UniversalPanel>().PanelId + " , GetPanelFocus() => " + Camera.main.GetComponent<CameraSequencer>().GetPanelFocus());
        if (gameObject.GetComponent<UniversalPanel>().PanelId != Camera.main.GetComponent<CameraSequencer>().GetPanelFocus() + 1)
        {

            return;
        }

        if (comicManagerMixin.previousPanel < 0 || (comicManagerMixin.previousPanel > comicManagerMixin.currentPanel && comicManagerMixin.currentPage == 0))
        {

            if(GetLastTuple().Item4 == (int)Transition.transitionTypes.HRotate)
            {
                    GetLastTuple().Item1.GetComponent<HRotateMixin>().triggered = false;                    
                    GetLastTuple().Item1.GetComponent<HRotateMixin>().canTransition = false;
                    triggered = false;

            }
               
            

        }
        else if (GetTupleByIndex(0, BoolIntOut(comicManagerMixin.currentChapter,comicManagerMixin.previousChapter,comicManagerMixin.currentComic,comicManagerMixin.previousComic), BoolIntOut(comicManagerMixin.currentPage,comicManagerMixin.previousPage,comicManagerMixin.currentChapter,comicManagerMixin.previousChapter),BoolIntOut(comicManagerMixin.currentPanel,comicManagerMixin.previousPanel,comicManagerMixin.currentPage,comicManagerMixin.previousPage) ,comicManagerMixin.previousPanel).Item4 == (int)Transition.transitionTypes.HRotate)
        {
            HRotateMixin hRotateMixin = GetTupleByIndex(0, BoolIntOut(comicManagerMixin.currentChapter, comicManagerMixin.previousChapter, comicManagerMixin.currentComic, comicManagerMixin.previousComic), BoolIntOut(comicManagerMixin.currentPage, comicManagerMixin.previousPage, comicManagerMixin.currentChapter, comicManagerMixin.previousChapter), BoolIntOut(comicManagerMixin.currentPanel, comicManagerMixin.previousPanel, comicManagerMixin.currentPage, comicManagerMixin.previousPage), comicManagerMixin.previousPanel).Item1.GetComponent<HRotateMixin>();
            
            hRotateMixin.triggered = false;
            hRotateMixin.canTransition = false;
            triggered = false;

        }






        
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
            else if (heading >= -270)
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

        // Should be either 1 or -1 but may need to be checked and corrected
        
        quaternion = new Quaternion(Input.gyro.attitude.x, Input.gyro.attitude.y, Input.gyro.attitude.z, compassAdjust * Input.gyro.attitude.w);
       
        template.UpdateRotation(quaternion.z - initialRotation.z, quaternion.eulerAngles.z - initialRotation.eulerAngles.z);
        template.OnRotatePhone(camera);

        if (template.IsTransitionConditionMet() == false && !triggered)
            triggered = true;

        if (!triggered) // YOU SHALLL NOT PASS INSERT GANDALF ASCHII ART HAHA! If triggered is false, then the execution aborts.
        {
            Debug.Log("Triggered: " + triggered + " , IsTransitionConditionsMet() => " + template.IsTransitionConditionMet());
            return;
        }
        


        


        canTransition = template.IsTransitionConditionMet();
        
        if (MathF.Truncate(Time.realtimeSinceStartup) % 5f == 0 && isRunningDebug == false)
        {
            //mixin.CallbackExecute("TryTransition",Debug.Log(""));

            //Debug.Log("bool TryTransition() Returned:\t" + template.IsTransitionConditionMet() + ",\t"+gameObject.GetComponent<UniversalPanel>().PanelId);
           // Debug.Log("quaternionMonitoring: " + quaternion.ToString() + ",\t" + gameObject.GetComponent<UniversalPanel>().PanelId);
           // Debug.Log("compass adjust: " + compassAdjust + ",\t" + gameObject.GetComponent<UniversalPanel>().PanelId);
            isRunningDebug = true;
        }
        else if (MathF.Truncate(Time.realtimeSinceStartup) % 5f != 0)
        {
            isRunningDebug = false;
        }


        if(canTransition)
        {
            this.TriggerTransition();
            triggered = false;
        }


    }
    


}
