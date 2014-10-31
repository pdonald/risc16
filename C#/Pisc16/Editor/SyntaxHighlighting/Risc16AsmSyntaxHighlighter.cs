using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;

namespace Pisc16
{
    public class Risc16AsmSyntaxHighlighter : ISyntaxHighlighter
    {
        static readonly string[] opcodes = { "add", "addi", "nand", "lui", "sw", "lw", "beq", "jalr",
                                             "nop", "halt", "lli", "movi", ".fill", ".space", ".org" };

        public SyntaxHighlighterResult[] Highlight(string text, int startPosition, int length)
        {
            List<SyntaxHighlighterResult> highlights = new List<SyntaxHighlighterResult>();

            for (int i = startPosition, start = startPosition; i < startPosition + length; i++)
            {
                if (text[i] == '\n' || i == startPosition + length - 1)
                {
                    var hh = Highlight(text.Substring(start, i - start + 1));
                    hh.ForEach((h) => h.Start += start);

                    highlights.AddRange(hh);
                    start = i + 1;
                }
            }

            return highlights.ToArray();
        }

        private List<SyntaxHighlighterResult> Highlight(string line)
        {
            line = line.TrimEnd().ToLower();

            System.Diagnostics.Debug.WriteLine(line);
            List<SyntaxHighlighterResult> highlights = new List<SyntaxHighlighterResult>();

            foreach (Match token in new Regex("([^ \t,#:]+)").Matches(line))
            {
                SyntaxHighlighterResult highlight = new SyntaxHighlighterResult
                {
                    Start = token.Index,
                    Length = token.Length
                };

                if (token.Index + token.Length < line.Length && line[token.Index + token.Length] == ':')
                {
                    highlight.Color = Color.Teal;
                    highlights.Add(highlight);
                    continue;
                }

                if (IsNumeral(token.Value) && token.Value != "add")
                {
                    highlight.Color = Color.Red;
                    highlights.Add(highlight);
                    break;
                }

                if (IsRegister(token.Value))
                {
                    highlight.Color = Color.Orange;
                    highlights.Add(highlight);
                    continue;
                }

                foreach (string opcode in opcodes)
                {
                    if (token.Value == opcode)
                    {
                        highlight.Color = Color.Navy;
                        highlights.Add(highlight);
                        break;
                    }
                }
            }

            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == ',' || line[i] == ':' || line[i] == '.')
                {
                    highlights.Add(new SyntaxHighlighterResult {
                        Start = i,
                        Length = 1,
                        Color = Color.Gray
                    });
                }
            }

            if (line.Contains("#"))
            {
                highlights.Add(new SyntaxHighlighterResult {
                    Start = line.IndexOf('#'),
                    Length = line.Length - line.IndexOf('#'),
                    Color = Color.Green
                });
            }

            return highlights;
        }

        private static bool IsRegister(string token)
        {
            if (token.StartsWith("R", StringComparison.InvariantCultureIgnoreCase))
            {
                if (token.Length > 1)
                {
                    for (int i = 1; i < token.Length; i++)
                    {
                        if (!char.IsDigit(token[i]))
                            return false;
                    }
                    return true;
                }
                return false;
            }
            return false;
        }

        private static bool IsNumeral(string token)
        {
            token = token.ToLower();

            for (int i = 0; i < token.Length; i++)
            {
                if (token[i] == '-')
                {
                    if (i > 0)
                        return false;
                }
                else
                {
                    if (!(char.IsDigit(token[i]) || (token[i] >= 'a' && token[i] <= 'f')))
                        return false;
                }
            }

            return true;
        }
    }
}
