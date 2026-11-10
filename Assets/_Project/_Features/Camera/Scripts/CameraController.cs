using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private PlayerInput input;
    [SerializeField] private GameObject playerAirship;
    [SerializeField] private UIManager ui;

    [Header("Cameras")]
    [SerializeField] private CinemachineVirtualCamera airshipCamera;
    [SerializeField] private CinemachineVirtualCamera freeCamera;

    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 0.25f;
    [SerializeField] private float minPitch = -89f;
    [SerializeField] private float maxPitch = 89f;
    [SerializeField] private Vector3 startingRotation = new Vector3(-8.5f, 138f, 0f);

    [Header("Zoom")]
    [SerializeField] private float zoomSpeed = 0.5f;
    [SerializeField] private float minZoom = 2f;
    [SerializeField] private float maxZoom = 20f;
    [SerializeField] private float startingZoom = 5f;

    [Header("FOV")]
    [SerializeField] private float minFOV = 20f;
    [SerializeField] private float maxFOV = 80f;
    [SerializeField] private float defaultFOV = 50f;

    [Header("Free camera")]
    [SerializeField] private float startingMovingSpeed = 0.1f;
    [SerializeField] private float minMovingSpeed = 0.01f;
    [SerializeField] private float maxMovingSpeed = 5f;
    [SerializeField] private float minY = 1f;
    [SerializeField] private float maxY = 50f;
    [SerializeField] private float changingSpeed = 0.01f;
    [SerializeField] private float initialOffset = 5f;

    private float yaw;
    private float pitch;
    private float movingSpeed;

    private Cinemachine3rdPersonFollow thirdPersonFollow;

    private void Awake()
    {
        transform.Rotate(startingRotation);
        yaw = startingRotation.y;
        pitch = startingRotation.x;

        movingSpeed = startingMovingSpeed;

        thirdPersonFollow = airshipCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
        thirdPersonFollow.CameraDistance = startingZoom;

        airshipCamera.m_Lens.FieldOfView = defaultFOV;
    }

    private void Update()
    {
        if (GameManager.instance.InFlight && !GameManager.instance.FreeCam)
            SetPosition(playerAirship.transform.position);
    }

    // -----------------------------
    // ROTATION (called by InputRouter)
    // -----------------------------

    public void Rotate(Vector2 lookDelta)
    {
        yaw += lookDelta.x * rotationSpeed;
        pitch -= lookDelta.y * rotationSpeed;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
    }

    // -----------------------------
    // ZOOM (called by InputRouter)
    // -----------------------------

    public void Zoom(float scrollDelta)
    {
        if (GameManager.instance.FreeCam)
        {
            movingSpeed += scrollDelta * changingSpeed;
            movingSpeed = Mathf.Clamp(movingSpeed, minMovingSpeed, maxMovingSpeed);
            return;
        }

        if (ui.activePopup == null)
            thirdPersonFollow.CameraDistance -= scrollDelta * zoomSpeed;

        thirdPersonFollow.CameraDistance =
            Mathf.Clamp(thirdPersonFollow.CameraDistance, minZoom, maxZoom);
    }

    // -----------------------------
    // FREE CAM MOVEMENT (called by InputRouter)
    // -----------------------------

    public void MoveForward(int direction)
    {
        transform.Translate(Vector3.forward * direction * movingSpeed);
        ClampVerticalPosition();
    }

    public void MoveLeft(int direction)
    {
        transform.Translate(Vector3.right * direction * movingSpeed);
    }

    public void MoveUp(int direction)
    {
        float newY = transform.position.y + direction * movingSpeed;
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(newY, minY, maxY), transform.position.z);
    }

    private void ClampVerticalPosition()
    {
        transform.position = new Vector3(
            transform.position.x,
            Mathf.Clamp(transform.position.y, minY, maxY),
            transform.position.z
        );
    }

    // -----------------------------
    // FREE CAM TOGGLE
    // -----------------------------

    public void EnterFreeCam()
    {
        SwapCameras();

        Vector3 newPosition = new Vector3(
            playerAirship.transform.position.x,
            playerAirship.transform.position.y,
            playerAirship.transform.position.z - initialOffset
        );

        SetPosition(newPosition);

        yaw = 0f;
        pitch = 0f;
        transform.eulerAngles = Vector3.zero;

        MessagesManager.instance.CreateMessage("Free Camera ON.");
    }

    public void ExitFreeCam()
    {
        SwapCameras();
        MessagesManager.instance.CreateMessage("Free Camera OFF.");
    }

    private void SwapCameras()
    {
        airshipCamera.gameObject.SetActive(!airshipCamera.gameObject.activeSelf);
        freeCamera.gameObject.SetActive(!freeCamera.gameObject.activeSelf);
    }

    // -----------------------------
    // POSITIONING
    // -----------------------------

    public void SetPosition(Vector3 position)
    {
        transform.position = new Vector3(position.x, Mathf.Clamp(position.y, minY, maxY), position.z);
    }

    public void SetFOV(float fov)
    {
        airshipCamera.m_Lens.FieldOfView = Mathf.Lerp(minFOV, maxFOV, fov);
        freeCamera.m_Lens.FieldOfView = Mathf.Lerp(minFOV, maxFOV, fov);
    }
}
