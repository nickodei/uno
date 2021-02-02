﻿// MUX reference NavigationViewItemBase.h, commit c2d4b16

namespace Microsoft.UI.Xaml.Controls
{
	public partial class NavigationViewItemBase
	{
		protected virtual void OnNavigationViewItemBasePositionChanged()
		{
		}

		protected virtual void OnNavigationViewItemBaseDepthChanged()
		{
		}

		protected virtual void OnNavigationViewItemBaseIsSelectedChanged()
		{
		}

		// Constant is a temporary measure. Potentially expose using TemplateSettings.
		protected const int c_itemIndentation = 25;

		internal bool IsTopLevelItem { get; set; } = false;

		/// <summary>
		/// Flag to keep track of whether this item was created by the custom internal NavigationViewItemsFactory.
		/// This is required in order to achieve proper recycling
		/// </summary>
		internal bool CreatedByNavigationViewItemsFactory { get; set; } = false;

		protected NavigationView m_navigationView = null;

		private NavigationViewRepeaterPosition m_position = NavigationViewRepeaterPosition.LeftNav;
		private int m_depth = 0;
	}
}
