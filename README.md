# Death Star

> ![Nuget](https://img.shields.io/nuget/dt/dotnet-death-star?color=%2300&logo=NuGet) | ![Version](https://img.shields.io/nuget/vpre/dotnet-death-star?logo=NuGet)


A DLQ/Actives queue simple command line Helper.
## TODO List
- [x] Menage environments as JSON (start with json file)
  	- [x] Save one
  	- [x] List all	
  	- [x] List by name
  	- [x] Remove one
- [ ] Menage queue operations
	- [ ] Azure Service bus 
		- [x] Get DLQ message count
		- [x] Peek all DLQ messages
		- [x] Peek one DLQ message
		- [x] Print DLQs info as table
		- [X] Pull all DLQ
		  - [ ] Create a temp file to ensure no message lose. (WIP)
		- [ ] Push all to QUEUE   	
- [ ] Automatize NUGET push
  - [x] Create a nuget package (dotnet-death-star)
  - [x] First PUSH
  - [x] Create a GH Action to push it
  - [ ] Add SemVer
- [ ] Create Documentation



## How to install

### From nuget

```bash
dotnet tool install --global dotnet-death-star --version 0.0.1-alpha-2
```

### From Local

```bash
git clone https://github.com/nathancaracho/death-star.git && \
cd death-star && \
dotnet pack -o out && \
dotnet tool install dotnet-death-star --add-source /DeathStar.App/out --version 0.0.1-alpha-3
```

## Commands

### Environment Command

> The `env` command is responsible to menage **queue** environments using `save`, `list` and `remove`  subcommands

### Save an environment

```bash
$ dotnet death-star env save --name dev --connection "sb://"
(º-)  Saving env .....
Success: May the force be with 
New env dev saved 
 - Name: dev, Connection: sb://,  Show warning: False
```

### List all environments
```bash
$ dotnet death-star env list --all
(º-)  Try find env .....
Success: May the force be with 
 Name: test, Connection: just a test, Show warning: False
```
### Remove an environment

```bash
$ dotnet death-star env remove --name test
(º-)  Removing env .....
Success: May the force be with 
 Env test was removed with success
```



## Queue management 

> The command `queue` is responsible to menage queues messages **pulling**, **peeking** or **pushing** using subcommand `dlq`. 

### DLQs menagement

> The command `dlq` is responsible to menage **DLQ** messages **counting**, **peeking** or **pulling** one or more messages.

#### Count DLQs messages

```bash
 $ dotnet death-star queue dlq count --env test --queue some-queue
 (º-)  Get DLQ Count.....
 Success: May the force be with  
  The DLQ queue some-queue have 78 itens for env test.
```
#### Peek all DLQs messages

```bash
 $ dotnet death-star queue dlq peek --all --env test --queue some-queue
 (º-)  Peeking messages...
 [ ■■■■■■■■■■■ ] - 78≃100%
 (º-)  Serializing message...
 (º-)  Saving...
 Success: May the force be with  
  The DLQ queue has been saved.
```
### Receive all and delete DLQs messages(WIP)

> Receive all messages and **DELETE**

```bash
$ dotnet death-star queue dlq receive --env dev third-queue
(º-)  Get DQL queues and delete.....
(º-)  Serializing message...0%
(º-)  Saving...
Success: May the force be with 
 The DLQ queue customer-insert for env dev was saved.

```
### Queue report

> The report command return a table with **Active** and **DLQ** queues messages counts.

```bash
$ dotnet dotnet death-star queue report --env dev first-queue second-queue third-queue
(º-)  Getting queues infos...
Success: May the force be with 
 

|                    Name                      |   Active   |     DLQ    |
|----------------------------------------------|------------|------------|
|                                   first-queue|         123|          46|
|                              	   second-queue|        9088|          67|
|                                   third-queue|           0|          98|

```