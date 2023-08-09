using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesController : MonoBehaviour
{
    [SerializeField] ParticleSystem dashParticle;
    [SerializeField] int occurAfterDash;
    [SerializeField] float dustFormatPeriod;

    private float counter;

    private void Update()
    {
        counter += Time.deltaTime;
        if(Mathf.Abs(counter) > occurAfterDash)
        {
            if(counter > dustFormatPeriod)
            {
                dashParticle.Play();
                counter = 0;
            }
        }
    }
}