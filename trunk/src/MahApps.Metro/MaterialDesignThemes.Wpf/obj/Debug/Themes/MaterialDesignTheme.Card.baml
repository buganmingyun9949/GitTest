   M S B A M L   `   `   `  ����W  SMaterialDesignThemes.Wpf, Version=1.2.0.33441, Culture=neutral, PublicKeyToken=nullC&clr-namespace:MaterialDesignThemes.WpfMaterialDesignThemes.Wpf  5      Y1clr-namespace:MaterialDesignThemes.Wpf.Converters#MaterialDesignThemes.Wpf.Converters  5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089  �� MergedDictionaries �� Source;    5MaterialDesignThemes.Wpf.Converters.CardClipConverter   CardClipConverter �� 
TargetType#   MaterialDesignThemes.Wpf.Card  CardTemplate � 	Resources=   7MaterialDesignThemes.Wpf.Converters.ShadowEdgeConverter  Key J� OpacityMask  ShadowEdgeConverter `� 	Converter �� Path �� RelativeSource J� 	CacheMode	   UniformCornerRadius
 �� CornerRadius J� Effect9   3MaterialDesignThemes.Wpf.Converters.ShadowConverter   Instance �� 	Converter ��Name �� Padding   ContentClip J� Clip ��Name �� ContentStringFormat �� ContentStringFormat �� 
TargetType �� Property �� Value   MaterialDesignCardBackground+   %MaterialDesignThemes.Wpf.ShadowAssist�� 5      D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    :x,http://schemas.microsoft.com/winfx/2006/xaml     5      .wpf&clr-namespace:MaterialDesignThemes.Wpf  5      @
converters1clr-namespace:MaterialDesignThemes.Wpf.Converters  5        5      �� 5   
   $g apack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Shadows.xaml?�6   %
  &	        &	      7  '      7     5
      6#   �� 5      "  6    5   
   ��  5      ShadowEdgeConverter 6-   5      .R�5   
   � #��z��6   
������  5      `� 5      8   .1�5      �� $ ActualWidth�6P    6   �� *TemplatedParent+6P   �� 5      $ ActualHeight�6P    6   �� *TemplatedParent+6P   �� 5      $$ (wpf:ShadowAssist.ShadowDepth)�6P    6   �� *TemplatedParent+6P   �� 5      $$ (wpf:ShadowAssist.ShadowEdges)�6P    6   �� *TemplatedParent+6P   5      .+�5      ��  6   ��  �� *Self+$" (wpf:ShadowAssist.CacheMode)�.u�5      �� #
 z	 5       5      ��  �� *TemplatedParent+$$ (wpf:ShadowAssist.ShadowDepth)�# Z ._�5      �� $ PART_ClipBorder��5      #��z��5      # z��6G   # z 5      5      5       �� 5!      $ ContentPresenter��5"      #��z��5#      # z 5$      #��z��5%      #��z��5&      #��z��5'      # z 5(      5)      5*      5+      �� 5,      "  6   .�5-   
   ��  � ��8   6%   �� 5.   
    � ��# �  6'   �� 5/   
    � ��$	 Top=�6.   �� 50   
    �@ ShadowDepth$ Depth2=�69   �� 51   
    � |� .  6&   52      54      