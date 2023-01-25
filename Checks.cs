using Terraria;
using Terraria.ID;

namespace VisualRadar {
	public static class Checks {
		public static bool IsBoss(NPC npc) {
			if (npc.boss || NPCID.Sets.ShouldBeCountedAsBoss[npc.type] || npc.GetBossHeadTextureIndex() >= 0) { return true; }
			switch (npc.type) {
				case NPCID.EaterofWorldsHead:
				case NPCID.GolemHead:
				case NPCID.QueenBee:
					return true;
			}
			return false;
		}

		public static bool IsBossPiece(NPC npc) {
			if (IsChild(npc) && (Main.npc[npc.realLife].boss || NPCID.Sets.ShouldBeCountedAsBoss[Main.npc[npc.realLife].type] || Main.npc[npc.realLife].GetBossHeadTextureIndex() >= 0)) { return true; }
			switch (npc.type) {
				case NPCID.Creeper:
				case NPCID.EaterofWorldsBody:
				case NPCID.EaterofWorldsTail:
				case NPCID.Golem:
				case NPCID.GolemFistLeft:
				case NPCID.GolemFistRight:
				case NPCID.MartianSaucer:
				case NPCID.MartianSaucerCannon:
				case NPCID.MartianSaucerTurret:
				case NPCID.MoonLordCore:
				case NPCID.MoonLordHand:
				case NPCID.MoonLordHead:
				case NPCID.MoonLordFreeEye:
				case NPCID.PlanterasHook:
				case NPCID.PlanterasTentacle:
				case NPCID.PrimeCannon:
				case NPCID.PrimeLaser:
				case NPCID.PrimeSaw:
				case NPCID.PrimeVice:
				case NPCID.SkeletronHand:
				case NPCID.SkeletronHead:
				case NPCID.TheDestroyerBody:
				case NPCID.TheDestroyerTail:
				case NPCID.TheHungry:
				case NPCID.TheHungryII:
				case NPCID.WallofFlesh:
				case NPCID.WallofFleshEye:
				case NPCID.Probe:
					return true;
			}
			return false;
		}

		public static bool IsChild(NPC npc) {
			return (npc.realLife >= 0 && npc.realLife != npc.whoAmI);
		}

		public static bool IsValidNPC(NPC npc) {
			return (!npc.friendly && npc.damage > 0 && npc.lifeMax > 0 && !npc.dontCountMe && !npc.hide);
		}
	}
}
