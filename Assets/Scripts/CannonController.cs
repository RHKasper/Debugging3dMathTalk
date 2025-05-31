using UnityEngine;

public class CannonController : MonoBehaviour
{
    public const float MinBarrelPitch = -89;
    public const float MaxBarrelPitch = 15;
    public const float MinPlatformYaw = -110;
    public const float MaxPlatformYaw = 110;
    
    
    [Header("Settings")]
    [Range(-89, 15)]
    public float desiredBarrelPitch = 0f;
    [Range(-110, 110)]
    public float desiredPlatformYaw = 0f;
    
    public float muzzleSpeed = 20;
    public float aimingDuration = 1f;
    public float barrelPitchSpeed = 100f;
    public float platformYawSpeed = 100f;
    
    [Header("External Refs")]
    [SerializeField] private Rigidbody cannonBallPrefab;
    
    [Header("Internal Refs")]
    [SerializeField] private Transform barrelSwivelTransform;
    [SerializeField] private Transform cannonBallSpawnTransform;
    
    private float _latestDesiredBarrelPitch;
    private float _latestDesiredPlatformYaw;
    private float _elapsedAimTime;
    
    bool DesiredPitchChangedThisFrame => Mathf.Abs(desiredBarrelPitch - _latestDesiredBarrelPitch) > 0.01f;
    private bool DesiredYawChangedThisFrame => Mathf.Abs(desiredPlatformYaw - _latestDesiredPlatformYaw) > .01f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetBarrelPitch(desiredBarrelPitch);
        SetCannonYaw(desiredPlatformYaw);
        _latestDesiredBarrelPitch = desiredBarrelPitch;
        _latestDesiredPlatformYaw = desiredPlatformYaw;
    }

    // Update is called once per frame
    void Update()
    {
        if (DesiredPitchChangedThisFrame || DesiredYawChangedThisFrame)
        {
            _latestDesiredBarrelPitch = desiredBarrelPitch;
            _latestDesiredPlatformYaw = desiredPlatformYaw;
        }

        
        SetBarrelPitch(_latestDesiredBarrelPitch);
        SetCannonYaw(_latestDesiredPlatformYaw);

        if (_elapsedAimTime >= aimingDuration)
        {
            Fire();
            _elapsedAimTime = 0;
        }
        else
        {
            _elapsedAimTime += Time.deltaTime;
        }
    }
    
    private void SetBarrelPitch(float pitch) => barrelSwivelTransform.localRotation = Quaternion.Euler(new Vector3(pitch, 0, 90));
    private void SetCannonYaw(float yaw) => transform.localRotation = Quaternion.Euler(new Vector3(0, yaw, 0));

    private void Fire()
    {
        var cannonBall = Instantiate(cannonBallPrefab);
        cannonBall.transform.SetPositionAndRotation(cannonBallSpawnTransform.position, cannonBallSpawnTransform.rotation);
        cannonBall.linearVelocity = cannonBallSpawnTransform.forward * muzzleSpeed;
    }

    public Transform GetCannonBallSpawnTransform() => cannonBallSpawnTransform;
    
    public void AimAndFire(float pitch, float yaw, float launchSpeed)
    {
        desiredBarrelPitch = pitch;
        desiredPlatformYaw = yaw;
        muzzleSpeed = launchSpeed;
    }
}
