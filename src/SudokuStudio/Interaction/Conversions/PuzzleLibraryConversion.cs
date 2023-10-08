using Microsoft.UI.Xaml;
using SudokuStudio.BindableSource;
using static SudokuStudio.Strings.StringsAccessor;

namespace SudokuStudio.Interaction.Conversions;

internal static class PuzzleLibraryConversion
{
	public static int GetModeRawValue(LibraryDataUpdatingMode mode) => (int)mode;

	public static int GetTotalPagesCount(PuzzleLibraryBindableSource? source) => source?.Puzzles.Length ?? -1;

	public static string GetTotalPagesCountText(PuzzleLibraryBindableSource? source) => $"/ {GetTotalPagesCount(source)}";

	public static string GetLoadingOrAddingDialogTitle(LibraryDataUpdatingMode mode)
		=> GetString(
			mode switch
			{
				LibraryDataUpdatingMode.Add => "LibraryPage_AddLibraryTitle",
				LibraryDataUpdatingMode.Load => "LibraryPage_LoadLibraryTitle",
				LibraryDataUpdatingMode.Update => "LibraryPage_UpdateLibraryTitle"
			}
		);

	public static string GetTags(string[] tags) => string.Format(GetString("LibraryPage_TagsAre"), string.Join(GetString("_Token_Comma"), tags));

	public static string GetPuzzlesCountText(int count)
		=> string.Format(GetString(count == 1 ? "LibraryPage_PuzzlesCountIsSingular" : "LibraryPage_PuzzlesCountIsPlural"), count);

	public static Visibility GetPagingControlsVisibility(PuzzleLibraryBindableSource? source)
		=> source?.Puzzles.Length switch { null or 0 or 1 => Visibility.Collapsed, _ => Visibility.Visible };

	public static Grid GetCurrentGrid(PuzzleLibraryBindableSource source, int puzzleIndex) => source.Puzzles[puzzleIndex];
}