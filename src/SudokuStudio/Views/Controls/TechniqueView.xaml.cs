using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Sudoku.Analytics.Categorization;
using SudokuStudio.BindableSource;
using SudokuStudio.ComponentModel;
using SudokuStudio.Interaction;

namespace SudokuStudio.Views.Controls;

/// <summary>
/// Represents for a technique view.
/// </summary>
[DependencyProperty<double>("HorizontalSpacing", DocSummary = "Indicates the horizontal spacing.")]
[DependencyProperty<double>("VerticalSpacing", DocSummary = "Indicates the vertical spacing.")]
[DependencyProperty<TechniqueViewSelectionMode>("SelectionMode", DefaultValue = TechniqueViewSelectionMode.Single, DocSummary = "Indicates the selection mode.")]
[DependencyProperty<TechniqueSet>("SelectedTechniques", DocSummary = "Indicates the final selected techniques.")]
public sealed partial class TechniqueView : UserControl
{
	[Default]
	private static readonly TechniqueSet SelectedTechniquesDefaultValue = new();


	/// <summary>
	/// Initializes a <see cref="TechniqueView"/> instance.
	/// </summary>
	public TechniqueView() => InitializeComponent();


	/// <summary>
	/// The items source.
	/// </summary>
	private TechniqueViewGroupBindableSource[] ItemsSource
		=> [
			..
			from technique in Enum.GetValues<Technique>()[1..]
			where !technique.GetFeature().Flags(TechniqueFeature.NotImplemented)
			select new TechniqueViewBindableSource(technique) into item
			group item by item.ContainingGroup into itemGroup
			orderby itemGroup.Key
			select new TechniqueViewGroupBindableSource(itemGroup.Key, [.. itemGroup])
		];


	private void TokenButton_Checked(object sender, RoutedEventArgs e)
	{
		if (SelectionMode == TechniqueViewSelectionMode.None)
		{
			return;
		}

		var techniqueField = ((TechniqueToggleButton)sender).Source.TechniqueField;
		_ = SelectionMode == TechniqueViewSelectionMode.Single
			? SelectedTechniques.Replace(techniqueField)
			: SelectedTechniques.Add(techniqueField);

		if (SelectionMode == TechniqueViewSelectionMode.Single)
		{
			foreach (var control in MainListView.ItemsPanelRoot.Children)
			{
				foreach (var childForGrid in (control as GridLayout)?.Children ?? (IEnumerable<UIElement>)[])
				{
					if (childForGrid is ItemsRepeater { ItemsSourceView: var itemsSourceView })
					{
						for (var i = 0; i < itemsSourceView.Count; i++)
						{
							if (itemsSourceView.GetAt(i) is TechniqueToggleButton { Source.TechniqueField: var currentTechnique } button
								&& !SelectedTechniques.Contains(currentTechnique))
							{
								button.IsChecked = false;
							}
						}
					}
				}
			}
		}
	}

	private void TokenButton_Unchecked(object sender, RoutedEventArgs e)
	{
		if (SelectionMode == TechniqueViewSelectionMode.None)
		{
			return;
		}

		SelectedTechniques.Remove(((TechniqueToggleButton)sender).Source.TechniqueField);
	}
}
