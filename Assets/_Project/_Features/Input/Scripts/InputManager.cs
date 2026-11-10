using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputManager : MonoBehaviour
{
    private PlayerInput input;

    [SerializeField] private GameManager game;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private PlayerAirshipManager airship;

    private void Awake()
    {
        input = GetComponent<PlayerInput>();

        input.actions.FindActionMap("GameControls").Enable();
        input.actions.FindActionMap("CameraControls").Enable();
        input.actions.FindActionMap("AirshipControls").Enable();
    }

    private void Update()
    {
        HandleGameInput();
        HandleCameraInput();
        HandleAirshipInput();
    }

    // -----------------------------
    // GAME MAP
    // -----------------------------

    private void HandleGameInput()
    {
        if (input.actions["Pause"].WasPerformedThisFrame())
            game.TogglePause();

        if (input.actions["ToggleUI"].WasPerformedThisFrame())
            game.ToggleUi();

        if (input.actions["ToggleFreeCam"].WasPerformedThisFrame())
            game.ToggleFreeCam();
    }

    // -----------------------------
    // CAMERA MAP
    // -----------------------------

    private void HandleCameraInput()
    {
        // Rotation
        if (input.actions["Hold"].IsPressed())
        {
            Vector2 look = input.actions["Look"].ReadValue<Vector2>();
            cameraController.Rotate(look);
        }

        // Zoom
        float scroll = input.actions["Zoom"].ReadValue<float>();
        if (scroll != 0)
            cameraController.Zoom(scroll);

        // FreeCam movement (delegated to GameManager)
        if (game.FreeCam && input.actions["FreeCamModifier"].IsPressed())
        {
            Vector2 move = input.actions["FreeCamMove"].ReadValue<Vector2>();
            float moveUp = input.actions["FreeCamMoveUpDown"].ReadValue<float>();

            if (move.y != 0)
                cameraController.MoveForward((int)move.y);

            if (move.x != 0)
                cameraController.MoveLeft((int)move.x);

            if (moveUp != 0)
                cameraController.MoveUp((int)moveUp);
        }
    }

    // -----------------------------
    // AIRSHIP MAP
    // -----------------------------

    private void HandleAirshipInput()
    {
        if (!game.InFlight || game.mainVehicle != Vehicle.Airship || input.actions["FreeCamModifier"].IsPressed())
            return;

        if (input.actions["ThrottleUp"].WasPerformedThisFrame())
            airship.SetTargetThrottle(airship.TargetThrottle + 1);

        if (input.actions["ThrottleDown"].WasPerformedThisFrame())
            airship.SetTargetThrottle(airship.TargetThrottle - 1);

        if ((input.actions["Yaw"].WasPerformedThisFrame() || input.actions["Pitch"].WasPerformedThisFrame() || input.actions["Roll"].WasPerformedThisFrame()) && CrewManager.instance.GetAssigned("steerage") == 0)
            MessagesManager.instance.CreateMessage("No crew assigned to the steerage.");

        float yaw = input.actions["Yaw"].ReadValue<float>();
        if (yaw != 0)
            airship.Yaw((int)yaw);

        float pitch = input.actions["Pitch"].ReadValue<float>();
        if (pitch != 0)
            airship.Pitch((int)pitch);

        float roll = input.actions["Roll"].ReadValue<float>();
        if (roll != 0)
            airship.Roll((int)roll);

        if ((input.actions["Descend"].WasPerformedThisFrame() || input.actions["Ascend"].WasPerformedThisFrame()) && CrewManager.instance.GetAssigned("buoyancy") == 0)
            MessagesManager.instance.CreateMessage("No crew assigned to the buoyancy control.");

        if (input.actions["Descend"].IsPressed())
            airship.ReleaseGas();

        if (input.actions["Ascend"].IsPressed())
            airship.ReleaseBallast();

        if (input.actions["ToggleNavLights"].WasPerformedThisFrame())
            airship.ToggleNavLights();

        if (input.actions["ToggleMainLight"].WasPerformedThisFrame())
            airship.ToggleMainLight();

        if (input.actions["ToggleMooringCable"].WasPerformedThisFrame())
            airship.ToggleMooringCable();

        if (input.actions["PullMooringCable"].WasPerformedThisFrame() && CrewManager.instance.GetAssigned("mooring") == 0)
            MessagesManager.instance.CreateMessage("No crew assigned to the mooring.");

        if (input.actions["PullMooringCable"].IsPressed() || airship.Docked)
            airship.PullMooringCable();

        if (input.actions["Repair"].WasPerformedThisFrame())
            airship.Repair();
    }
}
