   M S B A M L   `   `   `  ����G  CPersonal-App, Version=2.3.7.0, Culture=neutral, PublicKeyToken=nullC&clr-namespace:Personal_App.Domain.ExamPersonal_App.Domain.Exam  5      W SFramework.VoiceRecorder.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]Fclr-namespace:VoiceRecorder.Core;assembly=Framework.VoiceRecorder.CoreVoiceRecorder.Core 5      '    !Personal_App.Domain.Exam.ExamPage   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      8local&clr-namespace:Personal_App.Domain.Exam     5      W SMaterialDesignThemes.Wpf, Version=1.2.0.33442, Culture=neutral, PublicKeyToken=nullFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5      QcoreFclr-namespace:VoiceRecorder.Core;assembly=Framework.VoiceRecorder.Core  5      $��Microsoft YaHei)�5
   8     � 	Resources  5       �� MergedDictionaries�� 5   
    5      %    5      .��5      � 
������ .+�5   
   �� -   5        $	��top��5   M   $��1q�6   ���#30cc75 �� CornerRadius$ 15��6;   
�������._�5      �  � RowDefinitions 5      �� 5      $��54 �6$   62   �� 5      $
��600* �6$   64   �� 5      $��72 �6$   62   .+�5      � $��0��6   .+�5      �� -   5         ��Name$ TopLogoImage��5      $��	12 15 0 0q�62   $
��None=�6E   $	��Top=�5      $
��Left=�64   5      �� 5      $��	82 20 0 0q�6    $
��Left=�63   $	��Top=�6N      
WhiteBrush#���   5        $��16(�6J   $
��Bold"�6X   模考系统5"      �� 5#      $��20��6   $��20��6(   $��	0 20 15 0q�64   $��Right=�5$      $	��Top=�69     MainCircleButtonStyle#���  5%      
�������  
GrayBrush5#���  5&      $��
MainDialog��5'   J    � FocusVisualStyle 5#   G   Z� ��5'      �� *RunExtendedDialogCommand+.��5(      '  !MaterialDesignThemes.Wpf.PackIcon  $��24��5)      $��24��5*      �  �ControlzEx.PackIconBase`1[[MaterialDesignThemes.Wpf.PackIconKind, MaterialDesignThemes.Wpf, Version=1.2.0.33442, Culture=neutral, PublicKeyToken=null]]   Kind$ WindowClose=�5+      5,      5-      �� 5.      $��1��6   $��0 1 0 1q�6&     
GrayBrush8#���  6@   ._�5/      � .+�50      �  � ColumnDefinitions 51      �� 52   "   $
��788* �63   6B   �� 53   "   $	��220 �63   6A   .+�55      � 56      � 57      $��1��6#   ���#f6f6f6 58   "   �� 59   &   $	��550 �64   6C   �� 5:   &   $��92 �64   6B   .+�5<   "   (   "Personal_App.Domain.Exam.UserPanel  $��0��62    � DataContext 6?   �� *UserPanelVM+6e   0   *Personal_App.Domain.Exam.ChangeVolumePanel  5=   "   $��1��6:   6I   5>      5?      5@      5A      � 5B      $��2��6   .+�5C      �  5D      �� 5E      $	��152 �6/   6=   �� 5F      $
��300* �6/   6>   .+�5H      �� $��0��6$   $��42 0 0 0q�64   $��听语音...��6F   ���#ff6161$��20(�6i   $
��Bold"�6w   |�.  $��Center=�6�   $
��Left=�6�   5I      � 5J      $��1��6   $y�Visible=�6/    5K      �� 5L   "   $
��300* �63   6B   �� 5M   "   $	��220 �63   6A   .+�5P      � $��0��6#   .+�5Q   "   �� $
��3/10��6,   
���   �$��20(�6K   $
��Bold"�6Y   $	��Top=�6k   $��Center=�6�   6�   �� 5R   "   {�.  $	��620��6;   $��0 40 0 0q�6G   	 � Minimum$	 0Q�6�   $	��Top=�6�   $��Center=�6�     CountDownProgress#���  6�   $��8��6X  ���#30cc75
 � Maximum
 6Y   ��  �� ElementName$ TxtTotalTime�� �� Path$
 Text� �� FallbackValue$ 1�� � Value 6  �� $ TxtPlayingTime��$
 Text�$ 1��6c  5S      )   #Personal_App.Domain.Exam.AudioPanel  5T      $��1��6T    6/   �� *AudioPanelVM+6f   5s      5t      5u      5v      5x      5y      5z      