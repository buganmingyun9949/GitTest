   M S B A M L   `   `   `  ����W  SMaterialDesignThemes.Wpf, Version=1.2.0.33441, Culture=neutral, PublicKeyToken=nullC&clr-namespace:MaterialDesignThemes.WpfMaterialDesignThemes.Wpf  5      Y1clr-namespace:MaterialDesignThemes.Wpf.Converters#MaterialDesignThemes.Wpf.Converters  5      'clr-namespace:ControlzEx
ControlzEx  5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089  �� MergedDictionaries �� Source    BooleanToVisibilityConverterG    AMaterialDesignThemes.Wpf.Converters.NullableToVisibilityConverter  NullVisibilityConverter # InvertedNullVisibilityConverter    	NullValue    NotNullValue=   7MaterialDesignThemes.Wpf.Converters.BrushRoundConverter  BrushRoundConverter'   !MaterialDesignThemes.Wpf.PackIcon�   �ControlzEx.PackIconBase`1[[MaterialDesignThemes.Wpf.PackIconKind, MaterialDesignThemes.Wpf, Version=1.2.0.33441, Culture=neutral, PublicKeyToken=null]]   Kind ' #MaterialDesignPopupBoxToggleContent �� RelativeSource �� AncestorType �� Path'   !MaterialDesignThemes.Wpf.PopupBox �� 
TargetType  MaterialDesignPopupBox	 �� Property
 �� Value+   %MaterialDesignThemes.Wpf.RippleAssist " MaterialDesignFlatButtonRipple+   %MaterialDesignThemes.Wpf.ShadowAssist �� 
TargetType � 	Resources  MaterialDesignSeparator �� BasedOn  Key%   MaterialDesignThemes.Wpf.Ripple   
IsCentered J� SnapsToDevicePixels d�Name 	 ToggleButtonStyle   ToggleContent   ToggleContentTemplate � Cursor d� 	IsChecked �� Mode   ControlzEx.PopupEx  Name � HorizontalOffset � VerticalOffset � AllowsTransparency# � CustomPopupPlacementCallback � PlacementTarget �� ElementName#	   MaterialDesignThemes.Wpf.Card   PopupContent   PopupContentTemplate 
 MaterialDesignBody �� ClearTypeHint  � 	Resources'
  !System.Windows.VisualStateManager! 
  VisualStateGroups%  System.Windows.VisualStateGroup"   Name#   Transitions%  System.Windows.VisualTransition$   GeneratedDuration	%   To&   GeneratedEasingFunction/  )System.Windows.Media.Animation.CircleEase7  1System.Windows.Media.Animation.EasingFunctionBase'   
EasingMode(   States   System.Windows.VisualState)   Name*   
Storyboard+ �� 
TargetName, �� TargetProperty	- W� To. g� Duration/ ��Name0 J� Opacity1 �� 	Converter2   Feedback3 �� Triggers4 P� Property5 P� Value  MaterialDesignToolPopupBox ( $MaterialDesignToolForegroundPopupBox 1 -MaterialDesignMultiFloatingActionPopupBoxIcon - )MaterialDesignMultiFloatingActionPopupBox  PrimaryHueMidBrush   PrimaryHueMidForegroundBrush  MaterialDesignToolTip6 �Name7   From8   
Storyboard9 W� EasingFunction2  ,System.Windows.Media.Animation.QuadraticEase: J� 	CacheMode; A�Name< J� Effect9   3MaterialDesignThemes.Wpf.Converters.ShadowConverter=   Instance> �� 	Converter? J� Clip@ J� RenderTransformOriginA �� AngleB J� IsHitTestVisibleC �� 
TargetName0   *MaterialDesignThemes.Wpf.PopupBoxPopupModeD   	MouseOverE �� ValueF �� BindingG   MouseOverEager-  'System.Windows.Media.Animation.SineEaseH � ToolTipI ^� 	PlacementJ ��NameK   ToggleCheckedContent#L   ToggleCheckedContentTemplate / +MaterialDesignFloatingActionMiniLightButtonM N� EnterActions  OpenN N� ExitActions 	 CloseO �� Triggers 2 .MaterialDesignMultiFloatingActionLightPopupBox  PrimaryHueLightBrush " PrimaryHueLightForegroundBrush 1 -MaterialDesignMultiFloatingActionDarkPopupBox  PrimaryHueDarkBrush ! PrimaryHueDarkForegroundBrush 3 /MaterialDesignMultiFloatingActionAccentPopupBox  SecondaryAccentBrush " SecondaryAccentForegroundBrush�� 5      D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    :x,http://schemas.microsoft.com/winfx/2006/xaml     5      .wpf&clr-namespace:MaterialDesignThemes.Wpf  5      @
converters1clr-namespace:MaterialDesignThemes.Wpf.Converters  5      '
controlzExclr-namespace:ControlzEx  5        5      �� 5   
   $f `pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Button.xaml?�6   �� 5	   
   $d ^pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Menu.xaml?�6   �� 5
   
   $g apack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.ToolTip.xaml?�6   %@)  &	        &	      &	 &     &	 \     &	 o    &	     7  7  7 	 7  7  &	 �    7  &	     7  &	 �   &	     7  7 	 7  7  7  7   7  7  7  7  7  &	 �&    7  &	 L'    7  &	 �'    7  �� 5      6#      5      6/      5      $ Visible=�6W   $ 	Collapsed=�6k     5      6%     5      $ DotsVertical=�6   $��4 0 4 0q�6d   $��24��6u   $��24��6�   ��5      ��  �� *FindAncestor+$ wpf:PopupBoxL�$ 
Foreground��� 5      "  6   .�5   
   �� 	 �@ ToggleContent8
   6*   �� 5   
   	 �@ Feedback#
 �  66   �� 5   
   	 � ��$

 Left=�60   �� 5   
   	 �@ ShadowDepth$
 Depth3=�69   �� 5   
   	 � ��$
 Normal"�63   �� 5   
   	 � ��$
 0 8 0 8q�6$   �� 5   
   	 � |�
 .  6&   �� 5   
   	 � ��
 5      �� 5      "  6"    5       ���� 5!      '��       " ��6    8  67   �� 5"      'd�       " d�6    ToggleButtonStyle .�5#      �� 	 � ��
 5$   "   �� 5%   &   " d�66   .R�5&   *     #��z��65   #��z��6Y   |�.  }�.   . #��z��5)   5   #��z��5*   5   #��z��5+   5   # z 5,   5   5-   '   5/      50      51      .R�52      � .+�53      d� $ PART_Toggle��6'   8�� #��z 54   '   #��z 6Q   $
 Hand��55   '   $��Center=�56   '    53   g   ��  �� *TemplatedParent+$ IsPopupOpen�$ TwoWay=�56   '     58      $ 
PART_Popup��6-   $ 5��5:   -   $ 5��5;   -   $��Custom=�5=   -   $
��Fade=�5>   -    . ��58   A   ��  �� *TemplatedParent+$ IsPopupOpen�$ TwoWay=� 59   -   ��  �� *TemplatedParent+$ PopupPlacementMethod� 5<   -   �� $ PART_Toggle��.��5@      	  #��z 6'   #��z 6P   #��� 
 5A   '   $��15(�5B   '   $��Regular"�5C   '   #��z��5D   '   $ Enabled=�5E   '   $��5q�5F   '     5G   "   ���� 5H   &   '��       " ��6,   .�5I   *   �� 	 � ��$
 Bottom=�6U   �� 5J   *   	 � ��$

 Left=�6W   �� 5K   *   	 � ��$
 
16 0 16 16q�6D   �� 5L   *   	 � ��#
 � 
 6G   �� 5M   *   	 � ��
 5N   .   �� 5O   2   " ��6B   .R�5P   6   � ! 5Q   :     5R   >   $" CommonStates��6O   # 5S   B     5T   F   $$ 0:0:0.3D�6W   $% Normal��6s   & 5U   J     5V   N   $' EaseOut=�6Y   5X   G   .( 5Z   B     $) Normal��6N     5[   B   $) 	MouseOver��6N   .* 5\   F   �� .��5]   J   W� $+ MouseOverBorder��6Z   $, Opacity�6�   $	- 0.1X�5^   Z   $. 0D�6c   5_   G   5`   C     5a   B   $) Disabled��6N   .* 5b   F   �� .��5c   J   W� $, Opacity�6Z   $
- 0.48X�5d   Z   $. 0D�6d   5e   G   5f   C   5g   ?   .+�5i   :   �� $/ MouseOverBorder��6A   $0 0Q�5j   A   ��5k   A   �� *
Foreground+81    5l   :   |�.  $��48��5n   E   #��z��5o   E   #��z��5p   E   #��z��5q   E   # z 5r   E   #��z��5s   E   #��z��5t   E   #��z��5u   E   2 5l   E   �� *
Foreground+81  5u   E   5v   7   5w   3   5y   +   5z   '   5{   #   5|      5}      5~      3 5      P� 5�      4 � {�5 .  . �5�      �� 	 �@J�Opacity$

 0.23Q�68   5�      5�      5�      5�      �� 5�      8   "  6a   .�5�   
   �� 	 � ��
 �#6161616'   �� 5�   
   	 � ��$
 4q�6$   5�      �� 5�      8   "  6o   .�5�   
   �� 	 � ��
 6'   ��  �� " �$ (TextElement.Foreground)�5�        5�      $
 Plus=�5�      $��24��6   $��24��6*   ��66   ��  �� *FindAncestor+$ wpf:PopupBoxL�$ 
Foreground��� 5�      "  6   .�5�   
   �� 	 � ��#
 �  6'   �� 5�   
   	 � ��#
 �  6(   �� 5�   
   	 � ��#
 �  6'   �� 5�   
   	 �@ Feedback

 �����66   �� 5�   
   	 �@ PlacementMode$
 TopAndAlignCentres=�6*   �� 5�   
   	 �@ 	PopupMode$
 MouseOverEager=�6&   �� 5�   
   	 �@^�	Placement$

 Left=�65   �� 5�   
   	 �@^�	IsEnabled
 .  65   �� 5�   
   	 � ��$

 Left=�60   �� 5�   
   	 � ��$
 1q�6,   �� 5�   
   	 �@�Cursor$

 Hand��6#   �� 5�   
   	 �@ ShadowDepth$
 Depth2=�69   �� 5�   
   	 � ��$
 56��6"   �� 5�   
   	 � ��$
 56��6#   �� 5�   
   	 � |�
 .  6&   �� 5�   
   	 � ��
 5�      �� 5�      "  6"    5�      ���� 5�      '_�       " _�6    8   �5�      �� 5�   "   	 � ��$
 	8 10 8 10q�6;   5�      �� 5�      'd�       " d�6    ToggleButtonStyle .�5�      �� 	 � ��
 5�   "   �� 5�   &   " d�66   .R�5�   *   � $6 RootGrid��6/   ��6A   Z� ! 5�   .     5�   2   $" CheckedStates��6C   # 5�   6     5�   :   $7 *��6K   $% Checked��6T   .8 5�   >   �� .��5�   B   W� $+ ContentRotateTransform��6R   $, Angle�5�   R   $- 45X�5�   R   $. 0:0:0.1D�5�   R   9 5�   F     5�   J   $' 	EaseInOut=�6X   5�   C   5�   ?   5�   ;     5�   :   $7 Checked��6K   $% 	Unchecked��6Z   .8 5�   >   �� .��5�   B   W� $+ ContentRotateTransform��6R   $, Angle�5�   R   $- 0X�5�   R   $. 0:0:0.1D�5�   R   9 5�   F     5�   J   $' 	EaseInOut=�6X   5�   C   5�   ?   5�   ;   .( 5�   6     Checked��.* 5�   :   �� .��5�   >   W� $+ ContentRotateTransform��6N   $, Angle�5�   N   $- 45X�5�   N   $. 0D�5�   N   5�   ;   5�   7     5�   6   	Unchecked��.* 5�   :   �� .��5�   >   W� $+ ContentRotateTransform��6N   $, Angle�5�   N   $- 0X�5�   N   $. 0D�5�   N   5�   ;   5�   7   5�   3   .+�5�   .   �� : 6?   ��  �� *Self+$" (wpf:ShadowAssist.CacheMode)�.u�5�   2   A� $; border��5�   :   #��z��5�   :   #��z��6^   #��z��6�   < 5�   :   ��  �� *FindAncestor+$ wpf:PopupBoxL�$$ (wpf:ShadowAssist.ShadowDepth)�#> Z= 5�   3   5�   /     5�   .   |�.  }�.  . #��z��5�   9   #��z��5�   9   #��z��5�   9   # z 5�   9   ? 5�   9   �� $ GeometryEllipse��$ RenderedGeometry�.��5�   2   �� #��z��6A   #��z��6e   $@ .5,.5&�5�   A   |�.  z�5�   6   �� 5�   :   ContentRotateTransform��$A 0Q�6j   5�   3   5�   /   A� 5�   .   $; GeometryEllipse��66   
������ B .  |�.  6{   5�   +   3 5�   *   P� 5�   .   4 � {�5 .  . �5�   2   �� 	 �@J�Opacity$

 0.23Q�6L   5�   /   P� 5�   .   4 �@J�IsMouseOver5 . . �5�   2   �� $C border��69   	 �@ Darken
 . 6p   5�   /   �� 5�   .   #E ZD 5   :   F 5�   :   ��  �� *FindAncestor+$ wpf:PopupBoxL�$ 	PopupMode�.a�5  2   �� $C RootGrid��69   	 � ��

 ���� 6e   5  /   �� 5  .   #E ZG 5  :   F 5  :   ��  �� *FindAncestor+$ wpf:PopupBoxL�$ 	PopupMode�.a�5  2   �� $C RootGrid��69   	 � ��

 ���� 6e   5  /   5  '   5
     5     �� 5     Open .��5     W� $+ PART_PopupContentControl��6.   $, Opacity�6_   $- 1X�5  .   $. 0:0:0.1D�65   9 5  "     5  &   $' 	EaseInOut=�6/   5     W� 5     $+ ExplicitToggleContent��6.   $, Opacity�6\   $- 0X�5  .   $. 0:0:0.1D�65   9 5  "     5  &   $' 	EaseInOut=�6/   5     W� 5     $"+ ExplicitToggleCheckedContent��6.   $, Opacity�6c   $- 1X�5  .   $. 0:0:0.1D�65   9 5  "     5  &   $' 	EaseInOut=�6/   5     5     �� 5      	Close .��5!     W� $+ PART_PopupContentControl��6.   $, Opacity�6_   $. 0:0:0.8D�5"  .   9 5#  "     5$  &   $' 	EaseInOut=�6/   5&     W� 5'     $+ ExplicitToggleContent��6.   $, Opacity�6\   $- 1X�5(  .   $. 0:0:0.1D�65   9 5)  "     5*  &   $' 	EaseInOut=�6/   5,     W� 5-     $"+ ExplicitToggleCheckedContent��6.   $, Opacity�6c   $- 0X�5.  .   $. 0:0:0.1D�65   9 5/  "     50  &   $' 	EaseInOut=�6/   52     53     54     .R�55     � .+�56     d� $ PART_Toggle��6'   8�� $
 Hand��57  '   #��z��58  '   #��z��59  '   #��z��5:  '   $��Stretch=�5;  '   $��Stretch=�5<  '   #H zH 5=  '    56  g   ��  �� *TemplatedParent+$ IsPopupOpen�$ TwoWay=�I 5>  '   ��  �� *FindAncestor+$ wpf:PopupBoxL�$  (ToolTipService.Placement)�.��5?     � .+�5@  "   �� $J StandardToggleContent��61   #��z 6P   #��z 6z   |�.  y�5A  1   �� *ToggleCheckedContent+81  6�   �� 5B  "   $J ExplicitToggleContent��61   #��z 6P   #��z 6z   |�.  y�5C  1   �� *ToggleCheckedContent+81  6�   �� 5D  "   $"J ExplicitToggleCheckedContent��61   #��zK 6W   #��zL 6�   $0 0Q�5F  1   $@ .5,.5&�5G  1   |�.  y�5E  1   �� *ToggleCheckedContent+81  z�5I  &   �� 5J  *   $	A -45Q�6:   5L  #   5M     5N       5O     $ 
PART_Popup��6-   ��.  $��Custom=�5S  +   $
��None=�5T  +    .  5Q  +   ��  �� *TemplatedParent+$ PopupPlacementMethod� 5R  +   �� $ PART_Toggle��.��5V     � .+�5[  "   �� 
�������$0 0.002Q�6<   �� 5\  "   $J PART_PopupContentControl��5_  =   #��z 5\  1   #��z 6Z   $��5q�5^  1   $��8q�6<   $0 0Q�5_  1   y�5]  1   �� *	IsEnabled+81    5`  &   �� 5a  *   �� 5b  .   '_�       " _�64   8  �5c  2   �� 5d  6   	 � ��$
 	16 4 16 4q�6O   5f  /   �� 5g  .   '��       " ��64   8  �5h  2   �� 5i  6   	 �@^�	Placement
 6a   ��  �� *FindAncestor+$ wpf:PopupBoxL�$  (ToolTipService.Placement)��� 5j  6   	 �@^�InitialShowDelay$
 0��6h   �� 5k  6   	 � ��$
 4q�6O   5m  /   5n  +   5p  #   5q     5r     5s     3 5t     P� 5u     4 �@ IsPopupOpen5 . . �5v     �� $C 
PART_Popup��6%   	 � ��
 . 6O   M 5w     �� 5x  "   8�� 62   N 5z     �� 5{  "   8��	 62   5}     P� 5~     4 �@ 	PopupMode$5 MouseOverEager=�67   . �5     �� $C 
PART_Popup��6%   	 � ��
 . 6O   5�     5�     5�     �� 5�  
   	 �@ ToggleContent8
 
 6*   O 5�  
   P� 5�     4 �@ UnfurlOrientation$5 
Horizontal=�63   . �5�     �� 	 �@^�	Placement$
 Custom=�6=   5�     5�     �� 5�     "  6   8   .�5�  
   �� 	 � ��#
 �  6'   �� 5�  
   	 � ��#
 �  6(   �� 5�  
   	 � ��#
 �  6'   5�     �� 5�     "  6   8   .�5�  
   �� 	 � ��#
 �  6'   �� 5�  
   	 � ��#
 �  6(   �� 5�  
   	 � ��#
 �  6'   5�     �� 5�     "  6   8   .�5�  
   �� 	 � ��#
 �  6'   �� 5�  
   	 � ��#
 �  6(   �� 5�  
   	 � ��#
 �  6'   5�     5�     