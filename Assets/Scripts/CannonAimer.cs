using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CannonAimer : MonoBehaviour
{
    [SerializeField] private Camera camera;
    [SerializeField] private CannonController cannon;

    private LayerMask _layerMask;

    private void Start()
    {
        _layerMask = LayerMask.GetMask("Terrain");
    }

    // Update is called once per frame
    void Update()
    {
        Physics.Raycast(camera.ScreenPointToRay(Pointer.current.position.ReadValue()), out var hit, 1000, _layerMask);

        if (hit.collider != null)
        {
            var target = hit.point;
            Vector3 deltaPosition = cannon.GetCannonBallSpawnTransform().position - target;
            
            // calculate yaw
            float yaw = Quaternion.LookRotation(target - cannon.transform.position, Vector3.up).eulerAngles.y;
            
            
            // calculate pitch with quadratic formula
            cannon.AimAndFire(-20, yaw);

        }
    }
}
