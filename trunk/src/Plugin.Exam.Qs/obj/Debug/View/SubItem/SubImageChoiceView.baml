   M S B A M L   `   `   `  ����I  EPlugin.Exam.Qs, Version=1.3.2.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:Plugin.Exam.Qs.ViewPlugin.Exam.Qs.View  5      D @ST.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullF1clr-namespace:ST.Common.Domain;assembly=ST.CommonST.Common.Domain 5      4    .Plugin.Exam.Qs.View.SubItem.SubImageChoiceView   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      3local!clr-namespace:Plugin.Exam.Qs.View     5      Ddomain1clr-namespace:ST.Common.Domain;assembly=ST.Common     5      $	��780��5      $��Microsoft YaHei)�5	   7     � 	Resources  5       �� MergedDictionaries �� Source%  ST.Common.Domain.ValueConverter   valueConverter�� 5   
    5      �� 5      $G Apack://application:,,,/ST.Style;component/ControlEx/SliderEx.xaml?�6%   5      �� 5      $G Apack://application:,,,/ST.Style;component/ControlEx/ButtonEx.xaml?�6%   6r   �� 5      $J Dpack://application:,,,/ST.Style;component/ControlEx/ExamStyleEx.xaml?�6%   6u   �� 5      $L Fpack://application:,,,/ST.Style;component/ControlEx/DialogStyleEx.xaml?�6%   6w   %   &	          5      6$   5      .��5      � 
�������$��0 4 0 20q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   $	��120��6*   6<   �� 5      $
��Auto �6   6,   .+�5   
   �� $��0��6     OptionTitleStyle#���  5      $	��640��6G   $��	90 10 0 0q�6S   $��WrapWithOverflow=�5       �� 
LineHeight$ 32��6=   ��5   !   �� *QsItemContent+ �� FallbackValue$" 1、选择题第一小题？��5      �� 5!   
   $��1��6   $��	90 0 10 0q�6"   .+�5"      � $
��Left=�6   $��Center=�6.   $��
20 10 10 0q�6I   .+�5$      	�   OptionRadioButtonStyle#���  5%      $��24(�5'      ��5$      �� *QsItemOptionA+$ ../../Resources/nopic.png�� d� 	IsChecked 5&      ��  �� Path$ CurrentOption� �� Mode$ TwoWay=�	 �� 	Converter#	 [  
 �� ConverterParameter$	
 1-1����5(      �� 5)      .R�5*      � $��Center=�6(   $��80��6C   .+�5+   "   �� $��A.��6,   $��Center=�66   6S   �� 5,   "   #��z��6(   $��Center=�6K   $��72��6f   $	��120��6u   $��Uniform=�6�   6�   5-      5.      50      54      � 55      $
��Left=�6   $��Center=�6.   $��
20 10 10 0q�6I   .+�56      	� #���  57   "   $��24(�59   #   ��56      �� *QsItemOptionB+$ 
B. 选项B�� 58   "   �� $ CurrentOption�$ TwoWay=�#	 [  $	
 1-2����5:      �� 5;      .R�5<      � $��Center=�6(   $��80��6C   .+�5=   "   �� $��B.��6,   $��Center=�66   6S   �� 5>   "   #��z��6(   $��Center=�6K   $��72��6f   $	��120��6u   $��Uniform=�6�   6�   5?      5@      5B      5F      � 5G      $
��Left=�6   $��Center=�6.   $��
20 10 10 0q�6I   .+�5H      	� #���  5I   "   $��24(�5K   #   ��5H      �� *QsItemOptionC+$ 
C. 选项C�� 5J   "   �� $ CurrentOption�$ TwoWay=�#	 [  $	
 1-3����5L      �� 5M      .R�5N      � $��Center=�6(   $��80��6C   .+�5O   "   �� $��C.��6,   $��Center=�66   6S   �� 5Q   "   #��z��6(   $��Center=�6K   $��72��6f   $	��120��6u   $��Uniform=�6�   6�   5R      5S      5U      5V      5W      5X      5Y      