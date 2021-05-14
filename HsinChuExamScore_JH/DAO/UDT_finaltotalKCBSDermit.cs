using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FISCA.UDT;

namespace HsinChuExamScore_JH.DAO
{
    [TableName("finaltotalkcbsdermit")]
    public class UDT_finalTotalKCBSDermit : ActiveRecord
    {

        // 2021/05/13 ，依照新需求增列最終累積康橋懲戒

        /// <summary>
        /// 學生系統流水號
        /// </summary>
        [Field(Field = "ref_student_id", Indexed = false)]
        public int Ref_student_id { get; set; }
   
        /// <summary>
        /// 懲戒單號
        /// </summary>
        [Field(Field = "lastStatus", Indexed = false)]
        public string LastStatus { get; set; }

        // 取得最後更新時間(ischool UDT 內建)
        /// <summary>
        /// LastUpdate
        /// </summary>
        [Field(Field = "last_update", Indexed = false)]
        public DateTime LastUpdate { get; set; }

    }
}
