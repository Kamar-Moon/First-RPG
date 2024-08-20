using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Models;

namespace Engine.Factories
{
    internal static class QuestFactory
    {
        private static readonly List<Quest> _quests = new List<Quest>();

        static QuestFactory()
        {
            // declare the items that are needed to complete the quest, and its rewards for completion.
            List<ItemQuantity> itemsToComplete = new List<ItemQuantity>();
            List<ItemQuantity> rewardItems = new List<ItemQuantity>();

            itemsToComplete.Add(new ItemQuantity(9001, 5)); // You need 5 snake fags to complete this quest

            rewardItems.Add(new ItemQuantity(1002, 1)); // the reward for completion is a rusty sword

            // Create the quest
            _quests.Add(new Quest(1, "Clear the herb garden", 
                "Defeat the snakes in the Herbalist's garden", 
                itemsToComplete, 25, 10, rewardItems)); // 25 exp and 10 gold and 1 rusty sword for rewards
        }

        internal static Quest GetQuestID(int id)
        {
            return _quests.FirstOrDefault(quest => quest.ID == id);
        }
    }
}
