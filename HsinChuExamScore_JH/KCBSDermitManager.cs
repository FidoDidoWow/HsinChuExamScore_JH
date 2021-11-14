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

        // 選擇學生範圍　懲戒清單
        List<DAO.UDT_finalTotalKCBSDermit> finaldermitList = new List<DAO.UDT_finalTotalKCBSDermit>();

        // 六大項康橋懲戒對照
        List<DAO.UDT_KCBSDermitComparison> dermitComparisonList = new List<DAO.UDT_KCBSDermitComparison>();

        // <學生ID,懲戒 KEY 值,Value>
        Dictionary<string, Dictionary<string, int>> dermitDict = new Dictionary<string, Dictionary<string, int>>();

        // <學生ID,最終懲戒 KEY 值,Value>
        Dictionary<string, Dictionary<string, string>> finaldermitDict = new Dictionary<string, Dictionary<string, string>>();

        // 1~6 級　對應變數中文
        Dictionary<string, string> dermitComparisonDict = new Dictionary<string, string>();

        // 各康橋學校設定懲戒名稱List
        List<DAO.UDT_KCBSDermitComparison> dermitNameSettingList = new List<DAO.UDT_KCBSDermitComparison>();

        // 各康橋學校設定懲戒名稱Dict
        Dictionary<string, string> dermitNameSettingDict = new Dictionary<string, string>();

        DateTime beginDate;
        DateTime endDate;

        public KCBSDermitManager(DateTime bd, DateTime ed)
        {
            beginDate = bd;
            endDate = ed;

            dermitComparisonDict.Add("1", "康橋1級懲戒支數");
            dermitComparisonDict.Add("2", "康橋2級懲戒支數");
            dermitComparisonDict.Add("3", "康橋3級懲戒支數");
            dermitComparisonDict.Add("4", "康橋4級懲戒支數");
            dermitComparisonDict.Add("5", "康橋5級懲戒支數");
            dermitComparisonDict.Add("6", "康橋6級懲戒支數");

            // 全部把該學校的設定抓出來，
            AccessHelper _AccessHelper = new AccessHelper();
            dermitNameSettingList = _AccessHelper.Select<DAO.UDT_KCBSDermitComparison>();

            // 預設
            dermitNameSettingDict.Add("0", "無"); // 討論後，決定 累計0 顯示無
            dermitNameSettingDict.Add("1", "第一級");
            dermitNameSettingDict.Add("2", "第二級");
            dermitNameSettingDict.Add("3", "第三級");
            dermitNameSettingDict.Add("4", "第四級");
            dermitNameSettingDict.Add("5", "第五級");
            dermitNameSettingDict.Add("6", "第六級");

            foreach (DAO.UDT_KCBSDermitComparison record in dermitNameSettingList)
            {
                dermitNameSettingDict[record.LevelNum] = record.Name;
            }

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

            row["康橋1級懲戒名稱"] = dermitNameSettingDict["1"];
            row["康橋2級懲戒名稱"] = dermitNameSettingDict["2"];
            row["康橋3級懲戒名稱"] = dermitNameSettingDict["3"];
            row["康橋4級懲戒名稱"] = dermitNameSettingDict["4"];
            row["康橋5級懲戒名稱"] = dermitNameSettingDict["5"];
            row["康橋6級懲戒名稱"] = dermitNameSettingDict["6"];

            row["康橋1級懲戒支數"] = dermitDict[stuid]["康橋1級懲戒支數"];
            row["康橋2級懲戒支數"] = dermitDict[stuid]["康橋2級懲戒支數"];
            row["康橋3級懲戒支數"] = dermitDict[stuid]["康橋3級懲戒支數"];
            row["康橋4級懲戒支數"] = dermitDict[stuid]["康橋4級懲戒支數"];
            row["康橋5級懲戒支數"] = dermitDict[stuid]["康橋5級懲戒支數"];
            row["康橋6級懲戒支數"] = dermitDict[stuid]["康橋6級懲戒支數"];

            //// 看最後累計多少支 直接對應 該級數懲戒名稱
            //if (dermitDict[stuid]["康橋累計懲戒"] <= 6)
            //{
            //    row["康橋累計懲戒"] = dermitNameSettingDict["" + dermitDict[stuid]["康橋累計懲戒"]];
            //}
            //else
            //{
            //    // 超過6 通通以最高級 6 來記
            //    row["康橋累計懲戒"] = "超過" + dermitNameSettingDict["6"];
            //}

            row["康橋累計懲戒"] = finaldermitDict[stuid]["康橋累計懲戒"];

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

            finaldermitList = _AccessHelper.Select<DAO.UDT_finalTotalKCBSDermit>(query);

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

            foreach (string sid in _stuIDList)
            {
                finaldermitDict.Add(sid, new Dictionary<string, string>());
                finaldermitDict[sid].Add("康橋累計懲戒", "無");
            }

            foreach (DAO.UDT_KCBSDermit record in dermitList)
            {
                // 銷過不採計
                if (record.IsDelete)
                {
                    continue;
                }

                // 僅將範圍時間內 列入
                if (record.Occur_date.Date >= beginDate && record.Occur_date.Date <= endDate)
                {
                    valueTransfer(record, record.LevelNum);
                }
            }

            foreach (DAO.UDT_finalTotalKCBSDermit record in finaldermitList)
            {
                finaldermitDict["" + record.Ref_student_id]["康橋累計懲戒"] = record.LastStatus;
            }
        }

        public void valueTransfer(DAO.UDT_KCBSDermit record, int LevelNum)
        {
            //2021/06/21 穎驊註解，累計方式改變，0級不必再累積計算
            //2021/11/14 穎驊註解，康橋明裕反映 可能會有 7級以上，不能有錯誤訊息，但也不需列印出來
            if (LevelNum != 0 && dermitComparisonDict.ContainsKey("" + LevelNum))
            {
                dermitDict["" + record.Ref_student_id][dermitComparisonDict["" + LevelNum]] += 1;
            }
            //dermitDict["" + record.Ref_student_id]["康橋累計懲戒"] += LevelNum; //　把所有級別加起來
        }
    }
}
