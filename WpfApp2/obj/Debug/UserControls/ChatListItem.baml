   M S B A M L   `   `   `  ����=  9UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullE'clr-namespace:UI.UIPresenter.ConvertersUI.UIPresenter.Converters  5      1clr-namespace:UI.UserControlsUI.UserControls  5      E'clr-namespace:UI.UIPresenter.ViewModelsUI.UIPresenter.ViewModels  5	      W0clr-namespace:UI.UIPresenter.ViewModels.Commands"UI.UIPresenter.ViewModels.Commands  5
      "    UI.UserControls.ChatListItem   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      >
converters'clr-namespace:UI.UIPresenter.Converters     5      /localclr-namespace:UI.UserControls     5      =	viewModel'clr-namespace:UI.UIPresenter.ViewModels     5	      Ecommands0clr-namespace:UI.UIPresenter.ViewModels.Commands     5
      $��60��5        � 	Resources  5       �� 
TargetType   ContentControl �� Property �� Value �� 
TargetType  DarkBgBrush �� CornerRadius J� InputBindings d� MouseAction7   1UI.UIPresenter.ViewModels.Commands.CommonCommands   OpenChat	 �� Command
 �� CommandParameter � ColumnDefinitions  LightGrayBgBrush  MiddleGrayBgBrush  RegularText �� 	Converter3   -UI.UIPresenter.Converters.DateToTextConverter   OpenGroupInfo J� Clip @� Center @� RadiusX @� RadiusY  GrayBGBrush 
 Sumbol  LightBlueBrush<   6UI.UIPresenter.Converters.MessageStatusToIconConverter �� Triggers �� Value �� Binding �� 
TargetName :� RoutedEvent g� Duration �� 
TargetName �� TargetProperty  GrayBGHover	 �� To��%�	  &	        7  7  7  7  7  7  7  7  7  7  7  7  �� 5   
   " ��6   .�5      ��  � �� 5      �� 5      " ��6&   .R�5      �� $��
background��6!   8��  $ 5 0 0 5��6^    5      d� 5   "   $ 	LeftClicke�6/   #	 Z 6G   
 6}   �� *	GroupData+._�5      �� $��	container��6%   
������ $ 5 0 0 5��6P   ._�5       �  5!      �� 5$   "   $��* �63   �� 5'   "   $��3* �63   .+�5,      � $��1��6#    5.   "   �� 5/   &   $��* �67   �� 50   &   $��10 �67   �� 51   &   $
��Auto �67   .+�55   "   �� 8�� $��CharacterEllipsis=�57   ,   $��0 3 0 0q�58   ,   $��13(�59   ,   ��55   ,   �� *
UserName+59   ,   �� 5<   "   $��2��6(   $��0 3 10 0q�68   $��10(�6J   $	��Top=�5=   (   $��Right=�6G   8�� 8�� ��5@   (   �� *LastMessageTime+   5B      �� 5F   &   $��40��6-   $
��10 5q�5G   !   $��40��5H   !   8�� $ 20��5J   !    5L   *   d� 5M   .   $ 	LeftClicke�6;   #	 Z 6S   
 6�   �� *	GroupData+._�5P   *   � $��Center=�6/   $��40��5Q   -   $��40��5R   -   $��Center=�5S   -    5T   .   @� 5U   2   $ 20,20&�6B   $ 20Q�6Q   $ 20Q�6^   .+�5W   .   �� $��Center=�5X   1   $��Center=�5Y   1   ��5W   4   �� *
GroupPhoto+5Y   1   5[   +   5]   '   �� 5`      $��	MessageBG��6%   $��1��66   $��30��6F   $��Bottom=�6R   $��0 0 10 7q�6m   $ 
10 10 10 2��6�   8�� ._�5a   "   �  5d   &   �� 5e   *   $��* �6;   �� 5f   *   $��27 �6;   .+�5j   &   �� $��10 0 0 0q�60   $��13(�5k   0   $��Center=�5l   0   $��WordEllipsis=�5m   0   8�� ��5n   0   �� *LastMessageText+5o   0   �� 5r   &   8�� $��1��5s   ,   $��0 0 4 0q�5t   ,   8�� $��Right=�5v   ,   $	��Top=�6H   $��9(�5w   ,   ��5x   ,   �� *MessageStatus+   5z   #   5{      5}      5      5�       5�      �� 5�      $
 true��6I    6*   �� *
IsSelected+.a�5�   "   �� $ 
background��6?   8 	  � ��6W   �� 5�   "   $ 	MessageBG��6?   8 
  � ��6V   5�      :� 5�      $ 
MouseEnter��6+   .[�5�   "   �� .��5�   &   �� .��5�   *   �� $ 0:0:0.01D�69   $ 	container��6M   $ Background.Color�5�   >   8  5�   >   5�   '   5�   #   5�      :� 5�      $ 
MouseLeave��6+   .[�5�   "   �� .��5�   &   �� .��5�   *   �� $ 0:0:0.01D�69   $ 	container��6M   $ Background.Color�5�   >   $ TransparentX�5�   >   5�   '   5�   #   5�      5�      5�      5�      5�      .��5�      �� #��[  6   5�      