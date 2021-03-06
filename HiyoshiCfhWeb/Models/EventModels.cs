﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace HiyoshiCfhWeb.Models
{
    public class Event
    {
        public static List<Event> Events { get; } = new List<Event> {
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
            new Event(201705,
                "出撃！北東方面 第五艦隊",
                new List<SortieTag>
                {
                    new SortieTag("第五艦隊 先遣隊", Color.FromArgb(0x3C677F), 1),
                    new SortieTag("千島方面 根拠地隊", Color.FromArgb(0xC0B812), 2),
                    new SortieTag("連合部隊", Color.FromArgb(0x3B662A), 3),
                    new SortieTag("逆上陸部隊", Color.FromArgb(0x906736), 4)
                },
                new DateTimeOffset(2017,  5,  2, 23, 30, 0, new TimeSpan(9, 0, 0)),
                new DateTimeOffset(2017,  5, 22, 11,  0, 0, new TimeSpan(9, 0, 0))
            ),
            // 2017年下半期のデータは未追加
            new Event(201802,
                "捷号決戦！邀撃、レイテ沖海戦(後篇)",
                new List<SortieTag>
                {
                    new SortieTag("警戒部隊", Color.Gray, 1),
                    new SortieTag("栗田艦隊", Color.FromArgb(0x3B662A), 2),
                    new SortieTag("西村艦隊", Color.FromArgb(0xC0B812), 3),
                    new SortieTag("小沢艦隊", Color.FromArgb(0x3C677F), 4),
                    new SortieTag("志摩艦隊", Color.Purple, 5)
                },
                new DateTimeOffset(2018,  2, 17,  3,  0, 0, new TimeSpan(9, 0, 0)),
                new DateTimeOffset(2018,  3, 23, 11,  0, 0, new TimeSpan(9, 0, 0))
            ),
            new Event(201809,
                "抜錨！連合艦隊、西へ！",
                new List<SortieTag>
                {
                    new SortieTag("警戒部隊", Color.Gray, 1),
                    new SortieTag("海峡派遣艦隊", Color.FromArgb(0xC0B812), 2),
                    new SortieTag("西方作戦部隊", Color.FromArgb(0x3B662A), 3),
                    new SortieTag("欧州特務艦隊", Color.Orange, 4),
                    new SortieTag("Force H", Color.Blue, 5),
                    new SortieTag("ライン演習部隊", Color.Purple, 6)
                },
                new DateTimeOffset(2018,  9,  9,  3,  0, 0, new TimeSpan(9, 0, 0)),
                new DateTimeOffset(2018, 10,  5, 11,  0, 0, new TimeSpan(9, 0, 0))
            ),
            new Event(201812,
                "邀撃！ブイン防衛作戦",
                new List<SortieTag>
                {
                    new SortieTag("鼠輸送部隊", Color.Gray, 1),
                    new SortieTag("ラバウル艦隊", Color.FromArgb(0x3B662A), 2),
                    new SortieTag("連合艦隊", Color.Blue, 3),
                    new SortieTag("ブイン派遣隊", Color.FromArgb(0xC0B812), 4)
                },
                new DateTimeOffset(2018, 12, 27,  3, 30, 0, new TimeSpan(9, 0, 0)),
                new DateTimeOffset(2019,  1, 22, 12,  0, 0, new TimeSpan(9, 0, 0))
            ),
            new Event(201905,
                "発動！友軍救援「第二次ハワイ作戦」",
                new List<SortieTag>
                {
                    new SortieTag("第百四戦隊", Color.Gray, 1),
                    new SortieTag("第二艦隊", Color.DarkOrange, 2),
                    new SortieTag("北方部隊", Color.Blue, 3),
                    new SortieTag("機動部隊", Color.FromArgb(0x3B662A), 4),
                    new SortieTag("攻略部隊", Color.FromArgb(0xC0B812), 5),
                    new SortieTag("ハワイ派遣艦隊", Color.YellowGreen, 6),
                },
                new DateTimeOffset(2019,  5, 21,  8, 50, 0, new TimeSpan(9, 0, 0)),
                new DateTimeOffset(2019,  6, 25, 12,  0, 0, new TimeSpan(9, 0, 0))
            ),
            new Event(201908,
                "欧州方面反撃作戦 発動！「シングル作戦」",
                new List<SortieTag>
                {
                    new SortieTag("欧州防衛艦隊", Color.Blue, 1),
                    new SortieTag("連合艦隊", Color.FromArgb(0x3B662A), 2),
                    new SortieTag("地中海艦隊", Color.DarkOrange, 3),
                },
                new DateTimeOffset(2019,  8, 30, 20,  0, 0, new TimeSpan(9, 0, 0)),
                new DateTimeOffset(2019,  9, 30, 12,  0, 0, new TimeSpan(9, 0, 0))
            ),
            new Event(201911,
                "進撃！第二次作戦「南方作戦」",
                new List<SortieTag>
                {
                    new SortieTag("攻略護衛隊", Color.Pink, 1),
                    new SortieTag("空襲部隊", Color.Cyan, 2),
                    new SortieTag("蘭印部隊", Color.Blue, 3),
                    new SortieTag("馬来部隊", Color.FromArgb(0x3B662A), 4),
                    new SortieTag("哨戒部隊", Color.Gray, 5),
                    new SortieTag("決戦部隊", Color.DarkOrange, 6),
                },
                new DateTimeOffset(2019, 11, 30, 10, 30, 0, new TimeSpan(9, 0, 0)),
                new DateTimeOffset(2019, 12, 30, 12,  0, 0, new TimeSpan(9, 0, 0))
            ),
            new Event(202003,
                "桃の節句！沖に立つ波",
                new List<SortieTag> {},
                new DateTimeOffset(2020,  3,  3, 19,  0, 0, new TimeSpan(9, 0, 0)),
                new DateTimeOffset(2019,  3, 27, 11,  0, 0, new TimeSpan(9, 0, 0))
            ),
        };

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
