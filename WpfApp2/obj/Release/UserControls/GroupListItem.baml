   M S B A M L   `   `   `  ���� =  9UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:UI.UserControlsUI.UserControls  E'clr-namespace:UI.UIPresenter.ConvertersUI.UIPresenter.Converters  W0clr-namespace:UI.UIPresenter.ViewModels.Commands"UI.UIPresenter.ViewModels.Commands  #    UI.UserControls.GroupListItem   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     /localclr-namespace:UI.UserControls     >
converters'clr-namespace:UI.UIPresenter.Converters     Ecommands0clr-namespace:UI.UIPresenter.ViewModels.Commands       � 	Resources   �� 
TargetType   Content �� Property �� Value  GrayBGBrush �� CornerRadius J� InputBindings d� MouseAction7   1UI.UIPresenter.ViewModels.Commands.CommonCommands   OpenUserInfo �� Command	 �� CommandParameter
 � ColumnDefinitions �� Orientation  DarkBgBrush  LightGrayBgBrush
 � Tag   	LeaveChat  IconicButton �� 	Converter9  3UI.UIPresenter.Converters.BoolToVisibilityConverter   JoinChat �� ConverterParameter �� Triggers :� RoutedEvent g� Duration �� 
TargetName �� TargetProperty  GrayBGHover	 �� To 
 GrayBG��%
  &	        7  7  7  7  7  7  7  7  �� " ��.���  � �� �� .R��� $��	Container��8��  $ 5��$	��0 3q� d� $ 	LeftClicke�# Z 	 �� *
UserInfo+._�� 
 �� $��* ��� $
��auto �.+��� $ 
Horizontal=�.+�� .+��� $��30��$
��10 5q�$��30��8�� $ 20���� $��Center=�8�� ���� *	GroupName+�� $ 
Horizontal=�$
��10 0q�$��1��.+��� $��	LeaveChat��$ LightRedBrush��#��Z $	����$	��8 5q�$��Black"�8�� ���� *	GroupInfo+y��� *IsYourGroup+   �� $��JoinChat��#��Z $ OrangeBrush��$
��Bold"�$	����$	��8 5q�8�� ���� *	GroupInfo+y��� *IsYourGroup+   $
 true���� $��InviteContact��$ LightBlueBrush��$	����$
��Bold"�$	��8 5q�8��  :� $ 
MouseEnter��[��� .���� .���� $ 0:0:0.1D�$ 	Container��$1 +(Border.Background).(SolidColorBrush.Color)�8  :� $ 
MouseLeave��[��� .���� .���� $ 0:0:0.1D�$ 	Container��$1 +(Border.Background).(SolidColorBrush.Color)�8  .���� #��[  