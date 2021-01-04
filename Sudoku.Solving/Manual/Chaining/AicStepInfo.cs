﻿using System;
using System.Collections.Generic;
using Sudoku.Data;
using Sudoku.Data.Collections;
using Sudoku.Drawing;
using Sudoku.Solving.Extensions;
using Sudoku.Windows;

namespace Sudoku.Solving.Manual.Chaining
{
	/// <summary>
	/// Provides a usage of <b>(grouped) alternating inference chain</b> technique.
	/// </summary>
	/// <param name="Conclusions">All conclusions.</param>
	/// <param name="Views">All views.</param>
	/// <param name="XEnabled">Indicates whether the chain is enabled X strong relations.</param>
	/// <param name="YEnabled">Indicates whether the chain is enabled Y strong relations.</param>
	/// <param name="Target">The target node.</param>
	public sealed record AicStepInfo(
		IReadOnlyList<Conclusion> Conclusions, IReadOnlyList<View> Views, bool XEnabled, bool YEnabled,
		in Node Target)
		: ChainingStepInfo(Conclusions, Views, XEnabled, YEnabled, default, default, default, default)
	{
		/// <inheritdoc/>
		public override decimal Difficulty =>
			TechniqueCode switch
			{
				TechniqueCode.MWing => 4.5M,
				TechniqueCode.SplitWing or TechniqueCode.HybridWing or TechniqueCode.LocalWing => 4.8M,
				_ => (XEnabled && YEnabled ? 5.0M : 4.6M) + (FlatComplexity - 2).GetExtraDifficultyByLength()
			};

#if DOUBLE_LAYERED_ASSUMPTION
		/// <inheritdoc/>
		public override Node[] ChainsTargets => new[] { Target };
#endif

		/// <inheritdoc/>
		public override DifficultyLevel DifficultyLevel =>
			TechniqueCode is TechniqueCode.MWing or TechniqueCode.SplitWing
			or TechniqueCode.HybridWing or TechniqueCode.LocalWing
			? DifficultyLevel.Hard
			: DifficultyLevel.Fiendish;

		/// <inheritdoc/>
		public override ChainingTypeCode SortKey => Enum.Parse<ChainingTypeCode>(TechniqueCode.ToString());

		/// <inheritdoc/>
		public override int FlatComplexity => Target.AncestorsCount;

		/// <inheritdoc/>
		public override string Name => Resources.GetValue(TechniqueCode.ToString());

		/// <inheritdoc/>
		public override TechniqueCode TechniqueCode =>
			IsXChain
			? TechniqueCode.XChain
			: IsMWing
			? TechniqueCode.MWing
			: IsSplitWing
			? TechniqueCode.SplitWing
			: IsHybridWing
			? TechniqueCode.HybridWing
			: IsLocalWing
			? TechniqueCode.LocalWing
			: Target.Chain is var chain && chain[^2].Digit == chain[1].Digit
			? IsXyChain ? TechniqueCode.XyChain : TechniqueCode.Aic
			: Conclusions.Count switch
			{
				1 => TechniqueCode.DiscontinuousNiceLoop,
				2 => TechniqueCode.XyXChain,
				_ => TechniqueCode.Aic
			};

		/// <summary>
		/// Indicates whether the specified chain is an X-Chain.
		/// </summary>
		private bool IsXChain => XEnabled && !YEnabled;

		/// <summary>
		/// Indicates whether the chain is M-Wing (<c>(x = y) - y = (y - x) = x</c>).
		/// </summary>
		/// <returns>A <see cref="bool"/> value indicating that.</returns>
		private bool IsMWing
		{
			get
			{
				if (FlatComplexity != 8)
				{
					return false;
				}

				var chain = Target.Chain;
				var (a, _) = chain[1];
				var (b, _) = chain[2];
				var (c, _) = chain[3];
				var (d, _) = chain[4];
				var (e, _) = chain[5];
				var (f, _) = chain[6];

				return a / 9 == b / 9 && d / 9 == e / 9
					&& b % 9 == c % 9 && c % 9 == d % 9
					&& a % 9 == e % 9 && e % 9 == f % 9
					|| f / 9 == e / 9 && c / 9 == b / 9 // Reverse case.
					&& d % 9 == e % 9 && c % 9 == d % 9
					&& b % 9 == f % 9 && a % 9 == b % 9;
			}
		}

		/// <summary>
		/// Indicates whether the chain is Split-Wing (<c>x = x - (x = y) - y = y</c>).
		/// </summary>
		/// <returns>A <see cref="bool"/> value indicating that.</returns>
		private bool IsSplitWing
		{
			get
			{
				if (FlatComplexity != 8)
				{
					return false;
				}

				var chain = Target.Chain;
				var (a, _) = chain[1];
				var (b, _) = chain[2];
				var (c, _) = chain[3];
				var (d, _) = chain[4];
				var (e, _) = chain[5];
				var (f, _) = chain[6];

				return a % 9 == b % 9 && b % 9 == c % 9 // First three nodes hold a same digit.
					&& d % 9 == e % 9 && e % 9 == f % 9 // Last three nodes hold a same digit.
					&& c / 9 == d / 9; // In same cell.
			}
		}

		/// <summary>
		/// Indicates whether the chain is Hybrid-Wing.
		/// This wing has two types:
		/// <list type="bullet">
		/// <item><c>(x = y) - y = (y - z) = z</c></item>
		/// <item><c>(x = y) - (y = z) - z = z</c></item>
		/// </list>
		/// </summary>
		/// <returns>A <see cref="bool"/> value indicating that.</returns>
		private bool IsHybridWing
		{
			get
			{
				if (FlatComplexity != 8)
				{
					return false;
				}

				var chain = Target.Chain;
				var (a, _) = chain[1];
				var (b, _) = chain[2];
				var (c, _) = chain[3];
				var (d, _) = chain[4];
				var (e, _) = chain[5];
				var (f, _) = chain[6];

				return a / 9 == b / 9 && d / 9 == e / 9
					&& b % 9 == c % 9 && c % 9 == d % 9
					&& e % 9 == f % 9
					|| e / 9 == f / 9 && b / 9 == c / 9
					&& d % 9 == e % 9 && c % 9 == d % 9
					&& a % 9 == b % 9
					|| a / 9 == b / 9 && c / 9 == d / 9 // Reverse case.
					&& b % 9 == c % 9
					&& d % 9 == e % 9 && e % 9 == f % 9
					|| e / 9 == f / 9 && c / 9 == d / 9
					&& d % 9 == e % 9
					&& b % 9 == c % 9 && a % 9 == b % 9;
			}
		}

		/// <summary>
		/// Indicates whether the chain is Local-Wing (<c>x = (x - z) = (z - y) = y</c>).
		/// </summary>
		/// <returns>A <see cref="bool"/> value indicating that.</returns>
		private bool IsLocalWing
		{
			get
			{
				if (FlatComplexity != 8)
				{
					return false;
				}

				var chain = Target.Chain;
				var (a, _) = chain[1];
				var (b, _) = chain[2];
				var (c, _) = chain[3];
				var (d, _) = chain[4];
				var (e, _) = chain[5];
				var (f, _) = chain[6];

				return b / 9 == c / 9 && d / 9 == e / 9
					&& a % 9 == b % 9 && c % 9 == d % 9 && e % 9 == f % 9;
			}
		}


		/// <inheritdoc/>
		public override string ToString()
		{
			string chainStr = new LinkCollection(Views[0].Links!).ToString();
			string elimStr = new ConclusionCollection(Conclusions).ToString();
			return $"{Name}: {chainStr} => {elimStr}";
		}
	}
}
