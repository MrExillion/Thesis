using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnDialBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.rotation = Camera.main.transform.rotation;
    }
}
