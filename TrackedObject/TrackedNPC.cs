using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

using DALib.Checks;
using Terraria.ID;
using System.Collections.Generic;
using System.Linq;
using Terraria.GameContent.Animations;
using System;

namespace VisualRadar
{
	public class TrackedNPC : TrackedObject
	{
		public static bool RadarEnabled => player.accThirdEye && !player.hideInfo[InfoDisplay.Radar.Type];
		public static bool LifeformAnalyzerEnabled => player.accCritterGuide && !player.hideInfo[InfoDisplay.LifeformAnalyzer.Type];
		public static bool RadarOrLifeformAnalyzerEnabled => RadarEnabled || LifeformAnalyzerEnabled;
		public const int radarSearchDistance = 2000;
		public const int lifeformAnalyzerSearchDistance = 1300;

		public static bool IsValid(NPC npc)
		{
			return (!npc.friendly && npc.damage > 0 && npc.lifeMax > 0 && !npc.dontCountMe && !npc.hide);
		}

		public void Get(TrackedContext context, NPC npc, NPCTracker instance)
		{
			active = false;
			scale = 1f;
			if (!npc.active || npc.type == NPCID.None) return;
			bool isChild = npc.IsChild();
			bool isParent = npc.realLife > 0 && npc.realLife == npc.whoAmI;
			bool isBoss = npc.IsBoss();
			bool isBossPiece = npc.IsBossPiece();
			bool isRare = npc.rarity > 0;
			bool isNormal = (!isChild && IsValid(npc)) || (isChild && IsValid(Main.npc[npc.realLife]));
			float npcDistance = Vector2.Distance(player.Center, npc.Center);
			if ((isChild || isBoss || isBossPiece || isRare || isNormal) && ((isBoss && !isBossPiece) || (npcDistance < radarSearchDistance * (isBossPiece ? 2f : 1f))))
			{
				iconText = npc.FullName;
				drawArrow = !isChild;
				position = npc.Center;
				lerpMode = LerpMode.Fast;
				lerpPosition = context == TrackedContext.Screen;
				fixedDistance = isBoss ? 192 : isBossPiece ? 160 : isRare ? 128 : 96;
				if (isBoss && ((context == TrackedContext.Map && Config.Combined.Map.BossEnabled) || (context == TrackedContext.Screen && Config.Combined.Screen.BossEnabled)) && RadarOrLifeformAnalyzerEnabled)
				{
					icon = TextureAssets.InfoIcon[8].Value;
					if (npc.GetBossHeadTextureIndex() >= 0 && npc.GetBossHeadTextureIndex() < TextureAssets.NpcHeadBoss.Length)
					{
						icon = TextureAssets.NpcHeadBoss[npc.GetBossHeadTextureIndex()].Value;
					}
					active = true;
				}
				else if (isBossPiece)
				{
					if (((context == TrackedContext.Map && Config.Combined.Map.BossPieceEnabled) || (context == TrackedContext.Screen && Config.Combined.Screen.BossPieceEnabled)) && RadarOrLifeformAnalyzerEnabled)
					{
						icon = TextureAssets.InfoIcon[8].Value;
						active = true;
					}
				}
				else if (isRare && ((context == TrackedContext.Map && Config.Combined.Map.RareEnabled) || (context == TrackedContext.Screen && Config.Combined.Screen.RareEnabled)) && LifeformAnalyzerEnabled && npcDistance < lifeformAnalyzerSearchDistance)
				{
					icon = TextureAssets.InfoIcon[11].Value;
					active = true;
				}
				else if (isNormal && ((context == TrackedContext.Map && Config.Combined.Map.EnemyEnabled) || (context == TrackedContext.Screen && Config.Combined.Screen.EnemyEnabled)) && RadarEnabled)
				{
					icon = TextureAssets.InfoIcon[5].Value;
					active = true;
				}
				if (active)
				{
					if (isChild)
					{
						active = false;
						icon = TextureAssets.InfoIcon[isBossPiece ? 8 : 6].Value;
						if ((context == TrackedContext.Map && Config.Combined.Map.WormBodyStyle == WormBodyStyle.WholeBody) || (context == TrackedContext.Screen && Config.Combined.Screen.WormBodyStyle == WormBodyStyle.WholeBody))
						{
							active = true;
							scale = (2f / 3f);
						}
						else if ((context == TrackedContext.Map && Config.Combined.Map.WormBodyStyle == WormBodyStyle.ClosestAndHead) || (context == TrackedContext.Screen && Config.Combined.Screen.WormBodyStyle == WormBodyStyle.ClosestAndHead))
						{
							// Apply single icon to whole body. Head still keeps the original icon.
							NPC parent = Main.npc[npc.realLife];
							bool average = false; //parent.aiStyle != NPCAIStyleID.Worm && parent.aiStyle != NPCAIStyleID.TheDestroyer;
							int trackedSegmentID = (int)npc.realLife;
							if (trackedSegmentID > 0 && Main.npc[trackedSegmentID].active)
							{
								NPC segment = Main.npc[trackedSegmentID];
								TrackedNPC trackedSegment = (TrackedNPC)instance.tracked[trackedSegmentID];
								trackedSegment.icon = TextureAssets.InfoIcon[parent.IsBoss() ? 8 : 6].Value;
								trackedSegment.fixedDistance = parent.IsBoss() ? 192 : parent.IsBossPiece() ? 160 : parent.rarity > 0 ? 128 : 96;
								trackedSegment.position = segment.Center;
								trackedSegment.lerpMode = LerpMode.Fast;
								trackedSegment.drawArrow = true;
								trackedSegment.active = true;
								trackedSegment.iconText = iconText;
								if (average)
								{
									List<Vector2> positions = new();
									segment = Main.npc[trackedSegmentID];
									for (int i = 0; segment.ai[0] > 0 && i < 200; i++)
									{
										if (Main.npc[(int)segment.ai[0]].active)
										{
											segment = Main.npc[(int)segment.ai[0]];
											positions.Add(segment.Center);
										}
									}
									trackedSegment.position = new Vector2(positions.Average(x => x.X), positions.Average(x => x.Y));
								}
								else
								{
									segment = Main.npc[trackedSegmentID];
									for (int i = 0; segment.ai[0] > 0 && i < 200; i++)
									{
										if (Main.npc[(int)segment.ai[0]].active)
										{
											segment = Main.npc[(int)segment.ai[0]];
											float parentDistance = Vector2.Distance(player.Center, trackedSegment.position);
											float segmentDistance = Vector2.Distance(player.Center, segment.Center);
											if (segmentDistance < parentDistance)
											{
												trackedSegment.position = segment.Center;
											}
										}
									}
								}
							}
						}
					}
					else if (isParent)
					{
						if ((context == TrackedContext.Map && Config.Combined.Map.WormBodyStyle == WormBodyStyle.ClosestOnly) || (context == TrackedContext.Screen && Config.Combined.Screen.WormBodyStyle == WormBodyStyle.ClosestOnly))
						{
							// Move head icon across the whole body
							NPC segment = npc;
							bool average = false; //npc.aiStyle != NPCAIStyleID.Worm && npc.aiStyle != NPCAIStyleID.TheDestroyer;
							if (average)
							{
								List<Vector2> positions = new();
								for (int i = 0; segment.ai[0] > 0 && i < 200; i++)
								{
									if (Main.npc[(int)segment.ai[0]].active)
									{
										segment = Main.npc[(int)segment.ai[0]];
										positions.Add(segment.Center);
									}
								}
								position = new Vector2(positions.Average(x => x.X), positions.Average(x => x.Y));
							}
							else
							{
								for (int i = 0; segment.ai[0] > 0 && i < 200; i++)
								{
									if (Main.npc[(int)segment.ai[0]].active)
									{
										segment = Main.npc[(int)segment.ai[0]];
										float parentDistance = Vector2.Distance(player.Center, position);
										float segmentDistance = Vector2.Distance(player.Center, segment.Center);
										if (segmentDistance < parentDistance)
										{
											position = segment.Center;
										}
									}
								}
							}
						}
					}
				}
			}
		}
	}
}