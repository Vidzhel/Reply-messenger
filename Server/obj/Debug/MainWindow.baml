   M S B A M L   `   `   `  ����A  =Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:ServerServer  5   	       Server.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   &localclr-namespace:Server     5   	     � Title$  Server��5	   	   $	��600��6   $
��1024��6%   .��5
      �    DarkBgBrush#��[  6    � RowDefinitions 5   
   �� 5      $��5* �6   �� 5      $��* �6   .+�5   
   �� -   5        $��	LogScreen��5      $�� ��6#   $��3q�6,     LightGrayBgBrush#��[ 6@    �� TextWrapping$
 Wrap=�6p   $
��Auto=�6�     GrayBGBrush#��[ 6�   
������� �� 
IsReadOnly .  � Cursor$ Arrow��6  |�.  $��20q�6$  �� 5   
   $��	20 0 20 0q�6   $��1��6(    �� Orientation$ 
Horizontal=�65   .+�5      �� -   5        $��StartServer��5      $��Start Server��6l   $��	10 5 10 5q�6�   $��30��6�   {�6(   �� *ServerOffline+��6L   �� *StartServer+6�   �� 5      -   5        $��
StopServer��5      $��Stop Server��6i   $��	20 0 20 0q�6   $��	10 5 10 5q�6�   $��30��6�   {�6'   �� *ServerOnline+��6J   �� *
StopServer+6�   �� 5      $��20 0 0 0q�6     RegularText#��[ 6&   $��Center=�6K   $��Online:��6m   �� 5       -   5        $��	txtOnline��5       #��[ 6%   $��Center=�6J   ��6l   �� *Online+5"      5#      5$      