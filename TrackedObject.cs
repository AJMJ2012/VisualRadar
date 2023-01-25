using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;

namespace VisualRadar
{
    public class TrackedObject {
		public Texture2D icon;
		public float scale;
        public bool lerpScale = true;
		public float iconScale;
		public Vector2 position;
        public bool lerpPosition = false;
        public Vector2 iconPosition;
        public string iconText;
		public int fixedDistance = 0;
        public bool drawArrow = true;
        public bool linearLerp = false;

		public static Player player => Main.LocalPlayer;
		public const float lerpStep = 32f;
		public const int lerpMaxDistance = 4000;
		public void SetScale(string context) {
			if (context == "screen") scale *= Config.Client.Screen.IconScale / 100f;
            if (!lerpScale) {
                iconScale = scale;
                return;
            }
			if (iconScale < scale) {
				iconScale += (1f / lerpStep);
			}
			else if (iconScale > scale) {
				iconScale -= (1f / lerpStep);
			}
			if (iconScale > scale - (1f / lerpStep) && iconScale < scale + (1f / lerpStep)) {
				iconScale = scale;
			}
		}

		public void SetPosition(string context) {
            if (!lerpPosition) {
                iconPosition = position;
                return;
            }
			float distance = Vector2.Distance(position, iconPosition);
			if (distance > lerpMaxDistance || iconPosition == Vector2.Zero) {
				iconPosition = position;
				scale = 0;
				distance = 0;
			}
			if (iconPosition != position) {
				float relativeAngle = (float)Math.Atan2(iconPosition.Y - position.Y, iconPosition.X - position.X);
				Vector2 lerpVector = new(
					(float)Math.Cos(relativeAngle),
					(float)Math.Sin(relativeAngle)
				);
				Vector2 newPosition = iconPosition - lerpVector * (distance / lerpStep);
				Vector2 newPositionLinear = iconPosition - (lerpVector * lerpStep * 2f);
				if (linearLerp && Vector2.Distance(newPositionLinear, position) > lerpStep * 2f) {
					iconPosition = newPositionLinear;
				}
				else {
					iconPosition = newPosition;
				}
			}
		}
	}
}