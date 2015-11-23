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
            new Event(
                "突入！海上輸送作戦",
                new List<SortieTag> {
                    new SortieTag("輸送作戦", Color.FromArgb(0x3B662A), 1),
                    new SortieTag("派遣作戦", Color.FromArgb(0x906736), 2)
                },
                new DateTimeOffset(2015, 11, 18, 21, 17, 0, new TimeSpan(9, 0, 0)),
                new DateTimeOffset(2015, 12,  7, 11,  0, 0, new TimeSpan(9, 0, 0))
            )
        };

        public static List<Event> Events
        {
            get
            {
                return events;
            }
        }

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

        private Event(string name, List<SortieTag> tags, DateTimeOffset start, DateTimeOffset finish)
        {
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

        public SortieTag(string name, Color color, int id)
        {
            Name = name;
            BaseColor = color;
            InternalId = id;
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
