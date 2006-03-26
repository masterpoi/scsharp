
using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace Starcraft {

	public static class SpriteManager {
		static List<Sprite> sprites = new List<Sprite>();
		static Painter painter;

		static MPQ our_mpq;

		public static Sprite CreateSprite (MPQ mpq, int sprite_number)
		{
			our_mpq = mpq;
			return CreateSprite (sprite_number);
		}

		public static Sprite CreateSprite (int sprite_number)
		{
			Sprite sprite = new Sprite (our_mpq, sprite_number);
			sprites.Add (sprite);
			if (painter != null)
				sprite.AddToPainter (painter);

			return sprite;
		}


		public static void RemoveSprite (Sprite sprite)
		{
			if (painter != null)
				sprite.RemoveFromPainter (painter);
			sprites.Remove (sprite);
		}

		static void SpriteManagerTick (Gdk.Pixbuf pb, DateTime now)
		{
			IEnumerator<Sprite> e = sprites.GetEnumerator();
			
			while (e.MoveNext ()) {
				if (e.Current.Tick (now) == false) {
					Console.WriteLine ("removing sprite!!!!");
					RemoveSprite (e.Current);
				}
			}
		}

		public static void AddToPainter (Painter p)
		{
			p.Add (Layer.Background, SpriteManagerTick);

			painter = p;
			foreach (Sprite s in sprites)
				s.AddToPainter (painter);
		}

		public static void RemoveFromPainter (Painter p)
		{
			p.Remove (Layer.Background, SpriteManagerTick);

			foreach (Sprite s in sprites)
				s.RemoveFromPainter (p);
			painter = null;
		}
	}

}
