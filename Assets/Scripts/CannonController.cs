using UnityEngine;

public class CannonController : MonoBehaviour
{
    [Header("Settings")]
    public float barrelPitch = 0f;
    public float platformYaw = 0f;
    public float muzzleSpeed = 20;
    
    [Header("External Refs")]
    [SerializeField] private Rigidbody cannonBallPrefab;
    
    [Header("Internal Refs")]
    [SerializeField] private Transform barrelSwivelTransform;
    
    private bool _waitingToShoot;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetBarrelPitch(barrelPitch);
        SetCannonYaw(platformYaw);
    }

    // Update is called once per frame
    void Update()
    {
        SetBarrelPitch(barrelPitch);
        SetCannonYaw(platformYaw);
    }
    
    private void SetBarrelPitch(float pitch) => barrelSwivelTransform.localEulerAngles = new Vector3(pitch, 0, 90);
    private void SetCannonYaw(float yaw) => transform.localEulerAngles = new Vector3(0, yaw, 0);
}
