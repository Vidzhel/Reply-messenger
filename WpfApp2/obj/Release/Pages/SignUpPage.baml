   M S B A M L   `   `   `  ���� =  9UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:UI.PagesUI.Pages  E'clr-namespace:UI.UIPresenter.ViewModelsUI.UIPresenter.ViewModels  E'clr-namespace:UI.UIPresenter.ConvertersUI.UIPresenter.Converters      UI.Pages.SignUpPage   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     (localclr-namespace:UI.Pages     >
viewModels'clr-namespace:UI.UIPresenter.ViewModels     >
converters'clr-namespace:UI.UIPresenter.Converters       N� Title$  
SignUpPage��.���    DarkBgBrush#��[   � ColumnDefinitions �� $��* ��� $��2* �.+��� -   $��SignUpPanel��$��Center=�$��0��#��[  .+���   RegularText#��[ $��	40 0 0 40q�$��Sign Up��$��30(��� -   $��SignUpUserName��
 � Tag$ 	User name��$��0��  	DataField#��[ $��
40 0 40 20q����� *
UserName+�� -   $��SignUpEmail��$ E-mail��$��0��#��[ $��
40 0 40 20q����� *Email+�� -   $��SignUpPassword��$ Password��$��1��#��[ $��
40 0 40 20q��� -   $��SignUpRepeatPassword��$ Repeat Password��$��1��#��[ $��	40 0 40 0q��� #��[   LightRedBrush#��[ $��
40 10 40 0q����� *ErrorMessage+�� -   $��SignUpButton�� �� IsInputMethodEnabled . $��
40 50 40 0q�$��3��  RegularButton#��[ $��SIGN UP������ *SignUpCommand+��`�  `� 	Converter :   4UI.UIPresenter.Converters.ParametersToArrayConverter  .1���  �� ElementName$ SignInPassword�� �� Path$
 Text��� $ SignUpRepeatPassword��$
 Text��� -   $��SignUpLabel��$��4��$��
SignInPage��$
��Left=�  
LinkButton#��[ $��
45 10 0 20q�$��Sign In������ *OpenNextPageCommand+�� $��1��  GrayBGBrush#��[  �� CornerRadius$ 10��._�� 