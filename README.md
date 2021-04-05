# Death Star (WIP)
## TODO List
- [x] Menage environments as JSON
	- [x] Save one
  - [X] List all
  - [x] List by nam
  - [x] Remove one
- [ ] Menage pull and push operations with service bus first 


## Environment
### Save a environment
```bash
    env save -n PRD -connection sb:// -w
    (º-)  Try find env .....
    Success!! 
```
### List all environments
```bash
    env list --all
    (º-)  Try find env .....
    Success!! 
        Name: PRD, Connection: sb://, Show warning: True
```
## Queue management (wip)
> development only ;P
```bash
  dotnet run -p DeathStar.App asb-queue count --env dev --queue customer-configuration
```
