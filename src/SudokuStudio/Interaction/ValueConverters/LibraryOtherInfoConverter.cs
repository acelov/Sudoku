namespace SudokuStudio.Interaction.ValueConverters;

/// <summary>
/// Represents a value converter type that can convert multiple values in <see cref="LibraryBindableSource"/>
/// into a <see cref="string"/>.
/// </summary>
/// <seealso cref="LibraryBindableSource"/>
public sealed class LibraryOtherInfoConverter : IValueConverter
{
	/// <inheritdoc/>
	public object Convert(object value, Type targetType, object parameter, string language)
		=> value switch
		{
			LibraryBindableSource { Tags: var tags, LibraryInfo.LastModifiedTime: var time }
				=> $"{tags switch
				{
					not { Length: not 0 } => string.Join(ResourceDictionary.Get("_Token_Comma"), tags),
					_ => ResourceDictionary.Get("NoTags")
				}} | {time.ToString(App.CurrentCulture)}",
			_ => throw new InvalidOperationException("Converter error - invalid library bindable source.")
		};

	/// <inheritdoc/>
	[DoesNotReturn]
	public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
}
