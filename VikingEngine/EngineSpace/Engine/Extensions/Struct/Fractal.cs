using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.EngineSpace.Engine.Extensions.Struct
{
    public struct Fraction
    {
        public int Numerator;
        public int Denominator;

        // --- Constructor ---

        public Fraction(int value)
        {
            Numerator = value;
            Denominator = 1;
        }
        public Fraction(int numerator, int denominator)
        {
            if (denominator == 0) throw new DivideByZeroException("Denominator cannot be zero.");

            // Simplify using Greatest Common Divisor
            int gcd = GetGCD(Math.Abs(numerator), Math.Abs(denominator));
            Numerator = numerator / gcd;
            Denominator = denominator / gcd;

            // Normalize the sign to always be on the Numerator
            if (Denominator < 0)
            {
                Numerator = -Numerator;
                Denominator = -Denominator;
            }
        }

        // --- Conversions ---

        // Implicitly convert int to Fraction (e.g., Fraction f = 5;)
        public static implicit operator Fraction(int value) => new Fraction(value, 1);

        // Explicitly convert Fraction to int (e.g., int x = (int)f;)
        public static explicit operator int(Fraction fraction)
        {
            if (fraction.Denominator != 1)
            {
                throw new InvalidCastException($"Cannot convert {fraction} to an integer because it is not a whole number.");
            }
            return fraction.Numerator;
        }

        // --- Math Operators ---

        public static Fraction operator +(Fraction a, Fraction b) =>
            new Fraction((a.Numerator * b.Denominator) + (b.Numerator * a.Denominator), a.Denominator * b.Denominator);

        public static Fraction operator -(Fraction a, Fraction b) =>
            new Fraction((a.Numerator * b.Denominator) - (b.Numerator * a.Denominator), a.Denominator * b.Denominator);

        public static Fraction operator *(Fraction a, Fraction b) =>
            new Fraction(a.Numerator * b.Numerator, a.Denominator * b.Denominator);

        public static Fraction operator /(Fraction a, Fraction b)
        {
            if (b.Numerator == 0) throw new DivideByZeroException("Cannot divide by a fraction with a value of zero.");
            return new Fraction(a.Numerator * b.Denominator, a.Denominator * b.Numerator);
        }

        // --- Equality Operators ---

        public static bool operator ==(Fraction a, Fraction b) =>
            a.Numerator == b.Numerator && a.Denominator == b.Denominator;

        public static bool operator !=(Fraction a, Fraction b) => !(a == b);

        public override bool Equals(object obj) => obj is Fraction other && this == other;

        public override int GetHashCode() => HashCode.Combine(Numerator, Denominator);

        // --- Helper Methods ---

        private static int GetGCD(int a, int b)
        {
            while (b != 0)
            {
                int temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        // Print nicely. If Denominator is 1, just print the whole number!
        public override string ToString() => Denominator == 1 ? Numerator.ToString() : $"{Numerator}/{Denominator}";
    }
}
