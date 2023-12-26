using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SensorInputPrototype.MixinInterfaces;


public class LightSensorTransitionComponent : MonoBehaviour, MLightSensorTransition
{
    //Hidden Variables
    private UniversalPanel universalPanel;

    //INSPECTOR VARIABLES
    public float lightSensorCoverTime = 0.5f;
    void Start()
    {
        universalPanel = GetComponent<UniversalPanel>();
        this.SetupLightSensor(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (universalPanel != GlobalReferenceManager.GetCurrentUniversalPanel())
        {
            return;
        }
        this.LightSensorUpdate();
        //Debug.Log("LightSensorTransitionComponent is missing if transitiontype == 6, then TriggerTransition and it is also missing a transitiontype == 7 TriggerTransition if it is also fulfilling a MMicrophoneFifoAmp Volume and frequency readouts."); // This is handled inside the mixin function.
            this.OnLightSensorCovered(lightSensorCoverTime);
        this.LightSensorReadOutDump();


    }
}
