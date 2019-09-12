using System.Collections;
using UnityEngine;

public class Water : MonoBehaviour
{
    private Vector3 _waterEvaporationPosition;
    private Vector3 _waterNormalPosition;
    private ParticleSystem _smokeParticleSystem;

    private WaitForEndOfFrame _waitForEndOfFrame;
    private float _lerpAmountPerUpdate;
    private float _lerpDurationTimeInSeconds;

    private bool _evaporating, _canEvaporate;
    private bool _condensing, _canCondense;

    private void Start()
    {
        _waterNormalPosition = transform.localPosition;

        _waterEvaporationPosition = transform.localPosition;
        _waterEvaporationPosition.y -= 0.13f;

        _lerpDurationTimeInSeconds = 5f;
        _lerpAmountPerUpdate = 0f;

        _smokeParticleSystem = GetComponentInChildren<ParticleSystem>();
        _smokeParticleSystem.Stop();

        _evaporating = false;
        _canEvaporate = true;
        _condensing = false;
        _canCondense = false;

        _waitForEndOfFrame = new WaitForEndOfFrame();
    }

    public void Evaporate()
    {
        if ((!_evaporating || !_condensing) && _canEvaporate)
        {
            StartCoroutine(Evaporate(_waterNormalPosition, _waterEvaporationPosition));
        }
    }

    public void Condense()
    {
        if ((!_evaporating || !_condensing) && _canCondense)
        {
            StartCoroutine(Condense(_waterEvaporationPosition, _waterNormalPosition));
        }
    }

    IEnumerator Evaporate(Vector3 currentPos, Vector3 targetPos)
    {
        while (transform.localPosition != _waterEvaporationPosition)
        {
            if (!_smokeParticleSystem.isPlaying)
            {
                _smokeParticleSystem.Play();
            }

            _evaporating = true;
            _canEvaporate = false;
            _canCondense = false;

            transform.localPosition = Vector3.Lerp(currentPos, targetPos, _lerpAmountPerUpdate);

            if (_lerpAmountPerUpdate < 1)
            {
                _lerpAmountPerUpdate += Time.deltaTime / _lerpDurationTimeInSeconds;

                if (_lerpAmountPerUpdate > 1)
                {
                    transform.localPosition = _waterEvaporationPosition;
                    _lerpAmountPerUpdate = 0;
                    _evaporating = false;
                    _canEvaporate = false;
                    _canCondense = true;
                    _smokeParticleSystem.Stop();
                }
            }

            yield return _waitForEndOfFrame;
        }
    }

    IEnumerator Condense(Vector3 currentPos, Vector3 targetPos)
    {
        while (transform.localPosition != _waterNormalPosition)
        {
            _condensing = true;
            _canEvaporate = false;
            _canCondense = false;

            transform.localPosition = Vector3.Lerp(currentPos, targetPos, _lerpAmountPerUpdate);

            if (_lerpAmountPerUpdate < 1)
            {
                _lerpAmountPerUpdate += Time.deltaTime / _lerpDurationTimeInSeconds;

                if (_lerpAmountPerUpdate > 1)
                {
                    transform.localPosition = _waterNormalPosition;
                    _lerpAmountPerUpdate = 0;
                    _condensing = false;
                    _canCondense = false;
                    _canEvaporate = true;
                }
            }

            yield return _waitForEndOfFrame;
        }
    }
}