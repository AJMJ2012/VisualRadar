using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace VisualRadar
{
	public class VisualRadarScreen : ModSystem {
		public static Player player => Main.LocalPlayer;
        public override void PostDrawInterface(SpriteBatch spriteBatch) {
            if (Main.netMode == NetmodeID.Server || !Config.Server.Screen.AllowAll || !Config.Client.Screen.AllEnabled) return;
			try {
				foreach (Tracker tracked in VisualRadarTracker.instance.trackedScreenObjects)
				{
					foreach (TrackedObject trackedObject in tracked.tracked)
					{
						if (trackedObject.icon != null && (trackedObject.context & TrackedContext.Screen) != 0 && trackedObject.enabled && trackedObject.iconScale > 0f)
							Draw(spriteBatch, trackedObject);
					}
				}
			}
			catch {}
        }

		public void Draw(SpriteBatch spriteBatch, TrackedObject trackedObject)
		{
			if (Main.netMode == NetmodeID.Server) return;
			Texture2D texture = trackedObject.icon;
			Vector2 position = trackedObject.iconPosition;
			float animation = trackedObject.iconAnimation;
			float smoothAnimation = trackedObject.iconSmoothAnimation;
			float scale = trackedObject.iconScale;
			float fixedDistance = trackedObject.iconFixedDistance;
			bool drawArrow = trackedObject.drawArrow;
			float animFade = animation * MathHelper.Clamp((animation - 0.5f) * (2f), 0f, 1f);
			float animEase = smoothAnimation;

			scale /= Main.UIScale;
			float scaleFactor = 0f;
			if (Config.Client.Screen.AttachToUIScale) scaleFactor = Math.Max(scaleFactor, Main.UIScale);
			if (Config.Client.Screen.AttachToGameZoom) scaleFactor = Math.Max(scaleFactor, Main.GameViewMatrix.Zoom.X);
			if (scaleFactor <= 0f) scaleFactor = 1f;
			scale *= scaleFactor;
			float edge = (32) * (0.5f + (scaleFactor / 2f));
			fixedDistance = fixedDistance * Config.Client.Screen.FixedDistanceMult;
			int fixedDistOffset = Config.Client.Screen.FixedBaseDistance;
			Vector2 playerScreenPosition = new Vector2(player.Center.X - Main.screenPosition.X, player.Center.Y - Main.screenPosition.Y) / Main.UIScale;
			float relativeAngle = (float)Math.Atan2(player.Center.Y - position.Y, player.Center.X - position.X);
			float arrowRotation = relativeAngle;
			float distance = Vector2.Distance(player.Center, position);
			float alpha = Config.Client.Screen.IconAlpha / 100f;
			float iconDistance = distance / 2f;
			float arrowScale = scale;
			if (Config.Combined.Screen.IconDistance == IconDistance.Fixed) iconDistance = MathHelper.Clamp(iconDistance, 0f, fixedDistance + fixedDistOffset);
			//if (iconDistance < fixedDistance + fixedDistOffset)
			if (iconDistance < 32)
			{
				//float factor = MathHelper.Clamp((((iconDistance - fixedDistOffset) / fixedDistance) - 0.5f) * 2f, 0f, 1f);
				float factor = MathHelper.Clamp((((iconDistance - fixedDistOffset) / 32) - 0.5f) * 2f, 0f, 1f);
				alpha *= factor;
				arrowScale *= factor;
			}
			iconDistance *= animEase;
			iconDistance /= Main.UIScale;
			iconDistance *= Main.GameViewMatrix.Zoom.X;
			Texture2D arrowTexture = ModContent.Textures["Arrow"].Value;
			Vector2 arrowCenter = new Vector2(arrowTexture.Width, arrowTexture.Height) / 2f;
			Rectangle? arrowRect = new Rectangle?(new Rectangle(0, 0, arrowTexture.Width, arrowTexture.Height));
			Rectangle? textureRect = new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height));
			Vector2 textureCenter = new Vector2(texture.Width,texture.Height) / 2f;
			Vector2 iconPosition = new Vector2(
				MathHelper.Clamp((float)(Math.Cos(relativeAngle) * -(iconDistance) + playerScreenPosition.X), edge, (Main.screenWidth) - edge),
				MathHelper.Clamp((float)(Math.Sin(relativeAngle) * -(iconDistance) + playerScreenPosition.Y), edge, (Main.screenHeight) - edge)
			);
			alpha *= animFade;
			//scale *= animFade;
			arrowScale *= animFade;
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