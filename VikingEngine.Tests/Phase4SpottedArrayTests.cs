using System;
using VikingEngine.Tests.Legacy;
using Xunit;

namespace VikingEngine.Tests
{
    public class Phase4SpottedArrayTests
    {
        [Fact]
        public void TrimExcess_WhenOversized_ShrinksCapacityAndPreservesElements()
        {
            var array = new SpottedArray<string>(64);
            int idx0 = array.Add("Item0");
            int idx1 = array.Add("Item1");
            int idx2 = array.Add("Item2");

            Assert.Equal(64, array.Array.Length);
            Assert.Equal(3, array.Count);
            Assert.Equal(3, array.SpottedLength);

            bool trimmed = array.TrimExcess();

            Assert.True(trimmed);
            Assert.True(array.Array.Length < 64);
            Assert.Equal("Item0", array.Array[idx0]);
            Assert.Equal("Item1", array.Array[idx1]);
            Assert.Equal("Item2", array.Array[idx2]);
            Assert.Equal(3, array.Count);
            Assert.Equal(3, array.SpottedLength);
        }

        [Fact]
        public void TrimExcess_PreservesIndicesWhenHolesExist()
        {
            var array = new SpottedArray<string>(64);
            int idx0 = array.Add("Item0");
            int idx1 = array.Add("Item1");
            int idx2 = array.Add("Item2");
            int idx3 = array.Add("Item3");

            // Create a hole in the middle
            array.RemoveAt(idx1);

            Assert.Equal(3, array.Count);
            Assert.Equal(4, array.SpottedLength);

            bool trimmed = array.TrimExcess();

            Assert.True(trimmed);
            Assert.Equal("Item0", array.Array[idx0]);
            Assert.Null(array.Array[idx1]);
            Assert.Equal("Item2", array.Array[idx2]);
            Assert.Equal("Item3", array.Array[idx3]);
            Assert.Equal(3, array.Count);
            Assert.Equal(4, array.SpottedLength);
        }

        [Fact]
        public void TrimExcess_WhenNotOversized_ReturnsFalseWithoutModifyingArray()
        {
            var array = new SpottedArray<string>(8);
            for (int i = 0; i < 6; i++)
            {
                array.Add($"Item{i}");
            }

            Assert.Equal(8, array.Array.Length);
            bool trimmed = array.TrimExcess();

            Assert.False(trimmed);
            Assert.Equal(8, array.Array.Length);
        }

        [Fact]
        public void TrimExcessExact_ShrinksToSpottedLength()
        {
            var array = new SpottedArray<string>(128);
            array.Add("A");
            array.Add("B");

            Assert.Equal(128, array.Array.Length);

            bool trimmed = array.TrimExcessExact(8);

            Assert.True(trimmed);
            Assert.Equal(8, array.Array.Length);
            Assert.Equal("A", array.Array[0]);
            Assert.Equal("B", array.Array[1]);
        }

        [Fact]
        public void LegacyComparison_LegacyRetainsPeakCapacity_ModernReclaimsMemory()
        {
            var legacy = new LegacySpottedArray<string>(4);
            var modern = new SpottedArray<string>(4);

            // Expand both to 128 elements
            for (int i = 0; i < 100; i++)
            {
                legacy.Add($"Item{i}");
                modern.Add($"Item{i}");
            }

            Assert.True(legacy.Array.Length >= 128);
            Assert.True(modern.Array.Length >= 128);

            // Remove 95 items (now only 5 items left)
            for (int i = 5; i < 100; i++)
            {
                legacy.RemoveAt(i);
                modern.RemoveAt(i);
            }

            Assert.Equal(5, legacy.Count);
            Assert.Equal(5, modern.Count);

            // Legacy has no TrimExcess and retains peak buffer forever
            Assert.True(legacy.Array.Length >= 128);

            // Modern reclaims unused buffer
            bool trimmed = modern.TrimExcess();
            Assert.True(trimmed);
            Assert.True(modern.Array.Length <= 16);
            Assert.Equal(5, modern.Count);
        }
    }
}
