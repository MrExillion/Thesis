using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarPropagationMove : MonoBehaviour
{
    [SerializeField] private float maximumLifetime = 15f;
    private float lifeTime = 0f;
    [SerializeField] private Vector3 propagationScaler = new Vector3(1f, 1f, 1f); // Vector at least at 100% scale this scale is applied per frame, adjusted by time fixed delta time.
    private Vector3 waveScale; // private current variable for the current scale
    [SerializeField] private float moveSpeed = 1; // scalar multiplied by time fixed delta time and the current position of the transform in order to Move() the rigidBody
    // Start is called before the first frame update
    void OnEnable()
    {
        waveScale = gameObject.transform.localScale;
        lifeTime = Time.timeSinceLevelLoad;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 new_waveScale = waveScale * Time.fixedDeltaTime; // calibrating for physics framerate.
        gameObject.transform.localScale.Set(new_waveScale.x * propagationScaler.x, new_waveScale.y * propagationScaler.y, new_waveScale.z * propagationScaler.z);
        gameObject.GetComponent<Rigidbody>().Move(new Vector3 (transform.position.x, transform.position.y, transform.position.z) + (transform.forward * moveSpeed * Time.fixedDeltaTime), Quaternion.identity);
    }

    private void Update()
    {
        if (lifeTime + Time.timeSinceLevelLoad >= maximumLifetime)
        {
            Destroy(this);
        }
    }
}
