using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.Map;
using Terraria.ModLoader;

namespace VisualRadar {
    public class TrackedTreasure : TrackedObject {
		public static bool MetalDetectorEnabled => player.accOreFinder && !player.hideInfo[InfoDisplay.MetalDetector.Type];
		public void Get(string context) {
			scale = 0f;
			if (((context == "map" && Config.Combined.Map.TreasureEnabled) || (context == "screen" && Config.Combined.Screen.TreasureEnabled)) && MetalDetectorEnabled) {
				if (Main.SceneMetrics.bestOre > 0) {
					int baseOption = 0;
					int num2 = Main.SceneMetrics.bestOre;
					if (Main.SceneMetrics.ClosestOrePosition.HasValue) {
						Point point = Main.SceneMetrics.ClosestOrePosition.Value;
						Tile tileSafely = Framing.GetTileSafely(point);
						if (tileSafely.HasTile) {
							MapHelper.GetTileBaseOption(point.Y, tileSafely, ref baseOption);
							num2 = tileSafely.TileType;
							if (TileID.Sets.BasicChest[num2] || TileID.Sets.BasicChestFake[num2]) {
								baseOption = 0;
							}
						}
                        icon = TextureAssets.InfoIcon[10].Value;
						position = new Vector2(point.X + 0.5f, point.Y + 0.5f) * 16f;
						fixedDistance = 16;
						scale = 1f;
						iconText = Lang.GetMapObjectName(MapHelper.TileToLookup(num2, baseOption));
					}
				}
			}
		}
    }
}