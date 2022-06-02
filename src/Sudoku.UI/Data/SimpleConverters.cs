﻿namespace Sudoku.UI.Data;

/// <summary>
/// Provides with a set of methods as simple converters that can be used and called by XAML files.
/// </summary>
internal static class SimpleConverters
{
	private static readonly SolidColorBrush[] DifficultyLevel_Foregrounds =
	{
		new(Color.FromArgb(255,   0,  51, 204)),
		new(Color.FromArgb(255,   0, 102,   0)),
		new(Color.FromArgb(255, 102,  51,   0)),
		new(Color.FromArgb(255, 102,  51,   0)),
		new(Color.FromArgb(255, 102,   0,   0))
	};

	private static readonly SolidColorBrush[] DifficultyLevel_Backgrounds =
	{
		new(Color.FromArgb(255, 204, 204, 255)),
		new(Color.FromArgb(255, 100, 255, 100)),
		new(Color.FromArgb(255, 255, 255, 100)),
		new(Color.FromArgb(255, 255, 150,  80)),
		new(Color.FromArgb(255, 255, 100, 100))
	};


	/// <summary>
	/// Indicates the license displaying value on <see cref="RepositoryInfo.OpenSourceLicense"/>.
	/// </summary>
	/// <param name="input">The license name.</param>
	/// <returns>The converted result string.</returns>
	/// <seealso cref="RepositoryInfo.OpenSourceLicense"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static string License(string input) => $"{input} {Get("AboutPage_License")}";

	/// <summary>
	/// Indicates the conversion on <see cref="RepositoryInfo.IsForReference"/>.
	/// </summary>
	/// <param name="input">The input value.</param>
	/// <returns>The converted result string value.</returns>
	/// <seealso cref="RepositoryInfo.IsForReference"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static string ForReference(bool input) => input ? Get("AboutPage_ForReference") : string.Empty;

	/// <summary>
	/// Gets the title of the info bar via its severity.
	/// </summary>
	/// <param name="severity">The severity.</param>
	/// <returns>The title of the info bar.</returns>
	/// <exception cref="ArgumentOutOfRangeException">Throws when the severity is not defined.</exception>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static string InfoBarTitle(InfoBarSeverity severity)
		=> Get(
			severity switch
			{
				InfoBarSeverity.Informational => "SudokuPage_InfoBar_SeverityInfo",
				InfoBarSeverity.Success => "SudokuPage_InfoBar_SeveritySuccess",
				InfoBarSeverity.Warning => "SudokuPage_InfoBar_SeverityWarning",
				InfoBarSeverity.Error => "SudokuPage_InfoBar_SeverityError",
				_ => throw new ArgumentOutOfRangeException(nameof(severity))
			}
		);

	public static string SliderPossibleValueString(double min, double max, double stepFrequency, double tickFrequency)
		=> $"{Get("SliderPossibleValue")}{min:0.0} - {max:0.0}{Get("SliderStepFrequency")}{stepFrequency:0.0}{Get("SliderTickFrequency")}{tickFrequency:0.0}";

	public static string SliderPossibleValueStringWithFormat(double min, double max, double stepFrequency, double tickFrequency, string format)
		=> $"{Get("SliderPossibleValue")}{min.ToString(format)} - {max.ToString(format)}{Get("SliderStepFrequency")}{stepFrequency.ToString(format)}{Get("SliderTickFrequency")}{tickFrequency.ToString(format)}";

	public static string DifficultyLevelToResourceText(DifficultyLevel difficultyLevel)
		=> difficultyLevel switch
		{
			DifficultyLevel.Easy => Get("SudokuPage_AnalysisResultColumn_Easy"),
			DifficultyLevel.Moderate => Get("SudokuPage_AnalysisResultColumn_Moderate"),
			DifficultyLevel.Hard => Get("SudokuPage_AnalysisResultColumn_Hard"),
			DifficultyLevel.Fiendish => Get("SudokuPage_AnalysisResultColumn_Fiendish"),
			DifficultyLevel.Nightmare => Get("SudokuPage_AnalysisResultColumn_Nightmare"),
			_ => string.Empty,
		};

	public static Visibility StringToVisibility(string? s)
		=> string.IsNullOrWhiteSpace(s) ? Visibility.Collapsed : Visibility.Visible;

	public static SolidColorBrush DifficultyLevelToForeground(DifficultyLevel difficultyLevel)
		=> difficultyLevel switch
		{
			DifficultyLevel and (0 or > DifficultyLevel.Nightmare) => new(Colors.Transparent),
			_ => DifficultyLevel_Foregrounds[Log2((byte)difficultyLevel)]
		};

	public static SolidColorBrush DifficultyLevelToBackground(DifficultyLevel difficultyLevel)
		=> difficultyLevel switch
		{
			DifficultyLevel and (0 or > DifficultyLevel.Nightmare) => new(Colors.Transparent),
			_ => DifficultyLevel_Backgrounds[Log2((byte)difficultyLevel)]
		};

	public static IList<string> GetFontNames()
		=> (from fontName in CanvasTextFormat.GetSystemFontFamilies() orderby fontName select fontName).ToList();
}
