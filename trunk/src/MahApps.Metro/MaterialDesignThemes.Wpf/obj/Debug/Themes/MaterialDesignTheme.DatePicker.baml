   M S B A M L   `   `   `  ����W  SMaterialDesignThemes.Wpf, Version=1.2.0.33441, Culture=neutral, PublicKeyToken=nullC&clr-namespace:MaterialDesignThemes.WpfMaterialDesignThemes.Wpf  5      Y1clr-namespace:MaterialDesignThemes.Wpf.Converters#MaterialDesignThemes.Wpf.Converters  5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089  �� MergedDictionaries �� SourceJ    DMaterialDesignThemes.Wpf.Converters.TextFieldHintVisibilityConverter $   TextFieldHintVisibilityConverter # MaterialDesignDatePickerTextBox �� 
TargetType:  4System.Windows.Controls.Primitives.DatePickerTextBox �� Property �� Value �� RelativeSource �� AncestorType �� Path.   (MaterialDesignThemes.Wpf.TextFieldAssist �� 
TargetType	 � 	Resources  Key
 �� Color'  !System.Windows.VisualStateManager   VisualStateGroups%  System.Windows.VisualStateGroup   Transitions%  System.Windows.VisualTransition   GeneratedDuration	   To   States   System.Windows.VisualState   
Storyboard g� Duration	 W� To �� TargetProperty �� 
TargetName �Name(   "MaterialDesignThemes.Wpf.SmartHint  Name   UseFloating   Hint   	HintProxyB   <MaterialDesignThemes.Wpf.Converters.HintProxyFabricConverter   Instance �� 	Converter   HintOpacity ��Name J� IsHitTestVisible J� Opacity  ��Name! �� Triggers" ]� 
Conditions# �� 
SourceName$ �� Property% �� Value& �� 
TargetName  PrimaryHueMidBrush , (MaterialDesignDatePickerCalendarPortrait&	   System.Windows.Controls.Calendar  MaterialDesignBody  MaterialDesignPaper # MaterialDesignCalendarDayButton   MaterialDesignCalendarButton' J� 	CacheMode 	 MaterialDesignShadowDepth4( J� Effect) �� Padding5
  /System.Windows.Controls.Primitives.CalendarItem* 
 Name &
 "MaterialDesignCalendarItemPortrait  MaterialDesignDatePicker(  "System.Windows.Controls.DatePicker  MaterialDesignTextBoxBorder ) %MaterialDesignValidationErrorTemplate+ ��Name, J� SnapsToDevicePixels- � ColumnDefinitions. ��Name  DropDownButtonTemplate/  Name0 �Name1 � AllowsTransparencyA   ;MaterialDesignThemes.Wpf.CustomPopupPlacementCallbackHelper2   LargePopupCallback#3 � CustomPopupPlacementCallback4 � 	StaysOpen5 � PlacementTarget6 �� ElementName(   "MaterialDesignThemes.Wpf.Underline7  Name8 P� Property9 P� Value  ValidationErrorBrush ( $MaterialDesignFloatingHintDatePicker: �� BasedOn)   #MaterialDesignThemes.Wpf.HintAssist�� 5      D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    :x,http://schemas.microsoft.com/winfx/2006/xaml     5      .wpf&clr-namespace:MaterialDesignThemes.Wpf  5      @
converters1clr-namespace:MaterialDesignThemes.Wpf.Converters  5        5      �� 5   
   $' !MaterialDesignTheme.Calendar.xaml?�6   �� 5   
   $6 0MaterialDesignTheme.ValidationErrorTemplate.xaml?�6   %*  &	        &	      &	 ?    7  7  &	 J
    7  7  7  &	 �    7     5      62   �� 5      "  64   .�5      ��  � �� 6!   ��  �� " �$ (TextElement.Foreground)��� 5       �@ TextBoxViewMargin$ 0 0 0 0q�6<   �� 5       � ��
 ���� 6!   �� 5       �@��
CaretBrush 6!   ��  �� *Self+$ BorderBrush��� 5       �@��PanningMode$ VerticalFirst=�6/   �� 5       �@��IsFlicksEnabled .  6-   �� 5       � �� 5      �� 5      "  6   .R�5      � 	 5      ���� 5   	   WatermarkBrush #
 z��60   5   	    5        5   	   CommonStates�� 5   
     5      $ 0D�6     5      $ 0:0:0.1D�6   $ 	MouseOver��68   . 5!   
     
Normal��6     5"   
   	MouseOver��6   5#   
     5$   	   WatermarkStates�� 5%   
     5&      $ 0D�6   . 5(   
     Unwatermarked��6     5)   
   Watermarked��. 5*      �� .��5+      W� $ 0D�6   $	 .23X�6)   $ Opacity�62   $ PART_Watermark��6V   5,      5-      5.   
     5/   	   FocusStates�� 50   
     51      $ 0D�6   . 53   
     	Unfocused��6     54   
   Focused��6   55   
   .+�57      � $ WatermarkContent��6   #��z��69   #��z��6|   .+�58        $
 Hint��6,   #��z��59   ,    5:   ,   �� $! (wpf:HintAssist.IsFloating)� �� *TemplatedParent+ 5;   ,   �� $ (wpf:HintAssist.Hint)� �� *TemplatedParent+ 5<   ,   ��  �� *TemplatedParent+# Z  5=   ,   �� $" (wpf:HintAssist.HintOpacity)� �� *TemplatedParent+�� 5>   	   $ PART_Watermark��6   |�.   .  $ 0Q�6[   $y�	Collapsed=�6g   �� 5?   	   $  PART_ContentHost��6   #��z��60   $��0q�6z   #��z��6�   5@   	   5A      ! 5B      ]� 5C      " 5D      �� 5E   "   $
# Hint��6,   $ �@ IsContentNullOrEmpty% .  6^   �� 5F   "   $ �@J�IsKeyboardFocused% . 6I   ./�5H      �� $
& Hint��6%   # �  6M    � ���� 5I      $
& Hint��6%    �@ HintOpacity$ 1Q�6N   5J      5L      5N      5O      �� 5Q      " 	 6=   .�5R      ��  � ��# �  6!   �� 5S       � ��# �  6!   �� 5T       �@	 CalendarDayButtonStyle8   6-   �� 5U       �@	 CalendarButtonStyle8  6*   �� 5V       � �� 5W      �� 5X      " 	 6   .R�5Y      �� ' 6'   ��  �� *Self+$" (wpf:ShadowAssist.CacheMode)�.u�5Z      �� #( � 	 6!   $) 
16 8 16 24q�6W   ._�5[      
  $* PART_CalendarItem��6   #��z��68   #��z��6d   #��� 
 6�   5\      5]      5^      5`      5a      �� 5c      "  6*   .�5d      ��  � �� 6!   ��  �� " �$ (TextElement.Foreground)��� 5e       � ��# �  6"   �� 5f   
    � ��
 ���� 6'   �� 5g   
    �@ CalendarStyle8   6*   �� 5h       �@ IsTodayHighlighted . 6)   �� 5i       �@ SelectedDateFormat$ Short=�6)   �� 5j       � ��$ 0 0 0 1q�6&   �� 5k   
    � ��$ Stretch=�67   �� 5l   
    � ��$ 0 0 0 0q�6#   �� 5m   
    �@=�ErrorTemplate8  65   �� 5n   
    � �� 5o      �� 5p      "  6   .R�5q      � .+�5r      �� $+ border��5u   !   #��z��5r   !   #��z��5s   !   #��z��5t   !   $) 0 4 0 4q�5v   !   , .  5x        5y   "   CommonStates��. 5z   &     
Normal��62     5{   &   Disabled��. 5|   *   �� .��5}   .   W� $ 0D�6>   $	 .56X�6K   $ Opacity�6T   $ 	PART_Root��6x   5~   +   5   '   5�   #   ._�5�      � $ 	PART_Root��6#   #��z��5�   #   #��z��5�   #   
������ 	 5�   "   ���� 5�   &   DropDownButtonTemplate " ��6U   .R�5�   *   � 
������  5�   .     5�   2   CommonStates�� 5�   6     5�   :   $ 0D�6K     5�   :   $ 0:0:0.1D�6K   $ 	MouseOver��6g     5�   :   $ 0:0:0.1D�6K   $ Pressed��6g   . 5�   6     
Normal��6B     5�   6   	MouseOver��6B     5�   6   Pressed��6B     5�   6   Disabled��6B   5�   3   .+�5�   .   (� .��5�   2   �� $��24��69   $��24��6D   .+�5�   6   F� �������!��!�@KL ��!�@KL  z ���! z � $� @B � $� ��- � z ��- � z @B р�[ @B р�[ ��- �@KL ��- �P[; ��- ��- P[; ��- @KL ���- ��!�@KL @o@ �� �� ���!@o@�@o@��! �� �� �@o@@KL �@o@P[; ��2��- ��!��- р���- р�@B �@f � � �  � � � @f�@f@f�@f � 	#��z��6�   5�   3   5�   /   5�   +   5�   '   5�   #   - 5�   "   �� 5�   &   $��* �67   �� 5�   &   $
��Auto �67   .+�5�   "   �� $. PART_Button��6)   $��1��5�   )   #��z��5�   )   |�.  $��Right=�5�   )   $��4 0 0 -3q�5�   )   $��0��5�   )   8�� $��0q�5�   \   $��17��5�        5�   "   $/ PART_TextBox��64   $��0��5�   4   #|�z|�5�   4   $��Stretch=�5�   4   $��0��5�   4   $��Center=�5�   4   #���  5�   4   $��Stretch=�5�      � 5�   "   $0 
PART_Popup��6(   1 . $��Custom=�5�   (   #3 Z2 5�   (   4 .  $
��Fade=�5�      5 5�   (   �� $6 PART_TextBox��5�      5�      5�        5�      $7 	Underline��6(   y�6;   �� $0 *(wpf:TextFieldAssist.DecorationVisibility)� �� *TemplatedParent+5�      ! 5�      P� 5�      8 � {�9 .  . �5�   	   �� $& border��6#    �@J�Opacity$
 0.56Q�67   5�   	   P� 5�      8 �@J�IsMouseOver9 . . �5�   	   �� # �  6'    � ��5�   	   P� 5�      8 �@J�IsKeyboardFocusWithin9 . . �5�      �� $& 	Underline��6%    �@ IsActive . 6P   �� 5�   	   # �  6(    � ��5�   	   P� 5�      8 �@=�HasError9 . . �5�   	   �� # �  6'    � ���� 5�      $& 	Underline��6%   # �  6R    � ��5�   	   5�      5�      5�      �� 5�      "  69   8:   .�5�   
   ��  �@ 
IsFloating . 66   5�      5�      