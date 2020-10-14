using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FISCA.UDT;

namespace HsinChuExamScore_JH.DAO
{
    [TableName("kcbsdermitcomparison")]
    public class UDT_KCBSDermitComparison : ActiveRecord
    {

        // 2020/10/14 ，本UDT 為穎驊 受康橋怡芬、明裕所託建立，用意為對接Assota 系統的資料，
        // 在ishool 端 只檢視 還有出報表， 新增與更新的部分 將由康橋端 每日自動更新資料庫
        // 此表僅有兩個欄位，　對應１～６級，及其各學校設定名稱（ＥＸ：白１、白２、藍１、藍２、紅１、紅２）
      
        /// <summary>
        /// 懲戒層級
        /// </summary>
        [Field(Field = "LevelNum", Indexed = false)]
        public string LevelNum { get; set; }

        /// <summary>
        /// 懲戒名稱
        /// </summary>
        [Field(Field = "name", Indexed = false)]
        public string Name { get; set; }






    }
}
