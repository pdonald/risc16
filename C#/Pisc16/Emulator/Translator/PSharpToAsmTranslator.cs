using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Pisc16
{
    // mainīgie
    // aritmētiskās darbības
    // funkcijas
    // cikli
    // daļskaitļi, simboli
    public class PSharpToAsmTranslator
    {
        const int variableBaseAddress = 0x3d;
        Dictionary<string, int> variables = new Dictionary<string, int>();

        Regex variableDecleration = new Regex(@"^\s*var\s*(?<Name>[a-z]+)\s*$", RegexOptions.IgnoreCase);
        Regex variableValueAssignment = new Regex(@"^\s*(?<Variable>[a-z]+)\s*=\s*(?<Value>[0-9]+)\s*$", RegexOptions.IgnoreCase);
        Regex variableValueDisplay = new Regex(@"^\s*echo\s*(?<Variable>[a-z]+)\s*$", RegexOptions.IgnoreCase);
        Regex sum = new Regex(@"^\s*(?<Result>[a-z]+)\s*=\s*(?<A>[a-z]+|[0-9]+)\s*\+\s*(?<B>[a-z]+|[0-9]+)\s*$", RegexOptions.IgnoreCase);

        public string[] Translate(string[] lines)
        {
            List<string> asm = new List<string>();

            foreach (string line in lines)
            {
                if (variableDecleration.IsMatch(line))
                {
                    Match match = variableDecleration.Match(line);
                    string name = match.Groups["Name"].Value;
                    int address = variableBaseAddress + (variables.Count > 0 ? variables.Select(v => v.Value).Max() + 1 : 0);
                    variables.Add(name, address);

                    asm.AddRange(LoadNumber(1, address));
                    asm.Add("SW R0, R1, 0");
                }

                if (variableValueAssignment.IsMatch(line))
                {
                    Match match = variableValueAssignment.Match(line);
                    string variable = match.Groups["Variable"].Value;
                    int value = int.Parse(match.Groups["Value"].Value);

                    if (!variables.ContainsKey(variable))
                        throw new InvalidOperationException("Undeclared variable " + variable);

                    asm.AddRange(LoadNumber(1, variables[variable]));
                    asm.AddRange(LoadNumber(2, value));
                    asm.Add("SW R2, R1, 0");
                }

                if (variableValueDisplay.IsMatch(line))
                {
                    Match match = variableValueDisplay.Match(line);
                    string variable = match.Groups["Variable"].Value;

                    if (!variables.ContainsKey(variable))
                        throw new InvalidOperationException("Undeclared variable " + variable);

                    asm.AddRange(LoadNumber(1, variables[variable]));
                    asm.Add("LW R2, R1, 0"); // lui + lw
                    asm.Add("# " + variable + " = R2");
                }

                if (sum.IsMatch(line))
                {
                    Match match = sum.Match(line);
                    string result = match.Groups["Result"].Value;
                    string a = match.Groups["A"].Value;
                    string b = match.Groups["B"].Value;

                    if (!variables.ContainsKey(result))
                        throw new InvalidOperationException("Undeclared variable " + result);

                    if (char.IsDigit(a[0]))
                    {
                        asm.AddRange(LoadNumber(1, int.Parse(a)));
                    }
                    else
                    {
                        asm.AddRange(LoadNumber(2, variables[a]));
                        asm.Add("LW R1, R2, 0");
                    }

                    if (char.IsDigit(b[0]))
                    {
                        asm.AddRange(LoadNumber(2, int.Parse(b)));
                    }
                    else
                    {
                        asm.AddRange(LoadNumber(3, variables[b]));
                        asm.Add("LW R2, R3, 0");
                    }

                    asm.Add("ADD R1, R1, R2");
                    asm.AddRange(LoadNumber(2, variables[result]));
                    asm.Add("SW R1, R2, 0");
                }
            }

            return asm.ToArray();
        }

        private string[] LoadNumber(int reg, int n)
        {
            return new string[] { "MOVI R" + reg + ", " + n.ToString("X") };
        }
    }
}
