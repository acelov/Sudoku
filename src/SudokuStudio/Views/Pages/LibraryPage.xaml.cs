namespace SudokuStudio.Views.Pages;

/// <summary>
/// Represents library page.
/// </summary>
public sealed partial class LibraryPage : Page
{
	/// <summary>
	/// Initializes a <see cref="LibraryPage"/> instance.
	/// </summary>
	public LibraryPage() => InitializeComponent();


	private async void AddOnePuzzleItem_ClickAsync(object sender, RoutedEventArgs e)
	{
		if (sender is not MenuFlyoutItem { Tag: MenuFlyout { Target: GridViewItem { Content: LibraryBindableSource { LibraryInfo: var lib } } } })
		{
			return;
		}

		var dialog = new ContentDialog
		{
			XamlRoot = XamlRoot,
			Title = ResourceDictionary.Get("LibraryPage_AddOnePuzzleDialogTitle"),
			DefaultButton = ContentDialogButton.Primary,
			PrimaryButtonText = ResourceDictionary.Get("LibraryPage_AddOnePuzzleDialogSure"),
			CloseButtonText = ResourceDictionary.Get("LibraryPage_AddOnePuzzleDialogCancel"),
			Content = new AddOnePuzzleDialogContent()
		};
		if (await dialog.ShowAsync() != ContentDialogResult.Primary)
		{
			return;
		}

		var text = ((AddOnePuzzleDialogContent)dialog.Content).TextCodeInput.Text;
		if (!Grid.TryParse(text, out _))
		{
			return;
		}

		await lib.AppendPuzzleAsync(text);
	}

	private async void RemoveDuplicatePuzzlesItem_ClickAsync(object sender, RoutedEventArgs e)
	{
		if (sender is not MenuFlyoutItem { Tag: MenuFlyout { Target: GridViewItem { Content: LibraryBindableSource { LibraryInfo: var lib } } } })
		{
			return;
		}

		await lib.RemoveDuplicatePuzzlesAsync();
	}

	private void VisitItem_Click(object sender, RoutedEventArgs e)
	{
	}

	private void LibrariesDisplayer_ItemClick(object sender, ItemClickEventArgs e)
	{
		if (sender is not GridView { ItemsPanelRoot.Children: var children })
		{
			return;
		}

		foreach (var child in children)
		{
			if (child is not GridViewItem { Content: LibraryBindableSource source, ContextFlyout: var flyout })
			{
				continue;
			}

			if (!ReferenceEquals(source, (LibraryBindableSource)e.ClickedItem))
			{
				continue;
			}

			flyout.ShowAt(child, new() { Placement = FlyoutPlacementMode.Auto });
			break;
		}
	}

	private void ClearPuzzlesItem_Click(object sender, RoutedEventArgs e)
	{
		if (sender is not MenuFlyoutItem { Tag: MenuFlyout { Target: GridViewItem { Content: LibraryBindableSource { LibraryInfo: var lib } } } })
		{
			return;
		}

		lib.ClearPuzzles();
	}

	private void DeleteLibraryItem_Click(object sender, RoutedEventArgs e)
	{
		if (sender is not MenuFlyoutItem { Tag: MenuFlyout { Target: GridViewItem { Content: LibraryBindableSource { LibraryInfo: var lib } } } })
		{
			return;
		}

		lib.Delete();

		var p = (ObservableCollection<LibraryBindableSource>)LibrariesDisplayer.ItemsSource;
		for (var i = 0; i < p.Count; i++)
		{
			var libraryBindableSource = p[i];
			if (libraryBindableSource.LibraryInfo == lib)
			{
				((ObservableCollection<LibraryBindableSource>)LibrariesDisplayer.ItemsSource).RemoveAt(i);
				return;
			}
		}
	}

	private async void PropertiesItem_ClickAsync(object sender, RoutedEventArgs e)
	{
		if (sender is not MenuFlyoutItem { Tag: MenuFlyout { Target: GridViewItem { Content: LibraryBindableSource { LibraryInfo: var lib } } } })
		{
			return;
		}

		var dialog = new ContentDialog
		{
			XamlRoot = XamlRoot,
			Title = ResourceDictionary.Get("LibraryPage_LibraryPropertiesDialogTitle"),
			DefaultButton = ContentDialogButton.Close,
			IsPrimaryButtonEnabled = false,
			CloseButtonText = ResourceDictionary.Get("LibraryPage_LibraryPropertiesDialogClose"),
			Content = new LibraryPropertiesDialogContent
			{
				LibraryName = lib.Name ?? LibraryBindableSource.NameDefaultValue,
				LibraryAuthor = lib.Author ?? LibraryBindableSource.AuthorDefaultValue,
				LibraryPath = lib.LibraryFilePath,
				LibraryConfigPath = lib.ConfigFilePath,
				LibraryDescription = lib.Description ?? LibraryBindableSource.DescriptionDefaultValue,
				LibraryTags = lib.Tags,
				LibraryLastModifiedTime = lib.LastModifiedTime
			}
		};
		await dialog.ShowAsync();
	}
}
