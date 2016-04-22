# Сборка из исходников #

проект собирается с помощью
  * **Sharp Develop 3.2** - http://www.icsharpcode.net/OpenSource/SD/Download/
  * тип репозитария исходников **SVN**

для тестирования необходимо иметь дополнительной
  * платформа **1C Предприятие 8.1/8.2** (http://v8.1c.ru/overview/Platform.htm)
  * **ApacheMQ** - http://activemq.apache.org/
  * ....

# создание инсталятора #

  * **Nullsoft Install System** - http://nsis.sourceforge.net/Main_Page
  * с плагином управления службами  http://nsis.sourceforge.net/NSIS_Simple_Service_Plugin
  * с установленным в системе TortoiseSVN - необходим SubWCRevCOM.exe = COM для работы с репозитарием SVN из скриптов VBS