   M S B A M L   `   `   `  ����  PasswordManagerWPF+    %PasswordManagerWPF.SavePasswordDialog   5   	   W SPresentationCore, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$   ��5   	   $	��200��6   $	��400��6    � WindowStartupLocation$ CenterOwner=�5   	    � 
ResizeMode$ NoResize=�5   	   .��5      � $��10q�6    � RowDefinitions 5   
   �� 5	      $
��Auto �6   �� 5
      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� $A��;Введите имя/описание для пароля:��6   $��5q�6<   �� 5   
   $��1��6   $��5q�6b   ��6   �� *PasswordName+ �� UpdateSourceTrigger$ PropertyChanged=�6b   �� 5   
   $��3��6    �� Orientation$ 
Horizontal=�6"   $��Right=�6;   .+�5      �� $��Сохранить��6    �� 	IsDefault$
 True��6)   $��80��6:   $��5q�6E   ��5      �� *SaveCommand+�� 5      $��Отмена��6    �� IsCancel$
 True��6&   $��80��66   $��5q�6A   5      5      5      