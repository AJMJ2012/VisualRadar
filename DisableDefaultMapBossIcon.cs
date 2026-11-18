using Microsoft.Xna.Framework.Graphics;
using System;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;

namespace VisualRadar;

public class DisableDefaultMapBossIcon : ModSystem
{
	public MethodInfo DrawNPCHeadBoss_Method => typeof(Terraria.Main).GetMethod("DrawNPCHeadBoss", BindingFlags.NonPublic | BindingFlags.Static);
	public delegate void DrawNPCHeadBoss_Delegate(Entity theNPC, byte alpha, float headScale, float rotation, SpriteEffects effects, int npcID, float x, float y);
	public override void Load()
	{
		if (Main.netMode == 2) return;
		try { MonoModHooks.Add(DrawNPCHeadBoss_Method, Override_DrawNPCHeadBoss); } catch { throw new Exception("Could not hook into Main.DrawNPCHeadBoss"); }
	}
	public void Override_DrawNPCHeadBoss(DrawNPCHeadBoss_Delegate DrawNPCHeadBoss, Entity theNPC, byte alpha, float headScale, float rotation, SpriteEffects effects, int npcID, float x, float y) { }
}
