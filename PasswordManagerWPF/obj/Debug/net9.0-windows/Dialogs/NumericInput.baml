   M S B A M L   `   `   `  ����  PasswordManagerWPF-    'PasswordManagerWPF.Dialogs.NumericInput   5   	   W SPresentationCore, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  Числовой ввод��5   	   $	��200��6   $	��300��6,    � WindowStartupLocation$ CenterOwner=�5   	    � 
ResizeMode$ NoResize=�5   	   .��5      � $��10q�6    � RowDefinitions 5   
   �� 5	      $
��Auto �6   �� 5
      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� $��0,0,0,10q�6,   $��0��6>   ��6   �� *Prompt+6>   �� 5   
   -   5         ��Name$ ValueSlider��5       �� TickFrequency$ 1Q�5   1    �� IsSnapToTickEnabled$
 True��6C   $��0,0,0,10q�5      $��1��6#    � Minimum 5   &   �� *
MinValue+ � Maximum 6C   �� *
MaxValue+	 � Value	 5      �� *SelectedValue+5   #   �� 5   
   $��Center=�63   $��2��6P   ��6   �� *SelectedValue+6P   �� 5   
   $��3��6   
 �� Orientation$
 
Horizontal=�6"   $��Right=�6;   .+�5      �� -   5        $��OK��5      $��80��6"   $��0,0,5,0q�6-   6>   �� 5      $��Отмена��6   $��80��6&    �� IsCancel$
 True��61   5      5      5      