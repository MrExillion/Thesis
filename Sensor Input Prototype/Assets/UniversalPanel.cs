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
    /// <summary>
    /// Transition types are passed and used as ints across mixins. Due to Microphone NOT being a multi instance capable class that ISN'T Proprietary, any new Transitions added using <see cref="Microphone.Start"/> MUST go through <seealso cref="MicrophoneBlowAirTrigger.cs"/> AND include an update to the private property <code>int[] microphoneTransitions</code>, with fixed size, as this is checking for active panel transitions that use the microphone before calling <see cref="Microphone.End"/>. Resulting int the microphone appearing to be recording, but has samples less than a second resulting in FFT showing band output with memory accretion, and amplitude to always return 0! This breaks transitions relying on the <see cref="Microphone"/>.
    /// </summary>
    enum transitionTypeEnum { HRotate = 0, DefaultTab = 1, TabOnObject = 2, Swipe = 3, DragAndDrop = 4, MicrophoneBlowAir = 5, LightSensorBlocked = 6, LightSensorBlockedPlusShushMicrophone = 7 } // I want this to maybe be some fancier solution in the future, either by automatically finding the transition through appliance, or by using Enums to be more inspector reader friendly
    [SerializeField] transitionTypeEnum transitionTypes;
    /// <summary>
    /// This int is based on the Inspector set Enum Dropdown. 
    /// <para>
    /// <br><see cref="HRotate"/>.cs AND (Interface as Mixin Map) <seealso cref="MHRotate"/>.cs AND <see cref="HRotateTemplate"/>.cs AND <see cref="HRotateMixin"/>.cs = 0,</br>
    /// <br> <see cref="TabTransition"/>.cs AND (Interface as Mixin Map) <see cref="MTabTransition"/>.cs AND <see cref="TabTransitionTemplateMixin"/>.cs = [1;3],</br>
    /// <br> <see cref="TabTransition"/>.cs AND (Interface as Mixin Map) <seealso cref="MTabTransition"/>.cs AND <see cref="TabTransitionTemplateMixin"/>.cs AND <see cref="DragAndDrop"/>.cs = 4,</br>
    /// <br> <see cref="MicrophoneFifoAmp"/>.cs AND (Interface as Mixin Map) <seealso cref="MMicrophoneFifoAmp"/>.cs AND <see cref="MicrophoneBlowAirTrigger"/>.cs = 5,</br>   
    /// <br> <see cref="LightSensorTransition"/>.cs AND (Interface as Mixin Map) <seealso cref="MLightSensorTransition"/>.cs AND <see cref="LightSensorTransitionComponent"/>.cs = 6,</br>
    /// <br> <see cref="MicrophoneFifoAmp"/>.cs AND (Interface as Mixin Map) <seealso cref="MMicrophoneFifoAmp"/>.cs AND <see cref="MicrophoneBlowAirTrigger"/>.cs AND <see cref="LightSensorTransition"/>.cs AND (Interface as Mixin Map) <seealso cref="MLightSensorTransition"/>.cs AND <see cref="LightSensorTransitionComponent"/>.cs = 7</br>
    ///</para>
    ///
    /// <br>Important for Developers Only:</br>
    /// <br>Due to Microphone NOT being a multi instance capable class that ISN'T Proprietary, any new Transitions added using <see cref="Microphone.Start"/> MUST go through MicrophoneBlowAirTrigger.cs AND include an update to the private property <code>int[] microphoneTransitions</code>, with fixed size, as this is checking for active panel transitions that use the microphone before calling <see cref="Microphone.End"/>. Resulting int the microphone appearing to be recording, but has samples less than a second resulting in FFT showing band output with memory accretion, and amplitude to always return 0! This breaks transitions relying on the Microphone.</br>
    /// </summary>
    [HideInInspector]public int transitionType = 0;
    private Type TransitionBehaviour;

    private void Awake()
    {
        transitionType = (int)transitionTypes;
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

    public static bool ResetConditions(UniversalPanel panel)
    {
        switch (panel.transitionType)
        {
            case (int)Transition.transitionTypes.HRotate:
                {

                    
                    return (Camera.main.GetComponent<CameraSequencer>().IsResetConditionMet());


                    

                }

            default:
                return true;
                break;
        
        
        }

                
    }

   


}
