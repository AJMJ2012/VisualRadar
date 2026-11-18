using System;
using System.ComponentModel;
using Terraria.Localization;
using Terraria.ModLoader.Config;

namespace VisualRadar;

public enum WormBodyStyle : byte
{
	ClosestOnly,
	WholeBody,
	ClosestAndHead,
}
public enum OverrideWormBodyStyle : byte
{
	Unset,
	ClosestOnly,
	WholeBody,
	ClosestAndHead,
}

public enum IconDistance : byte
{
	Fixed,
	Floating,
}

public enum OverrideIconDistance : byte
{
	Unset,
	Fixed,
	Floating,
}

public class ClientConfig : ModConfig
{
	public override ConfigScope Mode => ConfigScope.ClientSide;
	public static ClientConfig Instance;

	public MapConfig Map = new();
	public class MapConfig
	{
		[DefaultValue(true)]
		public bool AllEnabled = true;

		[DefaultValue(true)]
		public bool BossEnabled = true;

		[DefaultValue(true)]
		public bool BossPieceEnabled = true;

		[DefaultValue(true)]
		public bool RareEnabled = true;

		[DefaultValue(true)]
		public bool EnemyEnabled = true;

		[DefaultValue(true)]
		public bool WormBodyEnabled = true;

		[DefaultValue(WormBodyStyle.WholeBody)]
		public WormBodyStyle WormBodyStyle = WormBodyStyle.WholeBody;

		[DefaultValue(true)]
		public bool BagsEnabled = true;

		[DefaultValue(true)]
		public bool TreasureEnabled = true;
	}

	public ScreenConfig Screen = new();
	public class ScreenConfig
	{
		[DefaultValue(true)]
		public bool AllEnabled = true;

		[DefaultValue(true)]
		public bool BossEnabled = true;

		[DefaultValue(false)]
		public bool BossPieceEnabled = false;

		[DefaultValue(true)]
		public bool RareEnabled = true;

		[DefaultValue(false)]
		public bool EnemyEnabled = false;

		[DefaultValue(true)]
		public bool WormBodyEnabled = true;

		[DefaultValue(WormBodyStyle.ClosestOnly)]
		public WormBodyStyle WormBodyStyle = WormBodyStyle.ClosestOnly;

		[DefaultValue(true)]
		public bool BagsEnabled = true;

		[DefaultValue(true)]
		public bool TreasureEnabled = true;

		[DefaultValue(IconDistance.Fixed)]
		public IconDistance IconDistance = IconDistance.Fixed;

		[Range(0, 64)]
		[Increment(1)]
		[Slider]
		[DefaultValue(0)]
		public int FixedBaseDistance = 0;

		[Range(1f, 3f)]
		[Increment(0.1f)]
		[Slider]
		[DefaultValue(1f)]
		public float FixedDistanceMult = 1f;

		[Range(0f, 100f)]
		[Increment(50f / 6f)]
		[Slider]
		[DefaultValue(100f)]
		public float IconScale = 75f;

		[DefaultValue(false)]
		public bool AttachToUIScale = true;

		[DefaultValue(true)]
		public bool AttachToGameZoom = false;

		[DefaultValue(true)]
		public bool IconBorder = true;

		[Range(0, 100)]
		[Increment(1)]
		[Slider]
		public int IconAlpha => 100;
	}

}

public class ServerConfig : ModConfig
{
	public override ConfigScope Mode => ConfigScope.ServerSide;
	public static ServerConfig Instance;

	public MapConfig Map = new();
	public class MapConfig
	{
		[DefaultValue(true)]
		public bool AllowAll = true;

		[DefaultValue(true)]
		public bool AllowBoss = true;

		[DefaultValue(true)]
		public bool AllowBossPiece = true;

		[DefaultValue(true)]
		public bool AllowRare = true;

		[DefaultValue(true)]
		public bool AllowEnemy = true;

		[DefaultValue(true)]
		public bool AllowWormBody = true;

		[DefaultValue(true)]
		public OverrideWormBodyStyle OverrideWormBodyStyle = OverrideWormBodyStyle.Unset;

		[DefaultValue(true)]
		public bool AllowBags = true;

		[DefaultValue(true)]
		public bool AllowTreasure = true;
	}

	public ScreenConfig Screen = new();
	public class ScreenConfig
	{
		[DefaultValue(true)]
		public bool AllowAll = true;

		[DefaultValue(true)]
		public bool AllowBoss = true;

		[DefaultValue(true)]
		public bool AllowBossPiece = true;

		[DefaultValue(true)]
		public bool AllowRare = true;

		[DefaultValue(true)]
		public bool AllowEnemy = true;

		[DefaultValue(true)]
		public bool AllowWormBody = true;

		[DefaultValue(OverrideWormBodyStyle.Unset)]
		public OverrideWormBodyStyle OverrideWormBodyStyle = OverrideWormBodyStyle.Unset;

		[DefaultValue(true)]
		public bool AllowBags = true;

		[DefaultValue(true)]
		public bool AllowTreasure = true;

		[DefaultValue(OverrideIconDistance.Unset)]
		public OverrideIconDistance OverrideIconDistance = OverrideIconDistance.Unset;
	}

	public override bool AcceptClientChanges(ModConfig pendingConfig, int whoAmI, ref NetworkText message)
	{
		return DALib.Auth.IsAdmin(whoAmI, ref message);
	}
}

public class CombinedConfig
{
	public MapConfig Map = new();
	public class MapConfig
	{
		public bool Enabled => ClientConfig.Instance.Map.AllEnabled && ServerConfig.Instance.Map.AllowAll;
		public bool BossEnabled => ClientConfig.Instance.Map.BossEnabled && ServerConfig.Instance.Map.AllowBoss;
		public bool BossPieceEnabled => ClientConfig.Instance.Map.BossPieceEnabled && ServerConfig.Instance.Map.AllowBossPiece;
		public bool RareEnabled => ClientConfig.Instance.Map.RareEnabled && ServerConfig.Instance.Map.AllowRare;
		public bool EnemyEnabled => ClientConfig.Instance.Map.EnemyEnabled && ServerConfig.Instance.Map.AllowEnemy;
		public bool WormBodyEnabled => ClientConfig.Instance.Map.WormBodyEnabled && ServerConfig.Instance.Map.AllowWormBody;
		public WormBodyStyle WormBodyStyle => ServerConfig.Instance.Map.OverrideWormBodyStyle == OverrideWormBodyStyle.Unset ? ClientConfig.Instance.Map.WormBodyStyle : (WormBodyStyle)ServerConfig.Instance.Map.OverrideWormBodyStyle - 1;
		public bool BagEnabled => ClientConfig.Instance.Map.BagsEnabled && ServerConfig.Instance.Map.AllowBags;
		public bool TreasureEnabled => ClientConfig.Instance.Map.TreasureEnabled && ServerConfig.Instance.Map.AllowTreasure;
	}

	public ScreenConfig Screen = new();
	public class ScreenConfig
	{
		public bool Enabled => ClientConfig.Instance.Screen.AllEnabled && ServerConfig.Instance.Screen.AllowAll;
		public bool BossEnabled => ClientConfig.Instance.Screen.BossEnabled && ServerConfig.Instance.Screen.AllowBoss;
		public bool BossPieceEnabled => ClientConfig.Instance.Screen.BossPieceEnabled && ServerConfig.Instance.Screen.AllowBossPiece;
		public bool RareEnabled => ClientConfig.Instance.Screen.RareEnabled && ServerConfig.Instance.Screen.AllowRare;
		public bool EnemyEnabled => ClientConfig.Instance.Screen.EnemyEnabled && ServerConfig.Instance.Screen.AllowEnemy;
		public bool WormBodyEnabled => ClientConfig.Instance.Screen.WormBodyEnabled && ServerConfig.Instance.Screen.AllowWormBody;
		public WormBodyStyle WormBodyStyle => ServerConfig.Instance.Screen.OverrideWormBodyStyle == OverrideWormBodyStyle.Unset ? ClientConfig.Instance.Screen.WormBodyStyle : (WormBodyStyle)ServerConfig.Instance.Screen.OverrideWormBodyStyle - 1;
		public bool BagEnabled => ClientConfig.Instance.Screen.BagsEnabled && ServerConfig.Instance.Screen.AllowBags;
		public bool TreasureEnabled => ClientConfig.Instance.Screen.TreasureEnabled && ServerConfig.Instance.Screen.AllowTreasure;
		public IconDistance IconDistance => ServerConfig.Instance.Screen.OverrideIconDistance == OverrideIconDistance.Unset ? ClientConfig.Instance.Screen.IconDistance : (IconDistance)ServerConfig.Instance.Screen.OverrideIconDistance - 1;
	}
}

public static class Config
{
	public static ClientConfig Client => ClientConfig.Instance;
	public static ServerConfig Server => ServerConfig.Instance;
	public static CombinedConfig Combined => new CombinedConfig();

}