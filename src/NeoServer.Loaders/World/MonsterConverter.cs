﻿using NeoServer.Game.Contracts.Combat;
using NeoServer.Game.Contracts.Creatures;
using NeoServer.Game.Creatures.Combat.Attacks;
using NeoServer.Game.Creatures.Model.Monsters;
using NeoServer.Game.Enums.Creatures;
using NeoServer.Game.Enums.Item;
using System;
using System.Collections.Generic;
using NeoServer.Server.Helpers.Extensions;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace NeoServer.Loaders.World
{
    public class MonsterConverter
    {
        public static IMonsterType Convert(MonsterData monsterData)
        {
            var data = monsterData.Monster;
            var monster = new MonsterType()
            {
                Name = data.Name,
                MaxHealth = data.Health.Max,
                Look = new Dictionary<LookType, ushort>() { { LookType.Type, data.Look.Type }, { LookType.Corpse, data.Look.Corpse } },
                Speed = data.Speed,
                Armor = ushort.Parse(data.Defenses.Armor),
                Defence = ushort.Parse(data.Defenses.Defense),
                Attacks = new List<ICombatAttack>(),
                Experience = data.Experience
            };


            foreach (var attack in data.Attacks)
            {
                attack.TryGetValue("name", out string attackName);
                attack.TryGetValue("attack", out byte attackValue);
                attack.TryGetValue("skill", out byte skill);

                attack.TryGetValue("min", out decimal min);
                attack.TryGetValue("max", out decimal max);
                attack.TryGetValue("chance", out byte chance);
                attack.TryGetValue<JArray>("attributes", out var attributes);


                if (attack.ContainsKey("range"))
                {
                    attack.TryGetValue("range", out byte range);
                    attack.TryGetValue("radius", out byte radius);

                    var shootEffect = attributes?.FirstOrDefault(a => a.Value<string>("key") == "shootEffect")?.Value<string>("value");

                   
                    if (attackName == "manadrain")
                    {

                        monster.Attacks.Add(new ManaDrainCombatAttack(new CombatAttackOption
                        {
                            Chance = chance,
                            Range = range,
                            MinDamage = (ushort)Math.Abs(min),
                            MaxDamage = (ushort)Math.Abs(max)
                        }));
                    }
                    else if (attackName == "firefield")
                    {
                        monster.Attacks.Add(new FieldCombatAttack(new CombatAttackOption
                        {
                            Chance = chance,
                            Radius = radius,
                            Range = range,
                            MinDamage = (ushort)Math.Abs(min),
                            MaxDamage = (ushort)Math.Abs(max),
                            ShootType = ParseShootType(shootEffect)
                        }));
                    }
                    else if (attack.ContainsKey("radius"))
                    {

                        monster.Attacks.Add(new DistanceAreaCombatAttack(ParseDamageType(attackName), new CombatAttackOption
                        {
                            Chance = chance,
                            Range = range,
                            MinDamage = (ushort)Math.Abs(min),
                            MaxDamage = (ushort)Math.Abs(max),
                            Radius = radius,
                            ShootType = ParseShootType(shootEffect)
                        }));
                    }
                    else
                    {
                        monster.Attacks.Add(new DistanceCombatAttack(ParseDamageType(attackName), new CombatAttackOption
                        {
                            Chance = chance,
                            Range = range,
                            MinDamage = (ushort)Math.Abs(min),
                            MaxDamage = (ushort)Math.Abs(max),
                            ShootType = ParseShootType(shootEffect)
                        }));
                    }



                }
                else if (attack.ContainsKey("spread"))
                {
                    attack.TryGetValue("length", out byte length);
                    attack.TryGetValue("spread", out byte spread);


                    monster.Attacks.Add(new SpreadCombatAttack(ParseDamageType(attackName), new CombatAttackOption
                    {
                        Chance = chance,
                        MinDamage = (ushort)Math.Abs(min),
                        MaxDamage = (ushort)Math.Abs(max),
                        Length = length,
                        Spread = spread
                    }));

                }
                else if (ParseDamageType(attackName?.ToString()) == DamageType.Melee)
                {

                    monster.Attacks.Add(new MeleeCombatAttack(attackValue, skill));

                }
            }

            foreach (var flag in data.Flags)
            {
                var creatureFlag = ParseCreatureFlag(flag.Key);

                monster.Flags.Add(creatureFlag, flag.Value);
            }

            return monster;
        }

        private static DamageType ParseDamageType(string type)
        {
            return type switch
            {
                "melee" => DamageType.Melee,
                "physical" => DamageType.Physical,
                "energy" => DamageType.Energy,
                "fire" => DamageType.Fire,
                "manadrain" => DamageType.ManaDrain,
                _ => DamageType.Melee
            };
        }
        private static ShootType ParseShootType(string type)
        {
            return type switch
            {
                "bolt" => ShootType.Bolt,
                "spear" => ShootType.Spear,
                "star" => ShootType.ThrowingStar,
                "energy" => ShootType.Energy,
                "fire" => ShootType.Fire,
                _ => ShootType.None
            };
        }
        private static CreatureFlagAttribute ParseCreatureFlag(string flag)
        {
            return flag switch
            {
                "summonable" => CreatureFlagAttribute.Summonable,
                "attackable" => CreatureFlagAttribute.Attackable,
                "hostile" => CreatureFlagAttribute.Hostile,
                "illusionable" => CreatureFlagAttribute.Illusionable,
                "convinceable" => CreatureFlagAttribute.Convinceable,
                "pushable" => CreatureFlagAttribute.Pushable,
                "canpushitems" => CreatureFlagAttribute.CanPushItems,
                "canpushcreatures" => CreatureFlagAttribute.CanPushCreatures,
                "targetdistance" => CreatureFlagAttribute.TargetDistance,
                "staticattack" => CreatureFlagAttribute.StaticAttack,
                "runonhealth" => CreatureFlagAttribute.RunOnHealth,
                _ => CreatureFlagAttribute.None
            };
        }

    }
}
