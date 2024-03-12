namespace SudokuStudio.Views.Controls;

/// <summary>
/// Represents for a technique view.
/// </summary>
[DependencyProperty<double>("HorizontalSpacing", DocSummary = "Indicates the horizontal spacing.")]
[DependencyProperty<double>("VerticalSpacing", DocSummary = "Indicates the vertical spacing.")]
[DependencyProperty<TechniqueViewShowMode>("ShowMode", DefaultValue = TechniqueViewShowMode.Both, DocSummary = "Indicates which techniques whose conclusion types are specified will be shown.")]
[DependencyProperty<TechniqueViewSelectionMode>("SelectionMode", DefaultValue = TechniqueViewSelectionMode.Single, DocSummary = "Indicates the selection mode.")]
[DependencyProperty<TechniqueSet>("SelectedTechniques", DocSummary = "Indicates the final selected techniques.")]
public sealed partial class TechniqueView : UserControl
{
	[Default]
	private static readonly TechniqueSet SelectedTechniquesDefaultValue = TechniqueSets.None;


	/// <summary>
	/// Indicates the internal token views.
	/// </summary>
	private readonly List<TokenView> _tokenViews = [];

	/// <summary>
	/// Represents a collection that stores all technique groups used by list view in UI.
	/// </summary>
	private readonly ObservableCollection<TechniqueViewGroupBindableSource> _itemsSource = [];


	/// <summary>
	/// Initializes a <see cref="TechniqueView"/> instance.
	/// </summary>
	public TechniqueView()
	{
		InitializeComponent();
		UpdateShowMode(ShowMode);
	}


	/// <summary>
	/// The entry that can traverse for all tokens.
	/// </summary>
	private Dictionary<Technique, TokenItem> TokenItems
		=> new([
			..
			from view in _tokenViews
			from item in view.ItemsPanelRoot.Children
			let tokenItem = item as TokenItem
			where tokenItem is not null
			let content = (TechniqueViewBindableSource)tokenItem.Content
			select new KeyValuePair<Technique, TokenItem>(content.TechniqueField, tokenItem)
		]);


	/// <summary>
	/// Indicates the event triggered when selected techniques property is changed.
	/// </summary>
	public event TechniqueViewSelectedTechniquesChangedEventHandler? SelectedTechniquesChanged;

	/// <summary>
	/// Indicates the event triggered when the current selected technique is changed.
	/// </summary>
	public event TechniqueViewCurrentSelectedTechniqueChangedEventHandler? CurrentSelectedTechniqueChanged;


	/// <summary>
	/// Try to update all token items via selection state.
	/// </summary>
	private void UpdateSelection(TechniqueSet set)
	{
		foreach (var (technique, item) in TokenItems)
		{
			item.IsSelected = set.Contains(technique);
		}
	}

	/// <summary>
	/// Try to update visibility of technique bound controls.
	/// </summary>
	/// <param name="mode">The show mode.</param>
	private void UpdateShowMode(TechniqueViewShowMode mode)
	{
		foreach (var (technique, item) in TokenItems)
		{
			item.Visibility = GetVisibility(technique);
		}
	}

	/// <summary>
	/// Gets the visibility.
	/// </summary>
	/// <param name="technique">The technique.</param>
	/// <returns>The <see cref="Visibility"/> result.</returns>
	private Visibility GetVisibility(Technique technique)
		=> ShowMode switch
		{
			TechniqueViewShowMode.None => Visibility.Collapsed,
			TechniqueViewShowMode.OnlyAssignments => technique.IsAssignment() ? Visibility.Visible : Visibility.Collapsed,
			TechniqueViewShowMode.OnlyEliminations => technique.IsAssignment() ? Visibility.Collapsed : Visibility.Visible,
			_ => Visibility.Visible
		};


	[Callback]
	private static void ShowModePropertyCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		if ((d, e) is (TechniqueView view, { NewValue: TechniqueViewShowMode mode }))
		{
			view.UpdateShowMode(mode);
		}
	}

	[Callback]
	private static void SelectedTechniquesPropertyCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		if ((d, e) is (TechniqueView view, { NewValue: TechniqueSet set }))
		{
			view.UpdateSelection(set);
		}
	}

	[Callback]
	private static void SelectionModePropertyCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		if ((d, e) is (TechniqueView view, { NewValue: TechniqueViewSelectionMode mode }))
		{
			foreach (var tokenView in view._tokenViews)
			{
				tokenView.SelectionMode = mode switch
				{
					TechniqueViewSelectionMode.None => ListViewSelectionMode.None,
					TechniqueViewSelectionMode.Single => ListViewSelectionMode.Single,
					TechniqueViewSelectionMode.Multiple => ListViewSelectionMode.Multiple
				};
			}
		}
	}


	private void TokenView_Loaded(object sender, RoutedEventArgs e)
	{
		var p = (TokenView)sender;
		p.SelectionMode = SelectionMode switch
		{
			TechniqueViewSelectionMode.None => ListViewSelectionMode.None,
			TechniqueViewSelectionMode.Single => ListViewSelectionMode.Single,
			TechniqueViewSelectionMode.Multiple => ListViewSelectionMode.Multiple
		};

		_tokenViews.Add(p);

		if (_tokenViews.Count == _itemsSource.Count)
		{
			UpdateSelection(SelectedTechniques);
			UpdateShowMode(ShowMode);
		}
	}

	private void TokenView_ItemClick(object sender, ItemClickEventArgs e)
	{
		if (e is
			{
				OriginalSource: TokenView { ItemsPanelRoot.Children: var children } p,
				ClickedItem: TechniqueViewBindableSource { TechniqueField: var field }
			}
			&& children.OfType<TokenItem>().FirstOrDefault(s => lambda(s, field)) is { IsSelected: var isSelected } child)
		{
			var add = SelectedTechniques.Add;
			var remove = SelectedTechniques.Remove;
			(isSelected ? remove : add)(field);

			CurrentSelectedTechniqueChanged?.Invoke(this, new(field, isSelected));
			SelectedTechniquesChanged?.Invoke(this, new(SelectedTechniques));

			if (SelectionMode == TechniqueViewSelectionMode.Single)
			{
				// Special case: If the selection mode is "Single", we should remove all the other enabled token items.
				foreach (var q in _tokenViews)
				{
					if (!ReferenceEquals(p, q))
					{
						foreach (var element in q.ItemsPanelRoot.Children.OfType<TokenItem>())
						{
							if (element.IsSelected)
							{
								element.IsSelected = false;
							}
						}
					}
				}
			}
		}


		static bool lambda(TokenItem s, Technique field) => s.Content is TechniqueViewBindableSource { TechniqueField: var f } && f == field;
	}

	private async void UserControl_LoadedAsync(object sender, RoutedEventArgs e)
	{
		await Task.Delay(500);
		foreach (var source in
			from technique in Enum.GetValues<Technique>()[1..]
			where !technique.GetFeature().HasFlag(TechniqueFeature.NotImplemented)
			select new TechniqueViewBindableSource(technique) into item
			group item by item.ContainingGroup into itemGroup
			orderby itemGroup.Key
			select new TechniqueViewGroupBindableSource(itemGroup.Key, [.. itemGroup]))
		{
			_itemsSource.Add(source);
			await Task.Delay(100);
		}
	}
}
