﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using FISCA.Permission;
using FISCA.Presentation;
using K12.Data;
using FISCA.UDT;

namespace HsinChuExamScore_JH
{
    /// <summary>
    /// 新竹評量成績單
    /// </summary>
    public class Program
    {
        static DataTable _dtEpost = new DataTable();


        [FISCA.MainMethod]
        public static void Main()
        {
            // 先初始化
            AccessHelper _AccessHelper = new AccessHelper();

            List<HsinChuExamScore_JH.DAO.UDT_KCBSDermit> retVal1 = _AccessHelper.Select<DAO.UDT_KCBSDermit>();
            List<HsinChuExamScore_JH.DAO.UDT_KCBSDermitComparison> retVal2 = _AccessHelper.Select<DAO.UDT_KCBSDermitComparison>();
            List<HsinChuExamScore_JH.DAO.UDT_finalTotalKCBSDermit> retVal3 = _AccessHelper.Select<DAO.UDT_finalTotalKCBSDermit>();


            RibbonBarItem rbItem1 = MotherForm.RibbonBarItems["學生", "資料統計"];
            rbItem1["報表"]["成績相關報表"]["成績通知單(康橋)"]["評量成績通知單(固定排名)(康橋懲戒)"].Enable = UserAcl.Current["JH.Student.HsinChuExamScore_JH_Student_kcbs"].Executable;
            rbItem1["報表"]["成績相關報表"]["成績通知單(康橋)"]["評量成績通知單(固定排名)(康橋懲戒)"].Click += delegate
            {
                if (K12.Presentation.NLDPanels.Student.SelectedSource.Count > 0 && K12.Presentation.NLDPanels.Student.SelectedSource.Count < 111)
                {
                    PrintForm pf = new PrintForm(K12.Presentation.NLDPanels.Student.SelectedSource);
                    pf.ShowDialog();
                }
                else if (K12.Presentation.NLDPanels.Student.SelectedSource.Count > 110)
                {
                    FISCA.Presentation.Controls.MsgBox.Show("請選擇110位以下學生");
                    return;
                }
                else
                {
                    FISCA.Presentation.Controls.MsgBox.Show("請選擇選學生");
                    return;
                }
            };

            RibbonBarItem rbItem2 = MotherForm.RibbonBarItems["班級", "資料統計"];
            rbItem2["報表"]["成績相關報表"]["成績通知單(康橋)"]["評量成績通知單(固定排名)(康橋懲戒)"].Enable = UserAcl.Current["JH.Student.HsinChuExamScore_JH_Class_kcbs"].Executable;
            rbItem2["報表"]["成績相關報表"]["成績通知單(康橋)"]["評量成績通知單(固定排名)(康橋懲戒)"].Click += delegate
            {
                if (K12.Presentation.NLDPanels.Class.SelectedSource.Count > 0 && K12.Presentation.NLDPanels.Class.SelectedSource.Count < 4)
                {
                    List<string> StudentIDList = Utility.GetClassStudentIDList1ByClassID(K12.Presentation.NLDPanels.Class.SelectedSource);
                    PrintForm pf = new PrintForm(StudentIDList);
                    pf.ShowDialog();
                }
                else if (K12.Presentation.NLDPanels.Class.SelectedSource.Count > 3)
                {
                    FISCA.Presentation.Controls.MsgBox.Show("請選擇最多3個班級");
                    return;
                }
                else
                {
                    FISCA.Presentation.Controls.MsgBox.Show("請選擇選班級");
                    return;
                }

            };
            // 評量成績通知單
            Catalog catalog1a = RoleAclSource.Instance["學生"]["功能按鈕"];
            catalog1a.Add(new RibbonFeature("JH.Student.HsinChuExamScore_JH_Student_kcbs", "評量成績通知單(固定排名)(康橋懲戒)"));

            // 評量成績通知單
            Catalog catalog1b = RoleAclSource.Instance["班級"]["功能按鈕"];
            catalog1b.Add(new RibbonFeature("JH.Student.HsinChuExamScore_JH_Class_kcbs", "評量成績通知單(固定排名)(康橋懲戒)"));


            RibbonBarItem rbItem3 = MotherForm.RibbonBarItems["學生", "資料統計"];
            rbItem3["匯出"]["學務相關匯出"]["匯出康橋懲戒紀錄"].Enable = UserAcl.Current["JH.Student.HsinChuExamScore_JH_Student_kcbs_demrit_export"].Executable;
            rbItem3["匯出"]["學務相關匯出"]["匯出康橋懲戒紀錄"].Click += delegate
            {
                if (K12.Presentation.NLDPanels.Student.SelectedSource.Count > 0)
                {
                    ExportKCBSDermit exporter = new ExportKCBSDermit(K12.Presentation.NLDPanels.Student.SelectedSource);

                    exporter.export();
                }
                else
                {
                    FISCA.Presentation.Controls.MsgBox.Show("請選擇選學生");
                    return;
                }
            };

            // 評量成績通知單
            Catalog catalog1c = RoleAclSource.Instance["學生"]["功能按鈕"];
            catalog1c.Add(new RibbonFeature("JH.Student.HsinChuExamScore_JH_Student_kcbs_demrit_export", "匯出康橋懲戒紀錄"));



            Catalog ribbon = RoleAclSource.Instance["學生"]["資料項目"];
            ribbon.Add(new FISCA.Permission.DetailItemFeature("K12.Student.DemeritItem_KCBS", "康橋懲戒記錄"));

            FISCA.Permission.FeatureAce UserPermission;

            //懲戒記錄
            UserPermission = FISCA.Permission.UserAcl.Current["K12.Student.DemeritItem_KCBS"];
            if (UserPermission.Editable || UserPermission.Viewable)
                K12.Presentation.NLDPanels.Student.AddDetailBulider(new DetailBulider<DemeritItemKCBS>());
        }

    }
}
