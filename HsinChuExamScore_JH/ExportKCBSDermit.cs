using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using Aspose.Cells;
using K12.Data;
using System.Xml;
using System.Data;
using FISCA.Data;
using System.Xml.Linq;
using System.ComponentModel;
using FISCA.Presentation.Controls;
using FISCA.UDT;

namespace HsinChuExamScore_JH
{    
    // 匯出康橋獎懲
    class ExportKCBSDermit
    {
        private BackgroundWorker _worker;
        private List<string> _stuIDList;        

        public ExportKCBSDermit(List<string> stuIDList)
        {
            _stuIDList = stuIDList;
        }

        public void export()
        {
            _worker = new BackgroundWorker();
            _worker.DoWork += new DoWorkEventHandler(Worker_DoWork);
            _worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(Worker_RunWorkerCompleted);
            _worker.ProgressChanged += new ProgressChangedEventHandler(Worker_ProgressChanged);
            _worker.WorkerReportsProgress = true;
            _worker.RunWorkerAsync();
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            _worker.ReportProgress(0, "取得學生康橋獎懲資料...");

            string stuIDs = string.Join(",", _stuIDList);

            // 將目前選擇學生的 康橋缺曠紀錄取出                
            AccessHelper _AccessHelper = new AccessHelper();
            string query = "ref_student_id IN (" + stuIDs + ")";

            List<DAO.UDT_KCBSDermit> retVal = new List<DAO.UDT_KCBSDermit>();
            retVal = _AccessHelper.Select<DAO.UDT_KCBSDermit>(query);


            List<K12.Data.StudentRecord> srList = K12.Data.Student.SelectByIDs(_stuIDList);

            // 學生ID 資料　對照　字典
            Dictionary<string, K12.Data.StudentRecord> srDict = new Dictionary<string, StudentRecord>();

            foreach (K12.Data.StudentRecord record in srList)
            {
                srDict.Add(record.ID, record);
            }

                 

            Workbook book = new Workbook();
            book.Worksheets.Clear();
            Worksheet ws = book.Worksheets[book.Worksheets.Add()];
            ws.Name = "康橋獎懲資料匯出";

            List<string> colheaderList = new List<string>();

            colheaderList.Add("姓名");
            colheaderList.Add("班級");
            colheaderList.Add("座號");
            colheaderList.Add("學號");
            colheaderList.Add("在學狀態");
            colheaderList.Add("發生日期");
            colheaderList.Add("懲戒層級");
            colheaderList.Add("懲戒單號");
            colheaderList.Add("是否註銷");
            colheaderList.Add("最後修正日期");
            
            int columnIndex = 0;

            // 加入表頭
            foreach (string header in colheaderList)
            {                
                ws.Cells[0, columnIndex].PutValue(header);
                columnIndex++;
            }

            _worker.ReportProgress(80, "產生Excel報表...");

            //整理資料
            if (retVal.Count > 0)
            {
                int rowIndex = 1; //0為表頭，這裡從1 開始

                foreach (DAO.UDT_KCBSDermit record in retVal)
                {
                    #region 填入內容

                    

                    K12.Data.StudentRecord sr = srDict[""+record.Ref_student_id];

                    //姓名
                    ws.Cells[rowIndex, 0].PutValue(sr.Name);
                    //班級
                    if (sr.Class != null)
                    {
                        ws.Cells[rowIndex, 1].PutValue(sr.Class.Name);
                    }                    
                    //座號
                    ws.Cells[rowIndex, 2].PutValue(sr.SeatNo);
                    //學號
                    ws.Cells[rowIndex, 3].PutValue(sr.StudentNumber);
                    //狀態
                    ws.Cells[rowIndex, 4].PutValue(sr.StatusStr);
                    //發生日期
                    ws.Cells[rowIndex, 5].PutValue(record.Occur_date.ToShortDateString());
                    //懲戒層級
                    ws.Cells[rowIndex, 6].PutValue(record.LevelNum);
                    //懲戒單號
                    ws.Cells[rowIndex, 7].PutValue(record.ref_Assota_NO);
                    //是否註銷
                    string deleted = record.IsDelete ? "是" : "否";
                    ws.Cells[rowIndex, 8].PutValue(deleted);
                    //最後修正日期
                    ws.Cells[rowIndex, 9].PutValue(record.LastUpdate.ToShortDateString());

                    rowIndex++;
                    
                    #endregion
                }
            }

            ws.AutoFitColumns(); // 使 匯出excel 自動調整 欄寬

            e.Result = book;

            _worker.ReportProgress(100, "康橋獎懲資料匯出，產生完成。");
        }


        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Workbook book = new Workbook();

            if (e.Result == null)
            {               
                return;
            }
            else
            {
                book = (Workbook)e.Result;
            }
            
            SaveFileDialog sd = new SaveFileDialog();
            sd.FileName = "康橋獎懲資料匯出";
            sd.Filter = "Excel檔案(*.xlsx)|*.xlsx";
            if (sd.ShowDialog() == DialogResult.OK)
            {
                DialogResult result = new DialogResult();

                try
                {
                    book.Save(sd.FileName, SaveFormat.Xlsx);
                    result = MsgBox.Show("檔案儲存完成，是否開啟檔案?", "是否開啟", MessageBoxButtons.YesNo);
                }
                catch (Exception ex)
                {
                    MsgBox.Show("儲存失敗。" + ex.Message);
                }

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        System.Diagnostics.Process.Start(sd.FileName);
                    }
                    catch (Exception ex)
                    {
                        MsgBox.Show("開啟檔案發生失敗:" + ex.Message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            FISCA.Presentation.MotherForm.SetStatusBarMessage(""+e.UserState, e.ProgressPercentage);
        }
    }
}
