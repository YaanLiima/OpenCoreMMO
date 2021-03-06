﻿using NeoServer.Game.Contracts.Creatures;
using NeoServer.Game.Common.Creatures;
using NeoServer.Game.Common.Creatures.Players;
using NeoServer.Networking.Packets.Outgoing;
using NeoServer.Server.Model.Players.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace NeoServer.Server.Events.Player
{
    public class PlayerConditionChangedEventHandler
    {
        private readonly Game game;

        public PlayerConditionChangedEventHandler(Game game)
        {
            this.game = game;
        }

        public void Execute(ICreature creature, ICondition c)
        {
            ushort icons = 0;
            foreach (var condition in creature.Conditions)
            {
                icons |= (ushort)ToIcon(condition.Key);
            }

            if (game.CreatureManager.GetPlayerConnection(creature.CreatureId, out var connection))
            {
                connection.OutgoingPackets.Enqueue(new ConditionIconPacket(icons));
                connection.Send();
            }
        }

        public static ConditionIcon ToIcon(ConditionType type)
        {
            return type switch
            {
                ConditionType.Haste => ConditionIcon.Haste,
                ConditionType.Poison => ConditionIcon.Poison,
                ConditionType.InFight => ConditionIcon.Swords,
                ConditionType.Paralyze => ConditionIcon.Paralyze,
                ConditionType.Fire => ConditionIcon.Burn,
                ConditionType.Energy => ConditionIcon.Energy,
                ConditionType.Drunk => ConditionIcon.Drunk,
                ConditionType.Cursed => ConditionIcon.Cursed,
                ConditionType.Freezing => ConditionIcon.Freezing,
                ConditionType.Manashield => ConditionIcon.Manashield,
                ConditionType.Drown => ConditionIcon.Drowning,
                ConditionType.Pacified => ConditionIcon.Pigeon,
                _ => ConditionIcon.None
            };
        }
    }

}
