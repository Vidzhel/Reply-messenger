   M S B A M L   `   `   `  ����=  9UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:UI.UserControlsUI.UserControls  5      E'clr-namespace:UI.UIPresenter.ConvertersUI.UIPresenter.Converters  5      W0clr-namespace:UI.UIPresenter.ViewModels.Commands"UI.UIPresenter.ViewModels.Commands  5	      #    UI.UserControls.GroupListItem   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      /localclr-namespace:UI.UserControls     5      >
converters'clr-namespace:UI.UIPresenter.Converters     5      Ecommands0clr-namespace:UI.UIPresenter.ViewModels.Commands     5	        � 	Resources  5       �� 
TargetType   Content �� Property �� Value  GrayBGBrush �� CornerRadius J� InputBindings d� MouseAction7   1UI.UIPresenter.ViewModels.Commands.CommonCommands   OpenUserInfo �� Command	 �� CommandParameter
 � ColumnDefinitions �� Orientation  DarkBgBrush J� Clip @� Center @� RadiusX @� RadiusY  LightGrayBgBrush
 � Tag � ToolTip   	LeaveChat  IconicButton �� 	Converter9  3UI.UIPresenter.Converters.BoolToVisibilityConverter   JoinChat �� ConverterParameter �� Triggers :� RoutedEvent g� Duration �� 
TargetName �� TargetProperty  GrayBGHover	 �� To 
 GrayBG��%}  &	        7  7  7  7  7  7  7  7  �� 5   
   " ��6   .�5      ��  � �� 5      �� 5      .R�5      �� $��	Container��5   !   8��  $ 5��5   !   $	��0 3q�5   !    5      d� 5   "   $ 	LeftClicke�6/   # Z 6G   	 6�   �� *
UserInfo+._�5"      � 
 5$   "   �� 5'   &   $��* �67   �� 5*   &   $
��auto �67   .+�5/   "   �� $ 
Horizontal=�6-   .+�52   &   � .+�53   *   �� $��30��61   $
��10 5q�54   )   $��30��55   )   8�� $ 20��57   )   ._�59   .   � $��Center=�63   $��30��5:   3   $��30��5;   3   $��Center=�5<   3    5=   2   @� 5>   6   $ 15,15&�6F   $ 15Q�6U   $ 15Q�6b   .+�5@   2   �� $��Center=�5A   8   $��Center=�5B   8   ��5@   8   �� *
GroupPhoto+5B   8   5D   /   5F   +   5H   '   �� 5K   &   $��Center=�5L   (   8�� ��5K   ,   �� *	GroupName+5M   (   5P   #   �� 5S   "   $ 
Horizontal=�6-   $
��10 0q�5T   -   $��1��5U   -   .+�5W   &   �� $��	LeaveChat��6-   $ LightRedBrush��5X   -   $ 
Leave Chat��5Y   -   #��Z 5Z   -   $	����5\   -   $	��8 5q�69   $��Black"�5]   -   8�� ��5[   -   �� *	GroupInfo+y�5_   -   �� *IsYourGroup+   �� 5b   &   $��JoinChat��6-   #��Z 5c   -   $ 	Join Chat��5e   -   $ OrangeBrush��5f   -   $
��Bold"�5g   -   $	����5h   -   $	��8 5q�5i   -   8�� ��5d   -   �� *	GroupInfo+y�5k   -   �� *IsYourGroup+   $
 true���� 5n   &   $��InviteContact��6-   $ LightBlueBrush��5o   -   $ Invite to Chat��5p   -   $	����5q   -   $
��Bold"�5r   -   $	��8 5q�5s   -   8�� 5t   -   5x   #   5z      5|       5      :� 5�      $ 
MouseEnter��6+   [�5�   "   �� 5�   &   .��5�   *   �� .��5�   .   �� $ 0:0:0.1D�6=   $ 	Container��6P   $1 +(Border.Background).(SolidColorBrush.Color)�6r   8  6�   5�   +   5�   '   5�      :� 5�      $ 
MouseLeave��6+   [�5�   "   �� 5�   &   .��5�   *   �� .��5�   .   �� $ 0:0:0.1D�6=   $ 	Container��6P   $1 +(Border.Background).(SolidColorBrush.Color)�6r   8  6�   5�   +   5�   '   5�      5�      5�      5�      5�      .��5�      �� #��[  6   5�      