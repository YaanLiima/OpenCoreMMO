﻿using System;
using System.Collections.Generic;
using System.Text;

namespace NeoServer.Game.Contracts.Items
{
    public interface IItemModel
    {
        ushort ServerId { get; set; }
        byte Amount { get; set; }
        IEnumerable<IItemModel> Items { get; set; }
    }
}
