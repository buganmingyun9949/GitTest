   M S B A M L   `   `   `  ����D  @ST.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullF1clr-namespace:ST.Common.Domain;assembly=ST.CommonST.Common.Domain  5      O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem 5       Personal-App+   %Personal_App.Domain.Exam.ExamResultUC   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      W SMaterialDesignThemes.Wpf, Version=1.2.0.33442, Culture=neutral, PublicKeyToken=nullFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5      >domain1clr-namespace:ST.Common.Domain;assembly=ST.Common  5      3system&clr-namespace:System;assembly=mscorlib  5      -   5        $	��768��5	      $
��1024��6,   $��Microsoft YaHei)�5
   X     � 	Resources  5       �� MergedDictionaries �� Source1   +ST.Common.Domain.ResultChoiceValueConverter   valueConverter$   ST.Common.Domain.KindConverter  kindConverter/   )ST.Common.Domain.PlayControlIconConverter  playIconConverter�� 5   
    5      �� 5      $G Apack://application:,,,/ST.Style;component/ControlEx/ButtonEx.xaml?�6%   �� 5      $H Bpack://application:,,,/ST.Style;component/ControlEx/ListBoxEx.xaml?�6%   �� 5      $G Apack://application:,,,/ST.Style;component/ControlEx/SliderEx.xaml?�6%   �� 5      $M Gpack://application:,,,/ST.Style;component/ControlEx/ScrollViewerEx.xaml?�6%   6x   %W   &	        &	      &	 &       5      60     5      6#     5      6.   5      .��5      � .+�5   
   � 
�������.+�5      �  � RowDefinitions 5      �� 5      $��54 �6$   62   �� 5      $
��600* �6$   64   �� 5      $��20 �6$   62   .+�5       �� $��0��6   $��0 0 0 1q�6&     
GrayBrush7#���  6@   ._�5!      � .+�5"      �� -   5         ��Name$ TopLogoImage��5"       $��	12 15 0 0q�66   $
��None=�6I   $	��Top=�6X   $
��Left=�6p   5#      �� 5$      $��	12 17 0 0q�6$   $
��Left=�67   $	��Top=�6R     
GrayBrush5#��[ 6j   $��16(�6�   $
��Bold"�6�   $��Microsoft YaHei)�6�   $��	英语说��6�   5%      �� 5&      -   5         ��Name$ MinimizeBtn��5&   !   $��24��66   $��24��6A   $��	0 20 48 0q�6M   $��Right=�5'      $	��Top=�69     ButtonStyle#���  5(      #��[ 6C   $y�Visible=�5)   G    � ToolTip$ 	最小化��6\    � FocusVisualStyle 5&   `   Z� ��5)      �� *WindowMinimizeCommand+.��5*      '  !MaterialDesignThemes.Wpf.PackIcon  $��16��66   $��16��6A   $��Center=�6M   $��Center=�6j   �  �ControlzEx.PackIconBase`1[[MaterialDesignThemes.Wpf.PackIconKind, MaterialDesignThemes.Wpf, Version=1.2.0.33442, Culture=neutral, PublicKeyToken=null]]   Kind$ WindowMinimize=�6�   5+      �� 5,      $��24��6!   $��24��6,   $��	0 20 18 0q�68   $��Right=�5-      $	��Top=�65   #���  5.      #��[ 6?   $ 关闭 英语说��5/   D    5,   K   Z� ��5/      �� *WindowCloseCommand+��50      ��51   "   True��67   .��53        $��16��66   $��16��6A   $��Center=�6M   $��Center=�6j   $ WindowClose=�6�   54      55      56      � 57      $��1��6   .+�58      � 	 � ColumnDefinitions	 59      �� 5:      $
��788* �6/   6>   �� 5;      $	��220 �6/   6=   .+�5?      � $��0��6   y�6/   �� *ShowTotalScore+
 �� FallbackValue$
 Visible��.+�5@      � -   5         �Name$ TotalResultView��5@   #   $��0 20 0 0q�6<   .+�5A   "   � $	��660��6'   $	��500��63   
�������$��0 60 0 0q�6S   $	��Top=�6e   $��Center=�6}    J� Effect 5B   &   3  -System.Windows.Media.Effects.DropShadowEffect  5C   *      
BlurRadius$ 20Q�6;      Color$
 Aqua��6K      	Direction$ 20Q�6X      Opacity$	 0.8Q�6g      RenderingBias$ Performance=�6u      ShadowDepth$ 1Q�6�   5D   +   .+�5F   &   �� $	��260��6�   $��Center=�6�   $	��Top=�6�   $��0 70 0 0q�5G   (   ��5F   ,   �� *TotalStarScore+ �� Mode$ TwoWay=�$#
 ../../Resources/star-1-14.png��5G   <   �� 5H   &   $��../../Resources/cup.png��6,   $	��180��6M   $��Center=�6Y   $��Center=�6v   $��	0 -20 0 0q�5I   (   6=   �� 5K   &   $��	0 380 0 0q�61   .+�5L   *   � $��Center=�64   $	��Top=�6Q   .+�5M   .   �� ���#333333$��24(�6�   $
��Bold"�6�   ��68   �� *UserTotalScoreText+$%
 考试完成，您的得分是 ��6�   �� 5N   .   $��	0 -10 0 0q�68   $��32(�6�   $
��Bold"�6�   ��6K   �� *TotalScoreForeground+$
 #3CB371����6�   �� *UserTotalScore+$
 30��6�   5O   +   �� 5P   *   $��10q�64   $��Center=�6@   ���#333333$��24(�6�   $
��Bold"�6�   ��6]   �� *DefeatPercent+$
 加油哦！��6�   5Q   '   5R   #   5S      � 5T      $	��586��6#   $	��128��6/   
�������$��	0 458 0 0q�6O   $	��Top=�6b   $��Center=�6z   $y�	Collapsed=�6�    5U   "     5V   &   $ 20Q�67   $
 Aqua��6G   $ 20Q�6T   $	 0.8Q�6c   $ Performance=�6q   $ 1Q�6�   5W   '   .+�5Y   "   � .+�5Z   &   � $	��132��6+   .+�5[   *   ��   ExamScoreTbStyle#���  64   $��听后选择��6_   6o   � 5\   *     ExamScoreWPStyle#���  64   .+�5]   .   �� $��24(�6}   ��68   �� *ExamScoreForeground+$
 #3CB371����6�   �� *ChoiceUserScore+$
 10��6�   �� 5^   .   ���#333333$��24(�6M   $	�� / ��6[   6h   �� 5_   .   ���#333333$��24(�6M   ��6[   �� *ChoiceTotalScore+$
 10��6�   5`   +   5a   '   � 5b   &   $	��116��6+   .+�5c   *   �� #���  64   $��听后回答��6_   6m   � 5d   *   #���  64   .+�5e   .   �� $��24(�6}   ��68   �� *ExamScoreForeground+$
 #3CB371����6�   �� *SpeakUserScore+$
 10��6�   �� 5f   .   ���#333333$��24(�6M   $	�� / ��6[   6h   �� 5g   .   ���#333333$��24(�6M   ��6[   �� *SpeakTotalScore+$
 10��6�   5h   +   5i   '   � 5j   &   $	��202��6+   .+�5k   *   �� #���  64   $��听后记录并转述��6_   6p   � 5l   *   #���  64   .+�5m   .   �� $��24(�6}   ��68   �� *ExamScoreForeground+$
 #3CB371����6�   �� *RecordAndRelateUserScore+$
 10��6�   �� 5n   .   ���#333333$��24(�6M   $	�� / ��6[   6h   �� 5o   .   ���#333333$��24(�6M   ��6[   �� *RecordAndRelateTotalScore+$
 10��6�   5p   +   5q   '   � 5r   &   $	��132��6+   .+�5s   *   �� #���  64   $��短文朗读��6_   6m   � 5t   *   #���  64   .+�5u   .   �� $��24(�6}   ��68   �� *ExamScoreForeground+$
 #3CB371����6�   �� *ReadUserScore+$
 10��6�   �� 5v   .   ���#333333$��24(�6M   $	�� / ��6[   6h   �� 5w   .   ���#333333$��24(�6M   ��6[   �� *ReadTotalScore+$
 10��6�   5x   +   5y   '   5z   #   5{      5|      � 5      $��0��6   y�6/   �� *ShowDetailScore+$
 Visible��.+�5�      �� -   5         ��Name$ ResultSV��5�   +   $��2q�6=   |�.  .��5�   "   � -   5        $ PaperDetailView��5�   '   5�   #   5�      5�      � 5�      $��1��6   ���#F6F6F6.+�5�      �� .+�5�   "   �� -   5        $��btnOkCommand��5�   )   $y�	Collapsed=�6=   5�   #   �� 5�   "   -   5        $��BtnReturnHome��5�   )     BackHomePageButton#���  6>   ���#999999$	��220��5�   :   $��48��6F   $��20(�6R   $��返回首页��6`   $ 返回 首页��6o   5�   #   �� 5�   "   -	   5         ��Name$ QsItemlb��5�   *   $	��220��5�   &   $��20(�62    	 ExamResultToolListBox#��� 	 5�   &   $��Microsoft YaHei)�6X   ��5�   <   �� *
QsNaviName+$ TwoWay=� �� SelectedIndex 5�   &   �� *SelectedExamShow+$ TwoWay=�$
 1����5�   &   �� 5�   *   .R�5�   .   �� ��68   �� *	TitleName+6U   5�   +   5�   #   5�      5�      5�      5�      � 5�      $��2��6   .+�5�      � 	 5�      �� 5�      $
��788* �6/   6>   �� 5�      $	��220 �6/   6=   .+�5�      �� $��1��6!   $��0q�61   
������  �� CornerRadius$ 0 0 15 0��6_   ���#f6f6f65�      5�      5�      5�      5�      �� 5�   
   -
   5        $ CloseCurrentBtn��5�      $y�	Collapsed=�6*   $��22��6A   $��22��6L   $��
0 20 115 0q�6X   $��Right=�5�      $	��Top=�6)   
�������#���  5�      )  #MaterialDesignThemes.Wpf.DialogHost   CloseDialogCommand#��Z 5�       5�      Z� ��5�      ��5�      True��6'   .��5�        $��18��5�      $��18��5�      $��Center=�6   $��Center=�66   $ WindowClose=�5�      5�      5�      5�      