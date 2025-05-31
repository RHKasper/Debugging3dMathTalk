using UnityEngine;

public class CannonController : MonoBehaviour
{
    [Header("Settings")]
    [Range(-89, 15)]
    public float desiredBarrelPitch = 0f;
    [Range(-110, 110)]
    public float desiredPlatformYaw = 0f;
    public float muzzleSpeed = 20;
    public float aimingDuration = 1.5f;
    public float fireDelay = 1f;
    
    [Header("External Refs")]
    [SerializeField] private Rigidbody cannonBallPrefab;
    
    [Header("Internal Refs")]
    [SerializeField] private Transform barrelSwivelTransform;
    [SerializeField] private Transform cannonBallSpawnTransform;
    
    private bool _waitingToShoot;
    private float _latestDesiredBarrelPitch;
    private float _latestDesiredPlatformYaw;
    private float _latestStartBarrelPitch;
    private float _latestStartPlatformYaw;
    private float _elapsedAimTime;
    
    bool DesiredPitchChangedThisFrame
    {
        get
        {
            var delta = Mathf.Abs(desiredBarrelPitch - _latestDesiredBarrelPitch);
            if (delta > 0.01f)
            {
                Debug.Log("Pitch delta: " + delta);
                return true;
            }

            return false;
        }
    }

    bool DesiredYawChangedThisFrame
    {
        get
        {
            var delta = Mathf.Abs(desiredPlatformYaw - _latestDesiredPlatformYaw);
            if (delta > .01f)
            {
                Debug.Log("Yaw delta: " + delta);
                return true;
            }

            return false;
        }
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetBarrelPitch(desiredBarrelPitch);
        SetCannonYaw(desiredPlatformYaw);
        _latestDesiredBarrelPitch = desiredBarrelPitch;
        _latestDesiredPlatformYaw = desiredPlatformYaw;
        _latestStartBarrelPitch = desiredBarrelPitch;
        _latestStartPlatformYaw = desiredPlatformYaw;
    }

    // Update is called once per frame
    void Update()
    {
        if (DesiredPitchChangedThisFrame || DesiredYawChangedThisFrame)
        {
            _waitingToShoot = true;
            
            _latestDesiredBarrelPitch = desiredBarrelPitch;
            _latestDesiredPlatformYaw = desiredPlatformYaw;
            _latestStartBarrelPitch = barrelSwivelTransform.localEulerAngles.x;
            _latestStartPlatformYaw = transform.localEulerAngles.y;
            _elapsedAimTime = 0;
        }

        if (_waitingToShoot)
        {
            SetBarrelPitch(Mathf.LerpAngle(_latestStartBarrelPitch, _latestDesiredBarrelPitch, Mathf.Clamp01(_elapsedAimTime / aimingDuration)));
            SetCannonYaw(Mathf.LerpAngle(_latestStartPlatformYaw, _latestDesiredPlatformYaw, Mathf.Clamp01(_elapsedAimTime / aimingDuration)));

            if (_elapsedAimTime >= aimingDuration + fireDelay)
            {
                Fire();
                _waitingToShoot = false;
            }
        }
        
        _elapsedAimTime += Time.deltaTime;
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
    
    public void AimAndFire(float pitch, float yaw)
    {
        desiredBarrelPitch = pitch;
        desiredPlatformYaw = yaw;
    }
}
