namespace HsinChuExamScore_JH
{
    partial class DemeritItemKCBS
    {

        /// <summary> 
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 元件設計工具產生的程式碼

        /// <summary> 
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改這個方法的內容。
        ///
        /// </summary>
        private void InitializeComponent()
        {
            this.listView = new DevComponents.DotNetBar.Controls.ListViewEx();
            this.chOccurTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chDermitLevel = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chIsDelete = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chLastUpdateTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chAssotaNo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // listView
            // 
            // 
            // 
            // 
            this.listView.Border.Class = "ListViewBorder";
            this.listView.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chOccurTime,
            this.chDermitLevel,
            this.chAssotaNo,
            this.chIsDelete,
            this.chLastUpdateTime});
            this.listView.FullRowSelect = true;
            this.listView.HideSelection = false;
            this.listView.Location = new System.Drawing.Point(15, 41);
            this.listView.MultiSelect = false;
            this.listView.Name = "listView";
            this.listView.Size = new System.Drawing.Size(520, 150);
            this.listView.TabIndex = 0;
            this.listView.UseCompatibleStateImageBehavior = false;
            this.listView.View = System.Windows.Forms.View.Details;
            // 
            // chOccurTime
            // 
            this.chOccurTime.DisplayIndex = 0;
            this.chOccurTime.Text = "發生日期";
            this.chOccurTime.Width = 120;
            // 
            // chDermitLevel
            // 
            this.chDermitLevel.DisplayIndex = 1;
            this.chDermitLevel.Text = "懲戒層級";
            this.chDermitLevel.Width = 80;
            // 
            // chIsDelete
            // 
            this.chIsDelete.DisplayIndex = 3;
            this.chIsDelete.Text = "是否註銷";
            this.chIsDelete.Width = 80;
            // 
            // chLastUpdateTime
            // 
            this.chLastUpdateTime.DisplayIndex = 4;
            this.chLastUpdateTime.Text = "最後修正日期";
            this.chLastUpdateTime.Width = 160;
            // 
            // chAssotaNo
            // 
            this.chAssotaNo.DisplayIndex = 2;
            this.chAssotaNo.Text = "懲戒單號";
            this.chAssotaNo.Width = 120;
            // 
            // DemeritItemKCBS
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.listView);
            this.Group = "康橋懲戒記錄";
            this.Name = "DemeritItemKCBS";
            this.Size = new System.Drawing.Size(550, 230);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.ListViewEx listView;
        private System.Windows.Forms.ColumnHeader chOccurTime;
        private System.Windows.Forms.ColumnHeader chDermitLevel;
        private System.Windows.Forms.ColumnHeader chIsDelete;
        private System.Windows.Forms.ColumnHeader chLastUpdateTime;
        private System.Windows.Forms.ColumnHeader chAssotaNo;
    }
}
