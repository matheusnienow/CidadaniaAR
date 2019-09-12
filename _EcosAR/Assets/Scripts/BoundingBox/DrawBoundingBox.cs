using UnityEngine;
using UnityEngine.UI;

public class DrawBoundingBox : MonoBehaviour
{
    public Material material;
    public GameObject HelpButton;

    private Vector3[] _terrainVertices;
    private Vector3[] _windKnobVertices;
    private Vector3[] _temperatureKnobVertices;

    private GameObject _terrain;
    private GameObject _windKnob;
    private GameObject _temperatureKnob;

    private bool _isBeingTrackedWindTarget;
    private bool _isBeingTrackedSceneTarget;
    private bool _isBeingTrackedTemperatureTarget;
    private bool _showBoundingBox;

    private void Start()
    {
        _showBoundingBox = false;

        _isBeingTrackedWindTarget = VuforiaTools.IsBeingTracked("Wind Target");
        _isBeingTrackedSceneTarget = VuforiaTools.IsBeingTracked("Scene Target");
        _isBeingTrackedTemperatureTarget = VuforiaTools.IsBeingTracked("Temperature Target");

        _terrain = GameObject.Find("Terrain");
        _windKnob = GameObject.Find("Wind Knob");
        _temperatureKnob = GameObject.Find("Temperature Knob");

        var button = HelpButton.GetComponent<Button>();

        button.onClick.AddListener(HelpButtonClicked);
    }

    private void Update()
    {
        _isBeingTrackedWindTarget = VuforiaTools.IsBeingTracked("Wind Target");
        _isBeingTrackedSceneTarget = VuforiaTools.IsBeingTracked("Scene Target");
        _isBeingTrackedTemperatureTarget = VuforiaTools.IsBeingTracked("Temperature Target");
    }

    private void OnPostRender()
    {
        if (_showBoundingBox)
        {
            DrawSceneTargetBoundingBox();
            DrawWindTargetBoundingBox();
            DrawTemperatureTargetBoundingBox();
        }
    }

    void DrawSceneTargetBoundingBox()
    {
        if (_isBeingTrackedSceneTarget)
        {
            _terrainVertices = _terrain.GetComponent<BoundingBoxGenerator>().Vertices;

            material.SetPass(0);
            GL.PushMatrix();
            GL.Begin(GL.LINE_STRIP);
            {
                GL.Vertex3(_terrainVertices[0].x, _terrainVertices[0].y, _terrainVertices[0].z);
                GL.Vertex3(_terrainVertices[1].x, _terrainVertices[1].y, _terrainVertices[1].z);
                GL.Vertex3(_terrainVertices[2].x, _terrainVertices[2].y, _terrainVertices[2].z);
                GL.Vertex3(_terrainVertices[3].x, _terrainVertices[3].y, _terrainVertices[3].z);
            }
            {
                GL.Vertex3(_terrainVertices[0].x, _terrainVertices[0].y, _terrainVertices[0].z);
                GL.Vertex3(_terrainVertices[4].x, _terrainVertices[4].y, _terrainVertices[4].z);
                GL.Vertex3(_terrainVertices[5].x, _terrainVertices[5].y, _terrainVertices[5].z);
                GL.Vertex3(_terrainVertices[1].x, _terrainVertices[1].y, _terrainVertices[1].z);
            }
            {
                GL.Vertex3(_terrainVertices[2].x, _terrainVertices[2].y, _terrainVertices[2].z);
                GL.Vertex3(_terrainVertices[6].x, _terrainVertices[6].y, _terrainVertices[6].z);
                GL.Vertex3(_terrainVertices[7].x, _terrainVertices[7].y, _terrainVertices[7].z);
                GL.Vertex3(_terrainVertices[3].x, _terrainVertices[3].y, _terrainVertices[3].z);
            }
            GL.End();
            GL.Begin(GL.LINE_STRIP);
            {
                GL.Vertex3(_terrainVertices[5].x, _terrainVertices[5].y, _terrainVertices[5].z);
                GL.Vertex3(_terrainVertices[6].x, _terrainVertices[6].y, _terrainVertices[6].z);
            }
            GL.End();
            GL.Begin(GL.LINE_STRIP);
            {
                GL.Vertex3(_terrainVertices[4].x, _terrainVertices[4].y, _terrainVertices[4].z);
                GL.Vertex3(_terrainVertices[7].x, _terrainVertices[7].y, _terrainVertices[7].z);
            }
            GL.End();
            GL.PopMatrix();
        }
    }

    void DrawTemperatureTargetBoundingBox()
    {
        if (_isBeingTrackedTemperatureTarget)
        {
            _temperatureKnobVertices = _temperatureKnob.GetComponent<BoundingBoxGenerator>().Vertices;

            material.SetPass(0);

            GL.PushMatrix();
            GL.Begin(GL.LINE_STRIP);
            {
                GL.Vertex3(_temperatureKnobVertices[0].x, _temperatureKnobVertices[0].y, _temperatureKnobVertices[0].z);
                GL.Vertex3(_temperatureKnobVertices[1].x, _temperatureKnobVertices[1].y, _temperatureKnobVertices[1].z);
                GL.Vertex3(_temperatureKnobVertices[2].x, _temperatureKnobVertices[2].y, _temperatureKnobVertices[2].z);
                GL.Vertex3(_temperatureKnobVertices[3].x, _temperatureKnobVertices[3].y, _temperatureKnobVertices[3].z);
            }
            {
                GL.Vertex3(_temperatureKnobVertices[0].x, _temperatureKnobVertices[0].y, _temperatureKnobVertices[0].z);
                GL.Vertex3(_temperatureKnobVertices[4].x, _temperatureKnobVertices[4].y, _temperatureKnobVertices[4].z);
                GL.Vertex3(_temperatureKnobVertices[5].x, _temperatureKnobVertices[5].y, _temperatureKnobVertices[5].z);
                GL.Vertex3(_temperatureKnobVertices[1].x, _temperatureKnobVertices[1].y, _temperatureKnobVertices[1].z);
            }
            {
                GL.Vertex3(_temperatureKnobVertices[2].x, _temperatureKnobVertices[2].y, _temperatureKnobVertices[2].z);
                GL.Vertex3(_temperatureKnobVertices[6].x, _temperatureKnobVertices[6].y, _temperatureKnobVertices[6].z);
                GL.Vertex3(_temperatureKnobVertices[7].x, _temperatureKnobVertices[7].y, _temperatureKnobVertices[7].z);
                GL.Vertex3(_temperatureKnobVertices[3].x, _temperatureKnobVertices[3].y, _temperatureKnobVertices[3].z);
            }
            GL.End();
            GL.Begin(GL.LINE_STRIP);
            {
                GL.Vertex3(_temperatureKnobVertices[5].x, _temperatureKnobVertices[5].y, _temperatureKnobVertices[5].z);
                GL.Vertex3(_temperatureKnobVertices[6].x, _temperatureKnobVertices[6].y, _temperatureKnobVertices[6].z);
            }
            GL.End();
            GL.Begin(GL.LINE_STRIP);
            {
                GL.Vertex3(_temperatureKnobVertices[4].x, _temperatureKnobVertices[4].y, _temperatureKnobVertices[4].z);
                GL.Vertex3(_temperatureKnobVertices[7].x, _temperatureKnobVertices[7].y, _temperatureKnobVertices[7].z);
            }
            GL.End();
            GL.PopMatrix();
        }
    }

    void DrawWindTargetBoundingBox()
    {
        if (_isBeingTrackedWindTarget)
        {
            _windKnobVertices = _windKnob.GetComponent<BoundingBoxGenerator>().Vertices;

            material.SetPass(0);

            GL.PushMatrix();
            GL.Begin(GL.LINE_STRIP);
            {
                GL.Vertex3(_windKnobVertices[0].x, _windKnobVertices[0].y, _windKnobVertices[0].z);
                GL.Vertex3(_windKnobVertices[1].x, _windKnobVertices[1].y, _windKnobVertices[1].z);
                GL.Vertex3(_windKnobVertices[2].x, _windKnobVertices[2].y, _windKnobVertices[2].z);
                GL.Vertex3(_windKnobVertices[3].x, _windKnobVertices[3].y, _windKnobVertices[3].z);
            }
            {
                GL.Vertex3(_windKnobVertices[0].x, _windKnobVertices[0].y, _windKnobVertices[0].z);
                GL.Vertex3(_windKnobVertices[4].x, _windKnobVertices[4].y, _windKnobVertices[4].z);
                GL.Vertex3(_windKnobVertices[5].x, _windKnobVertices[5].y, _windKnobVertices[5].z);
                GL.Vertex3(_windKnobVertices[1].x, _windKnobVertices[1].y, _windKnobVertices[1].z);
            }
            {
                GL.Vertex3(_windKnobVertices[2].x, _windKnobVertices[2].y, _windKnobVertices[2].z);
                GL.Vertex3(_windKnobVertices[6].x, _windKnobVertices[6].y, _windKnobVertices[6].z);
                GL.Vertex3(_windKnobVertices[7].x, _windKnobVertices[7].y, _windKnobVertices[7].z);
                GL.Vertex3(_windKnobVertices[3].x, _windKnobVertices[3].y, _windKnobVertices[3].z);
            }
            GL.End();
            GL.Begin(GL.LINE_STRIP);
            {
                GL.Vertex3(_windKnobVertices[5].x, _windKnobVertices[5].y, _windKnobVertices[5].z);
                GL.Vertex3(_windKnobVertices[6].x, _windKnobVertices[6].y, _windKnobVertices[6].z);
            }
            GL.End();
            GL.Begin(GL.LINE_STRIP);
            {
                GL.Vertex3(_windKnobVertices[4].x, _windKnobVertices[4].y, _windKnobVertices[4].z);
                GL.Vertex3(_windKnobVertices[7].x, _windKnobVertices[7].y, _windKnobVertices[7].z);
            }
            GL.End();
            GL.PopMatrix();
        }
    }

    public void HelpButtonClicked()
    {
        _showBoundingBox = !_showBoundingBox;
    }
}