   M S B A M L   `   `   `  ����M  IPlugin.Exam.Report, Version=1.3.1.0, Culture=neutral, PublicKeyToken=nullQ-clr-namespace:Plugin.Exam.Report.View.SubItemPlugin.Exam.Report.View.SubItem  5      D @ST.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullF1clr-namespace:ST.Common.Domain;assembly=ST.CommonST.Common.Domain 5      6    0Plugin.Exam.Report.View.SubItem.SubTextChoice1RV   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      ?local-clr-namespace:Plugin.Exam.Report.View.SubItem     5      W SMaterialDesignThemes.Wpf, Version=1.2.0.33442, Culture=neutral, PublicKeyToken=nullFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5      >domain1clr-namespace:ST.Common.Domain;assembly=ST.Common  5      $	��780��5	         � 	Resources  5       �� MergedDictionaries �� Source1  +ST.Common.Domain.ResultChoiceValueConverter   valueConverter$  ST.Common.Domain.KindConverter  kindConverter/  )ST.Common.Domain.PlayControlIconConverter  playIconConverter/  )ST.Common.Domain.VisibilityValueConverter  visibilityValueConverter�� 5   
    5      �� 5      $J Dpack://application:,,,/ST.Style;component/ControlEx/ExamStyleEx.xaml?�6%   6u   �� 5      $G Apack://application:,,,/ST.Style;component/ControlEx/ButtonEx.xaml?�6%   �� 5      $H Bpack://application:,,,/ST.Style;component/ControlEx/ListBoxEx.xaml?�6%   �� 5      $M Gpack://application:,,,/ST.Style;component/ControlEx/ScrollViewerEx.xaml?�6%   �� 5      $G Apack://application:,,,/ST.Style;component/ControlEx/SliderEx.xaml?�6%   %t   &	        &	      &	 &     &	 9       5      60     5      6#     5      6.     5      6.   5      .��5      � .+�5   
   �� .+�5      �� 	 �� X1$ 0��6   	 �� Y1$ 0��6   	 �� X2$	 760��6!   	 �� Y2$ 0��6*     
GrayBrush5#���  61   $��1��6W   $��Center=�6k   $��	0 10 0 20q�6�   6�   � 5      $��	40 0 60 0q�6   .+�5      ��   OptionTitleStyle0#���  6P   $
��Auto��6|   ��6   �� *
QsTitle1+ �� FallbackValue$  1、选择题第一小题?��6�   �� 5      $
��（ ��6     OptionTitleStyle2#���  6&   6T   �� 5         OptionTitleStyle1#���  6`   ��6   �� *ChoiceUserScore+$ 0 分�� �� Mode$ TwoWay=���6�   �� *ChoiceUserScoreColor+$ TwoWay=�6�   �� 5!      $	�� / ��6   #���  6'   6U   �� 5"      #���  6c   ��6   �� *ChoiceTotalScore+$ 1.5 分��$ TwoWay=�6�   �� 5#      $
�� ）��6   #���  6&   6T   �� 5$      $��	10 -4 0 0q�6   $��choice��6�   
�������$��2q�6�   ���#30cc75���#30cc75$
��Left=�6�     ButtonStyle#���  6  $��96��68  $��36��6C  ��6,   �� *PlayQsAudio+y�6L   �� *ShowPlayBtn+$ 	Collapsed��.��5%      �� 	 �� Orientation$	 
Horizontal=�6!   .+�5&      �� $��播放原文��6$   $��Center=�60   6M   '  !MaterialDesignThemes.Wpf.PackIcon  5'      $��4 0 0 0q�62   �  �ControlzEx.PackIconBase`1[[MaterialDesignThemes.Wpf.PackIconKind, MaterialDesignThemes.Wpf, Version=1.2.0.33442, Culture=neutral, PublicKeyToken=null]]
   Kind
 6C   ��  �� Path$ PlayIcon�$ TwoWay=� �� 	Converter# [ $
 Play�� �� ConverterParameter$ 1-1-1��5(      5)      5*      � 5+      .+�5,      �� $��	40 10 0 0q�6   $	 
Horizontal=�61   .+�5-        $��0 0 5 0q�6[   $��30��6l   $��30��6x   ��6.   �� *OptionPackIconCorrectStyle+
 6�   �� $ OptionStyleA�$ TwoWay=�# [ $ Check����6�   �� $ OptionStyleA�$ TwoWay=�# [  $ #ffffff���� 5.      #���  6W   $��Normal"�6�   ��6    �� *ChoiceTextA+$" A、选择题第一小题？����6�   �� *OptionStyleA+$ TwoWay=�6�   5/      �� 50      $��	40 10 0 0q�6   $	 
Horizontal=�60   .+�51        $��0 0 5 0q�6[   $��30��6l   $��30��6x   ��6.   �� *OptionPackIconCorrectStyle+
 6�   �� $ OptionStyleB�$ TwoWay=�# [ $ Check����6�   �� $ OptionStyleB�$ TwoWay=�# [  $ #ffffff���� 52      #���  6X   $��Normal"�6�   ��6    �� *ChoiceTextB+$" B、选择题第一小题？����6�   �� *OptionStyleB+$ TwoWay=�6�   53      �� 54      $��	40 10 0 0q�6   $	 
Horizontal=�60   y�55      ��  �� ElementName$
 Txt3��$
 Text�# [  �� UpdateSourceTrigger$ PropertyChanged=�$ TwoWay=�.+�56        $��0 0 5 0q�6[   $��30��6l   $��30��6x   ��6.   �� *OptionPackIconCorrectStyle+
 6�   �� $ OptionStyleC�$ TwoWay=�# [ $ Check����6�   �� $ OptionStyleC�$ TwoWay=�# [  $ #FFFFFF���� 57      -   5         ��Name$
 Txt3��57       #���  6�   $��Normal"�6�   ��6.   �� *ChoiceTextC+$" C、选择题第一小题？��$ PropertyChanged=�$ TwoWay=���6�   �� *OptionStyleC+$ TwoWay=�6  58      �� 59      $��	40 10 0 0q�6   $	 
Horizontal=�60   y�5:      �� $
 Txt4��$
 Text�# [ $ PropertyChanged=�$ TwoWay=�.+�5;        $��0 0 5 0q�6[   $��30��6l   $��30��6x   ��6.   �� *OptionPackIconCorrectStyle+
 6�   �� $ OptionStyleD�$ TwoWay=�# [ $ Check����6�   �� $ OptionStyleD�$ TwoWay=�# [  $ #FFFFFF���� 5<      -   5        $
 Txt4��5<       #���  6�   $��Normal"�6�   ��6.   �� *ChoiceTextD+$" D、选择题第一小题？��$ PropertyChanged=�$ TwoWay=���6�   �� *OptionStyleD+$ TwoWay=�6  5=      �� 5>      $��	40 10 0 0q�6   $	 
Horizontal=�60   y�5?      �� $
 Txt5��$
 Text�# [ $ PropertyChanged=�$ TwoWay=�.+�5@        $��0 0 5 0q�6[   $��30��6l   $��30��6x   ��6.   �� *OptionPackIconCorrectStyle+
 6�   �� $ OptionStyleE�$ TwoWay=�# [ $ Check����6�   �� $ OptionStyleE�$ TwoWay=�# [  $ #FFFFFF���� 5A      -   5        $
 Txt5��5A       #���  6�   $��Normal"�6�   ��6.   �� *ChoiceTextE+$" E、选择题第一小题？��$ PropertyChanged=�$ TwoWay=���6�   �� *OptionStyleE+$ TwoWay=�6  5B      �� 5C      $��	40 10 0 0q�6   $	 
Horizontal=�60   y�5D      �� $
 Txt6��$
 Text�# [ $ PropertyChanged=�$ TwoWay=�.+�5E        $��0 0 5 0q�6[   $��30��6l   $��30��6x   ��6.   �� *OptionPackIconCorrectStyle+
 6�   �� $ OptionStyleF�$ TwoWay=�# [ $ Check����6�   �� $ OptionStyleF�$ TwoWay=�# [  $ #FFFFFF���� 5F      -   5        $
 Txt6��5F       #���  6�   $��Normal"�6�   ��6.   �� *ChoiceTextF+$" F、选择题第一小题？��$ PropertyChanged=�$ TwoWay=���6�   �� *OptionStyleF+$ TwoWay=�6  5G      5H      5I      5J      5K      