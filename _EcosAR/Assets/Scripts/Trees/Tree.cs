using System.Collections;
using UnityEngine;

public class Tree : MonoBehaviour
{
    [SerializeField]
    public TreeGrowthState TreeGrowthState;

    private Transform _treeTransform;

    private MeshRenderer _meshRenderer;

    private ParticleSystem _leavesParticleSystem;

    private Material _dryLeafMaterial;
    private Material _windLeafMaterial;
    private Material _transparentMaterial;
    private Material _seedLogMaterial;
    private Material _saplingLogMaterial;
    private Material _sproutLogMaterial;
    private Material _matureLogMaterial;
    private Material _snagLogMaterial;
    private Material _currentLogMaterial;
    private Material _meshLogMaterial;

    private Color _dryLeafColor;
    private Color _windLeafColor;

    private float _lerpDurationTimeInSeconds;
    private float _scaleLerpDurationTimeInSeconds;
    private float _lerpAmountPerUpdate;
    private WaitForEndOfFrame _waitForEndOfFrame;

    void Start()
    {
        _waitForEndOfFrame = new WaitForEndOfFrame();

        _treeTransform = GetComponent<Transform>();
        _meshRenderer = GetComponentInChildren<MeshRenderer>();

        _leavesParticleSystem = GetComponentInChildren<ParticleSystem>();

        _dryLeafMaterial = Resources.Load("Materials/Tree/DryLeaf", typeof(Material)) as Material;
        _windLeafMaterial = Resources.Load("Materials/Tree/WindLeaves", typeof(Material)) as Material;
        _transparentMaterial = Resources.Load("Materials/Tree/TransparentMaterial", typeof(Material)) as Material;
        _seedLogMaterial = Resources.Load("Materials/Tree/SeedLogMaterial", typeof(Material)) as Material;
        _saplingLogMaterial = Resources.Load("Materials/Tree/SaplingLogMaterial", typeof(Material)) as Material;
        _sproutLogMaterial = Resources.Load("Materials/Tree/SproutLogMaterial", typeof(Material)) as Material;
        _matureLogMaterial = Resources.Load("Materials/Tree/MatureLogMaterial", typeof(Material)) as Material;
        _snagLogMaterial = Resources.Load("Materials/Tree/SnagLogMaterial", typeof(Material)) as Material;
        _meshLogMaterial = _meshRenderer.materials[0];
        _currentLogMaterial = _meshRenderer.materials[0];

        _dryLeafColor = new Color((185 / 255f), (122 / 255f), (87 / 255f));
        _windLeafColor = _leavesParticleSystem.main.startColor.color;

        _lerpAmountPerUpdate = 0f;
        _scaleLerpDurationTimeInSeconds = 3f;
        _lerpDurationTimeInSeconds = _scaleLerpDurationTimeInSeconds / 2;

        SetGrowthState();
    }

    public void SetGrowthState()
    {
        var leavesParticleSystem = GetComponentInChildren<ParticleSystem>();
        
        switch (TreeGrowthState)
        {
            case TreeGrowthState.Seed:
                {
                    #region Update Particle Leaves

                    _leavesParticleSystem.Clear();
                    _leavesParticleSystem.Stop();

                    #endregion

                    #region Update Log Material

                    StartCoroutine(ChangeLogMaterialOverTime(_seedLogMaterial));

                    #endregion

                    #region Update Leaf Material

                    var materials = _meshRenderer.materials;

                    materials[1] = _transparentMaterial;

                    _meshRenderer.materials = materials;

                    #endregion

                    #region Scaling 

                    var destinationScale = new Vector3(0.1f, 0.1f, 0.1f);

                    StartCoroutine(ScaleOverTime(destinationScale));

                    #endregion

                    break;
                }
            case TreeGrowthState.Sprout:
                {
                    #region Update Particle Leaves

                    _leavesParticleSystem.Clear();

                    _leavesParticleSystem.Stop();

                    #endregion
                    
                    #region Update Log Material

                    StartCoroutine(ChangeLogMaterialOverTime(_sproutLogMaterial));

                    #endregion

                    #region Update Update Leaf Material

                    var materials = _meshRenderer.materials;

                    materials[1] = _transparentMaterial;

                    _meshRenderer.materials = materials;

                    #endregion

                    #region Scaling 
                    var destinationScale = new Vector3(0.4f, 0.4f, 0.4f);

                    StartCoroutine(ScaleOverTime(destinationScale));
                    #endregion

                    break;
                }
            case TreeGrowthState.Sapling:
                {
                    #region Update Particle Leaves

                    _leavesParticleSystem.Clear();
                    _leavesParticleSystem.Stop();

                    #endregion

                    #region Update Log Material

                    StartCoroutine(ChangeLogMaterialOverTime(_saplingLogMaterial));

                    #endregion

                    #region Scaling 

                    var destinationScale = new Vector3(0.6f, 0.6f, 0.6f);

                    StartCoroutine(ScaleOverTime(destinationScale));

                    #endregion

                    break;
                }
            case TreeGrowthState.Mature:
                {
                    #region Update Particle Leaves

                    var mainModule = _leavesParticleSystem.main;

                    mainModule.startColor = _windLeafColor;

                    _leavesParticleSystem.Clear();

                    _leavesParticleSystem.Play();

                    #endregion
                    
                    #region Update Log Material

                    StartCoroutine(ChangeLogMaterialOverTime(_matureLogMaterial));

                    #endregion

                    #region Update Leaf Material

                    var materials = _meshRenderer.materials;

                    materials[1] = _windLeafMaterial;

                    _meshRenderer.materials = materials;

                    #endregion

                    #region Scaling 
                    var destinationScale = new Vector3(1f, 1f, 1f);

                    StartCoroutine(ScaleOverTime(destinationScale));
                    #endregion

                    break;
                }
            case TreeGrowthState.Snag:
                {
                    #region Update Particle Leaves

                    _leavesParticleSystem.Clear();
                    _leavesParticleSystem.Play();

                    var mainModule = _leavesParticleSystem.main;

                    mainModule.startColor = _dryLeafColor;

                    #endregion

                    #region Update Log Material

                    StartCoroutine(ChangeLogMaterialOverTime(_snagLogMaterial));

                    #endregion

                    #region Update Update Leaf Material

                    var materials = _meshRenderer.materials;

                    materials[1] = _dryLeafMaterial;

                    _meshRenderer.materials = materials;

                    #endregion

                    #region Scaling 

                    var destinationScale = new Vector3(1.1f, 1.1f, 1.1f);

                    StartCoroutine(ScaleOverTime(destinationScale));

                    #endregion

                    break;
                }
        }
    }

    public void UpdateGrowthState()
    {
        Grow();
        SetGrowthState();
    }

    public void Grow()
    {
        switch (TreeGrowthState)
        {
            case TreeGrowthState.Seed:
                TreeGrowthState = TreeGrowthState.Sprout;
                break;
            case TreeGrowthState.Sprout:
                TreeGrowthState = TreeGrowthState.Sapling;
                break;
            case TreeGrowthState.Sapling:
                TreeGrowthState = TreeGrowthState.Mature;
                break;
            case TreeGrowthState.Mature:
                TreeGrowthState = TreeGrowthState.Snag;
                break;
            case TreeGrowthState.Snag:
                TreeGrowthState = TreeGrowthState.Seed;
                break;
        }
    }

    IEnumerator ScaleOverTime(Vector3 destinationScale)
    {
        Vector3 originalScale = _treeTransform.localScale;

        float currentTime = 0.0f;

        do
        {
            _treeTransform.localScale = Vector3.Lerp(originalScale, destinationScale, currentTime / _lerpDurationTimeInSeconds);

            currentTime += Time.deltaTime;

            yield return null;

        } while (currentTime <= _lerpDurationTimeInSeconds);
    }

    IEnumerator ChangeLogMaterialOverTime(Material transitionToMaterial)
    {
        while (_currentLogMaterial != transitionToMaterial)
        {
            _meshLogMaterial.color = Color.Lerp(_currentLogMaterial.color, transitionToMaterial.color, _lerpAmountPerUpdate);

            if (_lerpAmountPerUpdate < 1)
            {
                _lerpAmountPerUpdate += Time.deltaTime / _lerpDurationTimeInSeconds;

                if (_lerpAmountPerUpdate > 1)
                {
                    _currentLogMaterial = transitionToMaterial;

                    var materials = _meshRenderer.materials;

                    _meshRenderer.materials[0] = _currentLogMaterial;

                    _meshRenderer.materials = materials;

                    _lerpAmountPerUpdate = 0;
                }
            }

            yield return _waitForEndOfFrame;
        }
    }
}