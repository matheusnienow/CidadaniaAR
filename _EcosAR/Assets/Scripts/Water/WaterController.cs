using UnityEngine;

public class WaterController {

    private GameObject _water;

    public WaterController() {
        _water = GameObject.FindGameObjectWithTag("Water");
    }

    public void Update(float temperature, bool isRaining, bool isCloudsInCorrectPosition) {
        Evaporate(temperature, isRaining);
        Condense(temperature, isRaining, isCloudsInCorrectPosition);
    }

    void Evaporate(float temperature, bool isRaining) {
        if (temperature >= 40f && (!isRaining)) {
            _water.GetComponent<Water>().Evaporate();
        }
    }

    void Condense(float temperature, bool isRaining, bool isCloudsInCorrectPosition) {
        if (temperature < 40f && (isRaining && isCloudsInCorrectPosition)) {
            _water.GetComponent<Water>().Condense();
        }
    }
}