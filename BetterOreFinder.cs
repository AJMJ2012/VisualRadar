using Microsoft.Xna.Framework;
using MonoMod.RuntimeDetour.HookGen;
using System.Collections.Generic;
using System.Reflection;
using System;
using Terraria.ModLoader;
using Terraria;

namespace VisualRadar {
    public class BetterOreFinder : ModSystem {
		public MethodInfo UpdateOreFinderDataMethod => typeof(Terraria.SceneMetrics).GetMethod("UpdateOreFinderData", BindingFlags.NonPublic | BindingFlags.Instance);
		public override void Load() {
			if (Main.netMode == 2) return;
			HookEndpointManager.Add(UpdateOreFinderDataMethod, Override_UpdateOreFinderData);
		}
		public override void Unload() {
			if (Main.netMode == 2) return;
			HookEndpointManager.Remove(UpdateOreFinderDataMethod, Override_UpdateOreFinderData);
		}
		public void Override_UpdateOreFinderData(Action<SceneMetrics> UpdateOreFinderData, SceneMetrics instance) {
			try {
				int num = -1;
				float num2 = -1;
				foreach (Point oreFinderTileLocation in (List<Point>)instance.GetType().GetField("_oreFinderTileLocations", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(instance)) {
					Tile tile = Main.tile[oreFinderTileLocation.X, oreFinderTileLocation.Y];
					if (SceneMetrics.IsValidForOreFinder(tile) && (num < 0 || Main.tileOreFinderPriority[tile.TileType] >= Main.tileOreFinderPriority[num])) {
						num = tile.TileType;
						instance.GetType().GetProperty("ClosestOrePosition", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).SetValue(instance, oreFinderTileLocation);
					}
				}
				foreach (Point oreFinderTileLocation in (List<Point>)instance.GetType().GetField("_oreFinderTileLocations", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(instance)) {
					Tile tile = Main.tile[oreFinderTileLocation.X, oreFinderTileLocation.Y];
					float distance = Vector2.Distance((Main.LocalPlayer.Center / 16f), new Vector2(oreFinderTileLocation.X, oreFinderTileLocation.Y));
					if (SceneMetrics.IsValidForOreFinder(tile) && tile.TileType == num && (num2 < 0 || distance < num2)) {
						num2 = distance;
						instance.GetType().GetProperty("ClosestOrePosition", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).SetValue(instance, oreFinderTileLocation);
					}
				}
				instance.bestOre = num;
			}
			catch {}
		}
	}
}
