using Terraria;
using Terraria.GameContent;
using Terraria.ID;

namespace VisualRadar;

public class TrackedBag : TrackedObject
{
	public void Get(TrackedContext context, Item item)
	{
		scale = 1f;
		active = false;
		if (!item.active || item.IsAir) return;
		if ((context == TrackedContext.Map && (Config.BagIcons & ConfigEnableType.Map) > 0) || (context == TrackedContext.Screen && (Config.BagIcons & ConfigEnableType.Screen) > 0))
		{
			if (ItemID.Sets.BossBag[item.type])
			{
				iconText = item.AffixName();
				icon = TextureAssets.Item[item.type].Value;
				position = item.Center;
				fixedDistance = 256;
				active = true;
			}
		}
	}
}