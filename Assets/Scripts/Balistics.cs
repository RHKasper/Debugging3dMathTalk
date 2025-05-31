using UnityEngine;

public static class Ballistics
{
    // Returns true if a valid ballistic arc exists.
    // Outputs low/high pitch angles (in degrees) and corresponding flight times (in seconds).
    public static bool SolveBallisticPitch(
        float x, float y, float speed,
        out float lowAngleDeg, out float highAngleDeg,
        out float lowTime, out float highTime,
        float gravity = 9.81f)
    {
        lowAngleDeg = highAngleDeg = 0f;
        lowTime = highTime = 0f;

        float v2 = speed * speed;
        float v4 = v2 * v2;

        float gx = gravity * x;

        float discriminant = v4 - gravity * (gravity * x * x + 2 * y * v2);
        if (discriminant < 0f)
        {
            // No solution: target unreachable at this speed
            return false;
        }

        float sqrt = Mathf.Sqrt(discriminant);

        float angle1Rad = Mathf.Atan((v2 + sqrt) / gx);
        float angle2Rad = Mathf.Atan((v2 - sqrt) / gx);

        // Sort so that angle1 is the lower angle
        float thetaLow = Mathf.Min(angle1Rad, angle2Rad);
        float thetaHigh = Mathf.Max(angle1Rad, angle2Rad);

        lowAngleDeg = thetaLow * Mathf.Rad2Deg;
        highAngleDeg = thetaHigh * Mathf.Rad2Deg;

        // Time = horizontal distance / horizontal velocity component
        lowTime = x / (speed * Mathf.Cos(thetaLow));
        highTime = x / (speed * Mathf.Cos(thetaHigh));

        return true;
    }
}