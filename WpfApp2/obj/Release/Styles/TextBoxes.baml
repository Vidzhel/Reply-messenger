   M S B A M L   `   `   `  ���� =  9UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullE'clr-namespace:UI.UIPresenter.ConvertersUI.UIPresenter.Converters  W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0890    *UI.UIPresenter.Converters.FieldPlaceholder   PlaceholderControl  �� 
TargetType  	DataField �� Property �� Value  LightBlueBrush �� 
TargetType ��Name �� CornerRadius J� SnapsToDevicePixels `� 	Converter8   2UI.UIPresenter.Converters.ControlStateToColorBrush �� Path	 �� RelativeSource
 �� Mode �� AncestorType ��Name 
 Sumbol J� RenderTransformOrigin �� ElementName �� 	Converter*   $UI.UIPresenter.Converters.TextToIcon T� X T� Y J� IsHitTestVisible
 � Tag  LightGrayBgBrush  DarkBgBrush �� Triggers �� Value �� Binding N� EnterActions g� Duration �� TargetProperty	 W� To N� ExitActions �� Triggers P� Property P� Value �� 
TargetName  ]� 
Conditions! �� Property" �� Value  	SearchBox  GrayBGBrush-  'System.Windows.Input.KeyboardNavigation  MiddleGrayBgBrush 	 RegularText 
 RegularTextBox# �� 	Converter  MessageTypingBox�� D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    :x,http://schemas.microsoft.com/winfx/2006/xaml     6
converters'clr-namespace:UI.UIPresenter.Converters  %D  &	        &	      7  7  7  7  7  7  7  &	 �    7  7  7  7  7  7  7 	 &	
 ^    7  7  7  7 	 7  7  &	 Q    7  7  7  7 	    �� "  ��.���  � ��$ Center=���  � ��$ Center=���  � ��$ 5q���  � ��$ 14(���  � ��$ 30 0 0 0q���  � ��$ 35����  � ��$	 250����  �@��
CaretBrush8   ��  � ��$ 	Open Sans)���  � ��
 �������  � ��$ 2q���  � ��
 ���� ��  � �� �� " ��.R�� .+��� $ border��$ 2��#��z��#��z�� . ��`�    .1��� $ 
FieldState��� $	 Tag�	 �� $
 FindAncestor=�$ TextBoxL�._��� $ PART_ContentHost��|�.  $��Hidden=�$��Hidden=��� $��Center=�8�� $ 0.5,0.5&����� $ border��$ BorderBrush����� *Tag+   z�V� .~��� �� �� T� $ 5Q�$ 0Q���  .  $
��Auto��$��15��$
��Left=�#��z ���� $ border��$ BorderBrush����� "  ��.���  � ��8  ��  � ��8  ��  � ��$ Center=���  � ��$ Center=���  � ��$ 12(���  � ��$ 5 0 5 0q���  � z� V� .~�T� Move��$ 35Q�$ 0Q� ��  True `�     .1��� $ 	IsFocused�	 �� $
 FindAncestor=�$ TextBoxL��� $ Text.IsEmpty�	 �� $
 FindAncestor=�$ TextBoxL� �� .���� .��W� $ 	0:0:0.100D�$U O(UIElement.RenderTransform).(TransformGroup.Children)[0].(TranslateTransform.X)�$ 15X�W� $ 	0:0:0.100D�$U O(UIElement.RenderTransform).(TransformGroup.Children)[0].(TranslateTransform.Y)�$	 -18X� �� .���� .��W� $ 0:0:0.1D�$U O(UIElement.RenderTransform).(TransformGroup.Children)[0].(TranslateTransform.X)�$ 35X�W� $ 0:0:0.1D�$U O(UIElement.RenderTransform).(TransformGroup.Children)[0].(TranslateTransform.Y)�$ 0X� P�  �@J�IsMouseOver . . ��� $ border��8   � ����  �@�Cursor$ IBeam��P�  �@J�IsKeyboardFocused . . ��� $ border��8   � �� ]�   �� +! �@��#IsInactiveSelectionHighlightEnabled" . �� ! �@��IsSelectionActive" .  ./���  �@��SelectionBrush8  �� "  ��.���  � ��8   ��  � ��8  ��  �@��
CaretBrush8  ��  � ��$ 40 0 0 0q���  � ��$ 35����  � ��$ 14(���  � ��$ 0q���  �@ TabNavigation$
 None=���  � ��$
 Left=���  � ��$ Center=���  �@�FocusVisualStyle Z� ��  �@J�	AllowDrop . ��  �@��PanningMode$ VerticalFirst=���  �@��IsFlicksEnabled .  ��  � �� �� " ��.R�� .+��� $ border��$ 5��#��z��#��z��#��z�� . ._��� $ PART_ContentHost��|�.  $��Hidden=�$��Hidden=��� $��5 0 0 0q�$��15(�8�� 8�� $	����$��Center=��� $��Placeholder��$��35 0 0 0q�$��14(�8�� 8�� $��	Search...��$��Center=� P�  � {� .  . ��� $ border�� �@J�Opacity$
 0.56Q�P�  �@J�IsMouseOver . . ���  �@�Cursor$ IBeam���� $
 True�� `�     .1��� $ 	IsFocused�	 �� $

 Self=��� $ Text.IsEmpty�	 �� $

 Self=�.a��� $ Placeholder�� � y�$ 	Collapsed=� ]�   �� +! �@��#IsInactiveSelectionHighlightEnabled" . �� ! �@��IsSelectionActive" .  ./���  �@��SelectionBrush# � /��� "  ��.���  � ��
 ���� ��  � ��8   ��  �@��
CaretBrush8  ��  � ��$ 40 0 0 0q���  � ��$ 35����  � ��$ 14(���  � ��$ 0,0,0,2q���  �@ TabNavigation$
 None=���  � ��$
 Left=���  � ��$ Center=���  �@�FocusVisualStyle Z� ��  �@J�	AllowDrop . ��  �@��PanningMode$ VerticalFirst=���  �@��IsFlicksEnabled .  ��  � �� �� " ��.R�� .+��� $ border��#��z��#��z�� . ��`�    .1��� $ 
FieldState��� $	 Tag�	 �� $
 FindAncestor=�$ TextBoxL�._��� $ PART_ContentHost��|�.  $��Hidden=�$��Hidden=��� $��5 0 0 0q�$��15(�8�� $��Center=����� $ border��$ BorderBrush����� $ Placeholder��$ Content�#   �� $��Placeholder��$��35 0 0 0q�$��14(�8�� $��Center=����� $ border��$ BorderBrush����� $	 Tag�	 �� $ TextBoxL�$
 FindAncestor=� P�  � {� .  . ��� $ border�� �@J�Opacity$
 0.56Q�P�  �@J�IsMouseOver . . ��� $ border��8   � ����  �@�Cursor$ IBeam��P�  �@J�IsKeyboardFocused . . ��� $ border��8   � ���� $
 True�� `�     .1��� $ 	IsFocused�	 �� $

 Self=��� $ Text.IsEmpty�	 �� $

 Self=�.a��� $ Placeholder�� � y�$ 	Collapsed=� ]�   �� +! �@��#IsInactiveSelectionHighlightEnabled" . �� ! �@��IsSelectionActive" .  ./���  �@��SelectionBrush# � /��� "  ��.���  � ��
 ���� ��  � ��8   ��  �@��
CaretBrush8  ��  � ��$ 20 5 0 5q���  � ��$ 35����  � ��$ 16(���  � ��$ 0q���  �@ TabNavigation$
 None=���  � ��$
 Left=���  � ��$ Center=���  �@�FocusVisualStyle Z� ��  �@J�	AllowDrop . ��  �@��PanningMode$ VerticalFirst=���  �@��IsFlicksEnabled .  ��  � �� �� " ��.R�� .+��� $ border��$ 5��#��z��#��z��#��z�� . ._��� $ PART_ContentHost��|�.  $��Hidden=�$��Hidden=��� $��Placeholder��$��25 0 0 0q�$��14(�8�� 8�� $��Type something...��$��Center=� P�  � {� .  . ��� $ border�� �@J�Opacity$
 0.56Q�P�  �@J�IsMouseOver . . ���  �@�Cursor$ IBeam���� $
 True�� `�     .1��� $ 	IsFocused�	 �� $

 Self=��� $ Text.IsEmpty�	 �� $

 Self=�.a��� $ Placeholder�� � y�$ 	Collapsed=� ]�   �� +! �@��#IsInactiveSelectionHighlightEnabled" . �� ! �@��IsSelectionActive" .  ./���  �@��SelectionBrush# � /�