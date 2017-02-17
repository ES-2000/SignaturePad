using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Input.Inking;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace SignaturePad
{
    public sealed partial class SignaturePadView : UserControl
    {
        public bool IsBlank
        {
            get { return inkCanvas.InkPresenter.StrokeContainer.GetStrokes().Count == 0; }
        }

        public List<Point> Points { get; set; }

        Color strokeColor;
        public Color StrokeColor
        {
            get { return strokeColor; }
            set
            {
                if (strokeColor == value)
                    return;
                strokeColor = value;
                var ida = new InkDrawingAttributes();
                ida.Color = strokeColor;
                inkCanvas.InkPresenter.UpdateDefaultDrawingAttributes(ida);
            }
        }

        Color backgroundColor;
        public Color BackgroundColor
        {
            get { return backgroundColor; }
            set
            {
                backgroundColor = value;
                LayoutRoot.Background = new SolidColorBrush(value);
            }
        }

        double strokeWidth;
        public double StrokeWidth
        {
            get { return strokeWidth; }
            set
            {
                if (strokeWidth == value)
                    return;
                strokeWidth = value;
                var ida = new InkDrawingAttributes();
                ida.Size = new Size(strokeWidth, strokeWidth);
                inkCanvas.InkPresenter.UpdateDefaultDrawingAttributes(ida);
            }
        }

        public TextBlock Caption
        {
            get { return tblCaption; }
        }

        public string CaptionText
        {
            get { return tblCaption.Text; }
            set { tblCaption.Text = value; }
        }

        public TextBlock ClearLabel
        {
            get { return btnClear; }
        }

        public string ClearLabelText
        {
            get { return btnClear.Text; }
            set { btnClear.Text = value; }
        }

        public TextBlock SignaturePrompt
        {
            get { return tblDummyText; }
        }

        public string SignaturePromptText
        {
            get { return tblDummyText.Text; }
            set { tblDummyText.Text = value; }
        }

        public Brush SignatureLineBrush
        {
            get { return brdSignatureline.Background; }
            set { brdSignatureline.Background = value; }
        }

        public SignaturePadView()
        {
            InitializeComponent();
            Initialize();
            DataContext = this;
            inkCanvas.InkPresenter.InputDeviceTypes = Windows.UI.Core.CoreInputDeviceTypes.Mouse | Windows.UI.Core.CoreInputDeviceTypes.Touch;
        }

        void Initialize()
        {
            StrokeColor = Colors.Black;
            BackgroundColor = Colors.White;
            StrokeWidth = 5.0;
        }

        public void ClearSignature()
        {
            inkCanvas.InkPresenter.StrokeContainer.Clear();
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            ClearSignature();
        }

        public async Task<Stream> GetImageStream()
        {
            using (InMemoryRandomAccessStream stream = new InMemoryRandomAccessStream())
            {
                await inkCanvas.InkPresenter.StrokeContainer.SaveAsync(stream);

                var reader = new DataReader(stream.GetInputStreamAt(0));
                var bytes = new byte[stream.Size];
                await reader.LoadAsync((uint)stream.Size);
                reader.ReadBytes(bytes);

                return new MemoryStream(bytes);
            }
        }

        public void LoadPoints(Point[] loadedPoints)
        {
            if (loadedPoints == null || loadedPoints.Count() == 0)
                return;
            //Need stream instead of Point array
            //inkCanvas.InkPresenter.StrokeContainer.LoadAsync(stream);
            throw new NotImplementedException();
        }

        private void btnClear_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            ClearSignature();
        }
    }
}
