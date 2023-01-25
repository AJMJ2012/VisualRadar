using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace VisualRadar {
    public class VisualRadarScreen : ModSystem {
		public static Player player => Main.LocalPlayer;
        public override void PostDrawInterface(SpriteBatch spriteBatch) {
            if (Main.netMode == 2 || !Config.Server.Screen.Enabled || !Config.Client.Screen.Enabled) return;
			try {
				foreach (Tracker tracked in VisualRadarTracker.instance.trackedScreenObjects) {
					for (int i = tracked.tracked.Length - 1; i >= 0; i--) {
						TrackedObject trackedObject = tracked.tracked[i];
						if (trackedObject.icon != null && trackedObject.iconScale > 0f)
							Draw(spriteBatch, trackedObject.icon, trackedObject.iconPosition, trackedObject.iconScale, trackedObject.fixedDistance, trackedObject.drawArrow);
					}
				}
			}
			catch {}
        }
		public void Draw(SpriteBatch spriteBatch, Texture2D texture, Vector2 position, float scale = 1f, int fixedDistance = 0, bool drawArrow = true) {
			if (!Config.Client.Screen.AttachToUIScale) scale /= Main.UIScale;
			if (Config.Client.Screen.AttachToGameZoom) scale *= Main.GameViewMatrix.Zoom.X;
			fixedDistance = (int)(fixedDistance * Config.Client.Screen.FixedDistanceMult);
			int fixedDistOffset = Config.Client.Screen.FixedBaseDistance;
			fixedDistance += fixedDistOffset;
			Vector2 playerScreenPosition = new Vector2(player.Center.X - Main.screenPosition.X, player.Center.Y - Main.screenPosition.Y) / Main.UIScale;
			float relativeAngle = (float)Math.Atan2(player.Center.Y - position.Y, player.Center.X - position.X);
			float arrowRotation = relativeAngle;
			float distance = Vector2.Distance(player.Center, position);
			float alpha = Config.Client.Screen.IconAlpha / 100f;

			float iconDistance = distance / 3f;
			float arrowScale = scale * MathHelper.Clamp(iconDistance / 16f, 0f, 1f);
			if (Config.Combined.Screen.FixedDistance) {
				iconDistance = MathHelper.Clamp(iconDistance, 0f, fixedDistance);
//				arrowScale = scale * MathHelper.Clamp((distance - 32f) / fixedDistance, 0f, 1f);
			}
			iconDistance /= Main.UIScale;
			iconDistance *= Main.GameViewMatrix.Zoom.X;
			Texture2D arrowTexture = ModContent.Textures["Arrow"].Value;
			Vector2 arrowCenter = new Vector2(arrowTexture.Width, arrowTexture.Height) / 2f;
			Rectangle? arrowRect = new Rectangle?(new Rectangle(0, 0, arrowTexture.Width, arrowTexture.Height));
			Rectangle? textureRect = new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height));
			Vector2 textureCenter = new Vector2(texture.Width,texture.Height) / 2f;
			Vector2 iconPosition = new Vector2(
				MathHelper.Clamp((float)(Math.Cos(relativeAngle) * -(iconDistance) + playerScreenPosition.X), 32, Main.PendingResolutionWidth-32),
				MathHelper.Clamp((float)(Math.Sin(relativeAngle) * -(iconDistance) + playerScreenPosition.Y), 32, Main.PendingResolutionHeight-32)
			);
			if (Config.Client.Screen.IconBorder) {
				for (int j = 0; j <= 1; j++) {
					byte c = (byte)(j * 255);
					Texture2D arrowBorder = Utilities.StencilTexture(arrowTexture, new Color(c, c, c));
					Texture2D iconBorder = Utilities.StencilTexture(texture, new Color(c, c, c));
					int num = 2-j;
					for (int x = -num; x <= num; x++) {
						for (int y = -num; y <= num; y++) {
							Vector2 outlineoffset = new Vector2(x,y) * 2f;
							if (Math.Abs(x) + Math.Abs(y) == num) {
								if (drawArrow) {
									spriteBatch.Draw(arrowBorder, iconPosition, arrowRect, Color.White * alpha, arrowRotation, arrowCenter + new Vector2(textureCenter.X, 0f) + outlineoffset, arrowScale, SpriteEffects.None, 0f);
								}
								spriteBatch.Draw(iconBorder, iconPosition, textureRect, Color.White * alpha, 0f, textureCenter + outlineoffset, scale, SpriteEffects.None, 0f);
							}
						}
					}
				}
			}
			if (drawArrow) {
				spriteBatch.Draw(arrowTexture, iconPosition, arrowRect, Color.White * alpha, arrowRotation, arrowCenter + new Vector2(textureCenter.X, 0f), arrowScale, SpriteEffects.None, 0f);
			}
			spriteBatch.Draw(texture, iconPosition, textureRect, Color.White * alpha, 0f, textureCenter, scale, SpriteEffects.None, 0f);
		}
    }
}