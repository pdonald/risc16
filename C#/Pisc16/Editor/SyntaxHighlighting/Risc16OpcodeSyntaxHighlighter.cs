using System.Collections.Generic;
using System.Drawing;

namespace Pisc16
{
    public class Risc16OpcodeSyntaxHighlighter : ISyntaxHighlighter
    {
        public Color Instruction { get; set; }
        public Color RegisterA { get; set; }
        public Color RegisterB { get; set; }
        public Color RegisterC { get; set; }
        public Color EmptyBits { get; set; }
        public Color Immediate { get; set; }

        public Risc16OpcodeSyntaxHighlighter()
        {
            Instruction = Color.Navy;
            RegisterA = Color.Orange;
            RegisterB = Color.FromArgb(RegisterA.R, RegisterA.G - 50, RegisterA.B);
            RegisterC = Color.FromArgb(RegisterB.R, RegisterB.G - 20, RegisterB.B);
            EmptyBits = Color.Gray;
            Immediate = Color.Red;
        }

        public SyntaxHighlighterResult[] Highlight(string text, int startPosition, int length)
        {
            List<SyntaxHighlighterResult> highlights = new List<SyntaxHighlighterResult>();

            for (int i = startPosition, start = startPosition; i < startPosition + length; i++)
            {
                if (text[i] == '\n' || i == startPosition + length - 1)
                {
                    var hh = Highlight(text.Substring(start, i - start + 1));
                    
                    if (hh != null)
                    {
                        hh.ForEach((h) => h.Start += start);
                        highlights.AddRange(hh);
                    }

                    start = i + 1;
                }
            }

            return highlights.ToArray();
        }

        private List<SyntaxHighlighterResult> Highlight(string line)
        {
            line = line.TrimEnd();

            if (line.Length != 16)
                return null;

            for (int i = 0; i < line.Length; i++)
                if (!(line[i] == '0' || line[i] == '1'))
                    return null;

            List<SyntaxHighlighterResult> highlights = new List<SyntaxHighlighterResult>();

            // opcode
            highlights.Add(new SyntaxHighlighterResult(0, 3, Instruction));

            string opcode = line.Substring(0, 3);

            switch (opcode)
            {
                case "000":
                case "010":
                    highlights.Add(new SyntaxHighlighterResult(3, 3, RegisterA));
                    highlights.Add(new SyntaxHighlighterResult(6, 3, RegisterB));
                    highlights.Add(new SyntaxHighlighterResult(9, 4, EmptyBits));
                    highlights.Add(new SyntaxHighlighterResult(13, 3, RegisterC));
                    break;
                case "001":
                case "100":
                case "101":
                case "110":
                    highlights.Add(new SyntaxHighlighterResult(3, 3, RegisterA));
                    highlights.Add(new SyntaxHighlighterResult(6, 3, RegisterB));
                    highlights.Add(new SyntaxHighlighterResult(9, 7, Immediate));
                    break;
                case "011":
                    highlights.Add(new SyntaxHighlighterResult(3, 3, RegisterA));
                    highlights.Add(new SyntaxHighlighterResult(6, 10, Immediate));
                    break;
                case "111":
                    highlights.Add(new SyntaxHighlighterResult(3, 3, RegisterA));
                    highlights.Add(new SyntaxHighlighterResult(6, 3, RegisterB));
                    highlights.Add(new SyntaxHighlighterResult(9, 7, EmptyBits));

                    for (int i = 9; i < 16; i++)
                        if (line[i] == '1')
                            highlights.Add(new SyntaxHighlighterResult(i, 1, Immediate));

                    break;
            }

            return highlights;
        }
    }
}
