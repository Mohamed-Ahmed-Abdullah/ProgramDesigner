   M S B A M L   `   `   `  ����J  FProgramDesigner, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullC&clr-namespace:ProgramDesigner.ControlsProgramDesigner.Controls  5   	   G(clr-namespace:ProgramDesigner.ConvertersProgramDesigner.Converters  5   	        ProgramDesigner.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   ;controls&clr-namespace:ProgramDesigner.Controls     5   	   ?
converters(clr-namespace:ProgramDesigner.Converters     5   	     � Title$  
MainWindow��5   	   $��543.368��6   $��793.105��6-   .��5      �  � 	Resources 5   
      ContextMenu1 m� CommandParameter �� RelativeSource �� Path  ContextMenu23   -ProgramDesigner.Converters.BoolToIntConverter  BoolToIntConverter:   4ProgramDesigner.Converters.BoolToVisibilityConverter  BoolToVisibilityConverter9   3ProgramDesigner.Converters.BoolToThicknessConverter  BoolToThicknessConverter��%�  &	        &	 �     &	 l    &	     &	 �    �� 5	      .@�5
      m� -   5        $��Delete��5
       5      ��  �� *Self+$ Parent�6l   5      �� 5      .@�5      m� -   5        $��Delete��5       5      ��  �� *Self+$ Parent�6l   m� 5      -   5        $��Rename��5       5      ��  �� *Self+$ Parent�6l   5        5      6,     5      63     5      62   5       � RowDefinitions 5   
   �� 5      $
��Auto �6   6,   �� 5      $��* �6   6)   �� 5      $
��Auto �6   6,   .+�5   
   �� $��0��6    �� Orientation$ 
Horizontal=�6"   $��5q�6;   y�5      �� *IsNameDialogVisible+ �� UpdateSourceTrigger$ PropertyChanged=� �� 	Converter# [ .+�5       �� -   5        $��TextBoxRename��5       $	��300��6+   6[   �� 5!      -   5        $��Ok��5!      $
��20,0q�68   $��10,0,0,0q�6G   6[   5"      �� 5#   
   $��0q�6   $��1��6%   ._�5$      �� -   5        $��
MainCanvas��5$      
�������.+�5'      �� -   5        $
��Save��5'      $
��20,0q�6(   	 �� Left$		 186��67   

 �� Top$
 5��6I   5(   -   �� 5*      -   5        $��SelectionRectangle��5*      $	��250��66   $	��250��6B   $��1��6O   
����� � �� StrokeDashArray$	 7 7R�5+   1   $y�	Collapsed=�6G   �� 5-      -	   5        $��VerticalBarrier��5-      $		 180��63   $��1��5.      
��� E����6&   ��  ��  �� Mode$ FindAncestor=� �� AncestorType" ��$ ActualHeight�5/      �� 51      $
 70��6   $	��180��52      $��1��6(   
��� E��53   1   �� 55      -
   5        $��Control��55      
��� ��� �� CornerRadius$ 0��57      $	 20��6*   $
 5��6;   ��56      �� *IsControlsVisible+# [ $ PropertyChanged=�._�58      �� 
��� ��� �� Padding$
 10,0q�64   $��Control��59       61   5:      �� 5<      -   5        $	��Var��5<      
����� �$ 0��6>   $		 110��5>      $
 5��6+   ��5=      �� *IsVarVisible+# [ $ PropertyChanged=�._�5?      �� 
����� �$
 10,0q�69   $	��Var��5@       6-   5A      �� 5C      -   5        $��Token��5C      
���� ��$ 0��6?   $	 20��5E      $
 35��6*   ��5D      �� *IsTokenVisible+# [ $ PropertyChanged=�._�5F      �� 
���� ��$
 10,0q�68   $��Token��5G       6/   5H      5I      5J      � 5L   
   $��2��6    5M      �� 5N      $
��Auto �6    �� 5O      $��* �6    .+�5Q      �� $��0,0,0,2q�6   
����� �._�5R      �� $��Result:��6   6-   5S      �� 5T      -   5        $��Result��5T      
���   �
�������$��1��6J   $��60��6W   $��0,0,0,0q�6c   6v   5U      5V      5W      