   M S B A M L   `   `   `  ����G  CPersonal-App, Version=2.3.7.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:Personal_App.DomainPersonal_App.Domain  5      %    Personal_App.Domain.BindingCard   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      3local!clr-namespace:Personal_App.Domain     5      Acontrols,http://metro.mahapps.com/winfx/xaml/controls     5      W SMaterialDesignThemes.Wpf, Version=1.2.0.33442, Culture=neutral, PublicKeyToken=nullFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5      $	��896��5	      $	��718��6+   $��Microsoft YaHei)�5
   9   .��5      �� 
������ $��0q�6&     �� CornerRadius$  5��61   ._�5   
   � 
������ .+�5   
   �� $��10 0 0 0q�6   $��54��6#   $��24��6.   $	��Top=�6:   $
��Left=�6R      NoNormalButton#��[  5      $��
MainDialog��5   -   $��Microsoft YaHei)�6K   ��6   �� *	BackCmd+y�5      �� *ShowBackBtn+ �� FallbackValue$ 	Collapsed����5      � 5      .+�5      '  !MaterialDesignThemes.Wpf.PackIcon  $��20��6.   $��20��69   �  �ControlzEx.PackIconBase`1[[MaterialDesignThemes.Wpf.PackIconKind, MaterialDesignThemes.Wpf, Version=1.2.0.33442, Culture=neutral, PublicKeyToken=null]]   Kind$ UndoVariant=�6E   �� 5      $��返回��6    6,   5      5      � 5   
   $��0 80 0 0q�6   $��Center=�6!   .+�5      �� $��../Resources/xuexika.png��6   $	��519��66   $	��280��6B   $	��Top=�6O   6i   � 5      $��Bottom=�6   $��	0 0 0 266q�63   .+�5"      �� $��50 0 0 0q�6   
������ $��0q�6E   $  5��6Y     
GrayBrush9#���  5#       � Cursor$
 Hand��6G   $	��300��5$      $��42��6*   ._�5%      �� -   5         ��Name$ 	txtKeyNum��5%   0   $��16 0 0 0q�6   $��0q�5'   "    �� 	MaxLength$ 19��66   )  #MaterialDesignThemes.Wpf.HintAssist   Hint$  请输入16位学习卡号��5(   "   $��16(�5)   "   
������  �� CharacterCasing$ Upper=�6I   $	��260��5*   "   $��30��6.   $��Center=�6:   $��Center=�6U   $
��Left=�6w   ��5&   "   �� *
TxtCardNum+ �� Mode$ TwoWay=�	 �� UpdateSourceTrigger$	 PropertyChanged=�5*   w   5+      �� 5,      $��20 0 0 0q�6   $	��128��6,   $��40��68   $��Center=�6D   $��Center=�5-      $��绑定学习卡��5.      $��12(�6-     ZYBeginButton#��[ 5/      $��
MainDialog��50   @   $��Microsoft YaHei)�51      ��50      �� *BindingCardCmd+52      53      �� 54      -   5        
 ��Name$
 	errMsgOut��54   '   $��	0 0 0 220q�6   
���  ��$��12(�6K   $��Center=�55   R   $��Bottom=�56      ��55      �� *	ErrMsgOut+$ 查询失败!��$ TwoWay=�57      58      59      5:      5;      