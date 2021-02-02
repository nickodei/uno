﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uno.UI.Tests.App.Xaml;
using Uno.UI.Tests.Helpers;
using Uno.UI.Tests.Windows_UI_Xaml.Controls;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using FluentAssertions;
using FluentAssertions.Execution;
using Uno.UI.Extensions;

namespace Uno.UI.Tests.Windows_UI_Xaml
{
	[TestClass]
	public class Given_xLoad
	{
		[TestInitialize]
		public void Init()
		{
			UnitTestsApp.App.EnsureApplication();
		}

		[TestMethod]
		public void When_xLoad_Multiple()
		{
			var SUT = new When_xLoad();

			var stubs = SUT.EnumerateAllChildren().OfType<ElementStub>();

			Assert.AreEqual(7, stubs.Count());
		}

		[TestMethod]
		public void When_xLoad_LoadSingle()
		{
			var SUT = new When_xLoad_LoadSingle();

			var stubs = SUT.EnumerateAllChildren().OfType<ElementStub>();
			Assert.AreEqual(1, stubs.Count());

			Assert.IsNull(SUT.border1);

			var border1 = SUT.FindName("border1");
			Assert.AreEqual(SUT.border1, border1);
		}

		[TestMethod]
		public void When_xLoad_Deferred_StaticCollapsed()
		{
			var SUT = new When_xLoad_Deferred_StaticCollapsed();

			var stubs = SUT.EnumerateAllChildren().OfType<ElementStub>();
			Assert.AreEqual(1, stubs.Count());

			Assert.IsNull(SUT.border6);

			var border1 = SUT.FindName("border6");
			Assert.AreEqual(SUT.border6, border1);
		}

		[TestMethod]
		public void When_xLoad_Deferred_VisibilityBinding()
		{
			var SUT = new When_xLoad_Deferred_VisibilityBinding();
			SUT.ForceLoaded();

			var stubs = SUT.EnumerateAllChildren().OfType<ElementStub>();
			Assert.AreEqual(1, stubs.Count());

			Assert.IsNull(SUT.border7);

			SUT.DataContext = true;

			Assert.IsNotNull(SUT.border7);

			var border = SUT.FindName("border7");
			Assert.AreEqual(SUT.border7, border);
		}

		[TestMethod]
		public void When_xLoad_Deferred_VisibilityxBind()
		{
			var SUT = new When_xLoad_Deferred_VisibilityxBind();
			SUT.ForceLoaded();
			SUT.Measure(new Size(42, 42));

			var stubs = SUT.EnumerateAllChildren().OfType<ElementStub>();
			Assert.AreEqual(1, stubs.Count());

			Assert.IsNull(SUT.border8);

			SUT.MyVisibility = true;
			SUT.Measure(new Size(42, 42));

			Assert.IsNotNull(SUT.border8);

			var border1 = SUT.FindName("border8");
			Assert.AreEqual(SUT.border8, border1);
		}

		[TestMethod]
		public void When_Deferred_Visibility_and_StaticResource()
		{
			var SUT = new When_xLoad_Multiple();
			SUT.ForceLoaded();
			SUT.Measure(new Size(42, 42));
			SUT.DataContext = Visibility.Collapsed;

			var stubs = SUT.EnumerateAllChildren().OfType<ElementStub>();
			Assert.AreEqual(1, stubs.Count());

			Assert.IsNull(SUT.border1);

			var border1 = SUT.FindName("border1");
			SUT.Measure(new Size(42, 42));

			Assert.IsNotNull(SUT.border1);
			Assert.AreEqual(SUT.border1, border1);
		}
	}
}
