using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SCR_LoadScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Change", 2.0f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    //ÉVÅ[ÉìêÿÇËë÷Ç¶
    void Change()
    {
        SceneManager.LoadScene(SCR_ChangeScene.loadAfterScene);
    }
}
