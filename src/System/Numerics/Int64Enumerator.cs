namespace System.Numerics;

/// <summary>
/// Represents an enumerator that iterates a <see cref="long"/> or <see cref="ulong"/> value.
/// </summary>
/// <param name="value">The value to be iterated.</param>
[StructLayout(LayoutKind.Auto)]
public ref partial struct Int64Enumerator([RecordParameter(DataMemberKinds.Field, IsImplicitlyReadOnly = false)] ulong value)
{
	/// <inheritdoc cref="IEnumerator{T}.Current"/>
	public int Current { get; private set; } = -1;


	/// <inheritdoc cref="IEnumerator.MoveNext"/>
	public bool MoveNext()
	{
		while (++Current < 64)
		{
			if ((_value >> Current & 1) != 0)
			{
				return true;
			}
		}

		return false;
	}
}
