﻿using NeoServer.Game.Contracts.Creatures;
using NeoServer.Server.Contracts.Network;
using NeoServer.Server.Model.Players.Contracts;
using NeoServer.Server.Tasks;

namespace NeoServer.Server.Handlers.Player
{
    public class PlayerAttackHandler : PacketHandler
    {
        private readonly Game game;
        public PlayerAttackHandler(Game game)
        {
            this.game = game;
        }
        public override void HandlerMessage(IReadOnlyNetworkMessage message, IConnection connection)
        {
            var targetId = message.GetUInt32();
            if (!game.CreatureManager.TryGetPlayer(connection.PlayerId, out IPlayer player)) return;

            game.Dispatcher.AddEvent(new Event(() => 
            player.SetAttackTarget(targetId)));
        }
    }
}
