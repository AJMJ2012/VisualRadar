using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Map;
using Terraria.ModLoader;
using Terraria.UI;

namespace VisualRadar {
    public class VisualRadarMap : ModMapLayer {
        public override void Draw(ref MapOverlayDrawContext context, ref string text) {
            if (Main.netMode == 2 || !Config.Server.Map.Enabled || !Config.Client.Map.Enabled) return;
			try {
				foreach (Tracker tracked in VisualRadarTracker.instance.trackedMapObjects) {
					foreach (TrackedObject trackedObject in tracked.tracked) {
						if (trackedObject.icon != null && trackedObject.iconScale > 0f)
							Draw(ref context, trackedObject.icon, trackedObject.iconPosition, trackedObject.iconScale, trackedObject.iconText, ref text);
					}
				}
			}
			catch {}
        }

		public void Draw(ref MapOverlayDrawContext context, Texture2D texture, Vector2 position, float scale, string iconText, ref string text) {
            if (Main.netMode == 2) return;
			try {
				Vector2 texturePos = position / 16f;
				for (int j = 0; j <= 1; j++) {
					byte c = (byte)(j * 255);
					Texture2D iconBorder = Utilities.StencilTexture(texture, new Color(c, c, c));
					int num = 2-j;
					for (int x = -num; x <= num; x++) {
						for (int y = -num; y <= num; y++) {
							Vector2 offset = new Vector2(x,y) * 2f * (16f * (1f / (Main.mapFullscreen ? Main.mapFullscreenScale : ((Main.mapStyle != 1) ? Main.mapOverlayScale : Main.mapMinimapScale * 2f))));
							if (Math.Abs(x) + Math.Abs(y) == num) {
								context.Draw(iconBorder, texturePos + (offset / 16f), Color.White, new SpriteFrame(1,1,0,0), scale, scale, Alignment.Center);
							}
						}
					}
				}
				if (context.Draw(texture, texturePos, Color.White, new SpriteFrame(1,1,0,0), scale, scale, Alignment.Center).IsMouseOver) {
					text = iconText;
				}
			}
			catch {}
		}
    }
}