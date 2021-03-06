﻿using NeoServer.Game.Contracts.Spells;
using NeoServer.Game.Creatures.Spells;
using NeoServer.Server.Compiler;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NeoServer.Loaders.Spells
{
    public class SpellLoader
    {
        public void Load()
        {
            LoadSpells();
        }
        private void LoadSpells()
        {
            var basePath = "./data/spells";
            var jsonString = File.ReadAllText(Path.Combine(basePath, "spells.json"));
            var spells = JsonConvert.DeserializeObject<List<IDictionary<string, string>>>(jsonString);

            foreach (var spell in spells)
            {
                var type = ScriptList.Assemblies.FirstOrDefault(x => x.Key == spell["script"]).Value;
                var spellInstance = Activator.CreateInstance(type, true) as ISpell;

                spellInstance.Name = spell["name"];
                spellInstance.Cooldown = Convert.ToUInt32(spell["cooldown"]);
                spellInstance.Mana = Convert.ToUInt16(spell["mana"]);
                spellInstance.MinLevel = Convert.ToUInt16(spell["level"]);

                SpellList.Spells.Add(spell["words"], spellInstance);
            }
        }
    }
}
