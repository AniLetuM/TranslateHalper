   M S B A M L   `   `   `  ����J  FTranslateHalper, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:TranslateHalperTranslateHalper  5   	        TranslateHalper.MassegeWin   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   /localclr-namespace:TranslateHalper     5   	     � WindowStartupLocation$  CenterScreen=�5   	    � Title$+ %Информационное окно��5	   	   $��329.07��6%   $��432.558��65    � WindowStyle$
 None=�6E   
������� � 
ResizeMode$ NoResize=�6k    � AllowsTransparency . .��5
      � .+�5   
   �� $��Информация��6   $��Center=�6%   $��	10,10,0,0q�6B   $	��Top=�6U    J� RenderTransformOrigin$ 0.128,0.232&�6m   $	��276��6�   $��16(�6�   $��Center=�6�   $
��Bold"�6�   �� 5   
   $
��Left=�6   $��	10,46,0,0q�6/   $
��Wrap=�6B   $	��Top=�6V   $��89��6n   $	��407��6z   $��16                    (�6�   �� 6�   $�Иногда��6�   �� 6�   $�, ��6�   �� 6�   $���после работы программы с текстом сбивается кодировка. После чего некоторые символы (а иногда и весь текст) отображаются��6�   �� 6R  $� ��6V  �� 6a  $�некорректно.��6e  �� 6{  �� 6�  $�Пример:��6�  �� 6�  �� 6�  6�  �� 5   
   $t��n「I love you…♥♥」 => 0I love you &&e&e0
「I love you…♥♥」 => ��I love you��&e&e����6   $
��Left=�6�   $��
10,135,0,0q�6�   $	��Top=�6�   $��56��6�   $	��407��6�   $��16(�6�   $��Medium"�6�   �� 5   
   $
��Left=�6   $��
10,191,0,0q�6/   $
��Wrap=�6C   $����Если столкнулись с подобным случаем вы можете поменять кодировку на одну из предложенных в программе. ��6W   $	��Top=�6�   $��67��6�   $	��407��6�   $��16(�6�   �� 5   
   -   5        $��OK��5      $��Center=�6   $��116,263,127,0q�6;   $	��Top=�6R   $	��182��6j   $��43��6v   $��16(�6�   $
��Bold"�6�   6�   5      5      