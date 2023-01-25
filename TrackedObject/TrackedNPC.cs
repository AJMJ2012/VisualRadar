using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.Map;
using Terraria.ModLoader;

namespace VisualRadar {
    public class TrackedNPC : TrackedObject {
		public static bool RadarEnabled => player.accThirdEye && !player.hideInfo[InfoDisplay.Radar.Type];
		public static bool LifeformAnalyzerEnabled => player.accCritterGuide && !player.hideInfo[InfoDisplay.LifeformAnalyzer.Type];
		public static bool RadarOrLifeformAnalyzerEnabled => RadarEnabled || LifeformAnalyzerEnabled;
		public const int radarSearchDistance = 2000;
		public const int lifeformAnalyzerSearchDistance = 1300;
		public void Get(string context, NPC npc, NPCTracker instance) {
            scale = 0f;
			if (!npc.active || npc.type == 0) return;
			bool isChild = Checks.IsChild(npc);
			bool isBoss = Checks.IsBoss(npc);
			bool isBossPiece = Checks.IsBossPiece(npc);
			bool isRare = npc.rarity > 0;
			bool isNormal = (!isChild && Checks.IsValidNPC(npc)) || (isChild && Checks.IsValidNPC(Main.npc[npc.realLife]));
			float npcDistance = Vector2.Distance(player.Center, npc.Center);
			if ((isChild || isBoss || isBossPiece || isRare || isNormal) && ((isBoss && !isBossPiece) || npcDistance < radarSearchDistance)) {

			    iconText = npc.FullName;
                drawArrow = !isChild;
				position = npc.Center;

				if (isBoss && ((context == "map" && Config.Combined.Map.BossEnabled) || (context == "screen" && Config.Combined.Screen.BossEnabled)) && RadarOrLifeformAnalyzerEnabled) {
					icon = TextureAssets.InfoIcon[8].Value;
					if (npc.GetBossHeadTextureIndex() >= 0 && npc.GetBossHeadTextureIndex() < TextureAssets.NpcHeadBoss.Length) {
						icon = TextureAssets.NpcHeadBoss[npc.GetBossHeadTextureIndex()].Value;
					}
					fixedDistance = 64;
					scale = 1f;
				}
				else if (isBossPiece) {
					if (((context == "map" && Config.Combined.Map.BossPieceEnabled) || (context == "screen" && Config.Combined.Screen.BossPieceEnabled)) && RadarOrLifeformAnalyzerEnabled) {
						icon = TextureAssets.InfoIcon[8].Value;
						fixedDistance = 48;
						scale = 1f;
					}
				}
				else if (isRare && ((context == "map" && Config.Combined.Map.RareEnabled) || (context == "screen" && Config.Combined.Screen.RareEnabled)) && LifeformAnalyzerEnabled && npcDistance < lifeformAnalyzerSearchDistance) {
					icon = TextureAssets.InfoIcon[11].Value;
					fixedDistance = 48;
					scale = 1f;
				}
				else if (isNormal && ((context == "map" && Config.Combined.Map.EnemyEnabled) || (context == "screen" && Config.Combined.Screen.EnemyEnabled)) && RadarEnabled) {
					icon = TextureAssets.InfoIcon[5].Value;
					fixedDistance = 32;
					scale = 1f;
				}
				if (isChild && icon != null) {
					if ((context == "map" && Config.Combined.Map.WormTailsEnabled) || (context == "screen" && Config.Combined.Screen.WormTailsEnabled)) {
						icon = TextureAssets.InfoIcon[Checks.IsBoss(Main.npc[npc.realLife]) ? 8 : 6].Value;
						scale = (2f / 3f);
					}
					else {
						icon = null;
						scale = 0f;

						// Apply single icon to whole body.
						NPC segment = Main.npc[npc.realLife];

						int firstSegmentID = npc.realLife;
						if (segment.ai[0] > 0 && Main.npc[(int)segment.ai[0]].active) {
							firstSegmentID = (int)segment.ai[0];
							segment = Main.npc[firstSegmentID];
							instance.tracked[firstSegmentID].icon = TextureAssets.InfoIcon[Checks.IsBoss(Main.npc[npc.realLife]) ? 8 : 6].Value;
							instance.tracked[firstSegmentID].scale = (2f / 3f);
							instance.tracked[firstSegmentID].position = segment.Center;
							instance.tracked[firstSegmentID].lerpPosition = context == "screen";
							instance.tracked[firstSegmentID].linearLerp = true;
							instance.tracked[firstSegmentID].drawArrow = true;
						}

						for (int i = 0; segment.ai[0] > 0 && i < 200; i++) {
							if (Main.npc[(int)segment.ai[0]].active) {
								segment = Main.npc[(int)segment.ai[0]];
								float parentDistance = Vector2.Distance(player.Center, instance.tracked[firstSegmentID].position);
								float segmentDistance = Vector2.Distance(player.Center, segment.Center - segment.velocity);
								if (segmentDistance < parentDistance) {
									instance.tracked[firstSegmentID].position = segment.Center;
								}
							}
						}
					}
				}
			}
		}
    }
}