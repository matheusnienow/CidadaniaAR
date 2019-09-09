using UnityEngine;

public class SnowController
{
    public bool Snowing { get; set; }

    private ParticleSystem _snowParticleSystem;

    public SnowController()
    {
        _snowParticleSystem = GameObject.Find("Snow Particle System").GetComponent<ParticleSystem>();

        Snowing = false;
    }

    public void Update(float temperature)
    {
        if (temperature <= 5f && !Snowing)
        {
            StartSnowing();

        }
        else if (temperature > 10f)
        {
            StopSnowing();
        }

        if (Snowing)
        {
            StartSnowing();
        }
        else
        {
            StopSnowing();
        }
    }

    void StartSnowing()
    {
        Snowing = true;

        if (!_snowParticleSystem.isEmitting)
        {
            _snowParticleSystem.Play();
        }
    }

    void StopSnowing()
    {
        Snowing = false;

        if (_snowParticleSystem.isEmitting)
        {
            _snowParticleSystem.Stop();
        }
    }
}