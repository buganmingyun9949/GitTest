   M S B A M L   `   `   `  ����M  IPlugin.Exam.Report, Version=1.3.1.0, Culture=neutral, PublicKeyToken=nullA%clr-namespace:Plugin.Exam.Report.ViewPlugin.Exam.Report.View  5      D @ST.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullF1clr-namespace:ST.Common.Domain;assembly=ST.CommonST.Common.Domain 5      I)clr-namespace:Plugin.Exam.Report.ControlsPlugin.Exam.Report.Controls  5	      -    'Plugin.Exam.Report.View.ObtainInfoAskRV   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      7local%clr-namespace:Plugin.Exam.Report.View     5      W SMaterialDesignThemes.Wpf, Version=1.2.0.33442, Culture=neutral, PublicKeyToken=nullFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5      >domain1clr-namespace:ST.Common.Domain;assembly=ST.Common  5      8controls)clr-namespace:Plugin.Exam.Report.Controls  5	      $	��780��5
         � 	Resources  5       �� MergedDictionaries �� Source1  +ST.Common.Domain.ResultChoiceValueConverter   valueConverter$  ST.Common.Domain.KindConverter  kindConverter/  )ST.Common.Domain.PlayControlIconConverter  playIconConverter�� 5   
    5      �� 5      $J Dpack://application:,,,/ST.Style;component/ControlEx/ExamStyleEx.xaml?�6%   6u   �� 5      $G Apack://application:,,,/ST.Style;component/ControlEx/ButtonEx.xaml?�6%   �� 5      $H Bpack://application:,,,/ST.Style;component/ControlEx/ListBoxEx.xaml?�6%   �� 5      $M Gpack://application:,,,/ST.Style;component/ControlEx/ScrollViewerEx.xaml?�6%   �� 5      $G Apack://application:,,,/ST.Style;component/ControlEx/SliderEx.xaml?�6%   %W   &	        &	      &	 &       5      60     5      6#     5      6.   5      .��5      � .+�5   
   �  � RowDefinitions 5      �� 5      $��60 �6    6.   �� 5      $��* �6    6-   .+�5       � $��0��6   ���#f6f6f6.+�5!      � $
��Left=�6   $��Center=�67   $��20 0 0 0q�6R   .+�5"      �� ���#999999$��20(�65   $��0 0 10 0q�6C   $��得分��6U   6a   �� 5#      ���#64c0a0$��20(�65   $��0 0 10 0q�6C   ��6U   �� *PaperDetailUserScore+ �� FallbackValue$ 10�� �� Mode$ TwoWay=�6�   �� 5$      $��1��6   	 �� X1$ 0��6+   	 �� Y1$ 32��62   	 �� X2$ 0��6:   		 �� Y2$	 8��6A   $��0 -8 0 0q�6H     
GrayBrush5#��[ 6Z   $��1��6   6�   �� 5%      ���#999999$��20(�65   $��	10 0 10 0q�6C   $��总分��6V   6b   �� 5&      ���#999999$��20(�65   $��0 0 10 0q�6C   ��6U   �� *PaperDetailTotalScore+$ 10��$ TwoWay=�6�   5'      � 5(      $��Right=�6   $��Center=�68   $��0 0 20 0q�6S   51      � 52      $��Right=�6   $��Center=�68   $��	20 0 20 0q�6S   5:      5;      � 5<      $��1��6   $	��800��6    .+�5=      � $��0 0 0 10q�6    5>      �� 5?      $
��Auto �6(   68   �� 5@      $
��Auto �6(   68   �� 5A      $
��Auto �6(   68   �� 5B      $
��Auto �6(   68   �� 5C      $
��Auto �6(   68   �� 5D      $
��Auto �6(   68   �� 5E      $
��Auto �6(   68   �� 5F      $
��Auto �6(   68   .+�5H      �� $��0��6!   $��	40 0 40 0q�6.   $	��Top=�6A   $
��Left=�6Y   .+�5J      ��   BigTitleStyle#���  5K   $   $��0 20 0 8q�6L   $��SimSun)�6^   $��WrapWithOverflow=�5L   $   ��5J   $   �� *	QsTitle+$K E一、信息获取及转述（共5小题，每小题1分，共5分）��5M      �� 5N        SecondTitleStyle#���  5P   $   $��50 10 68 10q�6O   $��SimSun)�6d   $��WrapWithOverflow=�5Q   $   
 �� 
LineHeight$
 30��6D   ��5O   $   �� *QsTitleContent+$i c第一节 信息转述（6分） 听下面一篇短文，完成信息转述，短文播放两遍。��5R      �� 5S      .+�5T      �� #���  5V   !   $��
50 10 68 0q�6L   $��SimSun)�6`   $��WrapWithOverflow=�5W   !   $
 30��6A   ��5U   !   �� *QsTitleContent1+$� �请根据所听到的内容，先记录下面句子中空白处所缺的单词（每空仅填一个单词），然后在50秒内说一段话进行转述，内容包含下面各句子的信息。现在，你有15秒钟的时间阅读下面内容。��5X      � 5Y      $��40 2 2 6q�6(   .+�5Z   "   �� $
��（ ��6,     OptionTitleStyle2#���  66   6d   �� 5[   "     OptionTitleStyle1#���  6n   ��6,   �� *UserItemScore+$ 0 分��$ TwoWay=���6�   �� *ChoiceUserScoreColor+$ TwoWay=�6�   �� 5\   "   $	�� / ��6,   #���  67   6e   �� 5]   "   #���  6q   ��6,   �� *ItemTotalScore+$ 1.5 分��$ TwoWay=�6�   �� 5^   "   $
�� ）��6,   #���  66   6d   5_      �� 5`        ExamQsTextTbStyle#���  6X   $��50 10 40 10q�6�   $��SimSun)�6�   $��WrapWithOverflow=�6�   ��6(   �� *
TitleTwo+$ 提示文本��y�6�   �� *TitleTextVisibility+$ Visible��6  �� 5b      $��50 0 0 0q�6%   $��choice��6�   
�������$��2q�6�   ���#30cc75���#30cc75$
��Left=�6    	 ButtonStyle#��� 	 6  $��96��6A  $��36��6L  ��67   �� *PlayQsAudio+y�6W   �� *ShowPlayBtn+$ Visible��.��5c   "   ��  �� Orientation$ 
Horizontal=�6-   .+�5d   &   �� $��播放原文��60   $��Center=�6<   6Y   '  !MaterialDesignThemes.Wpf.PackIcon  5e   &   $��4 4 0 4q�6>   �  �ControlzEx.PackIconBase`1[[MaterialDesignThemes.Wpf.PackIconKind, MaterialDesignThemes.Wpf, Version=1.2.0.33442, Culture=neutral, PublicKeyToken=null]]   Kind 6O   ��  �� Path$ PlayIcon�$ TwoWay=� �� 	Converter# [ $
 Play�� �� ConverterParameter$	 9-1��5f   #   5g      5h      �� 5j      $	��300��6    $	��580��60   $
��Fill=�6^   ��6?   �� *QsInfoImage+6o   � 5k      $��50 10 80 10q�6   .+�5l      �� .+�5m   "   � $��	0 0 20 20q�6,   $��40��6?   $��Center=�6K   $
��Left=�6f   .+�5n   &   �� $��我的录音：��60   $��0 6 10 0q�6=    
 OptionTitleStyle3#��� 
 6O   6}   1   +Plugin.Exam.Report.Controls.PlayAudioViewUC  5o   &   $
��Left=�6?      AudioUrl 6Z   �� *PlayUserAudioUrl+$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�6�   5p   #   �� 5q   "   $��参考答案：��6,   #��� 
 69   6h   �� 5r   "     AnswerTextStyle#���  6d   $��0 20 0 0q�6�   $
��Auto��6�   ��6,   �� *SpeakRefText+$ 参考答案内容��6�   5s      5t      5u      �� 5w      -   5         ��Name$ QsItemContent��5w   .   $��1��6!   $��40 10 40 10q�6E   5x      �� 5y      $��2��6!   $��	40 0 40 0q�6.   $	��Top=�6A   $
��Left=�6Y   .+�5z      � $��1��6   #���  6*   .+�5{      �� $ 0��6#   $ 0��6*   $	 760��61   $	 0��6:   #���  6A   $��1��6g   $��Center=�6{   $��	0 10 0 20q�6�   6�   5|      �� 5~      #���  5�      $��50 10 68 10q�6H   $��SimSun)�6]   $��WrapWithOverflow=�5�      $
 30��6=   ��5      �� *QsTitleContent2+$� �第二节 询问信息（4分）你希望了解更多相关信息，请根据以下提示提两个问题。每个问题有15秒钟的准备时间和8秒钟的提问时间。��5�      �� 5�      $��50 10 40 10q�6%   .+�5�      � $��0 0 30 0q�6(   .+�5�   "   �� $��
0 10 20 10q�6,   #���  6x   ��6@   �� *QsItemContent1+$ 1.一个句子��6�   � 5�   "   $��0 10 2 0q�6,   .+�5�   &   �� $
��（ ��60   #���  6:   6h   �� 5�   &   #���  6s   ��60   �� *UserItemScore1+$ 0 分��$ TwoWay=���6�   �� *ChoiceUserScoreColor+$ TwoWay=�6�   �� 5�   &   $	�� / ��60   #���  6;   6i   �� 5�   &   #���  6v   ��60   �� *ItemTotalScore1+$ 1.5 分��$ TwoWay=�6�   �� 5�   &   $
�� ）��60   #���  6:   6h   5�   #   5�      � 5�      $��
0 10 20 10q�6(   $��40��6<   $��Center=�6H   $
��Left=�6c   .+�5�   "   �� $��我的录音：��6,   $��0 6 10 0q�69   #��� 
 6K   6y     5�   "   $
��Left=�6;    6V   �� *PlayUserAudioUrl1+$ TwoWay=�$ PropertyChanged=�6�   5�      �� 5�      $��参考答案：��6(   #��� 
 65   6d   �� 5�      #���  6a   $��0 20 0 0q�6�   $
��Auto��6�   ��6(   �� *SpeakRefText1+$ 参考答案内容��6�   5�      �� 5�      $��50 10 40 10q�6%   .+�5�      � $��0 0 30 0q�6(   .+�5�   "   �� $��
0 10 20 10q�6,   #���  6x   ��6@   �� *QsItemContent2+$ 2.一个句子��6�   � 5�   "   $��0 10 2 0q�6,   .+�5�   &   �� $
��（ ��60   #���  6:   6h   �� 5�   &   #���  6s   ��60   �� *UserItemScore2+$ 0 分��$ TwoWay=���6�   �� *ChoiceUserScoreColor+$ TwoWay=�6�   �� 5�   &   $	�� / ��60   #���  6;   6i   �� 5�   &   #���  6v   ��60   �� *ItemTotalScore2+$ 1.5 分��$ TwoWay=�6�   �� 5�   &   $
�� ）��60   #���  6:   6h   5�   #   5�      � 5�      $��
0 10 20 10q�6(   $��40��6<   $��Center=�6H   $
��Left=�6c   .+�5�   "   �� $��我的录音：��6,   $��0 6 10 0q�69   #��� 
 6K   6y     5�   "   $
��Left=�6;    6V   �� *PlayUserAudioUrl2+$ TwoWay=�$ PropertyChanged=�6�   5�      �� 5�      $��参考答案：��6(   #��� 
 65   6d   �� 5�      #���  6a   $��0 20 0 0q�6�   $
��Auto��6�   ��6(   �� *SpeakRefText2+$ 参考答案内容��6�   5�      5�      5�      5�      5�      5�      5�      