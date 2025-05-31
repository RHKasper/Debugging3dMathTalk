using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CannonAimer : MonoBehaviour
{
    [SerializeField] private new Camera camera;
    [SerializeField] private CannonController cannon;
    [SerializeField] private Transform target;
    [SerializeField] private bool aimAtMouse = true;
    [SerializeField] private bool drawTrajectory = false;
    
    private LayerMask _layerMask;

    private void Start()
    {
        _layerMask = LayerMask.GetMask("Terrain");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPosition;
        if (aimAtMouse)
        {
            Physics.Raycast(camera.ScreenPointToRay(Mouse.current.position.ReadValue()), out var hit, 1000, _layerMask);
            targetPosition = hit.point;
        }
        else
        {
            targetPosition = target.position;
        }

        // calculate yaw
        float yaw = Quaternion.LookRotation(targetPosition - cannon.transform.position, Vector3.up).eulerAngles.y;
        
        var launchData = CalculateLaunchData(cannon.GetCannonBallSpawnTransform().position, targetPosition);
        var pitch = Quaternion.LookRotation(launchData.initialVelocity, Vector3.up).eulerAngles.x;
        cannon.AimAndFire(pitch, yaw, launchData.initialVelocity.magnitude);
    }

    private Balistics.LaunchData CalculateLaunchData(Vector3 startPosition, Vector3 endPosition)
    {
        var launchData = Balistics.CalculateLaunchData(endPosition, startPosition, -9.81f, drawTrajectory);
        return launchData;
    }
}
