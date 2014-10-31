namespace Pisc16
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cpuToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runWithoutDebuggingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pauseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nextStepToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sleepToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.newToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.openToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.saveToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.cutToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.copyToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.pasteToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.runToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.runWithoutDebuggingToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.pauseToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.stopToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.nextStepToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.action = new System.Windows.Forms.ToolStripStatusLabel();
            this.registersGroupBox = new System.Windows.Forms.GroupBox();
            this.registersTable = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tabs = new System.Windows.Forms.TabControl();
            this.asmTabPage = new System.Windows.Forms.TabPage();
            this.asm = new Pisc16.SyntaxHighlightedRichTextBox();
            this.richTextBoxLineNumbers1 = new Pisc16.RichTextBoxLineNumbers();
            this.opcodeTabPage = new System.Windows.Forms.TabPage();
            this.opcode = new Pisc16.SyntaxHighlightedRichTextBox();
            this.richTextBoxLineNumbers2 = new Pisc16.RichTextBoxLineNumbers();
            this.cpuBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.asmOpenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.asmSaveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.memory = new System.Windows.Forms.TextBox();
            this.menuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.registersGroupBox.SuspendLayout();
            this.registersTable.SuspendLayout();
            this.tabs.SuspendLayout();
            this.asmTabPage.SuspendLayout();
            this.opcodeTabPage.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.cpuToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(826, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.toolStripSeparator1,
            this.closeToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(42, 20);
            this.fileToolStripMenuItem.Text = "&Fails";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.newToolStripMenuItem.Text = "Jau&ns";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.openToolStripMenuItem.Text = "Atvērt...";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.saveToolStripMenuItem.Text = "&Saglabāt";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.saveAsToolStripMenuItem.Text = "Saglabāt kā...";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(156, 6);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.closeToolStripMenuItem.Text = "Iziet";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // cpuToolStripMenuItem
            // 
            this.cpuToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.runToolStripMenuItem,
            this.runWithoutDebuggingToolStripMenuItem,
            this.pauseToolStripMenuItem,
            this.stopToolStripMenuItem,
            this.nextStepToolStripMenuItem,
            this.sleepToolStripMenuItem});
            this.cpuToolStripMenuItem.Name = "cpuToolStripMenuItem";
            this.cpuToolStripMenuItem.Size = new System.Drawing.Size(70, 20);
            this.cpuToolStripMenuItem.Text = "Procesors";
            // 
            // runToolStripMenuItem
            // 
            this.runToolStripMenuItem.Image = global::Pisc16.Properties.Resources.debug_run_icon;
            this.runToolStripMenuItem.Name = "runToolStripMenuItem";
            this.runToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.runToolStripMenuItem.Size = new System.Drawing.Size(266, 22);
            this.runToolStripMenuItem.Text = "Iedarbināt";
            this.runToolStripMenuItem.Click += new System.EventHandler(this.runToolStripMenuItem_Click);
            // 
            // runWithoutDebuggingToolStripMenuItem
            // 
            this.runWithoutDebuggingToolStripMenuItem.Image = global::Pisc16.Properties.Resources.debug_run_without_breakpoint_icon;
            this.runWithoutDebuggingToolStripMenuItem.Name = "runWithoutDebuggingToolStripMenuItem";
            this.runWithoutDebuggingToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F5)));
            this.runWithoutDebuggingToolStripMenuItem.Size = new System.Drawing.Size(266, 22);
            this.runWithoutDebuggingToolStripMenuItem.Text = "Iedarbināt bez atkļūdošanas";
            this.runWithoutDebuggingToolStripMenuItem.Click += new System.EventHandler(this.runWithoutDebuggingToolStripMenuItem_Click);
            // 
            // pauseToolStripMenuItem
            // 
            this.pauseToolStripMenuItem.Image = global::Pisc16.Properties.Resources.nsuspend;
            this.pauseToolStripMenuItem.Name = "pauseToolStripMenuItem";
            this.pauseToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F6;
            this.pauseToolStripMenuItem.Size = new System.Drawing.Size(266, 22);
            this.pauseToolStripMenuItem.Text = "Apturēt";
            this.pauseToolStripMenuItem.Click += new System.EventHandler(this.pauseToolStripMenuItem_Click);
            // 
            // stopToolStripMenuItem
            // 
            this.stopToolStripMenuItem.Image = global::Pisc16.Properties.Resources.ntermin;
            this.stopToolStripMenuItem.Name = "stopToolStripMenuItem";
            this.stopToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F7;
            this.stopToolStripMenuItem.Size = new System.Drawing.Size(266, 22);
            this.stopToolStripMenuItem.Text = "Pārtraukt";
            this.stopToolStripMenuItem.Click += new System.EventHandler(this.stopToolStripMenuItem_Click);
            // 
            // nextStepToolStripMenuItem
            // 
            this.nextStepToolStripMenuItem.Name = "nextStepToolStripMenuItem";
            this.nextStepToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F8;
            this.nextStepToolStripMenuItem.Size = new System.Drawing.Size(266, 22);
            this.nextStepToolStripMenuItem.Text = "Nākamais solis";
            this.nextStepToolStripMenuItem.Click += new System.EventHandler(this.nextStepToolStripMenuItem_Click);
            // 
            // sleepToolStripMenuItem
            // 
            this.sleepToolStripMenuItem.Name = "sleepToolStripMenuItem";
            this.sleepToolStripMenuItem.Size = new System.Drawing.Size(266, 22);
            this.sleepToolStripMenuItem.Text = "Laiks starp soļiem";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripButton,
            this.openToolStripButton,
            this.saveToolStripButton,
            this.toolStripSeparator,
            this.cutToolStripButton,
            this.copyToolStripButton,
            this.pasteToolStripButton,
            this.toolStripSeparator2,
            this.runToolStripButton,
            this.runWithoutDebuggingToolStripButton,
            this.pauseToolStripButton,
            this.stopToolStripButton,
            this.nextStepToolStripButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(826, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip";
            // 
            // newToolStripButton
            // 
            this.newToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.newToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("newToolStripButton.Image")));
            this.newToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.newToolStripButton.Name = "newToolStripButton";
            this.newToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.newToolStripButton.Text = "Jau&ns";
            this.newToolStripButton.ToolTipText = "Izveidot jaunu programmu";
            this.newToolStripButton.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // openToolStripButton
            // 
            this.openToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.openToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("openToolStripButton.Image")));
            this.openToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.openToolStripButton.Name = "openToolStripButton";
            this.openToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.openToolStripButton.Text = "Atvērt";
            this.openToolStripButton.ToolTipText = "Ielādēt programmu no faila";
            this.openToolStripButton.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripButton
            // 
            this.saveToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.saveToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("saveToolStripButton.Image")));
            this.saveToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.saveToolStripButton.Name = "saveToolStripButton";
            this.saveToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.saveToolStripButton.Text = "&Saglabāt";
            this.saveToolStripButton.ToolTipText = "Saglabāt izmaiņas failā";
            this.saveToolStripButton.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // toolStripSeparator
            // 
            this.toolStripSeparator.Name = "toolStripSeparator";
            this.toolStripSeparator.Size = new System.Drawing.Size(6, 25);
            // 
            // cutToolStripButton
            // 
            this.cutToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.cutToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("cutToolStripButton.Image")));
            this.cutToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.cutToolStripButton.Name = "cutToolStripButton";
            this.cutToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.cutToolStripButton.Text = "C&ut";
            // 
            // copyToolStripButton
            // 
            this.copyToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.copyToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("copyToolStripButton.Image")));
            this.copyToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.copyToolStripButton.Name = "copyToolStripButton";
            this.copyToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.copyToolStripButton.Text = "&Copy";
            // 
            // pasteToolStripButton
            // 
            this.pasteToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.pasteToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("pasteToolStripButton.Image")));
            this.pasteToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.pasteToolStripButton.Name = "pasteToolStripButton";
            this.pasteToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.pasteToolStripButton.Text = "&Paste";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // runToolStripButton
            // 
            this.runToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.runToolStripButton.Image = global::Pisc16.Properties.Resources.debug_run_icon;
            this.runToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.runToolStripButton.Name = "runToolStripButton";
            this.runToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.runToolStripButton.Text = "Palaist";
            this.runToolStripButton.Click += new System.EventHandler(this.runToolStripMenuItem_Click);
            // 
            // runWithoutDebuggingToolStripButton
            // 
            this.runWithoutDebuggingToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.runWithoutDebuggingToolStripButton.Image = global::Pisc16.Properties.Resources.debug_run_without_breakpoint_icon;
            this.runWithoutDebuggingToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.runWithoutDebuggingToolStripButton.Name = "runWithoutDebuggingToolStripButton";
            this.runWithoutDebuggingToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.runWithoutDebuggingToolStripButton.Text = "Palaist bez pauzēm";
            this.runWithoutDebuggingToolStripButton.Click += new System.EventHandler(this.runWithoutDebuggingToolStripMenuItem_Click);
            // 
            // pauseToolStripButton
            // 
            this.pauseToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.pauseToolStripButton.Image = global::Pisc16.Properties.Resources.nsuspend;
            this.pauseToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.pauseToolStripButton.Name = "pauseToolStripButton";
            this.pauseToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.pauseToolStripButton.Text = "Pause";
            this.pauseToolStripButton.Click += new System.EventHandler(this.pauseToolStripMenuItem_Click);
            // 
            // stopToolStripButton
            // 
            this.stopToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.stopToolStripButton.Image = global::Pisc16.Properties.Resources.ntermin;
            this.stopToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stopToolStripButton.Name = "stopToolStripButton";
            this.stopToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.stopToolStripButton.Text = "Stop";
            this.stopToolStripButton.Click += new System.EventHandler(this.stopToolStripMenuItem_Click);
            // 
            // nextStepToolStripButton
            // 
            this.nextStepToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.nextStepToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("nextStepToolStripButton.Image")));
            this.nextStepToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.nextStepToolStripButton.Name = "nextStepToolStripButton";
            this.nextStepToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.nextStepToolStripButton.Text = "Next Step";
            this.nextStepToolStripButton.Click += new System.EventHandler(this.nextStepToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.action});
            this.statusStrip1.Location = new System.Drawing.Point(0, 366);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(826, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip";
            // 
            // action
            // 
            this.action.Name = "action";
            this.action.Size = new System.Drawing.Size(0, 17);
            // 
            // registersGroupBox
            // 
            this.registersGroupBox.Controls.Add(this.registersTable);
            this.registersGroupBox.Location = new System.Drawing.Point(12, 52);
            this.registersGroupBox.Name = "registersGroupBox";
            this.registersGroupBox.Size = new System.Drawing.Size(264, 236);
            this.registersGroupBox.TabIndex = 3;
            this.registersGroupBox.TabStop = false;
            this.registersGroupBox.Text = "Reģistri";
            // 
            // registersTable
            // 
            this.registersTable.BackColor = System.Drawing.Color.Transparent;
            this.registersTable.ColumnCount = 4;
            this.registersTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.registersTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 56.4034F));
            this.registersTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 23.6938F));
            this.registersTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 19.9028F));
            this.registersTable.Controls.Add(this.label1, 0, 0);
            this.registersTable.Controls.Add(this.label2, 1, 0);
            this.registersTable.Controls.Add(this.label3, 2, 0);
            this.registersTable.Controls.Add(this.label4, 3, 0);
            this.registersTable.Location = new System.Drawing.Point(6, 25);
            this.registersTable.Name = "registersTable";
            this.registersTable.RowCount = 1;
            this.registersTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 234F));
            this.registersTable.Size = new System.Drawing.Size(252, 234);
            this.registersTable.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(21, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "R";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Location = new System.Drawing.Point(30, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(120, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Bin";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Top;
            this.label3.Location = new System.Drawing.Point(156, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Dec";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Top;
            this.label4.Location = new System.Drawing.Point(209, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(40, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Hex";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tabs
            // 
            this.tabs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabs.Controls.Add(this.asmTabPage);
            this.tabs.Controls.Add(this.opcodeTabPage);
            this.tabs.Location = new System.Drawing.Point(282, 52);
            this.tabs.Name = "tabs";
            this.tabs.SelectedIndex = 0;
            this.tabs.Size = new System.Drawing.Size(537, 304);
            this.tabs.TabIndex = 4;
            this.tabs.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // asmTabPage
            // 
            this.asmTabPage.Controls.Add(this.asm);
            this.asmTabPage.Controls.Add(this.richTextBoxLineNumbers1);
            this.asmTabPage.Location = new System.Drawing.Point(4, 22);
            this.asmTabPage.Name = "asmTabPage";
            this.asmTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.asmTabPage.Size = new System.Drawing.Size(529, 278);
            this.asmTabPage.TabIndex = 0;
            this.asmTabPage.Text = "Asamblers";
            this.asmTabPage.UseVisualStyleBackColor = true;
            // 
            // asm
            // 
            this.asm.AcceptsTab = true;
            this.asm.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.asm.DetectUrls = false;
            this.asm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.asm.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.asm.Location = new System.Drawing.Point(21, 3);
            this.asm.Name = "asm";
            this.asm.Size = new System.Drawing.Size(505, 272);
            this.asm.SyntaxHighlighter = null;
            this.asm.TabIndex = 0;
            this.asm.Text = "aaaa\nbeee\ncee";
            this.asm.WordWrap = false;
            this.asm.TextChanged += new System.EventHandler(this.asm_TextChanged);
            // 
            // richTextBoxLineNumbers1
            // 
            this.richTextBoxLineNumbers1._SeeThroughMode_ = false;
            this.richTextBoxLineNumbers1.AutoSizing = true;
            this.richTextBoxLineNumbers1.BackgroundGradient_AlphaColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.richTextBoxLineNumbers1.BackgroundGradient_BetaColor = System.Drawing.Color.LightSteelBlue;
            this.richTextBoxLineNumbers1.BackgroundGradient_Direction = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
            this.richTextBoxLineNumbers1.BorderLines_Color = System.Drawing.Color.SlateGray;
            this.richTextBoxLineNumbers1.BorderLines_Style = System.Drawing.Drawing2D.DashStyle.Solid;
            this.richTextBoxLineNumbers1.BorderLines_Thickness = 1F;
            this.richTextBoxLineNumbers1.Dock = System.Windows.Forms.DockStyle.Left;
            this.richTextBoxLineNumbers1.DockSide = Pisc16.RichTextBoxLineNumbers.LineNumberDockSide.Left;
            this.richTextBoxLineNumbers1.GridLines_Color = System.Drawing.Color.SlateGray;
            this.richTextBoxLineNumbers1.GridLines_Style = System.Drawing.Drawing2D.DashStyle.Dot;
            this.richTextBoxLineNumbers1.GridLines_Thickness = 1F;
            this.richTextBoxLineNumbers1.LineNrs_Alignment = System.Drawing.ContentAlignment.TopRight;
            this.richTextBoxLineNumbers1.LineNrs_AntiAlias = true;
            this.richTextBoxLineNumbers1.LineNrs_AsHexadecimal = false;
            this.richTextBoxLineNumbers1.LineNrs_ClippedByItemRectangle = true;
            this.richTextBoxLineNumbers1.LineNrs_LeadingZeroes = false;
            this.richTextBoxLineNumbers1.LineNrs_Offset = new System.Drawing.Size(0, 2);
            this.richTextBoxLineNumbers1.Location = new System.Drawing.Point(3, 3);
            this.richTextBoxLineNumbers1.Margin = new System.Windows.Forms.Padding(0);
            this.richTextBoxLineNumbers1.MarginLines_Color = System.Drawing.Color.SlateGray;
            this.richTextBoxLineNumbers1.MarginLines_Side = Pisc16.RichTextBoxLineNumbers.LineNumberDockSide.Right;
            this.richTextBoxLineNumbers1.MarginLines_Style = System.Drawing.Drawing2D.DashStyle.Solid;
            this.richTextBoxLineNumbers1.MarginLines_Thickness = 1F;
            this.richTextBoxLineNumbers1.Name = "richTextBoxLineNumbers1";
            this.richTextBoxLineNumbers1.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.richTextBoxLineNumbers1.ParentRichTextBox = this.asm;
            this.richTextBoxLineNumbers1.Show_BackgroundGradient = false;
            this.richTextBoxLineNumbers1.Show_BorderLines = false;
            this.richTextBoxLineNumbers1.Show_GridLines = false;
            this.richTextBoxLineNumbers1.Show_LineNrs = true;
            this.richTextBoxLineNumbers1.Show_MarginLines = false;
            this.richTextBoxLineNumbers1.Size = new System.Drawing.Size(18, 272);
            this.richTextBoxLineNumbers1.TabIndex = 1;
            // 
            // opcodeTabPage
            // 
            this.opcodeTabPage.Controls.Add(this.opcode);
            this.opcodeTabPage.Controls.Add(this.richTextBoxLineNumbers2);
            this.opcodeTabPage.Location = new System.Drawing.Point(4, 22);
            this.opcodeTabPage.Name = "opcodeTabPage";
            this.opcodeTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.opcodeTabPage.Size = new System.Drawing.Size(529, 278);
            this.opcodeTabPage.TabIndex = 1;
            this.opcodeTabPage.Text = "Mašīnkods";
            this.opcodeTabPage.UseVisualStyleBackColor = true;
            // 
            // opcode
            // 
            this.opcode.AcceptsTab = true;
            this.opcode.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.opcode.DetectUrls = false;
            this.opcode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opcode.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.opcode.Location = new System.Drawing.Point(21, 3);
            this.opcode.Name = "opcode";
            this.opcode.Size = new System.Drawing.Size(505, 272);
            this.opcode.SyntaxHighlighter = null;
            this.opcode.TabIndex = 0;
            this.opcode.Text = "234";
            this.opcode.WordWrap = false;
            // 
            // richTextBoxLineNumbers2
            // 
            this.richTextBoxLineNumbers2._SeeThroughMode_ = false;
            this.richTextBoxLineNumbers2.AutoSizing = true;
            this.richTextBoxLineNumbers2.BackgroundGradient_AlphaColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.richTextBoxLineNumbers2.BackgroundGradient_BetaColor = System.Drawing.Color.LightSteelBlue;
            this.richTextBoxLineNumbers2.BackgroundGradient_Direction = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
            this.richTextBoxLineNumbers2.BorderLines_Color = System.Drawing.Color.SlateGray;
            this.richTextBoxLineNumbers2.BorderLines_Style = System.Drawing.Drawing2D.DashStyle.Solid;
            this.richTextBoxLineNumbers2.BorderLines_Thickness = 1F;
            this.richTextBoxLineNumbers2.Dock = System.Windows.Forms.DockStyle.Left;
            this.richTextBoxLineNumbers2.DockSide = Pisc16.RichTextBoxLineNumbers.LineNumberDockSide.Left;
            this.richTextBoxLineNumbers2.GridLines_Color = System.Drawing.Color.SlateGray;
            this.richTextBoxLineNumbers2.GridLines_Style = System.Drawing.Drawing2D.DashStyle.Dot;
            this.richTextBoxLineNumbers2.GridLines_Thickness = 1F;
            this.richTextBoxLineNumbers2.LineNrs_Alignment = System.Drawing.ContentAlignment.TopRight;
            this.richTextBoxLineNumbers2.LineNrs_AntiAlias = true;
            this.richTextBoxLineNumbers2.LineNrs_AsHexadecimal = false;
            this.richTextBoxLineNumbers2.LineNrs_ClippedByItemRectangle = true;
            this.richTextBoxLineNumbers2.LineNrs_LeadingZeroes = false;
            this.richTextBoxLineNumbers2.LineNrs_Offset = new System.Drawing.Size(0, 2);
            this.richTextBoxLineNumbers2.Location = new System.Drawing.Point(3, 3);
            this.richTextBoxLineNumbers2.Margin = new System.Windows.Forms.Padding(0);
            this.richTextBoxLineNumbers2.MarginLines_Color = System.Drawing.Color.SlateGray;
            this.richTextBoxLineNumbers2.MarginLines_Side = Pisc16.RichTextBoxLineNumbers.LineNumberDockSide.Right;
            this.richTextBoxLineNumbers2.MarginLines_Style = System.Drawing.Drawing2D.DashStyle.Solid;
            this.richTextBoxLineNumbers2.MarginLines_Thickness = 1F;
            this.richTextBoxLineNumbers2.Name = "richTextBoxLineNumbers2";
            this.richTextBoxLineNumbers2.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.richTextBoxLineNumbers2.ParentRichTextBox = this.opcode;
            this.richTextBoxLineNumbers2.Show_BackgroundGradient = false;
            this.richTextBoxLineNumbers2.Show_BorderLines = false;
            this.richTextBoxLineNumbers2.Show_GridLines = false;
            this.richTextBoxLineNumbers2.Show_LineNrs = true;
            this.richTextBoxLineNumbers2.Show_MarginLines = false;
            this.richTextBoxLineNumbers2.Size = new System.Drawing.Size(18, 272);
            this.richTextBoxLineNumbers2.TabIndex = 1;
            // 
            // cpuBackgroundWorker
            // 
            this.cpuBackgroundWorker.WorkerSupportsCancellation = true;
            this.cpuBackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.cpuBackgroundWorker_DoWork);
            this.cpuBackgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.cpuBackgroundWorker_RunWorkerCompleted);
            // 
            // asmOpenFileDialog
            // 
            this.asmOpenFileDialog.AddExtension = false;
            this.asmOpenFileDialog.Filter = "ASM Files|*.asm|Text Files|*.txt|All Files|*.*";
            // 
            // asmSaveFileDialog
            // 
            this.asmSaveFileDialog.DefaultExt = "asm";
            this.asmSaveFileDialog.Filter = "ASM Files|*.asm|Text Files|*.txt|All Files|*.*";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.memory);
            this.groupBox1.Location = new System.Drawing.Point(12, 294);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(264, 62);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Atmiņa";
            // 
            // memory
            // 
            this.memory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.memory.BackColor = System.Drawing.SystemColors.Control;
            this.memory.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.memory.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.memory.Location = new System.Drawing.Point(12, 19);
            this.memory.Multiline = true;
            this.memory.Name = "memory";
            this.memory.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.memory.Size = new System.Drawing.Size(243, 36);
            this.memory.TabIndex = 6;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(826, 388);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.tabs);
            this.Controls.Add(this.registersGroupBox);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.registersGroupBox.ResumeLayout(false);
            this.registersTable.ResumeLayout(false);
            this.registersTable.PerformLayout();
            this.tabs.ResumeLayout(false);
            this.asmTabPage.ResumeLayout(false);
            this.opcodeTabPage.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.GroupBox registersGroupBox;
        private System.Windows.Forms.TabControl tabs;
        private System.Windows.Forms.TabPage asmTabPage;
        private SyntaxHighlightedRichTextBox asm;
        private System.Windows.Forms.TabPage opcodeTabPage;
        private SyntaxHighlightedRichTextBox opcode;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.TableLayoutPanel registersTable;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ToolStripButton runToolStripButton;
        private System.ComponentModel.BackgroundWorker cpuBackgroundWorker;
        private System.Windows.Forms.OpenFileDialog asmOpenFileDialog;
        private System.Windows.Forms.SaveFileDialog asmSaveFileDialog;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton stopToolStripButton;
        private System.Windows.Forms.ToolStripButton pauseToolStripButton;
        private System.Windows.Forms.ToolStripButton nextStepToolStripButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox memory;
        private System.Windows.Forms.ToolStripButton newToolStripButton;
        private System.Windows.Forms.ToolStripButton openToolStripButton;
        private System.Windows.Forms.ToolStripButton saveToolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
        private System.Windows.Forms.ToolStripButton cutToolStripButton;
        private System.Windows.Forms.ToolStripButton copyToolStripButton;
        private System.Windows.Forms.ToolStripButton pasteToolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton runWithoutDebuggingToolStripButton;
        private System.Windows.Forms.ToolStripMenuItem cpuToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem runToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem runWithoutDebuggingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pauseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stopToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nextStepToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sleepToolStripMenuItem;
        private RichTextBoxLineNumbers richTextBoxLineNumbers1;
        private RichTextBoxLineNumbers richTextBoxLineNumbers2;
        private System.Windows.Forms.ToolStripStatusLabel action;
    }
}

