using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HiyoshiCfhWeb.Models
{
    public static class Experience
    {

        private static int[] nextArray = {
                                             #region 経験値表(長いので折りたたみ)
                                             0, // ダミー
                                             100,
                                             200,
                                             300,
                                             400,
                                             500,
                                             600,
                                             700,
                                             800,
                                             900,
                                             1000,
                                             1100,
                                             1200,
                                             1300,
                                             1400,
                                             1500,
                                             1600,
                                             1700,
                                             1800,
                                             1900,
                                             2000,
                                             2100,
                                             2200,
                                             2300,
                                             2400,
                                             2500,
                                             2600,
                                             2700,
                                             2800,
                                             2900,
                                             3000,
                                             3100,
                                             3200,
                                             3300,
                                             3400,
                                             3500,
                                             3600,
                                             3700,
                                             3800,
                                             3900,
                                             4000,
                                             4100,
                                             4200,
                                             4300,
                                             4400,
                                             4500,
                                             4600,
                                             4700,
                                             4800,
                                             4900,
                                             5000,
                                             5200,
                                             5400,
                                             5600,
                                             5800,
                                             6000,
                                             6200,
                                             6400,
                                             6600,
                                             6800,
                                             7000,
                                             7300,
                                             7600,
                                             7900,
                                             8200,
                                             8500,
                                             8800,
                                             9100,
                                             9400,
                                             9700,
                                             10000,
                                             10400,
                                             10800,
                                             11200,
                                             11600,
                                             12000,
                                             12400,
                                             12800,
                                             13200,
                                             13600,
                                             14000,
                                             14500,
                                             15000,
                                             15500,
                                             16000,
                                             16500,
                                             17000,
                                             17500,
                                             18000,
                                             18500,
                                             19000,
                                             20000,
                                             22000,
                                             25000,
                                             30000,
                                             40000,
                                             60000,
                                             90000,
                                             148500,
                                             0, // Lv.99
                                             10000,
                                             1000,
                                             2000,
                                             3000,
                                             4000,
                                             5000,
                                             6000,
                                             7000,
                                             8000,
                                             9000,
                                             10000,
                                             12000,
                                             14000,
                                             16000,
                                             18000,
                                             20000,
                                             23000,
                                             26000,
                                             29000,
                                             32000,
                                             35000,
                                             39000,
                                             43000,
                                             47000,
                                             51000,
                                             55000,
                                             59000,
                                             63000,
                                             67000,
                                             71000,
                                             75000,
                                             80000,
                                             85000,
                                             90000,
                                             95000,
                                             100000,
                                             105000,
                                             110000,
                                             115000,
                                             120000,
                                             127000,
                                             134000,
                                             141000,
                                             148000,
                                             155000,
                                             163000,
                                             171000,
                                             179000,
                                             187000,
                                             195000, // Lv.149
                                             204000,
                                             213000,
                                             222000,
                                             231000,
                                             240000, // Lv.154
                                             #endregion
                                         };

        private static int[] totalArray = new int[nextArray.Length + 1];

        static Experience()
        {
            int sum = 0;
            totalArray[0] = 0;
            for (var i = 0; i < nextArray.Length; i++)
            {
                sum += nextArray[i];
                totalArray[i + 1] = sum;
            }
        }

        public static IEnumerable<int> TotalList
        {
            get
            {
                return totalArray.ToList();
            }
        }

        /// <summary>
        /// 次のレベルまでに必要な経験値を返します。
        /// </summary>
        /// <param name="level">レベル。1から154である必要があります。</param>
        /// <returns>次のレベルまでに必要な経験値</returns>
        public static int getNextExperience(int level)
        {
            if (level > 0 && level < 155)
            {
                return nextArray[level];
            }
            else
            {
                throw new ArgumentOutOfRangeException("level");
            }
        }

        /// <summary>
        /// そのレベルまでの累積経験値を返します。
        /// </summary>
        /// <param name="level">レベル。1から155である必要があります。</param>
        /// <returns>そのレベルまでの累積経験値</returns>
        public static int getTotalExperience(int level)
        {
            if (level > 0 && level <= 155)
            {
                return totalArray[level];
            }
            else
            {
                throw new ArgumentOutOfRangeException("level");
            }
        }
    }
}
