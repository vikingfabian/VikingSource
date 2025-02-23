﻿using System;

//Craig Martin

/*
 * use the simplex noise class rather than the perlin ones. simplex is an upgraded perlin
 * noise which provides better values and is more economical, ie. uses significantly less
 * clock cycles to produce the noise values
 * 
 * call the GetSimplexNoisePermTable method once with your seed, then call either the
 * 2d or 3d noise method to get noise values
 * 
 * you can call GetSimplexNoisePermTable multiple times with different seeds if you want
 * to use different noise 'tables' for different parts of the game (store the array)
 */

namespace VikingEngine.EngineSpace.Maths
{
	public class PerlinNoise1
	{
		private const int GradientSizeTable = 256;
		private readonly Random _random;
		private readonly double[] _gradients = new double[GradientSizeTable * 3];
		/* Borrowed from Darwyn Peachey (see references above).
		   The gradient table is indexed with an XYZ triplet, which is first turned
		   into a single random index using a lookup in this table. The table simply
		   contains all numbers in [0..255] in random order. */
		private readonly byte[] _perm = new byte[] {
              225,155,210,108,175,199,221,144,203,116, 70,213, 69,158, 33,252,
                5, 82,173,133,222,139,174, 27,  9, 71, 90,246, 75,130, 91,191,
              169,138,  2,151,194,235, 81,  7, 25,113,228,159,205,253,134,142,
              248, 65,224,217, 22,121,229, 63, 89,103, 96,104,156, 17,201,129,
               36,  8,165,110,237,117,231, 56,132,211,152, 20,181,111,239,218,
              170,163, 51,172,157, 47, 80,212,176,250, 87, 49, 99,242,136,189,
              162,115, 44, 43,124, 94,150, 16,141,247, 32, 10,198,223,255, 72,
               53,131, 84, 57,220,197, 58, 50,208, 11,241, 28,  3,192, 62,202,
               18,215,153, 24, 76, 41, 15,179, 39, 46, 55,  6,128,167, 23,188,
              106, 34,187,140,164, 73,112,182,244,195,227, 13, 35, 77,196,185,
               26,200,226,119, 31,123,168,125,249, 68,183,230,177,135,160,180,
               12,  1,243,148,102,166, 38,238,251, 37,240,126, 64, 74,161, 40,
              184,149,171,178,101, 66, 29, 59,146, 61,254,107, 42, 86,154,  4,
              236,232,120, 21,233,209, 45, 98,193,114, 78, 19,206, 14,118,127,
               48, 79,147, 85, 30,207,219, 54, 88,234,190,122, 95, 67,143,109,
              137,214,145, 93, 92,100,245,  0,216,186, 60, 83,105, 97,204, 52};

		public PerlinNoise1(int seed)
		{
			_random = new Random(seed);
			InitGradients();
		}

		public double Noise(double x, double y, double z)
		{
			/* The main noise function. Looks up the pseudorandom gradients at the nearest
			   lattice points, dots them with the input vector, and interpolates the
			   results to produce a single output value in [0, 1] range. */

			int ix = (int)Math.Floor(x);
			double fx0 = x - ix;
			double fx1 = fx0 - 1;
			double wx = Smooth(fx0);

			int iy = (int)Math.Floor(y);
			double fy0 = y - iy;
			double fy1 = fy0 - 1;
			double wy = Smooth(fy0);

			int iz = (int)Math.Floor(z);
			double fz0 = z - iz;
			double fz1 = fz0 - 1;
			double wz = Smooth(fz0);

			double vx0 = Lattice(ix, iy, iz, fx0, fy0, fz0);
			double vx1 = Lattice(ix + 1, iy, iz, fx1, fy0, fz0);
			double vy0 = Lerp(wx, vx0, vx1);

			vx0 = Lattice(ix, iy + 1, iz, fx0, fy1, fz0);
			vx1 = Lattice(ix + 1, iy + 1, iz, fx1, fy1, fz0);
			double vy1 = Lerp(wx, vx0, vx1);

			double vz0 = Lerp(wy, vy0, vy1);

			vx0 = Lattice(ix, iy, iz + 1, fx0, fy0, fz1);
			vx1 = Lattice(ix + 1, iy, iz + 1, fx1, fy0, fz1);
			vy0 = Lerp(wx, vx0, vx1);

			vx0 = Lattice(ix, iy + 1, iz + 1, fx0, fy1, fz1);
			vx1 = Lattice(ix + 1, iy + 1, iz + 1, fx1, fy1, fz1);
			vy1 = Lerp(wx, vx0, vx1);

			double vz1 = Lerp(wy, vy0, vy1);
			return Lerp(wz, vz0, vz1);
		}

		private void InitGradients()
		{
			for (int i = 0; i < GradientSizeTable; i++)
			{
				double z = 1f - 2f * _random.NextDouble();
				double r = Math.Sqrt(1f - z * z);
				double theta = 2 * Math.PI * _random.NextDouble();
				_gradients[i * 3] = r * Math.Cos(theta);
				_gradients[i * 3 + 1] = r * Math.Sin(theta);
				_gradients[i * 3 + 2] = z;
			}
		}

		private int Permutate(int x)
		{
			const int mask = GradientSizeTable - 1;
			return _perm[x & mask];
		}

		private int Index(int ix, int iy, int iz)
		{
			// Turn an XYZ triplet into a single gradient table index.
			return Permutate(ix + Permutate(iy + Permutate(iz)));
		}

		private double Lattice(int ix, int iy, int iz, double fx, double fy, double fz)
		{
			// Look up a random gradient at [ix,iy,iz] and dot it with the [fx,fy,fz] vector.
			int index = Index(ix, iy, iz);
			int g = index * 3;
			return _gradients[g] * fx + _gradients[g + 1] * fy + _gradients[g + 2] * fz;
		}

		private double Lerp(double t, double value0, double value1)
		{
			// Simple linear interpolation.
			return value0 + t * (value1 - value0);
		}

		private double Smooth(double x)
		{
			/* Smoothing curve. This is used to calculate interpolants so that the noise
			  doesn't look blocky when the frequency is low. */
			return x * x * (3 - 2 * x);
		}
	}

	public class PerlinNoise2
	{
	/* Unit vectors for gradients to points on cube,
	equal distances apart (ie vector from center to the middle of each side */
		static int[][] grad3 = new int[12][]
	{new int[] {1,1,0},new int[] {-1,1,0},new int[] {1,-1,0},new int[] {-1,-1,0},
	new int[] {1,0,1},new int[] {-1,0,1},new int[] {1,0,-1},new int[] {-1,0,-1},
	new int[] {0,1,1},new int[] {0,-1,1},new int[] {0,1,-1},new int[] {0,-1,-1}};

	//0..255, randomized
	static int[] p = new int[] {151,160,137,91,90,15,
		131,13,201,95,96,53,194,233,7,225,140,36,103,30,69,142,8,99,37,240,21,10,23,
		190, 6,148,247,120,234,75,0,26,197,62,94,252,219,203,117,35,11,32,57,177,33,
		88,237,149,56,87,174,20,125,136,171,168, 68,175,74,165,71,134,139,48,27,166,
		77,146,158,231,83,111,229,122,60,211,133,230,220,105,92,41,55,46,245,40,244,
		102,143,54, 65,25,63,161, 1,216,80,73,209,76,132,187,208, 89,18,169,200,196,
		135,130,116,188,159,86,164,100,109,198,173,186, 3,64,52,217,226,250,124,123,
		5,202,38,147,118,126,255,82,85,212,207,206,59,227,47,16,58,17,182,189,28,42,
		223,183,170,213,119,248,152, 2,44,154,163, 70,221,153,101,155,167, 43,172,9,
		129,22,39,253, 19,98,108,110,79,113,224,232,178,185, 112,104,218,246,97,228,
		251,34,242,193,238,210,144,12,191,179,162,241, 81,51,145,235,249,14,239,107,
		49,192,214, 31,181,199,106,157,184, 84,204,176,115,121,50,45,127, 4,150,254,
		138,236,205,93,222,114,67,29,24,72,243,141,128,195,78,66,215,61,156,180};

	// To remove the need for index wrapping, double the permutation table length
	static int[] perm = new int[512];

	static PerlinNoise2()
	{
		for(int i=0; i<512; i++) perm[i]=p[i & 255];
	}

	// This method is a *lot* faster than using (int)Math.floor(x)
	static int fastfloor(double x) {
		return x>0 ? (int)x : (int)x-1;
	}

	static double dot(int[] g, double x, double y) {
		return g[0]*x + g[1]*y; }

	static double dot(int[] g, double x, double y, double z) {
		return g[0]*x + g[1]*y + g[2]*z; }

	static double dot(int[] g, double x, double y, double z, double w) {
		return g[0]*x + g[1]*y + g[2]*z + g[3]*w; }

	// 2D simplex noise
	public double noise(double xin, double yin)
	{
		double n0, n1, n2; // Noise contributions from the three corners

		// Skew the input space to a square to determine which simplex cell we're in
		double F2 = 0.5*(Math.Sqrt(3.0)-1.0);
		double s = (xin+yin)*F2; // Hairy factor for 2D
		int i = fastfloor(xin+s);
		int j = fastfloor(yin+s);

		double G2 = (3.0-Math.Sqrt(3.0))/6.0;
		double t = (i+j)*G2;
		double X0 = i-t; // Unskew the cell origin back to (x,y) space
		double Y0 = j-t;
		double x0 = xin-X0; // The x,y distances from the cell origin
		double y0 = yin-Y0;

		// For the 2D case, the simplex shape is an equilateral triangle.
		// Determine which simplex we are in.
		int i1, j1; // Offsets for second (middle) corner of simplex in (i,j) coords
		if(x0>y0) {i1=1; j1=0;} // lower triangle, XY order: (0,0)->(1,0)->(1,1)
		else {i1=0; j1=1;} // upper triangle, YX order: (0,0)->(0,1)->(1,1)

		// A step of (1,0) in (i,j) means a step of (1-c,-c) in (x,y), and
		// a step of (0,1) in (i,j) means a step of (-c,1-c) in (x,y), where
		// c = (3-sqrt(3))/6

		double x1 = x0 - i1 + G2; // Offsets for middle corner in (x,y) unskewed coords
		double y1 = y0 - j1 + G2;
		double x2 = x0 - 1.0 + 2.0 * G2; // Offsets for last corner in (x,y) unskewed coords
		double y2 = y0 - 1.0 + 2.0 * G2;

		// Work out the hashed gradient indices of the three simplex corners
		int ii = i & 255;
		int jj = j & 255;
		int gi0 = perm[ii+perm[jj]] % 12;
		int gi1 = perm[ii+i1+perm[jj+j1]] % 12;
		int gi2 = perm[ii+1+perm[jj+1]] % 12;

		// Calculate the contribution from the three corners
		double t0 = 0.5 - x0*x0-y0*y0;
		if(t0<0) n0 = 0.0;
		else {
			t0 *= t0;
			n0 = t0 * t0 * dot(grad3[gi0], x0, y0); // (x,y) of grad3 used for 2D gradient
		}
		double t1 = 0.5 - x1*x1-y1*y1;
		if(t1<0) n1 = 0.0;
		else {
			t1 *= t1;
			n1 = t1 * t1 * dot(grad3[gi1], x1, y1);
		}
		double t2 = 0.5 - x2*x2-y2*y2;
		if(t2<0) n2 = 0.0;
		else {
			t2 *= t2;
			n2 = t2 * t2 * dot(grad3[gi2], x2, y2);
		}
		// Add contributions from each corner to get the final noise value.
		// The result is scaled to return values in the interval [-1,1].
		return 70.0 * (n0 + n1 + n2);
	}


	// 3D simplex noise
	public double noise(double xin, double yin, double zin)
	{
		double n0, n1, n2, n3; // Noise contributions from the four corners
		// Skew the input space to determine which simplex cell we're in
		double F3 = 1.0/3.0;
		double s = (xin+yin+zin)*F3; // Very nice and simple skew factor for 3D
		int i = fastfloor(xin+s);
		int j = fastfloor(yin+s);
		int k = fastfloor(zin+s);
		double G3 = 1.0/6.0; // Very nice and simple unskew factor, too
		double t = (i+j+k)*G3;
		double X0 = i-t; // Unskew the cell origin back to (x,y,z) space
		double Y0 = j-t;
		double Z0 = k-t;
		double x0 = xin-X0; // The x,y,z distances from the cell origin
		double y0 = yin-Y0;
		double z0 = zin-Z0;
		// For the 3D case, the simplex shape is a slightly irregular tetrahedron.
		// Determine which simplex we are in.
		int i1, j1, k1; // Offsets for second corner of simplex in (i,j,k) coords
		int i2, j2, k2; // Offsets for third corner of simplex in (i,j,k) coords
		if(x0>=y0) {
			if(y0>=z0)
			{ i1=1; j1=0; k1=0; i2=1; j2=1; k2=0; } // X Y Z order
			else if(x0>=z0) { i1=1; j1=0; k1=0; i2=1; j2=0; k2=1; } // X Z Y order
			else { i1=0; j1=0; k1=1; i2=1; j2=0; k2=1; } // Z X Y order
		}
		else { // x0<y0
			if(y0<z0) { i1=0; j1=0; k1=1; i2=0; j2=1; k2=1; } // Z Y X order
			else if(x0<z0) { i1=0; j1=1; k1=0; i2=0; j2=1; k2=1; } // Y Z X order
			else { i1=0; j1=1; k1=0; i2=1; j2=1; k2=0; } // Y X Z order
		}
		// A step of (1,0,0) in (i,j,k) means a step of (1-c,-c,-c) in (x,y,z),
		// a step of (0,1,0) in (i,j,k) means a step of (-c,1-c,-c) in (x,y,z), and
		// a step of (0,0,1) in (i,j,k) means a step of (-c,-c,1-c) in (x,y,z), where
		// c = 1/6.
		double x1 = x0 - i1 + G3; // Offsets for second corner in (x,y,z) coords
		double y1 = y0 - j1 + G3;
		double z1 = z0 - k1 + G3;
		double x2 = x0 - i2 + 2.0*G3; // Offsets for third corner in (x,y,z) coords
		double y2 = y0 - j2 + 2.0*G3;
		double z2 = z0 - k2 + 2.0*G3;
		double x3 = x0 - 1.0 + 3.0*G3; // Offsets for last corner in (x,y,z) coords
		double y3 = y0 - 1.0 + 3.0*G3;
		double z3 = z0 - 1.0 + 3.0*G3;
		// Work out the hashed gradient indices of the four simplex corners
		int ii = i & 255;
		int jj = j & 255;
		int kk = k & 255;
		int gi0 = perm[ii+perm[jj+perm[kk]]] % 12;
		int gi1 = perm[ii+i1+perm[jj+j1+perm[kk+k1]]] % 12;
		int gi2 = perm[ii+i2+perm[jj+j2+perm[kk+k2]]] % 12;
		int gi3 = perm[ii+1+perm[jj+1+perm[kk+1]]] % 12;
		// Calculate the contribution from the four corners
		double t0 = 0.6 - x0*x0 - y0*y0 - z0*z0;
		if(t0<0) n0 = 0.0;
		else {
			t0 *= t0;
			n0 = t0 * t0 * dot(grad3[gi0], x0, y0, z0);
		}
		double t1 = 0.6 - x1*x1 - y1*y1 - z1*z1;
		if(t1<0) n1 = 0.0;
		else {
			t1 *= t1;
			n1 = t1 * t1 * dot(grad3[gi1], x1, y1, z1);
		}
		double t2 = 0.6 - x2*x2 - y2*y2 - z2*z2;
		if(t2<0) n2 = 0.0;
		else {
			t2 *= t2;
			n2 = t2 * t2 * dot(grad3[gi2], x2, y2, z2);
		}
		double t3 = 0.6 - x3*x3 - y3*y3 - z3*z3;
		if(t3<0) n3 = 0.0;
		else {
			t3 *= t3;
			n3 = t3 * t3 * dot(grad3[gi3], x3, y3, z3);
		}
		// Add contributions from each corner to get the final noise value.
		// The result is scaled to stay just inside [-1,1]
		return 32.0*(n0 + n1 + n2 + n3);
	}
	}

	// amplitude = range (0 - 1?)
	// frequency = 1 / wavelength
	public class SimplexNoise1
	{
        #region Initizalize grad3

        private static int[][] grad3 = { 
                                           new int[]{1,1,0}, 
                                           new int[]{-1,1,0}, 
                                           new int[]{1,-1,0}, 
                                           new int[]{-1,-1,0}, 
                                           new int[]{1,0,1}, 
                                           new int[]{-1,0,1}, 
                                           new int[]{1,0,-1}, 
                                           new int[]{-1,0,-1}, 
                                           new int[]{0,1,1}, 
                                           new int[]{0,-1,1}, 
                                           new int[]{0,1,-1}, 
                                           new int[]{0,-1,-1} 
                                       };

        #endregion

        #region Initizalize grad4

        private static int[][] grad4 = { 
                                           new int[]{0,1,1,1}, 
                                           new int[]{0,1,1,-1}, 
                                           new int[]{0,1,-1,1}, 
                                           new int[]{0,1,-1,-1}, 
                                           new int[]{0,-1,1,1}, 
                                           new int[]{0,-1,1,-1}, 
                                           new int[]{0,-1,-1,1}, 
                                           new int[]{0,-1,-1,-1}, 
                                           new int[]{1,0,1,1}, 
                                           new int[]{1,0,1,-1}, 
                                           new int[]{1,0,-1,1}, 
                                           new int[]{1,0,-1,-1}, 
                                           new int[]{-1,0,1,1}, 
                                           new int[]{-1,0,1,-1}, 
                                           new int[]{-1,0,-1,1}, 
                                           new int[]{-1,0,-1,-1}, 
                                           new int[]{1,1,0,1}, 
                                           new int[]{1,1,0,-1}, 
                                           new int[]{1,-1,0,1}, 
                                           new int[]{1,-1,0,-1}, 
                                           new int[]{-1,1,0,1}, 
                                           new int[]{-1,1,0,-1}, 
                                           new int[]{-1,-1,0,1}, 
                                           new int[]{-1,-1,0,-1}, 
                                           new int[]{1,1,1,0}, 
                                           new int[]{1,1,-1,0}, 
                                           new int[]{1,-1,1,0}, 
                                           new int[]{1,-1,-1,0}, 
                                           new int[]{-1,1,1,0}, 
                                           new int[]{-1,1,-1,0}, 
                                           new int[]{-1,-1,1,0}, 
                                           new int[]{-1,-1,-1,0} 
                                       };

        #endregion

        // A lookup table to traverse the simplex around a given point in 4D. 
        // Details can be found where this table is used, in the 4D noise method. 
        private static int[][] simplex = 
		{ 
            new int[]{0,1,2,3},new int[]{0,1,3,2},new int[]{0,0,0,0},new int[]{0,2,3,1},new int[]{0,0,0,0},new int[]{0,0,0,0},new int[]{0,0,0,0},new int[]{1,2,3,0}, 
            new int[]{0,2,1,3},new int[]{0,0,0,0},new int[]{0,3,1,2},new int[]{0,3,2,1},new int[]{0,0,0,0},new int[]{0,0,0,0},new int[]{0,0,0,0},new int[]{1,3,2,0}, 
            new int[]{0,0,0,0},new int[]{0,0,0,0},new int[]{0,0,0,0},new int[]{0,0,0,0},new int[]{0,0,0,0},new int[]{0,0,0,0},new int[]{0,0,0,0},new int[]{0,0,0,0}, 
            new int[]{1,2,0,3},new int[]{0,0,0,0},new int[]{1,3,0,2},new int[]{0,0,0,0},new int[]{0,0,0,0},new int[]{0,0,0,0},new int[]{2,3,0,1},new int[]{2,3,1,0}, 
            new int[]{1,0,2,3},new int[]{1,0,3,2},new int[]{0,0,0,0},new int[]{0,0,0,0},new int[]{0,0,0,0},new int[]{2,0,3,1},new int[]{0,0,0,0},new int[]{2,1,3,0}, 
            new int[]{0,0,0,0},new int[]{0,0,0,0},new int[]{0,0,0,0},new int[]{0,0,0,0},new int[]{0,0,0,0},new int[]{0,0,0,0},new int[]{0,0,0,0},new int[]{0,0,0,0}, 
            new int[]{2,0,1,3},new int[]{0,0,0,0},new int[]{0,0,0,0},new int[]{0,0,0,0},new int[]{3,0,1,2},new int[]{3,0,2,1},new int[]{0,0,0,0},new int[]{3,1,2,0}, 
            new int[]{2,1,0,3},new int[]{0,0,0,0},new int[]{0,0,0,0},new int[]{0,0,0,0},new int[]{3,1,0,2},new int[]{0,0,0,0},new int[]{3,2,0,1},new int[]{3,2,1,0} 
        };

        #region Init p

        private static int[] p = {151,160,137,91,90,15,131,13,201,95,96,53,194,233,7,225,140,36,103,30,69,142,8,99,37,240,21,10,23, 
190, 6,148,247,120,234,75,0,26,197,62,94,252,219,203,117,35,11,32,57,177,33, 
88,237,149,56,87,174,20,125,136,171,168, 68,175,74,165,71,134,139,48,27,166, 
77,146,158,231,83,111,229,122,60,211,133,230,220,105,92,41,55,46,245,40,244, 
102,143,54, 65,25,63,161, 1,216,80,73,209,76,132,187,208, 89,18,169,200,196, 
135,130,116,188,159,86,164,100,109,198,173,186, 3,64,52,217,226,250,124,123, 
5,202,38,147,118,126,255,82,85,212,207,206,59,227,47,16,58,17,182,189,28,42, 
223,183,170,213,119,248,152, 2,44,154,163, 70,221,153,101,155,167, 43,172,9, 
129,22,39,253, 19,98,108,110,79,113,224,232,178,185, 112,104,218,246,97,228, 
251,34,242,193,238,210,144,12,191,179,162,241, 81,51,145,235,249,14,239,107, 
49,192,214, 31,181,199,106,157,184, 84,204,176,115,121,50,45,127, 4,150,254, 
138,236,205,93,222,114,67,29,24,72,243,141,128,195,78,66,215,61,156,180};

        #endregion

        // To remove the need for index wrapping, float the permutation table length 

        /// <summary> 
        /// Initializes the <see cref="PerlinSimplexNoise"/> class. 
        /// </summary> 
        /// <author>Sjef van Leeuwen 3-3-2007 18:27</author> 
		public static int[] GetSimplexNoisePermTable(int seed)
        {
            int[] perm = new int[512];
            var r = new PcgRandom(seed);
            for (int i = 0; i < 512; i++) perm[i] = (byte)(p[i & 255] * r.Int());
            return perm;
        }

        // This method is a *lot* faster than using (int)Math.floor(x) 
        private static int fastfloor(float x)
        {
            return x > 0 ? (int)x : (int)x - 1;
        }

        private static float dot(int[] g, float x, float y)
        {
            return g[0] * x + g[1] * y;
        }

        private static float dot(int[] g, float x, float y, float z)
        {
            return g[0] * x + g[1] * y + g[2] * z;
        }

        private static float dot(int[] g, float x, float y, float z, float w)
        {
            return g[0] * x + g[1] * y + g[2] * z + g[3] * w;
        }


        /// <summary> 
        /// 3D Simplex noise. 
        /// </summary> 
        /// <param name="xin">The xin.</param> 
        /// <param name="yin">The yin.</param> 
        /// <param name="zin">The zin.</param> 
        /// <returns></returns> 
        /// <author>Sjef van Leeuwen 3-3-2007 18:44</author> 
        public static float noise(float xin, float yin, float zin, int[] perm)
        {
            float n0, n1, n2, n3; // Noise contributions from the four corners 
            // Skew the input space to determine which simplex cell we're in 
            float F3 = 1.0f / 3.0f;
            float s = (xin + yin + zin) * F3; // Very nice and simple skew factor for 3D 
            int i = fastfloor(xin + s);
            int j = fastfloor(yin + s);
            int k = fastfloor(zin + s);
            float G3 = 1.0f / 6.0f; // Very nice and simple unskew factor, too 
            float t = (i + j + k) * G3;
            float X0 = i - t; // Unskew the cell origin back to (x,y,z) space 
            float Y0 = j - t;
            float Z0 = k - t;
            float x0 = xin - X0; // The x,y,z distances from the cell origin 
            float y0 = yin - Y0;
            float z0 = zin - Z0;
            // For the 3D case, the simplex shape is a slightly irregular tetrahedron. 
            // Determine which simplex we are in. 
            int i1, j1, k1; // Offsets for second corner of simplex in (i,j,k) coords 
            int i2, j2, k2; // Offsets for third corner of simplex in (i,j,k) coords 
            if (x0 >= y0)
            {
                if (y0 >= z0)
                {
                    i1 = 1; j1 = 0; k1 = 0; i2 = 1; j2 = 1; k2 = 0;
                } // X Y Z order 
                else if (x0 >= z0)
                {
                    i1 = 1; j1 = 0; k1 = 0; i2 = 1; j2 = 0; k2 = 1;
                } // X Z Y order 
                else
                {
                    i1 = 0; j1 = 0; k1 = 1; i2 = 1; j2 = 0; k2 = 1;
                } // Z X Y order 
            }
            else
            { // x0<y0 
                if (y0 < z0)
                {
                    i1 = 0; j1 = 0; k1 = 1; i2 = 0; j2 = 1; k2 = 1;
                } // Z Y X order 
                else if (x0 < z0)
                {
                    i1 = 0; j1 = 1; k1 = 0; i2 = 0; j2 = 1; k2 = 1;
                } // Y Z X order 
                else
                {
                    i1 = 0; j1 = 1; k1 = 0; i2 = 1; j2 = 1; k2 = 0;
                } // Y X Z order 
            }
            // A step of (1,0,0) in (i,j,k) means a step of (1-c,-c,-c) in (x,y,z), 
            // a step of (0,1,0) in (i,j,k) means a step of (-c,1-c,-c) in (x,y,z), and 
            // a step of (0,0,1) in (i,j,k) means a step of (-c,-c,1-c) in (x,y,z), where 
            // c = 1/6. 
            float x1 = x0 - i1 + G3; // Offsets for second corner in (x,y,z) coords 
            float y1 = y0 - j1 + G3;
            float z1 = z0 - k1 + G3;
            float x2 = x0 - i2 + 2.0f * G3; // Offsets for third corner in (x,y,z) coords 
            float y2 = y0 - j2 + 2.0f * G3;
            float z2 = z0 - k2 + 2.0f * G3;
            float x3 = x0 - 1.0f + 3.0f * G3; // Offsets for last corner in (x,y,z) coords 
            float y3 = y0 - 1.0f + 3.0f * G3;
            float z3 = z0 - 1.0f + 3.0f * G3;
            // Work out the hashed gradient indices of the four simplex corners 
            int ii = i & 255;
            int jj = j & 255;
            int kk = k & 255;
            int gi0 = perm[ii + perm[jj + perm[kk]]] % 12;
            int gi1 = perm[ii + i1 + perm[jj + j1 + perm[kk + k1]]] % 12;
            int gi2 = perm[ii + i2 + perm[jj + j2 + perm[kk + k2]]] % 12;
            int gi3 = perm[ii + 1 + perm[jj + 1 + perm[kk + 1]]] % 12;
            // Calculate the contribution from the four corners 
            float t0 = 0.6f - x0 * x0 - y0 * y0 - z0 * z0;
            if (t0 < 0) n0 = 0.0f;
            else
            {
                t0 *= t0;
                n0 = t0 * t0 * dot(grad3[gi0], x0, y0, z0);
            }
            float t1 = 0.6f - x1 * x1 - y1 * y1 - z1 * z1;
            if (t1 < 0) n1 = 0.0f;
            else
            {
                t1 *= t1;
                n1 = t1 * t1 * dot(grad3[gi1], x1, y1, z1);
            }
            float t2 = 0.6f - x2 * x2 - y2 * y2 - z2 * z2;
            if (t2 < 0) n2 = 0.0f;
            else
            {
                t2 *= t2;
                n2 = t2 * t2 * dot(grad3[gi2], x2, y2, z2);
            }
            float t3 = 0.6f - x3 * x3 - y3 * y3 - z3 * z3;
            if (t3 < 0) n3 = 0.0f;
            else
            {
                t3 *= t3;
                n3 = t3 * t3 * dot(grad3[gi3], x3, y3, z3);
            }
            // Add contributions from each corner to get the final noise value. 
            // The result is scaled to stay just inside [-1,1] 
            return 32.0f * (n0 + n1 + n2 + n3);
        }

		static double sqrt3 = Math.Sqrt(3.0);

        // 2D simplex noise 
        public static float noise(float xin, float yin, int[] perm)
        {
            float n0, n1, n2; // Noise contributions from the three corners 
            // Skew the input space to determine which simplex cell we're in 
			float F2 = (float)(0.5 * (sqrt3 - 1.0));
            float s = (xin + yin) * F2; // Hairy factor for 2D 
            int i = fastfloor(xin + s);
            int j = fastfloor(yin + s);
			float g2 = (float)((3.0 - sqrt3) / 6.0);
            float t = (i + j) * g2;
            float X0 = i - t; // Unskew the cell origin back to (x,y) space 
            float Y0 = j - t;
            float x0 = xin - X0; // The x,y distances from the cell origin 
            float y0 = yin - Y0;
            // For the 2D case, the simplex shape is an equilateral triangle. 
            // Determine which simplex we are in. 
            int i1, j1; // Offsets for second (middle) corner of simplex in (i,j) coords 
            if (x0 > y0)
            {
                i1 = 1; j1 = 0;
            } // lower triangle, XY order: (0,0)->(1,0)->(1,1) 
            else
            {
                i1 = 0; j1 = 1;
            } // upper triangle, YX order: (0,0)->(0,1)->(1,1) 
            // A step of (1,0) in (i,j) means a step of (1-c,-c) in (x,y), and 
            // a step of (0,1) in (i,j) means a step of (-c,1-c) in (x,y), where 
            // c = (3-sqrt(3))/6 
            float x1 = x0 - i1 + g2; // Offsets for middle corner in (x,y) unskewed coords 
            float y1 = y0 - j1 + g2;
            float x2 = x0 - 1.0f + 2.0f * g2; // Offsets for last corner in (x,y) unskewed coords 
            float y2 = y0 - 1.0f + 2.0f * g2;
            // Work out the hashed gradient indices of the three simplex corners 
            int ii = i & 255;
            int jj = j & 255;
            int gi0 = perm[ii + perm[jj]] % 12;
            int gi1 = perm[ii + i1 + perm[jj + j1]] % 12;
            int gi2 = perm[ii + 1 + perm[jj + 1]] % 12;
            // Calculate the contribution from the three corners 
            float t0 = 0.5f - x0 * x0 - y0 * y0;
            if (t0 < 0)
                n0 = 0.0f;
            else
            {
                t0 *= t0;
                n0 = t0 * t0 * dot(grad3[gi0], x0, y0); // (x,y) of grad3 used for 2D gradient 
            }
            float t1 = 0.5f - x1 * x1 - y1 * y1;
            if (t1 < 0)
                n1 = 0.0f;
            else
            {
                t1 *= t1;
                n1 = t1 * t1 * dot(grad3[gi1], x1, y1);
            }
            float t2 = 0.5f - x2 * x2 - y2 * y2;
            if (t2 < 0)
                n2 = 0.0f;
            else
            {
                t2 *= t2;
                n2 = t2 * t2 * dot(grad3[gi2], x2, y2);
            }
            // Add contributions from each corner to get the final noise value. 
            // The result is scaled to return values in the interval [-1,1]. 
            float returnNoise = 70.0f * (n0 + n1 + n2);
            // make it range from 0 to 1;
            return (returnNoise + 1.0f) * 0.5f;
        }
    } 
}
