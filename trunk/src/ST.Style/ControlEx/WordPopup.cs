using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ST.Style.ControlEx
{
    public class WordPopup : ContentControl
    {
        static WordPopup()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WordPopup), new FrameworkPropertyMetadata(typeof(WordPopup)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        #region Dependency Properties  

        /// <summary>  
        /// 单词文本  
        /// </summary>  
        public static readonly DependencyProperty WordContentProperty
            = DependencyProperty.Register("WordContent", typeof(string), typeof(WordPopup), new PropertyMetadata(default(string)));

        /// <summary>  
        /// 单词 发音
        /// </summary>  
        public static readonly DependencyProperty WordPronunciationProperty
            = DependencyProperty.Register("WordPronunciation", typeof(object), typeof(WordPopup), new PropertyMetadata(default(object)));

        /// <summary>  
        /// 单词 得分内容
        /// </summary>  
        public static readonly DependencyProperty WordScoreContentProperty
            = DependencyProperty.Register("WordScoreContent", typeof(object), typeof(WordPopup), new PropertyMetadata(default(object)));

        /// <summary>  
        /// 单词 翻译内容
        /// </summary>  
        public static readonly DependencyProperty WordTransContentProperty
            = DependencyProperty.Register("WordTransContent", typeof(object), typeof(WordPopup), new PropertyMetadata(default(object)));

        /// <summary>  
        /// 内容的背景颜色  
        /// </summary>  
        public static readonly DependencyProperty ContentBackgroundProperty
            = DependencyProperty.Register("ContentBackground", typeof(Brush), typeof(WordPopup));

        /// <summary>  
        /// 单词的字体颜色  
        /// </summary>  
        public static readonly DependencyProperty WordForegroundProperty
            = DependencyProperty.Register("WordForeground", typeof(Brush), typeof(WordPopup), new PropertyMetadata(null, null));

        /// <summary>  
        /// 单词位置  
        /// </summary>  
        public static readonly DependencyProperty WordMarginProperty
            = DependencyProperty.Register("WordMargin", typeof(Thickness), typeof(WordPopup), new PropertyMetadata(new Thickness(0, 0, 0, 0), null));
        /// <summary>  
        /// 单词 字体大小
        /// </summary>  
        public static readonly DependencyProperty WordFontSizeProperty
            = DependencyProperty.Register("WordFontSize", typeof(int), typeof(WordPopup), new PropertyMetadata(16, null));

        /// <summary>  
        /// 鼠标移上去的边框颜色  
        /// </summary>  
        public static readonly DependencyProperty MouseOverBorderBrushProperty
            = DependencyProperty.Register("MouseOverBorderBrush", typeof(Brush), typeof(WordPopup), new PropertyMetadata(null, null));

        /// <summary>  
        /// 鼠标按下去的边框颜色  
        /// </summary>  
        public static readonly DependencyProperty MouseDownBorderBrushProperty
            = DependencyProperty.Register("MouseDownBorderBrush", typeof(Brush), typeof(WordPopup), new PropertyMetadata(null, null));

        /// <summary>  
        /// 圆角  
        /// </summary>  
        public static readonly DependencyProperty CornerRadiusProperty
            = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(WordPopup), null);
        
        //图标高度  
        public static readonly DependencyProperty IconHeightProperty
            = DependencyProperty.Register("IconHeight", typeof(double), typeof(WordPopup), new PropertyMetadata(24.0, null));

        //图标宽度  
        public static readonly DependencyProperty IconWidthProperty
            = DependencyProperty.Register("IconWidth", typeof(double), typeof(WordPopup), new PropertyMetadata(24.0, null));

        //图标和内容的对齐方式  
        public static readonly DependencyProperty IconContentOrientationProperty
            = DependencyProperty.Register("IconContentOrientation", typeof(Orientation), typeof(WordPopup), new PropertyMetadata(Orientation.Horizontal, null));

        //图标和内容的距离  
        public static readonly DependencyProperty IconContentMarginProperty
            = DependencyProperty.Register("IconContentMargin", typeof(Thickness), typeof(WordPopup), new PropertyMetadata(new Thickness(0, 0, 0, 0), null));

        //图标 显示样式
        public static readonly DependencyProperty IconStretchProperty
            = DependencyProperty.Register("IconStretch", typeof(Stretch), typeof(WordPopup),
                new PropertyMetadata(Stretch.Fill, null));

        #endregion

        #region Property Wrappers  

        public string WordContent
        {
            get
            {
                return (string)GetValue(WordContentProperty);
            }
            set { SetValue(WordContentProperty, value); }
        }

        public Brush ContentBackground
        {
            get
            {
                return (Brush)GetValue(ContentBackgroundProperty);
            }
            set { SetValue(ContentBackgroundProperty, value); }
        }

        public Brush WordForeground
        {
            get
            {
                return (Brush)GetValue(WordForegroundProperty);
            }
            set { SetValue(WordForegroundProperty, value); }
        }

        public Thickness WordMargin
        {
            get { return (Thickness)GetValue(WordMarginProperty); }
            set { SetValue(WordMarginProperty, value); }
        }

        public int WordFontSize
        {
            get { return (int)GetValue(WordFontSizeProperty); }
            set { SetValue(WordFontSizeProperty, value); }
        }

        public Brush MouseOverBorderBrush
        {
            get { return (Brush)GetValue(MouseOverBorderBrushProperty); }
            set { SetValue(MouseOverBorderBrushProperty, value); }
        }

        public Brush MouseDownBorderBrush
        {
            get { return (Brush)GetValue(MouseDownBorderBrushProperty); }
            set { SetValue(MouseDownBorderBrushProperty, value); }
        }

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public Stretch IconStretch
        {
            get { return (Stretch)GetValue(IconStretchProperty); }
            set { SetValue(IconStretchProperty, value); }
        } 

        public double IconHeight
        {
            get { return (double)GetValue(IconHeightProperty); }
            set { SetValue(IconHeightProperty, value); }
        }

        public double IconWidth
        {
            get { return (double)GetValue(IconWidthProperty); }
            set { SetValue(IconWidthProperty, value); }
        }

        public Orientation IconContentOrientation
        {
            get { return (Orientation)GetValue(IconContentOrientationProperty); }
            set { SetValue(IconContentOrientationProperty, value); }
        }

        public Thickness IconContentMargin
        {
            get { return (Thickness)GetValue(IconContentMarginProperty); }
            set { SetValue(IconContentMarginProperty, value); }
        }

        public string WordPronunciation
        {
            get
            {
                return (string)GetValue(WordPronunciationProperty);
            }
            set { SetValue(WordPronunciationProperty, value); }
        }

        public object WordScoreContent
        {
            get
            {
                return (object)GetValue(WordScoreContentProperty);
            }
            set { SetValue(WordScoreContentProperty, value); }
        }

        public object WordTransContent
        {
            get
            {
                return (object)GetValue(WordTransContentProperty);
            }
            set { SetValue(WordTransContentProperty, value); }
        }

        #endregion
    }

}