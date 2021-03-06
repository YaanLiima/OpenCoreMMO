﻿using NeoServer.Game.Contracts.Creatures;
using NeoServer.Networking.Packets.Incoming;
using NeoServer.Server.Contracts.Network;
using NeoServer.Server.Contracts.Repositories;
using NeoServer.Server.Model.Players.Contracts;
using NeoServer.Server.Tasks;

namespace NeoServer.Server.Handlers.Authentication
{
    public class PlayerChangesModeHandler : PacketHandler
    {
        private readonly Game game;

        public PlayerChangesModeHandler(IAccountRepository repository, Game game)
        {
            this.game = game;
        }

        public override void HandlerMessage(IReadOnlyNetworkMessage message, IConnection connection)
        {
            var changeMode = new ChangeModePacket(message);

            if (!game.CreatureManager.TryGetPlayer(connection.PlayerId, out var player)) return;

            game.Dispatcher.AddEvent(new Event(() =>
            {
                player.SetFightMode(changeMode.FightMode);
                player.SetChaseMode(changeMode.ChaseMode);
                player.SetSecureMode(changeMode.SecureMode);
            }));
        }
    }
}
