using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;

namespace VisualRadar;

public class BetterOreFinder : ModSystem
{
	public MethodInfo UpdateOreFinderData_Method => typeof(SceneMetrics).GetMethod("UpdateOreFinderData", BindingFlags.NonPublic | BindingFlags.Instance);
	public delegate void UpdateOreFinderData_Delegate(SceneMetrics instance);
	public static FieldInfo _oreFinderTileLocations_Field = typeof(SceneMetrics).GetField("_oreFinderTileLocations", BindingFlags.NonPublic | BindingFlags.Instance);
	public override void Load()
	{
		if (Main.netMode == 2) return;
		try { MonoModHooks.Add(UpdateOreFinderData_Method, Override_UpdateOreFinderData); } catch { throw new Exception("Could not hook into SceneMetrics.UpdateOreFinderData"); }
	}
	public void Override_UpdateOreFinderData(UpdateOreFinderData_Delegate UpdateOreFinderData, SceneMetrics instance)
	{
		int num = -1;
		float num2 = -1;
		List<Point> _oreFinderTileLocations = (List<Point>)_oreFinderTileLocations_Field.GetValue(instance);
		foreach (Point oreFinderTileLocation in _oreFinderTileLocations)
		{
			Tile tile = Main.tile[oreFinderTileLocation.X, oreFinderTileLocation.Y];
			if (SceneMetrics.IsValidForOreFinder(tile) && (num < 0 || Main.tileOreFinderPriority[tile.TileType] >= Main.tileOreFinderPriority[num]))
			{
				num = tile.TileType;
				instance.ClosestOrePosition = oreFinderTileLocation;
			}
		}
		foreach (Point oreFinderTileLocation in _oreFinderTileLocations)
		{
			Tile tile = Main.tile[oreFinderTileLocation.X, oreFinderTileLocation.Y];
			float distance = Vector2.Distance((Main.LocalPlayer.Center / 16f), new Vector2(oreFinderTileLocation.X, oreFinderTileLocation.Y));
			if (SceneMetrics.IsValidForOreFinder(tile) && tile.TileType == num && (num2 < 0 || distance < num2))
			{
				num2 = distance;
				instance.ClosestOrePosition = oreFinderTileLocation;
			}
		}
		instance.bestOre = num;
	}
}
