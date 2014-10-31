using System;

namespace Pisc16
{
    public struct Word : IEquatable<Word>, IEquatable<bool[]>
    {
        bool[] bits;

        public Word(int length)
        {
            if (length < 0)
                throw new ArgumentOutOfRangeException();

            bits = new bool[length];
        }

        public int Length
        {
            get { return bits.Length; }
        }

        public bool this[int index]
        {
            get
            {
                if (index < 0 || index >= bits.Length)
                    throw new IndexOutOfRangeException();

                return bits[index];
            }
            set
            {
                if (index < 0 || index >= bits.Length)
                    throw new IndexOutOfRangeException();

                bits[index] = value;
            }
        }

        public bool Equals(Word other)
        {
            return Equals(other.bits);
        }

        public bool Equals(bool[] other)
        {
            if (bits.Length != other.Length)
                return false;

            for (int i = 0; i < other.Length; i++ )
            {
                if (bits[i] != other[i])
                    return false;
            }

            return true;
        }

        public static Word ToWord(int n)
        {
            return default(Word);
        }

        public Word Subbits(int startIndex, int length)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException("startIndex");
            if (length < 0)
                throw new ArgumentOutOfRangeException("length");
            if (startIndex + length >= bits.Length)
                throw new ArgumentOutOfRangeException();

            Word subbits = new Word(length - bits.Length);

            for (int i = 0; i < length; i++)
            {
                subbits[i] = bits[startIndex + i];
            }

            return subbits;
        }

        public Word ExpandLeftUnsigned(int length)
        {
            return ExpandLeftUnsigned(length, false);
        }

        public Word ExpandLeftUnsigned(int length, bool value)
        {
            if (length < 0)
                throw new ArgumentOutOfRangeException("length");
            if (length < bits.Length)
                throw new ArgumentOutOfRangeException("length");

            Word expanded = new Word(length);

            for (int i = 0; i < length; i++)
            {
                expanded[i] = (i - bits.Length - 1 < 0) ? value : bits[i - bits.Length - 1];
            }

            return expanded;
        }

        public Word ExpandLeftSigned(int length)
        {
            if (bits.Length < 2)
                throw new InvalidOperationException();

            return ExpandLeftUnsigned(length, bits[0]);
        }

        public Word Inverted
        {
            get
            {
                Word word = new Word(Length);

                for (int i = 0; i < word.Length; i++)
                {
                    word[i] = !bits[i];
                }

                return word;
            }
        }

        public Word Negative
        {
            get
            {
                Word word = Inverted;
                return word + 1;
            }
        }

        public static Word operator +(Word a, int b)
        {
            return a + Word.ToWord(b);
        }

        public static Word operator +(int a, Word b)
        {
            return Word.ToWord(a) + b;
        }

        public static Word operator +(Word a, Word b)
        {
            if (a.Length != b.Length)
            {
                int length = Math.Max(a.Length, b.Length);

                if (a.Length != length)
                    a = a.ExpandLeftSigned(length);
                else
                    b = b.ExpandLeftSigned(length);
            }

            // summa
            Word c = new Word(a.Length);

            // pārneses bits
            bool carry = false;

            for (int i = c.Length - 1; i >= 0; i--)
            {
                c[i] = a[i] == b[i] ? carry : !carry;
                carry = a[i] == b[i] ? (a[i] && b[i]) : carry;
            }

            return c;
        }

        public static Word operator -(Word a, Word b)
        {
            //return a + b.Negative();
            return default(Word);
        }
    }

    /// <summary>
    /// RISC-16 procesora emulācija.
    /// </summary>
    public class Risc16Cpu2
    {
        public RegisterCollection Registers { get; private set; }
        public MemoryClass Memory { get; private set; }
        
        public bool IsHalted { get; set; }
        public bool[] CurrentAddress { get { return null; } set { } }

        const int wordLength = 16;

        public Risc16Cpu2()
        {
            Registers = new RegisterCollection(8, wordLength);
            Memory = new MemoryClass(0xffff, wordLength);
        }

        public void Reset()
        {
            IsHalted = false;
            CurrentAddress = new bool[wordLength]; // 0000
        }

        public void Run()
        {
            while (!IsHalted)
            {
                NextStep();
            }
        }

        public void NextStep()
        {
            if (IsHalted)
                throw new InvalidOperationException();

            Execute(Memory[CurrentAddress]);
            CurrentAddress = Add(CurrentAddress, Expand(new bool[] { true }, wordLength, false));
        }

        private void Execute(bool[] bits)
        {
            if (bits == null)
                throw new ArgumentNullException("bits");
            if (bits.Length != wordLength)
                throw new ArgumentOutOfRangeException("bits");

            if (!bits[0] && !bits[1] && !bits[2]) // 000
            {
                bool[] regA = Subbits(bits, 3, 3);
                bool[] regB = Subbits(bits, 6, 3);
                bool[] regC = Subbits(bits, 13, 3);
                Add(regA, regB, regC);
            }
            else if (!bits[0] && !bits[1] && bits[2]) // 001
            {
                bool[] regA = Subbits(bits, 3, 3);
                bool[] regB = Subbits(bits, 6, 3);
                bool[] imm = Expand(Subbits(bits, 9, 7), wordLength, true);
                Addi(regA, regB, imm);
            }
            else if (!bits[0] && bits[1] && !bits[2]) // 010
            {
                bool[] regA = Subbits(bits, 3, 3);
                bool[] regB = Subbits(bits, 6, 3);
                bool[] regC = Subbits(bits, 13, 3);
                Nand(regA, regB, regC);
            }
            else if (!bits[0] && bits[1] && bits[2]) // 011
            {
                bool[] regA = Subbits(bits, 3, 3);
                bool[] imm = Subbits(bits, 6, 10);
                Lui(regA, imm);
            }
            else if (bits[0] && !bits[1] && bits[2]) // 101
            {
                bool[] regA = Subbits(bits, 3, 3);
                bool[] regB = Subbits(bits, 6, 3);
                bool[] imm = Expand(Subbits(bits, 9, 7), wordLength, true);
                Sw(regA, regB, imm);
            }
            else if (bits[0] && !bits[1] && !bits[2]) // 100
            {
                bool[] regA = Subbits(bits, 3, 3);
                bool[] regB = Subbits(bits, 6, 3);
                bool[] imm = Expand(Subbits(bits, 9, 7), wordLength, true);
                Lw(regA, regB, imm);
            }
            else if (bits[0] && bits[1] && !bits[2]) // 110
            {
                bool[] regA = Subbits(bits, 3, 3);
                bool[] regB = Subbits(bits, 6, 3);
                bool[] imm = Expand(Subbits(bits, 9, 7), wordLength, true);
                Beq(regA, regB, imm);
            }
            else if (bits[0] && bits[1] && bits[2]) // 111
            {
                bool[] regA = Subbits(bits, 3, 3);
                bool[] regB = Subbits(bits, 6, 3);
                bool[] imm = Subbits(bits, 9, 7);
                Jalr(regA, regB, imm);
            }
            else // never gonna happen
            {
                throw new NotImplementedException();
            }
        }

        #region Opcodes
        private void Add(bool[] regA, bool[] regB, bool[] regC)
        {
            Addi(regA, regB, Registers[regC]);
        }

        private void Addi(bool[] regA, bool[] regB, bool[] imm)
        {
            Registers[regA] = Add(Registers[regB], imm);
        }

        private void Nand(bool[] regA, bool[] regB, bool[] regC)
        {
            for (int i = 0; i < Registers.WordLength; i++)
            {
                Registers[regA][i] = !(Registers[regB][i] && Registers[regC][i]);
            }
        }

        private void Lui(bool[] regA, bool[] imm)
        {
            for (int i = 0; i < imm.Length; i++)
            {
                Registers[regA][i] = imm[i];
            }
        }

        private void Sw(bool[] regA, bool[] regB, bool[] imm)
        {
            bool[] address = Add(Registers[regB], imm);

            Memory[address] = Registers[regA];
        }

        private void Lw(bool[] regA, bool[] regB, bool[] imm)
        {
            bool[] address = Add(Registers[regB], imm);

            Registers[regA] = Memory[address];
        }

        private void Beq(bool[] regA, bool[] regB, bool[] imm)
        {
            bool equal = true;

            for (int i = 0; i < Registers.WordLength; i++)
            {
                if (Registers[regA][i] != Registers[regB][i])
                {
                    equal = false;
                    break;
                }
            }

            if (equal)
                CurrentAddress = Add(CurrentAddress, imm); // iekš Excute +1
        }

        private void Jalr(bool[] regA, bool[] regB, bool[] imm)
        {
            for (int i = 0; i < imm.Length; i++)
            {
                if (imm[i])
                {
                    IsHalted = true;
                    return;
                }
            }

            Registers[regA] = CurrentAddress;
            CurrentAddress = Add(Registers[regB], Expand(new bool[] { true }, 16, false)); // iekš Execute +1
        }
        #endregion Opcodes

        #region Helpers
        /// <summary>
        /// Izveido jaunu bitu virkni no bitu virknes ar noteiktu garumu un sākot no noteiktas vietas.
        /// </summary>
        /// <param name="bits">Bitu virkne.</param>
        /// <param name="startIndex">No kuras vietas sākt iekļaut jaunajā bitu virknē bitus no padotās bitu virknes.</param>
        /// <param name="length">Cik daudz bitu iekļaut jaunajā bitu virknē no padodās bitu virknes.</param>
        /// <returns></returns>
        private static bool[] Subbits(bool[] bits, int startIndex, int length)
        {
            if (bits == null)
                throw new ArgumentNullException("bits");
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException("startIndex");
            if (length < 0)
                throw new ArgumentOutOfRangeException("length");
            if (startIndex + length >= bits.Length)
                throw new ArgumentOutOfRangeException();

            bool[] subbits = new bool[length - bits.Length];

            for (int i = 0; i < length; i++)
            {
                subbits[i] = bits[startIndex + i];
            }

            return subbits;
        }

        /// <summary>
        /// Saskaita divus binārus skaitļus.
        /// Ja rezultātā sanāk garāks skaitlis, vecākais bits tiek atmests.
        /// </summary>
        /// <param name="a">Pirmais skaitlis. Jābūt vienādā garumā ar otro skaitli.</param>
        /// <param name="b">Otrais skaitlis. Jābūt vienādā garumā ar pirmo skaitli.</param>
        /// <returns>Pirmā un otrā skaitļa summa.</returns>
        private static bool[] Add(bool[] a, bool[] b)
        {
            if (a == null)
                throw new ArgumentNullException("a");
            if (b == null)
                throw new ArgumentNullException("b");
            if (a.Length != b.Length)
                throw new ArgumentException();

            // summa
            bool[] c = new bool[a.Length];

            // pārneses bits
            bool carry = false;

            for (int i = c.Length - 1; i >= 0; i--)
            {
                c[i] = a[i] == b[i] ? carry : !carry;
                carry = a[i] == b[i] ? (a[i] && b[i]) : carry;
            }

            return c;
        }

        /// <summary>
        /// Paplašina bitu virkni līdz noteiktam garumam,
        /// ievietojot trūkstošo vecāko bitu vietā 0, ja bitu virknē nav skaitlis ar zīmes bitu,
        /// vai ievieto trūkstošo vecāko bitu vietā zīmes bitu, ja bitu virkne ir skaitlis ar zīmes bitu.
        /// </summary>
        /// <param name="bits">Bitu virkne.</param>
        /// <param name="length">Paplašinātās bitu virknes garums. Jābūt lielākam vai vienādam ar padotās bitu virknes garumu.</param>
        /// <param name="signed">Vai padotā bitu virkne ir skaitlis ar zīmes bitu.</param>
        /// <returns>Jauna paplišināta bitu virkne.</returns>
        private static bool[] Expand(bool[] bits, int length, bool signed)
        {
            if (bits == null)
                throw new ArgumentNullException("bits");
            if (signed && bits.Length < 2)
                throw new ArgumentOutOfRangeException("bits");
            if (length < 0)
                throw new ArgumentOutOfRangeException("length");
            if (length < bits.Length)
                throw new ArgumentOutOfRangeException("length");

            bool[] expanded = new bool[length];

            if (signed)
            {
                for (int i = 1; i < length; i++)
                {
                    expanded[i] = (i - bits.Length - 1 < 0) ? bits[0] : bits[i - bits.Length - 1];
                }
            }
            else
            {
                for (int i = 0; i < length; i++)
                {
                    expanded[i] = (i - bits.Length - 1 < 0) ? false : bits[i - bits.Length - 1];
                }
            }

            return expanded;
        }
        #endregion Helpers

        /// <summary>
        /// Reģistru klase ar noteiktu skaitu reģistru, kuriem ir vienāds vārda garums.
        /// Nultais reģistrs vienmēr atgriež nulle.
        /// </summary>
        public class RegisterCollection
        {
            readonly int count;
            readonly int wordLength;

            bool[][] registers;

            public RegisterCollection(int count, int wordLength)
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

            public bool[] this[bool[] index]
            {
                get { return null; }
                set { }
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

        /// <summary>
        /// Atmiņa, kurā katrs vārds ir ar noteiktu un nemainīgu garumu.
        /// </summary>
        public class MemoryClass
        {
            readonly int size;
            readonly int wordLength;

            bool[][] memory;

            public MemoryClass(int size, int wordLength)
            {
                this.size = size + 1;
                this.wordLength = wordLength;

                memory = new bool[size + 1][];
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

            public bool[] this[bool[] index]
            {
                get { return null; }
                set { }
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
}
