   M S B A M L   `   `   `  ���� =  9UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:UI.UserControlsUI.UserControls  E'clr-namespace:UI.UIPresenter.ConvertersUI.UIPresenter.Converters  W0clr-namespace:UI.UIPresenter.ViewModels.Commands"UI.UIPresenter.ViewModels.Commands  %    UI.UserControls.ChatUserControl   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     /localclr-namespace:UI.UserControls     >
converters'clr-namespace:UI.UIPresenter.Converters     Ecommands0clr-namespace:UI.UIPresenter.ViewModels.Commands     .����    GrayBGBrush#��[    �� CornerRadius$  10��._��  � ColumnDefinitions �� $
��296* ��� $
��191* � � RowDefinitions �� $
��Auto ��� $��* ��� $
��Auto �.+�� $��2��$��0,0,0,0.333q�.+��� $	��Top=�$��
20 20 20 0q�$��45��  DarkBgBrush#��[ $  7��._��  �� $
��auto ��� $��* ��� $
��auto �.+�� $��20 3 0 3q� J� InputBindings d�  d� MouseAction$ 	LeftClicke�7   1UI.UIPresenter.ViewModels.Commands.CommonCommands   OpenGroupInfo �� Command# Z  �� CommandParameter �� *CurrentChat+.+��� -   $��Photo��$��35��$��35��#��[  $  25���� $��20 3 0 3q�$��1�� �� Orientation$ Vertical=�.+���   LightGrayBgBrush#��[ $��DemiBold"�$��13(����� *
ChatName+�� $��	0 -10 0 0q�$��10(����� *UsersOnline+	 �� 	Converter	 9   3UI.UIPresenter.Converters.IntToUsersOnlineConverter  ���� *UsersOnline+	 ;   5UI.UIPresenter.Converters.UsersOnlineToColorConverter  �� $��2��$��0 0 10 0q�$ 
Horizontal=�.+��� $��25��$��25��#��[  $  15��$��5 0 5 0q��� $��25��$��25��#��[  $  15��$��5 0 5 0q��� $��25��$��25��#��[  $  15��$��5 0 5 0q�!   UI.UserControls.MessageList  -   
  Name$
 ScrollMessagesList��$��1��$��40,4.667q�$��2�� � DataContext �� *MessageList+� $��2��$��2��$��0,0.333,0,0q�.+��� $	��Top=�$��
20 0 20 20q�$��45��$	��200��#��[ $  7��._��  �� $
��auto ��� $��* ��� $
��auto �.+��� $��
20 10 0 10q�$��Bottom=�$	����
 � Tag$ LightBlueBrush��$��20(�  IconicButton#��[ �� -   $��MessageTextBox��$��1��  MessageTypingBox#��[  �� TextWrapping$
 Wrap=����� *MessageContent+ �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=��� $��	20 0 30 0q�$��Bottom=�$��2��.+��� $	����#��[ $��20(� �� 	IsDefault . $ OrangeBrush��$��	0 10 0 10q�$��Black"�  LightBlueBrush#��[ ���� *SendMessage+