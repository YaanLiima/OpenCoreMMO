﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace NeoServer.Server.Helpers
{
	public class ServerResourcesManager
	{
		public const string ItemsFilesDirectory = "NeoServer.Game.Items.Data";
		public const string MapFilesDirectory = "NeoServer.Server.World.Data";
		public const string MapName = "neoserver.otbm";

		public static byte[] GetMap()
		{
			var assembly = Assembly.GetCallingAssembly();
			using (var stream = assembly.GetManifestResourceStream(MapFilesDirectory + "." + MapName))
				return ReadFully(stream);
		}

		public static Stream GetItems(string itemsFileName)
		{
			var assembly = Assembly.GetCallingAssembly();
			return assembly.GetManifestResourceStream(ItemsFilesDirectory + "." + itemsFileName);
		}

		public static byte[] GetItemsBytes(string itemsFileName)
		{
			var assembly = Assembly.GetCallingAssembly();
			using (var stream = assembly.GetManifestResourceStream(ItemsFilesDirectory + "." + itemsFileName))
				return ReadFully(stream);
		}

		private static byte[] ReadFully(Stream input)
		{
			using (var ms = new MemoryStream())
			{
				input.CopyTo(ms);
				return ms.ToArray();
			}
		}
	}
}
