using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using Terraria.Localization;
using Terraria.ModLoader.Config;
using Terraria.ModLoader.Config.UI;

namespace VisualRadar;

[Flags]
public enum ConfigEnableType : byte
{
	None,
	Map,
	Screen,
	Both,
}

[Flags]
public enum ConfigEnableTypeOverride : byte
{
	None,
	Map,
	Screen,
	Both,

	Unset = 0b10000000,
}

public enum ConfigWormBodyStyle : byte
{
	WholeBody,
	Closest,
	ClosestAndHead,
}
public enum ConfigWormBodyStyleOverride : byte
{
	WholeBody,
	Closest,
	ClosestAndHead,

	Unset = 0b10000000,
}

public enum ConfigIconType : byte
{
	Floating,
	Clamped,
}

public enum ConfigIconDistanceTypeOverride : byte
{
	Floating,
	Clamped,

	Unset = 0b10000000,
}

public enum ConfigIconZoomAttach : byte
{
	None,
	UIScale,
	GameZoom,
}

public class ClientConfig : ModConfig
{
	public override ConfigScope Mode => ConfigScope.ClientSide;
	public static ClientConfig Instance;

	[DefaultValue(ConfigEnableType.Both)]
	public ConfigEnableType EnabledFor = ConfigEnableType.Both;

	[DefaultValue(ConfigEnableType.Both)]
	public ConfigEnableType BossIcons = ConfigEnableType.Both;

	[DefaultValue(ConfigEnableType.Map)]
	public ConfigEnableType BossPieceIcons = ConfigEnableType.Map;

	[DefaultValue(ConfigEnableType.Both)]
	public ConfigEnableType RareIcons = ConfigEnableType.Both;

	[DefaultValue(ConfigEnableType.Map)]
	public ConfigEnableType EnemyIcons = ConfigEnableType.Map;

	[DefaultValue(ConfigEnableType.Map)]
	public ConfigEnableType WormBodyIcons = ConfigEnableType.Map;

	public WormBodyStyleConfig WormBodyStyles = new();
	public class WormBodyStyleConfig
	{
		[DefaultValue(ConfigWormBodyStyle.WholeBody)]
		public ConfigWormBodyStyle Map = ConfigWormBodyStyle.WholeBody;

		[DefaultValue(ConfigWormBodyStyle.Closest)]
		public ConfigWormBodyStyle Screen = ConfigWormBodyStyle.Closest;
	}

	[DefaultValue(ConfigEnableType.Both)]
	public ConfigEnableType BagIcons = ConfigEnableType.Both;

	[DefaultValue(ConfigEnableType.Both)]
	public ConfigEnableType TreasureIcons = ConfigEnableType.Both;

	public ScreenIconConfig ScreenIcons = new();
	public class ScreenIconConfig
	{
		[DefaultValue(ConfigIconType.Clamped)]
		public ConfigIconType Type = ConfigIconType.Clamped;

		public IconDistanceConfig Distances = new();
		public class IconDistanceConfig
		{
			[Range(10f, 100f)]
			[Increment(0.5f)]
			[Slider]
			[DefaultValue(50f)]
			public float Factor = 50f;

			public ClampedIconDistanceConfig Clamped = new();
			public class ClampedIconDistanceConfig {
				[Range(0.1f, 3f)]
				[Increment(0.05f)]
				[Slider]
				[DefaultValue(1f)]
				public float Multiplier = 1f;

				[Range(0, 64)]
				[Increment(1)]
				[Slider]
				[DefaultValue(0)]
				public int Offset = 0;
			}
		}

		[Range(0f, 100f)]
		[Increment(0.5f)]
		[Slider]
		[DefaultValue(75f)]
		public float Scale = 75f;

		[DefaultValue(ConfigIconZoomAttach.UIScale)]
		public ConfigIconZoomAttach AttachScaleTo = ConfigIconZoomAttach.UIScale;

		[DefaultValue(true)]
		public bool DrawBorder = true;

		[Range(0, 100f)]
		[Increment(0.5f)]
		[Slider]
		public float Alpha => 100f;
	}

}

public class ServerConfig : ModConfig
{
	public override ConfigScope Mode => ConfigScope.ServerSide;
	public static ServerConfig Instance;
	[Header("ServerOverrides")]

	[DefaultValue(ConfigEnableTypeOverride.Unset)]
	public ConfigEnableTypeOverride EnabledFor = ConfigEnableTypeOverride.Unset;

	[DefaultValue(ConfigEnableTypeOverride.Unset)]
	public ConfigEnableTypeOverride BossIcons = ConfigEnableTypeOverride.Unset;

	[DefaultValue(ConfigEnableTypeOverride.Unset)]
	public ConfigEnableTypeOverride BossPieceIcons = ConfigEnableTypeOverride.Unset;

	[DefaultValue(ConfigEnableTypeOverride.Unset)]
	public ConfigEnableTypeOverride RareIcons = ConfigEnableTypeOverride.Unset;

	[DefaultValue(ConfigEnableTypeOverride.Unset)]
	public ConfigEnableTypeOverride EnemyIcons = ConfigEnableTypeOverride.Unset;

	[DefaultValue(ConfigEnableTypeOverride.Unset)]
	public ConfigEnableTypeOverride WormBodyIcons = ConfigEnableTypeOverride.Unset;

	public WormBodyStyleConfig WormBodyStyles = new();
	public class WormBodyStyleConfig
	{
		[DefaultValue(ConfigWormBodyStyleOverride.WholeBody)]
		public ConfigWormBodyStyleOverride Map = ConfigWormBodyStyleOverride.Unset;

		[DefaultValue(ConfigWormBodyStyleOverride.Closest)]
		public ConfigWormBodyStyleOverride Screen = ConfigWormBodyStyleOverride.Unset;
	}

	[DefaultValue(ConfigEnableTypeOverride.Unset)]
	public ConfigEnableTypeOverride BagIcons = ConfigEnableTypeOverride.Unset;

	[DefaultValue(ConfigEnableTypeOverride.Unset)]
	public ConfigEnableTypeOverride TreasureIcons = ConfigEnableTypeOverride.Unset;

	public ScreenIconConfig ScreenIcons = new();
	public class ScreenIconConfig
	{
		[DefaultValue(ConfigIconDistanceTypeOverride.Unset)]
		public ConfigIconDistanceTypeOverride IconDistance = ConfigIconDistanceTypeOverride.Unset;
	}

	public override bool AcceptClientChanges(ModConfig pendingConfig, int whoAmI, ref NetworkText message)
	{
		return DALib.Auth.IsAdmin(whoAmI, ref message);
	}
}

public static class Config
{
	public static ClientConfig Client => ClientConfig.Instance;
	public static ServerConfig Server => ServerConfig.Instance;

	public static ConfigEnableType EnabledFor => ServerConfig.Instance.EnabledFor == ConfigEnableTypeOverride.Unset ? ClientConfig.Instance.EnabledFor : (ConfigEnableType)ServerConfig.Instance.EnabledFor;
	public static ConfigEnableType BossIcons => ServerConfig.Instance.BossIcons == ConfigEnableTypeOverride.Unset ? ClientConfig.Instance.BossIcons : (ConfigEnableType)ServerConfig.Instance.BossIcons;
	public static ConfigEnableType BossPieceIcons => ServerConfig.Instance.BossPieceIcons == ConfigEnableTypeOverride.Unset ? ClientConfig.Instance.BossPieceIcons : (ConfigEnableType)ServerConfig.Instance.BossPieceIcons;
	public static ConfigEnableType RareIcons => ServerConfig.Instance.RareIcons == ConfigEnableTypeOverride.Unset ? ClientConfig.Instance.RareIcons : (ConfigEnableType)ServerConfig.Instance.RareIcons;
	public static ConfigEnableType EnemyIcons => ServerConfig.Instance.EnemyIcons == ConfigEnableTypeOverride.Unset ? ClientConfig.Instance.EnemyIcons : (ConfigEnableType)ServerConfig.Instance.EnemyIcons;
	public static ConfigEnableType WormBodyIcons => ServerConfig.Instance.WormBodyIcons == ConfigEnableTypeOverride.Unset ? ClientConfig.Instance.WormBodyIcons : (ConfigEnableType)ServerConfig.Instance.WormBodyIcons;
	public static ConfigEnableType BagIcons => ServerConfig.Instance.BagIcons == ConfigEnableTypeOverride.Unset ? ClientConfig.Instance.BagIcons : (ConfigEnableType)ServerConfig.Instance.BagIcons;
	public static ConfigEnableType TreasureIcons => ServerConfig.Instance.TreasureIcons == ConfigEnableTypeOverride.Unset ? ClientConfig.Instance.TreasureIcons : (ConfigEnableType)ServerConfig.Instance.TreasureIcons;

	public static class WormBodyStyles
	{
		public static ConfigWormBodyStyle Map => ServerConfig.Instance.WormBodyStyles.Map == ConfigWormBodyStyleOverride.Unset ? ClientConfig.Instance.WormBodyStyles.Map : (ConfigWormBodyStyle)ServerConfig.Instance.WormBodyStyles.Map;

		public static ConfigWormBodyStyle Screen => ServerConfig.Instance.WormBodyStyles.Screen == ConfigWormBodyStyleOverride.Unset ? ClientConfig.Instance.WormBodyStyles.Screen : (ConfigWormBodyStyle)ServerConfig.Instance.WormBodyStyles.Screen;
	}

	public static class ScreenIcons
	{
		public static ConfigIconType Type => ServerConfig.Instance.ScreenIcons.IconDistance == ConfigIconDistanceTypeOverride.Unset ? ClientConfig.Instance.ScreenIcons.Type : (ConfigIconType)ServerConfig.Instance.ScreenIcons.IconDistance;
	}
}