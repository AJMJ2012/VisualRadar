using System;
using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace VisualRadar {
	[Label("Client Config")]
	public class ClientConfig : ModConfig {
		public override ConfigScope Mode => ConfigScope.ClientSide;
		public static ClientConfig Instance;

		public MapConfig Map = new();
		[Label("Map Settings")]
		public class MapConfig {
			[Label("All Enabled")]
			[DefaultValue(true)]
			public bool Enabled = true;

			[Label("Boss Enabled")]
			[DefaultValue(true)]
			public bool BossEnabled = true;

			[Label("Boss Piece Enabled")]
			[DefaultValue(true)]
			public bool BossPieceEnabled = true;

			[Label("Rare Enabled")]
			[DefaultValue(true)]
			public bool RareEnabled = true;

			[Label("Enemy Enabled")]
			[DefaultValue(true)]
			public bool EnemyEnabled = true;

			[Label("Mark Worm Tails")]
			[DefaultValue(true)]
			public bool WormTailsEnabled = true;

			[Label("Boss Bag Enabled")]
			[DefaultValue(true)]
			public bool BagEnabled = true;

			[Label("Treasure Enabled")]
			[DefaultValue(true)]
			public bool TreasureEnabled = true;
		}

		public ScreenConfig Screen = new();
		[Label("Screen Settings")]
		public class ScreenConfig {
			[Label("All Enabled")]
			[DefaultValue(true)]
			public bool Enabled = true;

			[Label("Boss Enabled")]
			[DefaultValue(true)]
			public bool BossEnabled = true;

			[Label("Boss Piece Enabled")]
			[DefaultValue(false)]
			public bool BossPieceEnabled = false;

			[Label("Rare Enabled")]
			[DefaultValue(false)]
			public bool RareEnabled = false;

			[Label("Enemy Enabled")]
			[DefaultValue(false)]
			public bool EnemyEnabled = false;

			[Label("Mark Worm Tails")]
			[DefaultValue(false)]
			public bool WormTailsEnabled = false;

			[Label("Boss Bag Enabled")]
			[DefaultValue(true)]
			public bool BagEnabled = true;

			[Label("Treasure Enabled")]
			[DefaultValue(true)]
			public bool TreasureEnabled = true;

			[Label("Fixed Icon Distance")]
			[DefaultValue(true)]
			public bool FixedDistance = true;

			[Label("Fixed Icon Base Distance")]
			[Range(16, 64)]
			[Increment(1)]
			[Slider]
			[DefaultValue(24)]
			public int FixedBaseDistance = 24;

			[Label("Fixed Icon Distance Multiplier")]
			[Range(1f, 3f)]
			[Increment(0.1f)]
			[Slider]
			[DefaultValue(1f)]
			public float FixedDistanceMult = 1f;

			[Label("Icon Scale (percent)")]
			[Range(0f, 100f)]
			[Increment(50f/6f)]
			[Slider]
			[DefaultValue(75f)]
			public float IconScale = 75f;
			
			[Label("Attach to UI Scale")]
			[DefaultValue(false)]
			public bool AttachToUIScale = false;
			
			[Label("Attach to Game Zoom")]
			[DefaultValue(true)]
			public bool AttachToGameZoom = true;

			[Label("Icon Border")]
			[DefaultValue(true)]
			public bool IconBorder = true;

			[Label("Icon Alpha (percent)")]
			[Tooltip("Doesn't work properly yet.")]
			[Range(0, 100)]
			[Increment(1)]
			[Slider]
			public int IconAlpha => 100;
		}

	}

	[Label("Server Config")]
	public class ServerConfig : ModConfig {
		public override ConfigScope Mode => ConfigScope.ServerSide;
		public static ServerConfig Instance;

		public MapConfig Map = new();
		[Label("Map Settings")]
		public class MapConfig {
			[Label("All Enabled")]
			[DefaultValue(true)]
			public bool Enabled = true;

			[Label("Boss Enabled")]
			[DefaultValue(true)]
			public bool BossEnabled = true;

			[Label("Boss Piece Enabled")]
			[DefaultValue(true)]
			public bool BossPieceEnabled = true;

			[Label("Rare Enabled")]
			[DefaultValue(true)]
			public bool RareEnabled = true;

			[Label("Enemy Enabled")]
			[DefaultValue(true)]
			public bool EnemyEnabled = true;

			[Label("Mark Worm Tails")]
			[DefaultValue(true)]
			public bool WormTailsEnabled = true;

			[Label("Boss Bag Enabled")]
			[DefaultValue(true)]
			public bool BagEnabled = true;

			[Label("Treasure Enabled")]
			[DefaultValue(true)]
			public bool TreasureEnabled = true;
		}

		public ScreenConfig Screen = new();
		[Label("Screen Settings")]
		public class ScreenConfig {
			[Label("All Enabled")]
			[DefaultValue(true)]
			public bool Enabled = true;

			[Label("Boss Enabled")]
			[DefaultValue(true)]
			public bool BossEnabled = true;

			[Label("Boss Piece Enabled")]
			[DefaultValue(false)]
			public bool BossPieceEnabled = false;

			[Label("Rare Enabled")]
			[DefaultValue(false)]
			public bool RareEnabled = false;

			[Label("Enemy Enabled")]
			[DefaultValue(false)]
			public bool EnemyEnabled = false;

			[Label("Mark Worm Tails")]
			[DefaultValue(false)]
			public bool WormTailsEnabled = false;

			[Label("Boss Bag Enabled")]
			[DefaultValue(true)]
			public bool BagEnabled = true;

			[Label("Treasure Enabled")]
			[DefaultValue(true)]
			public bool TreasureEnabled = true;

			[Label("Fixed Distance")]
			[Tooltip("-1: User\n0: Disabled\n1: Enabled")]
			[Range(-1, 1)]
			[Increment(1)]
			[Slider]
			[DefaultValue(-1)]
			public int FixedDistance = -1;
		}

		public override bool AcceptClientChanges(ModConfig pendingConfig, int whoAmI, ref string message) {
			return DALib.Auth.IsAdmin(whoAmI, ref message);
		}
	}

	public class CombinedConfig {
		public MapConfig Map = new();
		public class MapConfig {
			public bool Enabled => ClientConfig.Instance.Map.Enabled && ServerConfig.Instance.Map.Enabled;
			public bool BossEnabled => ClientConfig.Instance.Map.BossEnabled && ServerConfig.Instance.Map.BossEnabled;
			public bool BossPieceEnabled => ClientConfig.Instance.Map.BossPieceEnabled && ServerConfig.Instance.Map.BossPieceEnabled;
			public bool RareEnabled => ClientConfig.Instance.Map.RareEnabled && ServerConfig.Instance.Map.RareEnabled;
			public bool EnemyEnabled => ClientConfig.Instance.Map.EnemyEnabled && ServerConfig.Instance.Map.EnemyEnabled;
			public bool WormTailsEnabled => ClientConfig.Instance.Map.WormTailsEnabled && ServerConfig.Instance.Map.WormTailsEnabled;
			public bool BagEnabled => ClientConfig.Instance.Map.BagEnabled && ServerConfig.Instance.Map.BagEnabled;
			public bool TreasureEnabled => ClientConfig.Instance.Map.TreasureEnabled && ServerConfig.Instance.Map.TreasureEnabled;
		}

		public ScreenConfig Screen = new();
		public class ScreenConfig {
			public bool Enabled => ClientConfig.Instance.Screen.Enabled && ServerConfig.Instance.Screen.Enabled;
			public bool BossEnabled => ClientConfig.Instance.Screen.BossEnabled && ServerConfig.Instance.Screen.BossEnabled;
			public bool BossPieceEnabled => ClientConfig.Instance.Screen.BossPieceEnabled && ServerConfig.Instance.Screen.BossPieceEnabled;
			public bool RareEnabled => ClientConfig.Instance.Screen.RareEnabled && ServerConfig.Instance.Screen.RareEnabled;
			public bool EnemyEnabled => ClientConfig.Instance.Screen.EnemyEnabled && ServerConfig.Instance.Screen.EnemyEnabled;
			public bool WormTailsEnabled => ClientConfig.Instance.Screen.WormTailsEnabled && ServerConfig.Instance.Screen.WormTailsEnabled;
			public bool BagEnabled => ClientConfig.Instance.Screen.BagEnabled && ServerConfig.Instance.Screen.BagEnabled;
			public bool TreasureEnabled => ClientConfig.Instance.Screen.TreasureEnabled && ServerConfig.Instance.Screen.TreasureEnabled;
			public bool FixedDistance => (ServerConfig.Instance.Screen.FixedDistance <= -1 && ClientConfig.Instance.Screen.FixedDistance) || ServerConfig.Instance.Screen.FixedDistance >= 1;
		}
	}

	public static class Config {
		public static ClientConfig Client => ClientConfig.Instance;
		public static ServerConfig Server => ServerConfig.Instance;
		public static CombinedConfig Combined => new CombinedConfig();

	}
}