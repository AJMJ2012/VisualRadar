using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace VisualRadar
{
	public class ReplaceSpawnBedIcons : ModSystem {
		public override void Load() {
			if (Main.netMode == 2) return;
			TextureAssets.SpawnPoint = Mod.Assets.Request<Texture2D>("Content\\Images\\UI\\SpawnPoint");
			TextureAssets.SpawnBed = Mod.Assets.Request<Texture2D>("Content\\Images\\UI\\SpawnBed");
		}

		public override void Unload() {
			if (Main.netMode == 2) return;
			TextureAssets.SpawnPoint =  Main.Assets.Request<Texture2D>("Images\\UI\\SpawnPoint", AssetRequestMode.ImmediateLoad);
			TextureAssets.SpawnBed = Main.Assets.Request<Texture2D>("Images\\UI\\SpawnBed", AssetRequestMode.ImmediateLoad);
		}
	}
}
