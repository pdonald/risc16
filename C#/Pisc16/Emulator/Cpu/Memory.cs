using System;

namespace Pisc16
{
    /// <summary>
    /// Atmiņa, kurā katrs vārds ir ar noteiktu un nemainīgu garumu.
    /// </summary>
    public class FixedWordLengthMemory
    {
        readonly int size;
        readonly int wordLength;

        bool[][] memory;

        public FixedWordLengthMemory(int size, int wordLength)
        {
            this.size = size;
            this.wordLength = wordLength;

            memory = new bool[size][];
        }

        public bool[] this[int index]
        {
            get
            {
                if (memory[index] == null)
                    memory[index] = new bool[wordLength];

                return memory[index];
            }
            set
            {
                if (value != null && value.Length != wordLength)
                    throw new ArgumentException();

                memory[index] = value;
            }
        }

        public int Size
        {
            get { return size; }
        }

        public int WordLength
        {
            get { return wordLength; }
        }

        public void Load(bool[][] dump)
        {
            for (int i = 0; i < dump.Length; i++)
                this[i] = dump[i];
        }

        public void Clear()
        {
            for (int i = 0; i < Size; i++)
                memory[i] = null;
        }
    }
}
