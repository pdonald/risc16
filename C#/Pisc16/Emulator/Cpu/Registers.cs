using System;

namespace Pisc16
{
    public interface IRegisterCollection
    {
        bool[] this[int index] { get; set; }
        int Count { get; }
        int WordLength { get; }
    }

    /// <summary>
    /// Reģistru klase ar noteiktu skaitu reģistru, kuriem ir vienāds vārda garums.
    /// Nultais reģistrs vienmēr atgriež nulle.
    /// </summary>
    public class FixedWordLengthZeroBasedRegisterCollection : IRegisterCollection
    {
        readonly int count;
        readonly int wordLength;

        bool[][] registers;

        public FixedWordLengthZeroBasedRegisterCollection(int count, int wordLength)
        {
            if (count < 1)
                throw new ArgumentOutOfRangeException("count");
            if (wordLength < 0)
                throw new ArgumentOutOfRangeException("wordLength");
            
            this.count = count;
            this.wordLength = wordLength;

            registers = new bool[count][];

            for (int i = 1; i < registers.Length; i++)
                registers[i] = new bool[wordLength];
        }

        public bool[] this[int index]
        {
            get
            {
                if (index < 0 || index >= count)
                    throw new IndexOutOfRangeException();

                if (index == 0)
                    return new bool[wordLength];

                return registers[index];
            }
            set
            {
                if (index < 0 || index >= count)
                    throw new IndexOutOfRangeException();

                if (index != 0)
                {
                    if (value == null)
                        throw new ArgumentNullException();
                    if (value.Length != wordLength)
                        throw new ArgumentException();

                    registers[index] = value;
                }
            }
        }

        public int Count
        {
            get { return count; }
        }

        public int WordLength
        {
            get { return wordLength; }
        }
    }
}
