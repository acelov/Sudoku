﻿using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using Microsoft.Win32;
using Sudoku.Constants;
using Sudoku.Data;
using Sudoku.Drawing;
using Sudoku.Windows.Constants;
using static System.Drawing.Imaging.ImageFormat;
using static System.Drawing.StringAlignment;
using static Sudoku.Windows.Constants.Processings;
using DEncoder = System.Drawing.Imaging.Encoder;
using DFontStyle = System.Drawing.FontStyle;
using PointConverter = Sudoku.Drawing.PointConverter;

namespace Sudoku.Windows
{
	/// <summary>
	/// Interaction logic for <c>PictureSavingPreferencesWindow.xaml</c>.
	/// </summary>
	public partial class PictureSavingPreferencesWindow : Window
	{
		/// <summary>
		/// <para>
		/// Indicates the text that should be output. Sometimes this field may contain
		/// escape characters (such as "<c>$e</c>" means extension).
		/// </para>
		/// <para>If you don't want to add extra text, the field will be <see langword="null"/>.</para>
		/// </summary>
		/// <remarks>
		/// Escape characters use the character <c>$</c> as the header because the symbol
		/// cannot be used in file names. Supported formats are below:
		/// <list type="table">
		/// <item>
		/// <term><c>$f</c></term>
		/// <description>The file name.</description>
		/// </item>
		/// <item>
		/// <term><c>$e</c></term>
		/// <description>The file extension.</description>
		/// </item>
		/// <item>
		/// <term><c>$D</c></term>
		/// <description>The date without any extra characters. Such as <c>20200202</c>.</description>
		/// </item>
		/// <item>
		/// <term><c>$d</c></term>
		/// <description>The date with hyphen. Such as <c>2020-02-02</c>.</description>
		/// </item>
		/// </list>
		/// </remarks>
		private string? _format;

		/// <summary>
		/// Indicates the settings in main window.
		/// </summary>
		private readonly Settings _settings;

		/// <summary>
		/// Indicates the target painter.
		/// </summary>
		private readonly TargetPainter _targetPainter;

		/// <summary>
		/// Indicates the grid.
		/// </summary>
		private readonly Grid _grid;


		/// <summary>
		/// Initializes an instance with the specified size.
		/// </summary>
		/// <param name="grid">The grid.</param>
		/// <param name="settings">The settings.</param>
		/// <param name="targetPainter">The target painter.</param>
		public PictureSavingPreferencesWindow(Grid grid, Settings settings, TargetPainter targetPainter)
		{
			InitializeComponent();

			(_settings, _grid, _targetPainter) = (settings, grid, targetPainter);
			_numericUpDownSize.CurrentValue = (decimal)_settings.SavingPictureSize;
		}


		/// <summary>
		/// Indicates whether the format string is valid.
		/// </summary>
		/// <param name="filePath">The file path.</param>
		/// <param name="text">(<see langword="out"/> parameter) The output text.</param>
		/// <returns>The <see cref="bool"/> value indicating that.</returns>
		private bool IsFormatValid(string filePath, out string text)
		{
			var sb = new StringBuilder(_format);
			sb.Replace("$f", Path.GetFileNameWithoutExtension(filePath));
			sb.Replace("$e", Path.GetExtension(filePath));
			sb.Replace("$D", $"{DateTime.Now:yyyyMMdd}");
			sb.Replace("$d", $"{DateTime.Now:yyyy-MM-dd}");

			return !(text = sb.ToString()).Contains('$');
		}


		private void ButtonSave_Click(object sender, RoutedEventArgs e)
		{
			string originalString = _textBoxFormat.Text;
			_format = string.IsNullOrWhiteSpace(originalString) ? null : originalString;

			internalOperation((float)_numericUpDownSize.CurrentValue);
			Close();

			bool internalOperation(float size)
			{
				var saveFileDialog = new SaveFileDialog
				{
					AddExtension = true,
					DefaultExt = "png",
					Filter = (string)LangSource["PictureSavingFilter"],
					Title = (string)LangSource["PictureSavingSaveDialogTitle"]
				};

				if (!(saveFileDialog.ShowDialog() is true))
				{
					return !(e.Handled = true);
				}

				var targetPainter = new TargetPainter(new PointConverter(size, size), _settings)
				{
					Grid = _grid,
					View = _targetPainter.View, // May be null.
					CustomView = _targetPainter.CustomView // May be null.
				};

				Bitmap? bitmap = null;
				try
				{
					int selectedIndex = saveFileDialog.FilterIndex;
					string fileName = saveFileDialog.FileName;
					bitmap = targetPainter.Draw();
					switch (selectedIndex)
					{
						case int v when v >= -1 && v <= 3: // Normal picture formats.
						{
							if (!(_format is null))
							{
								if (!IsFormatValid(saveFileDialog.FileName, out string resultText))
								{
									Messagings.CheckFormatString();
									return !(e.Handled = true);
								}

								const int fontSize = 20;
								const string fontName = "Microsoft YaHei UI,Consolas,Times New Roman";
								var result = new Bitmap(bitmap.Width, bitmap.Height + (fontSize << 1));
								using var g = Graphics.FromImage(result);
								using var f = new Font(fontName, fontSize, DFontStyle.Bold);
								using var sf = new StringFormat { Alignment = Center, LineAlignment = Center };
								g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
								g.SmoothingMode = SmoothingMode.HighQuality;
								g.CompositingQuality = CompositingQuality.HighQuality;
								g.InterpolationMode = InterpolationMode.HighQualityBicubic;
								g.Clear(Color.White);
								g.DrawImage(bitmap, 0, 0);
								g.DrawString(
									resultText, f, Brushes.Black, bitmap.Width >> 1,
									bitmap.Height + (fontSize >> 1) + 8, sf);

								SavePicture(result, selectedIndex, fileName);
							}
							else
							{
								SavePicture(bitmap, selectedIndex, fileName);
							}

							break;
						}
						case 4: // Windows metafile format (WMF).
						{
							using var g = Graphics.FromImage(bitmap);
							using var metaFile = new Metafile(fileName, g.GetHdc());
							using var targetGraphics = Graphics.FromImage(metaFile);
							targetPainter.Draw(null, targetGraphics);
							targetGraphics.Save();

							break;
						}
					}
				}
				catch (Exception ex)
				{
					Messagings.ShowExceptionMessage(ex);
					return false;
				}
				finally
				{
					bitmap?.Dispose();
				}

				_settings.SavingPictureSize = size;
				return true;
			}
		}

		private static void SavePicture(Bitmap bitmap, int selectedIndex, string fileName) =>
			bitmap.Save(
				fileName,
				ImageCodecInfo.GetImageEncoders().FirstOrDefault(
					c => c.FormatID == (
						selectedIndex switch
						{
							-1 => Png,
							0 => Png,
							1 => Jpeg,
							2 => Bmp,
							3 => Gif,
							_ => throw Throwings.ImpossibleCase
						}).Guid) ?? throw new NullReferenceException("The return value is null."),
				new EncoderParameters(1) { Param = { [0] = new EncoderParameter(DEncoder.Quality, 100L) } });


		private void ButtonCancel_Click(object sender, RoutedEventArgs e) => Close();
	}
}
