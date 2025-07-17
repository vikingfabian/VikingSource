namespace NativeShared
{
    public class Test
    {
        public static float RunHeavyLoop()
        {
            float sum = 0;
            for (int i = 0; i < 100_000_000; i++)
            {
                float x = i * 0.0001f;
                sum += (x * x + x) / (x + 1);
            }
            return sum;
        }
    }
}
