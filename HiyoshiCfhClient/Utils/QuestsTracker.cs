using Codeplex.Data;
using Grabacr07.KanColleWrapper.Models.Raw;
using Nekoxy;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HiyoshiCfhClient.Utils
{
    class QuestsTracker
    {
        private int PageCount
        {
            get
            {
                if (QuestPages == null || QuestPages.Count == 0)
                {
                    return 0;
                }
                return QuestPages.Values[0].api_page_count;
            }
        }

        private int QuestCount
        {
            get
            {
                if (QuestPages == null || QuestPages.Count == 0)
                {
                    return 0;
                }
                return QuestPages.Values[0].api_count;
            }
        }

        public bool IsIntegral
        {
            get
            {
                if (QuestPages != null && Quests != null &&
                    PageCount == QuestPages.Count && QuestCount == Quests.Count)
                {
                    return true;
                }
                return false;
            }
        }

        public IEnumerable<kcsapi_quest> DisplayedQuests
        {
            get
            {
                return Quests.Values;
            }
        }

        private SortedList<int, kcsapi_questlist> QuestPages;
        private SortedList<int, kcsapi_quest> Quests;

        public void AddPage(kcsapi_questlist page)
        {
            var position = page.api_disp_page;
            if (PageCount == page.api_page_count && QuestCount == page.api_count)
            {
                try
                {
                    var old_page = QuestPages[position];
                    foreach (var quest in old_page.api_list)
                    {
                        Quests.Remove(quest.api_no);
                    }
                }
                catch (KeyNotFoundException) { }
            }
            else
            {
                Reset();
            }
            QuestPages[position] = page;
            try
            {
                foreach (var quest in page.api_list)
                {
                    Quests.Add(quest.api_no, quest);
                }
            }
            catch (ArgumentException)
            {
                Reset();
                AddPage(page);
            }
        }

        public QuestsTracker()
        {
            QuestPages = new SortedList<int, kcsapi_questlist>();
            Quests = new SortedList<int, kcsapi_quest>();
        }

        void Reset()
        {
            QuestPages.Clear();
            Quests.Clear();
        }

        public static kcsapi_questlist QuestListSerialize(Session session)
        {
            try
            {
                var djson = DynamicJson.Parse(session.Response.BodyAsString.Replace("svdata=", ""));
                var questlist = new kcsapi_questlist
                {
                    api_count = Convert.ToInt32(djson.api_data.api_count),
                    api_disp_page = Convert.ToInt32(djson.api_data.api_disp_page),
                    api_page_count = Convert.ToInt32(djson.api_data.api_page_count),
                    api_exec_count = Convert.ToInt32(djson.api_data.api_exec_count),
                };

                if (djson.api_data.api_list != null)
                {
                    var list = new List<kcsapi_quest>();
                    var serializer = new DataContractJsonSerializer(typeof(kcsapi_quest));
                    foreach (var x in (object[])djson.api_data.api_list)
                    {
                        try
                        {
                            list.Add(serializer.ReadObject(new MemoryStream(Encoding.UTF8.GetBytes(x.ToString()))) as kcsapi_quest);
                        }
                        catch (SerializationException ex) { }
                    }

                    questlist.api_list = list.ToArray();
                }

                return questlist;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
