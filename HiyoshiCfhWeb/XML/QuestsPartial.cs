using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HiyoshiCfhWeb.XML
{
    public partial class Quest
    {
        public QuestState State { get; set; }

        public Quest()
        {
            State = QuestState.Achieved;
        }

        public bool Compare(HiyoshiCfhWeb.Models.Quest quest)
        {
            if (Bonus.Fuel == quest.Fuel && Bonus.Bull == quest.Bull &&
                Bonus.Steel == quest.Steel && Bonus.Bauxite == quest.Bauxite)
            {
                switch (Category)
                {
                    case Category.composition:
                        if (quest.Category != Models.QuestCategory.Composition)
                            return false;
                        break;
                    case Category.sortie:
                        if (quest.Category != Models.QuestCategory.Sortie && quest.Category != Models.QuestCategory.Sortie2)
                            return false;
                        break;
                    case Category.practice:
                        if (quest.Category != Models.QuestCategory.Practice)
                            return false;
                        break;
                    case Category.expeditions:
                        if (quest.Category != Models.QuestCategory.Expeditions)
                            return false;
                        break;
                    case Category.supply:
                        if (quest.Category != Models.QuestCategory.Supply)
                            return false;
                        break;
                    case Category.building:
                        if (quest.Category != Models.QuestCategory.Building)
                            return false;
                        break;
                    case Category.remodelling:
                        if (quest.Category != Models.QuestCategory.Remodelling)
                            return false;
                        break;
                    default:
                        return false;
                }
                switch (Type)
                {
                    case Type.onetime:
                        if (quest.Type != Models.QuestType.OneTime)
                            return false;
                        break;
                    case Type.daily:
                        if (quest.Type != Models.QuestType.Daily)
                            return false;
                        break;
                    case Type.weekly:
                        if (quest.Type != Models.QuestType.Weekly)
                            return false;
                        break;
                    case Type.monthly:
                        if (quest.Type != Models.QuestType.Monthly)
                            return false;
                        break;
                    default:
                        return false;
                }
                if (Name.Trim() != quest.Name.Trim())
                    return false;
                if (Content.Trim() != quest.Content.Trim())
                    return false;
                return true;
            }
            return false;
        }
    }

    public enum QuestState
    {
        Invisible,
        Visible,
        Achieved,
    }
}
