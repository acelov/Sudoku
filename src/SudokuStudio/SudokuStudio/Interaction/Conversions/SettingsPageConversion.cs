﻿namespace SudokuStudio.Interaction.Conversions;

internal static class SettingsPageConversion
{
	public static string GetDashArrayString(DashArray value) => value.ToString()[1..^1];

	public static string GetSliderString(double value, string format) => value.ToString(format);

	public static string ToRgbString(Color color)
	{
		var (r, g, b) = color;
		return $"{r}, {g}, {b}";
	}

	public static string ToArgbString(Color color)
	{
		var (a, r, g, b) = color;
		return $"#{a:X2}{r:X2}{g:X2}{b:X2} ({nameof(Color.A)} = {a}, {nameof(Color.R)} = {r}, {nameof(Color.G)} = {g}, {nameof(Color.B)} = {b})";
	}

	public static CoordinateLabelDisplayMode GetCoordinateLabelDisplayMode(int index) => (CoordinateLabelDisplayMode)index;

	public static Visibility GetVisibility(string? text) => string.IsNullOrWhiteSpace(text) ? Visibility.Collapsed : Visibility.Visible;

	public static Brush GetBrush(Color color) => new SolidColorBrush(color);

	public static FontFamily GetFont(string fontName) => new(fontName);
}
