using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Pisc16
{
    public class SyntaxHighlighterResult
    {
        public SyntaxHighlighterResult()
        {
        }

        public SyntaxHighlighterResult(int start, int length, Color color) : this(start, length, color, null)
        {
        }

        public SyntaxHighlighterResult(int start, int length, Color color, Font font) : this()
        {
            Start = start;
            Length = length;
            Color = color;
            Font = font;
        }

        public int Start { get; set; }
        public int Length { get; set; }
        public Color Color { get; set; }
        public Font Font { get; set; }
    }

    public interface ISyntaxHighlighter
    {
        SyntaxHighlighterResult[] Highlight(string text, int startIndex, int length);
    }

    public class SyntaxHighlightedRichTextBox : RichTextBox
    {
        ISyntaxHighlighter syntaxHighlighter;
        bool pausePainting;

        public SyntaxHighlightedRichTextBox() : base()
        {
            this.AcceptsTab = true;
            this.WordWrap = false;
            this.DetectUrls = false;
            this.TextChanged += this.HighlightCurrentLine;
        }

        public SyntaxHighlightedRichTextBox(ISyntaxHighlighter syntaxHiglighter) : this()
        {
            this.SyntaxHighlighter = syntaxHiglighter;
        }

        public ISyntaxHighlighter SyntaxHighlighter
        {
            get
            {
                return syntaxHighlighter;
            }
            set
            {
                syntaxHighlighter = value;

                if (value != null)
                    HighlightAll();
            }
        }

        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            const short WM_PAINT = 0x00f;

            if (m.Msg == WM_PAINT && pausePainting)
            {
                m.Result = IntPtr.Zero;
                return;
            }

            base.WndProc(ref m);
        }

        public void HighlightAll()
        {
            // remember the current selection
            int originalSelectionStart = this.SelectionStart;
            int originalSelectionLength = this.SelectionLength;

            // stop painting to prevent flicker
            this.pausePainting = true;

            HighlightSection(0, this.Text.Length);

            // restore the original selection
            this.SelectionStart = originalSelectionStart;
            this.SelectionLength = originalSelectionLength;

            // resume painting
            this.pausePainting = false;
        }

        public void HighlightCurrentLine()
        {
            // remember the current selection
            int originalSelectionStart = this.SelectionStart;
            int originalSelectionLength = this.SelectionLength;

            // find the beginning of the line
            int startPosition = originalSelectionStart;
            while ((startPosition > 0) && (this.Text[startPosition - 1] != '\n'))
                startPosition--;

            // find the end of the line
            int endPosition = originalSelectionStart;
            while ((endPosition < this.Text.Length) && (this.Text[endPosition] != '\n'))
                endPosition++;

            // stop painting to prevent flicker
            this.pausePainting = true;

            HighlightSection(startPosition, endPosition - startPosition);

            // restore the original selection
            this.SelectionStart = originalSelectionStart;
            this.SelectionLength = originalSelectionLength;

            // resume painting
            this.pausePainting = false;
        }

        private void HighlightSection(int startPosition, int sectionLength)
        {
            if (SyntaxHighlighter == null)
                return;

            // default formatting
            this.SelectionStart = startPosition;
            this.SelectionLength = sectionLength;
            this.SelectionColor = this.ForeColor;
            this.SelectionFont = this.Font;

            // custom formatting
            foreach (SyntaxHighlighterResult highlight in SyntaxHighlighter.Highlight(this.Text, startPosition, sectionLength))
            {
                //if (highlight.Start >= 0 && highlight.Start < Text.Length)
                    this.SelectionStart = highlight.Start;
                //if (highlight.Length >= 0 && highlight.Start + highlight.Length < Text.Length)
                    this.SelectionLength = highlight.Length;
                if (highlight.Color != null)
                    this.SelectionColor = highlight.Color;
                if (highlight.Font != null)
                    this.SelectionFont = highlight.Font;
            }
        }

        private void HighlightCurrentLine(object sender, EventArgs e)
        {
            if (!pausePainting)
                HighlightCurrentLine();
        }
    }   
}
