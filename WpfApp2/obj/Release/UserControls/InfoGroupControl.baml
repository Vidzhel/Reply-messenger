   M S B A M L   `   `   `  ���� =  9UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:UI.UserControlsUI.UserControls  E'clr-namespace:UI.UIPresenter.ConvertersUI.UIPresenter.Converters  W0clr-namespace:UI.UIPresenter.ViewModels.Commands"UI.UIPresenter.ViewModels.Commands  &     UI.UserControls.InfoGroupControl   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     /localclr-namespace:UI.UserControls     >
converters'clr-namespace:UI.UIPresenter.Converters     Ecommands0clr-namespace:UI.UIPresenter.ViewModels.Commands     .����    GrayBGBrush#��[    �� CornerRadius$  10��._��  � ColumnDefinitions �� $��* ��� $
��auto �.+��� $��20 80 10 80q�  TabControlStyle1#���  y��� *IsYourAccaunt+ �� 	Converter 9  3UI.UIPresenter.Converters.BoolToVisibilityConverter  .@���   CustomTabPanel#��[ $��Members��.��"   UI.UserControls.ContactsList   � DataContext �� *ContactsList+�� #��[ $��Shared Media���� #��[ $��Shared Documents���� #��[ $��Change Chat Info��y��� *AreYouAdmin+   .���� $��50 20q�.+��� -   $��	GroupName��
 � Tag$ 
Group Name��$	��300��$
��Left=�$��0 0 0 30q�  RegularTextBox#��[ �� $��Chat Accessability��  RegularText#��[   LightGrayBgBrush#��[ $��15(�$��
-8 10 0 10q�$��Medium"���  �� Orientation$ 
Horizontal=�.+�	� $��Private��$��0 0 30 0q�  RadioButtonRegular#��[ 	� $��Public��#��[ �� $��	Chat Type��#��[ #��[ $��15(�$��
-8 10 0 10q�$��Medium"��� $ 
Horizontal=�.+�	� $
��Chat��$��0 0 30 0q�#��[ 	� $��Channel��#��[ �� $��0 0 0 20q�#��[   LightRedBrush#��[ ���� *ChangePassErrorMessage+�� $��Right=�$	��150��$��Bottom=�$��0 30 0 0q�$��30��  RegularButton#��[ $��Change Password������ *ChangeUserPass+��`�  `� 	Converter :   4UI.UIPresenter.Converters.ParametersToArrayConverter  .1���  �� ElementName$ OldPassField�� �� Path$
 Text��� $ NewPassField��$
 Text��� $ RepeatPassField��$
 Text��� $��Center=�$��1��$
��20 0q� 	 DarkBgBrush#��[	 $	��240��$  10��._��� $��10 20q�.+��� #��[  $  10��$	��150��._��� $ 
Horizontal=�$��	30 2 30 2q�$��Center=�$��Center=�.+��� $	����$��Black"�$��0 0 10 0q�$��12(�$��Center=� 

 Sumbol#��[
 �� $��	Chat Info��$��Center=�#��[ #��[ �� $	��100��$	��100��$��0 20 0 0q�$  50��#��[  �� #��[ $��0 5 0 0q�$��Center=�$��15(�#��[ ���� *	GroupName+�� #��[ $��Center=�$��	0 -10 0 0q�$��10(����� *
IsOnline+ 9   3UI.UIPresenter.Converters.IntToUsersOnlineConverter  y��� *IsYourAccaunt+   	 �� ConverterParameter$
	 true������ *
IsOnline+ ;   5UI.UIPresenter.Converters.UsersOnlineToColorConverter  �� $��0 10 0 0q�$��Center=�$ 
Horizontal=�.+��� $	����7   1UI.UIPresenter.ViewModels.Commands.CommonCommands
   JoinChat#��Z
 $��Center=�$��Center=�$��Black"�$��8q�$��14(�$ OrangeBrush��  IconicButton#��[ ���� *	GroupInfo+y��� *IsYourGroup+   $
	 true���� $	����$��Black"�$��Center=�$��Center=�   	LeaveChat#��Z $��8q�$��14(�$ LightRedBrush��#��[ y��� *IsYourGroup+   ���� *	GroupInfo+�� $	����$��Black"�$��Center=�$��Center=�$��8q�$��14(�$ LightBlueBrush��#��[ 