using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM_Controller : MonoBehaviour
{


    [SerializeField] AudioClip sound_BGM;
    [SerializeField] AudioClip sound_Win;
    [SerializeField] AudioClip sound_Lose;


    AudioSource audioSource;




    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0.5f;
        audioSource.loop = true;
        audioSource.clip = sound_BGM;
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Win( )
    {
        audioSource.PlayOneShot(sound_Win);

    }
    void Lose()
    {
        audioSource.PlayOneShot(sound_Lose);

    }


}
