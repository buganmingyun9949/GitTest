   M S B A M L   `   `   `  ����W  SMaterialDesignThemes.Wpf, Version=1.2.0.33441, Culture=neutral, PublicKeyToken=nullC&clr-namespace:MaterialDesignThemes.WpfMaterialDesignThemes.Wpf  5      Y1clr-namespace:MaterialDesignThemes.Wpf.Converters#MaterialDesignThemes.Wpf.Converters  5      O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem 5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089?    9MaterialDesignThemes.Wpf.Converters.MathMultipleConverter   MathMlpMultipleConverter     	Operation & "MaterialDesignExpanderToggleButton �� 
TargetType �� Property �� Value �� 
TargetType'  !System.Windows.VisualStateManager   VisualStateGroups%  System.Windows.VisualStateGroup   Transitions%  System.Windows.VisualTransition   From	   To	   GeneratedEasingFunction0  *System.Windows.Media.Animation.QuarticEase7  1System.Windows.Media.Animation.EasingFunctionBase
   
EasingMode   
Storyboard �� TargetProperty �� 
TargetName9  3System.Windows.Media.Animation.EasingDoubleKeyFrame O� KeyTime O� Value   GeneratedDuration   States   System.Windows.VisualState   
Storyboard'   !MaterialDesignThemes.Wpf.PackIcon  Name�	   �ControlzEx.PackIconBase`1[[MaterialDesignThemes.Wpf.PackIconKind, MaterialDesignThemes.Wpf, Version=1.2.0.33441, Culture=neutral, PublicKeyToken=null]] 	  Kind J� Opacity J� RenderTransformOrigin * &MaterialDesignExpanderRightHeaderStyle �� Padding � RowDefinitions d� 	IsChecked �� Path �� Mode �� RelativeSource �� Angle ) %MaterialDesignExpanderLeftHeaderStyle ) %MaterialDesignExpanderDownHeaderStyle � ColumnDefinitions �� ContentStringFormat  �� ContentStringFormat ' #MaterialDesignExpanderUpHeaderStyle  MaterialDesignExpander  MaterialDesignPaper! �� AncestorType.
  (System.Windows.Media.Animation.CubicEase" R� KeyTime  System.Windows.Visibility#   Visible$ R� Value	% W� To& g� Duration'   	Collapsed(   Hidden) � Cursor* �� HeaderStringFormat+ `� 	Converter, �� ElementName- �� Triggers. P� Property/ P� Value0 �� 
TargetName�� 5      D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    :x,http://schemas.microsoft.com/winfx/2006/xaml     5      .wpf&clr-namespace:MaterialDesignThemes.Wpf  5      @
converters1clr-namespace:MaterialDesignThemes.Wpf.Converters  5      1system&clr-namespace:System;assembly=mscorlib  5      %$  &	        &	 "     &	 �    7  &	 �    7  &	 c    7  &	 �    7  &	 �    7  7   7   7  7  7  7     5      $  Multiply=�6H   �� 5      " d�67   .�5   
   ��  � �� 5      �� 5      " d�6"   .R�5      �� 
������  5        5      CheckStates�� 5   "     5   &   $ 	Unchecked��67   $ Checked��6H   	 5   *     5   .   $
 EaseOut=�6:   . 5   *   �� .��5   .   U� $V P(UIElement.RenderTransform).(TransformGroup.Children)[2].(RotateTransform.Angle)�6L   $ 
ExpandPath��6�   .\�5   2     $ 0��6G   $ 0Q�6S     5   2   $ 0:0:0.2��6G   $
 -180Q�6Y   5   /   5   +   5   '     5   &   $ 0D�67   $ 	Unchecked��6M   . 5   *   �� .��5    .   U� $V P(UIElement.RenderTransform).(TransformGroup.Children)[2].(RotateTransform.Angle)�6L   $ 
ExpandPath��6�   .\�5!   2     $ 0��6G   $
 -180Q�6S   5"   /   5#   +   5$   '     5%   &   $ 0D�67   $ Checked��6M   . 5&   *   �� .��5'   .   U� $V P(UIElement.RenderTransform).(TransformGroup.Children)[2].(RotateTransform.Angle)�6L   $ 
ExpandPath��6�   .\�5(   2     $ 0��6G   $ 0Q�6S   5)   /   5*   +   5+   '     5,   &   $ Checked��67   $ 0D�6F   $ 	Unchecked��6\   	 5-   *     5.   .   $
 EaseOut=�6:   . 50   *   �� .��51   .   U� $V P(UIElement.RenderTransform).(TransformGroup.Children)[2].(RotateTransform.Angle)�6L   $ 
ExpandPath��6�   .\�52   2     $ 0��6G   $
 -180Q�6S     53   2   $ 0:0:0.2��6G   $ 0Q�6Y   54   /   55   +   56   '   . 58   "     Checked��. 59   &   �� .��5:   *   U� $V P(UIElement.RenderTransform).(TransformGroup.Children)[2].(RotateTransform.Angle)�6H   $ 
ExpandPath��6�   .\�5;   .     $ 0:0:0.2��6C   $
 -180Q�6U   5<   +   5=   '   5>   #     5?   "   	Unchecked��. 5@   &   �� .��5A   *   U� $V P(UIElement.RenderTransform).(TransformGroup.Children)[2].(RotateTransform.Angle)�6H   $ 
ExpandPath��6�   .\�5B   .     $ 0:0:0.2��6C   $ 0Q�6U   5C   +   5D   '   5E   #   5F      ._�5H        $ 
ExpandPath��6H   $ ChevronDown=�6'   $	 .38Q�6:   $ 0.5,0.5&�6\   $��24��5I   '   $��24��63   #��z��6>   z�5J      V� 5K   "   .~�5L   &   �� �� 5M   &   �� 5N   &   T� 5O   &   5P   #   5R      5S      5T      5V      5W      �� 5Y      " d�6;   .�5Z   
   ��  � ��
 ���� 6'   �� 5[   
    � ��$
 Left=�60   �� 5\   
    � ��$ Stretch=�6.   �� 5]   
    � �� 5^      �� 5_      " d�6"   .R�5`      �� #��z��6   $ 12,24,12,24q�6G   ._�5a      �  5b      �� 5c   "   $
��Auto �60   �� 5d   "   .+�5f      d� $��0��6+   $��Center=�5g   '   #��z��5h   '   8��  $ 0.5,0.5&�5k   '    5i   '   �� $ 	IsChecked�$ TwoWay=� �� *TemplatedParent+z�5l   "   �� 5m   &   $	 -90Q�66   5o      �� 5p      $��1��6/   $��Center=�5q   +   $��0,16,0,0q�5r   +   #��z��5s   +   $	��Top=�6O   5t      5u      5v      5x      5y      �� 5{      " d�6:   .�5|   
   ��  � ��
 ���� 6'   �� 5}   
    � ��$ Right=�60   �� 5~   
    � ��$ Stretch=�6.   �� 5   
    � �� 5�      �� 5�      " d�6"   .R�5�      �� #��z��6   $ 12,24,12,24q�6G   ._�5�      �  5�      �� 5�   "   $
��Auto �60   �� 5�   "   .+�5�      d� $��0��6+   $��Center=�5�   '   #��z��5�   '   8��  $ 0.5,0.5&�5�   '    5�   '   �� $ 	IsChecked�$ TwoWay=� �� *TemplatedParent+z�5�   "   �� 5�   &   $ 90Q�66   5�      �� 5�      $��1��6/   $��Center=�5�   +   $��0,16,0,0q�5�   +   #��z��5�   +   $	��Top=�6O   5�      5�      5�      5�      5�      �� 5�      " d�6:   .�5�   
   ��  � ��
 ���� 6'   �� 5�   
    � ��$ Stretch=�60   �� 5�   
    � ��$ Center=�6.   �� 5�   
    � �� 5�      �� 5�      " d�6"   .R�5�      �� #��z��6   $ 24,12,24,12q�6G   ._�5�      �  5�      �� 5�   "   �� 5�   "   $
��Auto �63   .+�5�      �� #��z��6/   #��z��5�   /   #  z 5�   /   #��z��5�   /   $��Center=�5�   /   $��0,0,16,0q�5�   /   d� 5�      $��1��6+   $��Center=�5�   +   #��z��5�   +   8��   5�   +   �� $ 	IsChecked�$ TwoWay=� �� *TemplatedParent+5�   +   5�      5�      5�      5�      5�      �� 5�      " d�68   .�5�   
   ��  � ��
 ���� 6'   �� 5�   
    � ��$ Stretch=�60   �� 5�   
    � ��$ Center=�6.   �� 5�   
    � �� 5�      �� 5�      " d�6"   .R�5�      �� #��z��6   $ 24,12,24,12q�6G   ._�5�      �  5�      �� 5�   "   �� 5�   "   $
��Auto �63   .+�5�      �� $��Center=�6/   #��z��5�   +   $��0,0,16,0q�5�   +   d� 5�      $��Center=�5�   +   #��z��5�   +   8��  $ 0.5,0.5&�5�   +   $��1��5�   +    5�   +   �� $ 	IsChecked�$ TwoWay=� �� *TemplatedParent+z�5�   "   �� 5�   &   $
 -180Q�66   5�      5�      5�      5�      5�      5�      �� 5�      " 9�6+   .�5�   
   ��  � ��$ 0q�6,   �� 5�   
    � ��# �  6'   �� 5�   
    � �� 6'   ��  �� "! �$ (TextElement.Foreground)��� 5�   
    � ��$ Stretch=�67   �� 5�   
    � ��$	 Top=�65   �� 5�   
    � �� 5�      �� 5�      " 9�6"   .R�5�      �� #��z��6   #��z��6Q    5�        5�      ExpansionStates�� 5�   "     5�   &   $ 0D�67   $ Expanded��6M   	 5�   *   
  5�   .   $
 EaseOut=�68   . 5�   *   �� .��5�   .   T� $ PART_Content��6L   $ 
Visibility�6q   ..�5�   2   i� $" 0��6I   #$ Z# 6U   5�   /   W� 5�   .   $ Opacity�6>   $ PART_Content��6b   $% 1X�6�   $& 0:0:0.3D�6�   5�   +   5�   '     5�   &   $ 0D�67   $ 	Collapsed��6M   	 5�   *   
  5�   .   $
 EaseOut=�68   . 5�   *   �� .��5�   .   T� $ PART_Content��6L   $ 
Visibility�6q   ..�5�   2   i� $" 0:0:0.3��6I   #$ Z' 6[   5�   /   W� 5�   .   $ Opacity�6>   $ PART_Content��6b   $% 0X�6�   $& 0:0:0.3D�6�   5�   +   5�   '   . 5  "     Expanded��. 5  &   �� .��5  *   W� $ Opacity�6:   $ PART_Content��6^   $% 1X�6�   $& 0D�6�   T� 5  *   $ PART_Content��6H   $ 
Visibility�6m   ..�5  .   i� $" 0��6E   #$ Z# 6Q   5  +   5  '   5  #     5	  "   	Collapsed��. 5
  &   �� .��5  *   W� $ Opacity�6:   $ PART_Content��6^   $% 0X�6�   $& 0D�6�   T� 5  *   $ PART_Content��6H   $ 
Visibility�6m   ..�5  .   i� $" 0��6E   #$ Z( 6Q   5  +   5  '   5  #   5     ._�5     ]� #��z��6$   .+�5     d� $��
HeaderSite��6+   $	��Top=�5  /   $��0q�5  /   $
) Hand��6C   8��  $��15(�5  /   $	 .87Q�5  /   #��z��5  /   #��z��5  /   #��z��5  /   #��z��5  /   # z* 5  /    5  /   �� $ 
IsExpanded�$ TwoWay=� �� *TemplatedParent+5  /   �� 5      $��ContentSite��6%   $��Bottom=�5!  )   ._�5"  "   �� $��ContentPanel��6-   #��z��5#  1   #��z��5$  1   #��z��5%  1   ��5&  &   `� 5'  *   8+  .1�5(  .   �� $, PART_Content��66   $ ActualHeight�6Q   �� 5)  .   $, PART_Content��66   $ Opacity�6Q   5*  +   ��5,  &   `� 5-  *   8+  .1�5.  .   �� $, PART_Content��66   $ ActualWidth�6Q   �� 5/  .   $, PART_Content��66   $ Opacity�6Q   50  +   .+�52  &   �� $��PART_Content��67   |�.  #��z��53  ;   #  z 54  ;   #��z��55  ;   56  #   57     58     59     - 5:     P� 5;     . �@9�ExpandDirection$/ Right=�6=   . �5<     �� $0 
HeaderSite��6?    � ��$
 Left=�6W   �� 5=     $0 ContentSite��6?    � ��$ Right=�6X   �� 5>     $0 ContentPanel��6<    �@��Orientation$ 
Horizontal=�6V   �� 5?     $0 ContentPanel��67    � ��$
 Auto��6Q   �� 5@     $0 
HeaderSite��66   8   � ��6N   5A     P� 5C     . �@9�ExpandDirection$
/ Left=�6=   . �5D     �� $0 
HeaderSite��6?    � ��$ Right=�6W   �� 5E     $0 ContentSite��6?    � ��$
 Left=�6X   �� 5F     $0 ContentPanel��6<    �@��Orientation$ 
Horizontal=�6V   �� 5G     $0 ContentPanel��67    � ��$
 Auto��6Q   �� 5H     $0 
HeaderSite��66   8   � ��6N   5I     P� 5K     . �@9�ExpandDirection$/ Up=�6=   . �5L     �� $0 
HeaderSite��6?    � ��$ Bottom=�6W   �� 5M     $0 ContentSite��6?    � ��$	 Top=�6X   �� 5N     $0 ContentPanel��6<    �@��Orientation$ Vertical=�6V   �� 5O     $0 ContentPanel��66    � ��$
 Auto��6P   �� 5P     $0 
HeaderSite��66   8   � ��6N   5Q     P� 5S     . �@9�ExpandDirection$
/ Down=�6=   . �5T     �� $0 
HeaderSite��6?    � ��$	 Top=�6W   �� 5U     $0 ContentSite��6?    � ��$ Bottom=�6X   �� 5V     $0 ContentPanel��6<    �@��Orientation$ Vertical=�6V   �� 5W     $0 ContentPanel��66    � ��$
 Auto��6P   �� 5X     $0 
HeaderSite��66   8   � ��6N   5Y     5[     5]     5^     5`     