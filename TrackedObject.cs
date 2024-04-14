using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;

namespace VisualRadar
{

	public enum TrackedContext : byte
	{
		None = 0,
		Screen = 1,
		Map = 2,
	}

	public enum LerpMode : byte
	{
		None = 0,
		Smooth = 1,
		Fast = 2,
		Linear = 3,
	}

	public class TrackedObject
	{
		public static Player player => Main.LocalPlayer;

		public bool active;
		public bool animate = true;
		public TrackedContext context = TrackedContext.Screen | TrackedContext.Map;
		internal bool enabled;
		public bool drawArrow = true;
		public int fixedDistance;
		public Texture2D icon;
		public float iconAnimation;
		public float iconSmoothAnimation;
		public float iconFixedDistance;
		public Vector2 iconPosition;
		public float iconScale;
		public string iconText;
		public Vector2 lastPosition;
		public LerpMode lerpMode = LerpMode.Smooth;
		public bool lerpPosition = true;
		public float lerpStep = 30f;
		public float linearLerpStep = 30f;
		public Vector2 position;
		public float scale;

		public void Animate(TrackedContext context)
		{
			int amount = active.ToInt();
			if (!animate)
			{
				iconAnimation = amount;
				iconSmoothAnimation = amount;
			}
			else
			{
				if (Math.Sign(amount - iconAnimation) != 0)
				{
					float lerp = (1f / lerpStep);
					iconAnimation = MathHelper.Clamp(iconAnimation + lerp * Math.Sign(amount - iconAnimation), 0f, 1f);
				}
				if (Math.Sign(amount - iconSmoothAnimation) != 0)
				{
					float lerp = (Math.Abs(amount - iconSmoothAnimation) / lerpStep);
					iconSmoothAnimation = MathHelper.Clamp(iconSmoothAnimation + lerp * Math.Sign(amount - iconSmoothAnimation), 0f, 1f);
				}
			}
			enabled = iconSmoothAnimation > (1f / lerpStep / lerpStep) || iconAnimation > 0;
		}

		public void SetScale(TrackedContext context)
		{
			float amount = scale;
			if (context == TrackedContext.Screen)
			{
				amount *= Config.Client.Screen.IconScale / 100f;
			}
			if (!animate)
			{
				iconScale = amount;
			}
			else if (Math.Sign(amount - iconScale) != 0)
			{
				float lerp = (1f / lerpStep);
				iconScale = MathHelper.Clamp(iconScale + lerp * Math.Sign(amount - iconScale), 0f, 1f);
			}
		}

		public bool succ = true;
		public void SetPosition(TrackedContext context)
		{
			if (!lerpPosition || !animate)
			{
				iconPosition = position;
				iconFixedDistance = fixedDistance;
			}
			else if (!enabled)
			{
				position = Vector2.Zero;
				lastPosition = Vector2.Zero;
				iconPosition = Vector2.Zero;
				fixedDistance = 0;
				iconFixedDistance = 0;
			}
			else
			{
				{
					if (iconFixedDistance == 0)
					{
						iconFixedDistance = fixedDistance;
					}
					float distance = Math.Abs(fixedDistance - iconFixedDistance);
					if (distance > 0)
					{
						float lerp = lerpMode switch
						{
							LerpMode.None => distance / 2f,
							LerpMode.Smooth => distance / lerpStep,
							LerpMode.Fast => distance / (float)Math.Sqrt(distance),
							LerpMode.Linear => Math.Min(distance, linearLerpStep / 16f),
							_ => throw new NotImplementedException(),
						};
						lerp = (distance / lerpStep);

						if (Math.Sign(fixedDistance - iconFixedDistance) != 0)
						{
							iconFixedDistance = iconFixedDistance + lerp * Math.Sign(fixedDistance - iconFixedDistance);
						}
					}
				}
				{
					if (iconPosition == Vector2.Zero)
					{
						iconPosition = position;
					}
					float distance = Vector2.Distance(iconPosition, position);
					if (distance > 0)
					{
						float relativeAngle = (float)Math.Atan2(iconPosition.Y - position.Y, iconPosition.X - position.X);
						Vector2 lerpVector = new(
							(float)Math.Cos(relativeAngle),
							(float)Math.Sin(relativeAngle)
						);
						float lerp = lerpMode switch
						{
							LerpMode.None => distance / 2f,
							LerpMode.Smooth => distance / lerpStep,
							LerpMode.Fast => distance / (float)Math.Sqrt(distance),
							LerpMode.Linear => Math.Min(distance, linearLerpStep),
							_ => throw new NotImplementedException(),
						};
						Vector2 newPosition = iconPosition - (lerpVector * lerp);
						float lerpDistance = Vector2.Distance(newPosition, position);
						if (lerpDistance > lerp)
						{
							iconPosition = newPosition;
						}
						else
						{
							iconPosition = position;
						}
					}
				}
			}
			lastPosition = position;
		}
	}
}