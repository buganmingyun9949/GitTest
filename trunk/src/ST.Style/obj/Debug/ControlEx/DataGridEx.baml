   M S B A M L   `   `   `  ����W  SMaterialDesignThemes.Wpf, Version=1.2.0.33442, Culture=neutral, PublicKeyToken=null{Sclr-namespace:MaterialDesignThemes.Wpf.Converters;assembly=MaterialDesignThemes.Wpf#MaterialDesignThemes.Wpf.Converters  5      C ?ST.Style, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:ST.Style.ControlExST.Style.ControlEx 5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089   SyncQsDataGridColumnHeader  �� 
TargetType=   7System.Windows.Controls.Primitives.DataGridColumnHeader �� Property �� Value  GrayBrush10 �� RelativeSource �� Path  MaterialDesignColumnHeader �� 
TargetType � 	Resources  Key �� 
TargetName �� TargetProperty9  3System.Windows.Media.Animation.EasingDoubleKeyFrame	 O� KeyTime
 O� Value �� Padding �� Orientation9   3MaterialDesignThemes.Wpf.ListSortDirectionIndicator  Name    SortDirection   ListSortDirection J� SnapsToDevicePixels n�Name ) %MaterialDesignGridColumnHeaderGripper �� Triggers P� Property P� Value  MaterialDesignBody P� 
SourceName N� EnterActions ��Name  GoToVisible N� ExitActions �� BeginStoryboardName  
GoToHidden  SyncQsDataGridRow)  #System.Windows.Controls.DataGridRow ��Name?  9System.Windows.Controls.Primitives.SelectiveScrollingGrid � ColumnDefinitions � RowDefinitions?  9System.Windows.Controls.Primitives.DataGridCellsPresenter   
ItemsPanelA  ;System.Windows.Controls.Primitives.DataGridDetailsPresenter   DetailsVisibility$   SelectiveScrollingOrientation;  5System.Windows.Controls.SelectiveScrollingOrientation    Vertical! �� ConverterParameter&   System.Windows.Controls.DataGrid#"   RowDetailsScrollingConverter# �� 	Converter$ �� AncestorType:	  4System.Windows.Controls.Primitives.DataGridRowHeader7
  1System.Windows.Controls.DataGridHeadersVisibility
% 
  Row!&   HeadersVisibilityConverter' �� Triggers 	 MaterialDesignSelection 
 MaterialDesignDivider  SyncQsDataGridCell*  $System.Windows.Controls.DataGridCell  MaterialDesignTextBoxBorder  
GrayBrush8( ]� 
Conditions) �� Property* �� Value�� 5      D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    :x,http://schemas.microsoft.com/winfx/2006/xaml     5      b
convertersSclr-namespace:MaterialDesignThemes.Wpf.Converters;assembly=MaterialDesignThemes.Wpf  5      FmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes   5      *local clr-namespace:ST.Style.ControlEx  5      %�  &	        7  7  7  7  &	 �    &	 �    �� 5      "    6/   .�5   
   ��  � ��$ 38��6#   �� 5	   
    � ��$	 150��6"   �� 5
   
    � ��# �  6'   �� 5   
    � ��$
 Left=�60   �� 5   
    � ��$ Center=�6.   �� 5   
    � ��$ Center=�65   �� 5   
    � ��$ 12(�6/   �� 5   
    � ��$ Medium"�61   �� 5   
    � �� 6$   ��  �� *Self+$9 3(materialDesign:DataGridAssist.ColumnHeaderPadding)��� 5   
    � ��$ 0q�6#   �� 5   
    � ��# �  6'   �� 5   
    � �� 5      �� 5      "   6"    5      ���� 5      GoToVisible .��5      U� $ SortDirectionIndicator��6<   $ Width�6k   .\�5   "     $	 0:0:0.0��67   $
 0Q�6I     5   "   $	 0:0:0.2��67   $
 12Q�6I   5      5      �� 5      
GoToHidden .��5      U� $ SortDirectionIndicator��6<   $ Width�6k   .\�5   "     $	 0:0:0.0��67   $
 12Q�6I     5    "   $	 0:0:0.2��67   $
 0Q�6I   5!      5"      5#      .R�5$      � .+�5%      �� #��z��6!   #��z��6M   #��z��6�   # z��6�   ._�5&      �� $ 
Horizontal=�6)   
������ .+�5'   "     $ SortDirectionIndicator��5(   A   $��0��5'   L   $��12��6V   # z 5)   A   �� 5+   "   #��z��63   ��. # z 6�   #��z��6�   #��z��5,   3   5-      5.      n� 5/      $ PART_LeftHeaderGripper��6    $
��Left=�6@   8��  6[   n� 50      $ PART_RightHeaderGripper��6    $��Right=�6A   8�� 6]   51       52      P� 53       �@J�IsMouseOver . . �54      �� # �  6;    � ��55      P� 56       �@  SortDirection$ 	AscendingX�6;   . �57      �� # �  6;    � ��58      P� 59       �@  SortDirection$ 
DescendingX�6;   . �5:      �� # �  6;    � ��5;      P� 5<      $ SortDirectionIndicator��6"    �@ 	IsNeutral .   5=      �� 5>   "   $ GoToVisible��6\   8��  5@      �� 5A   "   $ GoToVisible��63   �� 5B   "   8�� 62   5D      5F      5H      5I      �� 5K      "   6&   .�5L   
   ��  � ��
 ���� 6'   �� 5M   
    �@J�SnapsToDevicePixels . 60   �� 5N   
    �@=�ErrorTemplate 65   Z� �� 5O   
    �@ ValidationErrorTemplate 5P      �� 5Q      .R�5R      �� ���#d50000$��2,0,0,0q�65   $��!��6F   $��Center=�6O   5S      5U      �� 5V   
    � �� 5W      �� 5X      "  6"   .R�5Y      �� $ 
DGR_Border��6   #��z��61   #��z��6]   #��z��6�    . ._�5Z         5[      �� 5\   "   $
��Auto �63   �� 5]   "   $��* �63    5_      �� 5`   "   $��* �60   �� 5a   "   $
��Auto �60   .+�5c        $��1��65   #��z 6E   # z 6o     5d      $��1��67   $��1��6G   #y�z 6k   6T   �� *AreRowDetailsFrozen+#! Z  ## Z"  �� "$  6k  	  5e      $��2��60   $ Vertical=�6A   y�6�   �� *HeadersVisibility+#! Z% ## Z&  �� "$  5f      5g      5h      5j      ' 5k   
   P� 5l       �@ 
IsSelected . . �5m      ��  � ��# � 	 6/   5n      P� 5o       �@J�IsMouseOver . . �5p      ��  � ��# � 
 6/   5q      5x      �� 5z      "   6'   .�5{   
   ��  � ��$ 14(�6/   �� 5|   
    � ��$ 48��6#   �� 5}   
    � ��$	 150��6"   �� 5~   
    � ��$
 Left=�60   �� 5   
    � ��$ Center=�6.   �� 5�   
    � ��$ Center=�65   �� 5�   
    � �� 6$   ��  �� *Self+$1 +(materialDesign:DataGridAssist.CellPadding)��� 5�   
    � ��
 ���� 6'   �� 5�   
    � ��
 ���� 6(   �� 5�   
    � ��$ 0q�6,   �� 5�   
    � �� 5�      �� 5�      "  6"   .R�5�      �� #��z��6   #��z��6I   #��z��6}    . #��z��6�   #��z��6�   #��z��66  #��z��6V  ._�5�      �� # z 6+   $��8 0 4 0q�5�   +   #��z��6<   5�      5�      5�      ' 5�   
   P� 5�       �@J�IsKeyboardFocusWithin . . �5�      ��  � ��# �  60   5�      P� 5�       �@J�IsMouseOver . . �5�      ��  � �� �#FFFFFF6/   �� 5�       � ��# �  6/   5�      ]� 5�      ( 5�      �� 5�      ) �@ 
IsSelected* . 66   �� 5�      ) �@��IsSelectionActive* .  6F   ./�5�      ��  � �� �#FFFFFF6/   �� 5�       � �� �#30CC756/   5�      ]� 5�      ( 5�      �� 5�      ) �@ 
IsSelected* . 66   �� 5�      ) �@��IsSelectionActive* . 6F   ./�5�      ��  � �� �#FFFFFF6/   �� 5�       � �� �#30CC756/   5�      P� 5�       � {� .  . �5�      ��  �@J�Opacity$	 .56Q�6,   5�      5�      5�      