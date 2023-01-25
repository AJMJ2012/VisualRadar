using Microsoft.Xna.Framework.Graphics;
using MonoMod.RuntimeDetour.HookGen;
using System;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;

namespace VisualRadar {
    public class DisableDefaultMapBossIcon : ModSystem {
		public MethodInfo DrawNPCHeadBossMethod => typeof(Terraria.Main).GetMethod("DrawNPCHeadBoss", BindingFlags.NonPublic | BindingFlags.Static);
		public override void Load() {
			if (Main.netMode == 2) return;
			HookEndpointManager.Add(DrawNPCHeadBossMethod, Override_DrawNPCHeadBoss);
		}
		public override void Unload() {
			if (Main.netMode == 2) return;
			try { HookEndpointManager.Remove(DrawNPCHeadBossMethod, Override_DrawNPCHeadBoss); } catch {}
		}
		public void Override_DrawNPCHeadBoss(Action<Entity,byte,float,float,SpriteEffects,int,float,float> DrawNPCHeadBoss, Entity theNPC, byte alpha, float headScale, float rotation, SpriteEffects effects, int npcID, float x, float y) {}
	}
}
