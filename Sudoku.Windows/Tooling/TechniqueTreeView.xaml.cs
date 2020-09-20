﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using Sudoku.DocComments;
using Sudoku.Extensions;
using Sudoku.Solving.Manual;
using static Sudoku.Windows.Constants.Processings;
using CoreResources = Sudoku.Windows.Resources;

namespace Sudoku.Windows.Tooling
{
	/// <summary>
	/// Interaction logic for <c>TechniqueTreeView.xaml</c>.
	/// </summary>
	public partial class TechniqueTreeView : UserControl
	{
		/// <inheritdoc cref="DefaultConstructor"/>
		public TechniqueTreeView() => InitializeComponent();


		/// <inheritdoc/>
		protected override void OnInitialized(EventArgs e)
		{
			base.OnInitialized(e);

			GetAllTechniques();
		}


		/// <summary>
		/// Get all techniques to display.
		/// </summary>
		private void GetAllTechniques()
		{
			var selection = from technique in EnumEx.GetValues<TechniqueCode>()
							let nullableCategory = (string)LangSource[$"Group{technique}"]
							where nullableCategory is not null
							select (
								Technique: technique,
								Id: (int)technique,
								DisplayName: CoreResources.GetValue(technique.ToString()),
								Category: nullableCategory);

			var categories = new List<string>((from quadruple in selection select quadruple.Category).Distinct());


		Labal_Start:
			// Iterate on each category, and add the missing nodes.
			for (int i = 0; i < categories.Count; i++)
			{
				string category = categories[i];
				if (!category.Contains('>'))
				{
					continue;
				}

				string substring = category[..category.LastIndexOf('>')];
				bool alreadyExists = false;
				foreach (string c in categories)
				{
					if (c == substring)
					{
						alreadyExists = true;
						break;
					}
				}

				if (!alreadyExists)
				{
					categories.Insert(i, substring);
					goto Labal_Start;
				}
			}

			// Find the parents.
			var parentList = new List<(int Id, int ParentId, string Content)>();
			int id = 0;
			foreach (string category in categories)
			{
				if (category.Contains('>'))
				{
					// Non-root nodes.
					string parentContent = category[..category.LastIndexOf('>')];
					int parentId = 0;
					foreach (string c in categories)
					{
						if (c == parentContent)
						{
							parentList.Add((id, parentId, category));
							break;
						}

						parentId++;
					}
					if (parentId == categories.Count)
					{
						throw new InvalidOperationException();
					}
				}
				else
				{
					// Root nodes.
					parentList.Add((id, -1, category));
				}

				id++;
			}

			// Create nodes.
			var list = (from category in parentList select g(category)).ToList();

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			static TreeNode<string> g((int Id, int ParentId, string Content) triplet) =>
				new() { Content = triplet.Content, Id = triplet.Id, ParentId = triplet.ParentId };

			// The last step: get all techniques.
			var allNodes = new List<TreeNode<string>>(list);
			foreach (var node in list)
			{
				foreach (var (_, techniqueId, displayName, category) in selection)
				{
					if (node.Content == category)
					{
						allNodes.Add(
							new()
							{
								Content = displayName,
								Id = techniqueId + 1000,
								ParentId = node.Id
							});
					}
				}
			}

			// Now add them to the root node.
			var root = new TreeNode<string> { Content = (string)LangSource["Techniques"], Id = 0, ParentId = -1 };
			root.Children = GetSubnodes(root.ParentId, allNodes);
			list.Prepend(root);

			// Remove all sub-groups.
			list.RemoveAll(node => node.Content!.Contains('>'));

			// Only reserve the name (The parent content and '>' should not be displayed).
			foreach (var node in allNodes)
			{
				if (node.Content!.Contains('>'))
				{
					node.Content = node.Content[(node.Content.LastIndexOf('>') + 1)..];
				}
			}

			_treeViewMain.ItemsSource = list;
		}

		/// <summary>
		/// Get all sub-nodes recursively.
		/// </summary>
		/// <param name="parentId">The parent node ID.</param>
		/// <param name="nodes">The nodes to traverse.</param>
		/// <returns>All sub-nodes.</returns>
		private ICollection<TreeNode<string>> GetSubnodes(int parentId, ICollection<TreeNode<string>> nodes)
		{
			var mainNodes = (from node in nodes where node.ParentId == parentId select node).ToList();
			var otherNodes = nodes.Except(mainNodes).ToList();
			foreach (var mainNode in mainNodes)
			{
				mainNode.Children = GetSubnodes(mainNode.Id, otherNodes);
			}

			return mainNodes;
		}
	}
}
