using System;
using System.Collections;
using UnityEngine;

public class TerrainController
{
    private Func<IEnumerator, Coroutine> _startCoroutine;
    private Renderer _renderer;
    private GameObject _terrain;
    private Material _grassMaterial;
    private Material _snowMaterial;
    private Material _dryMaterial;
    private Material _currentMaterial;
    private float _lerpDurationTimeInSeconds;
    private float _lerpAmountPerUpdate;
    private bool _lerpingTerrain;
    private WaitForEndOfFrame _waitForEndOfFrame;

    public TerrainController(Func<IEnumerator, Coroutine> startCoroutine)
    {
        _terrain = GameObject.Find("Terrain");
        _renderer = _terrain.GetComponent<Renderer>();
        _grassMaterial = Resources.Load<Material>("Materials/Ground/Grass") as Material;
        _snowMaterial = Resources.Load<Material>("Materials/Ground/Snow") as Material;
        _dryMaterial = Resources.Load<Material>("Materials/Ground/Dry") as Material;

        _currentMaterial = _renderer.material;
        _lerpDurationTimeInSeconds = 5f;
        _lerpAmountPerUpdate = 0f;

        _startCoroutine = startCoroutine;
        _waitForEndOfFrame = new WaitForEndOfFrame();
    }

    public void Update(SceneState sceneState, float temperature)
    {
        if (sceneState == SceneState.Unfavorable && temperature >= 40)
        {
            if ((_currentMaterial != _dryMaterial) && !_lerpingTerrain)
            {
                _startCoroutine.Invoke(ChangeTerrain(_dryMaterial));
            }
        }
        else if (sceneState == SceneState.Unfavorable && temperature < 10)
        {
            if ((_currentMaterial != _snowMaterial) && !_lerpingTerrain)
            {
                _startCoroutine.Invoke(ChangeTerrain(_snowMaterial));
            }
        }
        else if (sceneState == SceneState.Favorable)
        {
            if ((_currentMaterial != _grassMaterial) && !_lerpingTerrain)
            {
                _startCoroutine.Invoke(ChangeTerrain(_grassMaterial));
            }
        }
    }

    private IEnumerator ChangeTerrain(Material transitionToMaterial)
    {
        while (_currentMaterial != transitionToMaterial)
        {
            _lerpingTerrain = true;

            _renderer.material.color = Color.Lerp(_currentMaterial.color, transitionToMaterial.color, _lerpAmountPerUpdate);

            if (_lerpAmountPerUpdate < 1)
            {
                _lerpAmountPerUpdate += Time.deltaTime / _lerpDurationTimeInSeconds;

                if (_lerpAmountPerUpdate > 1)
                {
                    _currentMaterial = transitionToMaterial;
                    _lerpAmountPerUpdate = 0;
                    _lerpingTerrain = false;
                }
            }

            yield return _waitForEndOfFrame;
        }
    }
}