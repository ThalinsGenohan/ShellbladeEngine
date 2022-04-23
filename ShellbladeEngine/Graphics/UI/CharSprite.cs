using System;
using SFML.Graphics;
using SFML.System;

namespace Shellblade.Graphics.UI;

public class CharSprite : Sprite
{
	public CharSprite(Texture texture, IntRect rect) : base(texture, rect) { }

	private const float SineA = 10f;
	private const float SineB = 1f;
	private const float TwoPi = 2f * MathF.PI;

	public bool IsShaky { get; set; } = false;
	public bool IsWavy  { get; set; } = false;
	public uint Delay   { get; set; } = 0;

	public new void Draw(RenderTarget target, RenderStates states)
	{
		var pos = new Vector2f(0f, 0f);
		if (IsShaky)
		{
			pos.X += Random.Shared.NextSingle();
			pos.Y += Random.Shared.NextSingle();
		}

		if (IsWavy)
		{
			pos.Y += MathF.Sin(Position.X / SineA + TwoPi * Game.Timer.ElapsedTime.AsSeconds() / SineB);
		}

		Origin = pos;
		base.Draw(target, states);
	}
}