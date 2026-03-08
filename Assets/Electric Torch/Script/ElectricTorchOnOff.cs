using UnityEngine;

public class ElectricTorchOnOff : MonoBehaviour
{
    EmissionMaterialGlassTorchFadeOut _emissionMaterialFade;
    BatteryPowerPickup _batteryPower;

    public enum LightChoose
    {
        noBattery,
        withBattery
    }

    public LightChoose modoLightChoose;

    public bool _PowerPickUp = false;

    public float intensityLight = 2.5f;

    private bool _flashLightOn = false;

    [SerializeField] float _lightTime = 0.05f;

    private Light torchLight;

    void Awake()
    {
        _batteryPower = FindFirstObjectByType<BatteryPowerPickup>();
        torchLight = GetComponent<Light>();
    }

    void Start()
    {
        GameObject _scriptControllerEmissionFade = GameObject.Find("default");

        if (_scriptControllerEmissionFade != null)
        {
            _emissionMaterialFade = _scriptControllerEmissionFade.GetComponent<EmissionMaterialGlassTorchFadeOut>();
        }
        else
        {
            Debug.Log("Cannot find 'EmissionMaterialGlassTorchFadeOut' script");
        }
    }

    void Update()
    {
        switch (modoLightChoose)
        {
            case LightChoose.noBattery:
                NoBatteryLight();
                break;

            case LightChoose.withBattery:
                WithBatteryLight();
                break;
        }
    }

    // XR WILL CALL THIS FUNCTION
    public void ToggleTorch()
    {
        _flashLightOn = !_flashLightOn;
    }

    void NoBatteryLight()
    {
        if (_flashLightOn)
        {
            torchLight.intensity = intensityLight;
            _emissionMaterialFade.OnEmission();
        }
        else
        {
            torchLight.intensity = 0f;
            _emissionMaterialFade.OffEmission();
        }
    }

    void WithBatteryLight()
    {
        if (_flashLightOn)
        {
            torchLight.intensity = intensityLight;

            intensityLight -= Time.deltaTime * _lightTime;

            _emissionMaterialFade.TimeEmission(_lightTime);

            if (intensityLight < 0)
                intensityLight = 0;

            if (_PowerPickUp)
                intensityLight = _batteryPower.PowerIntensityLight;
        }
        else
        {
            torchLight.intensity = 0f;
            _emissionMaterialFade.OffEmission();

            if (_PowerPickUp)
                intensityLight = _batteryPower.PowerIntensityLight;
        }
    }
}