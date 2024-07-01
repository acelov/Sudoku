namespace Sudoku.Analytics.Categorization;

public partial class TechniqueSet
{
	/// <summary>
	/// The enumerator that can iterate with <see cref="Technique"/> fields for a <see cref="TechniqueSet"/> instance.
	/// </summary>
	/// <param name="bits">The internal bits.</param>
	public ref struct Enumerator(BitArray bits) : IEnumerator<Technique>
	{
		/// <summary>
		/// The current index.
		/// </summary>
		private int _currentIndex = -1;


		/// <inheritdoc cref="IEnumerator{T}.Current"/>
		public readonly Technique Current => TechniqueProjectionBack(_currentIndex);

		/// <inheritdoc/>
		readonly object IEnumerator.Current => Current;


		/// <inheritdoc cref="IEnumerator.MoveNext"/>
		public bool MoveNext()
		{
			for (_currentIndex++; _currentIndex < bits.Length; _currentIndex++)
			{
				if (bits[_currentIndex])
				{
					return true;
				}
			}
			return false;
		}

		/// <inheritdoc/>
		readonly void IDisposable.Dispose() { }

		/// <inheritdoc/>
		[DoesNotReturn]
		readonly void IEnumerator.Reset() => throw new NotImplementedException();
	}
}
