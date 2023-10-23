using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SCR_NumDisplay : MonoBehaviour
{
    [SerializeField] List<Sprite> m_NumberSprite;
    [SerializeField] GameObject m_BaseNumberPos;
    [SerializeField] float m_margin;
    [SerializeField] public int m_Display;
    [SerializeField] List<int> m_NumbersList;
    [SerializeField] bool m_IsOrderUpper = false;

    int old = 0;
    List<GameObject> m_DisplayObjects;
    bool setSprite = false;
    Vector3 basePosition = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        foreach(Sprite s in m_NumberSprite)
        {
            if(s == null)
            {
                setSprite = false;
                break;

            }

            setSprite = true;
        }

        m_BaseNumberPos.GetComponent<Image>().color = new Color(1.0f,1.0f, 1.0f, 0.0f);
        m_NumbersList = new List<int>();
        m_DisplayObjects = new List<GameObject>();

        basePosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 0) return; //ヒットストップ中

        if (!setSprite || old == m_Display)
        {
            return;
        }

        if(old != m_Display)
        {
            foreach(GameObject g in m_DisplayObjects)
            {
                Destroy(g);
            }
        }

        if(m_Display < 0){
            m_Display = 0;
        }

        CalculateNumber();
        CreateDisplay();

    }

    private void CalculateNumber()
    {
        m_NumbersList = new List<int>();
        
        int temp = m_Display;
        if (temp <= 0)
        {
            m_NumbersList.Add(0);
        }
        else
        {
            while (temp > 0)
            {
                m_NumbersList.Add(temp % 10);
                temp = temp / 10;
            }
        }


        old = m_Display;
    }

    private void CreateDisplay()
    {
        RectTransform baseTransform = m_BaseNumberPos.GetComponent<RectTransform>();
        var pos = baseTransform.anchoredPosition;

        if (m_IsOrderUpper){
            m_NumbersList.Reverse();
        }
        else{
            
        }

        foreach(int n in m_NumbersList)
        {
            GameObject dispnum = new GameObject();
            m_DisplayObjects.Add(dispnum);
            dispnum.AddComponent<Image>();
            dispnum.GetComponent<Image>().sprite = m_NumberSprite[n];

            dispnum.transform.SetParent(this.gameObject.transform);
            RectTransform tf = dispnum.GetComponent<RectTransform>();
            tf.anchoredPosition = pos;
            tf.localScale = baseTransform.localScale;

            if (m_IsOrderUpper)
            {
                pos.x += m_margin;
            }
            else{
                pos.x -= m_margin;
            }
        }
    }
}
