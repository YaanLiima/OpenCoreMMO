﻿using NeoServer.Game.Common.Location.Structs;
using System;
using System.Collections.Generic;
using System.Text;

namespace NeoServer.Game.Contracts.Combat
{
    public interface IAreaAttack
    {
         Coordinate[] AffectedArea { get; }
    }
}
