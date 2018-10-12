using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using Microsoft.Graphics.Canvas.Effects;
using System.Numerics;
using Windows.Graphics.Effects;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Xaml.Hosting;
using Windows.UI.ViewManagement;
using Windows.ApplicationModel.Core;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace TransparentWindow.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AlternativeMainPage : Page
    {
        Compositor _compositor;
        SpriteVisual _hostVisual;
        public AlternativeMainPage()
        {
            this.InitializeComponent();
            InitCompositor(this);
        }

        private void InitCompositor(Page layoutRoot)
        {
            _compositor = ElementCompositionPreview.GetElementVisual(layoutRoot).Compositor;

            _hostVisual = _compositor.CreateSpriteVisual();
            _hostVisual.Size = new Vector2((float)layoutRoot.ActualHeight, (float)layoutRoot.ActualHeight);

            var factory = _compositor.CreateEffectFactory(new GaussianBlurEffect
            {
                BlurAmount = 0.0f,
                BorderMode = EffectBorderMode.Hard,
                Optimization = EffectOptimization.Balanced,
                Source = new CompositionEffectSourceParameter("source"),
            }, null);

            var effectBrush = factory.CreateBrush();
            effectBrush.SetSourceParameter("source", _compositor.CreateHostBackdropBrush());
            _hostVisual.Brush = effectBrush;

            ElementCompositionPreview.SetElementChildVisual(layoutRoot, _hostVisual);


            //make title bar transparent as well
            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
            ApplicationViewTitleBar titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.ButtonBackgroundColor = Colors.Transparent;
            titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;

            //ApplicationViewTitleBar formattableTitleBar = ApplicationView.GetForCurrentView().TitleBar;
            //formattableTitleBar.ButtonBackgroundColor = Colors.Transparent;
            //formattableTitleBar.ButtonForegroundColor = Colors.Transparent; //Colors.White;
            //CoreApplicationViewTitleBar coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            //coreTitleBar.ExtendViewIntoTitleBar = true;
        }

        private void SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (_hostVisual != null)
            {
                _hostVisual.Size = e.NewSize.ToVector2();
            }
        }
    }
}
