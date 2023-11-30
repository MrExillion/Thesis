using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Mathematics;
using SensorInputPrototype.InspectorReadOnlyCode;
using SensorInputPrototype.MixinInterfaces;
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
    //[SerializeField] public GameObject t;
    #if UNITY_EDITOR
    [ShowOnly] 
    #endif
    [SerializeField]
    private static HRotateTemplate template;
    private bool isRunningDebug;
    private bool triggered = true;
    private void Awake()
    {
        //if (gameObject.GetComponent<HRotateTemplate>() == t.GetComponent<HRotateTemplate>())
        if(template == null)
            template = gameObject.GetComponent<HRotateTemplate>();
        else
            return;
        //#if (PLATFORM_ANDROID == true && UNITY_EDITOR == false)

        //InputSystem.AddDevice<AndroidLightSensor>("AndroidLightSensor");
        //InputSystem.EnableDevice(InputSystem.GetDevice("AndroidLightSensor"));

        //#endif
        Input.gyro.enabled = true;
        Input.compass.enabled = true;
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

    // Update is called once per frame
    void Update()
    {
        if(gameObject.GetComponent<UniversalPanel>().PanelId != Camera.main.GetComponent<CameraSequencer>().GetPanelFocus()+1)
        {
            Debug.Log("PanelId: "+gameObject.GetComponent<UniversalPanel>().PanelId +" , GetPanelFocus() => "+ Camera.main.GetComponent<CameraSequencer>().GetPanelFocus());
            return;
        }
        if(!triggered && template.IsTransitionConditionMet())
        {
            Debug.Log("Triggered: " + triggered + " , IsTransitionConditionsMet() => " + template.IsTransitionConditionMet());   
            return;
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

        // Should be either 1 or -1 but may need to be checked and corrected
        
        quaternion = new Quaternion(Input.gyro.attitude.x, Input.gyro.attitude.y, Input.gyro.attitude.z, compassAdjust * Input.gyro.attitude.w);
       
        template.UpdateRotation(quaternion.z - initialRotation.z, quaternion.eulerAngles.z - initialRotation.eulerAngles.z);
        template.OnRotatePhone(camera);

        if (template.IsTransitionConditionMet() == false)
            triggered = true;


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


        if(canTransition && triggered)
        {
            this.TriggerTransition();
            triggered = false;
        }


    }
    


}
