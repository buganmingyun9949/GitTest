   M S B A M L   `   `   `  ����W  SMaterialDesignThemes.Wpf, Version=1.2.0.33441, Culture=neutral, PublicKeyToken=nullC&clr-namespace:MaterialDesignThemes.WpfMaterialDesignThemes.Wpf  5      Y1clr-namespace:MaterialDesignThemes.Wpf.Converters#MaterialDesignThemes.Wpf.Converters  5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089  �� MergedDictionaries �� SourceJ    DMaterialDesignThemes.Wpf.Converters.TextFieldHintVisibilityConverter $   TextFieldHintVisibilityConverter  MaterialDesignTimePicker �� 
TargetType)   #MaterialDesignThemes.Wpf.TimePicker �� Property �� Value �� RelativeSource �� AncestorType �� Path  MaterialDesignTextBoxBorder)   #MaterialDesignThemes.Wpf.HintAssist  MaterialDesignClock ) %MaterialDesignValidationErrorTemplate �� 
TargetType	 J� 	CacheMode  MaterialDesignShadowDepth4
 J� Effect �� Padding �� ContentStringFormat �� ContentStringFormat � 	Resources  Key ��Name(   "MaterialDesignThemes.Wpf.SmartHint  Name   UseFloating   Hint   	HintProxyB   <MaterialDesignThemes.Wpf.Converters.HintProxyFabricConverter   Instance �� 	Converter   HintOpacity �� Triggers ]� 
Conditions �� 
SourceName �� Property �� Value �� 
TargetName  PrimaryHueMidBrush'  !System.Windows.VisualStateManager   VisualStateGroups%  System.Windows.VisualStateGroup   Transitions%  System.Windows.VisualTransition   GeneratedDuration	    To!   States   System.Windows.VisualState" �Name# ��Name$ J� SnapsToDevicePixels% � ColumnDefinitions& ��Name.	   (MaterialDesignThemes.Wpf.TextFieldAssist' 	  TextBoxViewMargin  TextBoxTemplate( ��Name 	 DropDownButtonTemplate) �Name* � AllowsTransparencyA
   ;MaterialDesignThemes.Wpf.CustomPopupPlacementCallbackHelper+ 
  LargePopupCallback#, � CustomPopupPlacementCallback- � 	StaysOpen. � PlacementTarget/ �� ElementName(   "MaterialDesignThemes.Wpf.Underline0  Name1 P� Property2 P� Value 
 ValidationErrorBrush ( $MaterialDesignFloatingHintTimePicker3 �� BasedOn�� 5      D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    :x,http://schemas.microsoft.com/winfx/2006/xaml     5      .wpf&clr-namespace:MaterialDesignThemes.Wpf  5      @
converters1clr-namespace:MaterialDesignThemes.Wpf.Converters  5        5      �� 5   
   $6 0MaterialDesignTheme.ValidationErrorTemplate.xaml?�6   %  &	        &	      7  7  7 	 &	 �    7     5
      62   �� 5      "  6*   .�5      ��  � ��$	 Top=�6(   �� 5       � �� 6!   ��  �� " �$ (TextElement.Foreground)��� 5       � ��# �  6"   �� 5       � ��
 ���� 6!   �� 5       � ��$ 0 0 0 1q�6&   �� 5       � ��$ Stretch=�61   �� 5   
    � �� .  6&   �� 5   
    � ��$ 0 12 0 0q�6#   �� 5   
    �@ Hint$ Select time��60   �� 5       �@ 
ClockStyle# �  6!   �� 5   
    �@=�ErrorTemplate8   65   �� 5      $ �@ ClockHostContentControlStyle 5      �� 5      " ��6   �5      �� 5       � �� 5   	   �� 5   "   " ��62   .R�5   &   �� 	 67   ��  �� *Self+$" (wpf:ShadowAssist.CacheMode)�.u�5    *   �� #
 �  61   $ 
16 8 16 24q�6g   ._�5!   .   �� #��z��6?   #��z��6r   # z 6�   5"   +   5#   '   5$   #   5&   	   5(      5*      �� 5+       � �� 5,      �� 5-      "  6    5.      ���� 5/      TextBoxTemplate " ��60   .R�50   	   � .+�51   
   �� $ PART_ContentHost��6   |�.  $��Hidden=�6C   $��Hidden=�6j     53   "   $
 Hint��60   #��z��54   0    55   0   �� $! (wpf:HintAssist.IsFloating)� �� *TemplatedParent+ 56   0   �� $ (wpf:HintAssist.Hint)� �� *TemplatedParent+ 57   0   ��  �� *TemplatedParent+# Z  58   0   �� $" (wpf:HintAssist.HintOpacity)� �� *TemplatedParent+59   
    5:      ]� 5;   "    5<   &   �� 5=   *   $
 Hint��64    �@ IsContentNullOrEmpty .  6f   �� 5>   *    �@J�IsKeyboardFocused . 6Q   ./�5@   &   �� $
 Hint��6-   # �  6U    � ���� 5A   &   $
 Hint��6-    �@ HintOpacity$ 1Q�6V   5B   #   5D      �� 5E      DropDownButtonTemplate " ��67   .R�5F   	   � 
������  5G   
     5H      CommonStates�� 5I        5J      $ 0D�6     5K      $ 0:0:0.1D�6   $  	MouseOver��6:     5L      $ 0:0:0.1D�6   $  Pressed��6:   .! 5N        
Normal��6     5O      	MouseOver��6     5P      Pressed��6     5Q      Disabled��6   5R      .+�5T   
   (� .��5U      �� $��24��6   $��24��6   .+�5V      F� ����� �  -1� -1 �   z  z � �  	=   z  z � 	=  �   z  z � �  -1  z  z � � �� ׀�O � ���� ��� � � ��O���� ��� �p�b ��O�� `��  � � � �� ���� ��� � �� ��j � �� �� �@f@�� Q��� fffff&0@��ا @]� ��ا ��j � �� ��j 	#��z��6�   5W      5X      5Y   
   5Z   	   5[      .R�5\      � $" TemplateRoot��6   .+�5]      �� $# border��5`   !   #��z��5]      #��z��5^   !   #��z��5_   !   $ 0 4 0 4q�5a   !   $ . ._�5c      � % 5d      �� 5e      $��* �6   �� 5f      $
��Auto �6   .+�5h      �� $& PART_TextBox��6)   $��0q�6   $' .5 0 0 0q�5i      $��0q�5j      8�� 5k      �� 5m      $( PART_Button��5n      $��1��5m      #��z��6$   |�.  $��Right=�6a   $��4 0 0 -3q�6}   $��0��6�   $��0q�6�   $��17��5o      8�� 5p      � 5q      $) 
PART_Popup��6   * . $��Custom=�5r   (   #, Z+ 5s   (   - .  $
��Fade=�5u      . 5t   (   �� $/ PART_TextBox��5u      5v      5w        5x      $0 	Underline��6(   y�6;   �� $0 *(wpf:TextFieldAssist.DecorationVisibility)� �� *TemplatedParent+5y       5z      P� 5{      1 � {�2 .  . �5|      �� $ TemplateRoot��68    �@J�Opacity$
 0.56Q�6R   5}      P� 5~      1 �@J�IsMouseOver2 . . �5   	   �� # �  6'    � ��5�   	   P� 5�      1 �@J�IsKeyboardFocusWithin2 . . �5�      �� $ 	Underline��69    �@ IsActive . 6P   �� 5�   	   # �  6(    � ��5�   	   P� 5�      1 �@=�HasError2 . . �5�      �� # � 
 6<    � ���� 5�      $ 	Underline��6%   # � 
 6R    � ��5�      5�      5�      5�      �� 5�      "  69   83   .�5�   
   ��  �@ 
IsFloating . 66   5�      5�      