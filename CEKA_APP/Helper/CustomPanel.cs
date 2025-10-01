using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.ComponentModel;

namespace CEKA_APP.UsrControl
{
    public class CustomPanel : Panel
    {
        private Color _baseColor;
        private Color _hoverColor;
        private Color _currentColor;
        private Timer _colorTransitionTimer;
        private DateTime _transitionStartTime;
        private const int _transitionDuration = 250;
        private bool _isHovering = false;
        private bool _isMouseDown = false;
        private int _cornerRadius = 15;
        private bool _isFixed = false; 

        [Category("Appearance")]
        [Description("Panelin köşe yuvarlatma yarıçapı.")]
        public int CornerRadius
        {
            get { return _cornerRadius; }
            set
            {
                if (value >= 0)
                {
                    _cornerRadius = value;
                    this.Invalidate();
                }
            }
        }

        [Category("Behavior")]
        [Description("Panelin fare ile üzerine gelme ve tıklama etkileşimlerine tepki verip vermeyeceğini belirler.")]
        public bool IsFixed
        {
            get { return _isFixed; }
            set
            {
                _isFixed = value;
                if (value)
                {
                    _currentColor = _baseColor; 
                    _isHovering = false;
                    _isMouseDown = false;
                    _colorTransitionTimer.Stop();
                }
                this.Invalidate();
            }
        }
        [Category("Appearance")]
        [Description("Panelin temel rengi.")]
        public Color BaseColor
        {
            get { return _baseColor; }
            set
            {
                _baseColor = value;
                if (!_isHovering && !_isMouseDown)
                {
                    _currentColor = value;
                }
                this.Invalidate();
            }
        }

        [Category("Appearance")]
        [Description("Fare panelin üzerine geldiğinde kullanılacak renk.")]
        public Color HoverColor
        {
            get { return _hoverColor; }
            set { _hoverColor = value; }
        }

        public CustomPanel(Color baseColor, Color hoverColor)
        {
            _baseColor = baseColor;
            _hoverColor = hoverColor;
            _currentColor = baseColor;

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);

            _colorTransitionTimer = new Timer { Interval = 16 };
            _colorTransitionTimer.Tick += ColorTransitionTimer_Tick;
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            if (_isFixed) return;
            base.OnMouseEnter(e);
            _isHovering = true;
            StartTransition(_hoverColor);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            if (_isFixed) return;
            base.OnMouseLeave(e);
            _isHovering = false;
            StartTransition(_baseColor);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (_isFixed) return;
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left)
            {
                _isMouseDown = true;
                this.Invalidate(); 
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (_isFixed) return;
            base.OnMouseUp(e);
            if (e.Button == MouseButtons.Left)
            {
                _isMouseDown = false;
                this.Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.Clear(Parent?.BackColor ?? SystemColors.Control);

            using (var shadowPath = GetRoundedRectanglePath(new Rectangle(2, 2, Width - 5, Height - 5), _cornerRadius))
            {
                DrawShadow(e.Graphics, shadowPath);
            }

            var panelRect = new Rectangle(2, 2, Width - 5, Height - 5);
            if (_isMouseDown)
            {
                panelRect.Offset(0, 1);
            }

            using (var path = GetRoundedRectanglePath(panelRect, _cornerRadius))
            {
                using (var brush = new SolidBrush(_currentColor))
                {
                    e.Graphics.FillPath(brush, path);
                }

                using (var pen = new Pen(Color.FromArgb(50, Color.Gray), 1))
                {
                    e.Graphics.DrawPath(pen, path);
                }
            }
        }

        private void DrawShadow(Graphics g, GraphicsPath path)
        {
            int shadowOffset = _isFixed || _isMouseDown ? 0 : 2; 

            for (int i = 1; i <= 8; i++)
            {
                using (var shadowBrush = new SolidBrush(Color.FromArgb(5 - (int)(i * 0.5), 0, 0, 0)))
                {
                    g.TranslateTransform(shadowOffset, shadowOffset);
                    g.FillPath(shadowBrush, path);
                    g.TranslateTransform(-shadowOffset, -shadowOffset);
                }
            }
        }

        private void StartTransition(Color targetColor)
        {
            _baseColor = _currentColor;
            _hoverColor = targetColor;
            _transitionStartTime = DateTime.Now;
            _colorTransitionTimer.Start();
        }

        private void ColorTransitionTimer_Tick(object sender, EventArgs e)
        {
            var elapsed = DateTime.Now - _transitionStartTime;
            if (elapsed.TotalMilliseconds >= _transitionDuration)
            {
                _currentColor = _hoverColor;
                _colorTransitionTimer.Stop();
            }
            else
            {
                float progress = (float)elapsed.TotalMilliseconds / _transitionDuration;
                float t = (progress < 0.5f) ? 2 * progress * progress : -1 + (4 - 2 * progress) * progress;
                _currentColor = InterpolateColors(_baseColor, _hoverColor, t);
            }
            Invalidate();
        }

        private Color InterpolateColors(Color from, Color to, float progress)
        {
            int r = (int)(from.R + (to.R - from.R) * progress);
            int g = (int)(from.G + (to.G - from.G) * progress);
            int b = (int)(from.B + (to.B - from.B) * progress);
            return Color.FromArgb(r, g, b);
        }

        private GraphicsPath GetRoundedRectanglePath(Rectangle rect, int radius)
        {
            var path = new GraphicsPath();
            int diameter = radius * 2;
            var arcRect = new Rectangle(rect.Location, new Size(diameter, diameter));

            path.AddArc(arcRect, 180, 90);

            arcRect.X = rect.Right - diameter;
            path.AddArc(arcRect, 270, 90);

            arcRect.Y = rect.Bottom - diameter;
            path.AddArc(arcRect, 0, 90);

            arcRect.X = rect.X;
            path.AddArc(arcRect, 90, 90);

            path.CloseFigure();
            return path;
        }
    }
}
