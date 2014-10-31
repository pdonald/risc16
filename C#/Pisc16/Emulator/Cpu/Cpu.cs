using System;

namespace Pisc16
{
    /// <summary>
    /// RISC-16 procesora emulācija.
    /// </summary>
    public class Risc16Cpu
    {
        public IRegisterCollection Registers { get; private set; }
        public FixedWordLengthMemory Memory { get; private set; }
        
        public bool IsFinished { get { return CurrentStep >= Memory.Size; } }
        public int CurrentStep { get; private set; }

        public Risc16Cpu()
        {
            Registers = new FixedWordLengthZeroBasedRegisterCollection(8, 16);
            Memory = new FixedWordLengthMemory(0xffff + 1, 16);
        }

        public void Reset()
        {
            CurrentStep = 0;
        }

        public void Run()
        {
            CurrentStep = 0;

            while (!IsFinished)
            {
                NextStep();
            }
        }

        public void NextStep()
        {
            if (IsFinished)
                throw new InvalidOperationException();

            Execute(CurrentStep);
        }

        private void Execute(int line)
        {
            bool[] opcode = Memory[line];

            if (!opcode[0] && !opcode[1] && !opcode[2]) // 000
            {
                int regA = BinaryToInt(opcode, 3, 3);
                int regB = BinaryToInt(opcode, 6, 3);
                int regC = BinaryToInt(opcode, 13, 3);
                Add(regA, regB, regC);
            }
            else if (!opcode[0] && !opcode[1] && opcode[2]) // 001
            {
                int regA = BinaryToInt(opcode, 3, 3);
                int regB = BinaryToInt(opcode, 6, 3);
                bool[] imm = SignedNumber(opcode, 9);
                Addi(regA, regB, imm);
            }
            else if (!opcode[0] && opcode[1] && !opcode[2]) // 010
            {
                int regA = BinaryToInt(opcode, 3, 3);
                int regB = BinaryToInt(opcode, 6, 3);
                int regC = BinaryToInt(opcode, 13, 3);
                Nand(regA, regB, regC);
            }
            else if (!opcode[0] && opcode[1] && opcode[2]) // 011
            {
                int regA = BinaryToInt(opcode, 3, 3);
                bool[] imm = UnsignedNumber(opcode, 6);
                Lui(regA, imm);
            }
            else if (opcode[0] && !opcode[1] && opcode[2]) // 101
            {
                int regA = BinaryToInt(opcode, 3, 3);
                int regB = BinaryToInt(opcode, 6, 3);
                bool[] imm = SignedNumber(opcode, 9);
                Sw(regA, regB, imm);
            }
            else if (opcode[0] && !opcode[1] && !opcode[2]) // 100
            {
                int regA = BinaryToInt(opcode, 3, 3);
                int regB = BinaryToInt(opcode, 6, 3);
                bool[] imm = SignedNumber(opcode, 9);
                Lw(regA, regB, imm);
            }
            else if (opcode[0] && opcode[1] && !opcode[2]) // 110
            {
                int regA = BinaryToInt(opcode, 3, 3);
                int regB = BinaryToInt(opcode, 6, 3);
                bool[] imm = SignedNumber(opcode, 9);
                Beq(regA, regB, imm);
            }
            else if (opcode[0] && opcode[1] && opcode[2]) // 111
            {
                int regA = BinaryToInt(opcode, 3, 3);
                int regB = BinaryToInt(opcode, 6, 3);
                bool[] imm = UnsignedNumber(opcode, 9);
                Jalr(regA, regB, imm);
            }

            CurrentStep++;
        }

        private void Add(int regA, int regB, int regC)
        {
            Addi(regA, regB, Registers[regC]);
        }

        private void Addi(int regA, int regB, bool[] imm)
        {
            bool carry = false;

            for (int i = Registers.WordLength - 1; i >= 0; i--)
            {
                bool a = Registers[regB][i];
                bool b = imm[i];
                bool c;

                c = a == b ? carry : !carry;
                carry = a == b ? (a && b) : carry;

                Registers[regA][i] = c;
            }
        }

        private void Nand(int regA, int regB, int regC)
        {
            for (int i = 0; i < Registers.WordLength; i++)
            {
                Registers[regA][i] = !(Registers[regB][i] && Registers[regC][i]);
            }
        }

        private void Lui(int regA, bool[] imm)
        {
            for (int i = 0; i < imm.Length; i++)
            {
                Registers[regA][i] = imm[i];
            }
        }

        private void Sw(int regA, int regB, bool[] imm)
        {
            int address = Registers[regB].ToUnsignedInt32() + imm.ToInt32();

            if (Registers.WordLength != Memory.WordLength)
                throw new NotSupportedException();

            Memory[address] = Registers[regA];
        }

        private void Lw(int regA, int regB, bool[] imm)
        {
            int address = Registers[regB].ToUnsignedInt32() + imm.ToInt32();
            // TODO: add()

            if (Registers.WordLength != Memory.WordLength)
                throw new NotSupportedException();

            Registers[regA] = Memory[address];
        }

        private void Beq(int regA, int regB, bool[] imm)
        {
            bool equal = true;

            for (int i = 0; i < Registers.WordLength; i++)
            {
                for (int j = 0; j < Registers.WordLength; j++)
                {
                    if (Registers[regA][i] != Registers[regB][i])
                    {
                        equal = false;
                        break;
                    }
                }
            }

            if (equal)
                CurrentStep += imm.ToInt32(); // iekš Excute +1
        }

        private void Jalr(int regA, int regB, bool[] imm)
        {
            for (int i = 0; i < imm.Length; i++)
            {
                if (imm[i])
                {
                    CurrentStep = Memory.Size;
                    return;
                }
            }

            Registers[regA] = IntToBinary(CurrentStep + 1, 16);
            CurrentStep = Registers[regB].ToInt32() + 1; // iekš Execute +1
        }

        private int BinaryToInt(bool[] num, int offset, int count)
        {
            bool[] num2 = new bool[count + 1];
            num2[0] = false;

            for (int i = 0; i < count; i++)
                num2[i + 1] = num[offset + i];

            return num2.ToInt32();
        }

        private bool[] SignedNumber(bool[] source, int offset)
        {
            bool[] sub = new bool[source.Length];

            for (int i = 0; i < source.Length; i++)
            {
                if (i < offset)
                    sub[i] = source[offset];
                else
                    sub[i] = source[i];
            }

            return sub;
        }

        private bool[] UnsignedNumber(bool[] source, int offset)
        {
            bool[] sub = new bool[source.Length - offset];

            for (int i = 0; i < source.Length - offset; i++)
            {
                sub[i] = source[i + offset];
            }

            return sub;
        }

        private bool[] IntToBinary(int n, int length)
        {
            if (n < 0)
            {
                n += (int)Math.Pow(2, length);
            }

            string binary = Convert.ToString(n, 2).PadLeft(length, '0');
            bool[] bits = new bool[length];

            for (int i = 0; i < binary.Length; i++)
                bits[i] = binary[i] == '1';

            return bits;
        }
    }
}
