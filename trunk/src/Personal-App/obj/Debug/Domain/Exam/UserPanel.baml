   M S B A M L   `   `   `  ����  Personal-App(    "Personal_App.Domain.Exam.UserPanel   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      W SMaterialDesignThemes.Wpf, Version=1.2.0.33442, Culture=neutral, PublicKeyToken=nullFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5      $	��550��5   7   $	��220��6D   $��Microsoft YaHei)�6P   .��5	      � ���#f6f6f6  � RowDefinitions  5
   
   �� 5      $��36 �6   6*   �� 5      $��44 �6   6*   �� 5      $	��110 �6   6+   �� 5      $��40 �6   6*   �� 5      $��28 �6   6*   �� 5      $��36 �6   6*   �� 5      $��2 �6   6)   �� 5      $��40 �6   6*   �� 5      $	��220 �6   6+   .+�5   
   �� $��1��6   ���#30cc75$��22(�6m   $	��Top=�6{   $��Center=�6�   |�.  ��6!   �� *	ExamTitle+ �� FallbackValue$ 中考英语听说模考��6�   � 5   
   $��2��6   $	��Top=�6   .+�5      �� -   5         ��Name$ imageBorder��5      $
��Left=�6*   
�������$	��100��6Y   $	��100��6e   
���   �$��0q�6�   $��60 0 0 0q�6�    �� CornerRadius$ 50��6�   �� 5       -   5         ��Name$ AvatarImage��5    <   $	��100��6   $	��100��6    $
��Fill=�6-   ��6Q   �� *User.Avatar+ �� UpdateSourceTrigger$ PropertyChanged=� �� Mode$ TwoWay=� J� Clip 5!      �� 5"       �� RadiusX$ 50Q�6(   	 �� RadiusY$	 50Q�65   
 �� Rect$
 0,0,100,100�6B   5$      5%      �� 5&   
   $��3��6   ���#333333$��18(�6�   $��Center=�6�   $��Center=�6�   |�.  ��6!   �� *User.UserName+$ 学员��$ PropertyChanged=�$ TwoWay=�6�   � 5'   
   $��4��6   $��Center=�6!   $��Center=�6>   $y�	Collapsed=�6Z   .+�5(      '  !MaterialDesignThemes.Wpf.PackIcon  $��0 1 6 0q�6&   $��Center=�67   �  �ControlzEx.PackIconBase`1[[MaterialDesignThemes.Wpf.PackIconKind, MaterialDesignThemes.Wpf, Version=1.2.0.33442, Culture=neutral, PublicKeyToken=null]]   Kind$ 	MapMarker=�6R   ���#9999996c   �� 5)      $��4��6   ���#999999$��16(�6s   $��Center=�6�   $��Center=�6�   |�.  ��6%   �� *User.Card.CardName+$ 浙江��6�   5*      �� 5+   
   $��6��6   	 �� X1$ 20��6   	 �� Y1$ 0��6$   	 �� X2$	 200��6+   	 �� Y2$ 0��64      
GrayBrush5#���   6;   $��1��6a   $��Center=�6u   6�   � 5-   
   $��8��6   .+�5.      �� $	��160��6   $	��180��6!   $��0q�6.   ���#FFFFFF$ 5��6N   $	��Top=�6_   ._�5/      � .+�50      �� ���#30cc75$��20(�6q   $
��Bold"�6   $��
16 38 16 0q�6�   $	��Top=�6�   $��Center=�6�   |�.  $
��Wrap=�6�   ��6    �� *SelectPaperViewName+$ 模考试题��6�   �� 51      ���#30cc75$��74(�6j   $��0 64 0 0q�6x   $	��Top=�6�   $��Center=�6�   |�.  ��6    �� *SelectPaperNumber+$ 0��6�   52      53      54      5?      5@      