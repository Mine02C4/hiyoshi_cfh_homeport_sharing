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
        public static kcsapi_questlist QuestListSerialize(Session session)
        {
            try
            {
                var query = System.Web.HttpUtility.ParseQueryString(session.Request.BodyAsString);
                var hoge = session.Request;
                var djson = DynamicJson.Parse(session.Response.BodyAsString.Replace("svdata=", ""));
                var questlist = new kcsapi_questlist
                {
                    api_count = Convert.ToInt32(djson.api_data.api_count),
                    api_completed_kind = Convert.ToInt32(djson.api_data.api_completed_kind),
                    api_exec_count = Convert.ToInt32(djson.api_data.api_exec_count),
                    api_exec_type = Convert.ToInt32(djson.api_data.api_exec_type),
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

                if (query["api_tab_id"] != "0")
                {
                    questlist.api_count = -1;
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
