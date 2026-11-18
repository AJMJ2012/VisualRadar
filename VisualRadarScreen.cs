using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace VisualRadar;

public class VisualRadarScreen : ModSystem
{
	public static Player player => Main.LocalPlayer;
	public override void PostDrawInterface(SpriteBatch spriteBatch)
	{
		if (Main.netMode == NetmodeID.Server || (Config.EnabledFor & ConfigEnableType.Screen) <= 0) return;
		try
		{
			foreach (Tracker tracked in VisualRadarTracker.instance.trackedScreenObjects)
			{
				foreach (TrackedObject trackedObject in tracked.tracked)
				{
					if (trackedObject.icon != null && (trackedObject.context & TrackedContext.Screen) != 0 && trackedObject.enabled && trackedObject.iconScale > 0f)
						Draw(spriteBatch, trackedObject);
				}
			}
		}
		catch { }
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
		int minimum = 32;
		scale /= Main.UIScale;
		float scaleFactor = Config.Client.ScreenIcons.AttachScaleTo switch
		{
			ConfigIconZoomAttach.UIScale => Main.UIScale,
			ConfigIconZoomAttach.GameZoom => Main.GameViewMatrix.Zoom.X,
			_ => 1f,
		};
		scale *= scaleFactor;
		float edge = minimum * (0.5f + (scaleFactor / 2f));
        fixedDistance *= Config.Client.ScreenIcons.Distances.Clamped.Multiplier;
		int fixedDistOffset = Config.Client.ScreenIcons.Distances.Clamped.Offset;
		Vector2 playerScreenPosition = new Vector2(player.Center.X - Main.screenPosition.X, player.Center.Y - Main.screenPosition.Y) / Main.UIScale;
		float relativeAngle = (float)Math.Atan2(player.Center.Y - position.Y, player.Center.X - position.X);
		float arrowRotation = relativeAngle;
		float distance = Vector2.Distance(player.Center, position);
		float alpha = Config.Client.ScreenIcons.Alpha / 100f;
		float iconDistance = distance * Config.Client.ScreenIcons.Distances.Factor;
		float arrowScale = scale;
		if (Config.ScreenIcons.Type == ConfigIconType.Clamped)
			iconDistance = MathHelper.Clamp(iconDistance, 0f, fixedDistance + fixedDistOffset);
		if (iconDistance < minimum) // if (iconDistance < fixedDistance + fixedDistOffset)
		{
			float factor = MathHelper.Clamp((((iconDistance - fixedDistOffset) / minimum /*fixedDistance*/) - 0.5f) * 2f, 0f, 1f);
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
		Vector2 textureCenter = new Vector2(texture.Width, texture.Height) / 2f;
		Vector2 iconPosition = new Vector2(
			MathHelper.Clamp((float)(Math.Cos(relativeAngle) * -(iconDistance) + playerScreenPosition.X), edge, (Main.screenWidth) - edge),
			MathHelper.Clamp((float)(Math.Sin(relativeAngle) * -(iconDistance) + playerScreenPosition.Y), edge, (Main.screenHeight) - edge)
		);
		alpha *= animFade;
		//scale *= animFade;
		arrowScale *= animFade;
		if (Config.Client.ScreenIcons.DrawBorder)
		{
			for (int j = 0; j <= 1; j++)
			{
				byte c = (byte)(j * 255);
				Texture2D arrowBorder = Utilities.StencilTexture(arrowTexture, new Color(c, c, c));
				Texture2D iconBorder = Utilities.StencilTexture(texture, new Color(c, c, c));
				int num = 2 - j;
				for (int x = -num; x <= num; x++)
				{
					for (int y = -num; y <= num; y++)
					{
						Vector2 outlineoffset = new Vector2(x, y) * 2f;
						if (Math.Abs(x) + Math.Abs(y) == num)
						{
							if (drawArrow)
							{
								spriteBatch.Draw(arrowBorder, iconPosition, arrowRect, Color.White * alpha, arrowRotation, arrowCenter + new Vector2(textureCenter.X, 0f) + outlineoffset, arrowScale, SpriteEffects.None, 0f);
							}
							spriteBatch.Draw(iconBorder, iconPosition, textureRect, Color.White * alpha, 0f, textureCenter + outlineoffset, scale, SpriteEffects.None, 0f);
						}
					}
				}
			}
		}
		if (drawArrow)
		{
			spriteBatch.Draw(arrowTexture, iconPosition, arrowRect, Color.White * alpha, arrowRotation, arrowCenter + new Vector2(textureCenter.X, 0f), arrowScale, SpriteEffects.None, 0f);
		}
		spriteBatch.Draw(texture, iconPosition, textureRect, Color.White * alpha, 0f, textureCenter, scale, SpriteEffects.None, 0f);
	}
}