﻿using Sudoku.DocComments;
using DPoint = System.Drawing.Point;
using DPointF = System.Drawing.PointF;

namespace Sudoku.Painting.Extensions
{
	/// <summary>
	/// Provides extension methods on <see cref="DPoint"/> and <see cref="DPointF"/>.
	/// </summary>
	/// <seealso cref="DPoint"/>
	/// <seealso cref="DPointF"/>
	public static class PointEx
	{
		/// <inheritdoc cref="DeconstructMethod"/>
		/// <param name="this">The instance.</param>
		/// <param name="x">The x component.</param>
		/// <param name="y">The y component.</param>
		public static void Deconstruct(this in DPoint @this, out int x, out int y)
		{
			x = @this.X;
			y = @this.Y;
		}

		/// <inheritdoc cref="DeconstructMethod"/>
		/// <param name="this">The instance.</param>
		/// <param name="x">The x component.</param>
		/// <param name="y">The y component.</param>
		public static void Deconstruct(this in DPointF @this, out float x, out float y)
		{
			x = @this.X;
			y = @this.Y;
		}

		/// <summary>
		/// To truncate the point.
		/// </summary>
		/// <param name="this">The point to truncate.</param>
		/// <returns>The result.</returns>
		public static DPoint Truncate(this in DPointF @this) => new((int)@this.X, (int)@this.Y);

		/// <summary>
		/// Get a new <see cref="DPointF"/> instance created by the original one, with the specified offset
		/// added into the properties <see cref="DPointF.X"/> and <see cref="DPointF.Y"/>.
		/// </summary>
		/// <param name="this">The point.</param>
		/// <param name="offset">The offset.</param>
		/// <returns>The result point.</returns>
		/// <seealso cref="DPointF.X"/>
		/// <seealso cref="DPointF.Y"/>
		public static DPointF WithOffset(this in DPointF @this, float offset) =>
			new(@this.X + offset, @this.Y + offset);

		/// <summary>
		/// Get a new <see cref="DPointF"/> instance created by the original one, with the specified offset
		/// added into the properties <see cref="DPointF.X"/> and <see cref="DPointF.Y"/>.
		/// </summary>
		/// <param name="this">The point.</param>
		/// <param name="xOffset">The X offset.</param>
		/// <param name="yOffset">The Y offset.</param>
		/// <returns>The result point.</returns>
		/// <seealso cref="DPointF.X"/>
		/// <seealso cref="DPointF.Y"/>
		public static DPointF WithOffset(this in DPointF @this, float xOffset, float yOffset) =>
			new(@this.X + xOffset, @this.Y + yOffset);
	}
}
