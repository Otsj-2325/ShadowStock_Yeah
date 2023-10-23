using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SCR_IsHaveShadow : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI OutText;
    [SerializeField] bool isHave;

    // Start is called before the first frame update
    void Start()
    {
        OutText.text = "Don`t have";
        isHave = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isHave)
        {
            OutText.text = "Have";
        }
        else
        {
            OutText.text = "Don`t have";
        }
    }
}
