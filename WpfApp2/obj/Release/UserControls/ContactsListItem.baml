   M S B A M L   `   `   `  ���� =  9UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:UI.UserControlsUI.UserControls  E'clr-namespace:UI.UIPresenter.ConvertersUI.UIPresenter.Converters  W0clr-namespace:UI.UIPresenter.ViewModels.Commands"UI.UIPresenter.ViewModels.Commands  &     UI.UserControls.ContactsListItem   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     /localclr-namespace:UI.UserControls     >
converters'clr-namespace:UI.UIPresenter.Converters     Ecommands0clr-namespace:UI.UIPresenter.ViewModels.Commands       � 	Resources   �� 
TargetType   Content �� Property �� Value  GrayBGBrush �� CornerRadius J� InputBindings d� MouseAction7   1UI.UIPresenter.ViewModels.Commands.CommonCommands   OpenUserInfo �� Command	 �� CommandParameter
 � ColumnDefinitions �� Orientation  DarkBgBrush  LightGreenBrush K� ZIndex �� 	Converter9  3UI.UIPresenter.Converters.BoolToVisibilityConverter  LightGrayBgBrush �� ConverterParameter
 � Tag � ToolTip   InviteUserToChat  IconicButton `� 	Converter:   4UI.UIPresenter.Converters.ParametersToArrayConverter �� Path   RemoveUserFromChat   DeleteFromContactList   AddToContactList   	StartChat �� Triggers :� RoutedEvent g� Duration �� 
TargetName �� TargetProperty  GrayBGHover	 �� To 
 GrayBG��%�  &	        7  7  7  7  7  7  7  7  7  7  7  7  �� " ��.���  � �� �� .R��� $��	Container��8��  $ 5��$	��0 3q� d� $ 	LeftClicke�# Z 	 �� *
UserInfo+._�� 
 �� $��* ��� $
��auto �.+��� $ 
Horizontal=�.+�� .+��� $��30��$
��10 5q�$��30��8�� $ 20���� 8�� $��1q�$��Bottom=�$��Right=�$��	0 0 10 10q�8�� $��10��$��10��$ 5��$ 2��y��� *
IsOnline+   �� $��Center=�8�� ���� *
UserName+�� $ 
Horizontal=�$
��10 0q�$��1��y��� *IfYou+   $
 true��.+��� $��InviteToGroup��$ OrangeBrush��$ Invite to the chat��#��Z $	����$	��8 5q�8�� y��� *IsInviteList+   ��`�    .1��� $ UserInfo��� $ 	GroupInfo�� y��� *AreYouAdmin+   .+��� $��RemoveFromGroup��$ Remove from the chat��$ LightRedBrush��#��Z $	����$	��8 5q�8�� y��� *IsGroupContactsList+   ��`�    .1��� $ UserInfo��� $ 	GroupInfo��� $��DelateContact��$ Remove from the contacts��$ LightRedBrush��#��Z $	����$	��8 5q�8�� ���� *
UserInfo+y��� *IsYourContact+   �� $��
AddContact��#��Z $ Add to the contacts��$ OrangeBrush��$
��Bold"�$	����$	��8 5q�8�� ���� *
UserInfo+y��� *IsYourContact+   $
 true���� $��	StartChat��#��Z $ 
Start chat��$ LightBlueBrush��$	����$
��Bold"�$	��8 5q�8��	 ���� *
UserInfo+ :� $ 
MouseEnter��[��� .���� .���� $ 0:0:0.1D�$ 	Container��$1 +(Border.Background).(SolidColorBrush.Color)�8 
 :� $ 
MouseLeave��[��� .���� .���� $ 0:0:0.1D�$ 	Container��$1 +(Border.Background).(SolidColorBrush.Color)�8  .���� #��[  