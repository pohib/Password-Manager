   M S B A M L   `   `   `  ����  PasswordManagerWPF.    (PasswordManagerWPF.Dialogs.MultiPassword   5   	   W SPresentationCore, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$3  -Мультигенерация паролей��5   	   $	��450��6)   $	��500��66    � WindowStartupLocation$ CenterOwner=�5   	   .��5      � $��10q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5	      $
��Auto �6   �� 5
      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� $��0��6    �� Orientation$ 
Horizontal=�6"   $��0,0,0,10q�6;   .+�5      �� $��Количество:��6   $��Center=�6+   $��0,0,5,0q�6F   �� 5      $��40��5      $��0,0,10,0q�6    ��5      �� *PasswordsCount+ �� UpdateSourceTrigger$ PropertyChanged=�5       �� 5      $ ��Сгенерировать��6   $	��100��5      $��0,0,10,0q�6!   ��5   -   �� *GenerateCommand+5   !   5      �� 5   
   $��1��6   $@��:Выберите пароли для сохранения:��6!   $��0,0,0,5q�6H   �� 5   
   $��2��6    �� SelectionMode$ Multiple=�6F   ��6    �� *PasswordItems+��5      �� 5      .R�5      �� $��2q�5      ��5      �� *
Password+ d� 	IsChecked 5      �� *
IsSelected+ �� Mode$ TwoWay=�5      5      5       �� 5"   
   $��3��6   $ 
Horizontal=�6"   $��Right=�6;   $��0,10,0,0q�6W   .+�5#      �� $��Выбрать все��6   $	��100��6P   $	��5,0q�6\   ��6+   �� *SelectAllCommand+6\   �� 5$      $��Снять выбор��6   $	��100��6R   $	��5,0q�6^   ��6+   �� *DeselectAllCommand+6^   �� 5%      $��Сохранить��6   $	��120��6I   $	��5,0q�6U   ��6)   �� *SaveCommand+6U   �� 5&      $��Отмена��6    �� IsCancel$
 True��6&   $��80��66   $	��5,0q�6A   5'      5(      5)      