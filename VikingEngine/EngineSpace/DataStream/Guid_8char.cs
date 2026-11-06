using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine
{
    struct Guid_8char : IEquatable<Guid_8char>
    {
        public static readonly Guid_8char Empty = new Guid_8char(new string('0', 8));

        string _value;

        // 62 characters: Uppercase + Lowercase + Numbers
        const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        const int IdLength = 8;

        int _hash;

        // Private constructor forces usage of NewId() or parsing
        Guid_8char(string value)
        {
            _value = value;
            _hash = _value.GetHashCode(StringComparison.Ordinal);
        }

        public void write(System.IO.BinaryWriter w)
        {
            w.Write(_value.ToCharArray());
        }
        public static Guid_8char Read(System.IO.BinaryReader r)
        {
            return new Guid_8char(new string(r.ReadChars(IdLength)));
        }

        public void writeHash(System.IO.BinaryWriter w)
        {
            w.Write(_hash);
        }

        public static int ReadHash(System.IO.BinaryReader r)
        {
            return r.ReadInt32();
        }

        /// <summary>
        /// Generates a new random 8-character ShortId.
        /// </summary>
        public static Guid_8char NewId()
        {
            // Use stackalloc to avoid allocating an array on the heap
            Span<char> buffer = stackalloc char[IdLength];

            for (int i = 0; i < IdLength; i++)
            {
                buffer[i] = Alphabet[Random.Shared.Next(Alphabet.Length)];
            }

            return new Guid_8char(new string(buffer));
        }

        // Override ToString to return the actual string value
        public override string ToString() => _value ?? new string('0', IdLength);

        // Standard Struct Equality Boilerplate
        public bool Equals(Guid_8char other) => other._hash == _hash && string.Equals(_value, other._value, StringComparison.Ordinal);
        public override bool Equals(object? obj) => obj is Guid_8char other && Equals(other);
        public override int GetHashCode() => _hash;

        public static bool operator ==(Guid_8char left, Guid_8char right) => left.Equals(right);
        public static bool operator !=(Guid_8char left, Guid_8char right) => !left.Equals(right);

        // Optional: Allows you to implicitly pass this struct to methods expecting a string
        public static implicit operator string(Guid_8char id) => id.ToString();
    }
}
