using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CannonAimer : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool aimAtMouse = true;
    [SerializeField] private bool drawTrajectory = false;
    [SerializeField] private bool staticTarget = true;
    
    [Header("Refs")]
    [SerializeField] private new Camera camera;
    [SerializeField] private CannonController cannon;
    [SerializeField] private TargetMovementController target;
    
    private LayerMask _layerMask;

    private void Start()
    {
        _layerMask = LayerMask.GetMask("Terrain");
    }

    // Update is called once per frame
    void Update()
    {
        // set aim target
        Vector3 targetPosition;
        if (aimAtMouse)
        {
            Physics.Raycast(camera.ScreenPointToRay(Mouse.current.position.ReadValue()), out var hit, 1000, _layerMask);
            targetPosition = hit.point;
        }
        else
        {
            targetPosition = target.transform.position;
        }
        
        // calculate pitch, yaw, and launch speed
        (float pitch, float yaw, float launchSpeed) = CalculatePitchAndLaunchSpeed(cannon.GetCannonBallSpawnTransform().position, targetPosition);
        
        // fire cannon
        cannon.AimAndFire(pitch, yaw, launchSpeed);
    }

    private (float pitch, float yaw, float launchSpeed) CalculatePitchAndLaunchSpeed(Vector3 launchPosition, Vector3 endPosition)
    {
        Balistics.LaunchData launchData;
        float yaw;

        if (staticTarget)
        {
            launchData = CalculateLaunchData(launchPosition, endPosition);
            yaw = Quaternion.LookRotation(endPosition - cannon.transform.position, Vector3.up).eulerAngles.y;;
        }
        else
        {
            // lead the target
            Vector3 velocity = target.GetVelocity();
            Vector3 predictedEndPosition = endPosition + velocity;
            Debug.DrawLine(endPosition, predictedEndPosition, Color.red);
            
            launchData = CalculateLaunchData(launchPosition, predictedEndPosition);
            yaw = Quaternion.LookRotation(predictedEndPosition - launchPosition, Vector3.up).eulerAngles.y;
        }
        
        float launchSpeed = launchData.initialVelocity.magnitude;
        float pitch = Quaternion.LookRotation(launchData.initialVelocity, Vector3.up).eulerAngles.x;
        
        return (pitch, yaw, launchSpeed);
    }
    
    private Balistics.LaunchData CalculateLaunchData(Vector3 startPosition, Vector3 endPosition)
    {
        var launchData = Balistics.CalculateLaunchData(endPosition, startPosition, -9.81f, drawTrajectory);
        return launchData;
    }
}
