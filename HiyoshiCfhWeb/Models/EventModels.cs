using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace HiyoshiCfhWeb.Models
{
    public class Event
    {
        private static List<Event> events = new List<Event> {
            new Event(201508,
                "反撃！第二次SN作戦",
                new List<SortieTag> {
                    new SortieTag("初動作戦", Color.FromArgb(0x3C677F), 1),
                    new SortieTag("機動部隊", Color.FromArgb(0x3B662A), 2),
                    new SortieTag("挺身部隊", Color.FromArgb(0x839842), 3),
                    new SortieTag("西部方面", Color.FromArgb(0x906736), 4)
                },
                new DateTimeOffset(2015, 8, 10, 22, 43, 0, new TimeSpan(9, 0, 0)),
                new DateTimeOffset(2015, 9,  7, 11,  0, 0, new TimeSpan(9, 0, 0))
            ),
            new Event(201511,
                "突入！海上輸送作戦",
                new List<SortieTag> {
                    new SortieTag("輸送作戦", Color.FromArgb(0x3B662A), 1),
                    new SortieTag("派遣作戦", Color.FromArgb(0x906736), 2)
                },
                new DateTimeOffset(2015, 11, 18, 21, 17, 0, new TimeSpan(9, 0, 0)),
                new DateTimeOffset(2015, 12,  8, 11,  0, 0, new TimeSpan(9, 0, 0))
            ),
            new Event(201602,
                "出撃！礼号作戦",
                new List<SortieTag> {},
                new DateTimeOffset(2016,  2, 10, 18,  0, 0, new TimeSpan(9, 0, 0)),
                new DateTimeOffset(2016,  2, 29, 11,  0, 0, new TimeSpan(9, 0, 0))
            ),
            new Event(201605,
                "開設！基地航空隊",
                new List<SortieTag> {
                    new SortieTag("連合艦隊", Color.FromArgb(0x3C677F), 1),
                    new SortieTag("設営部隊", Color.FromArgb(0x3B662A), 2),
                    new SortieTag("南方部隊", Color.FromArgb(0x906736), 3),
                    new SortieTag("機動部隊", Color.FromArgb(0xC0B812), 4)
                },
                new DateTimeOffset(2016, 5,  3,  3, 50, 0, new TimeSpan(9, 0, 0)),
                new DateTimeOffset(2016, 6,  1, 11, 30, 0, new TimeSpan(9, 0, 0))
            ),
            new Event(201608,
                "迎撃！第二次マレー沖海戦",
                new List<SortieTag>
                {
                    new SortieTag("哨戒部隊", Color.FromArgb(0x3C677F), 1),
                    new SortieTag("輸送部隊", Color.FromArgb(0x3B662A), 2),
                    new SortieTag("連合艦隊", Color.FromArgb(0xC0B812), 3)
                },
                new DateTimeOffset(2016, 8, 12, 22,  0, 0, new TimeSpan(9, 0, 0)),
                new DateTimeOffset(2016, 8, 31, 11, 15, 0, new TimeSpan(9, 0, 0))
            ),
            new Event(201611,
                "発令！艦隊作戦第三法",
                new List<SortieTag>
                {
                    new SortieTag("輸送部隊", Color.FromArgb(0x3C677F), 1),
                    new SortieTag("迎撃部隊", Color.FromArgb(0x3B662A), 2),
                    new SortieTag("増派部隊", Color.FromArgb(0xC0B812), 3),
                    new SortieTag("決戦部隊", Color.FromArgb(0x906736), 4)
                },
                new DateTimeOffset(2016, 11, 18, 23,  0, 0, new TimeSpan(9, 0, 0)),
                new DateTimeOffset(2016, 12,  9, 11,  0, 0, new TimeSpan(9, 0, 0))
            ),
            new Event(201702,
                "偵察戦力緊急展開！「光」作戦",
                new List<SortieTag>
                {
                    new SortieTag("作戦部隊", Color.FromArgb(0x3C677F), 1),
                    new SortieTag("海上護衛部隊", Color.FromArgb(0x3B662A), 2)
                },
                new DateTimeOffset(2017,  2, 11, 21, 30, 0, new TimeSpan(9, 0, 0)),
                new DateTimeOffset(2017,  2, 28, 11,  0, 0, new TimeSpan(9, 0, 0))
            ),
        };

        public static List<Event> Events
        {
            get
            {
                return events;
            }
        }

        /// <summary>
        /// イベントID 通常は開催年と開催月をつなげた整数。(例: 201605)
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// イベント名
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 出撃識別札情報
        /// </summary>
        public List<SortieTag> SortieTags { get; private set; }

        /// <summary>
        /// 開始日時
        /// </summary>
        public DateTimeOffset StartTime { get; private set; }
        /// <summary>
        /// 終了日時
        /// </summary>
        public DateTimeOffset FinishTime { get; private set; }
        /// <summary>
        /// 開催中かどうか
        /// </summary>
        public bool IsInDeployment
        {
            get
            {
                return DateTimeOffset.Now > StartTime && DateTimeOffset.Now < FinishTime;
            }
        }

        private Event(int id, string name, List<SortieTag> tags, DateTimeOffset start, DateTimeOffset finish)
        {
            Id = id;
            Name = name;
            SortieTags = tags;
            StartTime = start;
            FinishTime = finish;
        }

        public SortieTag GetSortieTag(int id)
        {
            return SortieTags.Where(x => x.InternalId == id).FirstOrDefault();
        }
    }

    public class SortieTag
    {
        public string Name { get; private set; }
        public Color BaseColor { get; private set; }
        public int InternalId { get; private set; }

        public SortieTag(string name, Color color, int internalId)
        {
            Name = name;
            BaseColor = color;
            InternalId = internalId;
        }

        public string WebColor
        {
            get
            {
                return String.Format("#{0:X2}{1:X2}{2:X2}", BaseColor.R, BaseColor.G, BaseColor.B);
            }
        }
    }
}
