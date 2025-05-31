public static class Utils
{
    public static float SanitizeAngleNeg180To180(float angle)
    {
        return angle % 360;
    }
}