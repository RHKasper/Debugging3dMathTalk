using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CannonAimer : MonoBehaviour
{
    [SerializeField] private Camera camera;
    [SerializeField] private CannonController cannon;
    [SerializeField] private Transform target;
    
    private LayerMask _layerMask;

    private void Start()
    {
        _layerMask = LayerMask.GetMask("Terrain");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 deltaPosition = cannon.GetCannonBallSpawnTransform().position - target.position;
        
        // calculate yaw
        float yaw = Quaternion.LookRotation(target.position - cannon.transform.position, Vector3.up).eulerAngles.y;
        
        // calculate pitch with quadratic formula
        bool canHitTarget = CalculatePitch(deltaPosition, out float pitch, out float flightTime);

        if (canHitTarget)
        {
            cannon.AimAndFire(pitch, yaw);
        }
    }

    private bool CalculatePitch(Vector3 deltaPosition, out float pitch, out float flightTime)
    {
        Ballistics.SolveBallisticPitch(deltaPosition.x, deltaPosition.y, 10, out float lowAngle, out float highAngle, out float lowTime, out float highTime);

        if (-lowAngle >= CannonController.MinBarrelPitch && -lowAngle <= CannonController.MaxBarrelPitch)
        {
            pitch = -lowAngle;
            flightTime = lowTime;
            return true;
        }
        
        if (-highAngle >= CannonController.MinBarrelPitch && -highAngle <= CannonController.MaxBarrelPitch)
        {
            pitch = -highAngle;
            flightTime = highTime;
            return true;
        }

        pitch = 0;
        flightTime = 0;
        return false;
    }
}
