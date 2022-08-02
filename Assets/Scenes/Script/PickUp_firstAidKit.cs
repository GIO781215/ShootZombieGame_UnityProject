using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp_firstAidKit : MonoBehaviour
{
    //��U���o�����S�Ī���
    public ParticleSystem effect_1_1;
    public ParticleSystem effect_1_2;
    public ParticleSystem effect_2_1;
    public ParticleSystem effect_3_1;
    public ParticleSystem effect_3_2;

    float i = 255;

    [SerializeField] AudioClip sound_GetItem;
    AudioSource audioSource;



    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

    }


    [System.Obsolete]
    public void DestroySelf()
    {
        audioSource.PlayOneShot(sound_GetItem);
        StartCoroutine(graduallyDisappear());
    }

    [System.Obsolete]
    private IEnumerator graduallyDisappear()
    {
        while (true)
        {
            i = Mathf.Lerp(i, 0, 0.2f);

            effect_1_1.startColor = new Color(255f / 255f, 255f / 255f, 255f / 255f, i / 255f);
            effect_1_2.startColor = new Color(255f / 255f, 255f / 255f, 255f / 255f, i / 255f);
            effect_2_1.startColor = new Color(255f / 255f, 255f / 255f, 255f / 255f, i / 255f);
            effect_3_1.startColor = new Color(255f / 255f, 255f / 255f, 255f / 255f, i / 255f);
            effect_3_2.startColor = new Color(255f / 255f, 255f / 255f, 255f / 255f, i / 255f);

            yield return new WaitForSeconds(0.01f);

            if (i < 1)
            {
                Destroy(this.gameObject, 1.5f);
            }
        }
    }
}
