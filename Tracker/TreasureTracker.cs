namespace VisualRadar;

	public class TreasureTracker : Tracker {
    public TreasureTracker() {
			tracked = new TrackedTreasure[8];
			for (int i = 0; i < tracked.Length; i++)
				tracked[i] = new TrackedTreasure();
    }

		int current = 0;
		public override void Update(TrackedContext context)
		{
			// 8 trackers instead of one, but only one will be shown.
			// Mainly for animating between different ones when the distance is too great to interpolate movements.
			// 2 are only really needed but I bumped it to 8 in case you move around really fast.
			{
				((TrackedTreasure)tracked[current]).Check(context, ref current);
				if (current < 0 || current >= tracked.Length) current = tracked.Length - 1;
				((TrackedTreasure)tracked[current]).Get(context);
				tracked[current].SetPosition(context);
				for (int i = 0; i < tracked.Length; i++)
				{
					tracked[i].Animate(context);
					tracked[i].SetScale(context);
				}
			}
    }
}