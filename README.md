get the game id from this api call
https://www.speedrun.com/api/v1/games?name=ape escape 1

use the game id for the game=xxxxx for this api call.
you can only get 200 runs at a time so get the first 200 with this:
https://www.speedrun.com/api/v1/runs?game=576r7q18&orderby=submitted&direction=desc&embed=players&max=200&offset=0
the next 200 with this:
https://www.speedrun.com/api/v1/runs?game=576r7q18&orderby=submitted&direction=desc&embed=players&max=200&offset=200
and so on.

when you run the program it will ask you for a path to a json file. paste or write the path and hit enter, it should print the info on the cmd.
