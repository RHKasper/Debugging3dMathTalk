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
        cannon.AimAndFire(-20, yaw);
    }
}
