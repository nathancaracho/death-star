# Death Star (WIP)
    -[X] Menage environments as JSON
        -[X] Save one
        -[X] List all
        -[X] List by name
        -[X] Remove one
    -[ ] Menage pull and push operations with service bus first 
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