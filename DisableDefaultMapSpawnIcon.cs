using MonoMod.RuntimeDetour.HookGen;
using System;
using System.Reflection;
using Terraria;
using Terraria.Map;
using Terraria.ModLoader;

namespace VisualRadar {
	public class DisableDefaultMapSpawnIcon : ModSystem {
		public MethodInfo DrawMethod => typeof(Terraria.Map.SpawnMapLayer).GetMethod("Draw", BindingFlags.Public | BindingFlags.Instance);
		public override void Load() {
			if (Main.netMode == 2) return;
			HookEndpointManager.Add(DrawMethod, Override_Draw);
		}
		public override void Unload() {
			if (Main.netMode == 2) return;
			try { HookEndpointManager.Remove(DrawMethod, Override_Draw); } catch {}
		}
		public void Override_Draw(Action<SpawnMapLayer,MapOverlayDrawContext,string> Draw, SpawnMapLayer instance, ref MapOverlayDrawContext context, ref string text) {}
	}
}
