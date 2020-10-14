using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FISCA.Presentation;
using JHSchool;
using JHSchool.Data;
using FISCA.Presentation.Controls;
using FISCA.UDT;

namespace HsinChuExamScore_JH
{
    [FISCA.Permission.FeatureCode("K12.Student.DemeritItem_KCBS", "康橋懲戒記錄")]
    public partial class DemeritItemKCBS : DetailContent
    {
        private BackgroundWorker _worker;
      
        private bool _reseting = false;
        private string _RunningID;
        List<DAO.UDT_KCBSDermit> retVal = new List<DAO.UDT_KCBSDermit>();

        public DemeritItemKCBS()
        {
            InitializeComponent();

            InitializeBackgroundWorker();
     
            _RunningID = string.Empty;
        }

        private void InitializeBackgroundWorker()
        {
            _worker = new BackgroundWorker();
            _worker.DoWork += delegate(object sender, DoWorkEventArgs e)
            {
                // 將目前選擇學生的 康橋缺曠紀錄取出                
                AccessHelper _AccessHelper = new AccessHelper();
                string query = "ref_student_id='" + PrimaryKey + "'";
                retVal = _AccessHelper.Select<DAO.UDT_KCBSDermit>(query);
  
            };
            _worker.RunWorkerCompleted += delegate(object sender, RunWorkerCompletedEventArgs e)
            {
                if (_RunningID != PrimaryKey)
                {
                    _RunningID = PrimaryKey;
                    _worker.RunWorkerAsync(_RunningID);
                    return;
                }

                ResetListView();
                FillListView();
            };
        }

        protected override void OnPrimaryKeyChanged(EventArgs e)
        {
            if (!_worker.IsBusy)
            {
                _RunningID = PrimaryKey;
                _worker.RunWorkerAsync();
            }
            base.OnPrimaryKeyChanged(e);
        }

        private void ResetListView()
        {
            _reseting = true;

            listView.SuspendLayout();
            listView.Items.Clear();
            listView.ResumeLayout();

            _reseting = false;
        }


        private void FillListView()
        {
            foreach (DAO.UDT_KCBSDermit record in retVal)
            {
                // 是否已註銷
                string deleted = record.IsDelete ? "是" : "否";
                ListViewItem item = new ListViewItem(new string[] { "" + record.Occur_date.ToShortDateString(), "" + record.LevelNum, "" + record.ref_Assota_NO, "" + deleted, "" + record.LastUpdate.ToShortDateString() });
                
                listView.Items.Add(item);
            }
        }



        private void Filter_SelectedIndexChanged(object sender, EventArgs e)
        {           
            listView.ResumeLayout();
        }








    }
}
