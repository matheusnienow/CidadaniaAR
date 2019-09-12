using UnityEngine;

public class Fire : MonoBehaviour
{
    void Start()
    {
        var particleSystems = GetComponentsInChildren<ParticleSystem>();

        foreach (var particleSystem in particleSystems) {
            particleSystem.Stop();
        }
    }
}