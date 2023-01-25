using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;

namespace VisualRadar {
	public static class Utilities {
		// TODO: Fix memory leak issues with this method.

		static Dictionary<Tuple<Texture2D, Color>, Texture2D> stencilTexture = new();

		public static Texture2D StencilTexture(Texture2D inputTexture, Color color) {
			Tuple<Texture2D, Color> t = new(inputTexture, color);
			if (stencilTexture.ContainsKey(t)) {
				return stencilTexture[t];
			}
			else {
				Texture2D outputTexture = new Texture2D(Main.instance.GraphicsDevice, inputTexture.Width, inputTexture.Height);
				Color[] textureColor = new Color[inputTexture.Width * inputTexture.Height];
				inputTexture.GetData<Color>(textureColor);
				for(int i = 0; i < textureColor.Length; i++) {
					float alpha = (byte)textureColor[i].A / 255f;
					textureColor[i] = new Color(color.R, color.G, color.B) * alpha;
				}
				outputTexture.SetData(textureColor);
				stencilTexture.Add(t, outputTexture);
				return outputTexture;
			}
		}

		public static float Lerp(float firstFloat, float secondFloat, float by) {
			return firstFloat * (1 - by) + secondFloat * by;
		}

		public static Vector2 Lerp(Vector2 firstVector, Vector2 secondVector, float by) {
			float retX = Lerp(firstVector.X, secondVector.X, by);
			float retY = Lerp(firstVector.Y, secondVector.Y, by);
			return new Vector2(retX, retY);
		}
	}
}
