   M S B A M L   `   `   `  ����W  SMaterialDesignThemes.Wpf, Version=1.2.0.33441, Culture=neutral, PublicKeyToken=nullY1clr-namespace:MaterialDesignThemes.Wpf.Converters#MaterialDesignThemes.Wpf.Converters  5      O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem 5      C&clr-namespace:MaterialDesignThemes.WpfMaterialDesignThemes.Wpf  5      C&clr-namespace:MaterialDesignThemes.WpfMaterialDesignThemes.Wpf  5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089  �� MergedDictionaries �� SourceJ    DMaterialDesignThemes.Wpf.Converters.TextFieldHintVisibilityConverter $   TextFieldHintVisibilityConverter    IsNotEmptyValue7   1MaterialDesignThemes.Wpf.Converters.MathConverter  MathAddConverter   	Operation=   7MaterialDesignThemes.Wpf.Converters.BrushRoundConverter  BrushRoundConverter  PopupContentPresenterExtend  PopupTopBottomMargin  PopupLeftRightMargin  	TrueValue  
FalseValue  FocusVisual �� Property �� Value J� SnapsToDevicePixels �� StrokeDashArray 	 PopupContentUpTemplate �� 
TargetType	 �� ElementName
 �� Path �� 	Converter �� ConverterParameter � RowDefinitions 
 MaterialDesignShadowBrush �� CornerRadius J� Effect-  'System.Windows.Media.Effects.BlurEffect   Radius � ColumnDefinitions �� RelativeSource �� AncestorType,   &MaterialDesignThemes.Wpf.ComboBoxPopup  PopupContentDownTemplate  PopupContentClassicTemplate ) %MaterialDesignComboBoxEditableTextBox �� 
TargetType ��Name & "MaterialDesignComboBoxItemTemplate �Name'  !System.Windows.VisualStateManager   VisualStateGroups%  System.Windows.VisualStateGroup   Name   Transitions%  System.Windows.VisualTransition   GeneratedDuration	   To   GeneratedEasingFunction/  )System.Windows.Media.Animation.CircleEase7	  1System.Windows.Media.Animation.EasingFunctionBase 	  
EasingMode   States 
  System.Windows.VisualState 
  Name  
  
Storyboard! g� Duration" �� 
TargetName# �� TargetProperty	$ W� To% ��Name& J� Opacity' �� 	Converter( J� RenderTransformOrigin%   MaterialDesignThemes.Wpf.Ripple)   Feedback* �� Triggers+ P� Property, P� Value- �� 
TargetName. ]� 
Conditions/ �� Property0 �� Value # MaterialDesignComboBoxItemStyle 4 0MaterialDesignComboBoxItemSelectedCollapsedStyle1 �� BasedOn & "MaterialDesignComboBoxToggleButton2 F�Name3 �� Stretch4 ^� 
Conditions5 �� Binding  PrimaryHueMidBrush  PrimaryHueDarkBrush " MaterialDesignCheckBoxDisabled . *MaterialDesignFloatingHintComboBoxTemplate6 d�Name7 d� 	IsChecked8 �� Mode9 � UseLayoutRounding: ��Name; �� SelectionBoxItem#< �� SelectionBoxItemStringFormat= �� ContentStringFormat> �� SelectionBoxItemTemplate? J� IsHitTestVisible@ ��NameA �� 
IsReadOnlyB �� 
CaretBrush(   "MaterialDesignThemes.Wpf.SmartHintC  Name)   #MaterialDesignThemes.Wpf.HintAssistD   HintE   HintF   	HintProxyB   <MaterialDesignThemes.Wpf.Converters.HintProxyFabricConverterG   InstanceH   UseFloatingI   HintOpacity(   "MaterialDesignThemes.Wpf.UnderlineJ  NameK   IsActiveL  NameM � AllowsTransparencyN � HorizontalOffsetO � VerticalOffsetP   DefaultVerticalOffsetQ   DownVerticalOffsetR   UpVerticalOffsetS   UpContentTemplateT   DownContentTemplateU   ClassicContentTemplateV � PlacementTargetW   ClassicModeX �� MaxDropDownHeightY ��Name-  'System.Windows.Input.KeyboardNavigationZ   DirectionalNavigation[ P� 
SourceName5   /MaterialDesignThemes.Wpf.ComboBoxPopupPlacement\   Classic] �� 
SourceName  ValidationErrorBrush  MaterialDesignComboBox  MaterialDesignTextBoxBorder ) %MaterialDesignValidationErrorTemplate.   (MaterialDesignThemes.Wpf.TextFieldAssist^ �� Triggers-   'MaterialDesignThemes.Wpf.ComboBoxAssist & "MaterialDesignFloatingHintComboBox�� 5      D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    :x,http://schemas.microsoft.com/winfx/2006/xaml     5      @
converters1clr-namespace:MaterialDesignThemes.Wpf.Converters  5      1system&clr-namespace:System;assembly=mscorlib  5      .wpf&clr-namespace:MaterialDesignThemes.Wpf  5      9materialdesign&clr-namespace:MaterialDesignThemes.Wpf  5        5      �� 5	   
   $g apack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Shadows.xaml?�6   �� 5
   
   $w qpack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.ValidationErrorTemplate.xaml?�6   %B<  &	        &	 #     &	 @     &	 S     &	 l     &	 �     &	 �     &	 �     &	 �     &		 �    7  7  7  7  7  7  &	 �    7  7  7  7  7  7  &	 �    7  7  7  &	 �    &	 �    7  7  7  &	 x    7  7  &	 X    7  &	 �    &	 |"    7  7  7 	 7  7  7  &	 �5    7  7  7  7  7  7  &	 �:    7     5      $ 	Collapsed=�6[     5      $	 Add=�68     5      6%   X�5      4Q�6;   X�5      8Q�64   X�5      16Q�65   ��5      True��6-   ��5      	False��6/   �� 5      .�5   
   ��  � �� 5      �� 5      .R�5      �� $��2 2 2 2q�6     . #��� ��5       $	 1 2R�5       $��1��5       5       5"      5#      �� 5%      " ��65   .R�5&   
   � $��6q�5'      ��5&      �� $	 templateRoot��$
 ActualWidth�8   $ 32�� 5(      �� 5)      $��* �6    .+�5+      �� 
������ #��� 
 5,      $��1q�5-      $ 2��5.       5/        50      $ 6Q�6!   52      � 53      $��1q�6    54      �� 55      $
��Auto �6$   �� 56      $��* �6$   �� 57      $
��Auto �6$   �� 58      $
��Auto �6$   �� 59      $
��Auto �6$   .+�5;      �� $��0��6   $ 2 2 0 0��5<      8�� ��5=      �� $	 
PART_Popup��$
 
Background�5>      �� 5?      $��1��6#   �� 5@      $��2��6   8�� ��5A      �� $	 
PART_Popup��$
 
Background�5B      � 5D      $��3��6    5E      �� 5F      $
��Auto �6+   �� 5G      $
��Auto �6+   �� 5H      $��* �6+   .+�5J      �� $��0��6   8�� ��5L      �� $	 
PART_Popup��$
 
Background�� 5M      $��1��6   ��5N      ��  �� "  $
 VisiblePlacementWidth���5O      �� $	 templateRoot��$
 ActualHeight��� 5P      $��2��6   8�� ��5R      �� $	 
PART_Popup��$
 
Background�5S      �� 5U      $��4��6   $ 0 0 2 2��5V      8�� ��5X      �� $	 
PART_Popup��$
 
Background�5Y      5Z      5[      �� 5]      " ��67   .R�5^   
   � $��6q�5_      ��5^      �� $	 templateRoot��$
 ActualWidth�8   $ 32�� 5`      �� 5a      $��* �6    .+�5c      �� 
������ #��� 
 5d      $��1q�5e      $ 2��5f       5g        5h      $ 6Q�6!   5j      � 5k      $��1q�6    .  5m      �� 5n      $
��Auto �6$   �� 5o      $
��Auto �6$   �� 5p      $
��Auto �6$   �� 5q      $��* �6$   �� 5r      $
��Auto �6$   .+�5t      �� $��0��6   $ 2 2 0 0��5u      8�� ��5v      �� $	 
PART_Popup��$
 
Background�5w      � 5y      $��1��6    5z      �� 5{      $
��Auto �6+   �� 5|      $
��Auto �6+   �� 5}      $��* �6+   .+�5      �� $��0��6   8�� ��5�      �� $	 
PART_Popup��$
 
Background�� 5�      $��1��6   ��5�      ��  �� "  $
 VisiblePlacementWidth���5�      �� $	 templateRoot��$
 ActualHeight��� 5�      $��2��6   8�� ��5�      �� $	 
PART_Popup��$
 
Background�5�      �� 5�      $��2��6   8�� ��5�      �� $	 
PART_Popup��$
 
Background�5�      �� 5�      $��3��6#   �� 5�      $��4��6   $ 0 0 2 2��5�      8�� ��5�      �� $	 
PART_Popup��$
 
Background�5�      5�      5�      �� 5�      " ��6:   .R�5�   
   � $��6q�5�      ��5�      �� $	 templateRoot��$
 ActualWidth�8   $ 32�� 5�      �� 5�      $��* �6    .+�5�      �� 
������ #��� 
 5�      $��1q�5�      $ 2��5�       5�        5�      $ 6Q�6!   5�      � 5�      $��1q�6    5�      �� 5�      $
��Auto �6$   �� 5�      $��* �6$   �� 5�      $
��Auto �6$   .+�5�      �� $��0��6   $ 2 2 0 0��5�      8�� ��5�      �� $	 
PART_Popup��$
 
Background�5�      �� 5�      $��1��6#   �� 5�      $��2��6   $ 0 0 2 2��5�      8�� ��5�      �� $	 
PART_Popup��$
 
Background�5�      5�      5�      �� 5�      " ��6:   .�5�   
   ��  �@�OverridesDefaultStyle . 62   �� 5�   
    �@J�	AllowDrop . 6&   �� 5�   
    � ��$ 0��6%   �� 5�   
    � ��$ 0��6&   �� 5�   
    �@�FocusVisualStyle 6-   Z� �� 5�   
    �@��PanningMode$ VerticalFirst=�65   �� 5�   
    �@��IsFlicksEnabled .  63   �� 5�   
    � �� 5�      �� 5�      " ��6"   .R�5�      � .+�5�      �� $ PART_ContentHost��6'   |�.  $��Hidden=�5�   '   $��Hidden=�5�   '   5�      5�      5�      5�      �� 5�      " ��6A   .R�5�   
   � $ GridWrapper��6    5�        5�      $ CommonStates��6#    5�        5�      $ 0:0:0.3D�6+   $ Normal��6G    5�        5�   "   $ EaseOut=�6-   5�      . 5�      
  $ Normal��6"   
  5�      $ 	MouseOver��6"   .  5�      �� .��5�      W� $! 0D�6.   $" MouseOverBorder��5�   .   $# Opacity�5�   .   $	$ 0.1X�5�   .   5�      5�      5�        5�      $ SelectionStates��6#    5�        5�      $ 0:0:0.3D�6+   . 5�      
  $ Selected��6"   .  5�      �� .��5�      W� $! 0D�6.   $" SelectedBorder��5�   .   $# Opacity�5�   .   $
$ 0.18X�5�   .   5�      5�      
  5�      $ 
Unselected��6"   5�        5�      $ FocusStates��6#    5�        5�      $ 0:0:0.3D�6+   $ 	Unfocused��6G   . 5�      
  $ Focused��6"   .  5�      �� .��5�      W� $! 0D�6.   $" MouseOverBorder��5�   .   $# Opacity�5�   .   $	$ 0.1X�5�   .   5      5     
  5     $ 	Unfocused��6"   5     .+�5     �� $% 
BackBorder��6   #��z��5     #��z��5     #��z��5      . 5	     �� 5     $% MouseOverBorder��6   #��z��5     #��z��5     $& 0Q�5      . ��5     �� *
Foreground+8'   5     �� 5     $% SelectedBorder��6   $& 0Q�5     $( 0.5,0.5&�5     ��5     �� *
Foreground+8'  5       5     $��Stretch=�6   $��Stretch=�5     #��z��5     #��z��5     #��z��5     #��z��5     #��z��5     |�.  #��z��5!     # z 5"     ) 5     �� *
Foreground+8'  5"     5#     * 5$  
   P� 5%     + � {�, .  . �5&     �� $- GridWrapper��6    �@J�Opacity$
 0.56Q�6E   5'     ]� 5(     . 5)     �� 5*     / �@�Tag$	0 1.0��6/   �� 5+     / �@��
IsSelected0 . 66   ./�5-     ��  � ��$ 0��6+   5.     50     �� 52     " ��64   .�53  
   ��  �@J�SnapsToDevicePixels . 60   �� 54  
    � ��$
 16 8q�6$   �� 55  
    � �� 67   �� *HorizontalContentAlignment+ �� " ���� 56  
    � �� 65   �� *VerticalContentAlignment+ �� " ���� 57  
    � ��
 ���� 6'   �� 58  
    � ��
 ���� 6(   �� 59  
    � ��$ 0q�6,   �� 5:  
    �@�FocusVisualStyle8   6-   �� 5;  
    � ��8  6%   5<     �� 5>     81   " ��5@     .�5A  
   ��  �@�Tag$	 1.0��6    5B     �� 5D     " d�67   .�5E  
   ��  � �� 6'   ��  �� " �$
 (TextElement.Foreground)��� 5F  
    �@�OverridesDefaultStyle . 62   �� 5G  
    � �� .  6&   �� 5H  
    � |� .  6&   �� 5I  
    �@��	ClickMode$ Press=�6&   �� 5J  
    � �� 5K     �� 5L     " d�6"   .R�5M     � .+�5N     �� $% ToggleTemplateRoot��6!   #��z��5O  !   #��z��5P  !   #��z��5Q  !   ._�5R     �� $% splitBorder��6%   $��0q�5S  %   $��Right=�5T  %   $��Center=�5U  %   
������ $��0q�5W  %   ._�5X  "   F� $2 arrow��6'   $��8��5Y  /   $��8��69   $��0q�5Z  /   $3 Uniform=�5[  /   $��Right=�5\  /   $��Center=�5]  /   +������j ��� � � ��� �@f��� ���j ��� 	#��z��5_  /   5`     5a     5b     * 5c     ^� 5d     4 5e     �� 5f  "   5 6,   �� *IsMouseOver+ �� *Self+0 true6r   �� 5g  "   5 6,   �� *
IsEditable+ �� " ��	0 false6�   5i     ^� 5j     4 5k     �� 5l  "   5 6,   �� *IsMouseOver+ �� *Self+0 true6r   �� 5m  "   5 6,   �� *
IsEditable+ �� " ��0 true6�   .0�5o     �� # �  6<    � ��5p     P� 5q     + � ��, . . �5r     �� $- arrow��6%   # �  6H    � ��5s     P� 5t     + � {�, .  . �5u     �� $- arrow��6%   # �  6H    � ��5v     ^� 5w     4 5x     �� 5y  "   5 6,   �� *	IsEnabled+ �� *Self+	0 false6p   �� 5z  "   5 6,   �� *
IsEditable+ �� " ��	0 false6�   .0�5|     �� $- ToggleTemplateRoot��6%   # �  6\    � ��5}     ^� 5~     4 5     �� 5�  "   5 6,   �� *	IsEnabled+ �� *Self+	0 false6p   �� 5�  "   5 6,   �� *
IsEditable+ �� " ��0 true6�   .0�5�     �� $- ToggleTemplateRoot��6%   # �  6\    � ���� 5�     $- splitBorder��6%   # �  6U    � ��5�     5�     5�     5�     �� 5�     " ��6I   .R�5�  
   � $ templateRoot��6   #��z��5�      . .+�5�     � $ 	InnerRoot��6    5�     �� 5�     $��* �6'   �� 5�     $��0 �6'   #��� r�61   .+�5�     d� $6 toggleButton��6   $��2��5�     #��z��5�     #��z��5�     #��z��5�     8��  7 5�     �� *IsDropDownOpen+$8 TwoWay=� �� *TemplatedParent+5�     � 5�     #��z��6   #��z��5�     #��z��5�     # z 5�     #9 z9 5�     .+�5�     � $ 	InputRoot��6   $
��Left=�5�     .+�5�     �� $: contentPresenter��6+   #��z; 5�  +   #= z< 5�  +   #��z> 5�  +   #��z��5�  +   ? .  5�  +   �� 5�     $@ PART_EditableTextBox��6"   $
��Left=�5�  "   $��Stretch=�5�  "   8�� $y�	Collapsed=�5�  "   A 5�  "   �� *
IsReadOnly+ �� *TemplatedParent+B 5�  "   ��  �� *TemplatedParent+$
 BorderBrush�5�  "     5�     $
C Hint��6(   #��z��5�  (   # z 5�  (   #9 z9 5�  (   #E zD 5�  (   F 5�  (   ��  �� *TemplatedParent+# ZG H 5�  (   �� $!
 (wpf:HintAssist.IsFloating)� �� *TemplatedParent+I 5�  (   �� $"
 (wpf:HintAssist.HintOpacity)� �� *TemplatedParent+5�  (   5�     5�       5�     $J 	Underline��6    $��2��5�      K 5�      �� $	 PART_EditableTextBox��$
 IsKeyboardFocused�y�5�      �� $0
 *(wpf:TextFieldAssist.DecorationVisibility)� �� *TemplatedParent+  5�     $L 
PART_Popup��6$   M . |�.  $N -11.5��5�  $    . 9 . $��Custom=�5�  $   $
��Fade=�5�  $   $O 0��5�  $   $P 5Q�5�  $   $Q -15.5Q�5�  $   $R 15Q�5�  $   8S  8T  8U  ��5�  $   �� *IsDropDownOpen+$8 TwoWay=� �� *TemplatedParent+V 5�  $   �� $	 templateRoot��W 5�  $   �� $&
  (wpf:ComboBoxAssist.ClassicMode)� �� *TemplatedParent+.��5�     �� .��5�     �� #��zX 6'   ��5�  '   �� *
Background+$	 
PART_Popup��.��5�     �� $Y ItemsPresenter��6-   $Z 	Contained=�6E   5�     5�     5�     5�     5�     * 5�  
   P� 5�     $[ 
PART_Popup��6   #, Z\ 6H   + �@ PopupPlacement. �5�     �� 8   � ��67   5�     P� 5�     $[ 
PART_Popup��6   + � ��, . . �5�     �� $- templateRoot��6/    6I   �� *
Background+$	 
PART_Popup�� � ��5�     P� 5�     + � {�, .  . �5�     �� $- templateRoot��6    �@J�Opacity$
 0.56Q�6F   5�     P� 5�     + �@��
IsEditable, . . �5�     �� $- PART_EditableTextBox��6    � y�$ Visible=�6Q   �� 5�     $- 	Underline��6    6F   �� $0
 *(wpf:TextFieldAssist.DecorationVisibility)� �� *TemplatedParent+ � y��� 5�     $- contentPresenter��6    � y�$ 	Collapsed=�6M   5�     P� 5�     + �@ 
IsFloating, . . �5�     �� $- 	InnerRoot��6    � ��$ 
0 11.5 0 0q�6B   5�     ]� 5�     . 5�     �� 5�     / �@��
IsEditable0 .  66   �� 5�     / �@��IsDropDownOpen0 . 6:   ./�5�     �� $- 	Underline��6    � y�$ Hidden=�6F   �� 5�     $- toggleButton��6    � ��
 ���� 6J   5�     ]� 5�     . 5�     �� 5�     $
] Hint��6    / �@ IsContentNullOrEmpty0 .  6R   �� 5�     / �@J�IsKeyboardFocused0 . 6=   ./�5�     �� $
- Hint��6   # �  6A    � ���� 5�     $
- Hint��6    �@ HintOpacity$ 1Q�6B   5�     ]� 5�     . 5�     �� 5�     $
] Hint��6    / �@ IsContentNullOrEmpty0 .  6R   �� 5      $] PART_EditableTextBox��6    / �@J�IsKeyboardFocused0 . 6_   ./�5     �� $
- Hint��6   # �  6A    � ���� 5     $
- Hint��6    �@ HintOpacity$ 1Q�6B   5     ]� 5     . 5     �� 5     $
] Hint��6    / �@ IsContentNullOrEmpty0 .  6R   �� 5     / �@��IsDropDownOpen0 . 6:   ./�5
     �� $
- Hint��6   # �  6A    � ���� 5     $
- Hint��6    �@ HintOpacity$ 1Q�6B   5     ]� 5     . 5     �� 5     / �@��
IsGrouping0 . 66   ./�5     ��  � �� .  6B   5     P� 5     + �@J�IsKeyboardFocused, . . �5     �� $- 	Underline��6    �@ IsActive . 6D   5     P� 5     + �@=�HasError, . . �5     �� # �  60    � ���� 5     $- 	Underline��6   # �  6F    � ��5     ]� 5     . 5     �� 5     / �@J�IsMouseOver0 . 67   �� 5     / �@=�HasError0 . 6?   ./�5     �� # �  60    � ���� 5      $- 	Underline��6   # �  6F    � ��5!     ]� 5"     . 5#     �� 5$     / �@J�IsMouseOver0 . 67   �� 5%     / �@=�HasError0 .  6?   ./�5'     �� # �  60    � ���� 5(     $- 	Underline��6   # �  6F    � ��5)     5+     �� 5-     " ��6+   .�5.  
   ��  �@�FocusVisualStyle 6-   Z� �� 5/  
    � ��
 ���� 6'   �� 50  
    � ��# �  6(   �� 51  
    � �� 6'   ��  �� " �$
 (TextElement.Foreground)��� 52  
    � ��$ 0 0 0 1q�6,   �� 53  
    � ��8   6/   �� 54  
    � ��$
 Auto=�6G   �� 55  
    � ��$
 Auto=�6E   �� 56  
    � ��$ 0 6 0 6q�6$   �� 57  
    � ��$	 Top=�65   �� 58  
    � ��$ Stretch=�67   �� 59  
    � ��$ Stretch=�60   �� 5:  
    � ��$ Center=�6.   �� 5;  
    � �� . 6:   �� 5<  
    �@��PanningMode$
 Both=�65   �� 5=  
    �@��IsFlicksEnabled .  63   �� 5>  
    �@=�ErrorTemplate8  65   �� 5?  
    �@ TextBoxViewMargin$ 1 0 1 0q�6B   �� 5@  
    � ��8  6%   ^ 5A  
   P� 5B     + �@J�IsKeyboardFocused, . . �5C     ��  � ��# �  60   5D     P� 5E     + �@J�IsKeyboardFocusWithin, . . �5F     ��  � ��# �  60   5G     P� 5H     + �@��
IsEditable, . . �5I     ��  � �� .  6.   �� 5K      �@ ShowSelectedItem8  6H   5L     P� 5N     + �@ ShowSelectedItem8,  . �5O     ��  � ��8  67   5P     5R     �� 5T     81   " ��5V     .�5W  
   ��  �@ 
IsFloating . 66   5X     5Z     