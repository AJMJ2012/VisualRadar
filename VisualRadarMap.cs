using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.Map;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.ID;

namespace VisualRadar
{
	public class VisualRadarMap : ModMapLayer {
        public override void Draw(ref MapOverlayDrawContext context, ref string text) {
            if (Main.netMode == NetmodeID.Server || !Config.Server.Map.AllowAll || !Config.Client.Map.AllEnabled) return;
			try {
				foreach (Tracker tracked in VisualRadarTracker.instance.trackedMapObjects) {
					foreach (TrackedObject trackedObject in tracked.tracked) {
						if (trackedObject.icon != null && (trackedObject.context & TrackedContext.Map) != 0 && trackedObject.enabled && trackedObject.iconScale > 0f)
							Draw(ref context, ref text, trackedObject);
					}
				}
			}
			catch {}
        }

		public void Draw(ref MapOverlayDrawContext context, ref string text, TrackedObject trackedObject)
		{
			if (Main.netMode == NetmodeID.Server) return;
			Texture2D texture = trackedObject.icon;
			Vector2 position = trackedObject.iconPosition;
			float animation = trackedObject.iconAnimation;
			float scale = trackedObject.scale;
			string iconText = trackedObject.iconText;
			float animFade = animation * MathHelper.Clamp((animation - 0.5f) * (2f), 0f, 1f);
			try {
				float alpha = 1f;
				alpha *= animFade;
				Vector2 texturePos = position / 16f;
				for (int j = 0; j <= 1; j++) {
					byte c = (byte)(j * 255);
					Texture2D iconBorder = Utilities.StencilTexture(texture, new Color(c, c, c));
					int num = 2-j;
					for (int x = -num; x <= num; x++) {
						for (int y = -num; y <= num; y++) {
							Vector2 offset = new Vector2(x,y) * 2f * (16f * (1f / (Main.mapFullscreen ? Main.mapFullscreenScale : ((Main.mapStyle != 1) ? Main.mapOverlayScale : Main.mapMinimapScale * 2f))));
							if (Math.Abs(x) + Math.Abs(y) == num) {
								context.Draw(iconBorder, texturePos + (offset / 16f), Color.White * alpha, new SpriteFrame(1,1,0,0), scale, scale, Alignment.Center);
							}
						}
					}
				}
				if (context.Draw(texture, texturePos, Color.White * alpha, new SpriteFrame(1,1,0,0), scale, scale, Alignment.Center).IsMouseOver) {
					text = iconText;
				}
			}
			catch {}
		}
    }
}