using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Android;
using Gyroscope = UnityEngine.InputSystem.Gyroscope;
using SensorInputPrototype.InspectorReadOnlyCode;

public class BalanceInputScript : MonoBehaviour
{
    public float frequency = 16f;
    public Light light;
    public static LightSensor lightsensorref;
    #if UNITY_EDITOR 
    [ShowOnly]
    #endif
    [SerializeField]
    private float maxIntensity = 256f;
    #if UNITY_EDITOR
    [ShowOnly]
    #endif
    [SerializeField]
    private float curentIntensity;
    private void Awake()
    {
        //#if (PLATFORM_ANDROID == true && UNITY_EDITOR == false)

        //InputSystem.AddDevice<AndroidLightSensor>("AndroidLightSensor");
        //InputSystem.EnableDevice(InputSystem.GetDevice("AndroidLightSensor"));

        lightsensorref = InputSystem.GetDevice<LightSensor>();
        InputSystem.EnableDevice(lightsensorref);



        //#endif
    }

    // Start is called before the first frame update
    void Start()
    {
        if (lightsensorref.enabled)
            Debug.Log("light sensor is enabled");
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



        light = GameObject.Find("Directional Light").GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        lightsensorref.MakeCurrent();
        //if(Gyroscope.current != null)
        //gameObject.transform.rotation.Set(AttitudeSensor.current.attitude.value.x, AttitudeSensor.current.attitude.value.y, AttitudeSensor.current.attitude.value.z, AttitudeSensor.current.attitude.value.w);
        Debug.Log(""+Input.gyro.attitude.x+", "+Input.gyro.attitude.y + ", " + Input.gyro.attitude.z + ", " + Input.gyro.attitude.w);
        gameObject.transform.rotation = new Quaternion(Input.gyro.attitude.x, Input.gyro.attitude.z, Input.gyro.attitude.y, -1*Input.gyro.attitude.w);
        Input.gyro.enabled = true;
        curentIntensity = LightSensor.current.lightLevel.value;
        light.intensity = Mathf.Log10(curentIntensity)/10;
        //Debug.Log(light.intensity);
        
    
    }   
}
