using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace Pisc16
{
    public partial class MainForm : Form
    {
        Risc16Cpu cpu = new Risc16Cpu();
        
        TimeSpan pause = TimeSpan.FromMilliseconds(50);
        bool debugging;

        string filename;
        bool modifiedAfterSave;
        bool modifiedAfterCompilation = true;

        public MainForm()
        {
            List<string> psharp = new List<string>();
            psharp.Add("var a");
            psharp.Add("var b");
            psharp.Add("var c");
            psharp.Add("a = 123");
            psharp.Add("b = 456");
            psharp.Add("c = a+b");
            psharp.Add("echo c");
            //string[] lines = new PSharpToAsmTranslator().Translate(psharp.ToArray());

            //MessageBox.Show(string.Join(Environment.NewLine, lines));
            //return;

            InitializeComponent();

            asm.SyntaxHighlighter = new Risc16AsmSyntaxHighlighter();
            opcode.SyntaxHighlighter = new Risc16OpcodeSyntaxHighlighter();

            CreateRegiserLabels();
            CreateSleepDelays();
            newToolStripMenuItem_Click(this, new EventArgs());

            registersTable.RowStyles[0].Height = 25;
        }

        private void CreateRegiserLabels()
        {
            for (int i = 0; i < cpu.Registers.Count; i++)
            {
                Label register = new Label { Text = "R" + i, Name = "R" + i, TextAlign = ContentAlignment.TopCenter };
                registersTable.Controls.Add(register);
                registersTable.SetRow(register, i + 1);
                registersTable.SetColumn(register, 0);

                Label binary = new Label { Text = new string('0', 16), Name = "bin" + i, Dock = DockStyle.Fill, TextAlign = ContentAlignment.TopCenter };
                registersTable.Controls.Add(binary);
                registersTable.SetRow(binary, i + 1);
                registersTable.SetColumn(binary, 1);

                Label @decimal = new Label { Text = "0", Name = "dec" + i, Dock = DockStyle.Fill, TextAlign = ContentAlignment.TopCenter };
                registersTable.Controls.Add(@decimal);
                registersTable.SetRow(@decimal, i + 1);
                registersTable.SetColumn(@decimal, 2);

                Label hexdecimal = new Label { Text = "0", Name = "hex" + i, Dock = DockStyle.Fill, TextAlign = ContentAlignment.TopCenter };
                registersTable.Controls.Add(hexdecimal);
                registersTable.SetRow(hexdecimal, i + 1);
                registersTable.SetColumn(hexdecimal, 3);
            }
        }

        private void CreateSleepDelays()
        {
            sleepToolStripMenuItem.DropDownItems.Clear();

            foreach (int delay in new int[] { 10, 50, 100, 200, 300, 500, 750, 1000, 2000, 5000 })
            {
                var item = new ToolStripMenuItem(delay + " ms") { Tag = delay };
                item.Click += delegate
                {
                    pause = TimeSpan.FromMilliseconds((int)item.Tag);
                    foreach (ToolStripMenuItem item2 in sleepToolStripMenuItem.DropDownItems)
                        item2.Checked = false;
                    item.Checked = true;
                };

                sleepToolStripMenuItem.DropDownItems.Add(item);
            }
        }

        private void UpdateRegisterLabels()
        {
            for (int i = 0; i < cpu.Registers.Count; i++)
            {
                Label bin = (Label)Controls.Find("bin" + i, true)[0];
                Label dec = (Label)Controls.Find("dec" + i, true)[0];
                Label hex = (Label)Controls.Find("hex" + i, true)[0];

                bin.Text = cpu.Registers[i].ToBinaryString();
                dec.Text = cpu.Registers[i].ToInt32().ToString();
                hex.Text = string.Format("{0:X}", (short)cpu.Registers[i].ToInt32());
            }
        }

        private void UpdateMemoryTable()
        {
            memory.Clear();

            for (int i = 0; i < cpu.Memory.Size; i++)
            {
                if (i == 0 || cpu.Memory[i].ToInt32() != 0)
                {
                    memory.Text += i.ToString("X").PadLeft(4, '0') + " " + cpu.Memory[i].ToBinaryString() + Environment.NewLine;
                }
            }
        }

        private void Reset()
        {
            Stop();

            cpu = new Risc16Cpu();

            UpdateRegisterLabels();
            UpdateMemoryTable();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //return;
            if (tabs.SelectedTab == opcodeTabPage)
            {
                opcode.Clear();

                AsmToOpcodeTranslator translator = new AsmToOpcodeTranslator();

                //try
                //{
                    foreach (bool[] line in translator.Translate(asm.Lines))
                    {
                        if (line.ToInt32() != 0)
                            opcode.Text += line.ToBinaryString() + Environment.NewLine;
                    }

                    opcode.HighlightAll();
                //}
                //catch (TranslatorException ex)
                //{
                    //MessageBox.Show(ex.Message);
                //}
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Reset();

            filename = null;
            modifiedAfterSave = false;
            
            asm.Text = "";
            opcode.Text = "";
            
            action.Text = "Izveidota jauna programma.";
            Text = "Jauns fails";
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (asmOpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    asm.Text = System.IO.File.ReadAllText(asmOpenFileDialog.FileName);
                    asm.HighlightAll();

                    filename = asmOpenFileDialog.FileName;
                    modifiedAfterSave = false;

                    Text = filename;
                    action.Text = "Fails atvērts.";

                    Reset();
                    tabs.SelectedTab = asmTabPage;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Nevarēja atvērt failu " + filename + ": " + Environment.NewLine + Environment.NewLine + ex.Message, "Kļūda", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    action.Text = "Kļūda, atverot failu.";
                }
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (filename != null)
            {
                try
                {
                    System.IO.File.WriteAllText(filename, asm.Text);
                    modifiedAfterSave = false;
                    action.Text = "Izmaiņas saglabātas.";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Nevarēja saglabāt failu " + filename + ": " + Environment.NewLine + Environment.NewLine + ex.Message, "Kļūda", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    action.Text = "Kļūda, saglabājot failu.";
                }
            }
            else
            {
                saveAsToolStripMenuItem_Click(sender, e);
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (asmSaveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    System.IO.File.WriteAllText(asmSaveFileDialog.FileName, asm.Text);
                    filename = asmSaveFileDialog.FileName;
                    modifiedAfterSave = false;

                    Text = filename;
                    action.Text = "Fails saglabāts.";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Nevarēja saglabāt failu " + filename + ": " + Environment.NewLine + Environment.NewLine + ex.Message, "Kļūda", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    action.Text = "Kļūda, saglabājot failu.";
                }
            }
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            /* if (modified)
            {
                DialogResult result = MessageBox.Show("Vai saglabāt izmaiņas?", "Nesaglabātas izmaiņas", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    saveToolStripMenuItem_Click(sender, new EventArgs());
                }
                else if (result == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
            } */
        }

        private void asm_TextChanged(object sender, EventArgs e)
        {
            if (!modifiedAfterSave)
            {
                modifiedAfterSave = true;
                Text += " *";
            }

            if (!modifiedAfterCompilation)
                modifiedAfterCompilation = true;
        }

        private void runToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Run(true);
        }

        private void runWithoutDebuggingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Run(false);
        }

        private void pauseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pauseToolStripButton.Checked)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stop();
        }

        private void nextStepToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NextStep();
        }

        private bool Compile()
        {
            if (modifiedAfterCompilation || modifiedAfterSave)
            {
                action.Text = "Pārveido asambleru mašīnkodā...";

                try
                {
                    AsmToOpcodeTranslator translator = new AsmToOpcodeTranslator();
                    //cpu.Memory.Load(translator.Translate(asm.Lines, cpu.Memory.Size));
                    cpu.Memory.Load(translator.Translate(asm.Lines));
                    action.Text = "Pārveidošana mašīnkodā veiksmīgi pabeigta.";
                    modifiedAfterCompilation = false;
                    nextStepToolStripButton.Enabled = true;
                    return true;
                }
                catch (TranslatorException ex)
                {
                    action.Text = "Kļūda, pārveidojot mašīnkodā: " + ex.Message;
                    return false;
                }
            }
            return true;
        }

        private void Run(bool debugging)
        {
            if (IsPaused())
            {
                this.debugging = debugging;
                Resume();
                return;
            }

            if (Compile())
            {
                runToolStripButton.Enabled = false;
                runWithoutDebuggingToolStripButton.Enabled = false;
                pauseToolStripButton.Enabled = true;
                pauseToolStripButton.Checked = false;
                stopToolStripButton.Enabled = true;
                nextStepToolStripButton.Enabled = false;

                this.debugging = debugging;

                if (!cpuBackgroundWorker.IsBusy)
                {
                    action.Text = "Procesors iedarbināts" + (debugging ? " (ar atkļūdošanu)" : "") + ".";    
                    cpuBackgroundWorker.RunWorkerAsync();
                }
            }
        }

        private bool IsPaused()
        {
            return pauseToolStripButton.Checked;
        }

        private void Pause()
        {
            pauseToolStripButton.Checked = true;
            runToolStripButton.Enabled = true;
            runWithoutDebuggingToolStripButton.Enabled = true;
            nextStepToolStripButton.Enabled = true;

            if (cpuBackgroundWorker.IsBusy)
            {
                action.Text = "Procesora darbība uz laiku apturēta.";
                cpuBackgroundWorker.CancelAsync();
            }
        }

        private void Resume()
        {
            pauseToolStripButton.Checked = false;
            runToolStripButton.Enabled = false;
            runWithoutDebuggingToolStripButton.Enabled = false;
            nextStepToolStripButton.Enabled = false;

            if (!cpuBackgroundWorker.IsBusy)
            {
                action.Text = "Procesora darbība atsākta.";
                cpuBackgroundWorker.RunWorkerAsync();
            }
        }

        private void Stop()
        {
            runToolStripButton.Enabled = true;
            runWithoutDebuggingToolStripButton.Enabled = true;
            pauseToolStripButton.Enabled = false;
            pauseToolStripButton.Checked = false;
            stopToolStripButton.Enabled = false;
            nextStepToolStripButton.Enabled = !cpu.IsFinished;

            if (cpuBackgroundWorker.IsBusy)
            {
                action.Text = "Procesora darbība pārtraukta.";
                cpuBackgroundWorker.CancelAsync();
            }
        }

        private void NextStep()
        {
            if (Compile())
            {
                if (!cpuBackgroundWorker.IsBusy && !cpu.IsFinished)
                {
                    debugging = true;
                    ExecuteStep();
                }

                if (cpu.IsFinished)
                {
                    Stop();
                }
            }
        }

        private void cpuBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (!cpu.IsFinished && !cpuBackgroundWorker.CancellationPending)
            {
                ExecuteStep();

                if (debugging)
                {
                    Thread.Sleep(pause);
                }
            }
        }

        private void cpuBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            UpdateRegisterLabels();
            UpdateMemoryTable();

            if (!IsPaused())
                Stop();
        }

        private void ExecuteStep()
        {
            cpu.NextStep();

            if (debugging)
            {
                Invoke(new MethodInvoker(UpdateRegisterLabels));
                Invoke(new MethodInvoker(UpdateMemoryTable));
                statusStrip1.Invoke(new MethodInvoker(delegate { action.Text = "Izpilda komandu no atmiņas ar adresi " + cpu.CurrentStep + " (" + cpu.CurrentStep.ToString("X") + ")"; }));
            }
        }        
    }

    public static class ConvertExtensions
    {
        public static string ToBinaryString(this bool[] bits)
        {
            string binary = "";

            for (int i = 0; i < bits.Length; i++)
                binary += bits[i] ? "1" : "0";

            return binary;
        }

        public static int ToInt32(this bool[] bits)
        {
            int n = 0;
            
            for (int i = 1; i < bits.Length; i++)
                n += bits[i] ? (int)Math.Pow(2, bits.Length - i - 1) : 0;

            return bits[0] ? n - (int)Math.Pow(2, bits.Length - 1) : n;
        }

        public static int ToUnsignedInt32(this bool[] bits)
        {
            int n = 0;

            for (int i = 0; i < bits.Length; i++)
                n += bits[i] ? (int)Math.Pow(2, bits.Length - i - 1) : 0;

            return n;
        }
    }
}
