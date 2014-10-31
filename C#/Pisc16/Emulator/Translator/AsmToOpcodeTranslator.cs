using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pisc16
{
    public class AsmToOpcodeTranslator
    {
        public bool[][] Translate(string[] lines)
        {
            bool[][] opcode = new bool[lines.Length][];

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i].Trim();
                string label = "";

                if (line.Contains(":"))
                {
                    label = line.Substring(0, line.IndexOf('0')).Trim();
                    line = line.Remove(0, label.Length);
                }

                // komentāri

                opcode[i] = new bool[16];
                line = line.ToUpper();

                if (line.StartsWith("ADDI "))
                {
                    opcode[i] = ParseParamsWithTwoRegisters("ADDI", line.Substring(5).Trim());
                    opcode[i][2] = true; // 001
                }
                else if (line.StartsWith("ADD "))
                {
                    opcode[i] = ParseParamsWithThreeRegisters("ADD", line.Substring(4).Trim());
                    // 000
                }
                else if (line.StartsWith("NAND "))
                {
                    opcode[i] = ParseParamsWithThreeRegisters("NAND", line.Substring(5).Trim());
                    opcode[i][1] = true; // 010
                }
                else if (line.StartsWith("LUI "))
                {
                    opcode[i] = ParseParamsWithOneRegister("LUI", line.Substring(4).Trim());
                    opcode[i][1] = opcode[i][2] = true; // 011
                }
                else if (line.StartsWith("LW "))
                {
                    opcode[i] = ParseParamsWithTwoRegisters("LW", line.Substring(3).Trim());
                    opcode[i][0] = true; // 100
                }
                else if (line.StartsWith("SW "))
                {
                    opcode[i] = ParseParamsWithTwoRegisters("SW", line.Substring(3).Trim());
                    opcode[i][0] = opcode[i][2] = true; // 101
                }
                else if (line.StartsWith("BEQ "))
                {
                    opcode[i] = ParseParamsWithTwoRegisters("BEQ", line.Substring(4).Trim());
                    opcode[i][0] = opcode[i][1] = true; // 110
                }
                else if (line.StartsWith("JALR "))
                {
                    opcode[i] = ParseParamsWithTwoRegisters("JALR", line.Substring(4).Trim());
                    opcode[i][0] = opcode[i][1] = opcode[i][2] = true; // 110
                }
                else
                {
                    if (line.Trim() == "")
                        continue;

                    throw new TranslatorException("Unknown", i, "Neatpazīta komanda");
                }
            }

            return opcode;
        }

        private bool[] ParseParamsWithThreeRegisters(string command, string line)
        {
            bool[] word = new bool[16];

            int regA = ParseRegister(command, 1, line, out line);
            int regB = ParseRegister(command, 2, line, out line);
            int regC = ParseRegister(command, 3, line, out line);

            CheckLineEnding(command, line);

            RegToBinary(regA, word, 3);
            RegToBinary(regB, word, 6);
            RegToBinary(regC, word, 13);

            return word;
        }

        private bool[] ParseParamsWithTwoRegisters(string command, string line)
        {
            bool[] word = new bool[16];

            int regA = ParseRegister(command, 1, line, out line);
            int regB = ParseRegister(command, 2, line, out line);
            int imm = ParseNumber(command, 3, line, out line);

            //if (imm < -64 || imm > 63)
                //throw new TranslatorException(command, -1, "3. parametra skaitlis ir par lielu vai mazu");

            CheckLineEnding(command, line);

            RegToBinary(regA, word, 3);
            RegToBinary(regB, word, 6);
            NumToBinary(imm, word, 9, 7);

            return word;
        }

        private bool[] ParseParamsWithOneRegister(string command, string line)
        {
            bool[] word = new bool[16];

            int regA = ParseRegister(command, 1, line, out line);
            int imm = ParseNumber(command, 2, line, out line);

            if (imm < 0x0 || imm > 0x3ff)
                throw new TranslatorException(command, -1, "2. parametra skaitlis ir par lielu vai mazu");

            CheckLineEnding(command, line);

            RegToBinary(regA, word, 3);
            UnsignedNumberToBinary(imm, word, 6, 10);

            return word;
        }

        private void CheckLineEnding(string command, string @params)
        {
            if (@params.Trim() != "")
                throw new TranslatorException(command, -1, "Komandas beigās ir papildus simboli, kuri nav vajadzīgi");
        }

        private int ParseRegister(string command, int param, string aparams, out string @params)
        {
            // TODO cmd r1 ,, r2 ,, r3
            @params = aparams.Trim().Trim(',').Trim();

            if (!@params.StartsWith("R"))
                throw new TranslatorException(command, -1, param + ". parametram jābūt reģistram formātā Rx, šeit trūkst R burta");

            int i = 1; while (i < @params.Length && char.IsDigit(@params[i])) i++;

            if (i == 1)
                throw new TranslatorException(command, -1, param + ". parametram jābūt reģistram formātā Rx, trūkst x, kas ir reģistra numurs");

            int reg = int.Parse(@params.Substring(1, i - 1));

            if (reg < 0 || reg > 7)
                throw new TranslatorException(command, -1, param + ". parametram jābūt reģistram formātā Rx, kur x >= un x <= 7, šeit x ir ārpus apgabala");

            @params = @params.Substring(i);

            return reg;
        }

        private int ParseNumber(string command, int param, string aparams, out string @params)
        {
            @params = aparams.Trim().Trim(',').Trim();

            int length = 0;

            for (int i = 0; i < @params.Length && (char.IsDigit(@params[i]) || char.IsLetter(@params[i]) || (i == 0 && @params[i] == '-')); i++)
                length++;

            if (length == 0)
                throw new TranslatorException(command, -1, param + ". parametram jābūt skaitlim heksadecimālā formātā, šeit tādu nemana");

            int n;

            try
            {
                n = int.Parse(@params.Substring((@params[0] == '-') ? 1 : 0, (@params[0] == '-') ? length - 1 : length), System.Globalization.NumberStyles.HexNumber);
                n *= (@params[0] == '-') ? -1 : 1;

            }
            catch (FormatException)
            {
                throw new TranslatorException(command, -1, param + ". parametram jābūt skaitlim heksadecimālā formātā, šeit tas nav heksadecimāls skaitlis");
            }

            @params = @params.Substring(length);

            return n;
        }

        private void RegToBinary(int reg, bool[] word, int offset)
        {
            string binary = Convert.ToString(reg, 2).PadLeft(3, '0');

            for (int i = 0; i < binary.Length; i++)
                word[i + offset] = binary[i] == '1';
        }

        private void NumToBinary(int n, bool[] word, int offset, int length)
        {
            if (n < 0)
            {
                n += (int)Math.Pow(2, length);
            }

            string binary = Convert.ToString(n, 2).PadLeft(length, '0');

            for (int i = 0; i < binary.Length; i++)
                word[i + offset] = binary[i] == '1';
        }

        private void UnsignedNumberToBinary(int n, bool[] word, int offset, int length)
        {
            string binary = Convert.ToString(n, 2).PadLeft(length, '0');

            for (int i = 0; i < binary.Length; i++)
                word[i + offset] = binary[i] == '1';

            System.Diagnostics.Debug.WriteLine(binary);
        }
    }
}
