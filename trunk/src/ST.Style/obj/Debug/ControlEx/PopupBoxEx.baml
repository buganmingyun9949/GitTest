   M S B A M L   `   `   `  ����C  ?ST.Style, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:ST.Style.ControlExST.Style.ControlEx  5      W SMaterialDesignThemes.Wpf, Version=1.2.0.33442, Culture=neutral, PublicKeyToken=null{Sclr-namespace:MaterialDesignThemes.Wpf.Converters;assembly=MaterialDesignThemes.Wpf#MaterialDesignThemes.Wpf.Converters 5      I:clr-namespace:ControlzEx;assembly=MaterialDesignThemes.Wpf
ControlzEx 5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089  �� MergedDictionaries �� Source    BooleanToVisibilityConverterG   AMaterialDesignThemes.Wpf.Converters.NullableToVisibilityConverter  NullVisibilityConverter # InvertedNullVisibilityConverter    	NullValue    NotNullValue=  7MaterialDesignThemes.Wpf.Converters.BrushRoundConverter  BrushRoundConverter'  !MaterialDesignThemes.Wpf.PopupBox �� 
TargetType  MainHeadPopupBox �� Property  PrimaryHueMidBrush �� Value   PrimaryHueMidForegroundBrush+  %MaterialDesignThemes.Wpf.RippleAssist+  %MaterialDesignThemes.Wpf.ShadowAssist �� 
TargetType � 	Resources  MaterialDesignToolTip	 �� BasedOn  Key
 �Name'  !System.Windows.VisualStateManager   VisualStateGroups%  System.Windows.VisualStateGroup   Name   Transitions%  System.Windows.VisualTransition   From	   To   
Storyboard �� 
TargetName �� TargetProperty	 W� To g� Duration W� EasingFunction2  ,System.Windows.Media.Animation.QuadraticEase7	  1System.Windows.Media.Animation.EasingFunctionBase 	  
EasingMode   States 
  System.Windows.VisualState 
  
Storyboard J� 	CacheMode �� RelativeSource �� Path A�Name J� Effect �� AncestorType9  3MaterialDesignThemes.Wpf.Converters.ShadowConverter   Instance  �� 	Converter%  MaterialDesignThemes.Wpf.Ripple!   
IsCentered" J� SnapsToDevicePixels# J� Clip$ �� ElementName% J� RenderTransformOrigin& �� Angle' J� IsHitTestVisible( �� Triggers) P� Property* P� Value+ �� 
TargetName0  *MaterialDesignThemes.Wpf.PopupBoxPopupMode,   	MouseOver- �� Value. �� Binding/   MouseOverEager-  'System.Windows.Media.Animation.SineEase0 d�Name 	 ToggleButtonStyle1 � Cursor2 � ToolTip3 d� 	IsChecked4 �� Mode5 ^� 	Placement6 ��Name7   ToggleContent8   ToggleContentTemplate9 �� 	Converter:   ToggleCheckedContent#;   ToggleCheckedContentTemplate< J� Opacity  ControlzEx.PopupEx=  Name> � AllowsTransparency#? � CustomPopupPlacementCallback@ � PlacementTargetA ��NameB   PopupContentC   PopupContentTemplateD � 	Resources /
 +MaterialDesignFloatingActionMiniLightButtonE N� EnterActions  OpenF N� ExitActions 	 Close 2 .MaterialDesignMultiFloatingActionPopupBoxIcon1G �� Triggers'  !MaterialDesignThemes.Wpf.PackIcon�  �ControlzEx.PackIconBase`1[[MaterialDesignThemes.Wpf.PackIconKind, MaterialDesignThemes.Wpf, Version=1.2.0.33442, Culture=neutral, PublicKeyToken=null]]H   Kind�� 5      D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    :x,http://schemas.microsoft.com/winfx/2006/xaml     5      *local clr-namespace:ST.Style.ControlEx  5      ;wpf1http://materialdesigninxaml.net/winfx/xaml/themes  5      b
convertersSclr-namespace:MaterialDesignThemes.Wpf.Converters;assembly=MaterialDesignThemes.Wpf  5      JcontrolzEx1:clr-namespace:ControlzEx;assembly=MaterialDesignThemes.Wpf  5        5      �� 5	   
   $f `pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Button.xaml?�6   �� 5   
   $g apack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.ToolTip.xaml?�6   %V  &	        &	      &	 &     &	 \     &	 o     7  7 	 7  7  7  7   7  7 
 7  7  7  &	 d   �� 5      6#      5      6/      5      $ Visible=�6W   $ 	Collapsed=�6k     5      6%   �� 5      "  6   .�5   
   ��  � ��# �  6'   �� 5   
    � ��# �  6(   �� 5   
    � ��# �  6'   �� 5   
    �@ Feedback
 �����66   �� 5   
    �@ PlacementMode$ TopAndAlignCentres=�6*   �� 5   
    �@ 	PopupMode$ MouseOverEager=�6&   �� 5   
    �@^�	Placement$
 Left=�65   �� 5   
    �@^�	IsEnabled .  65   �� 5   
    � ��$
 Left=�60   �� 5   
    � ��$ 1q�6,   �� 5   
    �@�Cursor$
 Hand��6#   �� 5    
    �@ ShadowDepth$ Depth2=�69   �� 5!   
    � ��$ 56��6"   �� 5"   
    � ��$ 56��6#   �� 5#   
    � |� .  6&   �� 5$   
    � �� 5%      �� 5&      "  6"    5'      ���� 5(      '_�       " _�6    8	   �5)      �� 5*   "    � ��$ 	8 10 8 10q�6;   5,      �� 5-      'd�       " d�6    ToggleButtonStyle .�5.      ��  � �� 5/   "   �� 50   &   " d�66   .R�51   *   � $
 RootGrid��6/   ��6A   Z�  52   .     53   2   $ CheckedStates��6C    54   6     55   :   $ *��6K   $ Checked��6T   . 56   >   �� .��57   B   W� $ ContentRotateTransform��6R   $ Angle�58   R   $ 45X�59   R   $ 0:0:0.1D�5:   R    5;   F     5<   J   $ 	EaseInOut=�6X   5>   C   5?   ?   5@   ;     5A   :   $ Checked��6K   $ 	Unchecked��6Z   . 5B   >   �� .��5C   B   W� $ ContentRotateTransform��6R   $ Angle�5D   R   $ 0X�5E   R   $ 0:0:0.1D�5F   R    5G   F     5H   J   $ 	EaseInOut=�6X   5J   C   5K   ?   5L   ;   . 5N   6   
  Checked��. 5O   :   �� .��5P   >   W� $ ContentRotateTransform��6N   $ Angle�5Q   N   $ 45X�5R   N   $ 0D�5S   N   5T   ;   5U   7   
  5V   6   	Unchecked��. 5W   :   �� .��5X   >   W� $ ContentRotateTransform��6N   $ Angle�5Y   N   $ 0X�5Z   N   $ 0D�5[   N   5\   ;   5]   7   5^   3   .+�5`   .   ��  6?   ��  �� *Self+$" (wpf:ShadowAssist.CacheMode)�.u�5a   2   A� $ border��5c   :   #��z��5a   :   #��z��6^   #��z��6�    5b   :   ��  �� *FindAncestor+$ wpf:PopupBoxL�$$ (wpf:ShadowAssist.ShadowDepth)�#  Z 5d   3   5e   /     5f   .   |�.  }�. ! . #��z��5i   9   #��z��5j   9   #��z��5k   9   #" z" 5l   9   # 5g   9   �� $$ GeometryEllipse��$ RenderedGeometry�.��5n   2   �� #��z��6A   #��z��6e   $% .5,.5&�5o   A   |�.  z�5p   6   �� 5q   :   ContentRotateTransform��$& 0Q�6j   5s   3   5t   /   A� 5u   .   $ GeometryEllipse��66   
������ ' .  |�.  6{   5v   +   ( 5w   *   P� 5x   .   ) � {�* .  . �5y   2   ��  �@J�Opacity$
 0.23Q�6L   5z   /   P� 5{   .   ) �@J�IsMouseOver* . . �5|   2   �� $+ border��69    �@ Darken . 6p   5}   /   �� 5~   .   #- Z, 5   :   . 5~   :   ��  �� *FindAncestor+$ wpf:PopupBoxL�$ 	PopupMode�.a�5�   2   �� $+ RootGrid��69    � ��
 ���� 6e   5�   /   �� 5�   .   #- Z/ 5�   :   . 5�   :   ��  �� *FindAncestor+$ wpf:PopupBoxL�$ 	PopupMode�.a�5�   2   �� $+ RootGrid��69    � ��
 ���� 6e   5�   /   5�   '   5�      5�      �� 5�      Open .��5�      W� $ PART_PopupContentControl��6.   $ Opacity�6_   $ 1X�5�   .   $ 0:0:0.1D�65    5�   "     5�   &   $ 	EaseInOut=�6/   5�      W� 5�      $ ExplicitToggleContent��6.   $ Opacity�6\   $ 0X�5�   .   $ 0:0:0.1D�65    5�   "     5�   &   $ 	EaseInOut=�6/   5�      W� 5�      $" ExplicitToggleCheckedContent��6.   $ Opacity�6c   $ 1X�5�   .   $ 0:0:0.1D�65    5�   "     5�   &   $ 	EaseInOut=�6/   5�      5�      �� 5�      	Close .��5�      W� $ PART_PopupContentControl��6.   $ Opacity�6_   $ 0:0:0.8D�5�   .    5�   "     5�   &   $ 	EaseInOut=�6/   5�      W� 5�      $ ExplicitToggleContent��6.   $ Opacity�6\   $ 1X�5�   .   $ 0:0:0.1D�65    5�   "     5�   &   $ 	EaseInOut=�6/   5�      W� 5�      $" ExplicitToggleCheckedContent��6.   $ Opacity�6c   $ 0X�5�   .   $ 0:0:0.1D�65    5�   "     5�   &   $ 	EaseInOut=�6/   5�      5�      5�      .R�5�      � .+�5�      d� $0 PART_Toggle��6'   8�� $
1 Hand��5�   '   #��z��5�   '   #��z��5�   '   #��z��5�   '   $��Stretch=�5�   '   $��Stretch=�5�   '   #2 z2 5�   '   3 5�   g   ��  �� *TemplatedParent+$ IsPopupOpen�$4 TwoWay=�5 5�   '   ��  �� *FindAncestor+$ wpf:PopupBoxL�$  (ToolTipService.Placement)�.��5�      � .+�5�   "   �� $6 StandardToggleContent��61   #��z7 6P   #��z8 6z   |�.  y�5�   1   �� *ToggleCheckedContent+89  6�   �� 5�   "   $6 ExplicitToggleContent��61   #��z7 6P   #��z8 6z   |�.  y�5�   1   �� *ToggleCheckedContent+89  6�   �� 5�   "   $"6 ExplicitToggleCheckedContent��61   #��z: 6W   #��z; 6�   $< 0Q�5�   1   $% .5,.5&�5�   1   |�.  y�5�   1   �� *ToggleCheckedContent+89  z�5�   &   �� 5�   *   $	& -45Q�6:   5�   #   5�      5�        5�      $= 
PART_Popup��6.   ��.  $��Custom=�5�   +   $
��None=�5�   +   > . ? 5�   +   ��  �� *TemplatedParent+$ PopupPlacementMethod�@ 5�   +   �� $$ PART_Toggle��.��5�      � .+�5�   "   �� $A PART_Border��6)   
�������$y�Visible=�6Q   �� 5�   "   $6 PART_PopupContentControl��5�   =   #��zB 5�   1   #��zC 6Z   $��5q�5�   1   $��8q�6<   $< 0Q�5�   1   y�5�   1   �� *	IsEnabled+89  D 5�   &   �� 5�   *   �� 5�   .   '_�       " _�64   8	  �5�   2   �� 5�   6    � ��$ 	16 4 16 4q�6O   5�   /   �� 5�   .   '��       " ��64   8	  �5�   2   �� 5�   6    �@^�	Placement 6a   ��  �� *FindAncestor+$ wpf:PopupBoxL�$  (ToolTipService.Placement)��� 5�   6    �@^�InitialShowDelay$ 0��6h   �� 5�   6    � ��$ 4q�6O   5�   /   5�   +   5�   #   5�      5�      5�      ( 5�      P� 5�      ) �@ IsPopupOpen* . . �5�      �� $+ 
PART_Popup��6%    � �� . 6O   E 5�      �� 5�   "   8�� 62   F 5�      �� 5�   "   8��	 62   5�      P� 5�      ) �@ 	PopupMode$* 	MouseOver=�67   . �5�      �� $+ 
PART_Popup��6%    � �� . 6O   �� 5�      $+ PART_Border��6%    � y�$ Visible=�6T   5      5     5     �� 5  
    �@ ToggleContent8 
 6*   G 5  
   P� 5     ) �@ UnfurlOrientation$* 
Horizontal=�63   . �5     ��  �@^�	Placement$ Custom=�6=   5	     5       5     $
H Plus=�5     $��24��6   $��24��6*   ��66   ��  �� *FindAncestor+$ wpf:PopupBoxL�$ 
Foreground�5     