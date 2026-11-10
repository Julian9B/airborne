using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAirshipManager : MonoBehaviour
{
    [Header("Controls")]
    [SerializeField] private float throttlingSpeed = 0.5f;
    [SerializeField] private float repairingSpeed = 0.03f;
    [SerializeField] private float releasingSpeed = 0.01f;
    [SerializeField] private float pullingSpeed = 2f;

    [Header("Buoyancy")]
    [SerializeField] private float gasEffect = 100f;
    [SerializeField] private float ballastEffect = 100f;
    [SerializeField] private float buoyancyForce = 0.001f;

    [Header("Turning")]
    [SerializeField] private float pitchForce = 1f;
    [SerializeField] private float yawForce = 1f;
    [SerializeField] private float rollForce = 2f;

    [Header("Wind")]
    [SerializeField] private WindZone windZone;
    [SerializeField] private float windForce = 1f;

    [Header("Throttling")]
    [SerializeField] private float altitudeEfficiencyFactor = 1f;
    [SerializeField] private float thrustForce = 1f;

    [Header("Damage")]
    [SerializeField] private List<string> excludedColliders = new List<string>();
    [SerializeField] private float collisionDamageForce = 0.5f;
    [SerializeField] private float excessAngleDamageForce = 0.001f;
    [SerializeField] private int maxAngle = 20;

    [HideInInspector] public Vector3 mooringPosition;
    [HideInInspector] public Vector3 cableStartPosition;

    public float Buoyancy { get; private set; } = 0;
    public float Throttle { get; private set; } = 0;
    public int TargetThrottle { get; private set; } = 0;
    public float Airspeed { get; private set; } = 0;
    public float Altitude { get; private set; }
    public float Gas { get; private set; } = 1;
    public float Ballast { get; private set; } = 1;
    public float Health { get; private set; } = 1;
    public float Fuel { get; private set; } = 1;
    public bool CableOut { get; private set; } = false;
    public bool Captured { get; private set; } = false;
    public bool Docked { get; private set; } = false;

    public bool NavLightsOn { get; private set; } = false;
    public bool MainLightOn { get; private set; } = false;

    private Rigidbody rb;
    private bool repairing = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (GameManager.instance.InFlight == true)
        {
            ApplyBuoyancy();
            ClampAltitude();
            HandleFuelConsumption();
            ApplyThrust();
            ApplyWind();
        }
    }

    private void Update()
    {
        if(GameManager.instance.InFlight == true) {

            Airspeed = transform.InverseTransformDirection(rb.linearVelocity).z;

            // running out of fuel

            if (Fuel <= 0)
            {
                SetTargetThrottle(0);
                Fuel = 0;
                MessagesManager.instance.CreateMessage("You are out of fuel.");
            }

            // throttling

            Throttle = Mathf.MoveTowards(Throttle, TargetThrottle, throttlingSpeed * CrewManager.instance.GetAssigned("engines") * Time.deltaTime);

            // health

            if (repairing == true)
            {
                Health = Mathf.MoveTowards(Health, 1, repairingSpeed * CrewManager.instance.GetAssigned("repairs") * Time.deltaTime);
            }

            if (Health >= 1 && repairing == true)
            {
                repairing = false;
            }

            float normalizedX = transform.eulerAngles.x > 180 ? 360 - transform.eulerAngles.x : transform.eulerAngles.x;
            float normalizedZ = transform.eulerAngles.z > 180 ? 360 - transform.eulerAngles.z : transform.eulerAngles.z;
            float excessAngle = 0;

            if (normalizedX > maxAngle)
            {
                excessAngle = normalizedX - maxAngle;
            }

            if (normalizedZ > maxAngle)
            {
                excessAngle = normalizedZ - maxAngle;
            }

            TakeDamage(excessAngle * excessAngleDamageForce);
        }
    }

    // potrzebne?
    private void OnTriggerEnter(Collider other)
    {
        if (!excludedColliders.Contains(other.name))
        {
            Health = Mathf.MoveTowards(Health, 0, rb.linearVelocity.magnitude * collisionDamageForce);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!excludedColliders.Contains(other.name))
        {
            TakeDamage(rb.linearVelocity.magnitude * collisionDamageForce);
        }
    }

    private void ApplyBuoyancy()
    {
        Buoyancy = (Gas * gasEffect) - (Ballast * ballastEffect);

        float altitude = transform.position.y;
        float altitudeFactor = Mathf.Clamp01(1f - (altitude / GameManager.instance.airship.ceiling));

        Vector3 upwardForce = Vector3.up * Buoyancy * buoyancyForce * altitudeFactor;

        rb.AddForce(upwardForce, ForceMode.Acceleration);
    }

    private void ClampAltitude()
    {
        if (transform.position.y > GameManager.instance.airship.ceiling)
        {
            Vector3 pos = transform.position;
            pos.y = GameManager.instance.airship.ceiling;
            transform.position = pos;

            if (rb.linearVelocity.y > 0)
            {
                Vector3 vel = rb.linearVelocity;
                vel.y = 0;
                rb.linearVelocity = vel;
            }
        }

        Altitude = transform.position.y;
    }

    private void HandleFuelConsumption()
    {
        if (Fuel <= 0f)
        {
            return;
        }

        float altitude = transform.position.y;
        float altitudeModifier = Mathf.Clamp01(1f - altitude * altitudeEfficiencyFactor);
        float baseConsumptionPerSecond = (1f / (GameManager.instance.airship.range * 1000f / GameManager.instance.airship.maxSpeed));
        float thrustFactor = Mathf.Abs(Throttle);

        float fuelConsumption = baseConsumptionPerSecond * thrustFactor * thrustFactor * altitudeModifier;
        Fuel = Mathf.Max(0f, Fuel - fuelConsumption * Time.fixedDeltaTime);
    }

    private void ApplyThrust()
    {
        float targetSpeed = (Throttle / 3f) * GameManager.instance.airship.maxSpeed;
        rb.AddForce(transform.forward * targetSpeed * thrustForce, ForceMode.Force);
    }

    private void ApplyWind()
    {
        Vector3 windDirection = windZone.transform.forward;
        Vector3 wind = windDirection * windZone.windMain;
        rb.AddForce(wind * windForce, ForceMode.Force);
    }

    private void Refill()
    {
        TargetThrottle = 0;
        Throttle = 0;
        Gas = 1;
        Ballast = 1;
        Fuel = 1;
    }

    private void TakeDamage(float damage)
    {
        Health = Mathf.MoveTowards(Health, 0, damage * 0.01f);
    }

    public void Reset()
    {
        Refill();

        Health = 1;
        CableOut = false;
        Captured = false;
        Docked = false;

        rb.linearVelocity = new Vector3(0f, 0f, 0f);
        transform.eulerAngles = new Vector3(0f, 0f, 0f);
    }

    public void SetTargetThrottle(int targetThrottle)
    {
        if (GameManager.instance.InFlight && Docked == false)
        {
            if (Fuel > 0 || targetThrottle == 0)
            {
                if (targetThrottle >= -3 && targetThrottle <= 3)
                {
                    TargetThrottle = targetThrottle;
                }
            }
            else
            {
                MessagesManager.instance.CreateMessage("You can't set the throttle while out of fuel.");
            }

            if (CrewManager.instance.GetAssigned("engines") == 0)
            {
                MessagesManager.instance.CreateMessage("No crew assigned to engines.");
            }
        }
    }

    public void Repair()
    {
        if (Health >= 1)
        {
            MessagesManager.instance.CreateMessage("No need for repairs.");
            return;
        }

        MessagesManager.instance.CreateMessage("Repairs are underway.");
        repairing = true;

        if (CrewManager.instance.GetAssigned("repairs") == 0)
        {
            MessagesManager.instance.CreateMessage("No crew assigned to repairs.");
        }
    }

    public void ReleaseGas()
    {
        if (Gas > 0 && Docked == false)
        {
            Gas = Mathf.MoveTowards(Gas, 0, releasingSpeed * CrewManager.instance.GetAssigned("buoyancy") * Time.deltaTime);
        }
    }

    public void ReleaseBallast()
    {
        if (Ballast > 0 && Docked == false)
        {
            Ballast = Mathf.MoveTowards(Ballast, 0, releasingSpeed * CrewManager.instance.GetAssigned("buoyancy") * Time.deltaTime);
        }
    }

    public void Yaw(int direction)
    {
        transform.Rotate(0, direction * Airspeed * yawForce * CrewManager.instance.GetAssigned("steerage") * Time.deltaTime, 0);
    }

    public void Roll(int direction)
    {
        transform.Rotate(0, 0, direction * Airspeed * rollForce * CrewManager.instance.GetAssigned("steerage") * Time.deltaTime);
    }

    public void Pitch(int direction)
    {
        transform.Rotate(direction * Airspeed * pitchForce * CrewManager.instance.GetAssigned("steerage") * Time.deltaTime, 0, 0);
    }

    public void ToggleMooringCable()
    {
        string message;

        if (CableOut == true)
        {
            if (Docked == true)
            {
                Undock();
            }

            if (Captured == true)
            {
                Uncapture();
            }

            message = "Pulling the mooring cable in.";
        }
        else
        {
            message = "Releasing the mooring cable.";
        }

        MessagesManager.instance.CreateMessage(message);

        CableOut = !CableOut;

        if (CrewManager.instance.GetAssigned("mooring") == 0)
        {
            MessagesManager.instance.CreateMessage("No crew assigned to the mooring.");
        }
    }

    public void Capture()
    {
        Captured = true;
        MessagesManager.instance.CreateMessage("Mooring cable captured.", "Hold P to pull to the mast or press C to uncapture.");
    }

    public void PullMooringCable()
    {
        Vector3 direction = (mooringPosition - cableStartPosition).normalized;
        Vector3 force = direction * pullingSpeed * CrewManager.instance.GetAssigned("mooring");

        rb.AddForceAtPosition(force, cableStartPosition, ForceMode.Force);
    }

    public void Dock()
    {
        Docked = true;
        Refill();

        MessagesManager.instance.CreateMessage("Docked to the mast.", "Press C to undock.");
    }

    public void Undock()
    {
        Docked = false;
        Captured = false;
        MessagesManager.instance.CreateMessage("Undocked.");
    }

    public void Uncapture()
    {
        Captured = false;
        MessagesManager.instance.CreateMessage("Uncaptured.");
    }

    public void ToggleNavLights()
    {
        NavLightsOn = !NavLightsOn;
    }

    public void ToggleMainLight()
    {
        MainLightOn = !MainLightOn;
    }
}

