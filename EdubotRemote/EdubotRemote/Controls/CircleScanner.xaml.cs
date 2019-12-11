using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EdubotRemote.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CircleScanner : ContentView
    {
        private SKCanvasView _canvasView;
        private int _colorIndex;

        public ObservableCollection<Color> Colors { get; } = new ObservableCollection<Color>();

        public int Radius
        {
            get { return (int)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }

        public static readonly BindableProperty RadiusProperty = BindableProperty.Create(nameof(Radius), 
                                                                                            typeof(int), 
                                                                                            typeof(CircleScanner), 
                                                                                            0);

        public bool IsActive
        {
            get { return (bool)GetValue(IsActiveProperty); }
            set { SetValue(IsActiveProperty, value); }
        }

        public static readonly BindableProperty IsActiveProperty = BindableProperty.Create(nameof(IsActive), 
                                                                                            typeof(bool), 
                                                                                            typeof(CircleScanner), 
                                                                                            false, 
                                                                                            BindingMode.OneWayToSource, 
                                                                                            propertyChanged: OnIsActiveChanged);

        public CircleScanner()
        {
            InitializeComponent();
            RootPanel.BindingContext = this;

            _canvasView = CanvasView;
            _colorIndex = 0;

            Colors.CollectionChanged += Colors_CollectionChanged;
        }

        private static void OnIsActiveChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var scanner = bindable as CircleScanner;

            Device.StartTimer(TimeSpan.FromMilliseconds(33), () =>
            {
                if (scanner.IsActive)
                    scanner.CanvasView.InvalidateSurface();

                return scanner.IsActive;
            });
        }

        private void Colors_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            InitializeColors();
        }

        private void InitializeColors()
        {
            this.BackgroundColor = Colors[Colors.Count() - 1];
            _canvasView.InvalidateSurface();
        }

        private void OnPainting(object sender, SKPaintSurfaceEventArgs e)
        {
            SKImageInfo info = e.Info;
            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();

            SKPoint center = new SKPoint(info.Width / 2, info.Height / 2);

            SKPaint paint = new SKPaint
            {
                IsAntialias = false,
                Style = SKPaintStyle.Fill,
                Color = Colors[_colorIndex].ToSKColor()
            };

            canvas.DrawCircle(center.X, center.Y, Radius, paint);

            Radius += 40;

            int hRectInCircle = (int)((Radius / Math.Sqrt(2)) * 2);

            if (hRectInCircle >= Math.Max(info.Width, info.Height))
            {
                this.BackgroundColor = Colors[_colorIndex];

                if (_colorIndex >= Colors.Count() - 1)
                    _colorIndex = 0;
                else
                    _colorIndex++;

                Radius = 0;
            }
        }
    }
}