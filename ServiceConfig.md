## секции конфигурационного файла ##

**...**


## примеры описания соединений в секции Connections ##

### Файловый режим ###

```
<Connection Name="Test81" 
         Mode="File"
	 File="..\..\..\..\..\Test\TestIBs\v81test"
	 UserName=""
	 Password=""	
	 AdapterType="OneCService2.V81Adapter"
	 PoolSize="1"
	 PoolUserName="PoolUserName"
         PoolPassword="PoolPassword"/>

```

для файлового режима поддерживается возможность указания каталога БД относительно каталога установки и запуска службы

### Серверный режим ###
```
<Connection Name="IntalevDev" 
	Mode="Server"
	Server="as8.server.loc:1541"
	Base="intalevdev"
	UserName="lustin"
	Password="нетуттобыло"	
	AdapterType="OneCService2.V81Adapter"
	PoolSize="1"
	PoolUserName="PoolUserName"
	PoolPassword="PoolPassword"/>
```

### варианты адаптеров ###

секция _AdapterType=_ может принимать значения "OneCService2.V81Adapter" и "OneCService2.V82Adapter"