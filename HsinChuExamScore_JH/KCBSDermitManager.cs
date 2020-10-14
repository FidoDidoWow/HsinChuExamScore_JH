using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using FISCA.UDT;

namespace HsinChuExamScore_JH
{
    // 專門處理　康橋缺曠
    public class KCBSDermitManager
    {
        DataTable kcbsdt;

        // 選擇學生範圍　懲戒清單
        List<DAO.UDT_KCBSDermit> dermitList = new List<DAO.UDT_KCBSDermit>();

        // 六大項康橋懲戒對照
        List<DAO.UDT_KCBSDermitComparison> dermitComparisonList = new List<DAO.UDT_KCBSDermitComparison>();

        // <學生ID,懲戒 KEY 值,Value>
        Dictionary<string, Dictionary<string, int>> dermitDict = new Dictionary<string, Dictionary<string, int>>();

        // 1~6 級　對應變數中文
        Dictionary<string, string> ComparisonDict = new Dictionary<string, string>();

        public KCBSDermitManager()
        {
            ComparisonDict.Add("1", "康橋1級懲戒支數");
            ComparisonDict.Add("2", "康橋2級懲戒支數");
            ComparisonDict.Add("3", "康橋3級懲戒支數");
            ComparisonDict.Add("4", "康橋4級懲戒支數");
            ComparisonDict.Add("5", "康橋5級懲戒支數");
            ComparisonDict.Add("6", "康橋6級懲戒支數");
        }

        public DataTable NewKCBSTable(DataTable dt)
        {
            kcbsdt = dt;
            //　增加康橋專屬的欄位
            dt.Columns.Add("康橋1級懲戒名稱");
            dt.Columns.Add("康橋2級懲戒名稱");
            dt.Columns.Add("康橋3級懲戒名稱");
            dt.Columns.Add("康橋4級懲戒名稱");
            dt.Columns.Add("康橋5級懲戒名稱");
            dt.Columns.Add("康橋6級懲戒名稱");

            dt.Columns.Add("康橋1級懲戒支數");
            dt.Columns.Add("康橋2級懲戒支數");
            dt.Columns.Add("康橋3級懲戒支數");
            dt.Columns.Add("康橋4級懲戒支數");
            dt.Columns.Add("康橋5級懲戒支數");
            dt.Columns.Add("康橋6級懲戒支數");

            dt.Columns.Add("康橋累計懲戒");
            return kcbsdt;
        }

        //加入康橋的ROW 內容
        public DataRow NewKCBSROW(DataRow row)
        {
            string stuid = "" + row["StudentID"];

            row["康橋1級懲戒支數"] = dermitDict[stuid]["康橋1級懲戒支數"];
            row["康橋2級懲戒支數"] = dermitDict[stuid]["康橋2級懲戒支數"];
            row["康橋3級懲戒支數"] = dermitDict[stuid]["康橋3級懲戒支數"];
            row["康橋4級懲戒支數"] = dermitDict[stuid]["康橋4級懲戒支數"];
            row["康橋5級懲戒支數"] = dermitDict[stuid]["康橋5級懲戒支數"];
            row["康橋6級懲戒支數"] = dermitDict[stuid]["康橋6級懲戒支數"];
            row["康橋累計懲戒"] = dermitDict[stuid]["康橋累計懲戒"];

            return row;
        }

        

        // 取得康橋懲戒資料
        public void GetKCBSDermit(List<string> _stuIDList)
        {
            AccessHelper _AccessHelper = new AccessHelper();

            dermitComparisonList = _AccessHelper.Select<DAO.UDT_KCBSDermitComparison>();


            string stuIDs = string.Join(",", _stuIDList);

            // 將目前選擇學生的 康橋缺曠紀錄取出                            
            string query = "ref_student_id IN (" + stuIDs + ")";

            dermitList = _AccessHelper.Select<DAO.UDT_KCBSDermit>(query);

            foreach (string sid in _stuIDList)
            {
                dermitDict.Add(sid, new Dictionary<string, int>());
                dermitDict[sid].Add("康橋1級懲戒支數", 0);
                dermitDict[sid].Add("康橋2級懲戒支數", 0);
                dermitDict[sid].Add("康橋3級懲戒支數", 0);
                dermitDict[sid].Add("康橋4級懲戒支數", 0);
                dermitDict[sid].Add("康橋5級懲戒支數", 0);
                dermitDict[sid].Add("康橋6級懲戒支數", 0);
                dermitDict[sid].Add("康橋累計懲戒", 0);

            }

            foreach (DAO.UDT_KCBSDermit record in dermitList)
            {
                // 銷過不採計
                if (record.IsDelete)
                {
                    continue;
                }
                                           
                valueTransfer(record, record.LevelNum);                
            }
        }

        public void valueTransfer(DAO.UDT_KCBSDermit record, int LevelNum)
        {
            dermitDict["" + record.Ref_student_id][ComparisonDict["" + LevelNum]] += 1;
            dermitDict["" + record.Ref_student_id]["康橋累計懲戒"] += LevelNum; //　把所有級別加起來
        }
    }
}
