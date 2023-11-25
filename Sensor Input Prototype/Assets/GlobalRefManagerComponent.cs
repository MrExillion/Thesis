using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SensorInputPrototype.MixinInterfaces;

public class GlobalRefManagerComponent : MonoBehaviour, IGlobalReferenceManager
{

    private void Awake()
    {
       //I dont actually think these clases are needed for static classes with interfaces to be accessible?
    }
}
