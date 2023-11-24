using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalRefManagerComponent : MonoBehaviour, IGlobalReferenceManager
{

    private void Awake()
    {
       //I dont actually think these clases are needed for static classes with interfaces to be accessible?
    }
}
