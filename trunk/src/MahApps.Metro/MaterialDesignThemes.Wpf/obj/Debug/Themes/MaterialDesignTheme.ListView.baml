   M S B A M L   `   `   `  ����W  SMaterialDesignThemes.Wpf, Version=1.2.0.33441, Culture=neutral, PublicKeyToken=nullC&clr-namespace:MaterialDesignThemes.WpfMaterialDesignThemes.Wpf  5      Y1clr-namespace:MaterialDesignThemes.Wpf.Converters#MaterialDesignThemes.Wpf.Converters  5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089  �� MergedDictionaries �� Source �� 
TargetType �� Property �� Value �� 
TargetType � ColumnDefinitions � RowDefinitions1   +MaterialDesignThemes.Wpf.ScrollViewerAssist    SyncHorizontalOffset	 �� Mode
 �� RelativeSource   MaterialDesignDivider �� Orientation ��Name J� SnapsToDevicePixels �� AllowsColumnReorder �� Path! �� ColumnHeaderContainerStyle �� ColumnHeaderContextMenu �� ColumnHeaderTemplate# �� ColumnHeaderTemplateSelector �� ColumnHeaderToolTip �� Columns �� CanContentScroll �� CanHorizontallyScroll �� CanVerticallyScroll-  'System.Windows.Input.KeyboardNavigation   DirectionalNavigation �� ScrollableWidth � Maximum �� Orientation �� ViewportWidth �� ViewportSize, �� %ComputedHorizontalScrollBarVisibility  � Value! �� ScrollableHeight" �� ViewportHeight*# �� #ComputedVerticalScrollBarVisibility  MaterialDesignColumnHeader$ n�Name ) %MaterialDesignGridColumnHeaderGripper% ��Name& �� Padding' ��Name( �� Triggers) P� Property* P� Value  MaterialDesignBody+ �� Triggers  PrimaryHueMidBrush  MaterialDesignGridViewItem  MaterialDesignTextBoxBorder  MaterialDesignSelection, ]� 
Conditions- �� Property. �� ValueC   =MaterialDesignThemes.Wpf.Converters.ListViewGridViewConverter 5 1MaterialDesignListViewItemContainerStyleConverter 	 MaterialDesignListBoxItem/   DefaultValue0   	ViewValue +
 'MaterialDesignListViewTemplateConverter  MaterialDesignListView  MaterialDesignPaper1 �� 	Converter�� 5      D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    :x,http://schemas.microsoft.com/winfx/2006/xaml     5      .wpf&clr-namespace:MaterialDesignThemes.Wpf  5      @
converters1clr-namespace:MaterialDesignThemes.Wpf.Converters  5        5      �� 5   
   $g apack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.ListBox.xaml?�6   �� 5   
   $g apack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.TextBox.xaml?�6   �� 5	   
   $e _pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Thumb.xaml?�6   %m  (��       5      *'%GridView.GridViewScrollViewerStyleKey+)'�� �    7  &	     &	 F    7 	 7  &	
 g    &	     7  7 
 �� 6   " ��6E   .�5   
   ��  � �� 5      �� 5      " ��6"   .R�5      � #��z��6    5      �� 5      $��* �6/   �� 5      $
��Auto �6/    5      �� 5      $��* �6,   �� 5      $
��Auto �6,   .+�5      ]� #��z��6$   .+�5      �� $	��Top=�6+   |�.  $��Hidden=�5   +   $��Hidden=�5   +    5   +   �� *HorizontalOffset+$	 OneWay=�
 �� *TemplatedParent+.��5    "   �� #���   6)   $��0,0,0,1q�6_   ._�5!   &   �� $ 
Horizontal=�61   .+�5"   *   �� $% PART_GridViewHeaderRowPresenter��6E   # z 5*   A    5#   A   �� $. (TemplatedParent.View.AllowsColumnReorder�
 �� *TemplatedParent+ 5$   A   �� $5 /TemplatedParent.View.ColumnHeaderContainerStyle�
 �� *TemplatedParent+ 5%   A   �� $2 ,TemplatedParent.View.ColumnHeaderContextMenu�
 �� *TemplatedParent+ 5&   A   �� $/ )TemplatedParent.View.ColumnHeaderTemplate�
 �� *TemplatedParent+ 5'   A   �� $7 1TemplatedParent.View.ColumnHeaderTemplateSelector�
 �� *TemplatedParent+ 5(   A   �� $. (TemplatedParent.View.ColumnHeaderToolTip�
 �� *TemplatedParent+ 5)   A   �� $" TemplatedParent.View.Columns�
 �� *TemplatedParent+5*   A   �� 5,   *   ��64   �� 
 �� *Self+$. ((wpf:ListViewAssist.ListViewItemPadding)�5-   '   5.   #   50      �� 52      $!��PART_ScrollContentPresenter��65   # z��53   5    .   .  $ Local=�56   5   57      �� 59      $��PART_HorizontalScrollBar��6$   $��1��5:   $   # z 5;   $   $ 
Horizontal=�5<   $   # z 5=   $   #y�z 5>   $     5?   $   �� *HorizontalOffset+$	 OneWay=�
 �� *TemplatedParent+�� 5A      $��PART_VerticalScrollBar��6$   $��1��5B   $   # z! 5C   $   $ Vertical=�5D   $   # z" 5E   $   #y�z# 5F   $     5G   $   �� *VerticalOffset+$	 OneWay=�
 �� *TemplatedParent+5I      5J      5L      5M      �� 5O      " ��62   .�5P   
   ��  � ��$
 Left=�67   �� 5Q   
    � ��$ Center=�65   �� 5R   
    � ��$ Medium"�61   �� 5S   
    �@�OverridesDefaultStyle . 62   �� 5T   
    � �� 6$   �� 
 �� *Self+$. ((wpf:DataGridAssist.ColumnHeaderPadding)��� 5U   
    � ��# �  6'   �� 5V   
    � �� 5W      �� 5X      " ��6"   .R�5Y      ]� .+�5Z      n� $$ PART_HeaderGripper��6    $��Right=�5[       8��  5\       �� 5]      $% HeaderBorder��6!   #& z��5^   !   #��z��5_   !   ._�5`      �� $' HeaderContent��6/   #��z��5a   /   #��z��5b   /   #��z��5c   /   ��. # z 5e   /   5g      5h      ( 5i      P� 5j      ) �@J�IsMouseOver* . . �5k      �� # �  6;    � ��5l      5n      5p      + 5q   
   P� 5r      ) �@��Role$* Floating=�6&   . �5s      ��  �@J�Opacity$	 0.7Q�6,   �� 5t       � �� 5u      �� 5v      " ��6*   .R�5w      ]� $��PART_FloatingHeaderCanvas��6(   .+�5x   "   �� #���  6)   $��1q�6\   5y      5z      5|      5}      P� 5~      ) �@��Role$* Padding=�6&   . �5      ��  �@J�Opacity$	 0.7Q�6,   �� 5�       � �� 5�      �� 5�      " ��6*   .R�5�      �� $��PART_FloatingHeaderCanvas��6%   5�      5�      5�      5�      �� 5�      " ��6/   .�5�   
   ��  �@J�SnapsToDevicePixels . 60   �� 5�   
    �@�OverridesDefaultStyle . 62   �� 5�   
    � ��
 ���� 6'   �� 5�   
    � ��
 ���� 6(   �� 5�   
    � �� 5�      �� 5�      " ��6"   .R�5�      �� $% Border��6   #��z��5�      #���   5�      $��0,0,0,1q�5�      # z 5�      & 5�      �� 
 �� *Self+$. ((wpf:ListViewAssist.ListViewItemPadding)�._�5�      �� #��z��6/   5�      5�      5�      + 5�   
   P� 5�      ) �@J�IsKeyboardFocusWithin* . . �5�      ��  � ��# �  60   5�      P� 5�      ) �@��
IsSelected* . . �5�      ��  � ��# �  6/   5�      P� 5�      ) �@J�IsMouseOver* . . �5�      ��  � ��# �   6/   5�      ]� 5�      , 5�      �� 5�      - �@��
IsSelected. . 66   �� 5�      - �@��IsSelectionActive. .  6F   ./�5�      ��  � ��# �  6/   5�      ]� 5�      , 5�      �� 5�      - �@��
IsSelected. . 66   �� 5�      - �@��IsSelectionActive. . 6F   ./�5�      ��  � ��# �  6/   5�      P� 5�      ) � {�* .  . �5�      ��  �@J�Opacity$	 .56Q�6,   5�      P� 5�      ) �@��AlternationIndex$* 1��6?   . �5�      ��  � ��# �   6/   5�      5�        5�      8/   80  5�   	     5�      / 5�   
   �� 5�      " ��6   .R�5�      �� .��5�      �� 5�      ( 5�      P� 5�      ) �@��
IsGrouping* . . �5�      ��  � �� .  6J   5�      5�      0 5�   
   �� 5�      " ��6   .R�5�      �� #��� $�6   .��5�      �� 5�      ( 5�      P� 5�      ) �@��
IsGrouping* . . �5�      ��  � �� .  6J   5�      5�      5�      �� 5�      " ��6+   .�5�   
   ��  � ��# �  6'   �� 5�   
    � ��# �  6'   �� 5�   
    � ��# �  6(   �� 5�   
    � ��$ 0q�6,   �� 5�   
    � ��$ 13(�6%   �� 5�   
    � �� 6/   �� 
 �� *Self+$
 View�81   �� 5�   
    � �� . 6:   �� 5�   
    �@��PanningMode$
 Both=�65   �� 5�   
    �@��IsFlicksEnabled .  63   �� 5�   
    �@J�SnapsToDevicePixels . 60   �� 5�   
    �@�OverridesDefaultStyle .  62   �� 5�   
    � ��$
 Auto=�6G   �� 5�   
    � ��$
 Auto=�6E   �� 5�   
    � �� . 6:   �� 5�   
    � ��$ Center=�65   �� 5�   
    � �� 6%   �� 
 �� *Self+$
 View�81  5�      5�      