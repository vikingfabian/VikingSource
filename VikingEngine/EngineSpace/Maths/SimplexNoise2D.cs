using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.EngineSpace.Maths
{
    class SimplexNoise2D
    {
        /* Constants */
        private const uint PERMUTATION_COUNT = 512;

        /* Fields */
        public int seed;

        // Seeded simplex noise in 2d (a shame higher dimensions are patented...)
        public SimplexNoise2D(int seed)
        {
            this.seed = seed;
            Random prng = new Random(seed);

            perm = new int[2 * PERMUTATION_COUNT];
            for(int i = 0; i < PERMUTATION_COUNT; ++i)
            {
                int val = prng.Next(512);
                perm[i] = val;
                perm[i + PERMUTATION_COUNT] = val;
            }
        }

        public float OctaveNoise2D_Normal(float octaves, float persistence, float scale, float x, float y)
        {
            return (OctaveNoise2D(octaves, persistence, scale, x, y) + 1f) / 2f;
        }

        // Each octave adds a higher frequency/lower amplitude function.
        // Amplitude dies with the persistence factor [0-1] after each octave.
        public float OctaveNoise2D(float octaves, float persistence, float scale, float x, float y)
        {
            float total = 0;
            float frequency = scale / 512.0f;
            float amplitude = 1;

            // Keep track of maximum amplitude, for modifying into [-1,1] at end.
            float maxAmplitude = 0;

            for (int i = 0; i < octaves; i++)
            {
                total += RawNoise2D(x * frequency, y * frequency) * amplitude;

                frequency *= 2;
                maxAmplitude += amplitude;
                amplitude *= persistence;
            }

            return total / maxAmplitude;
        }

        /// <summary>
        /// Returns the range of the noise function.
        /// </summary>
        public static readonly IntervalF Range = new IntervalF(-1, 1);

        // Permutation table.  The same list is repeated twice.
        private int[] perm = null;

        // Ugly cached gradients, still gives speed though! :)))
        private static int[][] CubeGradients =
        {
            new int[2] {1,1},
            new int[2] {-1,1},
            new int[2] {1,-1},
            new int[2] {-1,-1},
            new int[2] {1,0},
            new int[2] {-1,0},
            new int[2] {1,0},
            new int[2] {-1,0},
            new int[2] {0,1},
            new int[2] {0,-1},
            new int[2] {0,1}, 
            new int[2] {0,-1}
        };

        // Faster floor function
        private int FastFloor( float x )
        { 
           return x > 0 ? (int) x : (int) x - 1;
        }

        // Haxy dot product for 2d for first two elements
        private float Dot2D( int[] u, float x, float y )
        {
            return u[0] * x + u[1] * y;
        }

        // 2D raw Simplex noise
        private float RawNoise2D( float x, float y )
        {
            // Noise contributions from the three corners
            float n0, n1, n2;
            n0 = n1 = n2 = 0.0f;

            // Skew the input space to determine which simplex cell we're in
            float F2 = (float)(0.5 * (Math.Sqrt(3.0) - 1.0));
            // Hairy factor for 2D
            float s = (x + y) * F2;
            int i = FastFloor( x + s );
            int j = FastFloor( y + s );

            float G2 = (float)((3.0 - Math.Sqrt(3.0)) / 6.0);
            float t = (i + j) * G2;
            // Unskew the cell origin back to (x,y) space
            float X0 = i-t;
            float Y0 = j-t;
            // The x,y distances from the cell origin
            float x0 = x-X0;
            float y0 = y-Y0;

            // For the 2D case, the simplex shape is an equilateral triangle.
            // Determine which simplex we are in.
            int i1, j1; // Offsets for second (middle) corner of simplex in (i,j) coords
            if (x0 > y0) // lower triangle, XY order: (0,0)->(1,0)->(1,1)
            {
                i1=1;
                j1=0;
            }
            else // upper triangle, YX order: (0,0)->(0,1)->(1,1)
            {
                i1=0;
                j1=1;
            }

            // A step of (1,0) in (i,j) means a step of (1-c,-c) in (x,y), and
            // a step of (0,1) in (i,j) means a step of (-c,1-c) in (x,y), where
            // c = (3-sqrt(3))/6
            float x1 = x0 - i1 + G2; // Offsets for middle corner in (x,y) unskewed coords
            float y1 = y0 - j1 + G2;
            float x2 = x0 - 1.0f + 2.0f * G2; // Offsets for last corner in (x,y) unskewed coords
            float y2 = y0 - 1.0f + 2.0f * G2;

            // Work out the hashed gradient indices of the three simplex corners
            int ii = i & 255;
            int jj = j & 255;
            int gi0 = perm[ii+perm[jj]] % 12;
            int gi1 = perm[ii+i1+perm[jj+j1]] % 12;
            int gi2 = perm[ii+1+perm[jj+1]] % 12;

            // Calculate the contribution from the three corners
            float t0 = 0.5f - x0*x0 - y0*y0;
            if(t0<0)
            {
                n0 = 0.0f;
            }
            else
            {
                t0 *= t0;
                n0 = t0 * t0 * Dot2D(CubeGradients[gi0], x0, y0); // (x,y) of grad3 used for 2D gradient
            }

            float t1 = 0.5f - x1*x1 - y1*y1;
            if(t1<0)
            {
                n1 = 0.0f;
            }
            else
            {
                t1 *= t1;
                n1 = t1 * t1 * Dot2D(CubeGradients[gi1], x1, y1);
            }

            float t2 = 0.5f - x2*x2 - y2*y2;
            if(t2<0)
            {
                n2 = 0.0f;
            }
            else
            {
                t2 *= t2;
                n2 = t2 * t2 * Dot2D(CubeGradients[gi2], x2, y2);
            }

            // Add contributions from each corner to get the final noise value.
            // The result is scaled to return values in the interval [-1,1].
            return 70.0f * (n0 + n1 + n2);
        }
    }
}
