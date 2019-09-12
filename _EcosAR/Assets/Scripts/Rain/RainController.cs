using UnityEngine;

public class RainController {

    public bool Raining { get; set; }

    private GameObject _clouds;

    private float _period = 5f;

    private float _checkInterval = 5f;

    private ParticleSystem[] _rainParticleSystems;

    public RainController() {
        _clouds = GameObject.FindGameObjectWithTag("Clouds");

        _rainParticleSystems = _clouds.GetComponentsInChildren<ParticleSystem>();

        foreach (var rainParticleSystem in _rainParticleSystems) {
            rainParticleSystem.Stop();
        }

        _clouds.SetActive(false);

        Raining = false;
    }

    //se a cena tem nuvens, faz chover de tempo em tempo.
    public void Update(bool cloudsCorrectPosition) {
        if (_clouds.activeSelf && _period > _checkInterval) {

            foreach (var rainParticleSystem in _rainParticleSystems) {

                if (rainParticleSystem.isStopped && cloudsCorrectPosition) {
                    rainParticleSystem.Play();
                    Raining = true;
                } else {
                    rainParticleSystem.Stop();
                    Raining = false;
                }

            }
            _period = 0;
        }

        _period += Time.deltaTime;
    }
}