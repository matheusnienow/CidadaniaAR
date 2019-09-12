using System.Collections;
using UnityEngine;

public class SimulationController : MonoBehaviour
{
    private DayNightCycleController _dayNightCycleController;
    private WindController _windController;
    private CloudController _cloudController;
    private RainController _rainController;
    private WaterController _waterController;
    private TemperatureController _temperatureController;
    private SnowController _snowController;
    private TerrainController _terrainController;
    private TreeGrowthStateController _treeGrowthStateController;
    private SceneState _currentSceneState;

    private bool _sceneRestarted;

    void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;

        _currentSceneState = SceneState.Unfavorable;

        _dayNightCycleController = new DayNightCycleController();
        _windController = new WindController();
        _cloudController = new CloudController(StartCoroutine);
        _rainController = new RainController();
        _temperatureController = new TemperatureController(StartCoroutine, _dayNightCycleController);
        _waterController = new WaterController();
        _terrainController = new TerrainController(StartCoroutine);
        _snowController = new SnowController();
        _treeGrowthStateController = new TreeGrowthStateController(StartCoroutine, _windController.UpdateTreeWindForceOnGrow);

        _sceneRestarted = false;
    }

    void Update()
    {
        UpdateSceneState();

        _dayNightCycleController.Update();
        _windController.Update();
        _temperatureController.Update();
        _snowController.Update(_temperatureController.Temperature);
        _cloudController.Update(_windController.WindForce, _temperatureController.Temperature);
        _rainController.Update(_cloudController.IsOnRainingPosition);
        _waterController.Update(_temperatureController.Temperature, _rainController.Raining, _cloudController.IsOnRainingPosition);
        _terrainController.Update(_currentSceneState, _temperatureController.Temperature);
        _treeGrowthStateController.Update(_currentSceneState);
    }

    void UpdateSceneState()
    {
        if (_temperatureController.Temperature < 10 || _temperatureController.Temperature >= 40)
        {
            _currentSceneState = SceneState.Unfavorable;
        }
        else if (_temperatureController.Temperature >= 10 && _temperatureController.Temperature < 40)
        {
            _currentSceneState = SceneState.Favorable;
        }

        RestartScene(_temperatureController.TargetTemperature);
    }

    IEnumerator RestartSceneAfterSeconds()
    {
        yield return new WaitForSeconds(5);

        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    void RestartScene(float temperature)
    {
        if (temperature >= 49.5f && !_sceneRestarted)
        {
            RestartSceneEffects();
            StartCoroutine(RestartSceneAfterSeconds());

            _sceneRestarted = true;
        }
    }

    void RestartSceneEffects()
    {
        var word = GameObject.FindGameObjectWithTag("Scene Target");

        var particleSystems = word.GetComponentsInChildren<ParticleSystem>();

        //para todas as particles
        foreach (var particleSystem in particleSystems)
        {
            particleSystem.Stop();
        }

        //ativa o fogo
        var fire = GameObject.Find("Fire");

        var fireParticles = fire.GetComponentsInChildren<ParticleSystem>();

        foreach (var particleSystem in fireParticles)
        {
            particleSystem.Play();
        }

        //remove todas as trees
        var trees = GameObject.FindGameObjectsWithTag("Tree");

        foreach (var tree in trees)
        {
            tree.SetActive(false);
        }

        //remove a água
        var waters = GameObject.FindGameObjectsWithTag("Water");

        foreach (var water in waters)
        {
            water.SetActive(false);
        }
    }
}