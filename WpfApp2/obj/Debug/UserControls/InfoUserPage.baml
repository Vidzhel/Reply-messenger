   M S B A M L   `   `   `  ����=  9UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:UI.UserControlsUI.UserControls  5      E'clr-namespace:UI.UIPresenter.ViewModelsUI.UIPresenter.ViewModels  5      E'clr-namespace:UI.UIPresenter.ConvertersUI.UIPresenter.Converters  5	      W0clr-namespace:UI.UIPresenter.ViewModels.Commands"UI.UIPresenter.ViewModels.Commands  5
      %    UI.UserControls.InfoUserControl   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      /localclr-namespace:UI.UserControls     5      >
viewModels'clr-namespace:UI.UIPresenter.ViewModels     5      >
converters'clr-namespace:UI.UIPresenter.Converters     5	      Ecommands0clr-namespace:UI.UIPresenter.ViewModels.Commands     5
      -   5          >�Name$
  Page��5      .��5      ��    GrayBGBrush#��[  6    �� CornerRadius$ 10��67   ._�5   
   �  � ColumnDefinitions 5      �� 5      $��* �6#   �� 5      $
��auto �6#   .+�5       �� $��20 80 10 80q�6     TabControlStyle1#���  5!      .@�5$      ��   CustomTabPanel#��[ 5%      $��Contacts��5&      y�5'      �� *IsYourAccaunt+ �� 	Converter 9  3UI.UIPresenter.Converters.BoolToVisibilityConverter  .��5)      "   UI.UserControls.ContactsList   � DataContext 6)   �� *ContactsList+5*      �� 5-      #��[ 5.      $��Create Group��5/      y�50      �� *IsYourAccaunt+   .��52      �� $��Center=�6!   $��50 20q�6<   .+�55      �� -   5        $��	GroupName��57   "   
 � Tag$ 
Group Name��56   "   $	��300��58   "   $
��Left=�59   "   $��0 0 0 30q�5:   "     RegularTextBox#��[ 5;   "   �� 5=      $��Chat Accessability��6      RegularText#��[ 5>         LightGrayBgBrush#��[ 5?       $��15(�5@       $��
-8 10 0 10q�5A       $��Medium"�5B       �� 5D       �� Orientation$ 
Horizontal=�6%   .+�5F      	� -   5        $��IsPrivateChat��5G   *   $��Private��5F   *   $ IsPrivateChat��5H   *   $��0 0 30 0q�5I   *     RadioButtonRegular#��[ 5J   *   	� 5L      -   5        $��IsPublicChat��5M   *   $��Public��5L   *   $ IsPublicChat��5N   *   #��[ 5O   *   5Q      �� 5S      $��	Chat Type��6    #��[ 5T       #��[ 5U       $��15(�5V       $��
-8 10 0 10q�5W       $��Medium"�5X       �� 5[      $ 
Horizontal=�6%   .+�5]      	� -   5        $��IsChat��5^   *   $
��Chat��5]   *   $ IsChat��5_   *   $��0 0 30 0q�5`   *   #��[ 6<   	� 5b      -   5        $��	IsChannel��5c   *   $��Channel��5b   *   $ 	IsChannel��5d   *   #��[ 5e   *   5g      �� 5j      $��	0 10 0 20q�6    #��[ 5k         LightRedBrush#��[ 5l       ��5m       �� *CreateGroupErrorMessage+�� 5o      $��Right=�6!   $	��150��5p   !   $��Bottom=�5q   !   $��0 20 0 0q�5r   !   $��30��5t   !     RegularButton#��[ 5u   !   $��Create Goup��5v   !   ��5s   !   �� *CreateGroup+��5x      `� 5z   "    `� 	Converter 6/   :   4UI.UIPresenter.Converters.ParametersToArrayConverter  .1�5|   &   ��  �� ElementName$ 	GroupName��6.   	 �� Path$
	 Text�6F   �� 5}   &   $ IsPrivateChat��6.   $	 	IsChecked�6J   �� 5~   &   $ IsPublicChat��6.   $	 	IsChecked�6I   �� 5   &   $ IsChat��6.   $	 	IsChecked�6C   �� 5�   &   $ 	IsChannel��6.   $	 	IsChecked�6F   5�   #   5�      5�      5�      �� 5�      #��[ 5�      $��Change profile info��5�      y�5�      �� *IsYourAccaunt+   .��5�      �� $��50 20q�6!   $��Center=�5�   !   .+�5�      �� -   5        $��	NameField��5�   "   $ Change Name��5�   "   $	��300��5�   "   $
��Left=�5�   "   $��0 0 0 30q�5�   "   #��[ 5�   "   �� 5�      -   5        $��
EmailField��5�   "   $ Change E-mail��5�   "   $
��Left=�5�   "   $	��300��5�   "   $��0 0 0 30q�5�   "   #��[ 5�   "   �� 5�      -	   5        $��BioField��5�   "   $ 
Change Bio��5�   "   $
��Left=�5�   "   $	��300��5�   "   $��0 0 0 10q�5�   "   #��[ 5�   "   �� 5�      $��0 0 0 20q�6    #��[ 5�       #��[ 5�       ��5�       �� *ChangeUserInfoErrorMessage+�� 5�      $��Right=�6!   $��Bottom=�5�   !   $	��130��5�   !   $��0 30 0 0q�5�   !   $��30��5�   !   #��[ 5�   !   $��Save changes��5�   !   ��5�   !   �� *ChangeUserInfo+��5�      `� 5�   "    6/     .1�5�   &   �� $ 	NameField��6.   $
	 Text�6F   �� 5�   &   $ 
EmailField��6.   $
	 Text�6G   �� 5�   &   $ BioField��6.   $
	 Text�6E   5�   #   5�      5�      5�      �� 5�      #��[ 6   $��Groups��5�      y�5�      �� *IsYourAccaunt+   
 �� ConverterParameter$

 true��.��5�          UI.UserControls.GroupsList   6'   �� *
GroupsList+5�      �� 5�      #��[ 5�      $��Change password��5�      y�5�      �� *IsYourAccaunt+   .��5�      �� $��50 20q�6!   $��Center=�5�   !   .+�5�      G� -
   5        $��OldPassField��5�   "   $ Old Password��5�   &   $	��300��5�   "   $
��Left=�5�   "   $��0 0 0 30q�5�   "    	 RegularPasswordBox#��[	 5�   "   G� 5�      -   5        $��NewPassField��5�   "   $ New Password��5�   &   $	��300��5�   "   $
��Left=�5�   "   $��0 0 0 30q�5�   "   #��[	 5�   "   G� 5�      -   5        $��RepeatPassField��5�   "   $ Repeat New Password��5�   &   $
��Left=�5�   "   $	��300��5�   "   $��0 0 0 10q�5�   "   #��[	 5�   "   �� 5�      $��0 0 0 20q�6    #��[ 5�       #��[ 5�       ��5�       �� *ChangePassErrorMessage+�� 5�      $��Right=�6!   $	��150��5�   !   $��Bottom=�5   !   $��0 30 0 0q�5  !   $��30��5  !   #��[ 5  !   $��Change Password��5  !   ��5  !   �� $
 Page����5  !   �� *ChangeUserPass+5  !   5	     5     5     �� 5     $��Center=�6   $��1��5     $
��20 0q�5      
 DarkBgBrush#��[
 5     $	��240��5     $ 10��5     ._�5     �� $��10 20q�6   .+�5     �� #��[  6   $ 10��6G   $	��150��6Y   ._�5     �� $ 
Horizontal=�6%   $��	30 2 30 2q�5  %   $��Center=�5  %   $��Center=�5  %   .+�5      �� $	����6$   $��Black"�5!  $   $��0 0 10 0q�5"  $   $��12(�5#  $   $��Center=�5$  $    
 Sumbol#��[ 5%  $   �� 5'     $��	User Info��6$   $��Center=�5(  $   #��[ 5)  $   #��[ 5*  $   5,     5-     �� 50     -   5         ��Name$ ProfilePhotoMask��54     $��0 20 0 0q�51     $��Center=�52     $��Center=�53     $ 50��55     $��5q�56     #��[  57      J� InputBindings 5:     d� 5;      d� MouseAction$ 	LeftClicke�6+    �� Command 6C   �� *ChangeProfilePhoto+._�5=     � $��Center=�6   $	��100��5>     $	��100��5?     $��Center=�5@      J� Clip 5A     @� 5B  "    @� Center$ 50,50&�62    @� RadiusX$ 50Q�6A    @� RadiusY$ 50Q�6N   .+�5D     �� $��Center=�5E  $   $��Center=�5F  $   ��5D  $   �� *ProfilePhoto+5F  $   5H     5I     �� 5L     #��[ 6   $��0 5 0 0q�5N     $��Center=�5O     $��15(�5P     #��[ 5Q     ��5M     �� *
UserName+5Q     �� 5T     #��[ 6   $��Center=�5W     $��	0 -10 0 0q�5X     $��10(�5Y       MiddleGrayBgBrush#��[ 5Z     ��5U     �� *Online+ 3   -UI.UIPresenter.Converters.DateToTextConverter  y�5V     �� *IsYourAccaunt+   $

 true��5Z     �� 5]     $��0 10 0 0q�6!   $��Center=�5_  !   $ 
Horizontal=�5`  !   y�5^  !   �� *IsYourAccaunt+   $

 true��.+�5c     �� $	����6!    � ToolTip$ Add to the contacts list��5d  !   7   1UI.UIPresenter.ViewModels.Commands.CommonCommands   AddToContactList#��Z 5e  !   $��Center=�5g  !   $��Center=�5h  !   $��Black"�5i  !   $��8q�5k  !   $��14(�5l  !   $ OrangeBrush��5m  !     IconicButton#��[ 5n  !   ��5f  !   �� *
UserInfo+y�5j  !   �� *IsYourContact+   $

 true��5n  !   �� 5r     $	����6!   $# Remove from the contacts list��5s  !   $��Center=�5t  !   $��Center=�5v  !      DeleteFromContactList#��Z 5w  !   $��8q�5y  !   $��14(�5z  !   $ LightRedBrush��5{  !   #��[ 5|  !   y�5u  !   �� *IsYourContact+   ��5x  !   �� *
UserInfo+5|  !   �� 5�     $	����6!      	StartChat#��Z 5�  !   $��Center=�5�  !   $��Center=�5�  !   $��8q�5�  !   $ 
Start Chat��5�  !   $��14(�5�  !   $��Black"�5�  !   $ LightBlueBrush��5�  !   #��[ 5�  !   ��5�  !   �� *
UserInfo+5�  !   5�     �� 5�     $ 
Horizontal=�6!   $��0 10 0 0q�5�  !   $��Center=�5�  !   .+�5�     �� #��[  6!   $ 20��5�  !   $��0 0 10 0q�5�  !   $��Center=�5�  !   ._�5�     �� $	����6$   $��12(�5�  $     LightBlueBrush#��[ 5�  $   $��Center=�5�  $   $��Center=�5�  $   #��[ 5�  $   5�     �� 5�     #��[  6!   $ 5��5�  !   $	��180��5�  !   ._�5�  "   �� $��Center=�5�  ,   $
��10 5q�5�  ,   $
��Wrap=�5�  ,   $��10(�5�  ,   #��[ 5�  ,   ��5�  ,   �� *Bio+5�  ,   5�     5�     �� 5�     $ 
Horizontal=�6!   $��0 10 0 0q�5�  !   $��Center=�5�  !   .+�5�     �� #��[  6!   $ 20��5�  !   $��0 0 10 0q�5�  !   $��Center=�5�  !   ._�5�     �� $	����6$   $��12(�5�  $   #��[ 5�  $   $��Center=�5�  $   $��Center=�5�  $   #��[ 5�  $   5�     �� 5�     #��[  6!   $ 5��5�  !   $	��180��5�  !   ._�5�  "   �� $��Center=�5�  ,   $
��10 5q�5�  ,   $
��Wrap=�5�  ,   $��10(�5�  ,   #��[ 5�  ,   ��5�  ,   �� *Email+5�  ,   5�     5�     5�     5�     5�     5�     5�     