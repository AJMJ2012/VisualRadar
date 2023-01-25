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
    public class TrackedBag : TrackedObject {
		public void Get(string context, Item item) {
            scale = 0f;
			if (!item.active || item.type == 0) return;
			if ((context == "map" && Config.Combined.Map.BagEnabled) || (context == "screen" && Config.Combined.Screen.BagEnabled)) {
				if (ItemID.Sets.BossBag[item.type]) {
			        iconText = item.AffixName();
					icon = TextureAssets.Item[item.type].Value;
					position = item.Center;
					fixedDistance = 64;
					scale = 1f;
				}
			}
		}
    }
}