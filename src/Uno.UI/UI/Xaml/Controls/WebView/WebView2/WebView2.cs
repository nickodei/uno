﻿#nullable enable

using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using System;
using System.Threading.Tasks;
using Uno;
using Microsoft.Web.WebView2.Core;
using Uno.UI.Xaml.Controls;

namespace Microsoft.UI.Xaml.Controls;

/// <summary>
/// Represents an object that enables the hosting of web content.
/// </summary>
#if NET461 || __WASM__ || __SKIA__ || __NETSTD_REFERENCE__
[Uno.NotImplemented("NET461", "__WASM__", "__SKIA__", "__NETSTD_REFERENCE__")]
#endif
public partial class WebView2 : Control, IWebView
{
	private bool _sourceChangeFromCore;
	private bool _coreWebView2Initialized;

	/// <summary>
	/// Initializes a new instance of the WebView2 class.
	/// </summary>
	public WebView2()
	{
		DefaultStyleKey = typeof(WebView2);

		CoreWebView2 = new CoreWebView2(this);
		CoreWebView2.HistoryChanged += CoreWebView2_HistoryChanged;
		CoreWebView2.NavigationStarting += CoreWebView2_NavigationStarting;
		CoreWebView2.NavigationCompleted += CoreWebView2_NavigationCompleted;
		CoreWebView2.SourceChanged += CoreWebView2_SourceChanged;
		CoreWebView2.WebMessageReceived += CoreWebView2_WebMessageReceived;

		Loaded += WebView2_Loaded;
	}

	public CoreWebView2 CoreWebView2 { get; }

	bool IWebView.IsLoaded => IsLoaded;

	protected override void OnApplyTemplate() => CoreWebView2.OnOwnerApplyTemplate();

	private void WebView2_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e) =>
		EnsureCoreWebView2();

	public IAsyncAction EnsureCoreWebView2Async() =>
		AsyncAction.FromTask(ct =>
		{
			EnsureCoreWebView2();
			return Task.CompletedTask;
		});

	public IAsyncOperation<string?> ExecuteScriptAsync(string javascriptCode) =>
		CoreWebView2.ExecuteScriptAsync(javascriptCode);

	public void Reload() => CoreWebView2.Reload();

	public void GoForward() => CoreWebView2.GoForward();

	public void GoBack() => CoreWebView2.GoBack();

	public void NavigateToString(string htmlContent) => CoreWebView2.NavigateToString(htmlContent);

	private void EnsureCoreWebView2()
	{
		if (!_coreWebView2Initialized)
		{
			CoreWebView2Initialized?.Invoke(this, new());
			_coreWebView2Initialized = true;
		}
	}

	private void CoreWebView2_NavigationStarting(CoreWebView2 sender, CoreWebView2NavigationStartingEventArgs args) =>
		NavigationStarting?.Invoke(this, args);

	private void CoreWebView2_NavigationCompleted(CoreWebView2 sender, CoreWebView2NavigationCompletedEventArgs args) =>
		NavigationCompleted?.Invoke(this, args);

	private void CoreWebView2_HistoryChanged(CoreWebView2 sender, object args) =>
		(CanGoBack, CanGoForward) = (sender.CanGoBack, sender.CanGoForward);

	private void CoreWebView2_WebMessageReceived(CoreWebView2 sender, CoreWebView2WebMessageReceivedEventArgs args) =>
		WebMessageReceived?.Invoke(this, args);

	private void CoreWebView2_SourceChanged(CoreWebView2 sender, CoreWebView2SourceChangedEventArgs args)
	{
		_sourceChangeFromCore = true;
		Source = Uri.TryCreate(sender.Source, UriKind.Absolute, out var uri) ? uri : CoreWebView2.BlankUri;
		_sourceChangeFromCore = false;
	}
}
