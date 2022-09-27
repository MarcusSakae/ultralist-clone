# Cheap Ultralist Knock off

- Get the real thing over here: https://ultralist.io/

- Automatic testing on commit: ![](https://github.com/MarcusSakae/ultralist-clone/actions/workflows/dotnet.yml/badge.svg)

## Usage examples:

    ul a some task      // Create new task
    ul l                // List Tasks
    ul c 1              // Mark task #1 as completed
    ul a +devops make *bob a sysadmin due tom

# Todo

    - [x] Context and projects
    - [x] Integration tests. (A few tests has been added.)
    - [x] Parse due dates from input
    - [] human readable dates ("tomorrow", "tom")
    - [] show expired dates in red
    - [] List by group. group:context group:project
    - [] Server interactions (sync, auth)
    - [] detailed help for each command, i.e. 'ul help add'
    - [] A heap of bugs...

# For the future... 

    - Instead of "@context", we are using "*context" because of how c# handles command line arguments.
    I think we need to use pinvoke commands to be able to pass in "@" and other symbols. 
    We'll just leave that for another day...
    https://www.pinvoke.net/default.aspx/kernel32/GetCommandLine.html

    - We have many command callbacks that operates on a task. We should add a callback type just
    for those so we don't have to 'GetTask()' and check result in each and every command.

    - JsonSerializer converts "+" to "\u002B", we'll live with it for now, but maybe there is some 
    option we can pass in if we look around.