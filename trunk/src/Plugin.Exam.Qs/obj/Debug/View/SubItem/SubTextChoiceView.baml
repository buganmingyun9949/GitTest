   M S B A M L   `   `   `  ����I  EPlugin.Exam.Qs, Version=1.3.2.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:Plugin.Exam.Qs.ViewPlugin.Exam.Qs.View  5      D @ST.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullF1clr-namespace:ST.Common.Domain;assembly=ST.CommonST.Common.Domain 5      3    -Plugin.Exam.Qs.View.SubItem.SubTextChoiceView   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      3local!clr-namespace:Plugin.Exam.Qs.View     5      Ddomain1clr-namespace:ST.Common.Domain;assembly=ST.Common     5      $	��780��5      $��Microsoft YaHei)�5	   7     � 	Resources  5       �� MergedDictionaries �� Source%  ST.Common.Domain.ValueConverter   valueConverter�� 5   
    5      �� 5      $G Apack://application:,,,/ST.Style;component/ControlEx/SliderEx.xaml?�6%   5      �� 5      $G Apack://application:,,,/ST.Style;component/ControlEx/ButtonEx.xaml?�6%   6r   �� 5      $J Dpack://application:,,,/ST.Style;component/ControlEx/ExamStyleEx.xaml?�6%   6u   �� 5      $L Fpack://application:,,,/ST.Style;component/ControlEx/DialogStyleEx.xaml?�6%   6w   %   &	          5      6$   5      .��5      � 
�������$��0 4 0 4q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   $	��120��6*   6<   �� 5      $��48 �6   6*   �� 5      $��48 �6   6*   �� 5      $��48 �6   6*   .+�5   
   �� $��0��6     OptionTitleStyle#���  5      $	��640��6G   $��	90 10 0 0q�6S   $��WrapWithOverflow=�5        �� 
LineHeight$ 32��6=   ��5   !   �� *QsItemContent+ �� FallbackValue$" 1、选择题第一小题？��5!      � 5"   
   $��1��6   $
��Left=�6   $��Center=�67   $��
120 0 10 0q�6R   .+�5#      	�   OptionRadioButtonStyle#���  5$   "   $��24(�5&   #   ��5#      �� *QsItemOptionA+$ 
A. 选项A�� d� 	IsChecked 5%   "   ��  �� Path$ CurrentOption� �� Mode$ TwoWay=�	 �� 	Converter#	 [  
 �� ConverterParameter$	
 1-1��5'      5+      � 5,   
   $��2��6   $
��Left=�6   $��Center=�67   $��
120 0 10 0q�6R   .+�5-      	� #���  5.   "   $��24(�50   #   ��5-      �� *QsItemOptionB+$ 
B. 选项B�� 5/   "   �� $ CurrentOption�$ TwoWay=�#	 [  $	
 1-2��51      55      � 56   
   $��3��6   $
��Left=�6   $��Center=�67   $��
120 0 10 0q�6R   .+�57      	� #���  58   "   $��24(�5:   #   ��57      �� *QsItemOptionC+$ 
C. 选项C�� 59   "   �� $ CurrentOption�$ TwoWay=�#	 [  $	
 1-3��5;      5<      5=      5>      