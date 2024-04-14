using MonoMod.RuntimeDetour.HookGen;
using System;
using System.Reflection;
using Terraria;
using Terraria.Map;
using Terraria.ModLoader;

namespace VisualRadar {
	public class DisableDefaultMapSpawnIcon : ModSystem {
		public MethodInfo DrawMethod => typeof(Terraria.Map.SpawnMapLayer).GetMethod("Draw", BindingFlags.Public | BindingFlags.Instance);
		public delegate void DrawDelegate(SpawnMapLayer instance, ref MapOverlayDrawContext context, ref string text);
		public override void Load() {
			if (Main.netMode == 2) return;
			try { MonoModHooks.Add(DrawMethod, Override_Draw); } catch { throw new Exception("Could not hook into SpawnMapLayer.Draw"); }
		}
		public void Override_Draw(DrawDelegate Draw, SpawnMapLayer instance, ref MapOverlayDrawContext context, ref string text) {}
	}
}
