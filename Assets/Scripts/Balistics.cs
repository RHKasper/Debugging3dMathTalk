using UnityEngine;
using System.Collections;

/// <summary>
/// Copied and modified from: https://github.com/SebLague/Kinematic-Equation-Problems/blob/master/Kinematics%20problem%2002/Assets/Scripts/BallLauncher.cs
/// https://www.youtube.com/@SebastianLague
/// </summary>
public static class Balistics 
{
    public static LaunchData CalculateLaunchData(Vector3 targetPosition, Vector3 startPosition, float gravity, bool drawLine) {
        float displacementY = targetPosition.y - startPosition.y;
        Vector3 displacementXZ = new Vector3 (targetPosition.x - startPosition.x, 0, targetPosition.z - startPosition.z);
        float time = Mathf.Sqrt(-2*startPosition.y/gravity) + Mathf.Sqrt(2*(displacementY - startPosition.y)/gravity);
        Vector3 velocityY = Vector3.up * Mathf.Sqrt (-2 * gravity * startPosition.y);
        Vector3 velocityXZ = displacementXZ / time;
        
        var launchData = new LaunchData(velocityXZ + velocityY * -Mathf.Sign(gravity), time); 
        if (drawLine)
        {
            DrawPath(startPosition, launchData, gravity);
        }

        return launchData;
    }

    private static void DrawPath(Vector3 startPosition, LaunchData launchData, float gravity) {
        Vector3 previousDrawPoint = startPosition;

        int resolution = 30;
        for (int i = 1; i <= resolution; i++) {
            float simulationTime = i / (float)resolution * launchData.timeToTarget;
            Vector3 displacement = launchData.initialVelocity * simulationTime + Vector3.up *gravity * simulationTime * simulationTime / 2f;
            Vector3 drawPoint = startPosition + displacement;
            Debug.DrawLine (previousDrawPoint, drawPoint, Color.green);
            previousDrawPoint = drawPoint;
        }
    }

    public struct LaunchData {
        public readonly Vector3 initialVelocity;
        public readonly float timeToTarget;

        public LaunchData (Vector3 initialVelocity, float timeToTarget)
        {
            this.initialVelocity = initialVelocity;
            this.timeToTarget = timeToTarget;
        }
		
    }
}