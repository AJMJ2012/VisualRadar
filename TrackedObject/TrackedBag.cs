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
		if ((context == TrackedContext.Map && Config.Combined.Map.BagEnabled) || (context == TrackedContext.Screen && Config.Combined.Screen.BagEnabled))
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