using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace VisualRadar
{
	public class VisualRadar : Mod {}
	public class ModContent : ModSystem {
		public static Dictionary<string, Asset<Texture2D>> Textures = new();
		public override void OnModLoad() {
			Textures["Arrow"] = Mod.Assets.Request<Texture2D>("ModContent\\Images\\Arrow");
		}
	}
}