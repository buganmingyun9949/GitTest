   M S B A M L   `   `   `  ����W  SMaterialDesignThemes.Wpf, Version=1.2.0.33441, Culture=neutral, PublicKeyToken=nullC&clr-namespace:MaterialDesignThemes.WpfMaterialDesignThemes.Wpf  5      Y1clr-namespace:MaterialDesignThemes.Wpf.Converters#MaterialDesignThemes.Wpf.Converters  5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089  �� MergedDictionaries �� SourceJ    DMaterialDesignThemes.Wpf.Converters.TextFieldHintVisibilityConverter $   TextFieldHintVisibilityConverter  MaterialDesignTextBox �� 
TargetType �� Property �� Value �� RelativeSource �� AncestorType �� Path  MaterialDesignTextBoxBorder.   (MaterialDesignThemes.Wpf.TextFieldAssist  PrimaryHueMidBrush-  'System.Windows.Input.KeyboardNavigation ) %MaterialDesignValidationErrorTemplate m� Command	 m� Icon'   !MaterialDesignThemes.Wpf.PackIcon�   �ControlzEx.PackIconBase`1[[MaterialDesignThemes.Wpf.PackIconKind, MaterialDesignThemes.Wpf, Version=1.2.0.33441, Culture=neutral, PublicKeyToken=null]]
   Kind �� 
TargetType ��Name J� SnapsToDevicePixels �� Padding �Name ��Name � UseLayoutRounding(   "MaterialDesignThemes.Wpf.SmartHint  Name   Hint   	HintProxyB   <MaterialDesignThemes.Wpf.Converters.HintProxyFabricConverter   Instance �� 	Converter   HintOpacity   UseFloating   FloatingScale   FloatingOffset(   "MaterialDesignThemes.Wpf.Underline  Name �� Triggers ]� 
Conditions �� 
SourceName �� Property  �� Value! �� 
TargetName)   #MaterialDesignThemes.Wpf.HintAssist" P� Property# P� Value ( $MaterialDesignTextFieldBoxBackground   MaterialDesignTextAreaBorder 0 ,MaterialDesignTextFieldBoxDisabledBackground ( $MaterialDesignTextAreaInactiveBorder 	 ValidationErrorBrush %
 !MaterialDesignFloatingHintTextBox$ �� BasedOn % !MaterialDesignTextFieldBoxTextBox ! MaterialDesignTextAreaTextBox  MaterialDesignTextBox1'	  !System.Windows.VisualStateManager% 	  VisualStateGroups%
  System.Windows.VisualStateGroup& 
  States   System.Windows.VisualState'   
Storyboard( �� TargetProperty) �� 
TargetName* W� From	+ W� To, g� Duration- W� EasingFunction-  'System.Windows.Media.Animation.SineEase7  1System.Windows.Media.Animation.EasingFunctionBase.   
EasingMode/ g� 	BeginTime - )MaterialDesignTextFieldBoxHoverBackground0 J� RenderTransformOrigin1 �� CornerRadius   BooleanToVisibilityConverter2 �� ScaleX3 �� ScaleY4 ��Name5 �� StrokeDashArray6 �� StrokeDashCap	7 �� X1	8 �� Y1	9 �� Y2: J� Opacity	; �� X2< �� ElementName=   CornerRadius + 'MaterialDesignOutlinedTextFieldTextBox1�� 5      D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    :x,http://schemas.microsoft.com/winfx/2006/xaml     5      .wpf&clr-namespace:MaterialDesignThemes.Wpf  5      @
converters1clr-namespace:MaterialDesignThemes.Wpf.Converters  5        5      �� 5   
   $6 0MaterialDesignTheme.ValidationErrorTemplate.xaml?�6   %�>  &	        &	      7  &	
 �    7  &	 �    7 
 &	 E    7 
 &	 �    7  7  &	 =    7     5
      62   �� 5      " ��6*   .�5   
   ��  � �� 6'   ��  �� " �$ (TextElement.Foreground)��� 5   
    � ��# �  6(   �� 5   
    � ��$ 0 0 0 1q�6,   �� 5   
    �@ TextBoxViewMargin$ 0 0 0 0q�6B   �� 5   
    � ��
 ���� 6'   �� 5   
    �@��
CaretBrush# �  6'   �� 5   
    �@ TabNavigation$
 None=�6=   �� 5   
    � ��$ Stretch=�67   �� 5   
    � ��$ Bottom=�65   �� 5   
    �@�FocusVisualStyle 6-   Z� �� 5   
    �@J�	AllowDrop . 6&   �� 5   
    �@��PanningMode$ VerticalFirst=�65   �� 5   
    �@��IsFlicksEnabled .  63   �� 5   
    �@=�ErrorTemplate8   65   �� 5   
   " �@ IncludeSpellingSuggestions 6K   ��  �� *Self+$ (SpellCheck.IsEnabled)��� 5   
    �@�ContextMenu 5      �� 5      .@�5       m� $
��_Cut��6   $	 Cut��6-   	 5!        5"      $
 
ContentCut=�6+   5$      m� 5%      $��_Copy��6   $
 Copy��6.   	 5&        5'      $
 ContentCopy=�6+   5)      m� 5*      $��_Paste��6   $ Paste��6/   	 5+        5,      $
 ContentPaste=�6+   5.      5/      51      �� 52   
    � �� 53      �� 54      " ��6"   .R�55      �� $ textFieldBoxBorder��6    . ._�57      � .+�58      �� $ border��6%   #��z��59   %   #��z��5:   %   #��z��5;   %    . $ 0 4 0 4q�5=   %   ._�5>   "   � $ textFieldGrid��6'   #��z��5?   '   #��z��5@   '   #��z��5A   '   .+�5B   &   �� $ PART_ContentHost��63   |�.  $��Hidden=�5C   3   $��Hidden=�6Z   # z 5D   3   # z 5E   3     5G   &   $
 Hint��64   #��z��5J   4   #��z��5K   4    5H   4   �� $ (wpf:HintAssist.Hint)� �� *TemplatedParent+ 5I   4   ��  �� *TemplatedParent+# Z  5L   4   �� $" (wpf:HintAssist.HintOpacity)� �� *TemplatedParent+ 5M   4   �� $! (wpf:HintAssist.IsFloating)� �� *TemplatedParent+ 5N   4   �� $$ (wpf:HintAssist.FloatingScale)� �� *TemplatedParent+ 5O   4   �� $% (wpf:HintAssist.FloatingOffset)� �� *TemplatedParent+5Q   #   5R        5S      $ 	Underline��6,   y�6?   �� $0 *(wpf:TextFieldAssist.DecorationVisibility)� �� *TemplatedParent+5T      5U       5V      ]� 5W       5X      �� 5Y   "   $
 Hint��6,    �@ IsContentNullOrEmpty  .  6^   �� 5Z   "    �@J�IsKeyboardFocused  . 6I   ./�5\      �� $
! Hint��6%   # �  6M    � ���� 5]      $
! Hint��6%    �@ HintOpacity$ 1Q�6N   5^      P� 5_      " �@ 
IsFloating# . . �5`      �� $! border��6%    � ��$ 0 12 0 0q�6K   5a      P� 5b      " �@ HasTextFieldBox# . . �5c      ��  � ��$	 Top=�6I   �� 5d      $! textFieldBoxBorder��6%   # �  6[    � ���� 5e      $! textFieldBoxBorder��6%    �@��CornerRadius$ 4��6]   �� 5f      $! textFieldBoxBorder��6%    �@��Padding$ 0,8,0,0q�6X   �� 5g      $! textFieldGrid��6%    � ��$ 	16,0,16,0q�6R   �� 5h      $! border��6%    � ��$ 0q�6T   �� 5i      $! 	Underline��6%    �@ CornerRadius$ 0,0,4,4��6T   �� 5j      $
! Hint��6%    � ��$ 0,0,0,16q�6I   �� 5k      $! PART_ContentHost��6%    � ��$ 0,8,0,8q�6U   5l      P� 5m      " �@ HasTextAreaBox# . . �5n      ��  � ��$	 Top=�6I   �� 5o      $! textFieldBoxBorder��6%    � ��
 ���� 6[   �� 5p      $! textFieldBoxBorder��6%    �@��CornerRadius$ 4��6]   �� 5q      $! textFieldBoxBorder��6%    � ��$ 1q�6`   �� 5r      $! textFieldBoxBorder��6%   # �  6\    � ���� 5s      $! textFieldBoxBorder��6%    �@��Padding$ 0,8,0,0q�6X   �� 5t      $! textFieldBoxBorder��6%    � ��$ -1q�6W   �� 5u      $! textFieldGrid��6%    � ��$ 	16,0,16,0q�6R   �� 5v      $! border��6%    � ��$ 0q�6T   �� 5w      $! 	Underline��6%    � y�$ 	Collapsed=�6R   �� 5x      $
! Hint��6%    � ��$ 0,0,0,16q�6I   �� 5y      $! PART_ContentHost��6%    � ��$ 0,8,0,8q�6U   5z      P� 5{      " � {�# .  . �5|      �� $! border��68    �@J�Opacity$
 0.56Q�6L   �� 5}      $! textFieldBoxBorder��6%   # �  6[    � ���� 5~      $! textFieldBoxBorder��6%   # �  6\    � ��5      P� 5�      " �@J�IsMouseOver# . . �5�      �� # �  6<    � ��5�      P� 5�      " �@J�IsKeyboardFocused# . . �5�      �� $! 	Underline��6%    �@ IsActive . 6P   5�      P� 5�      " �@=�HasError# . . �5�      �� # � 	 6<    � ���� 5�      $! 	Underline��6%   # � 	 6R    � ��5�      ]� 5�       5�      �� 5�   "    �@J�IsMouseOver  . 6C   �� 5�   "    �@ HasTextAreaBox  . 6Z   ./�5�      �� $! textFieldBoxBorder��6%    � ��$ -2q�6W   �� 5�      $! textFieldBoxBorder��6%    � ��$ 2q�6`   5�      ]� 5�       5�      �� 5�   "    �@J�IsKeyboardFocused  . 6I   �� 5�   "    �@ HasTextAreaBox  . 6Z   ./�5�      �� $! textFieldBoxBorder��6%    � ��$ -2q�6W   �� 5�      $! textFieldBoxBorder��6%    � ��$ 2q�6`   �� 5�      $! textFieldBoxBorder��6%   # �  6\    � ��5�      ]� 5�       5�      �� 5�   "    �@=�HasError  . 6K   �� 5�   "    �@ HasTextAreaBox  . 6Z   ./�5�      �� $! textFieldBoxBorder��6%    � ��$ -2q�6W   �� 5�      $! textFieldBoxBorder��6%    � ��$ 2q�6`   �� 5�      $! textFieldBoxBorder��6%   # � 	 6\    � ��5�      5�      5�      5�      �� 5�      " ��66   8$   .�5�   
   ��  �@ 
IsFloating . 66   5�      �� 5�      " ��66   8$   .�5�   
   ��  �@ HasTextFieldBox . 6@   5�      �� 5�      " ��62   8$   .�5�   
   ��  �@ HasTextAreaBox . 6?   5�      �� 5�      " ��6+   .�5�   
   ��  � �� 6'   ��  �� " �$ (TextElement.Foreground)��� 5�   
    � ��# �  6(   �� 5�   
    � ��$ 0 0 0 1q�6,   �� 5�   
    �@ TextBoxViewMargin$ 0 0 1 0q�6B   �� 5�   
    � ��
 ���� 6'   �� 5�   
    �@��
CaretBrush# �  6'   �� 5�   
    �@ TabNavigation$
 None=�6=   �� 5�   
    � ��$ Stretch=�67   �� 5�   
    � ��$ Bottom=�65   �� 5�   
    �@�FocusVisualStyle 6-   Z� �� 5�   
    �@J�	AllowDrop . 6&   �� 5�   
    �@��PanningMode$ VerticalFirst=�65   �� 5�   
    �@��IsFlicksEnabled .  63   �� 5�   
    �@=�ErrorTemplate8   65   �� 5�   
   " �@ IncludeSpellingSuggestions 6K   ��  �� *Self+$ (SpellCheck.IsEnabled)��� 5�   
    �@ UnderlineBrush# �  6?   �� 5�   
    �@ 
Foreground# �  66   �� 5�   
    �@�ContextMenu 5�      �� 5�      .@�5�      m� $	 Cut��6   	 5�        5�      $
 
ContentCut=�6+   5�      m� 5�      $
 Copy��6   	 5�        5�      $
 ContentCopy=�6+   5�      m� 5�      $ Paste��6   	 5�        5�      $
 ContentPaste=�6+   5�      5�      5�      �� 5�   
    � �� 5�      �� 5�      " ��6"   .R�5�      � % 5�      
  5�      FocusStates��.& 5�   "     Focused��.' 5�   &   �� .��5�   *   W� $( ScaleX�6:   $) ScaleTransform��6]   $* 0X�6�   $+ 1X�6�   $, 0:0:0.3D�6�   - 5�   .     5�   2   $. EaseOut=�6;   5�   +   W� 5�   *   $( ScaleY�6:   $) ScaleTransform��6]   $* 0X�6�   $+ 1X�6�   $, 0:0:0.3D�6�   - 5�   .     5�   2   $. EaseOut=�6;   5�   +   W� 5�   *   $( ScaleX�6:   $) ScaleTransform��6]   $+ 0X�6�   $/ 0:0:0.45X�6�   $, 0D�6�   W� 5�   *   $( ScaleY�6:   $) ScaleTransform��6]   $+ 0X�6�   $/ 0:0:0.45X�6�   $, 0D�6�   5�   '   5�   #     5�   "   	Unfocused��.' 5�   &   �� .��5�   *   W� $( ScaleX�6:   $) ScaleTransform��6]   $+ 0X�6�   $, 0D�6�   W� 5�   *   $( ScaleY�6:   $) ScaleTransform��6]   $+ 0X�6�   $, 0D�6�   5�   '   5�   #   5�      .+�5     �� $��Stretch=�6!   $��Stretch=�6?   #���  5  !   $0 0.5,0.5&�5  !   1 5  !   �� $1 +(wpf:TextFieldAssist.TextFieldCornerRadius)� �� *TemplatedParent+y�5  !   �� $0 *(wpf:TextFieldAssist.RippleOnFocusEnabled)� �� *TemplatedParent+8  z�5     �� 5  "   ScaleTransform��$2 0Q�6I   $3 0Q�6T   5	     �� 5
     $ textFieldBoxBorder��6!    . 1 5  !   �� $1 +(wpf:TextFieldAssist.TextFieldCornerRadius)� �� *TemplatedParent+._�5     � .+�5  "   �� $ border��6)   
������ $��0q�5  )   #��z��5  )    . $ 0 4 0 4q�5  )   ._�5  &   � $ textFieldGrid��6+   #��z��5  +   #��z��5  +   #��z��5  +   $��1��5  +   .+�5  *   �� $ PART_ContentHost��67   |�.  $��Hidden=�5  7   $��Hidden=�6^   # z 5  7   # z 5  7     5  *   $
 Hint��68   #��z��5!  8   #��z��5"  8    5  8   �� $ (wpf:HintAssist.Hint)� �� *TemplatedParent+ 5   8   ��  �� *TemplatedParent+# Z  5#  8   �� $" (wpf:HintAssist.HintOpacity)� �� *TemplatedParent+ 5$  8   �� $! (wpf:HintAssist.IsFloating)� �� *TemplatedParent+ 5%  8   �� $$ (wpf:HintAssist.FloatingScale)� �� *TemplatedParent+ 5&  8   �� $% (wpf:HintAssist.FloatingOffset)� �� *TemplatedParent+5(  '   5)  #   �� 5*  "   $ textFieldBoxBottomLine��6)   #��z��5+  )   $��0��5,  )   $��Stretch=�5.  )   $��Bottom=�5/  )    . 1 5-  )   �� $1 +(wpf:TextFieldAssist.UnderlineCornerRadius)� �� *TemplatedParent+50  )   �� 51  "   $4 
DashedLine��6'   $��Bottom=�6;   $y�	Collapsed=�6V   $
��1.25��52  '   $5 1,2.5R�6>   $6 Round=�6V   $7 0��53  '   $8 0��6]   $9 0��6d   #��z��54  '   $
: 0.56Q�6N   ; 53  .   �� *ActualWidth+$< border��54  N     55  "   $ 	Underline��60   y�6C   �� $0 *(wpf:TextFieldAssist.DecorationVisibility)� �� *TemplatedParent+= 56  0   �� $1 +(wpf:TextFieldAssist.UnderlineCornerRadius)� �� *TemplatedParent+��57  0   �� $* $(wpf:TextFieldAssist.UnderlineBrush)� �� *TemplatedParent+58     59     5:      5;     ]� 5<      5=     �� 5>  "    �@ 
IsFloating  . 6Q   �� 5?  "    �@J�IsKeyboardFocused  . 6I   ./�5A     �� $
! Hint��6%    6M   �� $! (wpf:HintAssist.Foreground)� �� *TemplatedParent+ � ���� 5B     $
! Hint��6%    �@ HintOpacity$ 1Q�6N   5C     P� 5D     " �@ 
IsFloating# . . �5E     �� $! border��6%    � ��$ 0 12 0 0q�6K   5F     P� 5G     " �@ HasFilledTextField# . . �5H     ��  � ��$	 Top=�6I   �� 5I     $! textFieldBoxBorder��6%   # �  6[    � ���� 5J     $! textFieldBoxBorder��6%    �@��Padding$ 0,8,0,0q�6X   �� 5K     $! textFieldGrid��6%    � ��$ 	16,0,16,0q�6R   �� 5L     $! border��6%    � ��$ 0q�6T   �� 5M     $! border��6%    �@�Cursor$ IBeam��6K   �� 5N     $
! Hint��6%    � ��$ 0,0,0,16q�6I   �� 5O     $! PART_ContentHost��6%    � ��$ 0,8,0,8q�6U   5P     P� 5Q     " �@ HasOutlinedTextField# . . �5R     ��  � ��$	 Top=�6I   �� 5S     $! textFieldBoxBorder��6%    � ��
 ���� 6[   �� 5T     $! textFieldBoxBorder��6%    � ��$ 2q�6`   �� 5U     $! textFieldBoxBorder��6%    � ��
 ���� 6\   �� 5V     $! textFieldBoxBorder��6%    �@��Padding$ 0q�6X   �� 5W     $! textFieldBoxBorder��6%    � ��$ -2q�6W   �� 5X     $! textFieldGrid��6%    � ��$ 	10,0,10,0q�6R   �� 5Y     $! border��6%    � ��$ 0q�6T   �� 5Z     $! border��6%    � ��$ 0q�6T   �� 5[     $! border��6%    �@�Cursor$ IBeam��6K   �� 5\     $! 	Underline��6%    � y�$ 	Collapsed=�6R   �� 5]     $
! Hint��6%    � ��$ 0,0,0,16q�6I   �� 5^     $! PART_ContentHost��6%    � ��$ 0,2,0,8q�6U   5_     P� 5`     " � {�# .  . �5a     �� $! textFieldBoxBottomLine��6%    � ��$ 0��6[   5b     ]� 5c      5d     �� 5e  "    � {�  .  6A   �� 5f  "    �@ HasOutlinedTextField  . 6`   ./�5h     �� $! textFieldBoxBorder��6%   # �  6\    � ��5i     ]� 5j      5k     �� 5l  "    � {�  .  6A   �� 5m  "    �@ HasOutlinedTextField  .  6`   ./�5o     �� $! border��68    �@J�Opacity$
 0.42Q�6L   �� 5p     $! border��6%    � ��
 ���� 6P   5q     ]� 5r      5s     �� 5t  "    � {�  .  6A   �� 5u  "    �@ HasOutlinedTextField  .  6`   �� 5v  "    �@ HasFilledTextField  .  6^   ./�5x     �� $! 
DashedLine��6%    � y�$ Visible=�6S   5y     P� 5z     " �@J�IsKeyboardFocused# . 5|     ]� 5}      5~     �� 5  "    �@J�IsKeyboardFocused  . 6I   �� 5�  "    �@ HasOutlinedTextField  . 6`   ./�5�     �� $! textFieldBoxBorder��6%    � ��$ -2q�6W   �� 5�     $! textFieldBoxBorder��6%    � ��$ 2q�6`   �� 5�     $! textFieldBoxBorder��6%    � ��
 ���� 6\   5�     ]� 5�      5�     �� 5�  "    �@J�IsKeyboardFocused  . 6I   �� 5�  "    �@ HasFilledTextField  . 6^   ./�5�     �� $! textFieldBoxBottomLine��6%    � ��$ 2��6[   5�     ]� 5�      5�     �� 5�  "    �@J�IsMouseOver  . 6C   �� 5�  "    �@ HasFilledTextField  .  6^   �� 5�  "    �@ HasOutlinedTextField  .  6`   �� 5�  "   " �@ NewSpecHighlightingEnabled  .  6f   ./�5�     ��  6<   �� $* $(wpf:TextFieldAssist.UnderlineBrush)� �� *Self+ � ��5�     ]� 5�      5�     �� 5�  "    �@J�IsMouseOver  . 6C   �� 5�  "    �@ HasFilledTextField  .  6^   �� 5�  "    �@ HasOutlinedTextField  .  6`   �� 5�  "   " �@ NewSpecHighlightingEnabled  . 6f   ./�5�     �� $! border��6%    � ��$ 0,0,0,2q�6T   �� 5�     $! border��6%    �@��Padding$ 0,4,0,3q�6L   5�     ]� 5�      5�     �� 5�  "    �@J�IsMouseOver  . 6C   �� 5�  "    �@ HasFilledTextField  . 6^   ./�5�     �� $! textFieldBoxBorder��6%   # �  6[    � ���� 5�     $! textFieldBoxBottomLine��6%    � ��$ 1��6[   5�     ]� 5�      5�     �� 5�  "    �@J�IsMouseOver  . 6C   �� 5�  "    �@ HasOutlinedTextField  . 6`   ./�5�     �� $! textFieldBoxBorder��6%    � ��$ -2q�6W   �� 5�     $! textFieldBoxBorder��6%    � ��$ 2q�6`   �� 5�     $! textFieldBoxBorder��6%    � ��
 ���� 6\   5�     P� 5�     " �@=�HasError# . . �5�     �� # � 	 6<    � ���� 5�     $! 	Underline��6%   # � 	 6R    � ��5�     ]� 5�      5�     �� 5�  "    �@=�HasError  . 6K   �� 5�  "    �@ HasOutlinedTextField  . 6`   ./�5�     �� $! textFieldBoxBorder��6%    � ��$ -2q�6W   �� 5�     $! textFieldBoxBorder��6%    � ��$ 2q�6`   �� 5�     $! textFieldBoxBorder��6%   # � 	 6\    � ��5�     5�     5�     5�     �� 5�     " ��6<   8$   .�5�  
   ��  �@ 
IsFloating . 66   �� 5�  
    �@ HasOutlinedTextField . 6E   �� 5�  
    �@ RippleOnFocusEnabled .  6E   �� 5�  
    �@ TextFieldCornerRadius$ 0��6F   �� 5�  
    �@ UnderlineCornerRadius$ 0��6F   5�     5�     