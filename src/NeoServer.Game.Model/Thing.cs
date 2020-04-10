﻿using NeoServer.Game.Contracts;
using NeoServer.Game.Enums.Location.Structs;

namespace NeoServer.Game.Model
{
    public abstract class Thing : IThing
    {
        public event OnThingStateChanged OnThingChanged;
        public event OnThingRemoved OnThingRemoved;

        protected ITile tile;
        protected Location location;

        public const ushort CreatureThingId = 0x63;

        public abstract ushort ThingId { get; }

        public abstract byte Count { get; }

        public abstract string InspectionText { get; }

        public abstract string CloseInspectionText { get; }

        public abstract bool CanBeMoved { get; }

        public Location Location
        {
            get
            {
                return location;
            }

            protected set
            {
                var oldValue = location;
                location = value;
                if (oldValue != location)
                {
                    OnThingChanged?.Invoke(this, new ThingStateChangedEventArgs() { PropertyChanged = nameof(Location) });
                }
            }
        }

        public ITile Tile
        {
            get
            {
                return tile;
            }

            set
            {
                if (value != null)
                {
                    Location = value.Location;
                }
                tile = value;
            }
        }

        public byte GetStackPosition()
        {
            return Tile.GetStackPositionOfThing(this);
        }

        public void Added()
        {
            // OnThingAdded?.Invoke();
        }
        public virtual void Moved()
        {
            // OnThingAdded?.Invoke();
        }
        public virtual void Moved(ITile fromTile, ITile toTile)
        {

        }

        public virtual void Removed()
        {
             OnThingRemoved?.Invoke(this);
        }
    }
}