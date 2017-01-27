namespace TuringMachine
{
    partial class FMain
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            this.gridInput = new System.Windows.Forms.DataGridView();
            this.cInputsType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cInputDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cInputCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cInputFails = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cInputCrashes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridLog = new System.Windows.Forms.DataGridView();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.folderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tcpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.executeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.randomToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gridConfig = new System.Windows.Forms.DataGridView();
            this.cConfigType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cConfigDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cConfigCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cConfigFails = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cConfigCrashes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolStrip3 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripDropDownButton2 = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStrip4 = new System.Windows.Forms.ToolStrip();
            this.tbPlay = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tbPause = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tbStop = new System.Windows.Forms.ToolStripButton();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.saveInputWithToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.originalInputToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cLogDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cLogOrigin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cLogType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cExploitable = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cLogInput = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cInputConfig = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cLogGoto = new System.Windows.Forms.DataGridViewButtonColumn();
            ((System.ComponentModel.ISupportInitialize)(this.gridInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridLog)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridConfig)).BeginInit();
            this.toolStrip3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.toolStrip4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gridInput
            // 
            this.gridInput.AllowUserToAddRows = false;
            this.gridInput.AllowUserToDeleteRows = false;
            this.gridInput.AllowUserToOrderColumns = true;
            this.gridInput.AllowUserToResizeRows = false;
            this.gridInput.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridInput.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cInputsType,
            this.cInputDescription,
            this.cInputCount,
            this.cInputFails,
            this.cInputCrashes});
            this.gridInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridInput.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.gridInput.Location = new System.Drawing.Point(0, 34);
            this.gridInput.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gridInput.Name = "gridInput";
            this.gridInput.ReadOnly = true;
            this.gridInput.RowHeadersVisible = false;
            this.gridInput.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridInput.Size = new System.Drawing.Size(444, 218);
            this.gridInput.TabIndex = 0;
            this.gridInput.VirtualMode = true;
            this.gridInput.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.gridConfig_ColumnHeaderMouseClick);
            this.gridInput.SelectionChanged += new System.EventHandler(this.gridInput_SelectionChanged);
            this.gridInput.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.gridConfig_SortCompare);
            this.gridInput.MouseClick += new System.Windows.Forms.MouseEventHandler(this.gridInput_MouseClick);
            // 
            // cInputsType
            // 
            this.cInputsType.DataPropertyName = "Type";
            this.cInputsType.HeaderText = "Type";
            this.cInputsType.Name = "cInputsType";
            this.cInputsType.ReadOnly = true;
            this.cInputsType.Width = 90;
            // 
            // cInputDescription
            // 
            this.cInputDescription.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.cInputDescription.DataPropertyName = "Description";
            this.cInputDescription.HeaderText = "Description";
            this.cInputDescription.Name = "cInputDescription";
            this.cInputDescription.ReadOnly = true;
            // 
            // cInputCount
            // 
            this.cInputCount.DataPropertyName = "Tests";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.cInputCount.DefaultCellStyle = dataGridViewCellStyle1;
            this.cInputCount.HeaderText = "Tests";
            this.cInputCount.Name = "cInputCount";
            this.cInputCount.ReadOnly = true;
            this.cInputCount.Width = 80;
            // 
            // cInputFails
            // 
            this.cInputFails.DataPropertyName = "Fails";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.cInputFails.DefaultCellStyle = dataGridViewCellStyle2;
            this.cInputFails.HeaderText = "Fails";
            this.cInputFails.Name = "cInputFails";
            this.cInputFails.ReadOnly = true;
            this.cInputFails.Width = 55;
            // 
            // cInputCrashes
            // 
            this.cInputCrashes.DataPropertyName = "Crashes";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.cInputCrashes.DefaultCellStyle = dataGridViewCellStyle3;
            this.cInputCrashes.HeaderText = "Crashes";
            this.cInputCrashes.Name = "cInputCrashes";
            this.cInputCrashes.ReadOnly = true;
            this.cInputCrashes.Width = 60;
            // 
            // gridLog
            // 
            this.gridLog.AllowUserToAddRows = false;
            this.gridLog.AllowUserToDeleteRows = false;
            this.gridLog.AllowUserToOrderColumns = true;
            this.gridLog.AllowUserToResizeRows = false;
            this.gridLog.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridLog.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cLogDate,
            this.cLogOrigin,
            this.cLogType,
            this.cExploitable,
            this.cLogInput,
            this.cInputConfig,
            this.cLogGoto});
            this.gridLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridLog.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.gridLog.Location = new System.Drawing.Point(0, 25);
            this.gridLog.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gridLog.Name = "gridLog";
            this.gridLog.ReadOnly = true;
            this.gridLog.RowHeadersVisible = false;
            this.gridLog.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridLog.Size = new System.Drawing.Size(912, 217);
            this.gridLog.TabIndex = 1;
            this.gridLog.VirtualMode = true;
            this.gridLog.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridLog_CellContentClick);
            this.gridLog.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.gridLog_CellFormatting);
            this.gridLog.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.gridConfig_ColumnHeaderMouseClick);
            this.gridLog.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.gridConfig_SortCompare);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.gridInput);
            this.splitContainer1.Panel1.Controls.Add(this.toolStrip1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.gridConfig);
            this.splitContainer1.Panel2.Controls.Add(this.toolStrip3);
            this.splitContainer1.Size = new System.Drawing.Size(912, 252);
            this.splitContainer1.SplitterDistance = 444;
            this.splitContainer1.TabIndex = 2;
            // 
            // toolStrip1
            // 
            this.toolStrip1.AutoSize = false;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.toolStripButton1,
            this.toolStripDropDownButton1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(444, 34);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(40, 31);
            this.toolStripLabel1.Text = "Inputs";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Enabled = false;
            this.toolStripButton1.Image = global::TuringMachine.Res.trash;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(36, 31);
            this.toolStripButton1.Text = "Remove selected";
            this.toolStripButton1.ToolTipText = "Remove selected";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.folderToolStripMenuItem,
            this.tcpToolStripMenuItem,
            this.executeToolStripMenuItem,
            this.randomToolStripMenuItem});
            this.toolStripDropDownButton1.Image = global::TuringMachine.Res.add;
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(45, 31);
            this.toolStripDropDownButton1.ToolTipText = "Add";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.Image = global::TuringMachine.Res.add_file;
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.fileToolStripMenuItem.Text = "File";
            this.fileToolStripMenuItem.Click += new System.EventHandler(this.fileToolStripMenuItem_Click);
            // 
            // folderToolStripMenuItem
            // 
            this.folderToolStripMenuItem.Image = global::TuringMachine.Res.add_folder;
            this.folderToolStripMenuItem.Name = "folderToolStripMenuItem";
            this.folderToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.folderToolStripMenuItem.Text = "Folder";
            this.folderToolStripMenuItem.Click += new System.EventHandler(this.folderToolStripMenuItem_Click);
            // 
            // tcpToolStripMenuItem
            // 
            this.tcpToolStripMenuItem.Image = global::TuringMachine.Res.add_socket;
            this.tcpToolStripMenuItem.Name = "tcpToolStripMenuItem";
            this.tcpToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.tcpToolStripMenuItem.Text = "Tcp Query";
            this.tcpToolStripMenuItem.Click += new System.EventHandler(this.socketToolStripMenuItem_Click);
            // 
            // executeToolStripMenuItem
            // 
            this.executeToolStripMenuItem.Image = global::TuringMachine.Res.add_execute;
            this.executeToolStripMenuItem.Name = "executeToolStripMenuItem";
            this.executeToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.executeToolStripMenuItem.Text = "Execute";
            this.executeToolStripMenuItem.Click += new System.EventHandler(this.executeToolStripMenuItem_Click);
            // 
            // randomToolStripMenuItem
            // 
            this.randomToolStripMenuItem.Image = global::TuringMachine.Res.add_random;
            this.randomToolStripMenuItem.Name = "randomToolStripMenuItem";
            this.randomToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.randomToolStripMenuItem.Text = "Random";
            this.randomToolStripMenuItem.Click += new System.EventHandler(this.randomToolStripMenuItem_Click);
            // 
            // gridConfig
            // 
            this.gridConfig.AllowUserToAddRows = false;
            this.gridConfig.AllowUserToDeleteRows = false;
            this.gridConfig.AllowUserToOrderColumns = true;
            this.gridConfig.AllowUserToResizeRows = false;
            this.gridConfig.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridConfig.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cConfigType,
            this.cConfigDescription,
            this.cConfigCount,
            this.cConfigFails,
            this.cConfigCrashes});
            this.gridConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridConfig.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.gridConfig.Location = new System.Drawing.Point(0, 34);
            this.gridConfig.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gridConfig.Name = "gridConfig";
            this.gridConfig.ReadOnly = true;
            this.gridConfig.RowHeadersVisible = false;
            this.gridConfig.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridConfig.Size = new System.Drawing.Size(464, 218);
            this.gridConfig.TabIndex = 2;
            this.gridConfig.VirtualMode = true;
            this.gridConfig.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.gridConfig_ColumnHeaderMouseClick);
            this.gridConfig.SelectionChanged += new System.EventHandler(this.gridConfig_SelectionChanged);
            this.gridConfig.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.gridConfig_SortCompare);
            // 
            // cConfigType
            // 
            this.cConfigType.DataPropertyName = "Type";
            this.cConfigType.HeaderText = "Type";
            this.cConfigType.Name = "cConfigType";
            this.cConfigType.ReadOnly = true;
            this.cConfigType.Width = 90;
            // 
            // cConfigDescription
            // 
            this.cConfigDescription.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.cConfigDescription.DataPropertyName = "Description";
            this.cConfigDescription.HeaderText = "Description";
            this.cConfigDescription.Name = "cConfigDescription";
            this.cConfigDescription.ReadOnly = true;
            // 
            // cConfigCount
            // 
            this.cConfigCount.DataPropertyName = "Tests";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.cConfigCount.DefaultCellStyle = dataGridViewCellStyle8;
            this.cConfigCount.HeaderText = "Tests";
            this.cConfigCount.Name = "cConfigCount";
            this.cConfigCount.ReadOnly = true;
            this.cConfigCount.Width = 80;
            // 
            // cConfigFails
            // 
            this.cConfigFails.DataPropertyName = "Fails";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.cConfigFails.DefaultCellStyle = dataGridViewCellStyle9;
            this.cConfigFails.HeaderText = "Fails";
            this.cConfigFails.Name = "cConfigFails";
            this.cConfigFails.ReadOnly = true;
            this.cConfigFails.Width = 55;
            // 
            // cConfigCrashes
            // 
            this.cConfigCrashes.DataPropertyName = "Crashes";
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.cConfigCrashes.DefaultCellStyle = dataGridViewCellStyle10;
            this.cConfigCrashes.HeaderText = "Crashes";
            this.cConfigCrashes.Name = "cConfigCrashes";
            this.cConfigCrashes.ReadOnly = true;
            this.cConfigCrashes.Width = 60;
            // 
            // toolStrip3
            // 
            this.toolStrip3.AutoSize = false;
            this.toolStrip3.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip3.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel3,
            this.toolStripButton2,
            this.toolStripDropDownButton2});
            this.toolStrip3.Location = new System.Drawing.Point(0, 0);
            this.toolStrip3.Name = "toolStrip3";
            this.toolStrip3.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip3.Size = new System.Drawing.Size(464, 34);
            this.toolStrip3.TabIndex = 3;
            this.toolStrip3.Text = "toolStrip3";
            // 
            // toolStripLabel3
            // 
            this.toolStripLabel3.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripLabel3.Name = "toolStripLabel3";
            this.toolStripLabel3.Size = new System.Drawing.Size(86, 31);
            this.toolStripLabel3.Text = "Configurations";
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Enabled = false;
            this.toolStripButton2.Image = global::TuringMachine.Res.trash;
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(36, 31);
            this.toolStripButton2.Text = "Remove selected";
            this.toolStripButton2.ToolTipText = "Remove selected";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // toolStripDropDownButton2
            // 
            this.toolStripDropDownButton2.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripDropDownButton2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2,
            this.toolStripMenuItem3});
            this.toolStripDropDownButton2.Image = global::TuringMachine.Res.add;
            this.toolStripDropDownButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton2.Name = "toolStripDropDownButton2";
            this.toolStripDropDownButton2.Size = new System.Drawing.Size(45, 31);
            this.toolStripDropDownButton2.ToolTipText = "Add";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Image = global::TuringMachine.Res.add_file;
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(107, 22);
            this.toolStripMenuItem2.Text = "File";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Image = global::TuringMachine.Res.add_folder;
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(107, 22);
            this.toolStripMenuItem3.Text = "Folder";
            this.toolStripMenuItem3.Click += new System.EventHandler(this.toolStripMenuItem3_Click);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 62);
            this.splitContainer2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.splitContainer1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.gridLog);
            this.splitContainer2.Panel2.Controls.Add(this.toolStrip2);
            this.splitContainer2.Size = new System.Drawing.Size(912, 498);
            this.splitContainer2.SplitterDistance = 252;
            this.splitContainer2.TabIndex = 3;
            // 
            // toolStrip2
            // 
            this.toolStrip2.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel2});
            this.toolStrip2.Location = new System.Drawing.Point(0, 0);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip2.Size = new System.Drawing.Size(912, 25);
            this.toolStrip2.TabIndex = 2;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(48, 22);
            this.toolStripLabel2.Text = "Crashes";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2});
            this.statusStrip1.Location = new System.Drawing.Point(0, 560);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(912, 22);
            this.statusStrip1.TabIndex = 4;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(85, 17);
            this.toolStripStatusLabel1.Text = "Read inputs at:";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(75, 17);
            this.toolStripStatusLabel2.Text = "0.0.0.0:7777";
            this.toolStripStatusLabel2.Click += new System.EventHandler(this.toolStripStatusLabel2_Click);
            // 
            // toolStrip4
            // 
            this.toolStrip4.AllowMerge = false;
            this.toolStrip4.AutoSize = false;
            this.toolStrip4.BackColor = System.Drawing.Color.White;
            this.toolStrip4.CanOverflow = false;
            this.toolStrip4.GripMargin = new System.Windows.Forms.Padding(0);
            this.toolStrip4.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip4.ImageScalingSize = new System.Drawing.Size(48, 48);
            this.toolStrip4.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbPlay,
            this.toolStripSeparator3,
            this.tbPause,
            this.toolStripSeparator2,
            this.tbStop});
            this.toolStrip4.Location = new System.Drawing.Point(0, 0);
            this.toolStrip4.Name = "toolStrip4";
            this.toolStrip4.Size = new System.Drawing.Size(912, 62);
            this.toolStrip4.TabIndex = 5;
            // 
            // tbPlay
            // 
            this.tbPlay.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbPlay.Image = global::TuringMachine.Res.play;
            this.tbPlay.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbPlay.Name = "tbPlay";
            this.tbPlay.Size = new System.Drawing.Size(52, 59);
            this.tbPlay.Text = "Play";
            this.tbPlay.Click += new System.EventHandler(this.tbPlay_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 62);
            // 
            // tbPause
            // 
            this.tbPause.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbPause.Enabled = false;
            this.tbPause.Image = global::TuringMachine.Res.pause;
            this.tbPause.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbPause.Name = "tbPause";
            this.tbPause.Size = new System.Drawing.Size(52, 59);
            this.tbPause.Text = "Pause";
            this.tbPause.Click += new System.EventHandler(this.tbPause_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 62);
            // 
            // tbStop
            // 
            this.tbStop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbStop.Enabled = false;
            this.tbStop.Image = global::TuringMachine.Res.stop;
            this.tbStop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbStop.Name = "tbStop";
            this.tbStop.Size = new System.Drawing.Size(52, 59);
            this.tbStop.Text = "Stop";
            this.tbStop.Click += new System.EventHandler(this.tbStop_Click);
            // 
            // chart1
            // 
            this.chart1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            chartArea1.AlignmentOrientation = System.Windows.Forms.DataVisualization.Charting.AreaAlignmentOrientations.Horizontal;
            chartArea1.AlignmentStyle = System.Windows.Forms.DataVisualization.Charting.AreaAlignmentStyles.AxesView;
            chartArea1.AxisX.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.False;
            chartArea1.AxisX.IsMarginVisible = false;
            chartArea1.AxisY.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.False;
            chartArea1.AxisY.IsMarginVisible = false;
            chartArea1.BorderWidth = 0;
            chartArea1.Name = "ChartArea1";
            chartArea1.Position.Auto = false;
            chartArea1.Position.Height = 100F;
            chartArea1.Position.Width = 88F;
            this.chart1.ChartAreas.Add(chartArea1);
            legend1.BackColor = System.Drawing.Color.Transparent;
            legend1.IsTextAutoFit = false;
            legend1.LegendStyle = System.Windows.Forms.DataVisualization.Charting.LegendStyle.Column;
            legend1.Name = "Legend1";
            legend1.Position.Auto = false;
            legend1.Position.Height = 85.2459F;
            legend1.Position.Width = 10.85482F;
            legend1.Position.X = 89F;
            legend1.Position.Y = 3F;
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(173, 0);
            this.chart1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Area;
            series1.Legend = "Legend1";
            series1.Name = "Test";
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Area;
            series2.Color = System.Drawing.Color.Red;
            series2.Legend = "Legend1";
            series2.Name = "Crash";
            series3.ChartArea = "ChartArea1";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series3.Color = System.Drawing.Color.Yellow;
            series3.Legend = "Legend1";
            series3.Name = "Fails";
            this.chart1.Series.Add(series1);
            this.chart1.Series.Add(series2);
            this.chart1.Series.Add(series3);
            this.chart1.Size = new System.Drawing.Size(738, 62);
            this.chart1.TabIndex = 6;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveInputWithToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(130, 26);
            // 
            // saveInputWithToolStripMenuItem
            // 
            this.saveInputWithToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.originalInputToolStripMenuItem});
            this.saveInputWithToolStripMenuItem.Name = "saveInputWithToolStripMenuItem";
            this.saveInputWithToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.saveInputWithToolStripMenuItem.Text = "Save input";
            this.saveInputWithToolStripMenuItem.DropDownOpening += new System.EventHandler(this.saveInputWithToolStripMenuItem_DropDownOpening);
            // 
            // originalInputToolStripMenuItem
            // 
            this.originalInputToolStripMenuItem.Name = "originalInputToolStripMenuItem";
            this.originalInputToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.originalInputToolStripMenuItem.Text = "Original input";
            this.originalInputToolStripMenuItem.Click += new System.EventHandler(this.originalInputToolStripMenuItem_Click);
            // 
            // cLogDate
            // 
            this.cLogDate.DataPropertyName = "Date";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.cLogDate.DefaultCellStyle = dataGridViewCellStyle4;
            this.cLogDate.HeaderText = "Date";
            this.cLogDate.Name = "cLogDate";
            this.cLogDate.ReadOnly = true;
            this.cLogDate.Width = 110;
            // 
            // cLogOrigin
            // 
            this.cLogOrigin.DataPropertyName = "Origin";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.cLogOrigin.DefaultCellStyle = dataGridViewCellStyle5;
            this.cLogOrigin.HeaderText = "Origin";
            this.cLogOrigin.Name = "cLogOrigin";
            this.cLogOrigin.ReadOnly = true;
            this.cLogOrigin.Width = 90;
            // 
            // cLogType
            // 
            this.cLogType.DataPropertyName = "Type";
            this.cLogType.HeaderText = "Type";
            this.cLogType.Name = "cLogType";
            this.cLogType.ReadOnly = true;
            this.cLogType.Width = 45;
            // 
            // cExploitable
            // 
            this.cExploitable.DataPropertyName = "ExplotationResult";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.cExploitable.DefaultCellStyle = dataGridViewCellStyle6;
            this.cExploitable.HeaderText = "Exploitable";
            this.cExploitable.Name = "cExploitable";
            this.cExploitable.ReadOnly = true;
            this.cExploitable.Width = 120;
            // 
            // cLogInput
            // 
            this.cLogInput.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.cLogInput.DataPropertyName = "Input";
            this.cLogInput.HeaderText = "Input";
            this.cLogInput.Name = "cLogInput";
            this.cLogInput.ReadOnly = true;
            // 
            // cInputConfig
            // 
            this.cInputConfig.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.cInputConfig.DataPropertyName = "Config";
            this.cInputConfig.HeaderText = "Config";
            this.cInputConfig.Name = "cInputConfig";
            this.cInputConfig.ReadOnly = true;
            // 
            // cLogGoto
            // 
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.NullValue = ">";
            this.cLogGoto.DefaultCellStyle = dataGridViewCellStyle7;
            this.cLogGoto.HeaderText = "Open";
            this.cLogGoto.Name = "cLogGoto";
            this.cLogGoto.ReadOnly = true;
            this.cLogGoto.ToolTipText = "Open path";
            this.cLogGoto.Width = 65;
            // 
            // FMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(912, 582);
            this.Controls.Add(this.chart1);
            this.Controls.Add(this.splitContainer2);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip4);
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MinimumSize = new System.Drawing.Size(688, 388);
            this.Name = "FMain";
            this.ShowIcon = false;
            this.Text = "Turing Machine";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FMain_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.gridInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridLog)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridConfig)).EndInit();
            this.toolStrip3.ResumeLayout(false);
            this.toolStrip3.PerformLayout();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.toolStrip4.ResumeLayout(false);
            this.toolStrip4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView gridInput;
        private System.Windows.Forms.DataGridView gridLog;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.DataGridView gridConfig;
        private System.Windows.Forms.ToolStrip toolStrip3;
        private System.Windows.Forms.ToolStripLabel toolStripLabel3;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStrip toolStrip4;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem folderToolStripMenuItem;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripMenuItem tcpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem executeToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton tbPlay;
        private System.Windows.Forms.ToolStripButton tbPause;
        private System.Windows.Forms.ToolStripButton tbStop;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ToolStripMenuItem randomToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem saveInputWithToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem originalInputToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn cInputsType;
        private System.Windows.Forms.DataGridViewTextBoxColumn cInputDescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn cInputCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn cInputFails;
        private System.Windows.Forms.DataGridViewTextBoxColumn cInputCrashes;
        private System.Windows.Forms.DataGridViewTextBoxColumn cConfigType;
        private System.Windows.Forms.DataGridViewTextBoxColumn cConfigDescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn cConfigCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn cConfigFails;
        private System.Windows.Forms.DataGridViewTextBoxColumn cConfigCrashes;
        private System.Windows.Forms.DataGridViewTextBoxColumn cLogDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn cLogOrigin;
        private System.Windows.Forms.DataGridViewTextBoxColumn cLogType;
        private System.Windows.Forms.DataGridViewTextBoxColumn cExploitable;
        private System.Windows.Forms.DataGridViewTextBoxColumn cLogInput;
        private System.Windows.Forms.DataGridViewTextBoxColumn cInputConfig;
        private System.Windows.Forms.DataGridViewButtonColumn cLogGoto;
    }
}

