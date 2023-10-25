using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class SCR_LightFall : MonoBehaviour
{
    private Rigidbody cp_Rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        cp_Rigidbody = GetComponent<Rigidbody>();
        cp_Rigidbody.useGravity = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
