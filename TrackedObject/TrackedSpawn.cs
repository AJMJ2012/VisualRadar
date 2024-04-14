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
    public class TrackedSpawn : TrackedObject {
		public void Get(TrackedContext context, bool checkBed) {
			active = false;
			scale = 1f;
			int[] mirrors = new int[]{
				ItemID.MagicMirror,
				ItemID.IceMirror,
				ItemID.CellPhone
			};
			int[] items = new int[]{
				player.inventory[player.selectedItem].type,
				Main.mouseItem.type,
				Main.HoverItem.type,
			};
			if (((context == TrackedContext.Screen && items.Any(x => mirrors.Contains(x))) || context == TrackedContext.Map) && (checkBed && player.SpawnX != -1 || !checkBed)) {
				if (checkBed && player.SpawnX != -1) {
					iconText = Language.GetTextValue("UI.SpawnBed");
					icon = TextureAssets.SpawnBed.Value;
					position = new Vector2(player.SpawnX, player.SpawnY) * 16;
				}
				else {
					iconText = Language.GetTextValue("UI.SpawnPoint");
					icon = TextureAssets.SpawnPoint.Value;
					position = new Vector2(Main.spawnTileX, Main.spawnTileY) * 16;
				}
				position += new Vector2(8f, -8f);
				fixedDistance = 48;
				active = true;
			}
		}
    }
}