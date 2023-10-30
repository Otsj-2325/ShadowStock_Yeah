using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class Sound : MonoBehaviour
{
    public static Sound instance = null;

    private AudioSource audioSource = null;

    [SerializeField] public List<AudioClip> sound;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void PlaySE(int n)
    {
        if (sound[n] != null)
        {
            audioSource.PlayOneShot(sound[n]);
        }
    }
}
