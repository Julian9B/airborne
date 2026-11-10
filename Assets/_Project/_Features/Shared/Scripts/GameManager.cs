using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [HideInInspector] public Airship airship;
    [HideInInspector] public Location startingLocation;

    [HideInInspector] public bool InFlight { get; private set; } = false;
    [HideInInspector] public bool FreeCam { get; private set; } = false;

    [SerializeField] private UIManager ui;
    [SerializeField] private GameObject gameUi;
    [SerializeField] private GameObject crashScreen;
    [SerializeField] private GameObject pauseUi;
    [SerializeField] private PlayerAirshipManager PlayerAirship;
    [SerializeField] private CameraController cameraTarget;
    [SerializeField] private Airships airshipDatabase;

    [SerializeField] private Airship defaultAirship;
    [SerializeField] private bool alwaysDefault = false;

    public float worldScale = 0.1f;

    [HideInInspector] public Vehicle mainVehicle = Vehicle.Airship;

    private const string NAME_KEY = "shipName";

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    private void Update()
    {
        if (InFlight && PlayerAirship.Health <= 0)
            Crash();
    }

    // -----------------------------
    // GAME STATE
    // -----------------------------

    public void TogglePause()
    {
        pauseUi.SetActive(!pauseUi.activeSelf);
        Time.timeScale = pauseUi.activeSelf ? 0 : 1;
    }

    public void ToggleUi()
    {
        ui.gameObject.SetActive(!ui.gameObject.activeSelf);
    }

    public void ToggleFreeCam()
    {
        FreeCam = !FreeCam;

        if (FreeCam)
            cameraTarget.EnterFreeCam();
        else
            cameraTarget.ExitFreeCam();
    }

    public void StartFlight()
    {
        InFlight = true;
        PlayerAirship.Reset();
    }

    public void StopFlight()
    {
        if (!ui.gameObject.activeSelf)
            ToggleUi();

        if (Time.timeScale == 0)
            TogglePause();

        if (FreeCam)
            ToggleFreeCam();

        InFlight = false;
        PlayerAirship.Reset();
    }

    // -----------------------------
    // AIRSHIP LOADING
    // -----------------------------

    public void GetAirship()
    {
        string shipName = PlayerPrefs.GetString(NAME_KEY);
        Airship airshipToLoad;

        if (defaultAirship.name != shipName && shipName != "" && !alwaysDefault)
            airshipToLoad = airshipDatabase.airships.Where(a => a.shipName == shipName).First();
        else
            airshipToLoad = defaultAirship;

        LoadAirship(airshipToLoad);
    }

    public void LoadAirship(Airship selectedAirship)
    {
        foreach (Transform airship in PlayerAirship.transform)
            Destroy(airship.gameObject);

        airship = selectedAirship;
        CrewManager.instance.Init(airship.crewCapacity);

        Instantiate(airship.prefab, PlayerAirship.transform);

        if (PlayerPrefs.GetString(NAME_KEY) != airship.shipName)
            PlayerPrefs.SetString(NAME_KEY, selectedAirship.shipName);
    }

    public void LoadAirshipPosition()
    {
        PlayerAirship.transform.position = cameraTarget.transform.position;
    }

    // -----------------------------
    // CRASH
    // -----------------------------

    private void Crash()
    {
        StopFlight();
        ui.ToggleVisibility(gameUi);
        ui.ToggleVisibility(crashScreen);
    }
}
