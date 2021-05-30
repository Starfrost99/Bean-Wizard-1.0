using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleAttract : MonoBehaviour
{
    private Vector3 location;
    public GameObject target;
    public Vector3 distance;
    private ParticleSystem ps;
    private ParticleSystem.Particle[] emittedParticles;
    public float multiplier = 1;
    // Start is called before the first frame update
    void Start()
    {
        ps = gameObject.GetComponent<ParticleSystem>();
        emittedParticles = new ParticleSystem.Particle[ps.particleCount];
        ps.GetParticles(emittedParticles);
    }

    // Update is called once per frame
    void Update()
    {
        location = gameObject.transform.position;
        //distance = target.transform.position - location;

        emittedParticles = new ParticleSystem.Particle[ps.particleCount];
        ps.GetParticles(emittedParticles);

        //Magnetize Particles
        for(int p = 0; p < emittedParticles.Length; p++)
        {
            distance = target.transform.position - emittedParticles[p].position;
            emittedParticles[p].velocity += distance * (multiplier/ (distance.magnitude*distance.magnitude));
            ps.SetParticles(emittedParticles, emittedParticles.Length);
        }

    }
}
