   M S B A M L   `   `   `  ����W  SMaterialDesignThemes.Wpf, Version=1.2.0.33441, Culture=neutral, PublicKeyToken=nullC&clr-namespace:MaterialDesignThemes.WpfMaterialDesignThemes.Wpf  5      Y1clr-namespace:MaterialDesignThemes.Wpf.Converters#MaterialDesignThemes.Wpf.Converters  5      O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem 5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089  �� MergedDictionaries �� SourceG    AMaterialDesignThemes.Wpf.Converters.NullableToVisibilityConverter !  NullableToVisibilityConverter?   9MaterialDesignThemes.Wpf.Converters.MathMultipleConverter  MathMultipleConverter   	Operation & "MaterialDesignSnackbarActionButton �� 
TargetType �� Property �� Value  SecondaryAccentBrush  FocusVisual+   %MaterialDesignThemes.Wpf.RippleAssist   MaterialDesignSnackbarRipple �� 
TargetType �Name%   MaterialDesignThemes.Wpf.Ripple J� SnapsToDevicePixels	 �� Triggers
 P� Property P� Value # MaterialDesignSnackbarMouseOver + 'MaterialDesignSnackbarActionLightButton �� BasedOn  PrimaryHueLightBrush )	 %MaterialDesignSnackbarActionMidButton 
 PrimaryHueMidBrush * &MaterialDesignSnackbarActionDarkButton  PrimaryHueDarkBrush.   (MaterialDesignThemes.Wpf.SnackbarMessage � ColumnDefinitions �� ContentStringFormat �� ContentStringFormat � 	Resources �� DataType   MaterialDesignBody1TextBlock ��Name   ActionCommand   ActionCommandParameter   ActionContent   ActionContentTemplate    ActionContentStringFormat$   ActionContentTemplateSelector �� 	Converter �� RelativeSource �� AncestorType'   !MaterialDesignThemes.Wpf.Snackbar �� Path  MaterialDesignPaper $  MaterialDesignSnackbarBackground � 	Resources  Key g� Duration �� 
TargetName  �� TargetProperty! W� From	" W� To# W� EasingFunction-  'System.Windows.Media.Animation.SineEase7  1System.Windows.Media.Animation.EasingFunctionBase$   
EasingMode% g� 	BeginTime& ��Name
' � Tag( `� 	Converter) �� ElementName* ��Name+ �� CornerRadius, ��Name-   Message. N� EnterActions  ActivateStoryboard/ N� ExitActions  DeactivateStoryboard0 �� 
TargetName1 �� Triggers2 X� MaxValue�� 5      D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    :x,http://schemas.microsoft.com/winfx/2006/xaml     5      .wpf&clr-namespace:MaterialDesignThemes.Wpf  5      @
converters1clr-namespace:MaterialDesignThemes.Wpf.Converters  5      1system&clr-namespace:System;assembly=mscorlib  5        5      �� 5   
   $f `pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Button.xaml?�6   �� 5	   
   $i cpack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.TextBlock.xaml?�6   %�  &	        &	      &	 5     7  &	 A    7  &		 �    7  &	 �    7  '  "    7  7   '  	    7  7  7  7     5      6/     5      $ Multiply=�6E   �� 5      " ��67   .�5   
   ��  � ��$ Stretch=�6.   �� 5   
    � ��# �  6'   �� 5   
    �@�FocusVisualStyle8   6-   �� 5   
    � ��
 ���� 6'   �� 5   
    � ��
 ���� 6(   �� 5   
    �@�Cursor$
 Hand��6#   �� 5   
    �@ Feedback# �  66   �� 5   
    � ��$ Medium"�61   �� 5   
    � ��$ 14(�6/   �� 5   
    � ��$ 1q�6,   �� 5   
    � ��$ Center=�67   �� 5   
    � ��$ Center=�65   �� 5   
    � ��$ 	24 0 24 0q�6$   �� 5   
    � ��$ 24 -14 -24 -14q�6#   �� 5   
    � �� 5      �� 5       " ��6"   .R�5!      � $
 Root��6   #��z��6)   .+�5"        #��z��6%   #��z��6I   |�.  #��z��5#   %   #��z��5$   %   #��z��5%   %   # z 5&   %   5'      	 5(      P� 5)      
 �@J�IsMouseOver . . �5*      �� # �  6;    � ��5+      P� 5,      
 � {� .  . �5-      ��  �@J�Opacity$
 0.23Q�68   5.      50      52      53      �� 55      " ��6<   8   .�56   
   ��  � ��# �  6'   57      �� 59      " ��6:   8   .�5:   
   ��  � ��# � 
 6'   5;      �� 5=      " ��6;   8   .�5>   
   ��  � ��# �  6'   5?      �� 5A      "  6   .�5B   
   ��  � ��$
 Left=�67   �� 5C   
    � �� 5D      �� 5E      "  6"   .R�5F      �  5G      �� 5H      $��* �6/   �� 5I      $
��Auto �6/   .+�5K      �� #��z��6+   #��z��5L   +   # z 5M   +   #��z��5N   +   #��z��5O   +   $
��Left=�5P   +   $��36��5Q   +    5R      ���� 5S   "   " ��6/   (��       *,��+).R�5T   &   �� ��60   �� ��5U   *   �� 5V   .   " ��64   8   .�5W   2   ��  � ��$ 14(�6M   �� 5X   2    � ��$ WrapWithOverflow=�6Q   �� 5Y   2    � ��$ CharacterEllipsis=�6Q   5Z   /   5\   '   5]   #   5^      5_      �� 5a      $ PART_ActionButton��6!   $��1��5b   !   #��z 5c   !   #��z 5d   !   #��z 5e   !   #��z 5f   !   # z 5g   !   #��z 5h   !   $��Right=�5i   !   $��Right=�5l   !   y�5j   !   �� *ActionContent+8  ��5k   !   ��  �� *FindAncestor+"  $ ActionButtonStyle�5l   !   5m      5n      5p      5q      �� 5s      "  6   .�5t   
   ��  � ��$ Bottom=�6.   �� 5u   
    � ��$ Center=�65   �� 5v   
    � ��$ Center=�60   �� 5w   
    � ��$ 14(�6%   �� 5x   
    � ��# �  6'   �� 5y   
    � ��# �  6'   �� 5z   
    �@ ActionButtonStyle8   6.   �� 5{   
    � ��$	 288��6%   �� 5|   
    � ��$	 568��6%   �� 5}   
    � ��$ 24 14 24 14q�6$   �� 5~   
    � }� . 6)   �� 5   
    � �� 5�      �� 5�      "  6"    5�      ���� 5�      ActivateStoryboard $ 0:0:0.3D�6@   .��5�      W� $
 Root��6.   $	  Tag�6K   $! 0X�6k   $" 1X�6t   $ 0:0:0.3D�6{   # 5�   "     5�   &   $$ EaseOut=�6/   5�      W� 5�      $ ContentPresenter��6.   $  Opacity�6W   $" 0X�6{   $% 0X�6�   $ 0D�6�   W� 5�      $ ContentPresenter��6.   $  Opacity�6W   $! 0X�6{   $" 1X�6�   $% 	0:0:0.075X�6�   $ 	0:0:0.225D�6�   # 5�   "     5�   &   $$ EaseOut=�6/   5�      5�      �� 5�      DeactivateStoryboard $ 0:0:0.3D�6B   .��5�      W� $
 Root��6.   $	  Tag�6K   $! 1X�6k   $" 0X�6t   $ 0:0:0.3D�6{   # 5�   "     5�   &   $$ EaseOut=�6/   5�      5�      5�      .R�5�      �� $
& Root��6!   ' 5�      X�5�      0.0Q�61   ��5�      `� 5�      8(  .1�5�   "   �� $) ContentBorder��6*   $ ActualHeight�6F   �� 5�   "   $	 Tag�6Q    6*   �� *Self+6Q   5�      .+�5�      �� $* ContentBorder��6!   #��z��5�   !   $+ 2��6K    . $��Stretch=�5�   !   $��24��5�   !   ._�5�      �� $, ContentPresenter��5�   /   #��z- 5�   /   #��z��5�   /   #��z��5�   /   5�   /   5�      5�      	 5�      P� 5�      
 �@ IsActive . . 5�      �� 5�   "   8�� 62   / 5�      �� 5�   "   8�� 62   5�      P� 5�      
 � ��$ Stretch=�6A   . �5�      �� $0 ContentBorder��6%    �@��CornerRadius$ 0��6X   5�      5�      5�      1 5�   
   P� 5�      
 � ��$ Stretch=�65   . �5�      ��  � ��# Z2 6-   5�      5�      5�      5�      