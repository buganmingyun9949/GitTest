   M S B A M L   `   `   `  ����G  CPersonal-App, Version=2.3.7.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:Personal_App.DomainPersonal_App.Domain  5      $    Personal_App.Domain.ExpireCard   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      3local!clr-namespace:Personal_App.Domain     5      Acontrols,http://metro.mahapps.com/winfx/xaml/controls     5      $	��860��5      $	��460��6+   .��5
      � 
������ .+�5   
   �� -   5          ��Name$  BindCardImage��5      $��
282 76 0 0q�6'   $
��None=�6;   $	��Top=�6J   $
��Left=�6b   6   �� 5   
   -   5         ��Name$ errMsg��5   %   $��368 320 0 0q�6   ���#ff6161$��16(�6J   $
��Bold"�6X   $6��0您的学习卡已经过期，请绑定新卡！��6j   $
��Left=�6�   $	��Top=�6�   6�   �� 5   
   -   5        $ 	errMsgOut��5   #   $��	0 460 0 0q�6   
���  ��$��16(�6G   $
��Bold"�6U   $��Center=�6�   $	��Top=�6�   ��6g   �� *	ErrMsgOut+ �� FallbackValue$ 绑定失败!��6�   �� 5   
   
������ $��0q�6+    �� CornerRadius$ 8��6@      
GrayBrush9#���   6Q   $��272 370 0 0q�6{   $	��332��6�   $��56��6�    �� Padding$ 0 8 0 0q�5   !   $
��Left=�63   $	��Top=�6N   ._�5      � .+�5      �� -   5         ��Name$ 	txtCardNo��5      $��2,0,0,0q�6
   $��0q�5   *    �� 	MaxLength$ 11��6>   $��16(�5   *   
������ $	��320��5   *   $��40��66   $	��Top=�6B   $
��Left=�6Z   $��Center=�5   *   ��5   .   �� *	TxtCardNo+ �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�5   *   5      5       �� 5#   
   $��612 370 0 0q�6   $	��140��6&   $��56��62   $	��Top=�6>   $
��Left=�6V   $��绑  卡��5$      
�������$��20(�63   ���#ff6161  ArcButtonStyle#���  5&      ��5'      �� *BindingCardCmd+5(      5)      5*      