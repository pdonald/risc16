using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Pisc16
{
    public class AsmToOpcodeTranslator2
    {
        static readonly char[] whitespace = { ' ', '\t' };

        public bool[][] Translate(string[] lines, int memorySize)
        {
            Dictionary<string, int> labels = ParseLabels(lines);

            bool[][] memory = new bool[memorySize][];

            for (int i = 0, address = 0; i < lines.Length; i++)
            {
                string line = lines[i].ToUpper();
                
                line = line.Trim(whitespace);
                line = StripComments(line);
                line = StripLabels(line);
                line = line.Trim(whitespace);

                string instruction = ParseInstruction(line);
                string @params = line.Substring(line.IndexOf(instruction) + instruction.Length);

                if (instruction == "")
                    continue;

                switch (instruction)
                {
                    case "ADD":
                        memory[address] = ParseParamsWithThreeRegisters("ADD", @params);
                        break;

                    case "ADDI":
                        memory[address] = ParseParamsWithTwoRegisters("ADDI", @params);
                        memory[address][2] = true; // 001
                        break;

                    case "NAND":
                        memory[address] = ParseParamsWithThreeRegisters("NAND", @params);
                        memory[address][1] = true; // 010
                        break;

                    case "LUI":
                        memory[address] = ParseParamsWithOneRegister("LUI", @params);
                        memory[address][1] = memory[address][2] = true; // 011
                        break;

                    case "LW":
                        memory[address] = ParseParamsWithTwoRegisters("LW", @params);
                        memory[address][0] = true; // 100
                        break;

                    case "SW":
                        memory[address] = ParseParamsWithTwoRegisters("SW", @params);
                        memory[address][0] = memory[address][2] = true; // 101
                        break;

                    case "BEQ":
                        memory[address] = ParseParamsWithTwoRegisters("BEQ", @params);
                        memory[address][0] = memory[address][1] = true; // 110
                        break;

                    case "JALR":
                        memory[address] = ParseParamsWithTwoRegisters("JALR", @params);
                        memory[address][0] = memory[address][1] = memory[address][2] = true; // 111
                        break;

                    case "NOP":
                        @params = "R0, R0, R0";
                        goto case "ADD";

                    case "HALT":
                        @params = "R0, R0, 1";
                        goto case "JALR";

                    case "LLI":
                        break;

                    case "MOVI":
                        break;

                    case ".FILL":
                        break;

                    case ".SPACE":
                        break;

                    case ".ORG":
                        address = ParseImmediate(".ORG", 1, @params, out @params) - 1; // jo cikla beigās address++
                        break;

                    case ".halt":
                    case ".lli":
                    case ".movi":
                        // did you mean?
                        break;

                    case "fill":
                    case "space":
                    case "org":
                        // did you mean?
                        break;

                    default:
                        throw new TranslatorException("?", i, "Neatpazīta komanda (" + instruction + ").");
                }

                address++;
            }

            return memory;
        }

        private Dictionary<string, int> ParseLabels(string[] lines)
        {
            Dictionary<string, int> labels = new Dictionary<string, int>();

            for (int i = 0, address = 0; i < lines.Length; i++, address++)
            {
                string line = lines[i].ToUpper();

                line = line.Trim(whitespace);
                line = StripComments(line);
                line = StripLabels(line);
                line = line.Trim(whitespace);

                string instruction = ParseInstruction(line);
                string @params = line.Substring(line.IndexOf(instruction) + instruction.Length);

                if (instruction == ".ORG")
                    address = ParseImmediate(".ORG", 1, @params, out @params) - 1; // jo cikla beigās address++

                if (lines[i].Contains(":"))
                {
                    string label = lines[i].Substring(0, lines[i].IndexOf(':')).TrimStart(whitespace);

                    if (!IsValidLabel(label))
                        throw new TranslatorException("Iezīmes", i, "Iezīme '" + labels + "' nav derīga, jo drīkst saturēt tikai latīņu burtus un ciparus");

                    if (labels.ContainsKey(label))
                        throw new TranslatorException("Iezīmes", i, "Iezīme '" + labels + "' parādās vairākkārt (" + (labels[label] + 1) + ". un " + (i + 1) + ". rindā)");
                    
                    labels[label] = i;
                }
            }

            return labels;
        }

        private bool IsValidLabel(string label)
        {
            for (int i = 0; i < label.Length; i++)
            {
                if (!char.IsLetterOrDigit(label[i]))
                {
                    return false;
                }
            }
            return true;
        }

        private string StripLabels(string line)
        {
            if (line.Contains(":"))
                return line.Remove(0, line.IndexOf(':') + 1);
            return line;
        }

        private string StripComments(string line)
        {
            if (line.Contains("#"))
                return line.Remove(line.IndexOf('#'));
            return line;
        }

        private string ParseInstruction(string line)
        {
            line = line.Trim(whitespace);
            string instruction = "";

            for (int i = 0; i < line.Length; i++)
            {
                for (int j = 0; j < whitespace.Length; j++)
                {
                    if (line[i] == whitespace[j])
                    {
                        return instruction;
                    }
                }
                instruction += line[i];
            }

            return instruction;
        }

        private bool[] ParseParamsWithThreeRegisters(string command, string @params)
        {
            bool[] word = new bool[16];

            int regA = ParseRegister(command, 1, @params, out @params);
            int regB = ParseRegister(command, 2, @params, out @params);
            int regC = ParseRegister(command, 3, @params, out @params);

            ParseLineEnding(command, @params);

            RegToBinary(regA, word, 3);
            RegToBinary(regB, word, 6);
            RegToBinary(regC, word, 13);

            return word;
        }

        private bool[] ParseParamsWithTwoRegisters(string command, string @params)
        {
            bool[] word = new bool[16];

            int regA = ParseRegister(command, 1, @params, out @params);
            int regB = ParseRegister(command, 2, @params, out @params);
            int imm = ParseImmediate(command, 3, @params, out @params);

            //if (imm < -64 || imm > 63)
              //  throw new TranslatorException(command, -1, "3. parametra skaitlis (" + imm + ", 0x" + imm.ToString("X") + ") ir par lielu vai mazu. Tam ir jābūt robežās -64 (7F) līdz 63 (3F).");

            ParseLineEnding(command, @params);

            RegToBinary(regA, word, 3);
            RegToBinary(regB, word, 6);
            SignedImmediateToBinary(imm, word, 9, 7);

            return word;
        }

        private bool[] ParseParamsWithOneRegister(string command, string @params)
        {
            bool[] word = new bool[16];

            int regA = ParseRegister(command, 1, @params, out @params);
            int imm = ParseImmediate(command, 2, @params, out @params);

            if (imm < 0x0 || imm > 0x3ff)
                throw new TranslatorException(command, -1, "2. parametra skaitlis ir par lielu vai mazu");

            ParseLineEnding(command, @params);

            RegToBinary(regA, word, 3);
            UnsignedImmediateToBinary(imm, word, 6, 10);

            return word;
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

        private int ParseImmediate(string command, int param, string aparams, out string @params)
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

        private void ParseLineEnding(string command, string @params)
        {
            @params = @params.Trim(whitespace);

            if (@params != "")
                throw new TranslatorException(command, -1, "Komandas beigās ir papildus simboli (" + @params + "), kuri nav vajadzīgi.");
        }

        private void RegToBinary(int reg, bool[] word, int offset)
        {
            UnsignedImmediateToBinary(reg, word, offset, 3);
        }

        private void SignedImmediateToBinary(int n, bool[] word, int offset, int length)
        {
            if (n < 0)
                n += (int)Math.Pow(2, length);

            UnsignedImmediateToBinary(n, word, offset, length);
        }

        private void UnsignedImmediateToBinary(int n, bool[] word, int offset, int length)
        {
            string binary = Convert.ToString(n, 2).PadLeft(length, '0');

            for (int i = 0; i < binary.Length; i++)
                word[i + offset] = binary[i] == '1';
        }
    }
}
