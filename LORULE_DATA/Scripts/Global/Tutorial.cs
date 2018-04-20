﻿using Darkages.Network.Game;
using Darkages.Scripting;
using Darkages.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Darkages.Storage.locales.Scripts.Global
{
    [Script("Tutorial", "Dean")]
    public class Tutorial : GlobalScript
    {
        private readonly GameClient client;

        public Dictionary<int, Dictionary<string, bool>> 
            Flags = new Dictionary<int, Dictionary<string, bool>>();

        public Tutorial(GameClient client) : base(client)
        {
            this.client = client;
        }

        public override void OnDeath(GameClient client, TimeSpan elapsedTime)
        {

        }

        public override void Update(TimeSpan elapsedTime)
        {

            if (!Flags.ContainsKey(client.Aisling.Serial))
            {
                Flags[client.Aisling.Serial] = new Dictionary<string, bool>();
                Flags[client.Aisling.Serial]["t1"] = false;
                Flags[client.Aisling.Serial]["t2"] = false;
                Flags[client.Aisling.Serial]["t3"] = false;
                Flags[client.Aisling.Serial]["t4"] = false;
                Flags[client.Aisling.Serial]["t5"] = false;
                Flags[client.Aisling.Serial]["t6"] = false;
                Flags[client.Aisling.Serial]["t7"] = false;
            }

            if (client != null && client.Aisling != null && client.Aisling.LoggedIn)
            {
                if (client.Aisling.CurrentMapId == 84 && !Flags[client.Aisling.Serial]["t1"])
                {
                    if (client.Aisling.WithinRangeOf(12, 22))
                    {
                        client.SendMessage(0x02, "Where the fuck am i.... I should head north.");
                        Flags[client.Aisling.Serial]["t1"] = true;
                    }
                }
                else if (client.Aisling.CurrentMapId == 85 && !Flags[client.Aisling.Serial]["t2"])
                {
                    if (client.Aisling.WithinRangeOf(34, 24))
                    {
                        client.SendMessage(0x02, "Where am i??... This looks like a safe spot to rest.");
                        Flags[client.Aisling.Serial]["t2"] = true;
                    }
                }
                else if (client.Aisling.CurrentMapId == 101 && !Flags[client.Aisling.Serial]["t4"])
                {
                    if (client.Aisling.WithinRangeOf(36, 21))
                    {
                        client.SendMessage(0x02, "You wonder why these people are here... what is this dragon thing?!");
                        Flags[client.Aisling.Serial]["t4"] = true;
                    }
                }               
                else if (client.Aisling.CurrentMapId == 83)
                {
                    var quest = client.Aisling.Quests.FirstOrDefault(i => i.Name == "macronator_quest");

                    if (client.Aisling.X == 8)
                    {
                        if (quest == null || (quest != null && !quest.Completed))
                        {
                            client.Aisling.X = 11;
                            client.SendMessage(0x02, "You are not ready to enter yet.");
                            client.Refresh();
                        }
                    }
                }
                else if (client.Aisling.CurrentMapId == ServerContext.Config.StartingMap)
                {
                    var quest = client.Aisling.Quests.FirstOrDefault(i => i.Name == "awakening");

                    if (quest == null)
                        quest = CreateQuest(quest);


                    quest.HandleQuest(client);

                    if (!quest.Completed && client.Aisling.Y >= 11)
                    {
                        quest.Started = true;
                        client.Aisling.Y = 10;
                        client.SendMessage(0x02, "You hear walkers outside. you better find some equipment first.");
                        client.Refresh();
                    }
                    else
                    {
                        if (client.Aisling.Position.DistanceFrom(2, 2) <= 1 && quest.Started)
                        {
                            if (!quest.Completed)
                            {
                                quest.OnCompleted(client.Aisling, true);

                                client.Aisling.Recover();

                                client.SendMessage(0x02, "You pick up your gear from the chest, and begin putting it on.");
                            }
                        }
                    }
                }
            }
        }

        private Quest CreateQuest(Quest quest)
        {
            quest = new Quest { Name = "awakening" };
            quest.LegendRewards.Add(new Legend.LegendItem
            {
                Category = "Event",
                Color = (byte)LegendColor.White,
                Icon = (byte)LegendIcon.Community,
                Value = "A Spiritual Awakening"
            });
            quest.GoldReward = 1000;
            quest.ItemRewards.Add(ServerContext.GlobalItemTemplateCache[client.Aisling.Gender == Gender.Male ? "Shirt" : "Blouse"]);
            quest.ItemRewards.Add(ServerContext.GlobalItemTemplateCache["Ancuisa Ceir"]);
            quest.ItemRewards.Add(ServerContext.GlobalItemTemplateCache["Stick"]);
            quest.ItemRewards.Add(ServerContext.GlobalItemTemplateCache["Small Emerald Ring"]);
            quest.ItemRewards.Add(ServerContext.GlobalItemTemplateCache["Small Spinal Ring"]);
            quest.ItemRewards.Add(ServerContext.GlobalItemTemplateCache["Wooden Shield"]);
            quest.ItemRewards.Add(ServerContext.GlobalItemTemplateCache["Silver Earrings"]);
            quest.ItemRewards.Add(ServerContext.GlobalItemTemplateCache["Leather Greaves"]);
            quest.ItemRewards.Add(ServerContext.GlobalItemTemplateCache["Boots"]);

            client.Aisling.Quests.Add(quest);
            quest.QuestStages = new List<QuestStep<Template>>();

            var q2 = new QuestStep<Template> { Type = QuestType.ItemHandIn };

            q2.Prerequisites.Add(new QuestRequirement
            {
                Type = QuestType.HasItem,
                Amount = 1,
                TemplateContext = ServerContext.GlobalItemTemplateCache["Stick"]
            });

            quest.QuestStages.Add(q2);

            return quest;
        }
    }
}