using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowPreviewProcess : MonoBehaviour
{
    [SerializeField] GameObject PreviewObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool SetPreviewObject(GameObject getObject)
    {
        if(PreviewObject == null){
            PreviewObject = getObject;
            return true;
        }

        return false;
    }

    


}