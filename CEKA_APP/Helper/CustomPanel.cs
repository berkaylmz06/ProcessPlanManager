using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CEKA_APP.UsrControl
{
    public class CustomPanel : Panel
    {
        private Color _baseColor;
        private Color _hoverColor;
        private Color _currentColor;
        private Timer _colorTransitionTimer;
        private int _transitionStep = 10;
        private int _currentStep = 0;
        private int _totalSteps = 20;

        public CustomPanel(Color baseColor, Color hoverColor)
        {
            _baseColor = baseColor;
            _hoverColor = hoverColor;
            _currentColor = baseColor;
            DoubleBuffered = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);

            _colorTransitionTimer = new Timer { Interval = 10 };
            _colorTransitionTimer.Tick += ColorTransitionTimer_Tick;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (var brush = new SolidBrush(_currentColor))
            using (var pen = new Pen(Color.FromArgb(150, 150, 150), 1))
            {
                var rect = new Rectangle(0, 0, Width - 1, Height - 1);
                var path = GetRoundedRectanglePath(rect, 15);
                e.Graphics.FillPath(brush, path);
                e.Graphics.DrawPath(pen, path);

                // Gölge efekti
                using (var shadowBrush = new SolidBrush(Color.FromArgb(50, 0, 0, 0)))
                {
                    var shadowPath = GetRoundedRectanglePath(new Rectangle(3, 3, Width - 1, Height - 1), 15);
                    e.Graphics.FillPath(shadowBrush, shadowPath);
                }
            }
        }

        private void ColorTransitionTimer_Tick(object sender, EventArgs e)
        {
            _currentStep++;
            float t = (float)_currentStep / _totalSteps;
            int r = (int)(_baseColor.R + t * (_hoverColor.R - _baseColor.R));
            int g = (int)(_baseColor.G + t * (_hoverColor.G - _baseColor.G));
            int b = (int)(_baseColor.B + t * (_hoverColor.B - _baseColor.B));
            _currentColor = Color.FromArgb(r, g, b);
            Invalidate();

            if (_currentStep >= _totalSteps)
            {
                _colorTransitionTimer.Stop();
                _currentStep = 0;
            }
        }

        public void StartHoverTransition(bool isHover)
        {
            _colorTransitionTimer.Stop();
            _currentStep = 0;
            _hoverColor = isHover ? _hoverColor : _baseColor;
            _colorTransitionTimer.Start();
        }

        private GraphicsPath GetRoundedRectanglePath(Rectangle rect, int radius)
        {
            var path = new GraphicsPath();
            int diameter = radius * 2;
            var arc = new Rectangle(rect.Location, new Size(diameter, diameter));
            path.AddArc(arc, 180, 90); // Sol üst
            arc.X = rect.Right - diameter;
            path.AddArc(arc, 270, 90); // Sağ üst
            arc.Y = rect.Bottom - diameter;
            path.AddArc(arc, 0, 90); // Sağ alt
            arc.X = rect.Left;
            path.AddArc(arc, 90, 90); // Sol alt
            path.CloseFigure();
            return path;
        }
    }
}