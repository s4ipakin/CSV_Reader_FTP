   M S B A M L   `   `   `  ����G  CVDI_VDO_Graf, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:VDI_VDO_GrafVDI_VDO_Graf  5      U QLiveCharts.Wpf, Version=0.9.7.0, Culture=neutral, PublicKeyToken=0bc1f845d1ebb8dfG4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.WpfLiveCharts.Wpf 5          VDI_VDO_Graf.GrafPage   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      ,localclr-namespace:VDI_VDO_Graf     5      Dlvc4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.Wpf     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5        N� Title$  GrafPage��5	      .��5      � -   5         �Name$
 grid��5      .+�5   
   #  LiveCharts.Wpf.CartesianChart  -   5          Name$ Chart_P��5      $��
0,36,0,236q�5      &   LiveCharts.Wpf.Charts.Base.Chart   LegendLocation$ Right=�5         Series 5      �� *SeriesCollection+   AxisY 5        LiveCharts.Wpf.Axis  -   5          Name$ 
chartAxisY��5         Title$# Скорость потока��5         FontSize$ 16Q�5      	   
FontWeight$	 	UltraBold"�5      
   MaxValue$
 10Q�5         LabelFormatter 5      �� *
YFormatter+5         AxisX 5        $ 
Время��5      $ 16Q�5       $	 Black"�5!         Labels 5"      �� *Labels+5$      (  "System.Windows.Controls.DatePicker  5%   
   -   5          Name$ 
pickedData��5&      $��24��5'      $��726,0,0,119q�5(      $
��Left=�5)      $��Bottom=�5*      �� 5+   
   -   5         ��Name$ btnFromDays��5,      $	��136��5-      $��35��5.      $��
0,0,80,124q�5/      $��Right=�50      $��Bottom=�51      $'��!Построить графики��53        54   
   -   5        $ PickerDataStop��55      $��26��56      $��
726,0,0,79q�57      $
��Left=�58      $��Bottom=�59      �� 5:   
   -   5        $ btnPrint��5;      $	��136��5<      $��35��5=      $��	0,0,80,80q�5>      $��Right=�5?      $��Bottom=�5@      $��Импорт в JPeg��5B      �� 5C   
   -   5         ��Name$ label��5D      $��417,0,0,118q�5E      $
��Left=�5F      $��Bottom=�5G      $!��Начало графика��5H      �� 5I   
   -	   5        $ 
label_Copy��5J      $��
417,0,0,73q�5K      $
��Left=�5L      $��Bottom=�5M      $��Конец графика��5N      �� 5O   
   -
   5        $ label_Copy1��5P      $��526,0,0,146q�5Q      $
��Left=�5R      $��Bottom=�5S      $��Часы��5T      �� 5U   
   -   5        $ label_Copy2��5V      $��604,0,0,146q�5W      $
��Left=�5X      $��Bottom=�5Y      $��Минуты��5Z      �� 5[   
   -   5        $ label_Copy3��5\      $��726,0,0,146q�5]      $
��Left=�5^      $��Bottom=�5_      $��Дата��5`      �� 5a   
   -   5         ��Name$ comboBoxHourStart��5b      $��60��5c      $��526,0,0,121q�5d      $
��Left=�5e      $��Bottom=�5f      �� 5g   
   -   5        $ comboBoxMinuteStart��5h      $��66��5i      $��604,0,0,121q�5j      $
��Left=�5k      $��Bottom=�5l      �� 5m   
   -   5        $ comboBoxHourEnd��5n      $��60��5o      $��
526,0,0,76q�5p      $
��Left=�5q      $��Bottom=�5r      �� 5s   
   -   5        $ comboBoxMinuteEnd��5t      $��66��5u      $��
604,0,0,76q�5v      $
��Left=�5w      $��Bottom=�5x      �� 5y   
   -   5        $ label1��5z      $	��206��5{      $��35��5|      $��
10,0,0,196q�5}      $
��Left=�5~      $��Bottom=�5      $,��&Сформировал________________��5�      $��16(�5�      $��Normal"�5�      �� 5�   
   -   5        $ label1_Copy��5�      $	��206��5�      $��35��5�      $��235,0,0,196q�5�      $
��Left=�5�      $��Bottom=�5�      $&�� Утвердил________________��5�      $��16(�5�      $��Normal"�5�      �� 5�   
   -   5        $ label2��5�      $��526,0,296,0q�5�      $	��Top=�5�      $d��^График контроля параметров системы водоподготовки��5�      $��16(�5�      �� 5�   
   -   5        $ 
LblLoading��5�      $��559,324,263,0q�5�      $	��Top=�5�      $+��%Загрузка лог файла...��5�      $y�Hidden=�5�      $��26(�5�      5�      5�      