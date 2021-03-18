using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Shellblade
{
	public class DebugWindow
	{
		private const float RefreshRate = 1f;
		private const float Margin      = 0.01f;

		private readonly Font         _font = new Font(@"assets/CONSOLA.TTF");
		private readonly RenderWindow _window;
		private readonly Text         _fpsText;
		private readonly Text         _refreshText;
		private readonly Clock        _clock = new Clock();

		private float _deltaTime     = 0f;
		private float _deltaCounter  = 0f;
		private float _averageDelta  = 0f;
		private int   _frameCounter  = 0;
		private int   _framesLastSec = 0;

		public DebugWindow()
		{
			_window = new RenderWindow(new VideoMode(400, 300), "Shellblade Debug", Styles.Close | Styles.Titlebar);

			_window.Closed += (sender, args) => _window.Close();

			_refreshText          = new Text($"Refresh rate: {RefreshRate}s", _font, 12);
			_refreshText.Position = new Vector2f(_window.Size.X * (1 - Margin) - _refreshText.GetGlobalBounds().Width, _window.Size.Y * Margin);

			_fpsText = new Text("FPS: --.-- [--]", _font, 12)
			{
				Position = new Vector2f(_window.Size.X * Margin, _window.Size.Y * Margin),
			};
		}

		public void Tick(Time dt, int objects, int uiObjects)
		{
			if (!_window.IsOpen) return;

			if (_clock.ElapsedTime.AsSeconds() >= RefreshRate)
			{
				_averageDelta = _deltaCounter / _frameCounter;
				_deltaCounter = 0f;

				_framesLastSec = _frameCounter;
				_frameCounter  = 0;

				_clock.Restart();
			}

			_deltaTime    =  dt.AsSeconds();
			_deltaCounter += _deltaTime;
			_frameCounter++;

			_fpsText.DisplayedString = $"FPS: {1f / _averageDelta:F2} [{_framesLastSec:D2}]\n" +
			                           $"Avg. Δ: {_averageDelta * 1000f:F2}ms\n" +
			                           $"Objects: {objects}\n" +
			                           $"UI Objs: {uiObjects}";

			_window.DispatchEvents();

			_window.Clear(Color.Black);
			_window.Draw(_fpsText);
			_window.Draw(_refreshText);
			_window.Display();
		}

		public void Stop()
		{
			if (_window.IsOpen)
				_window.Close();
		}
	}
}
