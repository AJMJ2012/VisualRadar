using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Map;
using Terraria.ModLoader;

namespace VisualRadar
{
	public class TrackedTreasure : TrackedObject {
		public static bool MetalDetectorEnabled => player.accOreFinder && !player.hideInfo[InfoDisplay.MetalDetector.Type];
		public void Get(TrackedContext context)
		{
			active = false;
			scale = 1f;
			if (((context == TrackedContext.Map && Config.Combined.Map.TreasureEnabled) || (context == TrackedContext.Screen && Config.Combined.Screen.TreasureEnabled)) && MetalDetectorEnabled)
			{
				if (Main.SceneMetrics.bestOre > 0)
				{
					int baseOption = 0;
					int num2 = Main.SceneMetrics.bestOre;
					if (Main.SceneMetrics.ClosestOrePosition.HasValue)
					{
						Point point = Main.SceneMetrics.ClosestOrePosition.Value;
						Tile tileSafely = Framing.GetTileSafely(point);
						if (tileSafely.HasTile)
						{
							MapHelper.GetTileBaseOption(point.X, point.Y, tileSafely.TileType, tileSafely, ref baseOption);
							num2 = tileSafely.TileType;
							if (TileID.Sets.BasicChest[num2] || TileID.Sets.BasicChestFake[num2])
							{
								baseOption = 0;
							}
						}
						icon = TextureAssets.InfoIcon[10].Value;
						position = new Vector2(point.X + 0.5f, point.Y + 0.5f) * 16f;
						fixedDistance = 64;
						iconText = Lang.GetMapObjectName(MapHelper.TileToLookup(num2, baseOption));
						active = true;
					}
				}
			}
			lerpPosition = context == TrackedContext.Screen;
		}

		// Reset icon when the next one is too far from the previous one.
		// Typically from teleport on screen
		// Or away from a cluster on map (TODO: Actual cluster check)
		public void Check(TrackedContext context, ref int current)
		{
			if (((context == TrackedContext.Map && Config.Combined.Map.TreasureEnabled) || (context == TrackedContext.Screen && Config.Combined.Screen.TreasureEnabled)) && MetalDetectorEnabled)
			{
				if (Main.SceneMetrics.bestOre > 0)
				{
					if (Main.SceneMetrics.ClosestOrePosition.HasValue)
					{
						Point point = Main.SceneMetrics.ClosestOrePosition.Value;
						Vector2 position = new Vector2(point.X + 0.5f, point.Y + 0.5f) * 16f;

						if (lastPosition != Vector2.Zero && Vector2.Distance(lastPosition, position) > 0)
						{
							Vector2 distance = new Vector2(Math.Abs(lastPosition.X - position.X), Math.Abs(lastPosition.Y - position.Y));
							Vector2 buffArea = new Vector2(Main.buffScanAreaWidth, Main.buffScanAreaHeight) * 16f;
							Vector2 playerSpeed = new Vector2(Math.Abs(Main.LocalPlayer.velocity.X), Math.Abs(Main.LocalPlayer.velocity.Y));
							if (context == TrackedContext.Map ? (Vector2.Distance(position, lastPosition) > 64) : (distance.X > buffArea.X + 1 + Math.Ceiling(playerSpeed.X) || distance.Y > buffArea.Y + 1 + Math.Ceiling(playerSpeed.Y)))
							{
								current--;
								active = false;
								//lerpPosition = context == TrackedContext.Screen;
							}
							else
							{
								//lerpPosition = true;
							}
						}
					}
				}
			}
		}
	}
}