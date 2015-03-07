﻿using System.Diagnostics;
using GameTimer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FontBuddyLib
{
	//how to justify the font writing
	public enum Justify
	{
		Left,
		Right,
		Center
	}

	/// <summary>
	/// This is just a simple class for drawing text on the screen.
	/// Because that is just way harder in XNA than it needs to be!
	/// </summary>
	public class FontBuddy : IFontBuddy
	{
		#region Properties

		/// <summary>
		/// The font this dude is "helping" with
		/// </summary>
		public virtual SpriteFont Font { get; set; }

		#endregion //Properties

		#region Methods

		/// <summary>
		/// constructor... you need to set the font or call load content if you use this one
		/// </summary>
		public FontBuddy()
		{
		}

		/// <summary>
		/// given a content manager and a resource name, load the resource as a bitmap font
		/// </summary>
		/// <param name="rContent"></param>
		/// <param name="strResource"></param>
		public void LoadContent(ContentManager rContent, string strResource)
		{
			//load font
			if (null == Font)
			{
				Font = rContent.Load<SpriteFont>(strResource);
			}
		}

		public virtual float Write(string text,
			Vector2 position,
			Justify justification,
			float scale,
			Color color,
			SpriteBatch spriteBatch,
			GameClock time)
		{
			return DrawText(text, position, justification, scale, color, spriteBatch, time);
		}

		public virtual float Write(string text,
			Point position,
			Justify justification,
			float scale,
			Color color,
			SpriteBatch spriteBatch,
			GameClock time)
		{
			return Write(text, new Vector2(position.X, position.Y), justification, scale, color, spriteBatch, time);
		}

		/// <summary>
		/// write something on the screen
		/// </summary>
		/// <param name="text">the text to write on the screen</param>
		/// <param name="position">where to write at... either upper left, upper center, or upper right, depending on justication</param>
		/// <param name="justification">how to justify the text</param>
		/// <param name="scale">how big to write.  This is not a point size to draw at, it is a multiple of the default font size!</param>
		/// <param name="color">the color to draw the text</param>
		/// <param name="spriteBatch">spritebatch to use to render the text</param>
		/// <param name="time">Most of the other font buddy classes use time somehow, but can jsut send in 0.0f for this dude or ignoer it</param>
		protected float DrawText(string text,
			Vector2 position,
			Justify justification,
			float scale,
			Color color,
			SpriteBatch spriteBatch,
			GameClock time)
		{
			Debug.Assert(null != Font);

			//if this thing is empty, dont do anything
			if (string.IsNullOrEmpty(text))
			{
				return position.X;
			}

			position = JustifiedPosition(text, position, justification, scale);

			//okay, draw the actual string
			spriteBatch.DrawString(Font,
									 text,
									 position,
									 color,
									 0.0f,
									 Vector2.Zero,
									 scale,
									 SpriteEffects.None,
									 0);

			//return the end of that string
			return position.X + (Font.MeasureString(text).X * scale);
		}

		protected Vector2 JustifiedPosition(string strText, Vector2 position, Justify eJustification, float fScale)
		{
			//Get the correct location
			Vector2 textSize = (!string.IsNullOrEmpty(strText) ? (Font.MeasureString(strText) * fScale) : Vector2.Zero);

			switch (eJustification)
			{
				//left = use teh x value (no cahnge)

				case Justify.Right:
				{
					//move teh x value
					position.X -= textSize.X;
				}
				break;

				case Justify.Center:
				{
					//move teh x value
					position.X -= (textSize.X / 2.0f);
				}
				break;
			}

			return position;
		}

		#endregion //Methods
	}
}