   M S B A M L   `   `   `  ���� =  9UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:UI.Styles	UI.Styles  E'clr-namespace:UI.UIPresenter.ConvertersUI.UIPresenter.Converters  W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089  �� 
TargetType   RegularButton �� Property  LightBlueBrush �� Value �� 
TargetType �� CornerRadius �� Triggers ]� 
Conditions �� Property �� Value	 �� 
TargetName3   -System.Windows.Media.Effects.DropShadowEffect
    ShadowDepth    	Direction    Color    Opacity    
BlurRadius  
LinkButton ��Name J� SnapsToDevicePixels ��Name P� Property P� Value  OrangeBrush  GrayBGBrush  IconicButton  MiddleGrayBgBrush  LightGrayBgBrush �� Path �� RelativeSource �� Mode �� 	Converter@   :UI.UIPresenter.Converters.ResourceKeyToColorBrushConverter  WindowSystemButtons'  !System.Windows.Shell.WindowChrome K� ZIndex J� Opacity 	 DarkBlueBrush N� EnterActions g� Duration �� 
TargetName �� TargetProperty	 W� To N� ExitActions  �� BasedOn 
 CloseWindowButton  LightRedBrush  MaximizeWindowButton  LightGreenBrush�� D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    :x,http://schemas.microsoft.com/winfx/2006/xaml     !localclr-namespace:UI.Styles  6
converters'clr-namespace:UI.UIPresenter.Converters  %�	  &	        7  7  &	 �    7  7  &	 �    7  7  &	 �    7  7 	 &	
 	    7  7  &	 5	    7  7  �� "  ��.���  � ��8   ��  � ��$ OpenSans)���  � ��$ Center=���  � ��$ Center=���  � ��$	 250����  � ��$ 40����  � ��$ 15(���  � ��
 �������  � �� �� " ��.R�� .+��� $��ButtonBackground��$ 3��8�� ._��� $��Center=�$��Center=� ]�  ��  �@J�IsMouseOver . ��  �@J�IsKeyboardFocused . /��� $	 ButtonBackground�� � �� �#5F6AFF��  �@�Cursor$
 Hand����  �@J�Effect    $
 4Q�$	 270Q�$ Black��$	 0.3Q�$ 6Q�]�  ��  � �� . /��� $	 ButtonBackground�� � �� �#3D49E3��  �@�Cursor$
 Hand���� "  ��.���  � ��
 ���� ��  � ��
 ���� ��  � ��
 �������  � ��$ 0q���  � ��$ Center=���  � ��$ Center=���  � ��$ 0q���  � �� �� " ��.R��� $ border��#��z��#��z��#��z�� . ._��� $ contentPresenter��|�.  #��z��#��z����. # z #��z�� P�  �@J�IsMouseOver . . ��� $	 contentPresenter��8    � ��P�  � �� . P�  � {� .  . ��� $	 contentPresenter��8   � ���� "  ��.���  � ��
 ���� ��  � ��
 �������  � ��8   ��  � ��$ 0q���  � ��$ Font Awesome 5 Pro)���  � ��$ Center=���  � ��$ Center=���  � ��$ 1q���  � �� �� " ��.R��� $ border��#��z��#��z��#��z�� . ._��� $ contentPresenter��|�.  #��z��#��z����. # z #��z�� P�  �@J�IsMouseOver . . ���  �@�Cursor$
 Hand���� 8   � ��P�  � �� . . ���  �@�Cursor$
 Hand����  �� $	 Tag� �� $
 Self=�    � ���� "  ��.���  � ��$ 10����  � ��$ 5 0 0 2q���  � ��8   ��   �@ IsHitTestVisibleInChrome . ��  � �� �� .R�� .+��� $��Overlay��$ 2��
�������$ 7��$ 0Q��� $��DarkOverlay��$ 2��8�� $ 7��$ 0Q��� #��z��$ 7��._�A� #��z��#��z�� P�  �@J�IsMouseOver .  �� .���� .��W� $ 0:0:0.1D�$ Overlay��$ (UIElement.Opacity)�$	 0.2X� �� .���� .��W� $ 0:0:0.1D�$ Overlay��$ (UIElement.Opacity)�$ 0X�P�  � �� . . ��� $	 DarkOverlay�� �@J�Opacity$	 0.2Q��� "  ��8    .���  � ��8  �� "  ��8    .���  � ��8  