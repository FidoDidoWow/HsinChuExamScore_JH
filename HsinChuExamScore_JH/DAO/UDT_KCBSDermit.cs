using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FISCA.UDT;

namespace HsinChuExamScore_JH.DAO
{
    [TableName("kcbsdermit")]
    public class UDT_KCBSDermit : ActiveRecord
    {

        // 2020/10/14 ，本UDT 為穎驊 受康橋怡芬、明裕所託建立，用意為對接Assota 系統的資料，
        // 在ishool 端 只檢視 還有出報表， 新增與更新的部分 將由康橋端 每日自動更新資料庫
        // 另外ischool UDT 的概念原本就有  Last_Update 的欄位，是在ischool系統中 這一列資料被更動的時間記錄
        // 而對接來的資料 LastUpdate >> 則是在 Assota 系統資料庫的更新時間，此點必須注意

        /// <summary>
        /// 學生系統流水號
        /// </summary>
        [Field(Field = "ref_student_id", Indexed = false)]
        public int Ref_student_id { get; set; }

        /// <summary>
        /// 發生日期
        /// </summary>
        [Field(Field = "occur_date", Indexed = false)]
        public DateTime Occur_date { get; set; }

        /// <summary>
        /// 懲戒單號
        /// </summary>
        [Field(Field = "ref_Assota_NO", Indexed = false)]
        public string ref_Assota_NO { get; set; }

        /// <summary>
        /// 懲戒層級
        /// </summary>
        [Field(Field = "LevelNum", Indexed = false)]
        public int LevelNum { get; set; }

        /// <summary>
        /// 是否註銷
        /// </summary>
        [Field(Field = "IsDelete", Indexed = false)]
        public bool IsDelete { get; set; }

        /// <summary>
        /// LastUpdate
        /// </summary>
        [Field(Field = "LastUpdate", Indexed = false)]
        public DateTime LastUpdate { get; set; }



    }
}
