namespace SudokuStudio.BindableSource;

/// <summary>
/// Represents a library bindable source. This type is different with <see cref="LibrarySimpleBindableSource"/>
/// - this type contains more properties that can be bound with UI.
/// </summary>
/// <seealso cref="LibrarySimpleBindableSource"/>
[DependencyProperty<bool>("IsActive", DocSummary = "Indicates whether the current library is loading, updating, etc..")]
[DependencyProperty<string>("Name", DocSummary = "Indicates the name of the library.")]
[DependencyProperty<string>("FileId", DocSummary = "Indicates the unique file name of the library.")]
[DependencyProperty<string>("Author", DocSummary = "Indicates the author of the library.")]
[DependencyProperty<string>("Description", DocSummary = "Indicates the description to the library.")]
[DependencyProperty<string[]>("Tags", DocSummary = "Indicates the tags of the library.")]
public sealed partial class LibraryBindableSource : DependencyObject
{
	[Default]
	internal static readonly string NameDefaultValue = SR.Get("NoName", App.CurrentCulture);

	[Default]
	internal static readonly string AuthorDefaultValue = SR.Get("Anonymous", App.CurrentCulture);

	[Default]
	internal static readonly string DescriptionDefaultValue = SR.Get("NoDescription", App.CurrentCulture);


	/// <summary>
	/// Indicates the corresponding <see cref="Library"/> instance.
	/// </summary>
	public LibraryInfo Library => new(CommonPaths.Library, FileId);


	/// <summary>
	/// Try to create <see cref="LibraryBindableSource"/> instances from local path.
	/// </summary>
	/// <returns>A list of <see cref="LibraryBindableSource"/> instances.</returns>
	internal static ObservableCollection<LibraryBindableSource> GetLibrariesFromLocal()
	{
		if (!Directory.Exists(CommonPaths.Library))
		{
			// Implicitly create directory if the library directory does not exist.
			Directory.CreateDirectory(CommonPaths.Library);
			return [];
		}

		return [
			..
			from file in Directory.EnumerateFiles(CommonPaths.Library)
			let extension = io::Path.GetExtension(file)
			where extension == FileExtensions.PuzzleLibrary
			let fileId = io::Path.GetFileNameWithoutExtension(file)
			let library = new LibraryInfo(CommonPaths.Library, fileId)
			where library.IsInitialized
			let firstContentLine = File.ReadLines(library.ConfigFilePath).FirstOrDefault()
			where firstContentLine == LibraryInfo.ConfigFileHeader
			select new LibraryBindableSource
			{
				Name = library.Name ?? NameDefaultValue,
				Author = library.Author ?? AuthorDefaultValue,
				Description = library.Description ?? DescriptionDefaultValue,
				Tags = library.Tags ?? [],
				FileId = fileId,
				IsActive = false
			}
		];
	}
}
