   M S B A M L   `   `   `  ����O  Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem  5   	   = 9UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:WpfApp2WpfApp2 5	   	   E'clr-namespace:UI.UIPresenter.ConvertersUI.UIPresenter.Converters 5
   	   =#clr-namespace:UI.InversionOfControlUI.InversionOfControl 5   	      WpfApp2.MainWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   6sys&clr-namespace:System;assembly=mscorlib     5   	   'localclr-namespace:WpfApp2     5	   	   >
converters'clr-namespace:UI.UIPresenter.Converters     5
   	   3ioc#clr-namespace:UI.InversionOfControl     5   	   -   5          �Name$  Window��5   	    � Title$ 
MainWindow��5   	   $	��600��5   	   $
��1024��5   	    � WindowStyle$
 None=�5   	    � AllowsTransparency . 
������  � WindowStartupLocation$ CenterScreen=�5   "   ��5   	   �� *	MinHeight+��5   	   �� *
MinWidth+'  !System.Windows.Shell.WindowChrome   WindowChrome 5        5   
   -   5        WindowChrome��   CornerRadius$ 0��5         GlassFrameThickness$ 0q�5         ResizeBorderThickness 5      �� *BorderResizeThickness+	   CaptionHeight	 5      �� *CaptionHeight+5      
 � 	Resources
 5#        WpfApp2.MainWindow �� 
TargetType �� Property �� Value �� 
TargetType �� Padding   GrayBGBrush  DarkBgBrush �� FallbackValue �� CornerRadius J� Effect3  -System.Windows.Media.Effects.DropShadowEffect   ShadowDepth   Opacity � RowDefinitions K� ZIndex � ColumnDefinitions �� Orientation 
 Sumbol �� 	Converter>  8UI.UIPresenter.Converters.BoolToIsConnectedTextConverter?  9UI.UIPresenter.Converters.BoolToIsConnectedColorConvertor  LightBlueBrush  WindowSystemButtons  MaximizeWindowButton  CloseWindowButton��%�  '        7   7  7  7  7  7  7  7  �� 5%   
   "  6   .�5&      ��  � �� 5'      �� 5(      " �6&   .R�5)      ��  6"   �� *OutherMarginThickness+._�5,      �� $��Border��6%   8��  8�� ��6]   �� *WindowBorderSizeThickness+$ 4�� 6�   �� *CornerRadius+ 5-   &     5.   *   $ 0Q�6;   $	 0.2Q�6K   ._�50   "   �  51   &   �� 54   *   ��68   �� *CaptionHeightGridLeight+$ 15���� 57   *   $
��Auto �68   �� 5:   *   $��* �68   .+�5?   &   � $��0��6+   $ 3��6;   8��  5@   *   �� 5B   .   $
��Auto �6?   �� 5E   .   $��* �6?   �� 5H   .   $
��Auto �6?   .+�5K   *   �� $ 
Horizontal=�65   .+�5M   .   �� $	����64   $��Black"�5N   4   $��12(�5O   4   8�� $��Center=�5Q   4   �� 5T   .   $��Center=�5V   4   $��12(�5W   4   $��3 0 0 0q�5X   4   ��5T   4   �� *ConnectedToServer+   ��5U   4   �� *ConnectedToServer+   5X   4   5Z   +   �� 5\   *   $��2��64   $��Center=�6H   $��0 0 0 2q�6c   $��Center=�6t   $��0��6�   
��������� 5]   .   8�� .�6_   Reply5_   /   
 Messenger5a   +   �� 5d   *   $��2��65   $ 
Horizontal=�6E   $��0 0 7 0q�6^   .+�5e   .   �� 8�� ��65   �� *ColapseWindow+6W   �� 5f   .   8�� ��65   �� *MaximazeWindow+6X   �� 5g   .   8�� ��65   �� *CloseWindow+6U   5h   +   5i   '   �� 5k   &   $��2��67   5l   #   5m   #   5n      5o      5q      5r      5t      .��5x       � -   5        $��
WindowPage��5x        � NavigationUIVisibility$ Hidden=�6   ��6>   �� *-+ApplicationViewModel.ApplicationCurrentPage+ 2  ,UI.UIPresenter.Converters.ToAppPageConverter  5z      