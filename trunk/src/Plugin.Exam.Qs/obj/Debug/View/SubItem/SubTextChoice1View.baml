   M S B A M L   `   `   `  ����I  EPlugin.Exam.Qs, Version=1.3.2.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:Plugin.Exam.Qs.ViewPlugin.Exam.Qs.View  5      D @ST.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullF1clr-namespace:ST.Common.Domain;assembly=ST.CommonST.Common.Domain 5      4    .Plugin.Exam.Qs.View.SubItem.SubTextChoice1View   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      3local!clr-namespace:Plugin.Exam.Qs.View     5      Ddomain1clr-namespace:ST.Common.Domain;assembly=ST.Common     5      $	��780��5      $��Microsoft YaHei)�5	   7     � 	Resources  5       �� MergedDictionaries �� Source%  ST.Common.Domain.ValueConverter   valueConverter/  )ST.Common.Domain.VisibilityValueConverter  visibilityValueConverter�� 5   
    5      �� 5      $G Apack://application:,,,/ST.Style;component/ControlEx/SliderEx.xaml?�6%   5      �� 5      $G Apack://application:,,,/ST.Style;component/ControlEx/ButtonEx.xaml?�6%   6r   �� 5      $J Dpack://application:,,,/ST.Style;component/ControlEx/ExamStyleEx.xaml?�6%   6u   �� 5      $L Fpack://application:,,,/ST.Style;component/ControlEx/DialogStyleEx.xaml?�6%   6w   %:   &	        &	        5      6$     5      6.   5      .��5      � 
�������$��0 4 0 4q�6   .+�5   
   � $��
90 10 10 0q�6   .+�5      ��   OptionTitleStyle#���  5      $��42��6G   $��0 0 0 0q�6R   $��WrapWithOverflow=�5        �� 
LineHeight$ 32��6=   ��5      �� *QsItemContent+ �� FallbackValue$
 1、��5!      � 5"      $��0 0 10 0q�6   .+�5#      � $
��Left=�6   $��Center=�62   $��	20 0 10 0q�6M   .+�5$      	�   OptionRadioButtonStyle#���  5%   "   $��24(�5'   #   ��5$   "   �� *QsItemOptionA+$ 
A. 选项A�� d� 	IsChecked 5&   "   ��  �� Path$ CurrentOption� �� Mode$ TwoWay=� �� 	Converter# [  	 �� ConverterParameter$		 1-1��5(      5,      � 5-      $
��Left=�6   $��Center=�62   $��	20 0 10 0q�6M   .+�5.      	� #���  5/   "   $��24(�51   #   ��5.   "   �� *QsItemOptionB+$ 
B. 选项B�� 50   "   �� $ CurrentOption�$ TwoWay=�# [  $		 1-2��52      56      � 57      $
��Left=�6   $��Center=�62   $��	20 0 10 0q�6M   .+�58      	� #���  59   "   $��24(�5<   #   ��58   "   �� *QsItemOptionC+$ 
C. 选项C��
 �� UpdateSourceTrigger$
 PropertyChanged=�$ TwoWay=� 5:   "   �� $ CurrentOption�$ TwoWay=�# [  $		 1-3��y�5;   "   ��  �� RelativeSource �� *Self+$ Content�# [ $
 PropertyChanged=�$ TwoWay=�5=      5>      � 5?      $
��Left=�6   $��Center=�62   $��	20 0 10 0q�6M   .+�5@      	� #���  5A      $��24(�5D      ��5@   "   �� *QsItemOptionD+$ 
D. 选项D��$
 PropertyChanged=�$ TwoWay=� 5B      �� $ CurrentOption�$ TwoWay=�# [  $		 1-4��y�5C      ��  �� *Self+$ Content�# [ $
 PropertyChanged=�$ TwoWay=�5E      5F      � 5G      $
��Left=�6   $��Center=�62   $��	20 0 10 0q�6M   .+�5H      	� #���  5I      $��24(�5L      ��5H   "   �� *QsItemOptionE+$ 
E. 选项E��$
 PropertyChanged=�$ TwoWay=� 5J      �� $ CurrentOption�$ TwoWay=�# [  $		 1-5��y�5K      ��  �� *Self+$ Content�# [ $
 PropertyChanged=�$ TwoWay=�5M      5N      � 5O      $
��Left=�6   $��Center=�62   $��	20 0 10 0q�6M   .+�5P      	� #���  5Q      $��24(�5T      ��5P   "   �� *QsItemOptionF+$ 
F. 选项F��$
 PropertyChanged=�$ TwoWay=� 5R      �� $ CurrentOption�$ TwoWay=�# [  $		 1-6��y�5S      ��  �� *Self+$ Content�# [ $
 PropertyChanged=�$ TwoWay=�5U      5V      5W      5X      5Y      5Z      